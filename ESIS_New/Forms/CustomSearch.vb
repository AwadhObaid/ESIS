Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Drawing.Printing
Imports PdfiumViewer


Public Class CustomSearch

#Region "************************ الإعلان عن المتغيرات والثوابت ***********************"
    ' تعريف المتغيرات
    ' تعريف المؤقت مع استخدام الفئة الصحيحة و WithEvents
    Private WithEvents DelayTimer As New System.Windows.Forms.Timer()
    Private ReadOnly delayTime As Integer = 2000 ' التأخير بالمللي ثانية (2000ms = 2 ثانية)
    Private iconsImageList As New ImageList()
    Dim xx As Short = 0
    Private cts As Threading.CancellationTokenSource
    Private searchTask As Task
    Private isUserSelecting As Boolean = False
    Private shouldAutoAddAfterSearch As Boolean = False
    Private selectedNodeTag As String = String.Empty
    Private results As New List(Of Tuple(Of String, String)) ' (اسم الملف، مسار الملف)
    Private isLoadingData As Boolean = False


    ' --- عارض PDF (بديل WebBrowser) ---
    Private _pdfDoc As PdfDocument = Nothing
    Private _isClosing As Boolean = False
    ' إضافة حدث لاختيار الملف
    Public Event DataSelected(filePath As String, fileName As String, fileExtension As String)
    Public Property SelectedFilePath As String
    Public Property SelectedFileName As String
    Public Property SelectedFileExtension As String
#End Region

