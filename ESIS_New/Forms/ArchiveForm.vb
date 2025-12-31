' ArchiveForm.vb - نموذج إدارة الأرشيف

Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports PdfiumViewer



Public Class ArchiveForm : Inherits BaseFormWithPermissions
    '========================================================
    ' PdfiumViewer Preview (بديل WebBrowser بدون تغيير التصميم)
    '========================================================
    Private _previewPicture As PictureBox
    Private _previewMessage As Label

    Private Sub InitPreviewSurface()
        Try
            If _previewPicture IsNot Nothing OrElse _previewMessage IsNot Nothing Then Return

            _previewPicture = New PictureBox() With {
                .Dock = DockStyle.Fill,
                .SizeMode = PictureBoxSizeMode.Zoom,
                .Visible = False
            }

            _previewMessage = New Label() With {
                .Dock = DockStyle.Fill,
                .TextAlign = ContentAlignment.MiddleCenter,
                .Visible = False,
                .BackColor = Color.White,
                .ForeColor = Color.Red,
                .Font = New Font("Cairo", 12.0F, FontStyle.Bold)
            }

            ' نفس مكان الـ WebBrowser1 (داخل GroupBox3) بدون تعديل التصميم
            Me.GroupBox3.Controls.Add(_previewPicture)
            Me.GroupBox3.Controls.Add(_previewMessage)

            pdfViewer.BringToFront()
        Catch
        End Try
    End Sub

    Private Sub ShowPreviewMessage(msg As String)
        Try
            If _previewMessage Is Nothing Then Return
            _previewMessage.Text = msg
            _previewMessage.Visible = True
            _previewMessage.BringToFront()
        Catch
        End Try
    End Sub

    Private Sub HidePreviewMessage()
        Try
            If _previewMessage Is Nothing Then Return
            _previewMessage.Visible = False
            _previewMessage.Text = ""
        Catch
        End Try
    End Sub

    Private Sub ClearPreview()
        Try
            HidePreviewMessage()

            If _previewPicture IsNot Nothing Then
                Dim oldImg = _previewPicture.Image
                _previewPicture.Image = Nothing
                _previewPicture.Visible = False
                If oldImg IsNot Nothing Then oldImg.Dispose()
            End If

            If pdfViewer IsNot Nothing AndAlso pdfViewer.Document IsNot Nothing Then
                pdfViewer.Document.Dispose()
                pdfViewer.Document = Nothing
            End If
        Catch
        End Try
    End Sub

    Private Sub PreviewFile(pathOrUrl As String)
        If String.IsNullOrWhiteSpace(pathOrUrl) Then
            ClearPreview()
            ShowPreviewMessage("لا يوجد مسار للعرض")
            Return
        End If

        If pathOrUrl.Equals("about:blank", StringComparison.OrdinalIgnoreCase) Then
            ClearPreview()
            Return
        End If

        Try
            If Not IO.File.Exists(pathOrUrl) Then
                ClearPreview()
                ShowPreviewMessage("الملف غير موجود")
                Return
            End If

            Dim ext As String = IO.Path.GetExtension(pathOrUrl).ToLowerInvariant()
            HidePreviewMessage()

            ' PDF
            If ext = ".pdf" Then
                If _previewPicture IsNot Nothing Then
                    Dim oldImg = _previewPicture.Image
                    _previewPicture.Image = Nothing
                    _previewPicture.Visible = False
                    If oldImg IsNot Nothing Then oldImg.Dispose()
                End If

                If pdfViewer IsNot Nothing Then
                    If pdfViewer.Document IsNot Nothing Then
                        pdfViewer.Document.Dispose()
                        pdfViewer.Document = Nothing
                    End If
                    pdfViewer.Document = PdfDocument.Load(pathOrUrl)
                    pdfViewer.BringToFront()
                End If
                Return
            End If

            ' صور (اختياري داخل نفس العارض)
            If ext = ".png" OrElse ext = ".jpg" OrElse ext = ".jpeg" OrElse ext = ".bmp" OrElse ext = ".gif" OrElse ext = ".tif" OrElse ext = ".tiff" Then
                ClearPreview()
                If _previewPicture IsNot Nothing Then
                    Using img As Image = Image.FromFile(pathOrUrl)
                        _previewPicture.Image = CType(img.Clone(), Image)
                    End Using
                    _previewPicture.Visible = True
                    _previewPicture.BringToFront()
                Else
                    Process.Start(New ProcessStartInfo(pathOrUrl) With {.UseShellExecute = True})
                End If
                Return
            End If

            ' أي نوع آخر افتحه خارجيًا
            ClearPreview()
            Process.Start(New ProcessStartInfo(pathOrUrl) With {.UseShellExecute = True})

        Catch
            ClearPreview()
            ShowPreviewMessage("خطأ في عرض الملف")
        End Try
    End Sub

    Private Sub ArchiveForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        ' تنظيف لتفادي أخطاء System.Drawing عند الإغلاق
        ClearPreview()
        Try
            If _previewPicture IsNot Nothing Then _previewPicture.Dispose()
            If _previewMessage IsNot Nothing Then _previewMessage.Dispose()
        Catch
        End Try
    End Sub
