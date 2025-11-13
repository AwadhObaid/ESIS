Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Threading
Imports System.Threading.Tasks

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


        ' ✅ بدلاً من ذلك، ابدأ البحث بعد أن يتم عرض النموذج
        AddHandler Me.Shown, AddressOf CustomSearch_Shown
    End Sub

    Private Sub CustomSearch_Shown(sender As Object, e As EventArgs)
        ' الآن النموذج جاهز تمامًا، ويمكنك بدء البحث بأمان
        StartSearch()
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
                Case ".pdf", ".html", ".htm"
                    ' عرض ملف PDF أو HTML داخل WebBrowser
                    ExecuteOnUIThread(Sub()
                                          Try
                                              ' جعل العنصر مرئيًا
                                              pdfViewer.Visible = True
                                              pdfViewer.BringToFront()

                                              ' التأكد من أن المسار آمن لـ WebBrowser
                                              Dim safePath As String = filePath.Replace("\", "/")
                                              If Not safePath.StartsWith("file://") Then
                                                  safePath = "file:///" & safePath
                                              End If

                                              ' محاولة فتح الملف
                                              Try
                                                  Dim uri As New Uri(filePath)
                                                  pdfViewer.Navigate(uri)
                                              Catch uriEx As Exception
                                                  Try
                                                      pdfViewer.Navigate("file:///" & filePath.Replace("\", "/"))
                                                  Catch ex2 As Exception
                                                      ' إذا فشلت جميع المحاولات، أظهر رسالة خطأ
                                                      ExecuteOnUIThread(Sub()
                                                                            LabelFileName.Text = "فشل في عرض الملف: " & ex2.Message
                                                                            LabelFileName.ForeColor = Color.Red
                                                                        End Sub)
                                                      MessageBox.Show("فشل في فتح الملف في WebBrowser: " & ex2.Message &
                                                                 vbNewLine & "قد تحتاج إلى تثبيت Adobe Reader أو استخدام متصفح ويب خارجي.",
                                                                 "خطأ في عرض الملف", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                                  End Try
                                              End Try
                                          Catch ex As Exception
                                              ExecuteOnUIThread(Sub()
                                                                    LabelFileName.Text = "خطأ في عرض الملف: " & ex.Message
                                                                    LabelFileName.ForeColor = Color.Red
                                                                End Sub)
                                          End Try
                                      End Sub)

                    ' إذا كان هذا نقرة مزدوجة أو ضغط Enter، قم بإرسال الإشارة
                    If isDoubleClick Then
                        ' حفظ معلومات الملف المختار
                        SelectedFilePath = filePath
                        SelectedFileName = fileName
                        SelectedFileExtension = fileExtension

                        ' إرسال إشارة لجميع النماذج المستمعة
                        ExecuteOnUIThread(Sub()
                                              RaiseEvent DataSelected(filePath, fileName, fileExtension)
                                              ' تحديث نموذج الأرشيف
                                              UpdateArchiveForm(filePath, fileName, fileExtension)
                                          End Sub)

                        '' إغلاق النموذج
                        'Me.Close()
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
        pdfViewer.ShowPrintDialog()
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
                    ' عرض الملف في WebBrowser
                    archiveForm.WebBrowser1.Navigate(filePath)

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
                                              pdfViewer.Navigate(filePath)
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
    Private Sub searchTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles searchTextBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            ' تأكد من وجود نص للبحث
            If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
                Return
            End If

            ' ابدأ عملية البحث
            StartSearch()

            ' تعيين علامة للاستخدام لاحقًا
            shouldAutoAddAfterSearch = True
        ElseIf e.KeyCode = Keys.Down Then
            If searchTextBox.Text <> Nothing Then
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
    Private Sub StartSearch()
        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
            MessageBox.Show("يرجى إدخال نص للبحث", "بحث فارغ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' تهيئة واجهة المستخدم للبحث
        InitializeSearchUI()

        ' بدء البحث
        cts = New CancellationTokenSource()
        ' استخدام Task.Run بدون .Result لتجنب تجميد الواجهة
        searchTask = Task.Run(Sub() PerformSearch(searchTextBox.Text, cts.Token), cts.Token)
    End Sub

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

#Region "************************ أحداث WebBrowser ***********************"
    ' معالجة حدث اكتمال تحميل المستند في WebBrowser
    Private Sub pdfViewer_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles pdfViewer.DocumentCompleted
        Try
            ' التحقق من حالة تحميل المستند
            If pdfViewer.ReadyState = WebBrowserReadyState.Complete Then
                ' يمكن إضافة أي معالجة إضافية هنا
                System.Diagnostics.Debug.WriteLine("تم تحميل المستند بنجاح")
            Else
                ExecuteOnUIThread(Sub()
                                      LabelFileName.Text = "فشل في تحميل الملف بالكامل"
                                      LabelFileName.ForeColor = Color.Red
                                  End Sub)
            End If
        Catch ex As Exception
            ExecuteOnUIThread(Sub()
                                  LabelFileName.Text = "خطأ في تحميل المستند: " & ex.Message
                                  LabelFileName.ForeColor = Color.Red
                              End Sub)
        End Try
    End Sub

    ' معالجة حدث التنقل في WebBrowser
    Private Sub pdfViewer_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles pdfViewer.Navigating
        Try
            ' تحديث تسمية الملف أثناء التنقل
            ExecuteOnUIThread(Sub()
                                  LabelFileName.Text = "جاري تحميل: " & Path.GetFileName(e.Url.ToString())
                                  LabelFileName.ForeColor = Color.Blue
                              End Sub)
        Catch ex As Exception
            ' تجاهل الأخطاء
        End Try
    End Sub
#End Region

End Class




'Imports System.IO
'Imports System.Data
'Imports System.Drawing
'Imports System.Windows.Forms
'Imports System.Threading
'Imports System.Threading.Tasks

'Public Class CustomSearch
'    ' تعريف المتغيرات
'    ' تعريف المؤقت مع استخدام الفئة الصحيحة و WithEvents
'    Private WithEvents DelayTimer As New System.Windows.Forms.Timer()
'    Private ReadOnly delayTime As Integer = 2000 ' التأخير بالمللي ثانية (2000ms = 2 ثانية)
'    Private iconsImageList As New ImageList()
'    Dim xx As Short = 0
'    Private cts As Threading.CancellationTokenSource
'    Private searchTask As Task
'    Private isUserSelecting As Boolean = False
'    Private shouldAutoAddAfterSearch As Boolean = False
'    Private selectedNodeTag As String = String.Empty
'    Private results As New List(Of Tuple(Of String, String)) ' (اسم الملف، مسار الملف)
'    Private isLoadingData As Boolean = False

'    ' إضافة حدث لاختيار الملف
'    Public Event DataSelected(filePath As String, fileName As String, fileExtension As String)
'    Public Property SelectedFilePath As String
'    Public Property SelectedFileName As String
'    Public Property SelectedFileExtension As String

'    ''' <summary>
'    ''' دالة البناء للنموذج - يتم استدعاؤها عند إنشاء النموذج
'    ''' </summary>
'    Public Sub New()
'        InitializeComponent()
'        InitializeComponents()
'    End Sub

'    ''' <summary>
'    ''' تهيئة العناصر المخصصة
'    ''' </summary>
'    Private Sub InitializeComponents()
'        ' تهيئة TreeView
'        resultsTreeView.ImageList = iconsImageList
'        resultsTreeView.ShowNodeToolTips = True

'        ' تهيئة ProgressBar
'        ProgressBar1.Style = ProgressBarStyle.Continuous
'        ProgressBar1.Minimum = 0
'        ProgressBar1.Maximum = 100
'        ProgressBar1.Value = 0
'        progressLabel.Text = "0%"

'        ' تهيئة Search TextBox
'        searchTextBox.Focus()
'    End Sub


'#Region "************************ أحداث شجرة العرض ***********************"
'    Private Sub ResultsTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles resultsTreeView.AfterSelect
'        OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=False)
'        btnOpenLoction.Enabled = True
'        btnPrint.Enabled = True
'        'Me.Close()
'        Dim archiveForm As ArchiveForm = TryCast(Application.OpenForms("ArchiveForm"), ArchiveForm)
'        If archiveForm IsNot Nothing Then
'            archiveForm.btnAdd.Focus()
'            archiveForm.btnAdd.Text = "اضغط للاضافة"
'        End If

'    End Sub

'    Private Sub ResultsTreeView_KeyDown(sender As Object, e As KeyEventArgs) Handles resultsTreeView.KeyDown
'        If e.KeyCode = Keys.Enter AndAlso resultsTreeView.SelectedNode IsNot Nothing Then
'            OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=True)
'        ElseIf e.KeyCode = Keys.Down Then
'            ' عند الضغط على السهم السفلي
'            If resultsTreeView.SelectedNode IsNot Nothing Then
'                'AutoAddToArchiveForm()
'            End If
'        End If
'    End Sub

'    Private Sub ResultsTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseClick
'        If e.Button = MouseButtons.Left Then
'            ' تحديث التحديد
'            resultsTreeView.SelectedNode = e.Node

'            ' عرض الملف المحدد فورًا
'            If e.Node.Tag IsNot Nothing Then
'                OpenSelectedFile(e.Node, isDoubleClick:=False)
'            End If
'        End If
'    End Sub

'    Private Sub ResultsTreeView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseDoubleClick
'        OpenSelectedFile(e.Node, isDoubleClick:=True)
'        Me.Close()
'    End Sub


'#End Region



'#Region "========================  الكود المعدل   ============================"

'    Private Sub OpenSelectedFile(selectedNode As TreeNode, isDoubleClick As Boolean)
'        ' التحقق من أن العقدة و Tag ليست Nothing
'        If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then
'            Return
'        End If

'        Dim filePath As String = selectedNode.Tag.ToString()

'        ' التحقق من وجود الملف
'        If Not File.Exists(filePath) Then
'            ExecuteOnUIThread(Sub()
'                                  LabelFileName.Text = "الملف غير موجود: " & filePath
'                                  LabelFileName.ForeColor = Color.Red
'                              End Sub)
'            MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Return
'        End If

'        Try
'            ' الحصول على امتداد الملف
'            Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'            Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)

'            ' تحديث تسمية عرض الملف
'            ExecuteOnUIThread(Sub()
'                                  LabelFileName.Text = filePath
'                                  LabelFileName.ForeColor = Color.DarkBlue
'                              End Sub)

'            ' فتح الملف بناءً على نوعه
'            Select Case fileExtension
'                Case ".pdf", ".html", ".htm"
'                    ' عرض ملف PDF أو HTML داخل WebBrowser
'                    ExecuteOnUIThread(Sub()
'                                          Try
'                                              ' جعل العنصر مرئيًا
'                                              pdfViewer.Visible = True
'                                              pdfViewer.BringToFront()

'                                              ' التأكد من أن المسار آمن لـ WebBrowser
'                                              Dim safePath As String = filePath.Replace("\", "/")
'                                              If Not safePath.StartsWith("file://") Then
'                                                  safePath = "file:///" & safePath
'                                              End If

'                                              ' محاولة فتح الملف
'                                              Try
'                                                  Dim uri As New Uri(filePath)
'                                                  pdfViewer.Navigate(uri)
'                                              Catch uriEx As Exception
'                                                  Try
'                                                      pdfViewer.Navigate("file:///" & filePath.Replace("\", "/"))
'                                                  Catch ex2 As Exception
'                                                      ' إذا فشلت جميع المحاولات، أظهر رسالة خطأ
'                                                      ExecuteOnUIThread(Sub()
'                                                                            LabelFileName.Text = "فشل في عرض الملف: " & ex2.Message
'                                                                            LabelFileName.ForeColor = Color.Red
'                                                                        End Sub)
'                                                      MessageBox.Show("فشل في فتح الملف في WebBrowser: " & ex2.Message &
'                                                                 vbNewLine & "قد تحتاج إلى تثبيت Adobe Reader أو استخدام متصفح ويب خارجي.",
'                                                                 "خطأ في عرض الملف", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                                                  End Try
'                                              End Try
'                                          Catch ex As Exception
'                                              ExecuteOnUIThread(Sub()
'                                                                    LabelFileName.Text = "خطأ في عرض الملف: " & ex.Message
'                                                                    LabelFileName.ForeColor = Color.Red
'                                                                End Sub)
'                                          End Try
'                                      End Sub)

'                    ' إذا كان هذا نقرة مزدوجة أو ضغط Enter، قم بإرسال الإشارة
'                    If isDoubleClick = True Then
'                        ' حفظ معلومات الملف المختار
'                        SelectedFilePath = filePath
'                        SelectedFileName = fileName
'                        SelectedFileExtension = fileExtension

'                        ' إرسال إشارة لجميع النماذج المستمعة
'                        ExecuteOnUIThread(Sub()
'                                              RaiseEvent DataSelected(filePath, fileName, fileExtension)
'                                              ' تحديث نموذج الأرشيف
'                                              UpdateArchiveForm(filePath, fileName, fileExtension)
'                                          End Sub)

'                        '' إغلاق النموذج
'                        'Me.Close()
'                    End If
'                Case Else
'                    ' فتح الملف باستخدام التطبيق الافتراضي
'                    ExecuteOnUIThread(Sub()
'                                          Try
'                                              pdfViewer.Visible = False
'                                              Process.Start(filePath)
'                                          Catch ex As Exception
'                                              ExecuteOnUIThread(Sub()
'                                                                    LabelFileName.Text = "غير قادر على فتح الملف: " & ex.Message
'                                                                    LabelFileName.ForeColor = Color.Red
'                                                                End Sub)
'                                              MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                                          End Try
'                                      End Sub)
'            End Select
'        Catch ex As Exception
'            ' التعامل مع الأخطاء
'            ExecuteOnUIThread(Sub()
'                                  LabelFileName.Text = "غير قادر على فتح الملف: " & ex.Message
'                                  LabelFileName.ForeColor = Color.Red
'                              End Sub)
'            MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub
'    ''' <summary>
'    ''' تحديث نموذج الأرشيف مع معلومات الملف المختار
'    ''' </summary>
'    Private Sub UpdateArchiveForm(filePath As String, fileName As String, fileExtension As String)
'        Try
'            ' البحث عن نموذج الأرشيف المفتوح
'            For Each form As Form In Application.OpenForms
'                If TypeOf form Is ArchiveForm Then
'                    Dim archiveForm As ArchiveForm = DirectCast(form, ArchiveForm)

'                    ' تحديث حقول النموذج
'                    archiveForm.txtOriginalPath.Text = filePath
'                    archiveForm.txtFilename.Text = fileName
'                    archiveForm.txtFileType.Text = fileExtension.TrimStart(".")

'                    ' لا تقم بتعيين قيمة ComboArchiveFolder من المسار النسبي
'                    ' هذه الخطوة كانت تسبب المشكلة
'                    ' حساب مسار الوجهة
'                    archiveForm.CalculateDestinationPath()
'                    ' عرض الملف في WebBrowser
'                    archiveForm.WebBrowser1.Navigate(filePath)

'                    ' تعبئة الحقول الافتراضية
'                    If String.IsNullOrWhiteSpace(archiveForm.txtArchiveNum.Text) Then
'                        archiveForm.txtArchiveNum.Text = GenerateArchiveNumber()
'                    End If

'                    If String.IsNullOrWhiteSpace(archiveForm.txtArchiveSubject.Text) Then
'                        archiveForm.txtArchiveSubject.Text = "موضوع الملف"
'                    End If



'                    Exit For
'                End If
'            Next

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث نموذج الأرشيف: {ex.Message}")
'        Finally
'            ' الضغط على زر الإضافة تلقائيًا
'            'ArchiveForm.btnAdd.PerformClick()

'            'Me.Close()
'        End Try
'    End Sub

'    Private Sub searchTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles searchTextBox.KeyDown
'        If e.KeyCode = Keys.Enter Then
'            ' تأكد من وجود نص للبحث
'            If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'                Return
'            End If

'            ' ابدأ عملية البحث
'            StartSearch()

'            ' تعيين علامة للاستخدام لاحقًا
'            shouldAutoAddAfterSearch = True
'        ElseIf e.KeyCode = Keys.Down Then
'            If searchTextBox.Text <> Nothing Then
'                resultsTreeView.Select()
'                If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'                    OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=False)
'                    resultsTreeView.Refresh()
'                End If
'            End If
'        End If
'    End Sub

'    ''' <summary>
'    ''' إضافة الملف تلقائيًا إلى نموذج الأرشيف عند الضغط على السهم السفلي
'    ''' </summary>
'    Private Sub AutoAddToArchiveForm()
'        Try
'            ' التحقق من وجود عقدة محددة
'            If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'                Dim filePath As String = resultsTreeView.SelectedNode.Tag.ToString()

'                ' التحقق من وجود الملف
'                If Not File.Exists(filePath) Then
'                    MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                    Return
'                End If

'                ' تحديث تسمية المسار لعرض المسار الكامل
'                ExecuteOnUIThread(Sub()
'                                      LabelFileName.Text = "المسار: " & filePath
'                                      LabelFileName.ForeColor = Color.DarkBlue
'                                  End Sub)

'                ' عرض الملف في WebBrowser إذا كان من النوع المدعوم
'                Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'                If fileExtension = ".pdf" OrElse fileExtension = ".html" OrElse fileExtension = ".htm" Then
'                    ExecuteOnUIThread(Sub()
'                                          Try
'                                              pdfViewer.Visible = True
'                                              pdfViewer.BringToFront()
'                                              pdfViewer.Navigate(filePath)
'                                          Catch ex As Exception
'                                              ExecuteOnUIThread(Sub()
'                                                                    LabelFileName.Text = "فشل في عرض الملف: " & ex.Message
'                                                                    LabelFileName.ForeColor = Color.Red
'                                                                End Sub)
'                                          End Try
'                                      End Sub)
'                End If

'                ' تحديث نموذج الأرشيف
'                UpdateArchiveForm(filePath, Path.GetFileNameWithoutExtension(filePath), fileExtension)
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في AutoAddToArchiveForm: {ex.Message}")
'        End Try
'    End Sub


'    ''' <summary>
'    ''' بدء عملية البحث
'    ''' </summary>
'    Private Sub StartSearch()
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            MessageBox.Show("يرجى إدخال نص للبحث", "بحث فارغ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'            Return
'        End If

'        ' تهيئة واجهة المستخدم للبحث
'        InitializeSearchUI()

'        ' بدء البحث
'        cts = New CancellationTokenSource()
'        ' استخدام Task.Run بدون .Result لتجنب تجميد الواجهة
'        searchTask = Task.Run(Sub() PerformSearch(searchTextBox.Text, cts.Token), cts.Token)
'    End Sub

'    ''' <summary>
'    ''' تنفيذ عملية البحث وإرجاع النتائج
'    ''' </summary>
'    Private Function PerformSearch(query As String, token As CancellationToken) As List(Of Tuple(Of String, String))
'        Dim searchResults As New List(Of Tuple(Of String, String))()

'        Try
'            ' حفظ التحديد الحالي قبل البدء في البحث
'            If Not isUserSelecting Then
'                SaveCurrentSelection()
'            End If

'            ' البحث في مجلد الأرشيف الافتراضي
'            Dim folderToSearch As String = My.Settings.DefaultPathSearch

'            ' التحقق من وجود المجلد
'            If Not String.IsNullOrEmpty(folderToSearch) AndAlso Directory.Exists(folderToSearch) Then
'                token.ThrowIfCancellationRequested()
'                SearchInFolder(folderToSearch, query, token, searchResults)
'            Else
'                ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'                Dim tempFolder As String = Path.GetTempPath()
'                If Directory.Exists(tempFolder) Then
'                    token.ThrowIfCancellationRequested()
'                    SearchInFolder(tempFolder, query, token, searchResults)
'                End If
'            End If
'        Catch ex As OperationCanceledException
'            ' تم إلغاء البحث
'        Catch ex As Exception
'            ' حدث خطأ
'            System.Diagnostics.Debug.WriteLine($"حدث خطأ في PerformSearch: {ex.Message}")
'        End Try

'        Return searchResults
'    End Function

'    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
'        If cts IsNot Nothing Then
'            cts.Cancel()
'        End If
'    End Sub

'    Private Sub SearchInFolder(folderPath As String, query As String, token As CancellationToken, ByRef results As List(Of Tuple(Of String, String)))
'        Try
'            ' التحقق من وجود المجلد
'            If Not Directory.Exists(folderPath) Then
'                Return
'            End If

'            ' البحث في الملفات
'            Dim files = Directory.GetFiles(folderPath, $"*{query}*", SearchOption.AllDirectories)
'            Dim totalFiles = files.Length

'            ' التحقق من وجود ملفات
'            If totalFiles = 0 Then
'                Return
'            End If

'            Dim processedFiles = 0

'            For Each file In files
'                ' التحقق من طلب الإلغاء
'                token.ThrowIfCancellationRequested()

'                Dim fileName As String = Path.GetFileName(file)
'                results.Add(Tuple.Create(fileName, file))

'                processedFiles += 1
'                Dim progress = CInt((processedFiles / totalFiles) * 100)

'                ' تحديث شريط التقدم
'                If ProgressBar1.GetCurrentParent().InvokeRequired Then
'                    ProgressBar1.GetCurrentParent().Invoke(Sub()
'                                                               ProgressBar1.Value = progress
'                                                               progressLabel.Text = $"{progress}%"
'                                                           End Sub)
'                Else
'                    ProgressBar1.Value = progress
'                    progressLabel.Text = $"{progress}%"
'                End If
'            Next
'        Catch ex As Exception
'            ' التعامل مع الأخطاء (مثل الوصول غير المصرح به)
'            System.Diagnostics.Debug.WriteLine($"Error accessing folder: {folderPath}. {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' توليد رقم أرشيف فريد
'    ''' </summary>
'    Private Function GenerateArchiveNumber() As String
'        Try
'            ' استخدام التاريخ والوقت لضمان الفريد
'            Return $"ARC-{DateTime.Now:yyyyMMdd-HHmmss}"
'        Catch ex As Exception
'            Return $"ARC-{DateTime.Now.Ticks}"
'        End Try
'    End Function



'    Private Async Sub DelayTimer_Tick(sender As Object, e As EventArgs) Handles DelayTimer.Tick
'        ' إيقاف Timer بعد انتهاء الوقت
'        DelayTimer.Stop()

'        ' التحقق من قيمة Tag
'        If Not (Me.Tag = 1 OrElse Me.Tag = 2) Then Return

'        ' التحقق من صحة النص المدخل
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            Dim errorMsg = If(Me.Tag = 1, "لم يتم العثور على ملف, الرجاء إدخال قيمة صحيحة", "Please enter a search term.")
'            TxtMsg.Text = errorMsg
'            Return
'        End If

'        ' تهيئة العناصر
'        InitializeSearchUI()

'        ' إعداد مصدر الإلغاء
'        cts = New Threading.CancellationTokenSource()
'        Dim token = cts.Token

'        Try
'            ' استخدام Task.Run بدون .Result لتجنب تجميد الواجهة
'            Dim results = Await Task.Run(Function() PerformSearch(searchTextBox.Text, token), token)
'            ExecuteOnUIThread(Sub()
'                                  UpdateUIWithResults(results)
'                                  CompleteSearchUI()

'                                  ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'                                  If Not isUserSelecting Then
'                                      RestoreSelection()

'                                      ' تحديد أول ملف تلقائيًا بعد انتهاء البحث
'                                      AutoSelectFirstResult()
'                                  End If
'                              End Sub)
'        Catch ex As OperationCanceledException
'            ExecuteOnUIThread(Sub()
'                                  CompleteSearchUI("تم إلغاء البحث")
'                              End Sub)
'        Catch ex As Exception
'            ExecuteOnUIThread(Sub()
'                                  CompleteSearchUI($"حدث خطأ: {ex.Message}")
'                              End Sub)
'        End Try
'    End Sub

'    ' دالة لتهيئة واجهة البحث
'    Private Sub InitializeSearchUI()
'        resultsTreeView.Nodes.Clear()
'        iconsImageList.Images.Clear()
'        ProgressBar1.Value = 0
'        ProgressBar1.Visible = True
'        progressLabel.Visible = True
'        progressLabel.Text = "0 %"
'        btnCancel.Enabled = True
'        btnOpenLoction.Enabled = False
'        btnPrint.Enabled = False
'    End Sub

'    ' تحسين دالة لتحديث واجهة المستخدم بالنتائج
'    Private Sub UpdateUIWithResults(results As List(Of Tuple(Of String, String)))
'        ' تنظيف TreeView والـ ImageList
'        iconsImageList.Images.Clear()
'        resultsTreeView.Nodes.Clear()

'        ' إضافة أيقونات افتراضية للمجلدات والملفات
'        Dim folderIcon As Icon = SystemIcons.WinLogo ' أيقونة المجلد (يمكنك تخصيصها)
'        Dim fileIcon As Icon = SystemIcons.Application ' أيقونة الملف (يمكنك تخصيصها)
'        iconsImageList.Images.Add("folder", folderIcon.ToBitmap())
'        iconsImageList.Images.Add("file", fileIcon.ToBitmap())

'        ' تجميع النتائج في TreeView
'        For Each result In results
'            Dim fileName = result.Item1
'            Dim filePath = result.Item2

'            ' التحقق إن كان المسار مجلدًا أو ملفًا
'            Dim isFolder = IO.Directory.Exists(filePath)

'            ' تحديد نوع الأيقونة
'            Dim iconKey = If(isFolder, "folder", "file")
'            If Not isFolder AndAlso File.Exists(filePath) Then
'                ' استخراج أيقونة مخصصة إذا كان ملفًا
'                Dim customIcon = Icon.ExtractAssociatedIcon(filePath)?.ToBitmap()
'                If customIcon IsNot Nothing Then
'                    iconsImageList.Images.Add(filePath, customIcon)
'                    iconKey = filePath ' استخدام الأيقونة الخاصة
'                End If
'            End If

'            ' إنشاء عقدة
'            Dim node = New TreeNode(fileName) With {
'                .Tag = filePath,
'                .ImageKey = iconKey,
'                .SelectedImageKey = iconKey
'            }

'            ' إذا كان مجلدًا، يمكن إضافة عقد فرعية
'            If isFolder Then
'                AddSubNodes(node, filePath)
'            End If

'            resultsTreeView.Nodes.Add(node)
'        Next
'    End Sub

'    ' دالة لإضافة المجلدات والملفات الفرعية
'    Private Sub AddSubNodes(parentNode As TreeNode, folderPath As String)
'        Try
'            ' إضافة المجلدات الفرعية
'            For Each subDir In IO.Directory.GetDirectories(folderPath)
'                Dim dirName = IO.Path.GetFileName(subDir)
'                Dim dirNode = New TreeNode(dirName) With {
'                    .Tag = subDir,
'                    .ImageKey = "folder",
'                    .SelectedImageKey = "folder"
'                }
'                ' استدعاء الدالة بشكل متكرر للعقد الفرعية
'                AddSubNodes(dirNode, subDir)
'                parentNode.Nodes.Add(dirNode)
'            Next

'            ' إضافة الملفات
'            For Each file In IO.Directory.GetFiles(folderPath)
'                Dim fileName = IO.Path.GetFileName(file)
'                Dim fileIcon = Icon.ExtractAssociatedIcon(file)?.ToBitmap()
'                Dim iconKey = "file"

'                ' إضافة أيقونة مخصصة إذا كانت متوفرة
'                If fileIcon IsNot Nothing Then
'                    iconsImageList.Images.Add(file, fileIcon)
'                    iconKey = file
'                End If

'                Dim fileNode = New TreeNode(fileName) With {
'                    .Tag = file,
'                    .ImageKey = iconKey,
'                    .SelectedImageKey = iconKey
'                }
'                parentNode.Nodes.Add(fileNode)
'            Next
'        Catch ex As Exception
'            ' التعامل مع الأخطاء (مثل الوصول غير المصرح به)
'            System.Diagnostics.Debug.WriteLine($"Error accessing folder: {folderPath}. {ex.Message}")
'        End Try
'    End Sub

'    ' دالة لإكمال واجهة البحث
'    Private Sub CompleteSearchUI(Optional message As String = Nothing)
'        ' إخفاء شريط التقدم
'        ProgressBar1.Visible = False
'        progressLabel.Visible = False

'        ' تحديث الرسالة
'        If Not String.IsNullOrEmpty(message) Then
'            TxtMsg.Text = message
'        End If
'    End Sub

'    ' دالة لتنفيذ الإجراء في الخيط الصحيح (الرئيسي)
'    Private Sub ExecuteOnUIThread(action As Action)
'        Try
'            If Me.InvokeRequired Then
'                Me.Invoke(action)
'            Else
'                action()
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في ExecuteOnUIThread: {ex.Message}")
'        End Try
'    End Sub

'    ' دالة لحفظ التحديد الحالي
'    Private Sub SaveCurrentSelection()
'        Try
'            If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'                selectedNodeTag = resultsTreeView.SelectedNode.Tag.ToString()
'            Else
'                selectedNodeTag = String.Empty
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في SaveCurrentSelection: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' استعادة التحديد السابق بعد تحديث النتائج
'    ''' </summary>
'    Private Sub RestoreSelection()
'        Try
'            If String.IsNullOrEmpty(selectedNodeTag) Then Return

'            ' البحث عن العقدة باستخدام التاغ
'            Dim node As TreeNode = FindNodeByTag(resultsTreeView.Nodes, selectedNodeTag)
'            If node IsNot Nothing Then
'                ' اختيار العقدة وجعلها مرئية
'                ExecuteOnUIThread(Sub()
'                                      ' تأكد من وجود العقدة
'                                      If resultsTreeView.Nodes.Contains(node) Then
'                                          resultsTreeView.SelectedNode = node
'                                          node.EnsureVisible()
'                                      Else
'                                          System.Diagnostics.Debug.WriteLine("العقدة المطلوبة غير موجودة في TreeView")
'                                      End If
'                                  End Sub)
'                Exit Sub
'            End If

'            ' إذا لم يتم العثور على العقدة، حاول البحث باستخدام مسارات بديلة
'            For Each alternativeNode As TreeNode In GetAllNodes(resultsTreeView.Nodes)
'                If alternativeNode.Tag IsNot Nothing AndAlso
'               alternativeNode.Tag.ToString().EndsWith(selectedNodeTag, StringComparison.OrdinalIgnoreCase) Then
'                    ExecuteOnUIThread(Sub()
'                                          ' تأكد من وجود العقدة
'                                          If resultsTreeView.Nodes.Contains(alternativeNode) Then
'                                              resultsTreeView.SelectedNode = alternativeNode
'                                              alternativeNode.EnsureVisible()
'                                          Else
'                                              System.Diagnostics.Debug.WriteLine("العقدة البديلة غير موجودة في TreeView")
'                                          End If
'                                      End Sub)
'                    Exit For
'                End If
'            Next
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في استعادة التحديد: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' البحث عن عقدة في TreeView باستخدام Tag
'    ''' </summary>
'    Private Function FindNodeByTag(nodes As TreeNodeCollection, tag As String) As TreeNode
'        For Each node As TreeNode In nodes
'            If node.Tag IsNot Nothing AndAlso node.Tag.ToString() = tag Then
'                Return node
'            End If

'            ' البحث في العقد الفرعية
'            Dim foundNode As TreeNode = FindNodeByTag(node.Nodes, tag)
'            If foundNode IsNot Nothing Then
'                Return foundNode
'            End If
'        Next
'        Return Nothing
'    End Function

'    ''' <summary>
'    ''' الحصول على جميع العقد في TreeView
'    ''' </summary>
'    Private Function GetAllNodes(nodes As TreeNodeCollection) As List(Of TreeNode)
'        Dim allNodes As New List(Of TreeNode)()
'        For Each node As TreeNode In nodes
'            allNodes.Add(node)
'            allNodes.AddRange(GetAllNodes(node.Nodes))
'        Next
'        Return allNodes
'    End Function

'    ''' <summary>
'    ''' توسيع جميع العقد الأصلية للعقدة المحددة
'    ''' </summary>
'    Private Sub ExpandParentNodes(node As TreeNode)
'        If node.Parent IsNot Nothing Then
'            ExpandParentNodes(node.Parent)
'            node.Parent.Expand()
'        End If
'    End Sub

'    ''' <summary>
'    ''' تحديد أول ملف في نتائج البحث تلقائيًا
'    ''' </summary>
'    Private Sub AutoSelectFirstResult()
'        Try
'            ' التحقق من وجود نتائج
'            If resultsTreeView.Nodes.Count > 0 Then
'                ExecuteOnUIThread(Sub()
'                                      ' تأكد مرة أخرى من وجود العقد
'                                      If resultsTreeView.Nodes.Count > 0 Then
'                                          ' حدد أول نتيجة
'                                          resultsTreeView.SelectedNode = resultsTreeView.Nodes(0)
'                                          resultsTreeView.SelectedNode.EnsureVisible()

'                                          ' عرض الملف المحدد
'                                          OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=False)

'                                          ' إذا كان وضع الأرشيف، حاول إضافة الملف تلقائيًا
'                                          If Me.Tag = 2 Then
'                                              ' تأخير بسيط لضمان عرض الملف قبل الإضافة
'                                              Threading.Thread.Sleep(500)
'                                              OpenSelectedFile(resultsTreeView.SelectedNode, isDoubleClick:=True)
'                                          End If
'                                      Else
'                                          System.Diagnostics.Debug.WriteLine("لم يتم تحديد أول ملف: أصبحت النتائج فارغة")
'                                      End If
'                                  End Sub)
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديد أول ملف: {ex.Message}")
'        End Try
'    End Sub

'    ' دالة لإعادة ضبط واجهة المستخدم بعد انتهاء البحث
'    Private Sub ResetUI()
'        btnCancel.Enabled = False
'        ProgressBar1.Visible = False
'        progressLabel.Visible = False
'    End Sub



'    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
'        pdfViewer.ShowPrintDialog()
'        btnPrint.Enabled = False
'    End Sub

'    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            resultsTreeView.Nodes.Clear()
'            TxtMsg.Text = "لم يتم العثور على ملف , الرجاء إدخال قيمة صحيحة في مربع البحث"
'        Else
'            ' إعادة تشغيل Timer عند تغيير النص
'            DelayTimer.Stop()
'            DelayTimer.Start()
'            TxtMsg.Text = Nothing
'        End If
'    End Sub

'    Private Sub CustomSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        ' إعدادات Timer
'        DelayTimer.Interval = delayTime

'        ' تحميل المجلد الافتراضي
'        InitializeDefaultFolder()

'        ' تعيين تركيز مربع البحث
'        searchTextBox.Select()

'        ' تعطيل أزرار الملفات حتى يتم اختيار ملف
'        btnOpenLoction.Enabled = False
'        btnPrint.Enabled = False
'    End Sub

'    ''' <summary>
'    ''' تحميل المجلد الافتراضي
'    ''' </summary>
'    Private Sub InitializeDefaultFolder()
'        ' تحديد المجلد الافتراضي للبحث
'        Dim defaultFolder As String = My.Settings.DefaultPathSearch

'        ' التأكد من وجود المجلد
'        If Not String.IsNullOrEmpty(defaultFolder) AndAlso Directory.Exists(defaultFolder) Then
'            ' يمكن إضافة شرح للمجلد الافتراضي إذا لزم
'        Else
'            ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'            defaultFolder = Path.GetTempPath()
'        End If
'    End Sub

'    ''' <summary>
'    ''' فتح موقع الملف المحدد
'    ''' </summary>
'    Private Sub BtnOpenLoction_Click(sender As Object, e As EventArgs) Handles btnOpenLoction.Click
'        If resultsTreeView.SelectedNode Is Nothing Then
'            Exit Sub
'        Else
'            Process.Start("explorer.exe", "/select," & resultsTreeView.SelectedNode.Tag)
'        End If
'    End Sub

'    ' معالجة حدث اكتمال تحميل المستند في WebBrowser
'    Private Sub pdfViewer_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles pdfViewer.DocumentCompleted
'        Try
'            ' التحقق من حالة تحميل المستند
'            If pdfViewer.ReadyState = WebBrowserReadyState.Complete Then
'                ' يمكن إضافة أي معالجة إضافية هنا
'                System.Diagnostics.Debug.WriteLine("تم تحميل المستند بنجاح")
'            Else
'                ExecuteOnUIThread(Sub()
'                                      LabelFileName.Text = "فشل في تحميل الملف بالكامل"
'                                      LabelFileName.ForeColor = Color.Red
'                                  End Sub)
'            End If
'        Catch ex As Exception
'            ExecuteOnUIThread(Sub()
'                                  LabelFileName.Text = "خطأ في تحميل المستند: " & ex.Message
'                                  LabelFileName.ForeColor = Color.Red
'                              End Sub)
'        End Try
'    End Sub

'    ' معالجة حدث التنقل في WebBrowser
'    Private Sub pdfViewer_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles pdfViewer.Navigating
'        Try
'            ' تحديث تسمية الملف أثناء التنقل
'            ExecuteOnUIThread(Sub()
'                                  LabelFileName.Text = "جاري تحميل: " & Path.GetFileName(e.Url.ToString())
'                                  LabelFileName.ForeColor = Color.Blue
'                              End Sub)
'        Catch ex As Exception
'            ' تجاهل الأخطاء
'        End Try
'    End Sub

'#End Region

'End Class

#Region "الكود القديم  ===================================================="
''Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles searchButton.Click
''    Dim searchQuery As String = searchTextBox.Text
''    resultsTreeView.Nodes.Clear()
''    iconsImageList.Images.Clear()

''    If Not String.IsNullOrWhiteSpace(searchQuery) Then
''        Try
''            Dim files = Directory.GetFiles(My.Settings.SelectedFolder, $"*{searchQuery}*", SearchOption.AllDirectories)
''            For Each file In files
''                Dim fileName As String = Path.GetFileName(file)

''                ' استخراج الأيقونة الخاصة بالملف
''                Dim fileIcon As Icon = Icon.ExtractAssociatedIcon(file)
''                Dim fileImage As Image = fileIcon.ToBitmap()
''                iconsImageList.Images.Add(file, fileImage)

''                ' إضافة الملف كعقدة إلى TreeView
''                Dim node As New TreeNode(fileName) With {
''                    .Tag = file, ' تخزين المسار الكامل للملف في الخاصية Tag
''                    .ImageKey = file,
''                    .SelectedImageKey = file
''                }
''                resultsTreeView.Nodes.Add(node)
''            Next
''        Catch ex As UnauthorizedAccessException
''            searchTextBox.Clear()
''            'MessageBox.Show("تم رفض الوصول إلى بعض الدلائل.")
''        Catch ex As Exception
''            MessageBox.Show("حدث خطأ: " & ex.Message)
''        End Try
''    Else
''        'MessageBox.Show("الرجاء إدخال مصطلح البحث.")
''    End If
''End Sub


'Private Sub searchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged

'    searchButton.PerformClick()
'End Sub



'Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles searchButton.Click
'    Dim searchQuery As String = searchTextBox.Text
'    resultsTreeView.Nodes.Clear()
'    iconsImageList.Images.Clear()
'    ProgressBar1.Value = 0
'    ProgressBar1.Visible = True
'    Threading.Thread.Sleep(3000)

'    If Not String.IsNullOrWhiteSpace(searchQuery) Then
'        If Not backgroundWorker.IsBusy Then
'            searchButton.Enabled = False
'            cancelButton.Enabled = True
'            backgroundWorker.RunWorkerAsync(searchQuery)
'        Else
'            MessageBox.Show("جاري البحث بالفعل.")
'        End If
'    Else
'        MessageBox.Show("الرجاء إدخال مصطلح البحث.")
'    End If
'End Sub

'Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
'    If backgroundWorker.IsBusy Then
'        backgroundWorker.CancelAsync()
'        cancelButton.Enabled = False ' تعطيل زر الإلغاء بعد طلب الإلغاء
'    End If
'End Sub

'Private Sub BackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles backgroundWorker.DoWork
'    Dim searchQuery As String = e.Argument.ToString()
'    Dim results As New List(Of Tuple(Of String, String)) ' قائمة لتخزين اسم الملف ومساره

'    Try
'        Dim files = Directory.GetFiles(My.Settings.FolderSaderPath, $"*{searchQuery}*", SearchOption.AllDirectories)
'        Dim totalFiles As Integer = files.Length
'        Dim processedFiles As Integer = 0


'        For Each file In files
'            If backgroundWorker.CancellationPending Then
'                e.Cancel = True
'                Exit For
'            End If

'            Dim fileName As String = Path.GetFileName(file)
'            results.Add(Tuple.Create(fileName, file))
'            processedFiles += 1

'            ' تحديث شريط التقدم
'            Dim progressPercentage As Integer = CInt((processedFiles / totalFiles) * 100)
'            backgroundWorker.ReportProgress(progressPercentage)
'        Next
'    Catch ex As UnauthorizedAccessException
'        e.Result = ex.Message
'    Catch ex As Exception
'        e.Result = ex.Message
'    End Try

'    e.Result = results
'End Sub

'Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles backgroundWorker.ProgressChanged
'    ProgressBar1.Value = e.ProgressPercentage
'End Sub

'Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles backgroundWorker.RunWorkerCompleted
'    searchButton.Enabled = True
'    cancelButton.Enabled = False
'    ProgressBar1.Visible = False


'    If e.Cancelled Then
'        MessageBox.Show("Search was cancelled.")
'    ElseIf e.Error IsNot Nothing Then
'        MessageBox.Show("An error occurred: " & e.Error.Message)
'    Else
'        Dim results = TryCast(e.Result, List(Of Tuple(Of String, String)))

'        If results IsNot Nothing Then
'            For Each result In results
'                Dim fileName = result.Item1
'                Dim filePath = result.Item2

'                ' استخراج الأيقونة الخاصة بالملف
'                Dim fileIcon As Icon = Icon.ExtractAssociatedIcon(filePath)
'                Dim fileImage As Image = fileIcon.ToBitmap()
'                iconsImageList.Images.Add(filePath, fileImage)

'                ' إضافة الملف كعقدة إلى TreeView
'                Dim node As New TreeNode(fileName) With {
'                    .Tag = filePath, ' تخزين المسار الكامل للملف في الخاصية Tag
'                    .ImageKey = filePath,
'                    .SelectedImageKey = filePath
'                }
'                resultsTreeView.Nodes.Add(node)
'            Next
'        End If
'    End If

'End Sub


'Private Sub OpenSelectedFile(selectedNode As TreeNode)
'    If selectedNode IsNot Nothing AndAlso selectedNode.Tag IsNot Nothing Then
'        Dim filePath As String = selectedNode.Tag.ToString()
'        Try
'            If Path.GetExtension(filePath).ToLower() = ".pdf" Or Path.GetExtension(filePath).ToLower() = ".html" Or Path.GetExtension(filePath).ToLower() = ".htm" Then
'                ' عرض ملف PDF داخل WebBrowser

'                pdfViewer.Visible = True
'                pdfViewer.Navigate(filePath)
'                Dim Frm As Frm_Sader_2024 = TryCast(Application.OpenForms("Frm_Sader_2024"), Frm_Sader_2024)
'                If Frm IsNot Nothing And xx = 0 Then
'                    Frm.WebBrows.Navigate(filePath)
'                    Frm.txt_path_Sader.Text = filePath
'                    Me.Close()
'                    Frm.btnSave.PerformClick()
'                Else
'                    Me.pdfViewer.Navigate(filePath)
'                    LabelFileName.Text = filePath
'                End If
'            Else
'                ' فتح الملف باستخدام التطبيق الافتراضي
'                pdfViewer.Visible = False
'                Process.Start(filePath)
'            End If

'        Catch ex As Exception
'            MessageBox.Show("غير قادر على فتح الملف: " & ex.Message)
'        End Try
'    End If
'End Sub

'Private Sub CustomSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'    resultsTreeView.Select()
'    OpenSelectedFile(resultsTreeView.SelectedNode)
'    ' إعداد BackgroundWorker
'    backgroundWorker.WorkerReportsProgress = True
'    backgroundWorker.WorkerSupportsCancellation = True
'    searchTextBox.Select()
'    btnPrint.Enabled = False
'End Sub

'Private Sub btnPath_Click(sender As Object, e As EventArgs) Handles btnPath.Click
'    Dim frm As New SelectPath
'    frm.ShowDialog()
'End Sub

'Private Sub resultsTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseClick
'    xx = 1
'    OpenSelectedFile(e.Node)
'End Sub

'Private Sub resultsTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles resultsTreeView.AfterSelect
'    xx = 1

'    OpenSelectedFile(e.Node)
'    resultsTreeView.Select()
'End Sub

'Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
'    pdfViewer.ShowPrintDialog()
'    btnPrint.Enabled = False
'End Sub


#End Region

'Imports System.IO
'Imports System.Data
'Imports System.Drawing
'Imports System.Windows.Forms
'Imports System.Threading
'Imports System.Threading.Tasks

'Public Class CustomSearch
'    ' تعريف المتغيرات
'    Private WithEvents DelayTimer As New System.Windows.Forms.Timer()
'    Private ReadOnly delayTime As Integer = 2000 ' التأخير بالمللي ثانية (2000ms = 2 ثانية)
'    Private cts As Threading.CancellationTokenSource
'    Private isArchiveMode As Boolean = False ' تحديد وضع النموذج (1: مستعرض ملفات، 2: وضع الأرشيف)
'    Public Event DataSelected(filePath As String, fileName As String, fileExtension As String) ' حدث لاختيار ملف

'    ' إضافة تعريف searchTask كما طلب الخطأ
'    Private searchTask As Task

'    ' متغيرات إضافية
'    Private results As New List(Of Tuple(Of String, String)) ' (اسم الملف، مسار الملف)
'    Private iconsImageList As New ImageList()
'    Private isSearchInProgress As Boolean = False
'    Private searchTimer As New Stopwatch()
'    Private searchQuery As String = String.Empty
'    Private isLoadingData As Boolean = False

'    ' متغير لحفظ حالة التحديد
'    Private selectedNodeTag As String = String.Empty
'    Private isUserSelecting As Boolean = False

'    Dim xx As Short = 0

'    ''' <summary>
'    ''' دالة البناء للنموذج - يتم استدعاؤها عند إنشاء النموذج
'    ''' </summary>
'    Public Sub New()
'        InitializeComponent()
'        InitializeComponents()
'    End Sub

'    ''' <summary>
'    ''' تهيئة المكونات الأساسية للنموذج
'    ''' </summary>
'    Private Sub InitializeComponents()
'        ' تعيين خصائص Timer
'        DelayTimer.Interval = delayTime

'        ' تعيين خصائص شريط التقدم
'        ProgressBar1.Style = ProgressBarStyle.Continuous
'        ProgressBar1.Minimum = 0
'        ProgressBar1.Maximum = 100

'        ' إعداد قائمة الصور
'        iconsImageList.ColorDepth = ColorDepth.Depth32Bit
'        iconsImageList.ImageSize = New Size(16, 16)

'        ' تهيئة عناصر واجهة المستخدم
'        InitializeUI()
'    End Sub

'    Private Sub InitializeUI()
'        ' تهيئة TreeView
'        resultsTreeView.ImageList = iconsImageList
'        resultsTreeView.ShowNodeToolTips = True
'        resultsTreeView.HideSelection = False ' لضمان ظهور التحديد حتى عند فقدان التركيز

'        ' تهيئة ProgressBar
'        ProgressBar1.Minimum = 0
'        ProgressBar1.Maximum = 100
'        ProgressBar1.Value = 0
'        progressLabel.Text = "0%"

'        ' تهيئة Search TextBox
'        searchTextBox.Focus()
'    End Sub
'    ''' <summary>
'    ''' بدء عملية البحث
'    ''' </summary>
'    Private Sub StartSearch()
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            MessageBox.Show("يرجى إدخال نص للبحث", "بحث فارغ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'            Return
'        End If

'        ' تهيئة واجهة المستخدم للبحث
'        InitializeSearchUI()

'        ' بدء البحث
'        cts = New CancellationTokenSource()
'        ' استخدام Task.Run بدون .Result لتجنب تجميد الواجهة
'        searchTask = Task.Run(Sub() PerformSearch(searchTextBox.Text, cts.Token), cts.Token)
'    End Sub

'    Private Sub InitializeSearchUI()
'        ' تفريغ النتائج
'        ExecuteOnUIThread(Sub()
'                              resultsTreeView.Nodes.Clear()
'                              iconsImageList.Images.Clear()
'                          End Sub)

'        ' تهيئة شريط التقدم
'        ExecuteOnUIThread(Sub()
'                              ProgressBar1.Value = 0
'                              progressLabel.Text = "0%"
'                              searchTimer.Restart()
'                              isSearchInProgress = True
'                              TxtMsg.Text = "جاري البحث..."
'                          End Sub)

'        searchQuery = searchTextBox.Text
'    End Sub

'    'Private Sub PerformSearch(query As String, token As CancellationToken)
'    '    Try
'    '        ' حفظ التحديد الحالي قبل البدء في البحث
'    '        If Not isUserSelecting Then
'    '            SaveCurrentSelection()
'    '        End If

'    '        ' البحث في مجلد الأرشيف الافتراضي
'    '        Dim folderToSearch As String = My.Settings.DefaultPathSearch
'    '        Dim searchResults As New List(Of Tuple(Of String, String))()

'    '        ' التحقق من وجود المجلد
'    '        If Not String.IsNullOrEmpty(folderToSearch) AndAlso Directory.Exists(folderToSearch) Then
'    '            token.ThrowIfCancellationRequested()
'    '            SearchInFolder(folderToSearch, query, token, searchResults)
'    '        Else
'    '            ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'    '            Dim tempFolder As String = Path.GetTempPath()
'    '            If Directory.Exists(tempFolder) Then
'    '                token.ThrowIfCancellationRequested()
'    '                SearchInFolder(tempFolder, query, token, searchResults)
'    '            End If
'    '        End If

'    '        ' تحديث النتائج
'    '        results = searchResults

'    '        ' تحديث واجهة المستخدم مع النتائج
'    '        ExecuteOnUIThread(Sub()
'    '                              UpdateUIWithResults(results)
'    '                              CompleteSearchUI()

'    '                              ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'    '                              If Not isUserSelecting Then
'    '                                  RestoreSelection()

'    '                                  ' عرض أول ملف تلقائيًا
'    '                                  If resultsTreeView.Nodes.Count > 0 Then
'    '                                      resultsTreeView.SelectedNode = resultsTreeView.Nodes(0)
'    '                                      ' استدعاء AfterSelect يدويًا
'    '                                      Dim args As New TreeViewEventArgs(resultsTreeView.SelectedNode)
'    '                                      resultsTreeView_AfterSelect(Nothing, args)
'    '                                  End If
'    '                              End If
'    '                          End Sub)
'    '    Catch ex As OperationCanceledException
'    '        ' تم إلغاء البحث
'    '        ExecuteOnUIThread(Sub()
'    '                              CompleteSearchUI("تم إلغاء البحث")
'    '                          End Sub)
'    '    Catch ex As Exception
'    '        ' حدث خطأ
'    '        ExecuteOnUIThread(Sub()
'    '                              CompleteSearchUI($"حدث خطأ: {ex.Message}")
'    '                          End Sub)
'    '    End Try
'    'End Sub

'    'Private Sub PerformSearch(query As String, token As CancellationToken)
'    '    Try
'    '        ' حفظ التحديد الحالي قبل البدء في البحث
'    '        If Not isUserSelecting Then
'    '            SaveCurrentSelection()
'    '        End If

'    '        ' البحث في مجلد الأرشيف الافتراضي
'    '        Dim folderToSearch As String = My.Settings.DefaultPathSearch
'    '        Dim searchResults As New List(Of Tuple(Of String, String))()

'    '        ' التحقق من وجود المجلد
'    '        If Not String.IsNullOrEmpty(folderToSearch) AndAlso Directory.Exists(folderToSearch) Then
'    '            token.ThrowIfCancellationRequested()
'    '            SearchInFolder(folderToSearch, query, token, searchResults)
'    '        Else
'    '            ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'    '            Dim tempFolder As String = Path.GetTempPath()
'    '            If Directory.Exists(tempFolder) Then
'    '                token.ThrowIfCancellationRequested()
'    '                SearchInFolder(tempFolder, query, token, searchResults)
'    '            End If
'    '        End If

'    '        ' تحديث النتائج
'    '        results = searchResults

'    '        ' تحديث واجهة المستخدم مع النتائج
'    '        ExecuteOnUIThread(Sub()
'    '                              UpdateUIWithResults(results)
'    '                              CompleteSearchUI()

'    '                              ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'    '                              If Not isUserSelecting Then
'    '                                  RestoreSelection()
'    '                              End If
'    '                          End Sub)
'    '    Catch ex As OperationCanceledException
'    '        ' تم إلغاء البحث
'    '        ExecuteOnUIThread(Sub()
'    '                              CompleteSearchUI("تم إلغاء البحث")
'    '                          End Sub)
'    '    Catch ex As Exception
'    '        ' حدث خطأ
'    '        ExecuteOnUIThread(Sub()
'    '                              CompleteSearchUI($"حدث خطأ: {ex.Message}")
'    '                          End Sub)
'    '    End Try
'    'End Sub

'    Private Sub SearchInFolder(folderPath As String, query As String, token As CancellationToken, ByRef results As List(Of Tuple(Of String, String)))
'        Try
'            ' التحقق من وجود المجلد
'            If Not Directory.Exists(folderPath) Then
'                Return
'            End If

'            ' البحث في الملفات فقط (بدون هيكل المجلدات)
'            Dim files = Directory.GetFiles(folderPath, $"*{query}*", SearchOption.AllDirectories)
'            Dim totalFiles = files.Length

'            ' التحقق من وجود ملفات
'            If totalFiles = 0 Then
'                Return
'            End If

'            Dim processedFiles = 0
'            For Each file In files
'                token.ThrowIfCancellationRequested()
'                Dim fileName As String = Path.GetFileName(file)
'                results.Add(Tuple.Create(fileName, file))
'                processedFiles += 1

'                ' تحديث شريط التقدم
'                Dim progress = CInt((processedFiles / totalFiles) * 100)
'                ExecuteOnUIThread(Sub()
'                                      ProgressBar1.Value = progress
'                                      progressLabel.Text = $"{progress}%"
'                                  End Sub)
'            Next
'        Catch ex As Exception
'            ' تجاهل الأخطاء في مجلد معين والبحث في المجلدات الأخرى
'        End Try
'    End Sub

'    Private Sub UpdateUIWithResults(results As List(Of Tuple(Of String, String)))
'        Try
'            ' تنظيف TreeView والـ ImageList
'            iconsImageList.Images.Clear()
'            resultsTreeView.Nodes.Clear()

'            ' إضافة أيقونات افتراضية للمجلدات والملفات
'            Dim fileIcon As Icon = SystemIcons.Application
'            iconsImageList.Images.Add("file", fileIcon.ToBitmap())

'            ' إضافة الملفات مباشرة إلى الجذر (بدون هيكل المجلدات)
'            For Each result In results
'                Dim fileName = result.Item1
'                Dim filePath = result.Item2

'                ' تحديد نوع الملف
'                Dim fileExtension As String = Path.GetExtension(fileName).ToLower()
'                Dim iconKey As String = "file"

'                ' إضافة الملف مباشرة إلى الجذر
'                Dim fileNode As New TreeNode(fileName)
'                fileNode.Tag = filePath
'                fileNode.ImageKey = iconKey
'                fileNode.SelectedImageKey = iconKey
'                fileNode.ToolTipText = filePath
'                resultsTreeView.Nodes.Add(fileNode)
'            Next

'            ' إذا كانت هناك نتائج، حدد أول ملف
'            If resultsTreeView.Nodes.Count > 0 Then
'                resultsTreeView.SelectedNode = resultsTreeView.Nodes(0)
'                resultsTreeView.SelectedNode.EnsureVisible()

'                ' عرض الملف المحدد
'                Dim filePath As String = resultsTreeView.SelectedNode.Tag.ToString()
'                Dim fileExtension As String = Path.GetExtension(filePath).ToLower()

'                ' تحديث تسمية المسار لعرض المسار الكامل
'                ExecuteOnUIThread(Sub()
'                                      DisplayFilePath(filePath)
'                                  End Sub)

'                ' عرض الملف في WebBrowser إذا كان من النوع المدعوم
'                If fileExtension = ".pdf" OrElse fileExtension = ".html" OrElse fileExtension = ".htm" Then
'                    ExecuteOnUIThread(Sub()
'                                          Try
'                                              pdfViewer.Visible = True
'                                              pdfViewer.BringToFront()
'                                              pdfViewer.Navigate(filePath)
'                                          Catch ex As Exception
'                                              ExecuteOnUIThread(Sub()
'                                                                    DisplayFilePath(filePath)
'                                                                End Sub)
'                                          End Try
'                                      End Sub)
'                End If
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateUIWithResults: {ex.Message}")
'        End Try
'    End Sub

'    'Private Sub UpdateUIWithResults(results As List(Of Tuple(Of String, String)))
'    '    Try
'    '        ' تنظيف TreeView والـ ImageList
'    '        iconsImageList.Images.Clear()
'    '        resultsTreeView.Nodes.Clear()

'    '        ' إضافة أيقونات افتراضية للمجلدات والملفات
'    '        Dim fileIcon As Icon = SystemIcons.Application
'    '        iconsImageList.Images.Add("file", fileIcon.ToBitmap())

'    '        ' إضافة الملفات مباشرة إلى الجذر (بدون هيكل المجلدات)
'    '        For Each result In results
'    '            Dim fileName = result.Item1
'    '            Dim filePath = result.Item2

'    '            ' تحديد نوع الملف
'    '            Dim fileExtension As String = Path.GetExtension(fileName).ToLower()
'    '            Dim iconKey As String = "file"

'    '            ' إضافة الملف مباشرة إلى الجذر
'    '            Dim fileNode As New TreeNode(fileName)
'    '            fileNode.Tag = filePath
'    '            fileNode.ImageKey = iconKey
'    '            fileNode.SelectedImageKey = iconKey
'    '            fileNode.ToolTipText = filePath
'    '            resultsTreeView.Nodes.Add(fileNode)
'    '        Next

'    '        ' إذا كانت هناك نتائج، حدد أول ملف
'    '        If resultsTreeView.Nodes.Count > 0 Then
'    '            resultsTreeView.SelectedNode = resultsTreeView.Nodes(0)
'    '            resultsTreeView.SelectedNode.EnsureVisible()
'    '        End If
'    '    Catch ex As Exception
'    '        System.Diagnostics.Debug.WriteLine($"خطأ في UpdateUIWithResults: {ex.Message}")
'    '    End Try
'    'End Sub

'    Private Sub CompleteSearchUI(Optional message As String = Nothing)
'        isSearchInProgress = False
'        searchTimer.Stop()

'        ' تحديث الرسالة
'        If String.IsNullOrEmpty(message) Then
'            If results.Count > 0 Then
'                TxtMsg.Text = $"تم العثور على {results.Count} ملف/ملفات مطابقة في {searchTimer.Elapsed.TotalSeconds:F2} ثانية"
'            Else
'                TxtMsg.Text = "لم يتم العثور على أي ملفات مطابقة للبحث."
'            End If
'        Else
'            TxtMsg.Text = message
'        End If
'    End Sub
'    ''' <summary>
'    ''' معالجة الضغط على مفتاح Enter في مربع البحث
'    ''' </summary>
'    Private Sub searchTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles searchTextBox.KeyDown
'        If e.KeyCode = Keys.Enter Then
'            ' تأكد من وجود نص للبحث
'            If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'                Return
'            End If

'            ' ابدأ عملية البحث
'            StartSearch()

'            ' تعيين علامة للاستخدام لاحقًا
'            shouldAutoAddAfterSearch = True
'        End If
'    End Sub

'    ' متغير لتحديد ما إذا كان يجب إضافة الملف تلقائيًا بعد البحث
'    Private shouldAutoAddAfterSearch As Boolean = False

'    ' تعديل دالة PerformSearch لإضافة الملف تلقائيًا بعد البحث

'    Private Sub PerformSearch(query As String, token As CancellationToken)
'        Try
'            ' حفظ التحديد الحالي قبل البدء في البحث
'            If Not isUserSelecting Then
'                SaveCurrentSelection()
'            End If

'            ' البحث في مجلد الأرشيف الافتراضي
'            Dim folderToSearch As String = My.Settings.DefaultPathSearch
'            Dim searchResults As New List(Of Tuple(Of String, String))()

'            ' التحقق من وجود المجلد
'            If Not String.IsNullOrEmpty(folderToSearch) AndAlso Directory.Exists(folderToSearch) Then
'                token.ThrowIfCancellationRequested()
'                SearchInFolder(folderToSearch, query, token, searchResults)
'            Else
'                ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'                Dim tempFolder As String = Path.GetTempPath()
'                If Directory.Exists(tempFolder) Then
'                    token.ThrowIfCancellationRequested()
'                    SearchInFolder(tempFolder, query, token, searchResults)
'                End If
'            End If

'            ' تحديث النتائج
'            results = searchResults

'            ' تحديث واجهة المستخدم مع النتائج
'            ExecuteOnUIThread(Sub()
'                                  UpdateUIWithResults(results)
'                                  CompleteSearchUI()

'                                  ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'                                  If Not isUserSelecting Then
'                                      RestoreSelection()

'                                      ' إذا طلب المستخدم الإضافة التلقائية بعد البحث
'                                      If shouldAutoAddAfterSearch Then
'                                          shouldAutoAddAfterSearch = False

'                                          ' تحقق من وجود نتائج
'                                          If resultsTreeView.Nodes.Count > 0 Then
'                                              ' حدد أول نتيجة
'                                              resultsTreeView.SelectedNode = resultsTreeView.Nodes(0)

'                                              ' افتح الملف
'                                              OpenSelectedFile(resultsTreeView.SelectedNode, True)
'                                          End If
'                                      End If
'                                  End If
'                              End Sub)
'        Catch ex As OperationCanceledException
'            ' تم إلغاء البحث
'            ExecuteOnUIThread(Sub()
'                                  CompleteSearchUI("تم إلغاء البحث")
'                              End Sub)
'        Catch ex As Exception
'            ' حدث خطأ
'            ExecuteOnUIThread(Sub()
'                                  CompleteSearchUI($"حدث خطأ: {ex.Message}")
'                              End Sub)
'        End Try
'    End Sub

'    'Private Sub searchTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles searchTextBox.KeyDown

'    '    If e.KeyCode = Keys.Enter Then
'    '        If searchTextBox.Text <> Nothing AndAlso resultsTreeView.Nodes.Count > 0 Then
'    '            resultsTreeView.Select()
'    '            If resultsTreeView.Nodes(0) IsNot Nothing Then
'    '                resultsTreeView.SelectedNode = resultsTreeView.Nodes(0)
'    '                OpenSelectedFile(resultsTreeView.SelectedNode, True)
'    '                ArchiveForm.btnAdd.PerformClick()
'    '            End If
'    '        End If
'    '    End If

'    '    'If e.KeyCode = Keys.Enter Then
'    '    '    StartSearch()
'    '    '    OpenSelectedFile(resultsTreeView.SelectedNode.Tag, True)

'    '    '    e.Handled = True
'    '    'End If
'    'End Sub

'    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
'        If isSearchInProgress AndAlso cts IsNot Nothing Then
'            cts.Cancel()
'        End If
'    End Sub

'    Private Sub resultsTreeView_KeyDown(sender As Object, e As KeyEventArgs) Handles resultsTreeView.KeyDown
'        If e.KeyCode = Keys.Enter AndAlso resultsTreeView.SelectedNode IsNot Nothing Then
'            OpenSelectedFile(resultsTreeView.SelectedNode, True)
'        ElseIf e.KeyCode = Keys.Down Then
'            ' عند الضغط على السهم السفلي
'            If resultsTreeView.SelectedNode IsNot Nothing Then
'                AutoAddToArchiveForm()
'            End If
'        ElseIf e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down Then
'            ' عند التنقل بين الملفات باستخدام الأسهم
'            If resultsTreeView.SelectedNode IsNot Nothing Then
'                ' تحديث تسمية المسار لعرض المسار الكامل
'                Dim filePath As String = resultsTreeView.SelectedNode.Tag.ToString()
'                ExecuteOnUIThread(Sub()
'                                      DisplayFilePath(filePath)
'                                  End Sub)

'                ' عرض الملف في WebBrowser إذا كان من النوع المدعوم
'                Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'                If fileExtension = ".pdf" OrElse fileExtension = ".html" OrElse fileExtension = ".htm" Then
'                    ExecuteOnUIThread(Sub()
'                                          Try
'                                              pdfViewer.Visible = True
'                                              pdfViewer.BringToFront()
'                                              pdfViewer.Navigate(filePath)
'                                          Catch ex As Exception
'                                              ExecuteOnUIThread(Sub()
'                                                                    DisplayFilePath(filePath)
'                                                                End Sub)
'                                          End Try
'                                      End Sub)
'                End If
'            End If
'        End If
'    End Sub

'    'Private Sub resultsTreeView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseDoubleClick
'    '    OpenSelectedFile(e.Node, True)
'    'End Sub

'    'Private Sub resultsTreeView_KeyDown(sender As Object, e As KeyEventArgs) Handles resultsTreeView.KeyDown
'    '    If e.KeyCode = Keys.Enter AndAlso resultsTreeView.SelectedNode IsNot Nothing Then
'    '        OpenSelectedFile(resultsTreeView.SelectedNode, True)
'    '    ElseIf e.KeyCode = Keys.Down Then
'    '        ' عند الضغط على السهم السفلي
'    '        If resultsTreeView.SelectedNode IsNot Nothing Then
'    '            AutoAddToArchiveForm()
'    '            Me.Close()
'    '        End If
'    '    End If
'    'End Sub

'    'Private Sub resultsTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseClick
'    '    Try
'    '        ' تحديد أن المستخدم يختار ملفًا
'    '        isUserSelecting = True

'    '        ' حفظ التحديد الحالي
'    '        SaveCurrentSelection()

'    '        ' إذا كان النقر على زر الماوس الأيسر
'    '        If e.Button = MouseButtons.Left Then
'    '            ' يمكن إضافة أي معالجة إضافية هنا
'    '        End If
'    '    Catch ex As Exception
'    '        System.Diagnostics.Debug.WriteLine($"خطأ في حدث NodeMouseClick: {ex.Message}")
'    '    Finally
'    '        ' إعادة التهيئة بعد فترة قصيرة
'    '        Task.Run(Sub()
'    '                     Threading.Thread.Sleep(300)
'    '                     isUserSelecting = False
'    '                 End Sub)
'    '    End Try
'    'End Sub

'    Private Sub resultsTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseClick
'        Try
'            ' تحديد أن المستخدم يختار ملفًا
'            isUserSelecting = True

'            ' حفظ التحديد الحالي
'            SaveCurrentSelection()

'            ' إذا كان النقر على زر الماوس الأيسر
'            If e.Button = MouseButtons.Left Then
'                ' تحديث التحديد
'                resultsTreeView.SelectedNode = e.Node

'                ' عرض الملف المحدد فورًا
'                If e.Node.Tag IsNot Nothing Then
'                    Dim filePath As String = e.Node.Tag.ToString()

'                    ' التحقق من وجود الملف
'                    If File.Exists(filePath) Then
'                        ' تحديث تسمية المسار لعرض المسار الكامل فورًا
'                        ExecuteOnUIThread(Sub()
'                                              DisplayFilePath(filePath)
'                                          End Sub)

'                        ' عرض الملف في WebBrowser إذا كان من النوع المدعوم
'                        Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'                        If fileExtension = ".pdf" OrElse fileExtension = ".html" OrElse fileExtension = ".htm" Then
'                            ExecuteOnUIThread(Sub()
'                                                  Try
'                                                      pdfViewer.Visible = True
'                                                      pdfViewer.BringToFront()
'                                                      pdfViewer.Navigate(filePath)
'                                                  Catch ex As Exception
'                                                      ExecuteOnUIThread(Sub()
'                                                                            DisplayFilePath(filePath)
'                                                                        End Sub)
'                                                  End Try
'                                              End Sub)
'                        End If
'                    Else
'                        ExecuteOnUIThread(Sub()
'                                              DisplayFilePath(filePath)
'                                          End Sub)
'                    End If
'                End If
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في حدث NodeMouseClick: {ex.Message}")
'        Finally
'            ' إعادة التهيئة بعد فترة قصيرة
'            Task.Run(Sub()
'                         Threading.Thread.Sleep(300)
'                         isUserSelecting = False
'                     End Sub)
'        End Try
'    End Sub

'    Private Sub resultsTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles resultsTreeView.AfterSelect
'        Try
'            ' تعيين أن المستخدم يختار ملفًا
'            isUserSelecting = True

'            ' حفظ التحديد الحالي
'            SaveCurrentSelection()

'            ' عرض الملف المحدد فورًا
'            If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'                Dim filePath As String = resultsTreeView.SelectedNode.Tag.ToString()

'                ' التحقق من وجود الملف
'                If File.Exists(filePath) Then
'                    ' تحديث تسمية المسار لعرض المسار الكامل فورًا
'                    ExecuteOnUIThread(Sub()
'                                          DisplayFilePath(filePath)
'                                      End Sub)

'                    ' عرض الملف في WebBrowser إذا كان من النوع المدعوم
'                    Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'                    If fileExtension = ".pdf" OrElse fileExtension = ".html" OrElse fileExtension = ".htm" Then
'                        ExecuteOnUIThread(Sub()
'                                              Try
'                                                  pdfViewer.Visible = True
'                                                  pdfViewer.BringToFront()
'                                                  pdfViewer.Navigate(filePath)
'                                              Catch ex As Exception
'                                                  ExecuteOnUIThread(Sub()
'                                                                        DisplayFilePath(filePath)
'                                                                    End Sub)
'                                              End Try
'                                          End Sub)
'                    End If
'                Else
'                    ExecuteOnUIThread(Sub()
'                                          DisplayFilePath(filePath)
'                                      End Sub)
'                End If
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في AfterSelect: {ex.Message}")
'        Finally
'            ' إعادة التهيئة بعد فترة قصيرة
'            Task.Run(Sub()
'                         Threading.Thread.Sleep(300)
'                         isUserSelecting = False
'                     End Sub)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' عرض المسار الكامل للملف في LabelFileName
'    ''' </summary>
'    Private Sub DisplayFilePath(filePath As String)
'        ExecuteOnUIThread(Sub()
'                              Try
'                                  If File.Exists(filePath) Then
'                                      LabelFileName.Text = "المسار: " & filePath
'                                      LabelFileName.ForeColor = Color.DarkBlue
'                                  Else
'                                      LabelFileName.Text = "الملف غير موجود: " & filePath
'                                      LabelFileName.ForeColor = Color.Red
'                                  End If
'                              Catch ex As Exception
'                                  LabelFileName.Text = "خطأ في عرض المسار"
'                                  LabelFileName.ForeColor = Color.Red
'                              End Try
'                          End Sub)
'    End Sub

'    'Private Sub resultsTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles resultsTreeView.AfterSelect
'    '    Try
'    '        ' تعيين أن المستخدم يختار ملفًا
'    '        isUserSelecting = True

'    '        ' حفظ التحديد الحالي
'    '        SaveCurrentSelection()

'    '        ' عرض الملف المحدد فورًا
'    '        If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'    '            Dim filePath As String = resultsTreeView.SelectedNode.Tag.ToString()

'    '            ' التحقق من وجود الملف
'    '            If File.Exists(filePath) Then
'    '                ' تحديث تسمية المسار لعرض المسار الكامل
'    '                ExecuteOnUIThread(Sub()
'    '                                      LabelFileName.Text = "المسار: " & filePath
'    '                                      LabelFileName.ForeColor = Color.DarkBlue
'    '                                  End Sub)

'    '                ' عرض الملف في WebBrowser إذا كان من النوع المدعوم
'    '                Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'    '                If fileExtension = ".pdf" OrElse fileExtension = ".html" OrElse fileExtension = ".htm" Then
'    '                    ExecuteOnUIThread(Sub()
'    '                                          Try
'    '                                              pdfViewer.Visible = True
'    '                                              pdfViewer.BringToFront()
'    '                                              pdfViewer.Navigate(filePath)
'    '                                          Catch ex As Exception
'    '                                              ExecuteOnUIThread(Sub()
'    '                                                                    LabelFileName.Text = "فشل في عرض الملف: " & ex.Message
'    '                                                                    LabelFileName.ForeColor = Color.Red
'    '                                                                End Sub)
'    '                                          End Try
'    '                                      End Sub)
'    '                End If
'    '            Else
'    '                ExecuteOnUIThread(Sub()
'    '                                      LabelFileName.Text = "الملف غير موجود: " & filePath
'    '                                      LabelFileName.ForeColor = Color.Red
'    '                                  End Sub)
'    '            End If
'    '        End If
'    '    Catch ex As Exception
'    '        System.Diagnostics.Debug.WriteLine($"خطأ في AfterSelect: {ex.Message}")
'    '    Finally
'    '        ' إعادة التهيئة بعد فترة قصيرة
'    '        Task.Run(Sub()
'    '                     Threading.Thread.Sleep(300)
'    '                     isUserSelecting = False
'    '                 End Sub)
'    '    End Try
'    'End Sub

'    Private Sub resultsTreeView_Leave(sender As Object, e As EventArgs) Handles resultsTreeView.Leave
'        Try
'            ' إعادة تهيئة بعد فترة قصيرة
'            Task.Run(Sub()
'                         Threading.Thread.Sleep(300)
'                         isUserSelecting = False
'                     End Sub)
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في Leave: {ex.Message}")
'        End Try
'    End Sub

'    Private Sub SetBrowserEmulationMode()
'        Try
'            ' تحديد مفتاح التسجيل لـ FEATURE_BROWSER_EMULATION
'            Dim browserEmulationKey As Microsoft.Win32.RegistryKey = Nothing

'            Try
'                ' فتح مفتاح التسجيل
'                browserEmulationKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
'                "Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
'                True)

'                If browserEmulationKey IsNot Nothing Then
'                    ' الحصول على اسم التطبيق
'                    Dim appName As String = System.IO.Path.GetFileName(Application.ExecutablePath)

'                    ' تحديد الإصدار (11001 = IE11)
'                    browserEmulationKey.SetValue(appName, 11001, Microsoft.Win32.RegistryValueKind.DWord)

'                    System.Diagnostics.Debug.WriteLine($"تم تعيين إصدار WebBrowser لـ {appName} إلى IE11")
'                End If
'            Catch ex As Exception
'                System.Diagnostics.Debug.WriteLine($"خطأ في تعيين إصدار WebBrowser: {ex.Message}")
'            Finally
'                ' إغلاق مفتاح التسجيل
'                If browserEmulationKey IsNot Nothing Then
'                    browserEmulationKey.Close()
'                End If
'            End Try
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ عام في SetBrowserEmulationMode: {ex.Message}")
'        End Try
'    End Sub

'    Private Sub OpenSelectedFile(selectedNode As TreeNode, isDoubleClick As Boolean)
'        ' التحقق من أن العقدة و Tag ليست Nothing
'        If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then
'            Return
'        End If

'        Dim filePath As String = selectedNode.Tag.ToString()

'        ' التحقق من وجود الملف
'        If Not File.Exists(filePath) Then
'            ExecuteOnUIThread(Sub()
'                                  LabelFileName.Text = "الملف غير موجود: " & filePath
'                                  LabelFileName.ForeColor = Color.Red
'                              End Sub)
'            MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Return
'        End If

'        Try
'            ' الحصول على امتداد الملف
'            Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'            Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)

'            ' تحديث تسمية عرض الملف
'            ExecuteOnUIThread(Sub()
'                                  LabelFileName.Text = "المسار: " & filePath
'                                  LabelFileName.ForeColor = Color.DarkBlue
'                              End Sub)

'            ' فتح الملف بناءً على نوعه
'            Select Case fileExtension
'                Case ".pdf", ".html", ".htm"
'                    ' عرض ملف PDF أو HTML داخل WebBrowser
'                    ExecuteOnUIThread(Sub()
'                                          Try
'                                              ' جعل العنصر مرئيًا
'                                              pdfViewer.Visible = True
'                                              pdfViewer.BringToFront()

'                                              ' التأكد من أن المسار آمن لـ WebBrowser
'                                              Dim safePath As String = filePath.Replace("\", "/")
'                                              If Not safePath.StartsWith("file://") Then
'                                                  safePath = "file:///" & safePath
'                                              End If

'                                              ' محاولة فتح الملف
'                                              Try
'                                                  Dim uri As New Uri(filePath)
'                                                  pdfViewer.Navigate(uri)
'                                              Catch uriEx As Exception
'                                                  Try
'                                                      pdfViewer.Navigate("file:///" & filePath.Replace("\", "/"))
'                                                  Catch ex2 As Exception
'                                                      ' إذا فشلت جميع المحاولات، أظهر رسالة خطأ
'                                                      ExecuteOnUIThread(Sub()
'                                                                            LabelFileName.Text = "فشل في عرض الملف: " & ex2.Message
'                                                                            LabelFileName.ForeColor = Color.Red
'                                                                        End Sub)
'                                                      MessageBox.Show("فشل في فتح الملف في WebBrowser: " & ex2.Message &
'                                                                 vbNewLine & "قد تحتاج إلى تثبيت Adobe Reader أو استخدام متصفح ويب خارجي.",
'                                                                 "خطأ في عرض الملف", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                                                  End Try
'                                              End Try
'                                          Catch ex As Exception
'                                              ExecuteOnUIThread(Sub()
'                                                                    LabelFileName.Text = "خطأ في عرض الملف: " & ex.Message
'                                                                    LabelFileName.ForeColor = Color.Red
'                                                                End Sub)
'                                          End Try
'                                      End Sub)

'                    ' إذا كان هذا نقرة مزدوجة أو ضغط Enter، قم بإرسال الإشارة
'                    If isDoubleClick Then
'                        ' حفظ معلومات الملف المختار
'                        SelectedFilePath = filePath
'                        SelectedFileName = fileName
'                        SelectedFileExtension = fileExtension

'                        ' إرسال إشارة لجميع النماذج المستمعة
'                        ExecuteOnUIThread(Sub()
'                                              RaiseEvent DataSelected(filePath, fileName, fileExtension)
'                                              ' رفع حدث اختيار ملف للإضافة إلى الأرشيف
'                                              DataHelper.RaiseFileSelectedForArchive(filePath, fileName, fileExtension)
'                                          End Sub)

'                        ' إغلاق النموذج
'                        Me.Close()
'                    End If
'                Case Else
'                    ' فتح الملف باستخدام التطبيق الافتراضي
'                    ExecuteOnUIThread(Sub()
'                                          Try
'                                              pdfViewer.Visible = False
'                                              Process.Start(filePath)
'                                          Catch ex As Exception
'                                              ExecuteOnUIThread(Sub()
'                                                                    LabelFileName.Text = "غير قادر على فتح الملف: " & ex.Message
'                                                                    LabelFileName.ForeColor = Color.Red
'                                                                End Sub)
'                                              MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                                          End Try
'                                      End Sub)
'            End Select
'        Catch ex As Exception
'            ' التعامل مع الأخطاء
'            ExecuteOnUIThread(Sub()
'                                  LabelFileName.Text = "غير قادر على فتح الملف: " & ex.Message
'                                  LabelFileName.ForeColor = Color.Red
'                              End Sub)
'            MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub



'    'Private Sub OpenSelectedFile(selectedNode As TreeNode, isDoubleClick As Boolean)
'    '    ' التحقق من أن العقدة و Tag ليست Nothing
'    '    If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then
'    '        Return
'    '    End If

'    '    Dim filePath As String = selectedNode.Tag.ToString()

'    '    ' التحقق من وجود الملف
'    '    If Not File.Exists(filePath) Then
'    '        ExecuteOnUIThread(Sub()
'    '                              DisplayFilePath(filePath)
'    '                          End Sub)
'    '        MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '        Return
'    '    End If

'    '    Try
'    '        ' الحصول على امتداد الملف
'    '        Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'    '        Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)

'    '        ' تحديث تسمية عرض الملف لعرض المسار الكامل فورًا
'    '        ExecuteOnUIThread(Sub()
'    '                              DisplayFilePath(filePath)
'    '                          End Sub)

'    '        ' فتح الملف بناءً على نوعه
'    '        Select Case fileExtension
'    '            Case ".pdf", ".html", ".htm"
'    '                ' عرض ملف PDF أو HTML داخل WebBrowser
'    '                ExecuteOnUIThread(Sub()
'    '                                      Try
'    '                                          ' جعل العنصر مرئيًا
'    '                                          pdfViewer.Visible = True
'    '                                          pdfViewer.BringToFront()

'    '                                          ' التأكد من أن المسار آمن لـ WebBrowser
'    '                                          Dim safePath As String = filePath.Replace("\", "/")
'    '                                          If Not safePath.StartsWith("file://") Then
'    '                                              safePath = "file:///" & safePath
'    '                                          End If

'    '                                          ' محاولة فتح الملف
'    '                                          Try
'    '                                              Dim uri As New Uri(filePath)
'    '                                              pdfViewer.Navigate(uri)
'    '                                          Catch uriEx As Exception
'    '                                              Try
'    '                                                  pdfViewer.Navigate("file:///" & filePath.Replace("\", "/"))
'    '                                              Catch ex2 As Exception
'    '                                                  ' إذا فشلت جميع المحاولات، أظهر رسالة خطأ
'    '                                                  ExecuteOnUIThread(Sub()
'    '                                                                        DisplayFilePath(filePath)
'    '                                                                    End Sub)
'    '                                                  MessageBox.Show("فشل في فتح الملف في WebBrowser: " & ex2.Message &
'    '                                                             vbNewLine & "قد تحتاج إلى تثبيت Adobe Reader أو استخدام متصفح ويب خارجي.",
'    '                                                             "خطأ في عرض الملف", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '                                              End Try
'    '                                          End Try
'    '                                      Catch ex As Exception
'    '                                          ExecuteOnUIThread(Sub()
'    '                                                                DisplayFilePath(filePath)
'    '                                                            End Sub)
'    '                                      End Try
'    '                                  End Sub)

'    '                ' إذا كان هذا نقرة مزدوجة أو ضغط Enter، قم بإرسال الإشارة
'    '                If isDoubleClick Then
'    '                    ' حفظ معلومات الملف المختار
'    '                    SelectedFilePath = filePath
'    '                    SelectedFileName = fileName
'    '                    SelectedFileExtension = fileExtension

'    '                    ' إرسال إشارة لجميع النماذج المستمعة
'    '                    ExecuteOnUIThread(Sub()
'    '                                          RaiseEvent DataSelected(filePath, fileName, fileExtension)
'    '                                      End Sub)

'    '                    ' إغلاق النموذج
'    '                    Me.Close()
'    '                End If
'    '            Case Else
'    '                ' فتح الملف باستخدام التطبيق الافتراضي
'    '                ExecuteOnUIThread(Sub()
'    '                                      Try
'    '                                          pdfViewer.Visible = False
'    '                                          Process.Start(filePath)
'    '                                      Catch ex As Exception
'    '                                          ExecuteOnUIThread(Sub()
'    '                                                                DisplayFilePath(filePath)
'    '                                                            End Sub)
'    '                                          MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '                                      End Try
'    '                                  End Sub)
'    '        End Select
'    '    Catch ex As Exception
'    '        ' التعامل مع الأخطاء
'    '        ExecuteOnUIThread(Sub()
'    '                              DisplayFilePath(filePath)
'    '                          End Sub)
'    '        MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '    End Try
'    'End Sub


'    'Private Sub OpenSelectedFile(selectedNode As TreeNode, isDoubleClick As Boolean)
'    '    ' التحقق من أن العقدة و Tag ليست Nothing
'    '    If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then
'    '        Return
'    '    End If

'    '    Dim filePath As String = selectedNode.Tag.ToString()

'    '    ' التحقق من وجود الملف
'    '    If Not File.Exists(filePath) Then
'    '        ExecuteOnUIThread(Sub()
'    '                              LabelFileName.Text = "الملف غير موجود: " & Path.GetFileName(filePath)
'    '                              LabelFileName.ForeColor = Color.Red
'    '                          End Sub)
'    '        MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '        Return
'    '    End If

'    '    Try
'    '        ' الحصول على امتداد الملف
'    '        Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'    '        Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)
'    '        ' تحديث تسمية عرض الملف فورًا
'    '        UpdateFileNameLabel(filePath)


'    '        ' تحديث تسمية عرض الملف
'    '        ExecuteOnUIThread(Sub()
'    '                              LabelFileName.Text = "الملف المحدد: " & fileName & fileExtension
'    '                              LabelFileName.ForeColor = Color.DarkBlue
'    '                          End Sub)

'    '        ' فتح الملف بناءً على نوعه

'    '        Select Case fileExtension
'    '                    Case ".pdf", ".html", ".htm"
'    '                        ' عرض ملف PDF أو HTML داخل WebBrowser
'    '                        ExecuteOnUIThread(Sub()
'    '                                              Try
'    '                                                  ' جعل العنصر مرئيًا
'    '                                                  pdfViewer.Visible = True
'    '                                                  pdfViewer.BringToFront()

'    '                                                  ' تحديث تسمية الملف فورًا
'    '                                                  ExecuteOnUIThread(Sub()
'    '                                                                        LabelFileName.Text = "جاري تحميل: " & Path.GetFileName(filePath)
'    '                                                                        LabelFileName.ForeColor = Color.Blue
'    '                                                                    End Sub)

'    '                                                  ' التأكد من أن المسار آمن لـ WebBrowser
'    '                                                  Dim safePath As String = filePath.Replace("\", "/")
'    '                                                  If Not safePath.StartsWith("file://") Then
'    '                                                      safePath = "file:///" & safePath
'    '                                                  End If

'    '                                                  ' محاولة فتح الملف
'    '                                                  Try
'    '                                                      Dim uri As New Uri(filePath)
'    '                                                      pdfViewer.Navigate(uri)
'    '                                                  Catch uriEx As Exception
'    '                                                      Try
'    '                                                          pdfViewer.Navigate("file:///" & filePath.Replace("\", "/"))
'    '                                                      Catch ex2 As Exception
'    '                                                          ' إذا فشلت جميع المحاولات، أظهر رسالة خطأ
'    '                                                          ExecuteOnUIThread(Sub()
'    '                                                                                LabelFileName.Text = "فشل في عرض الملف: " & ex2.Message
'    '                                                                                LabelFileName.ForeColor = Color.Red
'    '                                                                            End Sub)
'    '                                                          MessageBox.Show("فشل في فتح الملف في WebBrowser: " & ex2.Message &
'    '                                                                         vbNewLine & "قد تحتاج إلى تثبيت Adobe Reader أو استخدام متصفح ويب خارجي.",
'    '                                                                         "خطأ في عرض الملف", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '                                                      End Try
'    '                                                  End Try
'    '                                              Catch ex As Exception
'    '                                                  ExecuteOnUIThread(Sub()
'    '                                                                        LabelFileName.Text = "خطأ في عرض الملف: " & ex.Message
'    '                                                                        LabelFileName.ForeColor = Color.Red
'    '                                                                    End Sub)
'    '                                              End Try
'    '                                          End Sub)


'    '                        'ExecuteOnUIThread(Sub()
'    '                        '                      Try
'    '                        '                          ' جعل العنصر مرئيًا
'    '                        '                          pdfViewer.Visible = True
'    '                        '                          pdfViewer.BringToFront()

'    '                        '                          ' التأكد من أن المسار آمن لـ WebBrowser
'    '                        '                          Dim safePath As String = filePath.Replace("\", "/")
'    '                        '                          If Not safePath.StartsWith("file://") Then
'    '                        '                              safePath = "file:///" & safePath
'    '                        '                          End If

'    '                        '                          ' محاولة فتح الملف
'    '                        '                          Try
'    '                        '                              Dim uri As New Uri(filePath)
'    '                        '                              pdfViewer.Navigate(uri)
'    '                        '                          Catch uriEx As Exception
'    '                        '                              Try
'    '                        '                                  pdfViewer.Navigate("file:///" & filePath.Replace("\", "/"))
'    '                        '                              Catch ex2 As Exception
'    '                        '                                  ' إذا فشلت جميع المحاولات، أظهر رسالة خطأ
'    '                        '                                  ExecuteOnUIThread(Sub()
'    '                        '                                                        LabelFileName.Text = "فشل في عرض الملف: " & ex2.Message
'    '                        '                                                        LabelFileName.ForeColor = Color.Red
'    '                        '                                                    End Sub)
'    '                        '                                  MessageBox.Show("فشل في فتح الملف في WebBrowser: " & ex2.Message &
'    '                        '                                             vbNewLine & "قد تحتاج إلى تثبيت Adobe Reader أو استخدام متصفح ويب خارجي.",
'    '                        '                                             "خطأ في عرض الملف", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '                        '                              End Try
'    '                        '                          End Try
'    '                        '                      Catch ex As Exception
'    '                        '                          ExecuteOnUIThread(Sub()
'    '                        '                                                LabelFileName.Text = "خطأ في عرض الملف: " & ex.Message
'    '                        '                                                LabelFileName.ForeColor = Color.Red
'    '                        '                                            End Sub)
'    '                        '                      End Try
'    '                        '                  End Sub)

'    '                        ' إذا كان هذا نقرة مزدوجة أو ضغط Enter، قم بإرسال الإشارة
'    '                        If isDoubleClick Then
'    '                    ' حفظ معلومات الملف المختار
'    '                    SelectedFilePath = filePath
'    '                    SelectedFileName = fileName
'    '                    SelectedFileExtension = fileExtension

'    '                    ' إرسال إشارة لجميع النماذج المستمعة
'    '                    ExecuteOnUIThread(Sub()
'    '                                          RaiseEvent DataSelected(filePath, fileName, fileExtension)
'    '                                      End Sub)

'    '                    ' إغلاق النموذج
'    '                    'Me.Close()
'    '                End If
'    '            Case Else
'    '                ' فتح الملف باستخدام التطبيق الافتراضي
'    '                ExecuteOnUIThread(Sub()
'    '                                      Try
'    '                                          pdfViewer.Visible = False
'    '                                          Process.Start(filePath)
'    '                                      Catch ex As Exception
'    '                                          ExecuteOnUIThread(Sub()
'    '                                                                LabelFileName.Text = "غير قادر على فتح الملف: " & ex.Message
'    '                                                                LabelFileName.ForeColor = Color.Red
'    '                                                            End Sub)
'    '                                          MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '                                      End Try
'    '                                  End Sub)
'    '        End Select
'    '    Catch ex As Exception
'    '        ' التعامل مع الأخطاء
'    '        ExecuteOnUIThread(Sub()
'    '                              LabelFileName.Text = "غير قادر على فتح الملف: " & ex.Message
'    '                              LabelFileName.ForeColor = Color.Red
'    '                          End Sub)
'    '        MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '    End Try
'    'End Sub

'    Private Sub UpdateFileNameLabel(filePath As String)
'        ExecuteOnUIThread(Sub()
'                              Try
'                                  If File.Exists(filePath) Then
'                                      Dim fileName = Path.GetFileName(filePath)
'                                      Dim fileExtension = Path.GetExtension(filePath).ToLower()
'                                      LabelFileName.Text = "الملف المحدد: " & fileName
'                                      LabelFileName.ForeColor = Color.DarkBlue
'                                  Else
'                                      LabelFileName.Text = "الملف غير موجود: " & Path.GetFileName(filePath)
'                                      LabelFileName.ForeColor = Color.Red
'                                  End If
'                              Catch ex As Exception
'                                  LabelFileName.Text = "خطأ في عرض اسم الملف"
'                                  LabelFileName.ForeColor = Color.Red
'                              End Try
'                          End Sub)
'    End Sub

'    ''' <summary>
'    ''' تنفيذ الإجراء في الخيط الصحيح (الرئيسي)
'    ''' </summary>
'    Private Sub ExecuteOnUIThread(action As Action)
'        Try
'            If Me.InvokeRequired Then
'                Me.Invoke(action)
'            Else
'                action()
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في ExecuteOnUIThread: {ex.Message}")
'        End Try
'    End Sub

'    ' تم إزالة المتغير غير المستخدم من الدالة التالية
'    Private Sub OpenFileInWebBrowser(filePath As String)
'        Try
'            ' الحصول على امتداد الملف
'            Dim fileExtension As String = Path.GetExtension(filePath).ToLower()

'            ' فتح الملف بناءً على نوعه
'            Select Case fileExtension
'                Case ".pdf", ".html", ".htm"
'                    ' عرض ملف PDF أو HTML داخل WebBrowser
'                    ExecuteOnUIThread(Sub()
'                                          Try
'                                              ' جعل العنصر مرئيًا
'                                              pdfViewer.Visible = True
'                                              pdfViewer.BringToFront()

'                                              ' التحقق من أن المسار آمن لـ WebBrowser
'                                              Dim safePath As String = filePath.Replace("\", "/")
'                                              If Not safePath.StartsWith("file://") Then
'                                                  safePath = "file:///" & safePath
'                                              End If

'                                              ' محاولة فتح الملف
'                                              Try
'                                                  Dim uri As New Uri(filePath)
'                                                  pdfViewer.Navigate(uri)
'                                              Catch uriEx As Exception
'                                                  Try
'                                                      pdfViewer.Navigate("file:///" & filePath.Replace("\", "/"))
'                                                  Catch ex2 As Exception
'                                                      ' إذا فشلت جميع المحاولات، أظهر رسالة خطأ
'                                                      DisplayFilePath(filePath)
'                                                      MessageBox.Show("فشل في فتح الملف في WebBrowser: " & ex2.Message &
'                                                                 vbNewLine & "قد تحتاج إلى تثبيت Adobe Reader أو استخدام متصفح ويب خارجي.",
'                                                                 "خطأ في عرض الملف", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                                                  End Try
'                                              End Try
'                                          Catch ex As Exception
'                                              DisplayFilePath(filePath)
'                                          End Try
'                                      End Sub)
'                Case Else
'                    ' فتح الملف باستخدام التطبيق الافتراضي
'                    ExecuteOnUIThread(Sub()
'                                          Try
'                                              Process.Start(filePath)
'                                          Catch ex As Exception
'                                              DisplayFilePath(filePath)
'                                          End Try
'                                      End Sub)
'            End Select
'        Catch ex As Exception
'            ExecuteOnUIThread(Sub()
'                                  DisplayFilePath(filePath)
'                              End Sub)
'        End Try
'    End Sub

'    Private Sub pdfViewer_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles pdfViewer.DocumentCompleted
'        Try
'            ' التحقق من حالة تحميل المستند بشكل أكثر ذكاءً
'            If pdfViewer.ReadyState = WebBrowserReadyState.Complete Then
'                ' تأكد من أننا نتعامل مع المستند الصحيح
'                If e.Url = pdfViewer.Url Then
'                    ExecuteOnUIThread(Sub()
'                                          ' تحديث تسمية الملف بناءً على المسار الحالي
'                                          If Not String.IsNullOrEmpty(pdfViewer.Url?.ToString()) Then
'                                              Dim fileName = Path.GetFileName(pdfViewer.Url.LocalPath)
'                                              DisplayFilePath(resultsTreeView.SelectedNode.Tag)
'                                          End If
'                                      End Sub)
'                End If
'            Else
'                ' لا تظهر رسالة خطأ مبكرة، انتظر قليلاً
'                System.Diagnostics.Debug.WriteLine($"حالة التحميل: {pdfViewer.ReadyState}")
'            End If
'        Catch ex As Exception
'            ExecuteOnUIThread(Sub()
'                                  DisplayFilePath(resultsTreeView.SelectedNode.Tag)
'                              End Sub)
'        End Try
'    End Sub

'    'Private Sub pdfViewer_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles pdfViewer.DocumentCompleted
'    '    Try
'    '        ' التحقق من حالة تحميل المستند
'    '        If pdfViewer.ReadyState = WebBrowserReadyState.Complete Then
'    '            ' يمكن إضافة أي معالجة إضافية هنا
'    '        Else
'    '            ExecuteOnUIThread(Sub()
'    '                                  LabelFileName.Text = "فشل في تحميل الملف بالكامل"
'    '                                  LabelFileName.ForeColor = Color.Red
'    '                              End Sub)
'    '        End If
'    '    Catch ex As Exception
'    '        ExecuteOnUIThread(Sub()
'    '                              LabelFileName.Text = "خطأ في تحميل المستند: " & ex.Message
'    '                              LabelFileName.ForeColor = Color.Red
'    '                          End Sub)
'    '    End Try
'    'End Sub

'    'Private Sub pdfViewer_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles pdfViewer.Navigating
'    '    Try
'    '        ' تحديث تسمية الملف أثناء التنقل
'    '        ExecuteOnUIThread(Sub()
'    '                              LabelFileName.Text = "جاري تحميل: " & Path.GetFileName(e.Url.ToString())
'    '                              LabelFileName.ForeColor = Color.Blue
'    '                          End Sub)
'    '    Catch ex As Exception
'    '        ' تجاهل الأخطاء
'    '    End Try
'    'End Sub


'    'Private Sub OpenSelectedFile(selectedNode As TreeNode, isDoubleClick As Boolean)
'    '    ' التحقق من أن العقدة و Tag ليست Nothing
'    '    If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then
'    '        Return
'    '    End If

'    '    Dim filePath As String = selectedNode.Tag.ToString()

'    '    ' التحقق من وجود الملف
'    '    If Not File.Exists(filePath) Then
'    '        MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '        Return
'    '    End If

'    '    Try
'    '        ' الحصول على امتداد الملف
'    '        Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'    '        Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)

'    '        ' فتح الملف بناءً على نوعه
'    '        Select Case fileExtension
'    '            Case ".pdf", ".html", ".htm"
'    '                ' عرض ملف PDF أو HTML داخل WebBrowser
'    '                ExecuteOnUIThread(Sub()
'    '                                      pdfViewer.Visible = True
'    '                                      pdfViewer.Navigate(filePath)
'    '                                      txtOriginalPath.Text = filePath
'    '                                  End Sub)

'    '                ' إذا كان هذا نقرة مزدوجة أو ضغط Enter، قم بإرسال الإشارة
'    '                If isDoubleClick Then
'    '                    ' حفظ معلومات الملف المختار
'    '                    SelectedFilePath = filePath
'    '                    SelectedFileName = fileName
'    '                    SelectedFileExtension = fileExtension

'    '                    ' إرسال إشارة لجميع النماذج المستمعة
'    '                    ExecuteOnUIThread(Sub()
'    '                                          RaiseEvent DataSelected(filePath, fileName, fileExtension)
'    '                                      End Sub)

'    '                    ' تحديث نموذج الأرشيف إذا كان مفتوحًا
'    '                    UpdateArchiveForm(filePath, fileName, fileExtension)

'    '                    ' إغلاق النموذج
'    '                    Me.Close()
'    '                End If
'    '            Case Else
'    '                ' فتح الملف باستخدام التطبيق الافتراضي
'    '                ExecuteOnUIThread(Sub()
'    '                                      pdfViewer.Visible = False
'    '                                      Process.Start(filePath)
'    '                                  End Sub)
'    '        End Select
'    '    Catch ex As Exception
'    '        ' التعامل مع الأخطاء
'    '        MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '    End Try
'    'End Sub

'    Private Sub pdfViewer_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles pdfViewer.Navigating
'        Try
'            ' التحقق من وجود ملف محدد في شجرة العرض
'            If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'                Dim filePath As String = resultsTreeView.SelectedNode.Tag.ToString()

'                ' التحقق من وجود الملف
'                If File.Exists(filePath) Then
'                    ExecuteOnUIThread(Sub()
'                                          ' عرض المسار الكامل دائمًا
'                                          DisplayFilePath(filePath)
'                                      End Sub)
'                End If
'            End If
'        Catch ex As Exception
'            ' تجاهل الأخطاء
'        End Try
'    End Sub

'    Private Sub DelayTimer_Tick(sender As Object, e As EventArgs) Handles DelayTimer.Tick
'        ' إيقاف Timer بعد انتهاء الوقت
'        DelayTimer.Stop()

'        ' التحقق من قيمة Tag
'        If Not (Me.Tag = 1 OrElse Me.Tag = 2) Then Return

'        ' التحقق من صحة النص المدخل
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            Dim errorMsg = If(Me.Tag = 1, "لم يتم العثور على ملف, الرجاء إدخال قيمة صحيحة", "Please enter a search term.")
'            ExecuteOnUIThread(Sub()
'                                  TxtMsg.Text = errorMsg
'                              End Sub)
'            Return
'        End If

'        ' تهيئة العناصر
'        ExecuteOnUIThread(Sub()
'                              InitializeSearchUI()
'                          End Sub)

'        ' إعداد مصدر الإلغاء
'        cts = New Threading.CancellationTokenSource()
'        Dim token = cts.Token

'        Try
'            ' استخدام Task.Run بدون .Result لتجنب تجميد الواجهة
'            Task.Run(Sub()
'                         Try
'                             ' التحقق من أن المستخدم لا يختار ملفًا حاليًا
'                             If Not isUserSelecting Then
'                                 PerformSearch(searchTextBox.Text, token)
'                             Else
'                                 ' إذا كان المستخدم يختار ملفًا، أعد جدولة البحث
'                                 ExecuteOnUIThread(Sub()
'                                                       DelayTimer.Stop()
'                                                       DelayTimer.Start()
'                                                   End Sub)
'                             End If
'                         Catch ex As Exception
'                             System.Diagnostics.Debug.WriteLine($"خطأ في Task.Run: {ex.Message}")
'                         End Try
'                     End Sub)
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في DelayTimer_Tick: {ex.Message}")
'        End Try
'    End Sub

'    ' دالة لإعادة ضبط واجهة المستخدم بعد انتهاء البحث
'    Private Sub ResetUI()
'        btnCancel.Enabled = False
'        ProgressBar1.Visible = False
'        progressLabel.Visible = False
'    End Sub

'    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            ExecuteOnUIThread(Sub()
'                                  resultsTreeView.Nodes.Clear()
'                                  TxtMsg.Text = "لم يتم العثور على ملف , الرجاء إدخال قيمة صحيحة في مربع البحث"
'                              End Sub)
'        Else
'            ' إعادة تشغيل Timer عند تغيير النص
'            ExecuteOnUIThread(Sub()
'                                  DelayTimer.Stop()
'                                  DelayTimer.Start()
'                                  TxtMsg.Text = Nothing
'                              End Sub)
'        End If
'    End Sub

'    Private Sub CustomSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load

'        ' تعيين إصدار WebBrowser
'        SetBrowserEmulationMode()

'        ' إعدادات Timer
'        DelayTimer.Interval = delayTime

'        ' تحميل المجلد الافتراضي
'        InitializeDefaultFolder()

'        ' تأكد من أن WebBrowser مرئي
'        ExecuteOnUIThread(Sub()
'                              pdfViewer.Visible = True
'                              pdfViewer.BringToFront()
'                          End Sub)
'    End Sub

'    Private Sub InitializeDefaultFolder()
'        ' تحديد المجلد الافتراضي للبحث
'        Dim defaultFolder As String = My.Settings.DefaultPathSearch

'        ' التأكد من وجود المجلد
'        If Not String.IsNullOrEmpty(defaultFolder) AndAlso Directory.Exists(defaultFolder) Then
'            ' يمكن إضافة شرح للمجلد الافتراضي إذا لزم
'        Else
'            ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'            defaultFolder = Path.GetTempPath()
'        End If
'    End Sub

'    Private Sub CustomSearch_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
'        ' إلغاء المهمة الجارية إذا كانت موجودة
'        If isSearchInProgress AndAlso cts IsNot Nothing Then
'            cts.Cancel()
'        End If
'    End Sub

'    ''' <summary>
'    ''' خاصية لتخزين مسار الملف المختار
'    ''' </summary>
'    Public Property SelectedFilePath As String = String.Empty
'    Public Property SelectedFileName As String = String.Empty
'    Public Property SelectedFileExtension As String = String.Empty

'#Region "الوظائف الإضافية"


'    ''' <summary>
'    ''' حفظ حالة التحديد الحالية
'    ''' </summary>
'    Private Sub SaveCurrentSelection()
'        ExecuteOnUIThread(Sub()
'                              If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'                                  selectedNodeTag = resultsTreeView.SelectedNode.Tag.ToString()
'                              Else
'                                  selectedNodeTag = String.Empty
'                              End If
'                          End Sub)
'    End Sub

'    ''' <summary>
'    ''' استعادة التحديد السابق بعد تحديث النتائج
'    ''' </summary>
'    Private Sub RestoreSelection()
'        Try
'            If String.IsNullOrEmpty(selectedNodeTag) Then Return

'            ' البحث عن العقدة باستخدام التاغ
'            Dim node As TreeNode = FindNodeByTag(resultsTreeView.Nodes, selectedNodeTag)
'            If node IsNot Nothing Then
'                ' اختيار العقدة وجعلها مرئية
'                ExecuteOnUIThread(Sub()
'                                      resultsTreeView.SelectedNode = node
'                                      node.EnsureVisible()
'                                  End Sub)
'            Else
'                ' إذا لم يتم العثور على العقدة، حاول البحث باستخدام مسارات بديلة
'                For Each currentNode As TreeNode In GetAllNodes(resultsTreeView.Nodes)
'                    If currentNode.Tag IsNot Nothing AndAlso
'                   currentNode.Tag.ToString().EndsWith(selectedNodeTag, StringComparison.OrdinalIgnoreCase) Then
'                        ExecuteOnUIThread(Sub()
'                                              resultsTreeView.SelectedNode = currentNode
'                                              currentNode.EnsureVisible()
'                                          End Sub)
'                        Exit For
'                    End If
'                Next
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في استعادة التحديد: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' البحث عن عقدة في TreeView باستخدام Tag
'    ''' </summary>
'    Private Function FindNodeByTag(nodes As TreeNodeCollection, tag As String) As TreeNode
'        ' تأكد من أن التاغ ليس فارغًا
'        If String.IsNullOrEmpty(tag) Then
'            Return Nothing
'        End If

'        For Each node As TreeNode In nodes
'            ' تحقق من وجود Tag ومقارنته
'            If node.Tag IsNot Nothing AndAlso
'               String.Equals(node.Tag.ToString(), tag, StringComparison.OrdinalIgnoreCase) Then
'                Return node
'            End If
'        Next
'        Return Nothing
'    End Function

'    ''' <summary>
'    ''' الحصول على جميع العقد في TreeView
'    ''' </summary>
'    Private Function GetAllNodes(nodes As TreeNodeCollection) As List(Of TreeNode)
'        Dim allNodes As New List(Of TreeNode)()

'        For Each node As TreeNode In nodes
'            allNodes.Add(node)
'        Next

'        Return allNodes
'    End Function

'    ''' <summary>
'    '''' توسيع جميع العقد الأصلية للعقدة المحددة
'    '''' </summary>
'    Private Sub ExpandParentNodes(node As TreeNode)
'        Dim currentNode As TreeNode = node
'        While currentNode.Parent IsNot Nothing
'            currentNode.Parent.Expand()
'            currentNode = currentNode.Parent
'        End While
'    End Sub

'    ''' <summary>
'    ''' الدالة الجديدة لتحديد أول نتيجة تلقائيًا وعرضها
'    ''' </summary>
'    Private Sub AutoSelectFirstResult()
'        ExecuteOnUIThread(Sub()
'                              Try
'                                  ' التحقق من وجود نتائج
'                                  If results.Count > 0 Then
'                                      Dim firstResult As Tuple(Of String, String) = results(0)
'                                      Dim filePath As String = firstResult.Item2

'                                      ' البحث عن العقدة في TreeView
'                                      Dim targetNode As TreeNode = FindNodeByTag(resultsTreeView.Nodes, filePath)
'                                      If targetNode IsNot Nothing Then
'                                          ' توسيع جميع العقد الأصلية
'                                          ExpandParentNodes(targetNode)

'                                          ' اختيار العقدة
'                                          resultsTreeView.SelectedNode = targetNode
'                                          targetNode.EnsureVisible()

'                                          ' حفظ التحديد الحالي
'                                          selectedNodeTag = filePath
'                                          isUserSelecting = False

'                                          ' تحديث تسمية المسار لعرض المسار الكامل
'                                          ExecuteOnUIThread(Sub()
'                                                                DisplayFilePath(filePath)
'                                                            End Sub)

'                                          ' عرض الملف في WebBrowser
'                                          Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'                                          If fileExtension = ".pdf" OrElse fileExtension = ".html" OrElse fileExtension = ".htm" Then
'                                              ExecuteOnUIThread(Sub()
'                                                                    Try
'                                                                        pdfViewer.Visible = True
'                                                                        pdfViewer.BringToFront()
'                                                                        pdfViewer.Navigate(filePath)
'                                                                    Catch ex As Exception
'                                                                        ExecuteOnUIThread(Sub()
'                                                                                              DisplayFilePath(filePath)
'                                                                                          End Sub)
'                                                                    End Try
'                                                                End Sub)
'                                          End If
'                                      Else
'                                          ' إذا لم يتم العثور على العقدة، حاول البحث باستخدام مسارات بديلة
'                                          For Each node As TreeNode In GetAllNodes(resultsTreeView.Nodes)
'                                              If node.Tag IsNot Nothing AndAlso
'                                             node.Tag.ToString().EndsWith(filePath, StringComparison.OrdinalIgnoreCase) Then
'                                                  resultsTreeView.SelectedNode = node
'                                                  node.EnsureVisible()
'                                                  selectedNodeTag = node.Tag.ToString()
'                                                  isUserSelecting = False

'                                                  ' تحديث تسمية المسار لعرض المسار الكامل
'                                                  ExecuteOnUIThread(Sub()
'                                                                        LabelFileName.Text = "المسار: " & filePath
'                                                                        LabelFileName.ForeColor = Color.DarkBlue
'                                                                    End Sub)

'                                                  ' عرض الملف في WebBrowser
'                                                  Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'                                                  If fileExtension = ".pdf" OrElse fileExtension = ".html" OrElse fileExtension = ".htm" Then
'                                                      ExecuteOnUIThread(Sub()
'                                                                            Try
'                                                                                pdfViewer.Visible = True
'                                                                                pdfViewer.BringToFront()
'                                                                                pdfViewer.Navigate(filePath)
'                                                                            Catch ex As Exception
'                                                                                ExecuteOnUIThread(Sub()
'                                                                                                      DisplayFilePath(filePath)
'                                                                                                  End Sub)
'                                                                            End Try
'                                                                        End Sub)
'                                                  End If
'                                                  Exit For
'                                              End If
'                                          Next
'                                      End If
'                                  End If
'                              Catch ex As Exception
'                                  System.Diagnostics.Debug.WriteLine($"خطأ في AutoSelectFirstResult: {ex.Message}")
'                              End Try
'                          End Sub)
'    End Sub

'    '''' <summary>
'    '''' فتح الملف في WebBrowser وتحديث النماذج المرتبطة
'    '''' </summary>
'    'Private Sub OpenFileInWebBrowser(filePath As String, fileName As String)
'    '    Try
'    '        ' الحصول على امتداد الملف
'    '        Dim fileExtension As String = Path.GetExtension(filePath).ToLower()

'    '        ' فتح الملف بناءً على نوعه
'    '        Select Case fileExtension
'    '            Case ".pdf", ".html", ".htm"
'    '                ' عرض ملف PDF أو HTML داخل WebBrowser
'    '                ExecuteOnUIThread(Sub()
'    '                                      pdfViewer.Visible = True
'    '                                      pdfViewer.Navigate(filePath)
'    '                                  End Sub)
'    '        End Select
'    '    Catch ex As Exception
'    '        MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    '    End Try
'    'End Sub

'    ''' <summary>
'    ''' تحديث نموذج الأرشيف مع معلومات الملف المختار
'    ''' </summary>
'    Private Sub UpdateArchiveForm(filePath As String, fileName As String, fileExtension As String)
'        Try
'            ' البحث عن نموذج الأرشيف المفتوح
'            For Each form As Form In Application.OpenForms
'                If TypeOf form Is ArchiveForm Then
'                    Dim archiveForm As ArchiveForm = DirectCast(form, ArchiveForm)

'                    ' تحديث حقول النموذج
'                    archiveForm.txtOriginalPath.Text = filePath
'                    archiveForm.txtFilename.Text = fileName
'                    archiveForm.txtFileType.Text = fileExtension.TrimStart(".")

'                    ' جلب اسم المجلد من المسار
'                    Dim folderPath As String = Path.GetDirectoryName(filePath)
'                    Dim archivePath As String = My.Settings.DefaultPathSearch

'                    If folderPath.StartsWith(archivePath) Then
'                        Dim relativePath As String = folderPath.Substring(archivePath.Length).Trim("\")
'                        If Not String.IsNullOrEmpty(relativePath) Then
'                            archiveForm.ComboArchiveFolder.Text = relativePath
'                        End If
'                    End If

'                    ' عرض الملف في WebBrowser
'                    archiveForm.WebBrowser1.Navigate(filePath)

'                    ' إحضار النموذج إلى المقدمة
'                    archiveForm.BringToFront()
'                    Exit For
'                End If
'            Next
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث نموذج الأرشيف: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' إضافة الملف تلقائيًا إلى نموذج الأرشيف عند الضغط على السهم السفلي
'    ''' </summary>
'    Private Sub AutoAddToArchiveForm()
'        Try
'            ' التحقق من وجود عقدة محددة
'            If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'                Dim filePath As String = resultsTreeView.SelectedNode.Tag.ToString()

'                ' التحقق من وجود الملف
'                If Not File.Exists(filePath) Then
'                    MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                    Return
'                End If

'                ' الحصول على معلومات الملف
'                Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)
'                Dim fileExtension As String = Path.GetExtension(filePath).ToLower()

'                ' تحديث نموذج الأرشيف
'                UpdateArchiveForm(filePath, fileName, fileExtension)

'                ' محاولة حفظ البيانات تلقائيًا في نموذج الأرشيف
'                TryAutoSaveInArchiveForm()

'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في AutoAddToArchiveForm: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' محاولة حفظ البيانات تلقائيًا في نموذج الأرشيف
'    ''' </summary>
'    Private Sub TryAutoSaveInArchiveForm()
'        Try
'            ' البحث عن نموذج الأرشيف المفتوح
'            For Each form As Form In Application.OpenForms
'                If TypeOf form Is ArchiveForm Then
'                    Dim archiveForm As ArchiveForm = DirectCast(form, ArchiveForm)

'                    ' التحقق من أن الحقول الأساسية معبأة
'                    If Not String.IsNullOrWhiteSpace(archiveForm.txtFilename.Text) AndAlso
'                       Not String.IsNullOrWhiteSpace(archiveForm.txtFileType.Text) AndAlso
'                       Not String.IsNullOrWhiteSpace(archiveForm.txtOriginalPath.Text) Then

'                        ' تعبئة الحقول المطلوبة إذا لم تكن معبأة
'                        If String.IsNullOrWhiteSpace(archiveForm.txtArchiveNum.Text) Then
'                            archiveForm.txtArchiveNum.Text = GenerateArchiveNumber()
'                        End If

'                        If String.IsNullOrWhiteSpace(archiveForm.txtArchiveSubject.Text) Then
'                            archiveForm.txtArchiveSubject.Text = "موضوع الملف"
'                        End If

'                        If String.IsNullOrWhiteSpace(archiveForm.ComboArchiveFolder.Text) Then
'                            archiveForm.ComboArchiveFolder.Text = "صادر"
'                        End If

'                        If String.IsNullOrWhiteSpace(archiveForm.txtUserName.Text) Then
'                            archiveForm.txtUserName.Text = GlobalData.LoggedInUserName
'                        End If

'                        ' الضغط على زر الإضافة تلقائيًا
'                        archiveForm.btnAutoAdd.PerformClick()
'                    End If
'                    Exit For
'                End If
'            Next
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في TryAutoSaveInArchiveForm: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' توليد رقم أرشيف فريد
'    ''' </summary>
'    Private Function GenerateArchiveNumber() As String
'        Try
'            ' استخدام التاريخ والوقت لضمان الفريد
'            Return $"ARC-{DateTime.Now:yyyyMMdd-HHmmss}"
'        Catch ex As Exception
'            Return $"ARC-{DateTime.Now.Ticks}"
'        End Try
'    End Function

'    Private Sub CustomSearch_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
'        searchTextBox.Focus()
'    End Sub
'#End Region
'End Class



' CustomSearch.vb - نموذج البحث العام

'Imports System.IO
'Imports System.Data
'Imports System.Drawing
'Imports System.Windows.Forms
'Imports System.Threading
'Imports System.Threading.Tasks

'Public Class CustomSearch
'    ' تعريف المتغيرات
'    Private WithEvents DelayTimer As New System.Windows.Forms.Timer()
'    Private ReadOnly delayTime As Integer = 2000 ' التأخير بالمللي ثانية (2000ms = 2 ثانية)
'    Private cts As Threading.CancellationTokenSource
'    Private isArchiveMode As Boolean = False ' تحديد وضع النموذج (1: مستعرض ملفات، 2: وضع الأرشيف)
'    Public Event DataSelected(filePath As String, fileName As String, fileExtension As String) ' حدث لاختيار ملف

'    ' إضافة تعريف searchTask كما طلب الخطأ
'    Private searchTask As Task

'    ' متغيرات إضافية
'    Private results As New List(Of Tuple(Of String, String)) ' (اسم الملف، مسار الملف)
'    Private iconsImageList As New ImageList()
'    Private isSearchInProgress As Boolean = False
'    Private searchTimer As New Stopwatch()
'    Private searchQuery As String = String.Empty
'    Private isLoadingData As Boolean = False
'    ' متغير لحفظ حالة التحديد
'    Private selectedNodeTag As String = String.Empty
'    Private isUserSelecting As Boolean = False
'    Dim xx As Short = 0

'    ''' <summary>
'    ''' دالة البناء للنموذج - يتم استدعاؤها عند إنشاء النموذج
'    ''' </summary>
'    Public Sub New()
'        InitializeComponent()
'        InitializeComponents()
'    End Sub

'    ''' <summary>
'    ''' تهيئة المكونات الأساسية للنموذج
'    ''' </summary>
'    Private Sub InitializeComponents()
'        ' تعيين خصائص Timer
'        DelayTimer.Interval = delayTime

'        ' تعيين خصائص شريط التقدم
'        ProgressBar1.Style = ProgressBarStyle.Continuous
'        ProgressBar1.Minimum = 0
'        ProgressBar1.Maximum = 100

'        ' إعداد قائمة الصور
'        iconsImageList.ColorDepth = ColorDepth.Depth32Bit
'        iconsImageList.ImageSize = New Size(16, 16)

'        ' تهيئة عناصر واجهة المستخدم
'        InitializeUI()
'    End Sub

'    Private Sub InitializeUI()
'        ' تهيئة TreeView
'        resultsTreeView.ImageList = iconsImageList
'        resultsTreeView.ShowNodeToolTips = True

'        ' تهيئة ProgressBar
'        ProgressBar1.Minimum = 0
'        ProgressBar1.Maximum = 100
'        ProgressBar1.Value = 0
'        progressLabel.Text = "0%"

'        ' تهيئة Search TextBox
'        searchTextBox.Focus()
'    End Sub

'    Private Sub StartSearch()
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            MessageBox.Show("يرجى إدخال نص للبحث", "بحث فارغ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'            Return
'        End If

'        ' تهيئة واجهة المستخدم للبحث
'        InitializeSearchUI()

'        ' بدء البحث
'        cts = New CancellationTokenSource()
'        ' استخدام Await بدلاً من Result لتجنب تجميد الواجهة
'        searchTask = Task.Run(Sub() PerformSearch(searchTextBox.Text, cts.Token), cts.Token)
'    End Sub

'    Private Sub InitializeSearchUI()
'        ' تفريغ النتائج
'        resultsTreeView.Nodes.Clear()
'        iconsImageList.Images.Clear()

'        ' تهيئة شريط التقدم
'        ProgressBar1.Value = 0
'        progressLabel.Text = "0%"
'        searchTimer.Restart()
'        isSearchInProgress = True

'        ' تحديث النصوص
'        TxtMsg.Text = "جاري البحث..."
'        searchQuery = searchTextBox.Text
'    End Sub

'    Private Sub PerformSearch(query As String, token As CancellationToken)
'        Try
'            ' حفظ التحديد الحالي قبل البدء في البحث
'            If Not isUserSelecting Then
'                SaveCurrentSelection()
'            End If

'            ' البحث في مجلد الأرشيف الافتراضي
'            Dim folderToSearch As String = My.Settings.DefaultPathSearch

'            ' التحقق من وجود المجلد
'            If Not String.IsNullOrEmpty(folderToSearch) AndAlso Directory.Exists(folderToSearch) Then
'                token.ThrowIfCancellationRequested()
'                SearchInFolder(folderToSearch, query, token)
'            Else
'                ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'                Dim tempFolder As String = Path.GetTempPath()
'                If Directory.Exists(tempFolder) Then
'                    token.ThrowIfCancellationRequested()
'                    SearchInFolder(tempFolder, query, token)
'                End If
'            End If

'            ' تحديث واجهة المستخدم مع النتائج
'            If Me.InvokeRequired Then
'                Me.Invoke(Sub()
'                              UpdateUIWithResultsInternal(results)
'                              CompleteSearchUI()

'                              ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'                              If Not isUserSelecting Then
'                                  RestoreSelection()
'                              End If
'                          End Sub)
'            Else
'                UpdateUIWithResultsInternal(results)
'                CompleteSearchUI()

'                ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'                If Not isUserSelecting Then
'                    RestoreSelection()
'                End If
'            End If
'        Catch ex As OperationCanceledException
'            ' تم إلغاء البحث
'            If Me.InvokeRequired Then
'                Me.Invoke(Sub()
'                              CompleteSearchUI("تم إلغاء البحث")

'                              ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'                              If Not isUserSelecting Then
'                                  RestoreSelection()
'                              End If
'                          End Sub)
'            Else
'                CompleteSearchUI("تم إلغاء البحث")

'                ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'                If Not isUserSelecting Then
'                    RestoreSelection()
'                End If
'            End If
'        Catch ex As Exception
'            ' حدث خطأ
'            If Me.InvokeRequired Then
'                Me.Invoke(Sub()
'                              CompleteSearchUI($"حدث خطأ: {ex.Message}")

'                              ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'                              If Not isUserSelecting Then
'                                  RestoreSelection()
'                              End If
'                          End Sub)
'            Else
'                CompleteSearchUI($"حدث خطأ: {ex.Message}")

'                ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'                If Not isUserSelecting Then
'                    RestoreSelection()
'                End If
'            End If
'        End Try
'    End Sub


'    'Private Sub PerformSearch(query As String, token As CancellationToken)
'    '    Try
'    '        ' حفظ التحديد الحالي قبل البدء في البحث
'    '        If Not isUserSelecting Then
'    '            SaveCurrentSelection()
'    '        End If

'    '        ' البحث في مجلد الأرشيف الافتراضي
'    '        Dim folderToSearch As String = My.Settings.DefaultPathSearch

'    '        ' التحقق من وجود المجلد
'    '        If Not String.IsNullOrEmpty(folderToSearch) AndAlso Directory.Exists(folderToSearch) Then
'    '            token.ThrowIfCancellationRequested()
'    '            SearchInFolder(folderToSearch, query, token)
'    '        Else
'    '            ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'    '            Dim tempFolder As String = Path.GetTempPath()
'    '            If Directory.Exists(tempFolder) Then
'    '                token.ThrowIfCancellationRequested()
'    '                SearchInFolder(tempFolder, query, token)
'    '            End If
'    '        End If

'    '        ' تحديث واجهة المستخدم مع النتائج
'    '        Me.Invoke(Sub()
'    '                      UpdateUIWithResults(results)
'    '                      CompleteSearchUI()

'    '                      ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'    '                      If Not isUserSelecting Then
'    '                          RestoreSelection()
'    '                      End If
'    '                  End Sub)
'    '    Catch ex As OperationCanceledException
'    '        ' تم إلغاء البحث
'    '        Me.Invoke(Sub()
'    '                      CompleteSearchUI("تم إلغاء البحث")

'    '                      ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'    '                      If Not isUserSelecting Then
'    '                          RestoreSelection()
'    '                      End If
'    '                  End Sub)
'    '    Catch ex As Exception
'    '        ' حدث خطأ
'    '        Me.Invoke(Sub()
'    '                      CompleteSearchUI($"حدث خطأ: {ex.Message}")

'    '                      ' استعادة التحديد فقط إذا لم يكن المستخدم يختار ملفًا حاليًا
'    '                      If Not isUserSelecting Then
'    '                          RestoreSelection()
'    '                      End If
'    '                  End Sub)
'    '    End Try
'    'End Sub

'    'Private Sub PerformSearch(query As String, token As CancellationToken)
'    '    Try
'    '        ' البحث في مجلد الأرشيف الافتراضي
'    '        Dim folderToSearch As String = My.Settings.DefaultPathSearch

'    '        ' التحقق من وجود المجلد
'    '        If Not String.IsNullOrEmpty(folderToSearch) AndAlso Directory.Exists(folderToSearch) Then
'    '            token.ThrowIfCancellationRequested()
'    '            SearchInFolder(folderToSearch, query, token)
'    '        Else
'    '            ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'    '            Dim tempFolder As String = Path.GetTempPath()
'    '            If Directory.Exists(tempFolder) Then
'    '                token.ThrowIfCancellationRequested()
'    '                SearchInFolder(tempFolder, query, token)
'    '            End If
'    '        End If

'    '        ' تحديث واجهة المستخدم مع النتائج
'    '        Me.Invoke(Sub()
'    '                      UpdateUIWithResults(results)
'    '                      CompleteSearchUI()
'    '                      AutoSelectFirstResult() ' استدعاء الدالة الجديدة
'    '                  End Sub)
'    '    Catch ex As OperationCanceledException
'    '        ' تم إلغاء البحث
'    '        Me.Invoke(Sub()
'    '                      CompleteSearchUI("تم إلغاء البحث")
'    '                  End Sub)
'    '    Catch ex As Exception
'    '        ' حدث خطأ
'    '        Me.Invoke(Sub()
'    '                      CompleteSearchUI($"حدث خطأ: {ex.Message}")
'    '                  End Sub)
'    '    End Try
'    'End Sub

'    Private Sub SearchInFolder(folderPath As String, query As String, token As CancellationToken)
'        Try
'            ' التحقق من وجود المجلد
'            If Not Directory.Exists(folderPath) Then
'                Return
'            End If

'            ' البحث في الملفات
'            Dim files = Directory.GetFiles(folderPath, $"*{query}*", SearchOption.AllDirectories)
'            Dim totalFiles = files.Length

'            ' التحقق من وجود ملفات
'            If totalFiles = 0 Then
'                Return
'            End If

'            Dim processedFiles = 0
'            For Each file In files
'                token.ThrowIfCancellationRequested()
'                Dim fileName As String = Path.GetFileName(file)
'                results.Add(Tuple.Create(fileName, file))
'                processedFiles += 1

'                ' تحديث شريط التقدم
'                Dim progress = CInt((processedFiles / totalFiles) * 100)
'                If ProgressBar1.GetCurrentParent().InvokeRequired Then
'                    ProgressBar1.GetCurrentParent().Invoke(Sub()
'                                                               ProgressBar1.Value = progress
'                                                               progressLabel.Text = $"{progress}%"
'                                                           End Sub)
'                Else
'                    ProgressBar1.Value = progress
'                    progressLabel.Text = $"{progress}%"
'                End If
'            Next
'        Catch ex As Exception
'            ' تجاهل الأخطاء في مجلد معين والبحث في المجلدات الأخرى
'        End Try
'    End Sub

'    ''' <summary>
'    ''' التحديث الداخلي لواجهة المستخدم مع النتائج (يجب استدعاءه فقط من الخيط الرئيسي)
'    ''' </summary>
'    Private Sub UpdateUIWithResultsInternal(results As List(Of Tuple(Of String, String)))
'        Try
'            ' تنظيف TreeView والـ ImageList
'            iconsImageList.Images.Clear()
'            resultsTreeView.Nodes.Clear()

'            ' إضافة أيقونات افتراضية للمجلدات والملفات
'            Dim folderIcon As Icon = SystemIcons.WinLogo
'            Dim fileIcon As Icon = SystemIcons.Application
'            iconsImageList.Images.Add("folder", folderIcon.ToBitmap())
'            iconsImageList.Images.Add("file", fileIcon.ToBitmap())

'            ' بناء هيكل الشجرة للنتائج
'            Dim rootNodes As New Dictionary(Of String, TreeNode)()

'            For Each result In results
'                Dim fileName = result.Item1
'                Dim filePath = result.Item2

'                ' تحديد نوع الملف
'                Dim fileExtension As String = Path.GetExtension(fileName).ToLower()
'                Dim iconKey As String = "file"

'                ' ترتيب الملفات حسب المجلدات
'                Dim directoryPath As String = Path.GetDirectoryName(filePath)
'                Dim relativePath As String = directoryPath.Replace(My.Settings.DefaultPathSearch, "").Trim("\")

'                ' إنشاء العقد حسب الهيكل
'                Dim currentNodes As TreeNodeCollection = resultsTreeView.Nodes
'                If Not String.IsNullOrEmpty(relativePath) Then
'                    Dim parts() As String = relativePath.Split("\"c)
'                    For Each part In parts
'                        If Not rootNodes.ContainsKey(part) Then
'                            Dim newNode As New TreeNode(part)
'                            newNode.ImageKey = "folder"
'                            newNode.SelectedImageKey = "folder"
'                            currentNodes.Add(newNode)
'                            rootNodes.Add(part, newNode)
'                            currentNodes = newNode.Nodes
'                        Else
'                            currentNodes = rootNodes(part).Nodes
'                        End If
'                    Next
'                End If

'                ' إضافة الملف
'                Dim fileNode As New TreeNode(fileName)
'                fileNode.Tag = filePath
'                fileNode.ImageKey = iconKey
'                fileNode.SelectedImageKey = iconKey
'                fileNode.ToolTipText = filePath
'                currentNodes.Add(fileNode)
'            Next
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateUIWithResultsInternal: {ex.Message}")
'        End Try
'    End Sub

'    'Private Sub UpdateUIWithResults(results As List(Of Tuple(Of String, String)))
'    '    Try
'    '        ' تخزين حالة التحميل
'    '        Dim wasLoading As Boolean = isLoadingData
'    '        isLoadingData = True

'    '        ' تنظيف TreeView والـ ImageList
'    '        iconsImageList.Images.Clear()
'    '        resultsTreeView.Nodes.Clear()

'    '        ' إضافة أيقونات افتراضية للمجلدات والملفات
'    '        Dim folderIcon As Icon = SystemIcons.WinLogo
'    '        Dim fileIcon As Icon = SystemIcons.Application
'    '        iconsImageList.Images.Add("folder", folderIcon.ToBitmap())
'    '        iconsImageList.Images.Add("file", fileIcon.ToBitmap())

'    '        ' بناء هيكل الشجرة للنتائج
'    '        Dim rootNodes As New Dictionary(Of String, TreeNode)()

'    '        For Each result In results
'    '            Dim fileName = result.Item1
'    '            Dim filePath = result.Item2

'    '            ' تحديد نوع الملف
'    '            Dim fileExtension As String = Path.GetExtension(fileName).ToLower()
'    '            Dim iconKey As String = "file"

'    '            ' ترتيب الملفات حسب المجلدات
'    '            Dim directoryPath As String = Path.GetDirectoryName(filePath)
'    '            Dim relativePath As String = directoryPath.Replace(My.Settings.DefaultPathSearch, "").Trim("\")

'    '            ' إنشاء العقد حسب الهيكل
'    '            Dim currentNodes As TreeNodeCollection = resultsTreeView.Nodes
'    '            If Not String.IsNullOrEmpty(relativePath) Then
'    '                Dim parts() As String = relativePath.Split("\"c)
'    '                For Each part In parts
'    '                    If Not rootNodes.ContainsKey(part) Then
'    '                        Dim newNode As New TreeNode(part)
'    '                        newNode.ImageKey = "folder"
'    '                        newNode.SelectedImageKey = "folder"
'    '                        currentNodes.Add(newNode)
'    '                        rootNodes.Add(part, newNode)
'    '                        currentNodes = newNode.Nodes
'    '                    Else
'    '                        currentNodes = rootNodes(part).Nodes
'    '                    End If
'    '                Next
'    '            End If

'    '            ' إضافة الملف
'    '            Dim fileNode As New TreeNode(fileName)
'    '            fileNode.Tag = filePath
'    '            fileNode.ImageKey = iconKey
'    '            fileNode.SelectedImageKey = iconKey
'    '            fileNode.ToolTipText = filePath
'    '            currentNodes.Add(fileNode)
'    '        Next
'    '    Catch ex As Exception
'    '        System.Diagnostics.Debug.WriteLine($"خطأ في UpdateUIWithResults: {ex.Message}")
'    '    Finally
'    '        isLoadingData = False
'    '    End Try
'    'End Sub

'    'Private Sub UpdateUIWithResults(results As List(Of Tuple(Of String, String)))
'    '    ' تنظيف TreeView والـ ImageList
'    '    iconsImageList.Images.Clear()
'    '    resultsTreeView.Nodes.Clear()

'    '    ' إضافة أيقونات افتراضية للمجلدات والملفات
'    '    Dim folderIcon As Icon = SystemIcons.WinLogo
'    '    Dim fileIcon As Icon = SystemIcons.Application
'    '    iconsImageList.Images.Add("folder", folderIcon.ToBitmap())
'    '    iconsImageList.Images.Add("file", fileIcon.ToBitmap())

'    '    ' بناء هيكل الشجرة للنتائج
'    '    Dim rootNodes As New Dictionary(Of String, TreeNode)()

'    '    For Each result In results
'    '        Dim fileName = result.Item1
'    '        Dim filePath = result.Item2

'    '        ' تحديد نوع الملف
'    '        Dim fileExtension As String = Path.GetExtension(fileName).ToLower()
'    '        Dim iconKey As String = "file"

'    '        ' ترتيب الملفات حسب المجلدات
'    '        Dim directoryPath As String = Path.GetDirectoryName(filePath)
'    '        Dim relativePath As String = directoryPath.Replace(My.Settings.DefaultPathSearch, "").Trim("\")

'    '        ' إنشاء العقد حسب الهيكل
'    '        Dim currentNodes As TreeNodeCollection = resultsTreeView.Nodes
'    '        If Not String.IsNullOrEmpty(relativePath) Then
'    '            Dim parts() As String = relativePath.Split("\"c)
'    '            For Each part In parts
'    '                If Not rootNodes.ContainsKey(part) Then
'    '                    Dim newNode As New TreeNode(part)
'    '                    newNode.ImageKey = "folder"
'    '                    newNode.SelectedImageKey = "folder"
'    '                    currentNodes.Add(newNode)
'    '                    rootNodes.Add(part, newNode)
'    '                    currentNodes = newNode.Nodes
'    '                Else
'    '                    currentNodes = rootNodes(part).Nodes
'    '                End If
'    '            Next
'    '        End If

'    '        ' إضافة الملف
'    '        Dim fileNode As New TreeNode(fileName)
'    '        fileNode.Tag = filePath
'    '        fileNode.ImageKey = iconKey
'    '        fileNode.SelectedImageKey = iconKey
'    '        fileNode.ToolTipText = filePath
'    '        currentNodes.Add(fileNode)
'    '    Next
'    'End Sub

'    Private Sub CompleteSearchUI(Optional message As String = Nothing)
'        isSearchInProgress = False
'        searchTimer.Stop()

'        ' تحديث الرسالة
'        If String.IsNullOrEmpty(message) Then
'            If results.Count > 0 Then
'                TxtMsg.Text = $"تم العثور على {results.Count} ملف/ملفات مطابقة في {searchTimer.Elapsed.TotalSeconds:F2} ثانية"
'            Else
'                TxtMsg.Text = "لم يتم العثور على أي ملفات مطابقة للبحث."
'            End If
'        Else
'            TxtMsg.Text = message
'        End If
'    End Sub

'    Private Sub searchTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles searchTextBox.KeyDown
'        If e.KeyCode = Keys.Enter Then
'            StartSearch()
'            e.Handled = True
'        End If
'    End Sub

'    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
'        If isSearchInProgress AndAlso cts IsNot Nothing Then
'            cts.Cancel()
'        End If
'    End Sub

'    Private Sub resultsTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseClick
'        Try
'            ' تحديد أن المستخدم يختار ملفًا
'            isUserSelecting = True

'            ' حفظ التحديد الحالي
'            SaveCurrentSelection()

'            ' إذا كان النقر على زر الماوس الأيسر
'            If e.Button = MouseButtons.Left Then
'                ' يمكن إضافة أي معالجة إضافية هنا
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في حدث NodeMouseClick: {ex.Message}")
'        Finally
'            ' إعادة التهيئة بعد فترة قصيرة
'            Task.Run(Sub()
'                         Threading.Thread.Sleep(300)
'                         isUserSelecting = False
'                     End Sub)
'        End Try
'    End Sub

'    Private Sub resultsTreeView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseDoubleClick
'        OpenSelectedFile(e.Node, True)
'    End Sub

'    Private Sub resultsTreeView_KeyDown(sender As Object, e As KeyEventArgs) Handles resultsTreeView.KeyDown
'        If e.KeyCode = Keys.Enter AndAlso resultsTreeView.SelectedNode IsNot Nothing Then
'            OpenSelectedFile(resultsTreeView.SelectedNode, True)
'        ElseIf e.KeyCode = Keys.Down Then
'            ' عند الضغط على السهم السفلي
'            If resultsTreeView.SelectedNode IsNot Nothing Then
'                AutoAddToArchiveForm()
'            End If
'        End If
'    End Sub

'    Private Sub OpenSelectedFile(selectedNode As TreeNode, isDoubleClick As Boolean)
'        ' التحقق من أن العقدة و Tag ليست Nothing
'        If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then
'            Return
'        End If

'        Dim filePath As String = selectedNode.Tag.ToString()

'        ' التحقق من وجود الملف
'        If Not File.Exists(filePath) Then
'            MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Return
'        End If

'        Try
'            ' الحصول على امتداد الملف
'            Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'            Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)

'            ' فتح الملف بناءً على نوعه
'            Select Case fileExtension
'                Case ".pdf", ".html", ".htm"
'                    ' عرض ملف PDF أو HTML داخل WebBrowser
'                    pdfViewer.Visible = True
'                    pdfViewer.Navigate(filePath)
'                    txtOriginalPath.Text = filePath

'                    ' إذا كان هذا نقرة مزدوجة أو ضغط Enter، قم بإرسال الإشارة
'                    If isDoubleClick Then
'                        ' حفظ معلومات الملف المختار
'                        SelectedFilePath = filePath
'                        SelectedFileName = fileName
'                        SelectedFileExtension = fileExtension

'                        ' إرسال إشارة لجميع النماذج المستمعة
'                        RaiseEvent DataSelected(filePath, fileName, fileExtension)

'                        ' إغلاق النموذج
'                        Me.Close()
'                    End If
'                Case Else
'                    ' فتح الملف باستخدام التطبيق الافتراضي
'                    pdfViewer.Visible = False
'                    Process.Start(filePath)
'            End Select
'        Catch ex As Exception
'            ' التعامل مع الأخطاء
'            MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub DelayTimer_Tick(sender As Object, e As EventArgs) Handles DelayTimer.Tick
'        ' إيقاف Timer بعد انتهاء الوقت
'        DelayTimer.Stop()

'        ' التحقق من قيمة Tag
'        If Not (Me.Tag = 1 OrElse Me.Tag = 2) Then Return

'        ' التحقق من صحة النص المدخل
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            Dim errorMsg = If(Me.Tag = 1, "لم يتم العثور على ملف, الرجاء إدخال قيمة صحيحة", "Please enter a search term.")
'            TxtMsg.Text = errorMsg
'            Return
'        End If

'        ' تهيئة العناصر
'        InitializeSearchUI()

'        ' إعداد مصدر الإلغاء
'        cts = New Threading.CancellationTokenSource()
'        Dim token = cts.Token

'        Try
'            ' استخدام Task.Run بدون .Result لتجنب تجميد الواجهة
'            Task.Run(Sub()
'                         ' التحقق من أن المستخدم لا يختار ملفًا حاليًا
'                         If Not isUserSelecting Then
'                             PerformSearch(searchTextBox.Text, token)
'                         Else
'                             ' إذا كان المستخدم يختار ملفًا، أعد جدولة البحث
'                             DelayTimer.Stop()
'                             DelayTimer.Start()
'                         End If
'                     End Sub)
'        Catch ex As OperationCanceledException
'            MessageBox.Show("Search cancelled.")
'        Catch ex As Exception
'            MessageBox.Show($"Error: {ex.Message}")
'        Finally
'            ' إعادة ضبط حالة الأزراب والعناصر
'            ResetUI()
'        End Try
'    End Sub

'    'Private Sub DelayTimer_Tick(sender As Object, e As EventArgs) Handles DelayTimer.Tick
'    '    ' إيقاف Timer بعد انتهاء الوقت
'    '    DelayTimer.Stop()

'    '    ' التحقق من قيمة Tag
'    '    If Not (Me.Tag = 1 OrElse Me.Tag = 2) Then Return

'    '    ' التحقق من صحة النص المدخل
'    '    If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'    '        Dim errorMsg = If(Me.Tag = 1, "لم يتم العثور على ملف, الرجاء إدخال قيمة صحيحة", "Please enter a search term.")
'    '        TxtMsg.Text = errorMsg
'    '        Return
'    '    End If

'    '    ' تهيئة العناصر
'    '    InitializeSearchUI()

'    '    ' إعداد مصدر الإلغاء
'    '    cts = New Threading.CancellationTokenSource()
'    '    Dim token = cts.Token

'    '    Try
'    '        ' استخدام Task.Run بدون .Result لتجنب تجميد الواجهة
'    '        Task.Run(Sub()
'    '                     PerformSearch(searchTextBox.Text, token)
'    '                 End Sub)
'    '    Catch ex As OperationCanceledException
'    '        MessageBox.Show("Search cancelled.")
'    '    Catch ex As Exception
'    '        MessageBox.Show($"Error: {ex.Message}")
'    '    Finally
'    '        ' إعادة ضبط حالة الأزرار والعناصر
'    '        ResetUI()
'    '    End Try
'    'End Sub

'    ' دالة لإعادة ضبط واجهة المستخدم بعد انتهاء البحث
'    Private Sub ResetUI()
'        btnCancel.Enabled = False
'        ProgressBar1.Visible = False
'        progressLabel.Visible = False
'    End Sub

'    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            resultsTreeView.Nodes.Clear()
'            TxtMsg.Text = "لم يتم العثور على ملف , الرجاء إدخال قيمة صحيحة في مربع البحث"
'        Else
'            ' إعادة تشغيل Timer عند تغيير النص
'            DelayTimer.Stop()
'            DelayTimer.Start()
'            TxtMsg.Text = Nothing
'        End If
'    End Sub

'    Private Sub CustomSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        ' إعدادات Timer
'        DelayTimer.Interval = delayTime

'        ' تحميل المجلد الافتراضي
'        InitializeDefaultFolder()
'    End Sub

'    Private Sub InitializeDefaultFolder()
'        ' تحديد المجلد الافتراضي للبحث
'        Dim defaultFolder As String = My.Settings.DefaultPathSearch

'        ' التأكد من وجود المجلد
'        If Not String.IsNullOrEmpty(defaultFolder) AndAlso Directory.Exists(defaultFolder) Then
'            ' يمكن إضافة شرح للمجلد الافتراضي إذا لزم
'        Else
'            ' إذا لم يكن المجلد موجودًا، استخدام مجلد النظام المؤقت
'            defaultFolder = Path.GetTempPath()
'        End If
'    End Sub

'    Private Sub CustomSearch_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
'        ' إلغاء المهمة الجارية إذا كانت موجودة
'        If isSearchInProgress AndAlso cts IsNot Nothing Then
'            cts.Cancel()
'        End If
'    End Sub

'    ''' <summary>
'    ''' خاصية لتخزين مسار الملف المختار
'    ''' </summary>
'    Public Property SelectedFilePath As String = String.Empty
'    Public Property SelectedFileName As String = String.Empty
'    Public Property SelectedFileExtension As String = String.Empty

'    '#Region "الوظائف الإضافية"
'    ''' <summary>
'    ''' الدالة الجديدة لتحديد أول نتيجة تلقائيًا
'    ''' </summary>
'    Private Sub AutoSelectFirstResult()
'        Try
'            ' التحقق من وجود نتائج
'            If results.Count > 0 Then
'                Dim firstResult As Tuple(Of String, String) = results(0)
'                Dim filePath As String = firstResult.Item2

'                ' البحث عن العقدة في TreeView
'                If Me.InvokeRequired Then
'                    Me.Invoke(Sub()
'                                  Dim targetNode As TreeNode = FindNodeByTag(resultsTreeView.Nodes, filePath)
'                                  If targetNode IsNot Nothing Then
'                                      ' توسيع جميع العقد الأصلية
'                                      ExpandParentNodes(targetNode)

'                                      ' اختيار العقدة
'                                      resultsTreeView.SelectedNode = targetNode
'                                      targetNode.EnsureVisible()

'                                      ' فتح الملف في WebBrowser
'                                      OpenFileInWebBrowser(filePath, firstResult.Item1)
'                                  End If
'                              End Sub)
'                Else
'                    Dim targetNode As TreeNode = FindNodeByTag(resultsTreeView.Nodes, filePath)
'                    If targetNode IsNot Nothing Then
'                        ' توسيع جميع العقد الأصلية
'                        ExpandParentNodes(targetNode)

'                        ' اختيار العقدة
'                        resultsTreeView.SelectedNode = targetNode
'                        targetNode.EnsureVisible()

'                        ' فتح الملف في WebBrowser
'                        OpenFileInWebBrowser(filePath, firstResult.Item1)
'                    End If
'                End If
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في AutoSelectFirstResult: {ex.Message}")
'        End Try
'    End Sub
'    ''' <summary>
'    ''' البحث عن عقدة في TreeView باستخدام Tag
'    ''' </summary>
'    Private Function FindNodeByTag(nodes As TreeNodeCollection, tag As String) As TreeNode
'        For Each node As TreeNode In nodes
'            If node.Tag IsNot Nothing AndAlso node.Tag.ToString() = tag Then
'                Return node
'            End If

'            ' البحث في العقد الفرعية
'            Dim foundNode As TreeNode = FindNodeByTag(node.Nodes, tag)
'            If foundNode IsNot Nothing Then
'                Return foundNode
'            End If
'        Next
'        Return Nothing
'    End Function

'    ''' <summary>
'    ''' معالجة تغيير التحديد
'    ''' </summary>
'    Private Sub resultsTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles resultsTreeView.AfterSelect
'        Try
'            ' تحديد أن المستخدم يختار ملفًا
'            isUserSelecting = True
'            SaveCurrentSelection()
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في AfterSelect: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' معالجة انتهاء التحديد
'    ''' </summary>
'    Private Sub resultsTreeView_Leave(sender As Object, e As EventArgs) Handles resultsTreeView.Leave
'        Try
'            ' إعادة تهيئة بعد فترة قصيرة
'            Task.Run(Sub()
'                         Threading.Thread.Sleep(300)
'                         isUserSelecting = False
'                     End Sub)
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في Leave: {ex.Message}")
'        End Try
'    End Sub

'    '''' <summary>
'    '''' البحث عن عقدة في TreeView باستخدام Tag
'    '''' </summary>
'    'Private Function FindNodeByTag(nodes As TreeNodeCollection, tag As String) As TreeNode
'    '    For Each node As TreeNode In nodes
'    '        If node.Tag IsNot Nothing AndAlso node.Tag.ToString() = tag Then
'    '            Return node
'    '        End If

'    '        ' البحث في العقد الفرعية
'    '        Dim foundNode As TreeNode = FindNodeByTag(node.Nodes, tag)
'    '        If foundNode IsNot Nothing Then
'    '            Return foundNode
'    '        End If
'    '    Next
'    '    Return Nothing
'    'End Function

'    '''' <summary>
'    '''' توسيع جميع العقد الأصلية للعقدة المحددة
'    '''' </summary>
'    'Private Sub ExpandParentNodes(node As TreeNode)
'    '    Dim currentNode As TreeNode = node
'    '    While currentNode.Parent IsNot Nothing
'    '        currentNode.Parent.Expand()
'    '        currentNode = currentNode.Parent
'    '    End While
'    'End Sub

'    ''' <summary>
'    ''' فتح الملف في WebBrowser وتحديث النماذج المرتبطة
'    ''' </summary>
'    Private Sub OpenFileInWebBrowser(filePath As String, fileName As String)
'        Try
'            ' الحصول على امتداد الملف
'            Dim fileExtension As String = Path.GetExtension(filePath).ToLower()

'            ' فتح الملف بناءً على نوعه
'            Select Case fileExtension
'                Case ".pdf", ".html", ".htm"
'                    ' عرض ملف PDF أو HTML داخل WebBrowser
'                    pdfViewer.Visible = True
'                    pdfViewer.Navigate(filePath)

'                    ' تحديث حقل المسار الأصلي
'                    txtOriginalPath.Text = filePath

'                    ' إرسال إشارة لجميع النماذج المستمعة
'                    RaiseEvent DataSelected(filePath, fileName, fileExtension)

'                    ' تحديث نموذج الأرشيف إذا كان مفتوحًا
'                    UpdateArchiveForm(filePath, fileName, fileExtension)
'            End Select
'        Catch ex As Exception
'            MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تحديث نموذج الأرشيف مع معلومات الملف المختار
'    ''' </summary>
'    Private Sub UpdateArchiveForm(filePath As String, fileName As String, fileExtension As String)
'        Try
'            ' البحث عن نموذج الأرشيف المفتوح
'            For Each form As Form In Application.OpenForms
'                If TypeOf form Is ArchiveForm Then
'                    Dim archiveForm As ArchiveForm = DirectCast(form, ArchiveForm)

'                    ' تحديث حقول النموذج
'                    archiveForm.txtOriginalPath.Text = filePath
'                    archiveForm.txtFilename.Text = fileName
'                    archiveForm.txtFileType.Text = fileExtension.TrimStart(".")
'                    archiveForm.WebBrowser1.Navigate(filePath)

'                    ' جلب اسم المجلد من المسار
'                    Dim folderPath As String = Path.GetDirectoryName(filePath)
'                    Dim archivePath As String = My.Settings.DefaultPathSearch

'                    If folderPath.StartsWith(archivePath) Then
'                        Dim relativePath As String = folderPath.Substring(archivePath.Length).Trim("\")
'                        If Not String.IsNullOrEmpty(relativePath) Then
'                            archiveForm.ComboArchiveFolder.Text = relativePath
'                        End If
'                    End If

'                    ' إحضار النموذج إلى المقدمة
'                    archiveForm.BringToFront()
'                    Exit For
'                End If
'            Next
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث نموذج الأرشيف: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' إضافة الملف تلقائيًا إلى نموذج الأرشيف عند الضغط على السهم السفلي
'    ''' </summary>
'    Private Sub AutoAddToArchiveForm()
'        Try
'            ' التحقق من وجود عقدة محددة
'            If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'                Dim filePath As String = resultsTreeView.SelectedNode.Tag.ToString()

'                ' التحقق من وجود الملف
'                If Not File.Exists(filePath) Then
'                    MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                    Return
'                End If

'                ' الحصول على معلومات الملف
'                Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)
'                Dim fileExtension As String = Path.GetExtension(filePath).ToLower()

'                ' تحديث نموذج الأرشيف
'                UpdateArchiveForm(filePath, fileName, fileExtension)

'                ' محاولة حفظ البيانات تلقائيًا في نموذج الأرشيف
'                TryAutoSaveInArchiveForm()
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في AutoAddToArchiveForm: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' محاولة حفظ البيانات تلقائيًا في نموذج الأرشيف
'    ''' </summary>
'    Private Sub TryAutoSaveInArchiveForm()
'        Try
'            ' البحث عن نموذج الأرشيف المفتوح
'            For Each form As Form In Application.OpenForms
'                If TypeOf form Is ArchiveForm Then
'                    Dim archiveForm As ArchiveForm = DirectCast(form, ArchiveForm)

'                    ' التحقق من أن الحقول الأساسية معبأة
'                    If Not String.IsNullOrWhiteSpace(archiveForm.txtFilename.Text) AndAlso
'                       Not String.IsNullOrWhiteSpace(archiveForm.txtFileType.Text) AndAlso
'                       Not String.IsNullOrWhiteSpace(archiveForm.txtOriginalPath.Text) Then

'                        ' تعبئة الحقول المطلوبة إذا لم تكن معبأة
'                        If String.IsNullOrWhiteSpace(archiveForm.txtArchiveNum.Text) Then
'                            archiveForm.txtArchiveNum.Text = GenerateArchiveNumber()
'                        End If

'                        If String.IsNullOrWhiteSpace(archiveForm.txtArchiveSubject.Text) Then
'                            archiveForm.txtArchiveSubject.Text = "موضوع الملف"
'                        End If

'                        If String.IsNullOrWhiteSpace(archiveForm.ComboArchiveFolder.Text) Then
'                            archiveForm.ComboArchiveFolder.Text = "صادر"
'                        End If

'                        If String.IsNullOrWhiteSpace(archiveForm.txtUserName.Text) Then
'                            archiveForm.txtUserName.Text = GlobalData.LoggedInUserName
'                        End If

'                        ' الضغط على زر الإضافة تلقائيًا
'                        archiveForm.btnAdd.PerformClick()
'                    End If
'                    Exit For
'                End If
'            Next
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في TryAutoSaveInArchiveForm: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' توليد رقم أرشيف فريد
'    ''' </summary>
'    Private Function GenerateArchiveNumber() As String
'        Try
'            ' استخدام التاريخ والوقت لضمان الفريد
'            Return $"ARC-{DateTime.Now:yyyyMMdd-HHmmss}"
'        Catch ex As Exception
'            Return $"ARC-{DateTime.Now.Ticks}"
'        End Try
'    End Function

'    ''' <summary>
'    ''' حفظ حالة التحديد الحالية
'    ''' </summary>
'    Private Sub SaveCurrentSelection()
'        If resultsTreeView.SelectedNode IsNot Nothing AndAlso resultsTreeView.SelectedNode.Tag IsNot Nothing Then
'            selectedNodeTag = resultsTreeView.SelectedNode.Tag.ToString()
'        Else
'            selectedNodeTag = String.Empty
'        End If
'    End Sub

'    ''' <summary>
'    ''' استعادة التحديد السابق بعد تحديث النتائج
'    ''' </summary>
'    Private Sub RestoreSelection()
'        Try
'            If String.IsNullOrEmpty(selectedNodeTag) Then Return

'            ' البحث عن العقدة باستخدام التاغ
'            Dim node As TreeNode = FindNodeByTag(resultsTreeView.Nodes, selectedNodeTag)
'            If node IsNot Nothing Then
'                ' توسيع جميع العقد الأصلية
'                ExpandParentNodes(node)

'                ' اختيار العقدة وجعلها مرئية
'                resultsTreeView.SelectedNode = node
'                node.EnsureVisible()
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في استعادة التحديد: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' توسيع جميع العقد الأصلية للعقدة المحددة
'    ''' </summary>
'    Private Sub ExpandParentNodes(node As TreeNode)
'        Dim currentNode As TreeNode = node
'        While currentNode.Parent IsNot Nothing
'            currentNode.Parent.Expand()
'            currentNode = currentNode.Parent
'        End While
'    End Sub

'    '#End Region
'End Class

'Imports System.IO
'Imports System.Drawing
'Imports System.Threading
'Imports System.Threading.Tasks

'Public Class CustomSearch
'#Region "المتغيرات والتهيئة الأساسية"
'    ' تعريف متغيرات النموذج
'    Private WithEvents DelayTimer As New System.Windows.Forms.Timer()
'    Private ReadOnly delayTime As Integer = 2000 ' التأخير بالمللي ثانية (2000ms = 2 ثانية)
'    Private cts As Threading.CancellationTokenSource
'    Private isArchiveMode As Boolean = False ' تحديد وضع النموذج (1: مستعرض ملفات، 2: وضع الأرشيف)
'    Public Event DataSelected(filePath As String, fileName As String, fileExtension As String) ' حدث لاختيار ملف

'    ''' <summary>
'    ''' دالة البناء للنموذج - يتم استدعاؤها عند إنشاء النموذج
'    ''' </summary>
'    Public Sub New()
'        InitializeComponent()
'        InitializeComponents()
'    End Sub

'    ''' <summary>
'    ''' تهيئة المكونات الأساسية للنموذج
'    ''' </summary>
'    Private Sub InitializeComponents()
'        ' تعيين خصائص Timer
'        DelayTimer.Interval = delayTime

'        ' تعيين خصائص شريط التقدم
'        ProgressBar1.Style = ProgressBarStyle.Continuous
'        ProgressBar1.Minimum = 0
'        ProgressBar1.Maximum = 100

'        ' إعداد قائمة الصور
'        iconsImageList.ColorDepth = ColorDepth.Depth32Bit
'        iconsImageList.ImageSize = New Size(16, 16)
'    End Sub
'#End Region

'#Region "معالجة اختيار الملفات"
'    ''' <summary>
'    ''' فتح الملف المحدد في العارض أو ربطه بنموذج الأرشيف
'    ''' </summary>
'    ''' <param name="selectedNode">العقدة المحددة في شجرة البحث</param>
'    ''' <param name="isDoubleClick">هل تم النقر المزدوج على العقدة؟</param>
'    Private Sub OpenSelectedFile(selectedNode As TreeNode, isDoubleClick As Boolean)
'        ' التحقق من أن العقدة و Tag ليست Nothing
'        If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then
'            Return
'        End If

'        Dim filePath As String = selectedNode.Tag.ToString()

'        ' التحقق من وجود الملف
'        If Not File.Exists(filePath) Then
'            MessageBox.Show("الملف غير موجود: " & filePath, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Return
'        End If

'        Try
'            ' الحصول على امتداد الملف
'            Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
'            Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)

'            ' فتح الملف بناءً على نوعه
'            Select Case fileExtension
'                Case ".pdf", ".html", ".htm"
'                    ' عرض ملف PDF أو HTML داخل WebBrowser
'                    pdfViewer.Visible = True
'                    pdfViewer.Navigate(filePath)
'                    txtOriginalPath.Text = filePath

'                    ' التحقق من وضع النموذج بناءً على Tag
'                    If isArchiveMode AndAlso isDoubleClick Then
'                        ' وضع الأرشيف: محاولة ربط الملف مع نموذج الأرشيف
'                        Dim frm As ArchiveForm = TryCast(Application.OpenForms("ArchiveForm"), ArchiveForm)
'                        If frm IsNot Nothing Then
'                            frm.WebBrowser1.Navigate(filePath)
'                            frm.txtFilepath.Text = filePath
'                            frm.txtFilename.Text = fileName
'                            frm.txtFileType.Text = fileExtension
'                            Me.Close()
'                            frm.btnAdd.PerformClick()
'                        Else
'                            ' إذا لم يكن نموذج الأرشيف مفتوحًا، افتحه
'                            Dim newArchiveForm As New ArchiveForm()
'                            newArchiveForm.WebBrowser1.Navigate(filePath)
'                            newArchiveForm.txtFilepath.Text = filePath
'                            newArchiveForm.txtFilename.Text = fileName
'                            newArchiveForm.txtFileType.Text = fileExtension
'                            newArchiveForm.Show()
'                            Me.Close()
'                        End If
'                    ElseIf isDoubleClick Then
'                        ' وضع المستعرض فقط: لا يتم ربط الملف مع أي نموذج
'                        ' يمكن إضافة سلوك مخصص هنا
'                    End If
'                Case Else
'                    ' فتح الملف باستخدام التطبيق الافتراضي
'                    pdfViewer.Visible = False
'                    Process.Start(filePath)
'            End Select
'        Catch ex As Exception
'            ' التعامل مع الأخطاء
'            MessageBox.Show("غير قادر على فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' معالجة ضغط مفتاح السهم لأسفل في مربع البحث
'    ''' </summary>
'    Private Sub searchTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles searchTextBox.KeyDown
'        If e.KeyCode = Keys.Down Then
'            If searchTextBox.Text <> Nothing AndAlso resultsTreeView.Nodes.Count > 0 Then
'                resultsTreeView.Select()
'                If resultsTreeView.Nodes(0) IsNot Nothing Then
'                    resultsTreeView.SelectedNode = resultsTreeView.Nodes(0)
'                    OpenSelectedFile(resultsTreeView.SelectedNode, False)
'                End If
'            End If
'        End If
'    End Sub

'    ''' <summary>
'    ''' معالجة النقر المزدوج على عقدة في شجرة البحث
'    ''' </summary>
'    Private Sub ResultsTreeView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseDoubleClick
'        OpenSelectedFile(e.Node, True)
'    End Sub

'    ''' <summary>
'    ''' معالجة الضغط على مفتاح Enter في شجرة البحث
'    ''' </summary>
'    Private Sub ResultsTreeView_KeyDown(sender As Object, e As KeyEventArgs) Handles resultsTreeView.KeyDown
'        If e.KeyCode = Keys.Enter AndAlso resultsTreeView.SelectedNode IsNot Nothing Then
'            OpenSelectedFile(resultsTreeView.SelectedNode, True)
'        End If
'    End Sub

'    ''' <summary>
'    ''' معالجة تحديد عقدة جديدة في شجرة البحث
'    ''' </summary>
'    Private Sub ResultsTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles resultsTreeView.AfterSelect
'        OpenSelectedFile(e.Node, False)
'        btnOpenLoction.Enabled = True
'        btnPrint.Enabled = True
'    End Sub

'    ''' <summary>
'    ''' معالجة النقر على عقدة في شجرة البحث
'    ''' </summary>
'    Private Sub ResultsTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles resultsTreeView.NodeMouseClick
'        OpenSelectedFile(e.Node, False)
'    End Sub
'#End Region

'#Region "البحث عن الملفات"
'    ''' <summary>
'    ''' إلغاء عملية البحث الجارية
'    ''' </summary>
'    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
'        If cts IsNot Nothing Then
'            cts.Cancel()
'        End If
'    End Sub

'    ''' <summary>
'    ''' تنفيذ عملية البحث عن الملفات
'    ''' </summary>
'    ''' <param name="query">نص الاستعلام</param>
'    ''' <param name="token">رمز الإلغاء</param>
'    ''' <returns>قائمة بالملفات المطابقة</returns>
'    Private Function PerformSearch(query As String, token As Threading.CancellationToken) As List(Of Tuple(Of String, String))
'        Dim results As New List(Of Tuple(Of String, String))()

'        Try
'            ' الحصول على قائمة الملفات المطابقة
'            Dim files = Directory.GetFiles(My.Settings.DefaultPathSearch, $"*{query}*", SearchOption.AllDirectories)
'            Dim totalFiles = files.Length

'            ' التحقق من وجود ملفات
'            If totalFiles = 0 Then
'                TxtMsg.Text = "لم يتم العثور على أي ملفات مطابقة للبحث."
'                Return results
'            Else
'                TxtMsg.Text = $"تم العثور على {totalFiles} ملف/ملفات مطابقة."
'            End If

'            Dim processedFiles = 0

'            ' التأكد من وجود ملفات
'            If totalFiles = 0 Then
'                Return results
'            End If

'            For Each file In files
'                ' التحقق من طلب الإلغاء
'                token.ThrowIfCancellationRequested()

'                Dim fileName As String = Path.GetFileName(file)
'                results.Add(Tuple.Create(fileName, file))

'                processedFiles += 1
'                Dim progress = CInt((processedFiles / totalFiles) * 100)

'                ' تحديث شريط التقدم
'                If Me.InvokeRequired Then
'                    Me.Invoke(Sub()
'                                  ProgressBar1.Value = progress
'                                  progressLabel.Text = $"{progress}%"
'                              End Sub)
'                Else
'                    ProgressBar1.Value = progress
'                    progressLabel.Text = $"{progress}%"
'                End If
'            Next
'        Catch ex As UnauthorizedAccessException
'            ' تسجيل الخطأ أو إعلام المستخدم
'            MessageBox.Show($"خطأ في الوصول إلى الملفات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        Catch ex As OperationCanceledException
'            ' التعامل مع إلغاء العملية
'            MessageBox.Show("تم إلغاء البحث.", "إلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        Catch ex As Exception
'            ' التعامل مع أي أخطاء أخرى
'            MessageBox.Show($"حدث خطأ غير متوقع: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try

'        Return results
'    End Function

'    ''' <summary>
'    ''' معالجة انتهاء التأخير وبدء عملية البحث
'    ''' </summary>
'    Private Async Sub DelayTimer_Tick(sender As Object, e As EventArgs) Handles DelayTimer.Tick
'        ' إيقاف Timer بعد انتهاء الوقت
'        DelayTimer.Stop()

'        ' التحقق من قيمة Tag
'        If Me.Tag Is Nothing Then
'            MessageBox.Show("يرجى تحديد وضع النموذج (Tag = 1 للمستعرض، Tag = 2 للأرشيف)", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Me.Close()
'            Return
'        End If

'        ' تحديد وضع النموذج بناءً على Tag
'        isArchiveMode = (Me.Tag.ToString() = "2")

'        ' التحقق من صحة النص المدخل
'        If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'            Dim errorMsg As String
'            If isArchiveMode Then
'                errorMsg = "الرجاء إدخال قيمة للبحث في مربع البحث لاختيار ملف للأرشيف"
'            Else
'                errorMsg = "الرجاء إدخال قيمة للبحث في مربع البحث لعرض الملفات"
'            End If
'            TxtMsg.Text = errorMsg
'            Return
'        End If

'        ' تهيئة العناصر
'        InitializeSearchUI()

'        ' إعداد مصدر الإلغاء
'        cts = New Threading.CancellationTokenSource()
'        Dim token = cts.Token

'        Try
'            ' تنفيذ عملية البحث
'            Dim results = Await Task.Run(Function() PerformSearch(searchTextBox.Text, token), token)
'            UpdateUIWithResults(results)
'        Catch ex As OperationCanceledException
'            ' تم إلغاء البحث
'        Catch ex As Exception
'            MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        Finally
'            ' إعادة ضبط حالة الأزرار والعناصر
'            ResetUI()
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تهيئة واجهة البحث قبل البدء
'    ''' </summary>
'    Private Sub InitializeSearchUI()
'        resultsTreeView.Nodes.Clear()
'        iconsImageList.Images.Clear()
'        ProgressBar1.Value = 0
'        ProgressBar1.Visible = True
'        progressLabel.Visible = True
'        progressLabel.Text = "0 %"
'        cancelButton.Enabled = True
'    End Sub

'    ''' <summary>
'    ''' تحديث واجهة المستخدم بنتائج البحث
'    ''' </summary>
'    ''' <param name="results">قائمة بنتائج البحث</param>
'    Private Sub UpdateUIWithResults(results As List(Of Tuple(Of String, String)))
'        ' تنظيف TreeView والـ ImageList
'        iconsImageList.Images.Clear()
'        resultsTreeView.Nodes.Clear()

'        ' إضافة أيقونات افتراضية للمجلدات والملفات
'        Dim folderIcon As Icon = SystemIcons.WinLogo ' أيقونة المجلد (يمكنك تخصيصها)
'        Dim fileIcon As Icon = SystemIcons.Application ' أيقونة الملف (يمكنك تخصيصها)
'        iconsImageList.Images.Add("folder", folderIcon.ToBitmap())
'        iconsImageList.Images.Add("file", fileIcon.ToBitmap())

'        ' تجميع النتائج في TreeView
'        For Each result In results
'            Dim fileName = result.Item1
'            Dim filePath = result.Item2

'            ' التحقق إن كان المسار ملفًا (لأن البحث يبحث عن الملفات فقط)
'            If File.Exists(filePath) Then
'                ' استخراج أيقونة مخصصة
'                Dim customIcon = Icon.ExtractAssociatedIcon(filePath)?.ToBitmap()
'                Dim iconKey = "file"

'                If customIcon IsNot Nothing Then
'                    iconsImageList.Images.Add(filePath, customIcon)
'                    iconKey = filePath ' استخدام الأيقونة الخاصة
'                End If

'                ' إنشاء عقدة
'                Dim node = New TreeNode(fileName) With {
'                    .Tag = filePath,
'                    .ImageKey = iconKey,
'                    .SelectedImageKey = iconKey
'                }
'                resultsTreeView.Nodes.Add(node)
'            End If
'        Next

'        ' تحديد العقدة الأولى إذا وجدت
'        If resultsTreeView.Nodes.Count > 0 Then
'            resultsTreeView.SelectedNode = resultsTreeView.Nodes(0)
'            OpenSelectedFile(resultsTreeView.SelectedNode, False)
'        End If
'    End Sub

'    ''' <summary>
'    ''' إعادة ضبط واجهة المستخدم بعد انتهاء البحث
'    ''' </summary>
'    Private Sub ResetUI()
'        cancelButton.Enabled = False
'        ProgressBar1.Visible = False
'        progressLabel.Visible = False
'    End Sub
'#End Region

'#Region "التعامل مع الأحداث والواجهة"
'    ''' <summary>
'    ''' معالجة الضغط على زر الطباعة
'    ''' </summary>
'    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
'        If pdfViewer.Url IsNot Nothing Then
'            pdfViewer.ShowPrintDialog()
'        End If
'        btnPrint.Enabled = False
'    End Sub

'''' <summary>
'''' معالجة تغيير النص في مربع البحث
'''' </summary>
'Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
'    If String.IsNullOrWhiteSpace(searchTextBox.Text) Then
'        resultsTreeView.Nodes.Clear()
'        Dim msg As String = If(isArchiveMode,
'                               "الرجاء إدخال قيمة للبحث في مربع البحث لاختيار ملف للأرشيف",
'                               "الرجاء إدخال قيمة للبحث في مربع البحث لعرض الملفات")
'        TxtMsg.Text = msg
'    Else
'        ' إعادة تشغيل Timer عند تغيير النص
'        DelayTimer.Stop()
'        DelayTimer.Start()
'        TxtMsg.Text = "جاري البحث..."
'    End If
'End Sub

'    ''' <summary>
'    ''' معالجة تحميل النموذج
'    ''' </summary>
'    Private Sub CustomSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        ' إعدادات Timer
'        DelayTimer.Interval = delayTime

'        ' تحديد وضع النموذج بناءً على الـ Tag
'        isArchiveMode = (Me.Tag IsNot Nothing AndAlso Me.Tag.ToString() = "2")

'        ' تعيين تركيز مربع البحث
'        searchTextBox.Select()
'        btnOpenLoction.Enabled = False
'        btnPrint.Enabled = False

'        ' إعداد WebBrowser
'        pdfViewer.ScriptErrorsSuppressed = True
'        pdfViewer.IsWebBrowserContextMenuEnabled = False

'        ' عرض رسالة توجيهية حسب الوضع
'        If isArchiveMode Then
'            TxtMsg.Text = "الرجاء إدخال قيمة للبحث في مربع البحث لاختيار ملف للأرشيف"
'        Else
'            TxtMsg.Text = "الرجاء إدخال قيمة للبحث في مربع البحث لعرض الملفات"
'        End If
'    End Sub

'    ''' <summary>
'    ''' معالجة الضغط على زر الإغلاق
'    ''' </summary>
'    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
'        Me.Close()
'    End Sub

'    ''' <summary>
'    ''' معالجة الضغط على زر فتح الموقع
'    ''' </summary>
'    Private Sub BtnOpenLoction_Click(sender As Object, e As EventArgs) Handles btnOpenLoction.Click
'        If resultsTreeView.SelectedNode IsNot Nothing AndAlso
'           resultsTreeView.SelectedNode.Tag IsNot Nothing AndAlso
'           File.Exists(resultsTreeView.SelectedNode.Tag.ToString()) Then

'            Dim filePath = resultsTreeView.SelectedNode.Tag.ToString()
'            Dim folderPath = Path.GetDirectoryName(filePath)

'            Try
'                Process.Start("explorer.exe", $"/select,""{filePath}""")
'            Catch ex As Exception
'                MessageBox.Show($"فشل في فتح الموقع: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            End Try
'        End If
'    End Sub

'    ''' <summary>
'    ''' معالجة عرض النموذج
'    ''' </summary>
'    Private Sub CustomSearch_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
'        searchTextBox.Focus()
'    End Sub

'    Private Sub btnPath_Click(sender As Object, e As EventArgs) Handles btnPath.Click
'        Dim frm As New SettingsForm
'        frm.ShowDialog()
'    End Sub
'#End Region
'End Class