#Region "************************ دوال التهيئة والأحداث الأساسية ***********************"
    ''' <summary>
    ''' دالة البناء للنموذج - يتم استدعاؤها عند إنشاء النموذج
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        InitializeComponents()
    End Sub

    ''' <summary>
    ''' تهيئة العناصر المخصصة
    ''' </summary>
    Private Sub InitializeComponents()
        ' تهيئة TreeView
        resultsTreeView.ImageList = iconsImageList
        resultsTreeView.ShowNodeToolTips = True

        ' تهيئة ProgressBar
        ProgressBar1.Style = ProgressBarStyle.Continuous
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100
        ProgressBar1.Value = 0
        progressLabel.Text = "0%"

        ' تهيئة Search TextBox
        searchTextBox.Focus()
    End Sub

    Private Sub CustomSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' إعدادات Timer
        DelayTimer.Interval = delayTime

        ' تحميل المجلد الافتراضي
        InitializeDefaultFolder()

        ' تعيين تركيز مربع البحث
        searchTextBox.Select()

        ' تعطيل أزرار الملفات حتى يتم اختيار ملف
        btnOpenLoction.Enabled = False
        btnPrint.Enabled = False
        txtSearchPath.Text = My.Settings.DefaultPathSearch

        ' ✅ بدلاً من ذلك، ابدأ البحث بعد أن يتم عرض النموذج
        AddHandler Me.Shown, AddressOf CustomSearch_Shown
    End Sub

    Private Async Sub CustomSearch_Shown(sender As Object, e As EventArgs)
        ' الآن النموذج جاهز تمامًا، ويمكنك بدء البحث بأمان
        Await StartSearch()
        ' أو يمكنك إزالة هذا الحدث إذا كنت لا تحتاجه لاحقًا
        RemoveHandler Me.Shown, AddressOf CustomSearch_Shown
    End Sub
#End Region

#Region "************************ أحداث شجرة العرض (TreeView) ***********************"
    Private Sub ResultsTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles resultsTreeView.AfterSelect
        OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=True)
        btnOpenLoction.Enabled = True
        btnPrint.Enabled = True

        ' استخدام BeginInvoke لإغلاق النموذج بعد انتهاء معالجة الحدث
        Me.BeginInvoke(Sub()
                           Dim archiveForm As ArchiveForm = TryCast(Application.OpenForms("ArchiveForm"), ArchiveForm)
                           If archiveForm IsNot Nothing Then
                               archiveForm.btnAdd.Text = "اضغط للاضافة"
                               archiveForm.btnAdd.Select()
                               Thread.Sleep(500)
                               Me.Close() ' إغلاق النموذج بعد انتهاء المعالجة
                           End If
                       End Sub)


        'OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=True)
        'btnOpenLoction.Enabled = True
        'btnPrint.Enabled = True
        ''Me.Close()
        'Dim archiveForm As ArchiveForm = TryCast(Application.OpenForms("ArchiveForm"), ArchiveForm)
        'If archiveForm IsNot Nothing Then

        '    archiveForm.btnAdd.Text = "اضغط للاضافة"
        '    archiveForm.btnAdd.Select()
        '    Thread.Sleep(500)
        '    Me.Close()
        'End If
    End Sub

    Private Sub ResultsTreeView_KeyDown(sender As Object, e As KeyEventArgs) Handles resultsTreeView.KeyDown
        If e.KeyCode = Keys.Enter AndAlso resultsTreeView.SelectedNode IsNot Nothing Then
            OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=True)
        ElseIf e.KeyCode = Keys.Down Then
            ' عند الضغط على السهم السفلي
            If resultsTreeView.SelectedNode IsNot Nothing Then
                'AutoAddToArchiveForm()
            End If
        End If
    End Sub

    Private Sub ResultsTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseClick
        If e.Button = MouseButtons.Left Then
            ' تحديث التحديد
            resultsTreeView.SelectedNode = e.Node

            ' عرض الملف المحدد فورًا
            If e.Node.Tag IsNot Nothing Then
                OpenSelectedFile(e.Node, isDoubleClick:=False)
            End If
        End If
    End Sub

    Private Sub ResultsTreeView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseDoubleClick
        OpenSelectedFile(e.Node, isDoubleClick:=True)
        Dim archiveForm As ArchiveForm = TryCast(Application.OpenForms("ArchiveForm"), ArchiveForm)
        If archiveForm IsNot Nothing Then
            archiveForm.btnAdd.Select()
        End If
        Me.Close()
    End Sub
#End Region

#Region "************************ إدارة الملفات والعرض ***********************"
    Private Sub OpenSelectedFile(selectedNode As TreeNode, isDoubleClick As Boolean)
        ' التحقق من أن العقدة و Tag ليست Nothing
        If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then
            Return
        End If

        Dim filePath As String = selectedNode.Tag.ToString()

        ' التحقق من وجود الملف
        If Not File.Exists(filePath) Then
            ExecuteOnUIThread(Sub()
                                  LabelFileName.Text = "الملف غير موجود: " & filePath
                                  LabelFileName.ForeColor = Color.Red
                              End Sub)
            MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            ' الحصول على امتداد الملف
            Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
            Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)

            ' تحديث تسمية عرض الملف
            ExecuteOnUIThread(Sub()
                                  LabelFileName.Text = filePath
                                  LabelFileName.ForeColor = Color.DarkBlue
                              End Sub)

            ' فتح الملف بناءً على نوعه
            Select Case fileExtension
                Case ".pdf"
                    ' عرض ملف PDF داخل PdfiumViewer
                    ExecuteOnUIThread(Sub()
                                          Try
                                              pdfViewer.Visible = True
                                              pdfViewer.BringToFront()
                                              ShowPdfInViewer(filePath)
                                          Catch ex As Exception
                                              LabelFileName.Text = "خطأ في عرض ملف PDF: " & ex.Message
                                              LabelFileName.ForeColor = Color.Red
                                          End Try
                                      End Sub)

                    ' إذا كان هذا نقرة مزدوجة أو ضغط Enter، قم بإرسال الإشارة
                    If isDoubleClick Then
                        SelectedFilePath = filePath
                        SelectedFileName = fileName
                        SelectedFileExtension = fileExtension

                        ExecuteOnUIThread(Sub()
                                              RaiseEvent DataSelected(filePath, fileName, fileExtension)
                                              UpdateArchiveForm(filePath, fileName, fileExtension)
                                          End Sub)
                    End If

                Case ".html", ".htm"
                    ' عارض PdfiumViewer لا يعرض HTML، لذلك افتحه بالتطبيق الافتراضي
                    ExecuteOnUIThread(Sub()
                                          Try
                                              pdfViewer.Visible = False
                                              Process.Start(filePath)
                                          Catch ex As Exception
                                              LabelFileName.Text = "غير قادر على فتح الملف: " & ex.Message
                                              LabelFileName.ForeColor = Color.Red
                                              MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                          End Try
                                      End Sub)

                    If isDoubleClick Then
                        SelectedFilePath = filePath
                        SelectedFileName = fileName
                        SelectedFileExtension = fileExtension

                        ExecuteOnUIThread(Sub()
                                              RaiseEvent DataSelected(filePath, fileName, fileExtension)
                                              UpdateArchiveForm(filePath, fileName, fileExtension)
                                          End Sub)
                    End If

                Case Else
                    ' فتح الملف باستخدام التطبيق الافتراضي
                    ExecuteOnUIThread(Sub()
                                          Try
                                              pdfViewer.Visible = False
                                              Process.Start(filePath)
                                          Catch ex As Exception
                                              ExecuteOnUIThread(Sub()
                                                                    LabelFileName.Text = "غير قادر على فتح الملف: " & ex.Message
                                                                    LabelFileName.ForeColor = Color.Red
                                                                End Sub)
                                              MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                          End Try
                                      End Sub)
            End Select
        Catch ex As Exception
            ' التعامل مع الأخطاء
            ExecuteOnUIThread(Sub()
                                  LabelFileName.Text = "غير قادر على فتح الملف: " & ex.Message
                                  LabelFileName.ForeColor = Color.Red
                              End Sub)
            MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح موقع الملف المحدد
    ''' </summary>
    Private Sub BtnOpenLoction_Click(sender As Object, e As EventArgs) Handles btnOpenLoction.Click
        If resultsTreeView.SelectedNode Is Nothing Then
            Exit Sub
        Else
            Process.Start("explorer.exe", "/select," & resultsTreeView.SelectedNode.Tag)
        End If
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        ShowPdfPrintDialog()
        btnPrint.Enabled = False
    End Sub