#Region "المتغيرات والتهيئة الأساسية"
    ' تعريف المتغيرات
    Private archiveClass As New ArchiveClass()
    Private scaler As ControlScaler
    Public Property SelectedArchiveID As Integer

    ' زر مخفي للإضافة التلقائية
    Friend WithEvents btnAutoAdd As New Button With {.Visible = False}
    ' تعريف الحدث DataSelected
    Public Event DataSelected(filePath As String, fileName As String, fileExtension As String)    ' هيكل لحفظ البيانات المنقولة من فورم الصادر


    Public Structure SaderDataStructure
        Public NumberBook As String      ' رقم الكتاب
        Public DateBook As Date          ' تاريخ الكتاب  
        Public SubjBook As String        ' موضوع الكتاب
        Public WillMaster As String      ' رقم البوليصة الرئيسية
        Public WillSub As String         ' رقم البوليصة الفرعية
        Public BookCategory As String    ' تصنيف الكتاب (صادر، وارد، مذكرات)
    End Structure

    Public saderData As SaderDataStructure
    Public isDataFromSader As Boolean = False
    Public isLoadingData As Boolean = False

    Private bookCategories As List(Of String) = New List(Of String)()



    Private Sub ArchiveForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ' التحقق من أن الكائن scaler مهيأ قبل استدعائه
        If scaler IsNot Nothing Then
            scaler.ScaleControls(Me)
        End If
    End Sub


    Private Sub ArchiveForm_DataSelected(filePath As String, fileName As String, fileExtension As String) Handles Me.DataSelected
        Try
            ' التحقق من أن البيانات تم تعبئتها
            If Not String.IsNullOrEmpty(txtArchiveNum.Text) AndAlso Not String.IsNullOrEmpty(txtArchiveSubject.Text) Then
                ' استخدام BeginInvoke لتأخير التركيز
                Me.BeginInvoke(Sub()
                                   ' وضع التركيز على زر الإضافة
                                   btnAdd.Select()
                               End Sub)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التركيز على زر الإضافة: {ex.Message}")
        End Try
    End Sub



    ''' <summary>
    ''' دالة البناء الافتراضية
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        isDataFromSader = False
        ' تهيئة الكلاس مع النموذج الحالي
        scaler = New ControlScaler(Me)
    End Sub

    ''' <summary>
    ''' دالة البناء الجديدة لاستقبال البيانات من فورم الصادر مع التصنيف
    ''' </summary>
    Public Sub New(numberBook As String, dateBook As Date, subjBook As String, willMaster As String, willSub As String, bookCategory As String)
        InitializeComponent()
        isDataFromSader = True
        ClearFields()
        ' حفظ البيانات في هيكل البيانات
        saderData.NumberBook = numberBook
        saderData.DateBook = dateBook
        saderData.SubjBook = subjBook
        saderData.WillMaster = willMaster
        saderData.WillSub = willSub
        saderData.BookCategory = bookCategory ' حفظ التصنيف
    End Sub
#End Region

#Region "الدوال الأساسية للنموذج"

    ''' <summary>
    ''' تحميل البيانات عند تحميل النموذج
    ''' </summary>
    Private Sub ArchiveForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            InitPreviewSurface()
            ' التحقق من صلاحية عرض نموذج المستخدمين
            If Not CheckPermission("View", "عرض نموذج إدارة الارشيف") Then
                MessageBox.Show("ليس لديك صلاحية عرض نموذج صلاحيات إدارة الأرشيف. يرجى الاتصال بمدير النظام.",
                              "صلاحيات محدودة",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information)
                Me.Close()
                Return
            End If

            isLoadingData = True
            dgvArchive.ClearSelection()
            btnAdd.Text = "إضافة"
            ' إذا كانت البيانات من فورم الصادر، قم بتعبئتها
            If isDataFromSader Then
                FillFieldsFromSader()
                'LoadBookCategories()
            End If

            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            ' إنشاء مجلدات التصنيف عند تحميل الفورم
            archiveClass.CreateCategoryFolders()

            LoadActiveArchive()
            ' تطبيق الصلاحيات عند تحميل النموذج

            'LoadArchive()
            LoadFileTypes()
            dtpArchiveDate.Value = DateTime.Now
            dtpStartDate.Value = DateTime.Now.AddDays(-30)
            dtpEndDate.Value = DateTime.Now
            ' تهيئة العناصر
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            dtpArchiveDate.Value = DateTime.Now
            txtUserName.Text = GlobalData.LoggedInUserName
            txtUserName.ReadOnly = True
            'fillCombo(ComboArchiveFolder, "SELECT DISTINCT SUBSTRING(ArchiveFolder, 1, CHARINDEX('\', ArchiveFolder + '\') - 1) AS MainFolder FROM TbArchive WHERE ArchiveFolder IS NOT NULL ORDER BY MainFolder", "MainFolder")
            FillComboBox(ComboArchiveFolder, "SELECT ArchiveFolder FROM TbArchive ORDER BY ArchiveFolder", "ArchiveFolder")
            ' الاشتراك في الحدث
            AddHandler DataHelper.ArchiveCountChanged, AddressOf HandleDataCountChanged
            AddHandler DataHelper.FileSelectedForArchive, AddressOf HandleFileSelectedForArchive
            AddHandler DataHelper.DeletedArchiveCountChanged, AddressOf HandleDeletedArchiveCountChanged
            ' تحديث العداد في الواجهة
            UpdateRecordCount()
            UpdateDeletedRecordCount()
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل فورم الأرشيف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            isLoadingData = False
        End Try
    End Sub

    ' معالج الضغط على الزر التلقائي
    ''' <summary>
    ''' معالج الضغط على الزر التلقائي
    ''' </summary>
    Private Sub btnAutoAdd_Click(sender As Object, e As EventArgs) Handles btnAutoAdd.Click
        ' التحقق من الصلاحيات
        ' التحقق من صلاحية الإضافة
        If Not CheckPermission(1, "إضافة ملف صادر") Then
            Return
        End If

        Try
            If ValidateBeforeAdd() Then
                ' التأكد من حساب مسار الوجهة قبل الإضافة
                CalculateDestinationPath()

                Dim newId As Integer = archiveClass.AddArchive(
                txtArchiveNum.Text.Trim(),
                txtArchiveSubject.Text.Trim(),
                ComboArchiveFolder.Text.Trim(),
                dtpArchiveDate.Value,
                txtArchiveWillMaster.Text.Trim(),
                txtArchiveWillSub.Text.Trim(),
                txtArchiveNotes.Text.Trim(),
                txtFilename.Text.Trim(),
                txtFileType.Text.Trim(),
                txtFilepath.Text.Trim(),
                txtOriginalPath.Text.Trim(),
                txtUserName.Text.Trim()
            )

                If newId > 0 Then
                    MessageBox.Show("تم إضافة الأرشيف بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ' تحديث العدد بعد الإضافة الناجحة
                    DataHelper.UpdateArchiveCount()

                    ' إذا كانت البيانات من فورم الصادر، قم بتحديث معرف الأرشيف في جدول الصادر
                    If isDataFromSader AndAlso Not String.IsNullOrEmpty(saderData.NumberBook) Then
                        Try
                            Dim saderClass As New SaderClass()
                            If saderClass.UpdateArchiveIDByNumberBook(saderData.NumberBook, newId) Then
                                MessageBox.Show($"تم ربط الأرشيف رقم {newId} بالصادر رقم {saderData.NumberBook} بنجاح", "ربط الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                ' تحديث فورم الصادر إذا كان مفتوحًا
                                For Each form As Form In Application.OpenForms
                                    If TypeOf form Is SaderForm Then
                                        DirectCast(form, SaderForm).RefreshData()
                                        DirectCast(form, SaderForm).SetArchiveIdFromArchiveForm(newId)
                                        Exit For
                                    End If
                                Next
                            End If
                        Catch ex As Exception
                            MessageBox.Show($"تم إضافة الأرشيف بنجاح ولكن فشل في ربطه بالصادر: {ex.Message}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    End If

                    LoadActiveArchive()
                    ClearFields()

                    ' عرض رسالة للسؤال عن ربط الأرشيف بالصادر
                    'Dim result As DialogResult = MessageBox.Show("هل تريد فتح نموذج الصادر وربط هذا الأرشيف به؟", "ربط الأرشيف بالصادر", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    'If result = DialogResult.Yes Then
                    ' البحث عن نموذج الصادر المفتوح
                    Dim saderFormFound As Boolean = False
                    For Each form As Form In Application.OpenForms
                        If TypeOf form Is SaderForm Then
                            ' إذا كان النموذج مفتوحًا، قم بتحديث البيانات
                            DirectCast(form, SaderForm).RefreshData()
                            DirectCast(form, SaderForm).SetArchiveIdFromArchiveForm(newId)
                            form.BringToFront()
                            saderFormFound = True
                            Exit For
                        End If
                    Next

                    ' إذا لم يكن النموذج مفتوحًا، قم بفتحه
                    If Not saderFormFound Then
                        Dim saderForm As New SaderForm()
                        saderForm.Show()
                        saderForm.RefreshData()
                        saderForm.SetArchiveIdFromArchiveForm(newId)
                    End If


                    ' إذا لم يكن النموذج مفتوحًا، قم بفتحه
                    If Not saderFormFound Then
                        Dim saderForm As New SaderForm()
                        saderForm.Show()
                        saderForm.RefreshData()
                        saderForm.SetArchiveIdFromArchiveForm(newId)
                    End If
                    'End If

                    Me.Close()
                Else
                    MessageBox.Show("فشل في إضافة الأرشيف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في إضافة الأرشيف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالج الحدث لتغيير العدد
    ''' </summary>
    Private Sub HandleDataCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
        ' تأكد أن التحديث لجدول الأرشيف
        If e.TableName = "Archive" Then
            UpdateRecordCount()
        End If
    End Sub

    ''' <summary>
    ''' دالة لتحديث تسمية العدد
    ''' </summary>
    Private Sub UpdateRecordCount()
        Dim count As Integer = DataHelper.GetCurrentArchiveCount()
        lblRecordCount.Text = $"عدد السجلات في الأرشيف: {count}"
    End Sub


    ''' <summary>
    ''' تحميل البيانات إلى جدول العرض
    ''' </summary>
    Public Sub LoadActiveArchive()
        Try
            isLoadingData = True
            Dim DT As DataTable = archiveClass.GetActiveArchives()
            dgvArchive.DataSource = DT

            ' تخصيص أسماء الأعمدة
            If dgvArchive.Columns.Contains("ID") Then dgvArchive.Columns("ID").HeaderText = "المعرف"
            If dgvArchive.Columns.Contains("ArchiveNum") Then dgvArchive.Columns("ArchiveNum").HeaderText = "الرقم"
            If dgvArchive.Columns.Contains("ArchiveDate") Then dgvArchive.Columns("ArchiveDate").HeaderText = "التاريخ"
            If dgvArchive.Columns.Contains("ArchiveSubject") Then dgvArchive.Columns("ArchiveSubject").HeaderText = "الموضوع "
            If dgvArchive.Columns.Contains("ArchiveFolder") Then dgvArchive.Columns("ArchiveFolder").HeaderText = "المجلد"
            If dgvArchive.Columns.Contains("FileName") Then dgvArchive.Columns("FileName").HeaderText = "اسم الملف"
            If dgvArchive.Columns.Contains("FilePath") Then dgvArchive.Columns("FilePath").HeaderText = "مسار الملف"
            If dgvArchive.Columns.Contains("ArchiveWillMaster") Then dgvArchive.Columns("ArchiveWillMaster").HeaderText = "الرئيسية"
            If dgvArchive.Columns.Contains("ArchiveWillSub") Then dgvArchive.Columns("ArchiveWillSub").HeaderText = "الفرعية"
            If dgvArchive.Columns.Contains("ArchiveNotes") Then dgvArchive.Columns("ArchiveNotes").HeaderText = "الملاحظات"
            If dgvArchive.Columns.Contains("UserName") Then dgvArchive.Columns("UserName").HeaderText = "المستخدم"
            If dgvArchive.Columns.Contains("CreatedDate") Then dgvArchive.Columns("CreatedDate").HeaderText = "تاريخ الإنشاء"
            If dgvArchive.Columns.Contains("SaderID") Then dgvArchive.Columns("SaderID").HeaderText = "معرف الصادر"


            ' تعديل حجم الاعمدة
            If dgvArchive.Columns.Contains("ID") Then dgvArchive.Columns("ID").Width = 50
            If dgvArchive.Columns.Contains("ArchiveNum") Then dgvArchive.Columns("ArchiveNum").Width = 100
            If dgvArchive.Columns.Contains("ArchiveDate") Then dgvArchive.Columns("ArchiveDate").Width = 100
            If dgvArchive.Columns.Contains("ArchiveSubject") Then dgvArchive.Columns("ArchiveSubject").Width = 250
            If dgvArchive.Columns.Contains("ArchiveFolder") Then dgvArchive.Columns("ArchiveFolder").Width = 200
            If dgvArchive.Columns.Contains("FileName") Then dgvArchive.Columns("FileName").Width = 200
            If dgvArchive.Columns.Contains("ArchiveWillMaster") Then dgvArchive.Columns("ArchiveWillMaster").Width = 120
            If dgvArchive.Columns.Contains("ArchiveWillSub") Then dgvArchive.Columns("ArchiveWillSub").Width = 120
            If dgvArchive.Columns.Contains("SaderID") Then dgvArchive.Columns("SaderID").Width = 120

            ' إخفاء عمود المعرف ومسار الملف
            If dgvArchive.Columns.Contains("FileType") Then dgvArchive.Columns("FileType").Visible = False
            'If dgvArchive.Columns.Contains("ID") Then dgvArchive.Columns("ID").Visible = False
            If dgvArchive.Columns.Contains("FilePath") Then dgvArchive.Columns("FilePath").Visible = False
            'If dgvArchive.Columns.Contains("SaderID") Then dgvArchive.Columns("SaderID").Visible = False
            If dgvArchive.Columns.Contains("OriginalFilePath") Then dgvArchive.Columns("OriginalFilePath").Visible = False
            If dgvArchive.Columns.Contains("NumericID") Then dgvArchive.Columns("NumericID").Visible = False
            If dgvArchive.Columns.Contains("IsDeleted") Then dgvArchive.Columns("IsDeleted").Visible = False
            If dgvArchive.Columns.Contains("DeletedDate") Then dgvArchive.Columns("DeletedDate").Visible = False
            If dgvArchive.Columns.Contains("DeletedBy") Then dgvArchive.Columns("DeletedBy").Visible = False

            dgvArchive.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            ' تحديث العداد
            lblRecordCount.Text = $"عدد السجلات في الأرشيف: {dgvArchive.Rows.Count}"
            UpdateRecordCount()
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات الأرشيف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            isLoadingData = False
        End Try
    End Sub

    ''' <summary>
    ''' تحميل أنواع الملفات إلى ComboBox
    ''' </summary>
    Private Sub LoadFileTypes()
        Try
            ' مسح العناصر الموجودة
            cmbFileType.Items.Clear()
            ' إضافة خيار "الكل" في البداية
            cmbFileType.Items.Add("الكل")
            ' جلب أنواع الملفات من قاعدة البيانات
            Dim dt As DataTable = archiveClass.GetDistinctFileTypes()
            ' إضافة أنواع الملفات إلى ComboBox
            For Each row As DataRow In dt.Rows
                If Not IsDBNull(row("FileType")) AndAlso Not String.IsNullOrEmpty(row("FileType").ToString()) Then
                    Dim fileType As String = row("FileType").ToString().Trim()
                    If Not cmbFileType.Items.Contains(fileType) Then
                        cmbFileType.Items.Add(fileType)
                    End If
                End If
            Next
            ' تعيين "الكل" كاختيار افتراضي
            cmbFileType.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل أنواع الملفات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub LoadPDFViewer()
        Try
            If Not String.IsNullOrEmpty(txtOriginalPath.Text) AndAlso IO.File.Exists(txtOriginalPath.Text) Then
                PreviewFile(txtOriginalPath.Text)
            End If
        Catch ex As Exception
            MessageBox.Show("تعذر عرض الملف: " & ex.Message)
        End Try
    End Sub


    '''' <summary>
    '''' دالة لتعبئة الحقول من بيانات فورم الصادر
    '''' </summary>
    'Private Sub FillFieldsFromSader()
    '    Try
    '        ClearAllFields()
    '        ' تعبئة البيانات من Structure
    '        txtArchiveNum.Text = saderData.NumberBook
    '        dtpArchiveDate.Value = saderData.DateBook
    '        txtArchiveSubject.Text = saderData.SubjBook
    '        txtArchiveWillMaster.Text = saderData.WillMaster
    '        txtArchiveWillSub.Text = saderData.WillSub

    '        ' اقتراح مجلد الأرشيف بناءً على التصنيف مع إضافة التاريخ الشهري
    '        Dim currentMonth As String = DateTime.Now.ToString("yyyy-MM")
    '        Select Case saderData.BookCategory.ToLower().Trim()
    '            Case "صادر", "outgoing"
    '                ComboArchiveFolder.Text = $"صادر\{currentMonth}"
    '            Case "وارد", "incoming"
    '                ComboArchiveFolder.Text = $"وارد\{currentMonth}"
    '            Case "مذكرات", "memos"
    '                ComboArchiveFolder.Text = $"مذكرات\{currentMonth}"
    '            Case "قرارات", "decisions"
    '                ComboArchiveFolder.Text = $"قرارات\{currentMonth}"
    '            Case "تقارير", "reports"
    '                ComboArchiveFolder.Text = $"تقارير\{currentMonth}"
    '            Case Else
    '                ComboArchiveFolder.Text = $"صادر\{currentMonth}" ' افتراضي
    '        End Select

    '        ' حساب مسار الوجهة بعد تعيين التصنيف
    '        CalculateDestinationPath()

    '        ' تعبئة الحقول الافتراضية
    '        FillDefaultFields()

    '        ' رسالة تأكيد (يمكن إزالتها لاحقاً)
    '        ' MessageBox.Show($"تم تعبئة البيانات من فورم الصادر بنجاح{vbNewLine}" &
    '        '     $"التصنيف: {saderData.BookCategory}{vbNewLine}" &
    '        '     $"المجلد المقترح: {ComboArchiveFolder.Text}",
    '        '     "تعبئة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Information)

    '    Catch ex As Exception
    '        MessageBox.Show("خطأ في تعبئة البيانات من فورم الصادر: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' دالة لتعبئة الحقول من بيانات فورم الصادر
    '''' </summary>
    'Private Sub LoadBookCategories()
    '    Try
    '        ' تحديد مسار الملف (يمكنك تعديله حسب موقع الملف)
    '        Dim filePath As String = Path.Combine(Application.StartupPath, "BookCategories.txt")

    '        ' التحقق من وجود الملف
    '        If Not File.Exists(filePath) Then
    '            ' إذا لم يكن الملف موجودًا، قم بإنشائه مع القيم الافتراضية
    '            Dim defaultCategories As String() = {"صادر", "وارد", "مذكرات", "قرارات", "تقارير"}
    '            File.WriteAllLines(filePath, defaultCategories)
    '        End If

    '        ' قراءة التصنيفات من الملف
    '        Dim categories As String() = File.ReadAllLines(filePath)

    '        ' تنظيف القائمة الحالية وإضافة التصنيفات الجديدة
    '        ComboArchiveFolder.Items.Clear()
    '        For Each category As String In categories
    '            ' تجاهل الأسطر الفارغة
    '            If Not String.IsNullOrWhiteSpace(category) Then
    '                ComboArchiveFolder.Items.Add(category.Trim())
    '            End If
    '        Next

    '        ' تعيين القيمة الافتراضية (إذا كانت هناك تصنيفات)
    '        If ComboArchiveFolder.Items.Count > 0 Then
    '            ComboArchiveFolder.SelectedIndex = 0
    '        End If

    '        ' حفظ التصنيفات في المتغير
    '        bookCategories = categories.Where(Function(c) Not String.IsNullOrWhiteSpace(c)).Select(Function(c) c.Trim()).ToList()

    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ أثناء تحميل تصنيفات الكتاب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    Private Sub FillFieldsFromSader()
        Try
            ClearAllFields()
            ' تعبئة البيانات من Structure
            txtArchiveNum.Text = saderData.NumberBook
            dtpArchiveDate.Value = saderData.DateBook
            txtArchiveSubject.Text = saderData.SubjBook
            txtArchiveWillMaster.Text = saderData.WillMaster
            txtArchiveWillSub.Text = saderData.WillSub
            'ComboArchiveFolder.Text = saderData.BookCategory

            ' اقتراح مجلد الأرشيف بناءً على التصنيف مع إضافة التاريخ الشهري
            Dim currentMonth As String = DateTime.Now.ToString("yyyy-MM")
            Dim selectedCategory As String = saderData.BookCategory.ToLower().Trim()

            ' البحث عن التصنيف في القائمة
            Dim matchedCategory As String = bookCategories.FirstOrDefault(Function(c) c.ToLower().Trim() = selectedCategory)

            If Not String.IsNullOrEmpty(matchedCategory) Then
                'ComboArchiveFolder.Text = selectedCategory & "\" & currentMonth
            Else
                ' إذا لم يتم العثور على التصنيف، استخدم "صادر" كافتراضي
                ComboArchiveFolder.Text = selectedCategory & "\" & currentMonth
                'ComboArchiveFolder.Text = $"صادر\{currentMonth}"
            End If

            ' حساب مسار الوجهة بعد تعيين التصنيف
            CalculateDestinationPath()

            ' تعبئة الحقول الافتراضية
            FillDefaultFields()

            ' رسالة تأكيد (يمكن إزالتها لاحقاً)
            MessageBox.Show($"تم تعبئة البيانات من فورم الصادر بنجاح{vbNewLine}" &
                $"التصنيف: {saderData.BookCategory}{vbNewLine}" &
                $"المجلد المقترح: {ComboArchiveFolder.Text}",
                "تعبئة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("خطأ في تعبئة البيانات من فورم الصادر: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تفريغ جميع الحقول
    ''' </summary>
    Private Sub ClearAllFields()
        Try
            txtArchiveNum.Clear()
            txtArchiveSubject.Clear()
            ComboArchiveFolder.SelectedIndex = 0
            txtFilename.Clear()
            txtFileType.Clear()
            txtFilepath.Clear()
            txtOriginalPath.Clear()
            txtArchiveWillMaster.Clear()
            txtArchiveWillSub.Clear()
            txtArchiveNotes.Clear()
            dtpArchiveDate.Value = DateTime.Now
            ' تفريغ WebBrowser
            If pdfViewer IsNot Nothing Then
                PreviewFile("about:blank")
            End If
        Catch ex As Exception
            ' تجاهل الأخطاء في التفريغ
        End Try
    End Sub
#End Region

#Region "التعامل مع البيانات (إضافة، تعديل، حذف)"
    ''' <summary>
    ''' إضافة أرشيف جديد
    ''' </summary>
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' التحقق من صلاحية الإضافة
        If Not CheckPermission(1, "إضافة ملف أرشيف") Then
            Return
        End If

        Try
            If ValidateInput() Then
                ' استخدام التصنيف الحالي في ComboBox
                Dim archiveFolder As String = ComboArchiveFolder.Text

                ' إذا كان التصنيف لا يحتوي على تاريخ شهري، أضفه
                If Not archiveFolder.Contains("\") Then
                    Dim currentMonth As String = DateTime.Now.ToString("yyyy-MM")
                    archiveFolder = $"{archiveFolder}\{currentMonth}"
                End If

                If ValidateBeforeAdd() Then
                    Dim newId As Integer = archiveClass.AddArchive(
                    txtArchiveNum.Text.Trim(),
                    txtArchiveSubject.Text.Trim(),
                    ComboArchiveFolder.Text,
                    dtpArchiveDate.Value,
                    txtArchiveWillMaster.Text.Trim(),
                    txtArchiveWillSub.Text.Trim(),
                    txtArchiveNotes.Text.Trim,
                    txtFilename.Text.Trim(),
                    txtFileType.Text.Trim(),
                    txtFilepath.Text.Trim(),
                    txtOriginalPath.Text.Trim(),
                    txtUserName.Text.Trim()
                )

                    If newId > 0 Then
                        SavedSuccessfully(Me.lblMessage, Me.PicMsg, Me.TimerMsg)
                        'MessageBox.Show("تم إضافة الأرشيف بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ' تحديث العدد بعد الإضافة الناجحة
                        DataHelper.UpdateArchiveCount()

                        ' إذا كانت البيانات من فورم الصادر، قم بتحديث معرف الأرشيف في جدول الصادر
                        If isDataFromSader AndAlso Not String.IsNullOrEmpty(saderData.NumberBook) Then
                            Try
                                Dim saderClass As New SaderClass()
                                If saderClass.UpdateArchiveIDByNumberBook(saderData.NumberBook, newId) Then
                                    'MessageBox.Show($"تم ربط الأرشيف رقم {newId} بالصادر رقم {saderData.NumberBook} بنجاح", "ربط الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    ' تحديث فورم الصادر إذا كان مفتوحًا
                                    For Each form As Form In Application.OpenForms
                                        If TypeOf form Is SaderForm Then
                                            DirectCast(form, SaderForm).RefreshData()
                                            DirectCast(form, SaderForm).SetArchiveIdFromArchiveForm(newId)
                                            Exit For
                                        End If
                                    Next

                                    ' تحديث عداد السجلات غير المؤرشفة
                                    DataHelper.UpdateUnarchivedSaderCount()

                                    ' ربط الأرشيف بالصادر باستخدام UpdateArchiveSaderId
                                    Dim archiveClass As New ArchiveClass()
                                    If archiveClass.UpdateArchiveSaderId(txtArchiveNum.Text.Trim(), newId) Then
                                        MessageBox.Show($"تم ربط الأرشيف رقم {newId} بالصادر رقم {saderData.NumberBook} بنجاح", "ربط الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    End If

                                End If
                            Catch ex As Exception
                                MessageBox.Show($"تم إضافة الأرشيف بنجاح ولكن فشل في ربطه بالصادر: {ex.Message}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            End Try
                        End If

                        LoadActiveArchive()
                        ClearFields()

                        ''عرض رسالة للسؤال عن ربط الأرشيف بالصادر
                        'Dim result As DialogResult = MessageBox.Show("هل تريد فتح نموذج الصادر وربط هذا الأرشيف به؟", "ربط الأرشيف بالصادر", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        'If result = DialogResult.Yes Then
                        ' البحث عن نموذج الصادر المفتوح
                        Dim saderFormFound As Boolean = False
                        For Each form As Form In Application.OpenForms
                            If TypeOf form Is SaderForm Then
                                ' إذا كان النموذج مفتوحًا، قم بتحديث البيانات
                                DirectCast(form, SaderForm).RefreshData()
                                DirectCast(form, SaderForm).SetArchiveIdFromArchiveForm(newId)
                                form.BringToFront()
                                saderFormFound = True
                                Exit For
                            End If
                        Next

                        ' إذا لم يكن النموذج مفتوحًا، قم بفتحه
                        If Not saderFormFound Then
                            Dim saderForm As New SaderForm()
                            saderForm.Show()
                            saderForm.RefreshData()
                            saderForm.SetArchiveIdFromArchiveForm(newId)
                        End If
                        'End If

                        Me.Close()
                    Else
                        MessageBox.Show("فشل في إضافة الأرشيف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في إضافة الأرشيف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
    '    ' التحقق من صلاحية الإضافة
    '    If Not CheckPermission(1, "إضافة ملف أرشيف") Then
    '        Return
    '    End If

    '    Try
    '        If ValidateInput() Then
    '            ' استخدام التصنيف الحالي في ComboBox
    '            Dim archiveFolder As String = ComboArchiveFolder.Text

    '            ' إذا كان التصنيف لا يحتوي على تاريخ شهري، أضفه
    '            If Not archiveFolder.Contains("\") Then
    '                Dim currentMonth As String = DateTime.Now.ToString("yyyy-MM")
    '                archiveFolder = $"{archiveFolder}\{currentMonth}"
    '            End If

    '            If ValidateBeforeAdd() Then
    '                Dim newId As Integer = archiveClass.AddArchive(
    '                    txtArchiveNum.Text.Trim(),
    '                    txtArchiveSubject.Text.Trim(),
    '                    ComboArchiveFolder.Text,
    '                    dtpArchiveDate.Value,
    '                    txtArchiveWillMaster.Text.Trim(),
    '                    txtArchiveWillSub.Text.Trim(),
    '                    txtArchiveNotes.Text.Trim,
    '                    txtFilename.Text.Trim(),
    '                    txtFileType.Text.Trim(),
    '                    txtFilepath.Text.Trim(),
    '                    txtOriginalPath.Text.Trim(),
    '                    txtUserName.Text.Trim()
    '                )

    '                If newId > 0 Then
    '                    SavedSuccessfully(Me.lblMessage, Me.PicMsg, Me.TimerMsg)
    '                    'MessageBox.Show("تم إضافة الأرشيف بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                    ' تحديث العدد بعد الإضافة الناجحة
    '                    DataHelper.UpdateArchiveCount()

    '                    ' إذا كانت البيانات من فورم الصادر، قم بتحديث معرف الأرشيف في جدول الصادر
    '                    If isDataFromSader AndAlso Not String.IsNullOrEmpty(saderData.NumberBook) Then
    '                        Try
    '                            Dim saderClass As New SaderClass()
    '                            If saderClass.UpdateArchiveIDByNumberBook(saderData.NumberBook, newId) Then
    '                                'MessageBox.Show($"تم ربط الأرشيف رقم {newId} بالصادر رقم {saderData.NumberBook} بنجاح", "ربط الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                                ' تحديث فورم الصادر إذا كان مفتوحًا
    '                                For Each form As Form In Application.OpenForms
    '                                    If TypeOf form Is SaderForm Then
    '                                        DirectCast(form, SaderForm).RefreshData()
    '                                        DirectCast(form, SaderForm).SetArchiveIdFromArchiveForm(newId)
    '                                        Exit For
    '                                    End If
    '                                Next

    '                                ' تحديث عداد السجلات غير المؤرشفة
    '                                DataHelper.UpdateUnarchivedSaderCount()
    '                            End If
    '                        Catch ex As Exception
    '                            MessageBox.Show($"تم إضافة الأرشيف بنجاح ولكن فشل في ربطه بالصادر: {ex.Message}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                        End Try
    '                    End If

    '                    LoadActiveArchive()
    '                    ClearFields()

    '                    ''عرض رسالة للسؤال عن ربط الأرشيف بالصادر
    '                    'Dim result As DialogResult = MessageBox.Show("هل تريد فتح نموذج الصادر وربط هذا الأرشيف به؟", "ربط الأرشيف بالصادر", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
    '                    'If result = DialogResult.Yes Then
    '                    ' البحث عن نموذج الصادر المفتوح
    '                    Dim saderFormFound As Boolean = False
    '                        For Each form As Form In Application.OpenForms
    '                        If TypeOf form Is SaderForm Then
    '                            ' إذا كان النموذج مفتوحًا، قم بتحديث البيانات
    '                            DirectCast(form, SaderForm).RefreshData()
    '                            DirectCast(form, SaderForm).SetArchiveIdFromArchiveForm(newId)
    '                            form.BringToFront()
    '                            saderFormFound = True
    '                            Exit For
    '                        End If
    '                    Next

    '                    ' إذا لم يكن النموذج مفتوحًا، قم بفتحه
    '                    If Not saderFormFound Then
    '                        Dim saderForm As New SaderForm()
    '                        saderForm.Show()
    '                        saderForm.RefreshData()
    '                        saderForm.SetArchiveIdFromArchiveForm(newId)
    '                    End If
    '                    'End If

    '                    Me.Close()
    '                Else
    '                    MessageBox.Show("فشل في إضافة الأرشيف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show("خطأ في إضافة الأرشيف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub



    ''' <summary>
    ''' تعديل أرشيف موجودGetArchiveById
    ''' </summary>
    Sub CloseFileIfInUse(filePath As String)
        Try
            Dim processes As Process() = Process.GetProcessesByName("AcroRd32") ' Adobe Reader
            For Each p As Process In processes
                Dim processPath As String = GetProcessPath(p)
                If processPath?.ToLower().Contains(filePath.ToLower()) Then
                    p.Kill()
                End If
            Next
        Catch
            ' تجاهل الأخطاء
        End Try
    End Sub

    Function GetProcessPath(process As Process) As String
        Try
            Return process.MainModule.FileName
        Catch
            Return Nothing
        End Try
    End Function
    'Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
    '    ' التحقق من صلاحية التعديل
    '    If Not CheckPermission(2, "تعديل ملف أرشيف") Then
    '        Return
    '    End If

    '    ' التحقق من وجود صف محدد
    '    If dgvArchive.SelectedRows.Count = 0 Then
    '        MessageBox.Show("يرجى اختيار أرشيف للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        Return
    '    End If

    '    Try
    '        If dgvArchive.SelectedRows.Count > 0 AndAlso ValidateInputForUpdate() Then
    '            Dim selectedId As Integer
    '            If Integer.TryParse(dgvArchive.SelectedRows(0).Cells("ID").Value?.ToString(), selectedId) Then
    '                Dim success As Boolean = archiveClass.UpdateArchive(
    '                selectedId,
    '                txtArchiveNum.Text.Trim(),
    '                txtArchiveSubject.Text.Trim(),
    '                ComboArchiveFolder.Text.Trim(),
    '                dtpArchiveDate.Value,
    '                txtArchiveWillMaster.Text.Trim(),
    '                txtArchiveWillSub.Text.Trim(),
    '                txtArchiveNotes.Text.Trim(),
    '                txtFilename.Text.Trim(),
    '                txtFileType.Text.Trim(),
    '                txtFilepath.Text.Trim(),
    '                txtOriginalPath.Text.Trim(),
    '                txtUserName.Text.Trim()
    '            )

    '                If success Then
    '                    UpdetedSuccessfully(Me.lblMessage, Me.PicMsg, Me.TimerMsg)
    '                    'MessageBox.Show("تم تحديث الأرشيف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)




    '                    LoadActiveArchive()
    '                    If File.Exists(txtFilepath.Text) Then
    '                        PreviewFile(txtFilepath.Text)
    '                    Else
    '                        PreviewFile("about:blank")
    '                    End If
    '                    ClearFields()
    '                Else
    '                    MessageBox.Show("لم يتم إجراء اي تغييرات على الارشيف المحدد", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                End If
    '            Else
    '                MessageBox.Show("المعرف غير صالح. يرجى اختيار سجل صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub


    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        ' التحقق من صلاحية التعديل
        If Not CheckPermission(2, "تعديل ملف أرشيف") Then
            Return
        End If

        ' التحقق من وجود صف محدد
        If dgvArchive.SelectedRows.Count = 0 Then
            MessageBox.Show("يرجى اختيار أرشيف للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            If dgvArchive.SelectedRows.Count > 0 AndAlso ValidateInputForUpdate() Then
                Dim selectedId As Integer
                If Integer.TryParse(dgvArchive.SelectedRows(0).Cells("ID").Value?.ToString(), selectedId) Then
                    Dim success As Boolean = archiveClass.UpdateArchive(
                    selectedId,
                    txtArchiveNum.Text.Trim(),
                    txtArchiveSubject.Text.Trim(),
                    ComboArchiveFolder.Text.Trim(),
                    dtpArchiveDate.Value,
                    txtArchiveWillMaster.Text.Trim(),
                    txtArchiveWillSub.Text.Trim(),
                    txtArchiveNotes.Text.Trim(),
                    txtFilename.Text.Trim(),
                    txtFileType.Text.Trim(),
                    txtFilepath.Text.Trim(),
                    txtOriginalPath.Text.Trim(),
                    txtUserName.Text.Trim()
                )

                    If success Then
                        UpdetedSuccessfully(Me.lblMessage, Me.PicMsg, Me.TimerMsg)
                        'MessageBox.Show("تم تحديث الأرشيف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' إذا كانت البيانات من فورم الصادر، قم بتحديث معرف الأرشيف في جدول الصادر
                        If isDataFromSader AndAlso Not String.IsNullOrEmpty(saderData.NumberBook) Then
                            Try
                                Dim saderClass As New SaderClass()
                                If saderClass.UpdateArchiveIDByNumberBook(saderData.NumberBook, selectedId) Then
                                    'MessageBox.Show($"تم ربط الأرشيف رقم {selectedId} بالصادر رقم {saderData.NumberBook} بنجاح", "ربط الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)

                                    ' تحديث فورم الصادر إذا كان مفتوحًا
                                    For Each form As Form In Application.OpenForms
                                        If TypeOf form Is SaderForm Then
                                            DirectCast(form, SaderForm).RefreshData()
                                            DirectCast(form, SaderForm).SetArchiveIdFromArchiveForm(selectedId)
                                            Exit For
                                        End If
                                    Next

                                    ' تحديث عداد السجلات غير المؤرشفة
                                    DataHelper.UpdateUnarchivedSaderCount()
                                End If
                            Catch ex As Exception
                                MessageBox.Show($"تم تحديث الأرشيف بنجاح ولكن فشل في ربطه بالصادر: {ex.Message}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            End Try
                        End If

                        LoadActiveArchive()

                        If File.Exists(txtFilepath.Text) Then
                            PreviewFile(txtFilepath.Text)
                        Else
                            PreviewFile("about:blank")
                        End If
                        ClearFields()
                    Else
                        MessageBox.Show("لم يتم إجراء أي تغييرات على الأرشيف المحدد", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Else
                    MessageBox.Show("المعرف غير صالح. يرجى اختيار سجل صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ''' <summary>
    ''' حذف أرشيف
    ''' </summary>
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        ' التحقق من صلاحية الحذف
        If Not CheckPermission(3, "حذف ملف أرشيف") Then
            Return
        End If

        Try
            ' التحقق من وجود صف محدد
            If dgvArchive.SelectedRows.Count = 0 Then
                MessageBox.Show("يرجى اختيار أرشيف للإخفاء", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من وجود عمود ID
            If Not dgvArchive.Columns.Contains("ID") Then
                MessageBox.Show("العمود المطلوب 'ID' غير موجود في جدول البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' تحويل المعرف مع التحقق من القيم الفارغة
            Dim idValue As Object = dgvArchive.SelectedRows(0).Cells("ID").Value
            Dim selectedId As Integer

            ' التحقق من أن القيمة ليست فارغة وقابلة للتحويل
            If idValue Is Nothing OrElse idValue Is DBNull.Value OrElse String.IsNullOrEmpty(idValue.ToString()) Then
                MessageBox.Show("قيمة المعرف غير صالحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' محاولة التحويل مع التعامل مع الأخطاء
            If Not Integer.TryParse(idValue.ToString(), selectedId) Then
                MessageBox.Show("قيمة المعرف ليست رقمية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' الحصول على رقم الأرشيف مع التعامل مع القيم الفارغة
            Dim archiveNum As String = "المعرف " & selectedId

            ' التحقق من وجود عمود ArchiveNum
            If dgvArchive.Columns.Contains("ArchiveNum") Then
                Dim archiveNumValue As Object = dgvArchive.SelectedRows(0).Cells("ArchiveNum").Value

                ' التحقق من أن القيمة ليست فارغة
                If archiveNumValue IsNot Nothing AndAlso archiveNumValue IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(archiveNumValue.ToString()) Then
                    archiveNum = archiveNumValue.ToString()
                End If
            End If

            ' طلب التأكيد
            Dim result As DialogResult = MessageBox.Show(
            $"هل أنت متأكد من رغبتك في إرسال الأرشيف رقم {archiveNum}؟" & vbNewLine &
            "سيتم إرسالة الى سلة المحذوفات ويمكن استرجاعه لاحقًا.",
            "تأكيد الإخفاء",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2)

            If result = DialogResult.Yes Then
                ' التأكد من أن اسم المستخدم معرف
                Dim currentUserName As String = GlobalData.LoggedInUserName
                If String.IsNullOrWhiteSpace(currentUserName) Then
                    currentUserName = "مستخدم غير معروف"
                End If

                ' محاولة إخفاء الأرشيف
                Dim success As Boolean = archiveClass.DeleteArchiveById(selectedId, currentUserName)

                If success Then
                    LoadActiveArchive() ' إعادة تحميل الأرشيفات النشطة
                    ClearFields()
                    DeletedArchiveSuccessfully(Me.lblMessage, Me.PicMsg, Me.TimerMsg)
                    'MessageBox.Show("تم إخفاء الأرشيف بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    DataHelper.UpdateDeletedArchiveCount()
                    DataHelper.UpdateUnarchivedSaderCount()
                Else
                    MessageBox.Show("فشل في إرسال الأرشيف. يرجى المحاولة مرة أخرى.", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Catch ex As Exception
            ' تضمين تفاصيل الخطأ في الرسالة
            MessageBox.Show("حدث خطأ غير متوقع أثناء محاولة إرسال الأرشيف: " & ex.Message &
                        vbNewLine & "التفاصيل التقنية: " & ex.StackTrace,
                        "خطأ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
            System.Diagnostics.Debug.WriteLine($"خطأ في btnHide_Click: {ex.Message}{vbNewLine}{ex.StackTrace}")
        End Try




        '' التحقق من الصلاحيات
        'If Not PermissionHelper.CanDelete() Then
        '    MessageBox.Show("ليس لديك صلاحية الحذف. يرجى الاتصال بمدير النظام.", "صلاحيات محدودة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    Return
        'End If

        'Try
        '    If dgvArchive.SelectedRows.Count > 0 Then
        '        ' استخدام Integer.TryParse لتجنب الأخطاء
        '        Dim selectedId As Integer
        '        If Integer.TryParse(dgvArchive.SelectedRows(0).Cells("ID").Value?.ToString(), selectedId) Then
        '            ' طلب تأكيد الحذف
        '            Dim result As DialogResult = MessageBox.Show(
        '                "هل أنت متأكد من رغبتك في حذف هذا الأرشيف؟",
        '                "تأكيد الحذف",
        '                MessageBoxButtons.YesNo,
        '                MessageBoxIcon.Question,
        '                MessageBoxDefaultButton.Button2)

        '            If result = DialogResult.Yes Then
        '                Dim success As Boolean = archiveClass.DeleteArchiveById(selectedId)

        '                If success Then
        '                    LoadArchive()
        '                    ClearFields()
        '                End If
        '            End If
        '        Else
        '            MessageBox.Show("المعرف غير صالح. يرجى اختيار سجل صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        End If
        '    Else
        '        MessageBox.Show("يرجى اختيار أرشيف للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    End If
        'Catch ex As Exception
        '    MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try




        'Try
        '    If dgvArchive.SelectedRows.Count > 0 Then
        '        ' استخدام Integer.TryParse لتجنب الأخطاء
        '        Dim selectedId As Integer
        '        If Integer.TryParse(dgvArchive.SelectedRows(0).Cells("ID").Value?.ToString(), selectedId) Then
        '            Dim result As Boolean = archiveClass.DeleteArchiveById(selectedId)

        '            If result Then
        '                MessageBox.Show("تم حذف الأرشيف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '                ClearFields()
        '                LoadArchive()
        '            Else
        '                MessageBox.Show("فشل في حذف الأرشيف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            End If
        '        Else
        '            MessageBox.Show("المعرف غير صالح. يرجى اختيار سجل صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        End If
        '    Else
        '        MessageBox.Show("يرجى اختيار أرشيف للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    End If
        'Catch ex As Exception
        '    MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
    End Sub


#End Region

#Region "التحقق من الصحة قبل الإضافة"
    ''' <summary>
    ''' التحقق من الصحة قبل الإضافة
    ''' </summary>
    Private Function ValidateBeforeAdd() As Boolean
        Try
            ' التحقق من جميع المدخلات الأخرى
            If Not ValidateInput() Then
                Return False
            End If

            ' التحقق من وجود مسار الوجهة
            If String.IsNullOrWhiteSpace(txtFilepath.Text) Then
                ' حساب مسار الوجهة إذا لم يتم حسابه بعد
                CalculateDestinationPath()

                ' التحقق مرة أخرى
                If String.IsNullOrWhiteSpace(txtFilepath.Text) Then
                    MessageBox.Show("يرجى تحديد مجلد الأرشيف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    ComboArchiveFolder.Focus()
                    Return False
                End If
            End If

            ' التحقق من وجود الملف في المسار
            If Not ValidateFileBeforeAddition(
                txtFilename.Text.Trim(),
                txtFileType.Text.Trim(),
                ComboArchiveFolder.Text,
                txtOriginalPath.Text.Trim(),
                txtFilepath.Text.Trim()) Then
                Return False
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show($"خطأ في التحقق قبل الإضافة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من صحة المدخلات قبل الإضافة
    ''' </summary>
    Private Function ValidateInput() As Boolean
        ' التحقق من رقم الأرشيف
        If String.IsNullOrWhiteSpace(txtArchiveNum.Text) Then
            MessageBox.Show("يرجى إدخال رقم الأرشيف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtArchiveNum.Focus()
            Return False
        End If

        ' التحقق من تكرار رقم الأرشيف
        If archiveClass.CheckDuplicateArchiveNumber(txtArchiveNum.Text.Trim()) Then
            Dim duplicateInfo As DataTable = archiveClass.GetDuplicateArchiveInfo(txtArchiveNum.Text.Trim())
            If duplicateInfo.Rows.Count > 0 Then
                Dim row As DataRow = duplicateInfo.Rows(0)
                MessageBox.Show($"رقم الأرشيف موجود مسبقًا ولا يمكن تكراره!{vbNewLine}{vbNewLine}" &
                              $"تفاصيل الأرشيف الموجود:{vbNewLine}" &
                              $"رقم الأرشيف: {row("ArchiveNum")}{vbNewLine}" &
                              $"الموضوع: {row("ArchiveSubject")}{vbNewLine}" &
                              $"المجلد: {row("ArchiveFolder")}{vbNewLine}" &
                              $"تاريخ الأرشفة: {Convert.ToDateTime(row("ArchiveDate")).ToShortDateString()}{vbNewLine}" &
                              $"المستخدم: {row("UserName")}{vbNewLine}{vbNewLine}" &
                              "يرجى استخدام رقم أرشيف مختلف.",
                              "رقم أرشيف مكرر", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("رقم الأرشيف موجود مسبقًا ولا يمكن تكراره!", "رقم أرشيف مكرر", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            txtArchiveNum.SelectAll()
            txtArchiveNum.Focus()
            Return False
        End If

        ' التحقق من موضوع الأرشيف
        If String.IsNullOrWhiteSpace(txtArchiveSubject.Text) Then
            MessageBox.Show("يرجى إدخال موضوع الأرشيف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtArchiveSubject.Focus()
            Return False
        End If

        ' التحقق من مجلد الأرشيف
        If String.IsNullOrWhiteSpace(ComboArchiveFolder.Text) Then
            MessageBox.Show("يرجى إدخال مجلد الأرشيف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ComboArchiveFolder.Focus()
            Return False
        End If

        ' التحقق من اسم الملف
        If String.IsNullOrWhiteSpace(txtFilename.Text) Then
            MessageBox.Show("يرجى إدخال اسم الملف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFilename.Focus()
            Return False
        End If

        ' التحقق من نوع الملف
        If String.IsNullOrWhiteSpace(txtFileType.Text) Then
            MessageBox.Show("يرجى إدخال نوع الملف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFileType.Focus()
            Return False
        End If

        ' التحقق من مسار الملف
        If String.IsNullOrWhiteSpace(txtFilepath.Text) Then
            MessageBox.Show("يرجى إدخال مسار الملف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFilepath.Focus()
            Return False
        End If

        ' التحقق من تكرار الملف
        If archiveClass.CheckDuplicateFile(txtFilename.Text.Trim(), txtFileType.Text.Trim(), ComboArchiveFolder.Text.Trim()) Then
            Dim duplicateFileInfo As DataTable = archiveClass.GetDuplicateFileInfo(txtFilename.Text.Trim(), txtFileType.Text.Trim(), ComboArchiveFolder.Text.Trim())
            If duplicateFileInfo.Rows.Count > 0 Then
                Dim row As DataRow = duplicateFileInfo.Rows(0)
                Dim result As DialogResult = MessageBox.Show($"الملف موجود مسبقًا في نفس المسار!{vbNewLine}{vbNewLine}" &
                                                           $"تفاصيل الملف الموجود:{vbNewLine}" &
                                                           $"رقم الأرشيف: {row("ArchiveNum")}{vbNewLine}" &
                                                           $"الموضوع: {row("ArchiveSubject")}{vbNewLine}" &
                                                           $"تاريخ الأرشفة: {Convert.ToDateTime(row("ArchiveDate")).ToShortDateString()}{vbNewLine}" &
                                                           $"المستخدم: {row("UserName")}{vbNewLine}{vbNewLine}" &
                                                           "هل تريد الاستمرار والاستبدال؟",
                                                           "ملف مكرر", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.No Then
                    txtFilename.SelectAll()
                    txtFilename.Focus()
                    Return False
                End If
            End If
        End If

        ' التحقق من اسم المستخدم
        If String.IsNullOrWhiteSpace(txtUserName.Text) Then
            MessageBox.Show("يرجى إدخال اسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUserName.Focus()
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' التحقق من صحة المدخلات قبل التحديث
    ''' </summary>
    Private Function ValidateInputForUpdate() As Boolean
        ' التحقق من رقم الأرشيف
        If String.IsNullOrWhiteSpace(txtArchiveNum.Text) Then
            MessageBox.Show("يرجى إدخال رقم الأرشيف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtArchiveNum.Focus()
            Return False
        End If

        ' التحقق من تكرار رقم الأرشيف عند التحديث
        Dim currentId As Integer = Convert.ToInt32(dgvArchive.SelectedRows(0).Cells("ID").Value)
        If archiveClass.CheckDuplicateArchiveNumberForUpdate(txtArchiveNum.Text.Trim(), currentId) Then
            Dim duplicateInfo As DataTable = archiveClass.GetDuplicateArchiveInfo(txtArchiveNum.Text.Trim())
            If duplicateInfo.Rows.Count > 0 Then
                Dim row As DataRow = duplicateInfo.Rows(0)
                MessageBox.Show($"رقم الأرشيف موجود مسبقًا في سجل آخر ولا يمكن تكراره!{vbNewLine}{vbNewLine}" &
                              $"تفاصيل الأرشيف الموجود:{vbNewLine}" &
                              $"رقم الأرشيف: {row("ArchiveNum")}{vbNewLine}" &
                              $"الموضوع: {row("ArchiveSubject")}{vbNewLine}" &
                              $"المجلد: {row("ArchiveFolder")}{vbNewLine}" &
                              $"تاريخ الأرشفة: {Convert.ToDateTime(row("ArchiveDate")).ToShortDateString()}{vbNewLine}" &
                              $"المستخدم: {row("UserName")}{vbNewLine}{vbNewLine}" &
                              "يرجى استخدام رقم أرشيف مختلف.",
                              "رقم أرشيف مكرر", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("رقم الأرشيف موجود مسبقًا في سجل آخر ولا يمكن تكراره!", "رقم أرشيف مكرر", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            txtArchiveNum.SelectAll()
            txtArchiveNum.Focus()
            Return False
        End If

        ' التحقق من موضوع الأرشيف
        If String.IsNullOrWhiteSpace(txtArchiveSubject.Text) Then
            MessageBox.Show("يرجى إدخال موضوع الأرشيف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtArchiveSubject.Focus()
            Return False
        End If

        ' التحقق من مجلد الأرشيف
        If String.IsNullOrWhiteSpace(ComboArchiveFolder.Text) Then
            MessageBox.Show("يرجى إدخال مجلد الأرشيف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ComboArchiveFolder.Focus()
            Return False
        End If

        ' التحقق من اسم الملف
        If String.IsNullOrWhiteSpace(txtFilename.Text) Then
            MessageBox.Show("يرجى إدخال اسم الملف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFilename.Focus()
            Return False
        End If

        ' التحقق من نوع الملف
        If String.IsNullOrWhiteSpace(txtFileType.Text) Then
            MessageBox.Show("يرجى إدخال نوع الملف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFileType.Focus()
            Return False
        End If

        ' التحقق من مسار الملف
        If String.IsNullOrWhiteSpace(txtFilepath.Text) Then
            MessageBox.Show("يرجى إدخال مسار الملف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFilepath.Focus()
            Return False
        End If

        ' التحقق من اسم المستخدم
        If String.IsNullOrWhiteSpace(txtUserName.Text) Then
            MessageBox.Show("يرجى إدخال اسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUserName.Focus()
            Return False
        End If

        Return True
    End Function


    ''' <summary>
    ''' تفريغ الحقول
    ''' </summary>
    Private Sub ClearFields()
        txtArchiveNum.Clear()
        txtArchiveSubject.Clear()
        txtFilename.Clear()
        txtFileType.Clear()
        txtFilepath.Clear()
        txtOriginalPath.Clear()
        txtArchiveWillMaster.Clear()
        txtArchiveWillSub.Clear()
        txtArchiveNotes.Clear()
        dtpArchiveDate.Value = DateTime.Now
        If ComboArchiveFolder.Items.Count > 0 Then
            ComboArchiveFolder.SelectedIndex = -1 ' إعادة تعيين التصنيف للافتراضي
        End If
        ' تفريغ WebBrowser
        If pdfViewer IsNot Nothing Then
            PreviewFile("about:blank")
        End If
    End Sub

    'Private Sub dgvArchive_SelectionChanged(sender As Object, e As EventArgs) Handles dgvArchive.SelectionChanged
    '    Try
    '        If dgvArchive.SelectedRows.Count > 0 AndAlso Not isLoadingData Then
    '            Dim selectedRow As DataGridViewRow = dgvArchive.SelectedRows(0)

    '            ' التحقق من أن قيمة المعرف يمكن تحويلها إلى رقم
    '            Dim idValue As Object = selectedRow.Cells("ID").Value
    '            Dim archiveId As Integer

    '            ' استخدام Integer.TryParse لتجنب الأخطاء
    '            If Integer.TryParse(idValue?.ToString(), archiveId) Then
    '                ' استخدام archiveId الآمن
    '                Dim archiveData As DataTable = archiveClass.GetArchiveById(archiveId)

    '                If archiveData IsNot Nothing AndAlso archiveData.Rows.Count > 0 Then
    '                    Dim row As DataRow = archiveData.Rows(0)

    '                    ' تعبئة الحقول
    '                    txtArchiveNum.Text = row("ArchiveNum").ToString()
    '                    txtArchiveSubject.Text = row("ArchiveSubject").ToString()
    '                    ComboArchiveFolder.Text = row("ArchiveFolder").ToString()
    '                    dtpArchiveDate.Value = Convert.ToDateTime(row("ArchiveDate"))
    '                    txtArchiveWillMaster.Text = row("ArchiveWillMaster").ToString()
    '                    txtArchiveWillSub.Text = row("ArchiveWillSub").ToString()
    '                    txtArchiveNotes.Text = row("ArchiveNotes").ToString()
    '                    txtFilename.Text = row("filename").ToString()
    '                    txtFileType.Text = row("fileType").ToString()
    '                    txtFilepath.Text = row("filepath").ToString()
    '                    txtOriginalPath.Text = row("originalFilepath").ToString()
    '                    txtUserName.Text = row("UserName").ToString()

    '                    ' عرض الملف في WebBrowser إذا كان موجودًا
    '                    If Not String.IsNullOrEmpty(txtFilepath.Text) AndAlso File.Exists(txtFilepath.Text) Then
    '                        Try
    '                            PreviewFile(txtFilepath.Text)
    '                        Catch ex As Exception
    '                            ' تجاهل أخطاء عرض الملف
    '                        End Try
    '                    End If
    '                End If
    '            Else
    '                ' التعامل مع الحالة التي لا يمكن فيها تحويل القيمة إلى رقم
    '                System.Diagnostics.Debug.WriteLine("قيمة ID غير صالحة: " & idValue?.ToString())

    '                ' محاولة استخدام اسم الملف كمعرف مؤقت
    '                Dim fileName As String = selectedRow.Cells("filename").Value?.ToString()
    '                If Not String.IsNullOrEmpty(fileName) Then
    '                    System.Diagnostics.Debug.WriteLine("استخدام اسم الملف كمعرف مؤقت: " & fileName)
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ' تجنب التداخل وعرض رسالة خطأ مفيدة
    '        System.Diagnostics.Debug.WriteLine($"خطأ في SelectionChanged: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' تصفية البيانات حسب نوع الملف
    ''' </summary>
    Private Sub cmbFileType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFileType.SelectedIndexChanged
        Try
            If cmbFileType.SelectedItem IsNot Nothing Then
                Dim selectedFileType As String = cmbFileType.SelectedItem.ToString()
                If selectedFileType <> "الكل" Then
                    Dim dt As DataTable = archiveClass.GetArchiveByFileType(selectedFileType)
                    dgvArchive.DataSource = dt
                Else
                    LoadActiveArchive()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في تصفية نوع الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' عرض نافذة اختيار الملف
    ''' </summary>
    Private Sub btnBrowseFile_Click(sender As Object, e As EventArgs) Handles btnBrowseFile.Click
        Try
            ' التحقق من صلاحية الإضافة
            If Not CheckPermission(1, "إستعراض ملف أرشيف") Then
                Return
            End If
            ' التحقق من وجود الفورم بالفعل
            Dim openForm As CustomSearch = CType(Application.OpenForms("CustomSearch"), CustomSearch)

            If openForm Is Nothing Then
                ' إذا لم يكن الفورم مفتوحًا، يتم فتحه
                Dim newForm As New CustomSearch With {
                .Tag = 2,
                .Text = "إختر الملف المطلوب"
            }
                newForm.searchTextBox.Text = txtArchiveNum.Text.Trim()
                newForm.ShowDialog(Me)
            Else
                ' إذا كان الفورم مفتوحًا بالفعل، يتم التركيز عليه
                ' في هذه الحالة، لا نحتاج لإنشاء نموذج جديد، بل نقوم بتحديث النموذج الموجود
                openForm.searchTextBox.Text = txtArchiveNum.Text.Trim()
                openForm.Focus()
                openForm.Activate()
            End If
        Catch ex As Exception
            Return
        End Try
    End Sub


    'Private Sub btnBrowseFile_Click(sender As Object, e As EventArgs) Handles btnBrowseFile.Click
    '    Try
    '        ' إنشاء نموذج البحث
    '        Dim searchForm As New CustomSearch With {
    '        .Tag = 2,
    '        .Text = "إختر الملف المطلوب"
    '    }

    '        ' تعيين قيمة txtArchiveNum إلى searchTextBox قبل عرض النموذج
    '        If Not String.IsNullOrWhiteSpace(txtArchiveNum.Text) Then
    '            searchForm.searchTextBox.Text = txtArchiveNum.Text.Trim()
    '        End If

    '        ' إضافة معالج للحدث DataSelected
    '        AddHandler searchForm.DataSelected, Sub(filePath, fileName, fileExtension)
    '                                                ' تعيين المسار الأصلي
    '                                                txtOriginalPath.Text = filePath
    '                                                ' تعيين اسم الملف ونوعه
    '                                                txtFilename.Text = fileName
    '                                                txtFileType.Text = fileExtension.TrimStart(".")

    '                                                ' تعيين مجلد الأرشيف الافتراضي إذا لم يكن محددًا
    '                                                If String.IsNullOrWhiteSpace(ComboArchiveFolder.Text) Then
    '                                                    Dim currentMonth As String = DateTime.Now.ToString("yyyy-MM")
    '                                                    ComboArchiveFolder.Text = $"صادر\{currentMonth}"
    '                                                End If

    '                                                ' حساب مسار الوجهة في مجلد الأرشيف
    '                                                CalculateDestinationPath()

    '                                                ' عرض الملف في WebBrowser
    '                                                PreviewFile(filePath)
    '                                            End Sub

    '        ' عرض نموذج البحث
    '        searchForm.ShowDialog()
    '    Catch ex As Exception
    '        MessageBox.Show($"حدث خطأ أثناء فتح نموذج البحث: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    ''' <summary>
    ''' فتح موقع الملف
    ''' </summary>
    Private Sub btnSelectArchive_Click(sender As Object, e As EventArgs) Handles btnSelectArchive.Click
        If Not String.IsNullOrEmpty(txtFilepath.Text) Then
            archiveClass.OpenFileLocation(txtFilepath.Text)
        End If
    End Sub

    ''' <summary>
    ''' تحميل البيانات الجديدة
    ''' </summary>
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        LoadActiveArchive()
        dgvArchive.ClearSelection()
        DataHelper.UpdateArchiveCount()
        PreviewFile("about:blank")
        ClearFields()
        btnUpdate.Enabled = False
        btnDelete.Enabled = False
    End Sub

    ''' <summary>
    ''' البحث عن البيانات
    ''' </summary>
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        ' التحقق من صلاحية البحث
        If Not CheckPermission(4, "البحث في الأرشيف") Then
            Return
        End If
        Try
            If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
                Dim dt As DataTable = archiveClass.SearchActiveArchive(txtSearch.Text.Trim())
                dgvArchive.DataSource = dt
            Else
                LoadActiveArchive()
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في البحث: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' البحث حسب النطاق الزمني
    ''' </summary>
    Private Sub btnSearchByDate_Click(sender As Object, e As EventArgs) Handles btnSearchByDate.Click
        Try
            ' تنفيذ البحث وحفظ النتائج
            Dim dt As DataTable = archiveClass.SearchSaderByDateRange(dtpStartDate.Value, dtpEndDate.Value)

            ' ربط النتائج بالـ DataGridView
            dgvArchive.DataSource = dt

            '' تحديث عناوين الأعمدة إذا كانت موجودة
            'UpdateColumnHeaders()

            ' تحديث عداد الصفوف
            lblRecordCount.Text = $"عدد السجلات المؤرشفة : {dgvArchive.Rows.Count}"

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء البحث حسب التاريخ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' البحث عند تغيير النص
    ''' </summary>
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        'التحقق من صلاحية البحث
        If Not CheckPermission(4, "البحث في الأرشيف") Then
            Return
        End If
        Try
            If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
                Dim dt As DataTable = archiveClass.SearchArchive(txtSearch.Text.Trim())
                dgvArchive.DataSource = dt
            Else
                LoadActiveArchive()
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في البحث: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحديث عداد الصفوف
    ''' </summary>
    Private Sub dgvArchive_RowsAdded(sender As Object, e As EventArgs) Handles dgvArchive.RowsAdded
        ' تحديث عداد الصفوف أو أي عمليات أخرى
        lblRecordCount.Text = $"عدد السجلات المؤرشفة : {dgvArchive.Rows.Count}"
    End Sub

    ''' <summary>
    ''' التعامل مع تحديد صف جديد
    ''' </summary>
    Private Sub dgvArchive_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvArchive.CellClick
        Try
            ' تجاهل الحدث أثناء تحميل البيانات
            If isLoadingData Then Return
            ' التحقق من وجود صف محدد
            'If dgvArchive.SelectedRows.Count = 0 Then
            '    MessageBox.Show("يرجى اختيار أرشيف للإخفاء", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            '    Return
            'End If
            ' التحقق من أن الفهرس صالح (ليس -1 للهيدر)
            If e.RowIndex < 0 Then
                Return
            End If

            ' التحقق من أن العمود صالح
            If e.ColumnIndex < 0 Then
                Return
            End If

            ' التحقق من وجود صفوف محددة
            If dgvArchive.Rows.Count = 0 OrElse e.RowIndex >= dgvArchive.Rows.Count Then
                Return
            End If

            ' تعبئة الحقول بالبيانات مع التحقق من الأمان
            txtArchiveNum.Text = GetSelectedRowStringValue("ArchiveNum")
            dtpArchiveDate.Value = GetSelectedRowDateValue("ArchiveDate")
            txtArchiveSubject.Text = GetSelectedRowStringValue("ArchiveSubject")
            txtArchiveNotes.Text = GetSelectedRowStringValue("ArchiveNotes")
            ComboArchiveFolder.Text = GetSelectedRowStringValue("ArchiveFolder")
            txtFilename.Text = GetSelectedRowStringValue("FileName")
            txtFileType.Text = GetSelectedRowStringValue("FileType")
            txtArchiveWillMaster.Text = GetSelectedRowStringValue("ArchiveWillMaster")
            txtArchiveWillSub.Text = GetSelectedRowStringValue("ArchiveWillSub")
            txtUserName.Text = GetSelectedRowStringValue("UserName")
            txtFilepath.Text = GetSelectedRowStringValue("FilePath") ' المسار الحالي
            txtOriginalPath.Text = GetSelectedRowStringValue("OriginalFilePath") ' المسار الأصلي

            ' عرض الملف في WebBrowser
            If Not String.IsNullOrEmpty(txtFilepath.Text) AndAlso File.Exists(txtFilepath.Text) Then
                PreviewFile(txtFilepath.Text)
            ElseIf Not String.IsNullOrEmpty(txtOriginalPath.Text) AndAlso File.Exists(txtOriginalPath.Text) Then
                PreviewFile(txtOriginalPath.Text)
            Else
                PreviewFile("about:blank")
            End If

            ' تعطيل حقل رقم الأرشيف للتعديل
            txtArchiveNum.ReadOnly = True
            'btnUpdate.Enabled = True
            ' تفعيل أدوات التعديل
            'EnableEditingTools(True)
            btnAdd.Enabled = False
            btnUpdate.Enabled = True
            btnDelete.Enabled = True

            ' التحقق من وجود صفوف محددة
            If dgvArchive.SelectedRows.Count > 0 Then
                ' إضافة تأثير بصري للصف المحدد
                HighlightSelectedRow()
                ' تعبئة الحقول بالبيانات الجديدة
                FillFieldsFromSelectedRow(dgvArchive.SelectedRows(0))

                ' تمكين أدوات التعديل
                'EnableEditingTools(True)
                btnAdd.Enabled = False
            Else
                ' تعطيل أدوات التعديل إذا لم يكن هناك تحديد صالح
                'EnableEditingTools(False)
                btnAdd.Enabled = PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, "ArchiveForm", "Add")
            End If

            '' إضافة تأثير بصري للصف المحدد
            'HighlightSelectedRow()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في dgvArchive_CellClick: {ex.Message}")
            'EnableEditingTools(False)
            btnAdd.Enabled = PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, "ArchiveForm", "Add")
        End Try
    End Sub

    ''' <summary>
    ''' تمييز الصف المحدد
    ''' </summary>
    Private Sub HighlightSelectedRow()
        If dgvArchive.SelectedRows.Count > 0 Then
            dgvArchive.SelectedRows(0).Selected = True
            dgvArchive.FirstDisplayedScrollingRowIndex = dgvArchive.SelectedRows(0).Index
        End If
    End Sub

    ''' <summary>
    ''' تمكين أو تعطيل أدوات التحرير بناءً على الصلاحيات والحالة
    ''' </summary>
    Private Sub EnableEditingTools(enabled As Boolean)
        Try
            ' تمكين/تعطيل زر التحديث بناءً على الحالة والصلاحية
            btnUpdate.Enabled = enabled AndAlso PermissionHelper.CanEdit()

            ' تمكين/تعطيل زر الحذف بناءً على الحالة والصلاحية
            btnDelete.Enabled = enabled AndAlso PermissionHelper.CanDelete()

            ' يمكن إضافة المزيد من العناصر حسب الحاجة
            'btnPrint.Enabled = enabled AndAlso PermissionHelper.CanPrint()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في EnableEditingTools: {ex.Message}")
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
        End Try
    End Sub

    ''' <summary>
    ''' عند عرض النموذج
    ''' </summary>
    Private Sub ArchiveForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        dgvArchive.ClearSelection()
    End Sub

    ''' <summary>
    ''' عند إغلاق النموذج
    ''' </summary>
    Private Sub ArchiveForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' إلغاء الاشتراك في الحدث
        RemoveHandler DataHelper.ArchiveCountChanged, AddressOf HandleDataCountChanged
        RemoveHandler DataHelper.FileSelectedForArchive, AddressOf HandleFileSelectedForArchive
    End Sub

    'Private Sub MyForm_FormClosing(sender As Object, e As FormClosingEventArgs)
    '    ' يمكنك وضع أي كود لتنظيف الموارد هنا
    '    ' على سبيل المثال، سؤال المستخدم إذا كان يريد حفظ التغييرات

    '    ' If MsgBox("هل أنت متأكد أنك تريد الإغلاق؟", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
    '    '     e.Cancel = True ' لإلغاء عملية الإغلاق
    '    ' End If
    'End Sub


#End Region

#Region "الوظائف المساعدة للصف المحدد"

    ''' <summary>
    ''' التحقق من صحة الصف المحدد
    ''' </summary>
    ''' <returns>صحيح إذا كان هناك صف محدد وصالح</returns>
    Private Function IsValidRowSelected() As Boolean
        Try
            ' التحقق من أن هناك صفوف محددة
            If dgvArchive.SelectedRows.Count = 0 Then
                Return False
            End If

            ' التحقق من أن الفهرس صالح
            If dgvArchive.SelectedRows(0).Index < 0 Then
                Return False
            End If

            ' التحقق من أن الفهرس ضمن النطاق
            If dgvArchive.SelectedRows(0).Index >= dgvArchive.Rows.Count Then
                Return False
            End If

            ' التحقق من أن الصف ليس صف الإدخال الجديد
            If dgvArchive.SelectedRows(0).IsNewRow Then
                Return False
            End If

            Return True
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في IsValidRowSelected: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' معالج الحدث عندما يتم اختيار ملف للإضافة إلى الأرشيف
    ''' </summary>
    Private Sub HandleFileSelectedForArchive(filePath As String, fileName As String, fileExtension As String)
        Try
            ' تعبئة الحقول المطلوبة
            txtOriginalPath.Text = filePath
            txtFilename.Text = fileName
            txtFileType.Text = fileExtension.TrimStart(".")

            ' جلب اسم المجلد من المسار 
            Dim folderPath As String = Path.GetDirectoryName(filePath)
            Dim archivePath As String = My.Settings.DefaultPathSearch

            ' استخراج اسم المجلد الرئيسي فقط
            If folderPath.StartsWith(archivePath) Then
                Dim relativePath As String = folderPath.Substring(archivePath.Length).Trim("\")
                If Not String.IsNullOrEmpty(relativePath) Then
                    ' تعيين المسار النسبي الكامل في ComboArchiveFolder
                    ComboArchiveFolder.Text = relativePath


                    ' حساب مسار الوجهة
                    CalculateDestinationPath()
                End If
            End If

            ' عرض الملف في WebBrowser
            PreviewFile(filePath)

            ' تعبئة الحقول الافتراضية
            FillDefaultFields()

            ' التحقق من أن جميع الحقول معبأة
            If ValidateBeforeAdd() Then
                ' الضغط على زر الإضافة تلقائيًا
                btnAutoAdd.PerformClick()
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة ملف مختار للإضافة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' استخراج اسم المجلد الرئيسي من المسار النسبي
    ''' </summary>
    Private Function GetMainFolderName(relativePath As String) As String
        If String.IsNullOrEmpty(relativePath) Then
            Return String.Empty
        End If

        ' تقسيم المسار إلى أجزاء
        Dim parts() As String = relativePath.Split("\"c, "/")

        ' إرجاع الجزء الأول (اسم المجلد الرئيسي)
        Return If(parts.Length > 0, parts(0), relativePath)
    End Function

    ''' <summary>
    ''' التحقق من أن اسم المجلد من التصنيفات المسموحة
    ''' </summary>
    Private Function IsValidArchiveCategory(folderName As String) As Boolean
        ' التصنيفات المسموحة
        Dim validCategories As String() = {"شركات اخرى", "الجمارك العامة", "مذكرات", "تصدير", "إفراج جمركي"}

        ' التحقق من وجود الاسم في التصنيفات المسموحة
        For Each category As String In validCategories
            If String.Equals(folderName, category, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' تعبئة الحقول الافتراضية
    ''' </summary>
    Private Sub FillDefaultFields()
        Try
            ' تعبئة رقم الأرشيف إذا كان فارغًا
            If String.IsNullOrWhiteSpace(txtArchiveNum.Text) Then
                txtArchiveNum.Text = GenerateArchiveNumber()
            End If

            ' تعبئة موضوع الأرشيف إذا كان فارغًا
            If String.IsNullOrWhiteSpace(txtArchiveSubject.Text) Then
                txtArchiveSubject.Text = "موضوع الملف"
            End If

            ' تعبئة اسم المستخدم إذا كان فارغًا
            If String.IsNullOrWhiteSpace(txtUserName.Text) Then
                txtUserName.Text = GlobalData.LoggedInUserName
            End If

            ' تعبئة تصنيف الأرشيف إذا كان فارغًا
            If String.IsNullOrWhiteSpace(ComboArchiveFolder.Text) Then
                ComboArchiveFolder.Text = "صادر"
            End If

            ' حساب مسار الوجهة
            CalculateDestinationPath()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعبئة الحقول الافتراضية: {ex.Message}")
        End Try
    End Sub

    Public Sub CalculateDestinationPath()
        Try
            ' التحقق من وجود مسار أصلي
            If String.IsNullOrWhiteSpace(txtOriginalPath.Text) Then
                Return
            End If

            ' التحقق من اختيار تصنيف أرشيف
            If String.IsNullOrWhiteSpace(ComboArchiveFolder.Text) Then
                Return
            End If

            ' الحصول على اسم التصنيف والمجلد الفرعي
            Dim category As String = ""
            Dim subFolder As String = ""

            ' تحليل ComboArchiveFolder.Text للحصول على التصنيف والمجلد الفرعي
            If ComboArchiveFolder.Text.Contains("\") Then
                Dim parts() As String = ComboArchiveFolder.Text.Split("\"c)
                category = parts(0)
                subFolder = parts(1)
            Else
                category = ComboArchiveFolder.Text
                subFolder = DateTime.Now.ToString("yyyy-MM")
            End If

            ' إنشاء المسار الكامل للمجلد الفرعي
            Dim finalArchivePath As String = archiveClass.CreateSubFolder(category, subFolder)

            ' التحقق من أن المسار تم إنشاؤه
            If String.IsNullOrEmpty(finalArchivePath) Then
                Return
            End If

            ' إنشاء اسم الملف الكامل
            Dim fileExtension As String = If(txtFileType.Text.StartsWith("."), txtFileType.Text, "." & txtFileType.Text)
            Dim fullFileName As String = If(txtFilename.Text.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase),
                                    txtFilename.Text,
                                    txtFilename.Text & fileExtension)

            ' تعيين مسار الوجهة
            txtFilepath.Text = Path.Combine(finalArchivePath, fullFileName)

            ' التحقق من وجود المجلد و إنشائه إذا لزم الأمر
            Dim destinationFolder As String = Path.GetDirectoryName(txtFilepath.Text)
            If Not Directory.Exists(destinationFolder) Then
                Directory.CreateDirectory(destinationFolder)
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حساب مسار الوجهة: {ex.Message}")
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

    ''' <summary>
    ''' الحصول على قيمة نصية من خلية الصف المحدد
    ''' </summary>
    ''' <param name="columnName">اسم العمود</param>
    ''' <returns>القيمة النصية أو سلسلة فارغة إذا كانت القيمة فارغة أو غير موجودة</returns>
    Friend Function GetSelectedRowStringValue(columnName As String) As String
        Try
            ' التحقق من وجود صفوف محددة
            If dgvArchive.SelectedRows.Count = 0 Then
                Return String.Empty
            End If

            ' التحقق من أن الفهرس صالح
            If dgvArchive.SelectedRows(0).Index < 0 Then
                Return String.Empty
            End If

            ' التحقق من أن العمود موجود
            If Not dgvArchive.Columns.Contains(columnName) Then
                Return String.Empty
            End If

            ' التحقق من وجود الخلية في الصف المحدد
            If dgvArchive.SelectedRows(0).Cells(columnName) Is Nothing Then
                Return String.Empty
            End If

            ' التحقق من وجود قيمة في الخلية
            If dgvArchive.SelectedRows(0).Cells(columnName).Value Is Nothing Then
                Return String.Empty
            End If

            ' التحقق من أن القيمة ليست DBNull
            If dgvArchive.SelectedRows(0).Cells(columnName).Value Is DBNull.Value Then
                Return String.Empty
            End If

            ' تحويل القيمة إلى سلسلة نصية
            Return dgvArchive.SelectedRows(0).Cells(columnName).Value.ToString()

        Catch ex As Exception
            ' تسجيل الخطأ للتنقيح
            System.Diagnostics.Debug.WriteLine($"خطأ في GetSelectedRowStringValue: {ex.Message}")
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' الحصول على قيمة تاريخ من خلية الصف المحدد
    ''' </summary>
    ''' <param name="columnName">اسم العمود</param>
    ''' <returns>قيمة التاريخ أو التاريخ الحالي إذا كانت القيمة فارغة أو غير صالحة</returns>
    Private Function GetSelectedRowDateValue(columnName As String) As Date
        Try
            ' التحقق من وجود صفوف محددة
            If dgvArchive.SelectedRows.Count = 0 Then
                Return DateTime.Now
            End If

            ' التحقق من أن الفهرس صالح
            If dgvArchive.SelectedRows(0).Index < 0 Then
                Return DateTime.Now
            End If

            ' التحقق من وجود الخلية في الصف المحدد
            If dgvArchive.SelectedRows(0).Cells(columnName) Is Nothing Then
                Return DateTime.Now
            End If

            ' التحقق من وجود قيمة في الخلية
            If dgvArchive.SelectedRows(0).Cells(columnName).Value Is Nothing Then
                Return DateTime.Now
            End If

            ' التحقق من أن القيمة ليست DBNull
            If dgvArchive.SelectedRows(0).Cells(columnName).Value Is DBNull.Value Then
                Return DateTime.Now
            End If

            ' تحويل القيمة إلى تاريخ
            Return Convert.ToDateTime(dgvArchive.SelectedRows(0).Cells(columnName).Value)

        Catch ex As Exception
            ' تسجيل الخطأ للتنقيح
            System.Diagnostics.Debug.WriteLine($"خطأ في GetSelectedRowDateValue: {ex.Message}")
            Return DateTime.Now
        End Try
    End Function

    ''' <summary>
    ''' الحصول على قيمة عددية من خلية الصف المحدد
    ''' </summary>
    ''' <param name="columnName">اسم العمود</param>
    ''' <returns>القيمة العددية أو 0 إذا كانت القيمة فارغة أو غير صالحة</returns>
    Private Function GetSelectedRowIntegerValue(columnName As String) As Integer?
        Try
            ' التحقق من وجود صفوف محددة
            If dgvArchive.SelectedRows.Count = 0 Then
                Return Nothing
            End If

            ' التحقق من أن الفهرس صالح
            If dgvArchive.SelectedRows(0).Index < 0 Then
                Return Nothing
            End If

            ' التحقق من وجود الخلية في الصف المحدد
            If dgvArchive.SelectedRows(0).Cells(columnName) Is Nothing Then
                Return Nothing
            End If

            ' التحقق من وجود قيمة في الخلية
            If dgvArchive.SelectedRows(0).Cells(columnName).Value Is Nothing Then
                Return Nothing
            End If

            ' التحقق من أن القيمة ليست DBNull
            If dgvArchive.SelectedRows(0).Cells(columnName).Value Is DBNull.Value Then
                Return Nothing
            End If

            ' تحويل القيمة إلى عدد صحيح
            Dim intValue As Integer
            If Integer.TryParse(dgvArchive.SelectedRows(0).Cells(columnName).Value.ToString(), intValue) Then
                Return intValue
            Else
                Return Nothing
            End If

        Catch ex As Exception
            ' تسجيل الخطأ للتنقيح
            System.Diagnostics.Debug.WriteLine($"خطأ في GetSelectedRowIntegerValue: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' التحقق من وجود الملف في المسار قبل الإضافة مع منع التكرار
    ''' </summary>
    ''' <param name="fileName">اسم الملف</param>
    ''' <param name="fileType">نوع الملف</param>
    ''' <param name="archiveFolder">مجلد الأرشيف</param>
    ''' <param name="originalFilePath">مسار الملف الأصلي</param>
    ''' <param name="currentFilePath">مسار الملف الحالي (اختياري)</param>
    ''' <returns>صحيح إذا كان يمكن المتابعة، خطأ إذا كان الملف موجودًا</returns>
    Public Function ValidateFileBeforeAddition(
    ByVal fileName As String,
    ByVal fileType As String,
    ByVal archiveFolder As String,
    ByVal originalFilePath As String,
    Optional ByVal currentFilePath As String = "") As Boolean
        Try
            ' التحقق من المدخلات الأساسية
            If String.IsNullOrWhiteSpace(fileName) Then
                MessageBox.Show("يرجى إدخال اسم الملف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            If String.IsNullOrWhiteSpace(fileType) Then
                MessageBox.Show("يرجى إدخال نوع الملف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            If String.IsNullOrWhiteSpace(archiveFolder) Then
                MessageBox.Show("يرجى إدخال مجلد الأرشيف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            ' تحليل مسار الأرشيف للحصول على المسار النهائي
            Dim finalArchivePath As String = archiveClass.GetFinalArchivePath(archiveFolder)
            If String.IsNullOrEmpty(finalArchivePath) Then
                MessageBox.Show("فشل في تحديد مسار الأرشيف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            ' تكوين اسم الملف الكامل
            Dim fileExtension As String = If(fileType.StartsWith("."), fileType, "." & fileType)
            Dim fullFileName As String = If(fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase),
                                      fileName,
                                      fileName & fileExtension)
            Dim destinationPath As String = Path.Combine(finalArchivePath, fullFileName)

            ' استخدام المسار الحالي إذا كان متوفرًا، وإلا استخدام المسار الأصلي
            Dim filePathToCheck As String = String.Empty

            ' التحقق من وجود مسار الملف
            If Not String.IsNullOrWhiteSpace(currentFilePath) Then
                filePathToCheck = currentFilePath
            ElseIf Not String.IsNullOrWhiteSpace(originalFilePath) Then
                filePathToCheck = originalFilePath
            Else
                filePathToCheck = destinationPath
            End If

            ' التحقق من وجود الملف في المسار المحدد
            If File.Exists(destinationPath) Then
                ' التحقق من أن الملف ليس هو نفسه (لمنع التبديل عند التعديل)
                If Not File.Exists(filePathToCheck) OrElse
               Path.GetFullPath(filePathToCheck).ToLower() <> Path.GetFullPath(destinationPath).ToLower() Then

                    Dim result As DialogResult = MessageBox.Show(
                    $"الملف '{fullFileName}' موجود مسبقًا في المسار: {finalArchivePath}{vbNewLine}{vbNewLine}" &
                    "يرجى تغيير اسم الملف أولاً لتجنب التكرار.{vbNewLine}" &
                    "هل تريد فتح مستعرض الملفات لتغيير الاسم؟",
                    "ملف موجود مسبقًا",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2)

                    Select Case result
                        Case DialogResult.Yes
                            ' فتح مستعرض الملفات للملف الموجود
                            Try
                                Process.Start("explorer.exe", $"/select,""{destinationPath}""")
                            Catch ex As Exception
                                MessageBox.Show($"فشل في فتح موقع الملف: {ex.Message}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            End Try
                            Return False

                        Case DialogResult.No
                            ' محاولة تغيير الاسم تلقائيًا بإضافة رقم
                            Dim newFileName As String = GetUniqueFileName(finalArchivePath, fullFileName)
                            Dim newDestinationPath As String = Path.Combine(finalArchivePath, newFileName)

                            Dim confirm As DialogResult = MessageBox.Show(
                            $"سيتم تغيير اسم الملف تلقائيًا إلى: {newFileName}{vbNewLine}" &
                            "هل توافق على هذا التغيير؟",
                            "تغيير اسم الملف تلقائيًا",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question)

                            If confirm = DialogResult.Yes Then
                                ' تحديث حقول النموذج
                                UpdateFormFields(newFileName, newDestinationPath)
                                Return True
                            Else
                                Return False
                            End If

                        Case DialogResult.Cancel
                            Return False
                    End Select
                End If
            End If

            ' التحقق من وجود ملف بنفس الاسم في قاعدة البيانات (لتقليل التكرار)
            If CheckDuplicateFileInDatabase(fileName, fileType, archiveFolder) Then
                Dim result As DialogResult = MessageBox.Show(
                $"يوجد ملف بنفس الاسم '{fullFileName}' في قاعدة البيانات.{vbNewLine}" &
                "هل تريد المتابعة على الرغم من التكرار؟",
                "تحذير التكرار",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

                If result = DialogResult.No Then
                    Return False
                End If
            End If

            Return True

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء التحقق من الملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    '''' <summary>
    '''' الحصول على المسار النهائي للمجلد بناءً على تصنيف الأرشيف
    '''' </summary>
    'Private Function GetFinalArchivePath(archiveFolder As String) As String
    '    Try
    '        Dim finalArchivePath As String = ""

    '        If archiveFolder.Contains("\") Then
    '            Dim parts() As String = archiveFolder.Split("\"c)
    '            If parts.Length >= 2 Then
    '                Dim category As String = parts(0)
    '                Dim subFolder As String = parts(1)
    '                finalArchivePath = CreateSubFolder(category, subFolder)
    '            Else
    '                finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
    '                If Not Directory.Exists(finalArchivePath) Then
    '                    Directory.CreateDirectory(finalArchivePath)
    '                End If
    '            End If
    '        Else
    '            finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
    '            If Not Directory.Exists(finalArchivePath) Then
    '                Directory.CreateDirectory(finalArchivePath)
    '            End If
    '        End If

    '        Return finalArchivePath
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على مسار الأرشيف: {ex.Message}")
    '        Return String.Empty
    '    End Try
    'End Function


    ''' <summary>
    ''' إنشاء مجلد فرعي بناءً على التصنيف والمجلد الفرعي
    ''' </summary>
    ''' <param name="category">التصنيف (مثل: صادر، وارد)</param>
    ''' <param name="subFolder">اسم المجلد الفرعي (مثل: 2024-12)</param>
    ''' <returns>المسار الكامل للمجلد الفرعي</returns>
    Public Function CreateSubFolder(category As String, subFolder As String) As String
        Try
            ' إنشاء مسار المجلد الرئيسي
            Dim categoryPath As String = Path.Combine(My.Settings.ARCHIVE_DEFAULT_PATH, category)

            ' إنشاء مسار المجلد الفرعي
            Dim subFolderPath As String = Path.Combine(categoryPath, subFolder)

            ' إنشاء المجلدات إذا لم تكن موجودة
            If Not Directory.Exists(categoryPath) Then
                Directory.CreateDirectory(categoryPath)
            End If

            If Not Directory.Exists(subFolderPath) Then
                Directory.CreateDirectory(subFolderPath)
            End If

            Return subFolderPath
        Catch ex As Exception
            Throw New Exception("خطأ في إنشاء المجلد الفرعي: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' التحقق من وجود ملف بنفس الاسم في قاعدة البيانات
    ''' </summary>
    Private Function CheckDuplicateFileInDatabase(fileName As String, fileType As String, archiveFolder As String) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE FileName = @FileName AND FileType = @FileType AND ArchiveFolder = @ArchiveFolder"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@FileName", fileName),
                DatabaseHelper.CreateParameter("@FileType", fileType),
                DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder)
            }
            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
            Return count > 0
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من تكرار الملف في قاعدة البيانات: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' توليد اسم ملف فريد
    ''' </summary>
    Private Function GetUniqueFileName(folderPath As String, fileName As String) As String
        Try
            Dim fileNameWithoutExt As String = Path.GetFileNameWithoutExtension(fileName)
            Dim fileExtension As String = Path.GetExtension(fileName)
            Dim counter As Integer = 1
            Dim newFileName As String = fileName

            While File.Exists(Path.Combine(folderPath, newFileName))
                newFileName = $"{fileNameWithoutExt}_{counter}{fileExtension}"
                counter += 1
            End While

            Return newFileName
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في توليد اسم ملف فريد: {ex.Message}")
            Return fileName
        End Try
    End Function

    ''' <summary>
    ''' تحديث حقول النموذج بالأسماء الجديدة
    ''' </summary>
    Private Sub UpdateFormFields(newFileName As String, newDestinationPath As String)
        Try
            ' تحديث الحقول في النموذج الحالي
            txtFilename.Text = Path.GetFileNameWithoutExtension(newFileName)
            txtFileType.Text = Path.GetExtension(newFileName).TrimStart(".")
            txtFilepath.Text = newDestinationPath
            txtOriginalPath.Text = newDestinationPath ' تحديث المسار الأصلي أيضًا

            ' تحديث واجهة المستخدم
            MessageBox.Show($"تم تحديث اسم الملف تلقائيًا إلى: {newFileName}", "تم التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث حقول النموذج: {ex.Message}")
        End Try
    End Sub

    Private Sub ComboArchiveFolder_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboArchiveFolder.SelectedIndexChanged
        CalculateDestinationPath()
    End Sub
#End Region

#Region "الدوال الجديدة  للحذف المنطقي"

    ''' <summary>
    ''' معالج الحدث لتغيير عداد المحذوفات
    ''' </summary>
    Private Sub HandleDeletedArchiveCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
        ' تأكد أن التحديث لجدول الأرشيفات المحذوفة
        If e.TableName = "DeletedArchive" Then
            UpdateDeletedRecordCount()
        End If
    End Sub

    ''' <summary>
    ''' تحديث تسمية عداد المحذوفات
    ''' </summary>
    Private Sub UpdateDeletedRecordCount()
        Dim count As Integer = DataHelper.DeletedArchiveCount
        lblDeletedRecordCount.Text = $"عدد المحذوفات: {count}"
    End Sub



#End Region

#Region " التعامل مع الداتا قريد فيو "

    ''' <summary>
    ''' معالجة ضغط مفاتيح الأسهم للتنقل بين الخلايا والصفوف
    ''' </summary>
    Private Sub dgvArchive_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvArchive.KeyDown
        Try
            ' التحقق من وجود صفوف محددة
            If dgvArchive.SelectedRows.Count = 0 Then Return

            Dim currentRow As DataGridViewRow = dgvArchive.SelectedRows(0)
            Dim currentIndex As Integer = currentRow.Index

            Select Case e.KeyCode
            ' السهم لأسفل
                Case Keys.Down
                    If currentIndex < dgvArchive.Rows.Count - 1 Then
                        dgvArchive.ClearSelection()
                        dgvArchive.Rows(currentIndex + 1).Selected = True
                        dgvArchive.FirstDisplayedScrollingRowIndex = currentIndex + 1

                        ' تعبئة الحقول بالبيانات الجديدة
                        FillFieldsFromSelectedRow(dgvArchive.Rows(currentIndex + 1))
                    End If

            ' السهم لأعلى
                Case Keys.Up
                    If currentIndex > 0 Then
                        dgvArchive.ClearSelection()
                        dgvArchive.Rows(currentIndex - 1).Selected = True
                        dgvArchive.FirstDisplayedScrollingRowIndex = currentIndex - 1

                        ' تعبئة الحقول بالبيانات الجديدة
                        FillFieldsFromSelectedRow(dgvArchive.Rows(currentIndex - 1))
                    End If

            ' السهم لليمين (للانتقال إلى الخلية التالية في نفس الصف)
                Case Keys.Right
                    Dim currentCell As DataGridViewCell = dgvArchive.CurrentCell
                    If currentCell IsNot Nothing AndAlso currentCell.ColumnIndex < dgvArchive.Columns.Count - 1 Then
                        dgvArchive.CurrentCell = dgvArchive(currentCell.ColumnIndex + 1, currentCell.RowIndex)
                    End If

            ' السهم لليسار (للانتقال إلى الخلية السابقة في نفس الصف)
                Case Keys.Left
                    Dim currentCell As DataGridViewCell = dgvArchive.CurrentCell
                    If currentCell IsNot Nothing AndAlso currentCell.ColumnIndex > 0 Then
                        dgvArchive.CurrentCell = dgvArchive(currentCell.ColumnIndex - 1, currentCell.RowIndex)
                    End If

            ' مفتاح Enter
                Case Keys.Enter
                    ' فتح الملف أو تنفيذ إجراء معين
                    OpenSelectedFile()

            ' مفتاح المسافة (Space)
                Case Keys.Space
                    ' تحديد/إلغاء تحديد الصف الحالي
                    ToggleRowSelection()

            ' Ctrl + A (تحديد جميع الصفوف)
                Case Keys.A
                    If e.Control Then
                        dgvArchive.SelectAll()
                    End If
            End Select

            ' منع القيمة الافتراضية لتجنب الأصوات عند الضغط على الأسهم
            e.Handled = True
            e.SuppressKeyPress = True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حدث KeyDown: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تعبئة الحقول بالبيانات من الصف المحدد
    ''' </summary>
    Private Sub FillFieldsFromSelectedRow(row As DataGridViewRow)
        Try
            ' التحقق من صحة الصف
            If row Is Nothing OrElse row.IsNewRow Then Return

            ' تعبئة الحقول بالبيانات
            txtArchiveNum.Text = GetCellValue(row, "ArchiveNum")
            dtpArchiveDate.Value = GetDateValue(row, "ArchiveDate")
            txtArchiveSubject.Text = GetCellValue(row, "ArchiveSubject")
            ComboArchiveFolder.Text = GetCellValue(row, "ArchiveFolder")
            txtFilename.Text = GetCellValue(row, "FileName")
            txtFileType.Text = GetCellValue(row, "FileType")
            txtArchiveWillMaster.Text = GetCellValue(row, "ArchiveWillMaster")
            txtArchiveWillSub.Text = GetCellValue(row, "ArchiveWillSub")
            txtArchiveNotes.Text = GetCellValue(row, "ArchiveNotes")

            ' عرض الملف في WebBrowser إذا كان موجودًا
            Dim filePath As String = GetCellValue(row, "FilePath")
            If Not String.IsNullOrEmpty(filePath) AndAlso File.Exists(filePath) Then
                Try
                    PreviewFile(filePath)
                Catch ex As Exception
                    System.Diagnostics.Debug.WriteLine($"فشل في عرض الملف: {ex.Message}")
                End Try
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعبئة الحقول: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' الحصول على قيمة خلية كسلسلة نصية
    ''' </summary>
    Private Function GetCellValue(row As DataGridViewRow, columnName As String) As String
        Try
            If row.Cells(columnName) IsNot Nothing AndAlso
               row.Cells(columnName).Value IsNot Nothing AndAlso
               Not Convert.IsDBNull(row.Cells(columnName).Value) Then
                Return row.Cells(columnName).Value.ToString()
            End If
            Return String.Empty
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على قيمة الخلية {columnName}: {ex.Message}")
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' الحصول على قيمة تاريخ من خلية
    ''' </summary>
    Private Function GetDateValue(row As DataGridViewRow, columnName As String) As Date
        Try
            If row.Cells(columnName) IsNot Nothing AndAlso
               row.Cells(columnName).Value IsNot Nothing AndAlso
               Not Convert.IsDBNull(row.Cells(columnName).Value) Then

                Dim value As Object = row.Cells(columnName).Value
                If TypeOf value Is Date Then
                    Return DirectCast(value, Date)
                ElseIf DateTime.TryParse(value.ToString(), Nothing, Globalization.DateTimeStyles.None, value) Then
                    Return CDate(value)
                End If
            End If
            Return DateTime.Now
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على قيمة التاريخ {columnName}: {ex.Message}")
            Return DateTime.Now
        End Try
    End Function

    ''' <summary>
    ''' تبديل تحديد الصف
    ''' </summary>
    Private Sub ToggleRowSelection()
        Try
            If dgvArchive.SelectedRows.Count > 0 Then
                Dim selectedRow As DataGridViewRow = dgvArchive.SelectedRows(0)
                selectedRow.Selected = Not selectedRow.Selected
                ClearAllFields()
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تبديل التحديد: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' فتح الملف المحدد
    ''' </summary>
    Private Sub OpenSelectedFile()
        Try
            If dgvArchive.SelectedRows.Count > 0 Then
                Dim selectedRow As DataGridViewRow = dgvArchive.SelectedRows(0)
                Dim filePath As String = GetCellValue(selectedRow, "FilePath")

                If Not String.IsNullOrEmpty(filePath) AndAlso File.Exists(filePath) Then
                    Process.Start(filePath)
                Else
                    MessageBox.Show("لم يتم العثور على الملف أو أن المسار غير صالح.", "ملف غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء محاولة فتح الملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '''' <summary>
    '''' معالجة تغيير التحديد في DataGridView
    '''' </summary>
    'Private Sub dgvArchive_SelectionChanged(sender As Object, e As EventArgs) Handles dgvArchive.SelectionChanged
    '    Try
    '        ' تجاهل الحدث أثناء تحميل البيانات
    '        If isLoadingData Then Return

    '        ' التحقق من وجود صفوف محددة
    '        If dgvArchive.SelectedRows.Count > 0 Then


    '            ' إضافة تأثير بصري للصف المحدد
    '            HighlightSelectedRow()
    '            ' تعبئة الحقول بالبيانات الجديدة
    '            FillFieldsFromSelectedRow(dgvArchive.SelectedRows(0))

    '            ' تمكين أدوات التعديل
    '            EnableEditingTools(True)
    '            btnAdd.Enabled = False
    '        Else

    '            ' تعطيل أدوات التعديل إذا لم يكن هناك تحديد صالح
    '            EnableEditingTools(False)
    '            btnAdd.Enabled = clsPermissions.HasPermission(GlobalData.LoggedInUserID, "ArchiveForm", "Add")
    '        End If

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في SelectionChanged: {ex.Message}")
    '        EnableEditingTools(False)
    '        btnAdd.Enabled = clsPermissions.HasPermission(GlobalData.LoggedInUserID, "ArchiveForm", "Add")
    '    End Try
    'End Sub



#End Region

#Region "الصلاحيات"
    ''' <summary>
    ''' تطبيق الصلاحيات على عناصر النموذج
    ''' </summary>
    Protected Overrides Sub ApplyCustomPermissions()
        Try
            Dim currentFormName As String = Me.GetType().Name

            ' التحقق أولاً مما إذا كان المستخدم هو المدير
            If IsCurrentUserAdmin() Then
                ' منح جميع الصلاحيات للمدير
                EnableAllEditingTools(True)
                lblWarning.Visible = False
                Return
            End If

            ' تطبيق الصلاحيات العادية فقط للمستخدمين غير الإداريين
            btnAdd.Enabled = PermissionsManager.Instance.CanAdd(currentFormName)
            btnUpdate.Enabled = PermissionsManager.Instance.CanEdit(currentFormName)
            btnDelete.Enabled = PermissionsManager.Instance.CanDelete(currentFormName)
            btnSearch.Enabled = PermissionsManager.Instance.CanSearch(currentFormName)

            ' عناصر إضافية خاصة بهذا النموذج
            ' ...

            ' عرض رسالة إذا لم تكن هناك صلاحيات كافية للتعديل
            If Not (btnAdd.Enabled OrElse btnUpdate.Enabled OrElse btnDelete.Enabled) Then
                lblWarning.Text = "ملاحظة: لا تملك صلاحيات كافية لتعديل البيانات"
                lblWarning.ForeColor = Color.Red
                lblWarning.Visible = True
            Else
                lblWarning.Visible = False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات: {ex.Message}")
            ' في حالة الخطأ، تعطيل الأدوات الحساسة
            EnableAllEditingTools(False)
            lblWarning.Text = "حدث خطأ في تحميل الصلاحيات"
            lblWarning.ForeColor = Color.Red
            lblWarning.Visible = True
        End Try
    End Sub

    ''' <summary>
    ''' التحقق مما إذا كان المستخدم الحالي هو مدير النظام
    ''' </summary>
    Private Function IsCurrentUserAdmin() As Boolean
        Try
            Dim query As String = "SELECT IsAdmin FROM TbUsers WHERE ID = @UserID AND IsActive = 1"
            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", GlobalData.LoggedInUserID)
                    Dim result As Object = command.ExecuteScalar()
                    Return result IsNot Nothing AndAlso Convert.ToBoolean(result)
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من صلاحية المدير: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' تم habilitation أو تعطيل جميع أدوات التحرير دفعة واحدة
    ''' </summary>
    Private Sub EnableAllEditingTools(enabled As Boolean)
        btnAdd.Enabled = enabled
        btnUpdate.Enabled = enabled
        btnDelete.Enabled = enabled
        btnSearch.Enabled = enabled
        ' يمكن إضافة المزيد من الأزرار حسب الحاجة
    End Sub

    Private Sub CbArchiveNum_CheckedChanged(sender As Object, e As EventArgs) Handles CbArchiveNum.CheckedChanged
        If CbArchiveNum.Checked Then
            txtArchiveNum.ReadOnly = False
        Else
            txtArchiveNum.ReadOnly = True
        End If
    End Sub

    Private Sub TimerMsg_Tick(sender As Object, e As EventArgs) Handles TimerMsg.Tick
        ClearConfirmMsgTools(Me.lblMessage, Me.PicMsg, Me.TimerMsg)
    End Sub
    Private Sub txtFilepath_TextChanged(sender As Object, e As EventArgs) Handles txtFilepath.TextChanged
        LoadFileIntoViewer()
    End Sub
    Public Sub LoadFileIntoViewer()
        Try
            Dim path As String = txtFilepath.Text.Trim()

            If String.IsNullOrEmpty(path) OrElse Not IO.File.Exists(path) Then
                ShowPreviewMessage("الملف غير موجود")
                Return
            End If

            PreviewFile(path)

        Catch ex As Exception
            ShowPreviewMessage("خطأ في عرض الملف")
        End Try
    End Sub


#End Region


End Class