#End Region

#Region "************************ إدارة الأرشيف والتحديثات ***********************"
    ''' <summary>
    ''' تحديث نموذج الأرشيف مع معلومات الملف المختار
    ''' </summary>
    Private Sub UpdateArchiveForm(filePath As String, fileName As String, fileExtension As String)
        Try
            ' البحث عن نموذج الأرشيف المفتوح
            For Each form As Form In Application.OpenForms
                If TypeOf form Is ArchiveForm Then
                    Dim archiveForm As ArchiveForm = DirectCast(form, ArchiveForm)

                    ' تحديث حقول النموذج
                    archiveForm.txtOriginalPath.Text = filePath
                    archiveForm.txtFilename.Text = fileName
                    archiveForm.txtFileType.Text = fileExtension.TrimStart(".")

                    ' لا تقم بتعيين قيمة ComboArchiveFolder من المسار النسبي
                    ' هذه الخطوة كانت تسبب المشكلة
                    ' حساب مسار الوجهة
                    archiveForm.CalculateDestinationPath()
                    '' عرض الملف في WebBrowser
                    'archiveForm.pdfViewer.Navigate(filePath)


                    Dim ext = IO.Path.GetExtension(filePath).ToLower()

                    If ext = ".pdf" Then
                        GlobalData.ShowPdf(archiveForm.pdfViewer, filePath)
                    Else
                        ' لو الملف ليس PDF افتحه خارجيًا (اختياري)
                        Process.Start(New ProcessStartInfo(filePath) With {.UseShellExecute = True})
                    End If




                    ' تعبئة الحقول الافتراضية
                    If String.IsNullOrWhiteSpace(archiveForm.txtArchiveNum.Text) Then
                        archiveForm.txtArchiveNum.Text = GenerateArchiveNumber()
                    End If

                    If String.IsNullOrWhiteSpace(archiveForm.txtArchiveSubject.Text) Then
                        archiveForm.txtArchiveSubject.Text = "موضوع الملف"
                    End If

                    Exit For

                End If
            Next

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث نموذج الأرشيف: {ex.Message}")
        Finally
            ' الضغط على زر الإضافة تلقائيًا
            'ArchiveForm.btnAdd.PerformClick()
            ' إرسال إشارة لتحديد التركيز على زر الإضافة

            'Me.Close()
        End Try
    End Sub

    ''' <summary>
    ''' إضافة الملف تلقائيًا إلى نموذج الأرشيف عند الضغط على السهم السفلي
    ''' </summary>
    Private Sub AutoAddToArchiveForm()
        Try
            ' التحقق من وجود عقدة محددة
            If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
                Dim filePath As String = resultsTreeView.SelectedNode.Tag.ToString()

                ' التحقق من وجود الملف
                If Not File.Exists(filePath) Then
                    MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' تحديث تسمية المسار لعرض المسار الكامل
                ExecuteOnUIThread(Sub()
                                      LabelFileName.Text = "المسار: " & filePath
                                      LabelFileName.ForeColor = Color.DarkBlue
                                  End Sub)

                ' عرض الملف في WebBrowser إذا كان من النوع المدعوم
                Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
                If fileExtension = ".pdf" OrElse fileExtension = ".html" OrElse fileExtension = ".htm" Then
                    ExecuteOnUIThread(Sub()
                                          Try
                                              pdfViewer.Visible = True
                                              pdfViewer.BringToFront()
                                              ShowPdf(filePath)
                                          Catch ex As Exception
                                              ExecuteOnUIThread(Sub()
                                                                    LabelFileName.Text = "فشل في عرض الملف: " & ex.Message
                                                                    LabelFileName.ForeColor = Color.Red
                                                                End Sub)
                                          End Try
                                      End Sub)
                End If

                ' تحديث نموذج الأرشيف
                UpdateArchiveForm(filePath, Path.GetFileNameWithoutExtension(filePath), fileExtension)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في AutoAddToArchiveForm: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' توليد رقم أرشيف فريد
    ''' </summary>
    Private Function GenerateArchiveNumber() As String
        Try
            ' استخدام التاريخ والوقت لضمان الفريد
            Return $"ARC-{DateTime.Now:yyyyMMdd-HHmmss}"
        Catch ex As Exception
            Return $"ARC-{DateTime.Now.Ticks}"
        End Try
    End Function
#End Region

#Region "************************ إدارة البحث والتنفيذ ***********************"
    Private Async Sub searchTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles searchTextBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            ' تأكد من وجود نص للبحث
            If String.IsNullOrWhiteSpace(searchTextBox.Text) Then Return

            ' منع تنفيذ Timer في نفس اللحظة
            DelayTimer.Stop()

            ' ابدأ عملية البحث (مع إضافة تلقائية للأرشيف)
            Await StartSearch(autoAddToArchive:=True)

            e.Handled = True
            e.SuppressKeyPress = True

        ElseIf e.KeyCode = Keys.Down Then
            If Not String.IsNullOrWhiteSpace(searchTextBox.Text) Then
                resultsTreeView.Select()
                If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
                    OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=False)
                    resultsTreeView.Refresh()
                End If
            End If
        End If
    End Sub
    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
            resultsTreeView.Nodes.Clear()
            TxtMsg.Text = "لم يتم العثور على ملف , الرجاء إدخال قيمة صحيحة في مربع البحث"
        Else
            ' إعادة تشغيل Timer عند تغيير النص
            DelayTimer.Stop()
            DelayTimer.Start()
            TxtMsg.Text = Nothing
        End If
    End Sub

    ''' <summary>
    ''' بدء عملية البحث
    ''' </summary>
    Private Async Function StartSearch(Optional autoAddToArchive As Boolean = False) As Task
        ' إيقاف مؤقت التأخير حتى لا يبدأ بحثًا آخر بالتوازي
        DelayTimer.Stop()

        ' إلغاء أي بحث سابق جارٍ
        If cts IsNot Nothing Then
            Try
                cts.Cancel()
            Catch
            End Try
            cts.Dispose()
            cts = Nothing
        End If

        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
            MessageBox.Show("يرجى إدخال نص للبحث", "بحث فارغ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' تهيئة واجهة المستخدم للبحث
        InitializeSearchUI()

        ' بدء البحث
        cts = New CancellationTokenSource()
        Dim token = cts.Token

        Try
            Dim r = Await Task.Run(Function() PerformSearch(searchTextBox.Text, token), token)

            ExecuteOnUIThread(Sub()
                                  UpdateUIWithResults(r)
                                  CompleteSearchUI()

                                  ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
                                  If Not isUserSelecting Then
                                      RestoreSelection()
                                      AutoSelectFirstResult()
                                  End If

                                  ' إضافة تلقائية للأرشيف عند الطلب
                                  If autoAddToArchive AndAlso resultsTreeView.Nodes.Count > 0 Then
                                      AutoAddToArchiveForm()
                                  End If
                              End Sub)

        Catch ex As OperationCanceledException
            ExecuteOnUIThread(Sub()
                                  CompleteSearchUI("تم إلغاء البحث")
                              End Sub)
        Catch ex As Exception
            ExecuteOnUIThread(Sub()
                                  CompleteSearchUI($"حدث خطأ: {ex.Message}")
                              End Sub)
        End Try
    End Function
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If cts IsNot Nothing Then
            cts.Cancel()
        End If
    End Sub
#End Region

#Region "************************ خوارزميات البحث والمعالجة ***********************"
    ''' <summary>
    ''' تنفيذ عملية البحث وإرجاع النتائج
    ''' </summary>
    Private Function PerformSearch(query As String, token As CancellationToken) As List(Of Tuple(Of String, String))
        Dim searchResults As New List(Of Tuple(Of String, String))()

        Try
            ' حفظ التحديد الحالي قبل البدء في البحث
            If Not isUserSelecting Then
                SaveCurrentSelection()
            End If

            ' البحث في مجلد الأرشيف الافتراضي
            Dim folderToSearch As String = My.Settings.DefaultPathSearch

            ' التحقق من وجود المجلد
            If Not String.IsNullOrEmpty(folderToSearch) AndAlso Directory.Exists(folderToSearch) Then
                token.ThrowIfCancellationRequested()
                SearchInFolder(folderToSearch, query, token, searchResults)
            Else
                ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
                Dim tempFolder As String = Path.GetTempPath()
                If Directory.Exists(tempFolder) Then
                    token.ThrowIfCancellationRequested()
                    SearchInFolder(tempFolder, query, token, searchResults)
                End If
            End If
        Catch ex As OperationCanceledException
            ' تم إلغاء البحث
        Catch ex As Exception
            ' حدث خطأ
            System.Diagnostics.Debug.WriteLine($"حدث خطأ في PerformSearch: {ex.Message}")
        End Try

        Return searchResults
    End Function

    Private Sub SearchInFolder(folderPath As String, query As String, token As CancellationToken, ByRef results As List(Of Tuple(Of String, String)))
        Try
            ' التحقق من وجود المجلد
            If Not Directory.Exists(folderPath) Then
                Return
            End If

            ' البحث في الملفات
            Dim files = Directory.GetFiles(folderPath, $"*{query}*", SearchOption.AllDirectories)
            Dim totalFiles = files.Length

            ' التحقق من وجود ملفات
            If totalFiles = 0 Then
                Return
            End If

            Dim processedFiles = 0

            For Each file In files
                ' التحقق من طلب الإلغاء
                token.ThrowIfCancellationRequested()

                Dim fileName As String = Path.GetFileName(file)
                results.Add(Tuple.Create(fileName, file))

                processedFiles += 1
                Dim progress = CInt((processedFiles / totalFiles) * 100)

                ' تحديث شريط التقدم
                If ProgressBar1.GetCurrentParent().InvokeRequired Then
                    ProgressBar1.GetCurrentParent().Invoke(Sub()
                                                               ProgressBar1.Value = progress
                                                               progressLabel.Text = $"{progress}%"
                                                           End Sub)
                Else
                    ProgressBar1.Value = progress
                    progressLabel.Text = $"{progress}%"
                End If
            Next
        Catch ex As Exception
            ' التعامل مع الأخطاء (مثل الوصول غير المصرح به)
            System.Diagnostics.Debug.WriteLine($"Error accessing folder: {folderPath}. {ex.Message}")
        End Try
    End Sub
#End Region

#Region "************************ إدارة واجهة المستخدم والتحكم ***********************"
    Private Async Sub DelayTimer_Tick(sender As Object, e As EventArgs) Handles DelayTimer.Tick
        ' إيقاف Timer بعد انتهاء الوقت
        DelayTimer.Stop()

        ' إلغاء أي بحث سابق جارٍ
        If cts IsNot Nothing Then
            Try
                cts.Cancel()
            Catch
            End Try
            cts.Dispose()
            cts = Nothing
        End If

        ' التحقق من قيمة Tag
        If Not (Me.Tag = 1 OrElse Me.Tag = 2) Then Return

        ' التحقق من صحة النص المدخل
        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
            Dim errorMsg = If(Me.Tag = 1, "لم يتم العثور على ملف, الرجاء إدخال قيمة صحيحة", "Please enter a search term.")
            TxtMsg.Text = errorMsg
            Return
        End If

        ' تهيئة العناصر
        InitializeSearchUI()

        ' إعداد مصدر الإلغاء
        cts = New Threading.CancellationTokenSource()
        Dim token = cts.Token

        Try
            ' استخدام Task.Run بدون .Result لتجنب تجميد الواجهة
            Dim results = Await Task.Run(Function() PerformSearch(searchTextBox.Text, token), token)
            ExecuteOnUIThread(Sub()
                                  UpdateUIWithResults(results)
                                  CompleteSearchUI()

                                  ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
                                  If Not isUserSelecting Then
                                      RestoreSelection()

                                      ' تحديد أول ملف تلقائيًا بعد انتهاء البحث
                                      AutoSelectFirstResult()
                                  End If
                              End Sub)
        Catch ex As OperationCanceledException
            ExecuteOnUIThread(Sub()
                                  CompleteSearchUI("تم إلغاء البحث")
                              End Sub)
        Catch ex As Exception
            ExecuteOnUIThread(Sub()
                                  CompleteSearchUI($"حدث خطأ: {ex.Message}")
                              End Sub)
        End Try
    End Sub

    ' دالة لتهيئة واجهة البحث
    Private Sub InitializeSearchUI()
        resultsTreeView.Nodes.Clear()
        iconsImageList.Images.Clear()
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True
        progressLabel.Visible = True
        progressLabel.Text = "0 %"
        btnCancel.Enabled = True
        btnOpenLoction.Enabled = False
        btnPrint.Enabled = False
    End Sub

    ' تحسين دالة لتحديث واجهة المستخدم بالنتائج
    Private Sub UpdateUIWithResults(results As List(Of Tuple(Of String, String)))
        ' تنظيف TreeView والـ ImageList
        iconsImageList.Images.Clear()
        resultsTreeView.Nodes.Clear()

        ' إضافة أيقونات افتراضية للمجلدات والملفات
        Dim folderIcon As Icon = SystemIcons.WinLogo ' أيقونة المجلد (يمكنك تخصيصها)
        Dim fileIcon As Icon = SystemIcons.Application ' أيقونة الملف (يمكنك تخصيصها)
        iconsImageList.Images.Add("folder", folderIcon.ToBitmap())
        iconsImageList.Images.Add("file", fileIcon.ToBitmap())

        ' تجميع النتائج في TreeView
        For Each result In results
            Dim fileName = result.Item1
            Dim filePath = result.Item2

            ' التحقق إن كان المسار مجلدًا أو ملفًا
            Dim isFolder = IO.Directory.Exists(filePath)

            ' تحديد نوع الأيقونة
            Dim iconKey = If(isFolder, "folder", "file")
            If Not isFolder AndAlso File.Exists(filePath) Then
                ' استخراج أيقونة مخصصة إذا كان ملفًا
                Dim customIcon = Icon.ExtractAssociatedIcon(filePath)?.ToBitmap()
                If customIcon IsNot Nothing Then
                    iconsImageList.Images.Add(filePath, customIcon)
                    iconKey = filePath ' استخدام الأيقونة الخاصة
                End If
            End If

            ' إنشاء عقدة
            Dim node = New TreeNode(fileName) With {
                .Tag = filePath,
                .ImageKey = iconKey,
                .SelectedImageKey = iconKey
            }

            ' إذا كان مجلدًا، يمكن إضافة عقد فرعية
            If isFolder Then
                AddSubNodes(node, filePath)
            End If

            resultsTreeView.Nodes.Add(node)
        Next
    End Sub

    ' دالة لإضافة المجلدات والملفات الفرعية
    Private Sub AddSubNodes(parentNode As TreeNode, folderPath As String)
        Try
            ' إضافة المجلدات الفرعية
            For Each subDir In IO.Directory.GetDirectories(folderPath)
                Dim dirName = IO.Path.GetFileName(subDir)
                Dim dirNode = New TreeNode(dirName) With {
                    .Tag = subDir,
                    .ImageKey = "folder",
                    .SelectedImageKey = "folder"
                }
                ' استدعاء الدالة بشكل متكرر للعقد الفرعية
                AddSubNodes(dirNode, subDir)
                parentNode.Nodes.Add(dirNode)
            Next

            ' إضافة الملفات
            For Each file In IO.Directory.GetFiles(folderPath)
                Dim fileName = IO.Path.GetFileName(file)
                Dim fileIcon = Icon.ExtractAssociatedIcon(file)?.ToBitmap()
                Dim iconKey = "file"

                ' إضافة أيقونة مخصصة إذا كانت متوفرة
                If fileIcon IsNot Nothing Then
                    iconsImageList.Images.Add(file, fileIcon)
                    iconKey = file
                End If

                Dim fileNode = New TreeNode(fileName) With {
                    .Tag = file,
                    .ImageKey = iconKey,
                    .SelectedImageKey = iconKey
                }
                parentNode.Nodes.Add(fileNode)
            Next
        Catch ex As Exception
            ' التعامل مع الأخطاء (مثل الوصول غير المصرح به)
            System.Diagnostics.Debug.WriteLine($"Error accessing folder: {folderPath}. {ex.Message}")
        End Try
    End Sub

    ' دالة لإكمال واجهة البحث
    Private Sub CompleteSearchUI(Optional message As String = Nothing)
        ' إخفاء شريط التقدم
        ProgressBar1.Visible = False
        progressLabel.Visible = False

        ' تحديث الرسالة
        If Not String.IsNullOrEmpty(message) Then
            TxtMsg.Text = message
        End If
    End Sub

    ' دالة لإعادة ضبط واجهة المستخدم بعد انتهاء البحث
    Private Sub ResetUI()
        btnCancel.Enabled = False
        ProgressBar1.Visible = False
        progressLabel.Visible = False
    End Sub

    Private Sub btnChooseFolder_Click(sender As Object, e As EventArgs) Handles btnChooseFolder.Click
        Using fbd As New FolderBrowserDialog()
            fbd.Description = "اختر مجلد البحث"
            fbd.ShowNewFolderButton = True

            ' افتح على المسار الحالي إن وجد
            Dim currentPath As String = My.Settings.DefaultPathSearch
            If Not String.IsNullOrWhiteSpace(currentPath) AndAlso IO.Directory.Exists(currentPath) Then
                fbd.SelectedPath = currentPath
            End If

            If fbd.ShowDialog(Me) = DialogResult.OK Then
                ' حفظ المسار في الإعدادات
                My.Settings.DefaultPathSearch = fbd.SelectedPath
                My.Settings.Save()

                ' (اختياري) عرض المسار في TextBox
                If Controls.Find("txtSearchPath", True).Length > 0 Then
                    Dim tb = TryCast(Me.Controls.Find("txtSearchPath", True).FirstOrDefault(), TextBox)
                    If tb IsNot Nothing Then tb.Text = fbd.SelectedPath
                End If

                ' (اختياري) إعادة تنفيذ البحث مباشرة إن كان عندك TextBox للبحث
                ' مثال: StartSearch() أو TriggerSearchWithDelay()
                ' StartSearch()
            End If
        End Using
    End Sub

#End Region

#Region "************************ أدوات مساعدة وخدمات ***********************"
    ' دالة لتنفيذ الإجراء في الخيط الصحيح (الرئيسي)
    Private Sub ExecuteOnUIThread(action As Action)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(action)
            Else
                action()
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ExecuteOnUIThread: {ex.Message}")
        End Try
    End Sub

    ' دالة لحفظ التحديد الحالي
    Private Sub SaveCurrentSelection()
        Try
            ' التحقق من أننا نستخدم الخيط الرئيسي
            If resultsTreeView.InvokeRequired Then
                ' إذا كنا في خيط غير رئيسي، استخدم Invoke
                resultsTreeView.Invoke(Sub()
                                           If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
                                               selectedNodeTag = resultsTreeView.SelectedNode.Tag.ToString()
                                           Else
                                               selectedNodeTag = String.Empty
                                           End If
                                       End Sub)
            Else
                ' إذا كنا في الخيط الرئيسي، قم بالعمل مباشرة
                If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
                    selectedNodeTag = resultsTreeView.SelectedNode.Tag.ToString()
                Else
                    selectedNodeTag = String.Empty
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SaveCurrentSelection: {ex.Message}")
        End Try
    End Sub


    'Private Sub SaveCurrentSelection()
    '    Try
    '        If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
    '            selectedNodeTag = resultsTreeView.SelectedNode.Tag.ToString()
    '        Else
    '            selectedNodeTag = String.Empty
    '        End If
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في SaveCurrentSelection: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' استعادة التحديد السابق بعد تحديث النتائج
    ''' </summary>
    Private Sub RestoreSelection()
        Try
            If String.IsNullOrEmpty(selectedNodeTag) Then Return

            ' البحث عن العقدة باستخدام التاغ
            Dim node As TreeNode = FindNodeByTag(resultsTreeView.Nodes, selectedNodeTag)
            If node IsNot Nothing Then
                ' اختيار العقدة وجعلها مرئية
                ExecuteOnUIThread(Sub()
                                      ' تأكد من وجود العقدة
                                      If resultsTreeView.Nodes.Contains(node) Then
                                          resultsTreeView.SelectedNode = node
                                          node.EnsureVisible()
                                      Else
                                          System.Diagnostics.Debug.WriteLine("العقدة المطلوبة غير موجودة في TreeView")
                                      End If
                                  End Sub)
                Exit Sub
            End If

            ' إذا لم يتم العثور على العقدة، حاول البحث باستخدام مسارات بديلة
            For Each alternativeNode As TreeNode In GetAllNodes(resultsTreeView.Nodes)
                If alternativeNode.Tag IsNot Nothing AndAlso
               alternativeNode.Tag.ToString().EndsWith(selectedNodeTag, StringComparison.OrdinalIgnoreCase) Then
                    ExecuteOnUIThread(Sub()
                                          ' تأكد من وجود العقدة
                                          If resultsTreeView.Nodes.Contains(alternativeNode) Then
                                              resultsTreeView.SelectedNode = alternativeNode
                                              alternativeNode.EnsureVisible()
                                          Else
                                              System.Diagnostics.Debug.WriteLine("العقدة البديلة غير موجودة في TreeView")
                                          End If
                                      End Sub)
                    Exit For
                End If
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في استعادة التحديد: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' البحث عن عقدة في TreeView باستخدام Tag
    ''' </summary>
    Private Function FindNodeByTag(nodes As TreeNodeCollection, tag As String) As TreeNode
        For Each node As TreeNode In nodes
            If node.Tag IsNot Nothing AndAlso node.Tag.ToString() = tag Then
                Return node
            End If

            ' البحث في العقد الفرعية
            Dim foundNode As TreeNode = FindNodeByTag(node.Nodes, tag)
            If foundNode IsNot Nothing Then
                Return foundNode
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' الحصول على جميع العقد في TreeView
    ''' </summary>
    Private Function GetAllNodes(nodes As TreeNodeCollection) As List(Of TreeNode)
        Dim allNodes As New List(Of TreeNode)()
        For Each node As TreeNode In nodes
            allNodes.Add(node)
            allNodes.AddRange(GetAllNodes(node.Nodes))
        Next
        Return allNodes
    End Function

    ''' <summary>
    ''' توسيع جميع العقد الأصلية للعقدة المحددة
    ''' </summary>
    Private Sub ExpandParentNodes(node As TreeNode)
        If node.Parent IsNot Nothing Then
            ExpandParentNodes(node.Parent)
            node.Parent.Expand()
        End If
    End Sub

    ''' <summary>
    ''' تحديد أول ملف في نتائج البحث تلقائيًا
    ''' </summary>
    Private Sub AutoSelectFirstResult()
        Try
            ' التحقق من وجود نتائج
            If resultsTreeView.Nodes.Count > 0 Then
                ExecuteOnUIThread(Sub()
                                      ' تأكد مرة أخرى من وجود العقد
                                      If resultsTreeView.Nodes.Count > 0 Then
                                          ' حدد أول نتيجة
                                          resultsTreeView.SelectedNode = resultsTreeView.Nodes(0)
                                          resultsTreeView.SelectedNode.EnsureVisible()

                                          ' عرض الملف المحدد
                                          OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=False)

                                          ' إذا كان وضع الأرشيف، حاول إضافة الملف تلقائيًا
                                          If Me.Tag = 2 Then
                                              ' تأخير بسيط لضمان عرض الملف قبل الإضافة
                                              Threading.Thread.Sleep(500)
                                              OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=True)
                                          End If
                                      Else
                                          System.Diagnostics.Debug.WriteLine("لم يتم تحديد أول ملف: أصبحت النتائج فارغة")
                                      End If
                                  End Sub)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديد أول ملف: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحميل المجلد الافتراضي
    ''' </summary>
    Private Sub InitializeDefaultFolder()
        ' تحديد المجلد الافتراضي للبحث
        Dim defaultFolder As String = My.Settings.DefaultPathSearch

        ' التأكد من وجود المجلد
        If Not String.IsNullOrEmpty(defaultFolder) AndAlso Directory.Exists(defaultFolder) Then
            ' يمكن إضافة شرح للمجلد الافتراضي إذا لزم
        Else
            ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
            defaultFolder = Path.GetTempPath()
        End If
    End Sub
#End Region

#Region "************************ عارض PDF (PdfiumViewer) ***********************"


    Private Sub ShowPdf(filePath As String)
        Try
            ' تخلّص من المستند القديم (مهم جدًا)
            If pdfViewer.Document IsNot Nothing Then
                pdfViewer.Document.Dispose()
                pdfViewer.Document = Nothing
            End If

            ' تحميل المستند الجديد
            pdfViewer.Document = PdfDocument.Load(filePath)

        Catch ex As Exception
            MessageBox.Show("تعذر عرض ملف PDF: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    ''' <summary>
    ''' عرض ملف PDF داخل عنصر pdfViewer (PdfiumViewer).
    ''' </summary>
    Private Sub ShowPdfInViewer(filePath As String)
        If _isClosing Then Return

        ' هذه الدالة يجب أن تُستدعى من UI Thread (نحن نستدعيها داخل ExecuteOnUIThread)
        ' التخلص من المستند السابق لتفادي تسريب الذاكرة/وأخطاء System.Drawing عند الإغلاق
        If _pdfDoc IsNot Nothing Then
            Try
                _pdfDoc.Dispose()
            Catch
            End Try
            _pdfDoc = Nothing
        End If

        _pdfDoc = PdfDocument.Load(filePath)
        pdfViewer.Document = _pdfDoc
    End Sub

    ''' <summary>
    ''' فتح نافذة الطباعة لملف الـ PDF المعروض.
    ''' </summary>
    Private Sub ShowPdfPrintDialog()
        If _pdfDoc Is Nothing OrElse _isClosing Then Return

        Try
            Using printDoc As PrintDocument = _pdfDoc.CreatePrintDocument()
                Using dlg As New PrintDialog()
                    dlg.Document = printDoc
                    dlg.UseEXDialog = True
                    If dlg.ShowDialog(Me) = DialogResult.OK Then
                        printDoc.Print()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("تعذر الطباعة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تنظيف الموارد عند إغلاق النموذج.
    ''' </summary>
    Private Sub CustomSearch_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        _isClosing = True

        Try
            DelayTimer.Stop()
        Catch
        End Try

        Try
            If cts IsNot Nothing Then cts.Cancel()
        Catch
        End Try

        Try
            If _pdfDoc IsNot Nothing Then
                _pdfDoc.Dispose()
                _pdfDoc = Nothing
            End If
        Catch
        End Try
    End Sub
#End Region



End Class
