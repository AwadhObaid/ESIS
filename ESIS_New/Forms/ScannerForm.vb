
'Imports System.IO
'Imports ALquhaliLibrary
'Imports TwainLib

'Public Class ScannerForm

'    Private scannedImages As New List(Of Image)
'    Private selectedScanner As String = My.Settings.DefaultScaner
'    Private Msg As String = ""
'    Public xPath As String
'    Private Sub btnSelectScanner_Click(sender As Object, e As EventArgs) Handles btnSelectScanner.Click
'        lblStatus.Text = "فتح واجهة اختيار الماسح..."
'        Application.DoEvents()

'        '' فتح واجهة TWAIN الأصلية
'        'TwainOperations.GetScanSource()

'        ' الحصول على اسم الماسح المختار
'        selectedScanner = TwainOperations.GetScanSource()

'        If selectedScanner.Trim() <> "" Then
'            btnScan.Enabled = True
'            lblStatus.Text = $"الماسح المحدد: {selectedScanner}"
'        Else
'            btnScan.Enabled = False
'            lblStatus.Text = "لم يتم اختيار ماسح"
'        End If
'    End Sub


'    Private Sub btnScan_Click(sender As Object, e As EventArgs) Handles btnScan.Click
'        Try
'            lblStatus.Text = "جارِ المسح... يرجى الانتظار"
'            Application.DoEvents()

'            Dim files = ScanImages(".jpg", True, selectedScanner)

'            For Each f In files
'                Dim img = Image.FromFile(f)
'                scannedImages.Add(img)
'                lstImages.Items.Add(Path.GetFileName(f))
'            Next
'            'For Each Item In files
'            '    lstImages.Items.Add(Item)
'            'Next

'            If scannedImages.Count > 0 Then
'                picPreview.Image = scannedImages(0)
'                btnSavePDF.Enabled = True
'                btnCrop.Enabled = True
'                btnFilter.Enabled = True
'            End If

'            lblStatus.Text = "تم المسح بنجاح"

'        Catch ex As Exception
'            MsgBox("خطأ أثناء المسح: " & ex.Message)
'        End Try
'    End Sub

'    Private Sub ShowSelectedItemOnCheckedListBox()
'        If lstImages.Items.Count = 0 Or lstImages.SelectedItem = Nothing Then Return
'        Try
'            If lstImages.Items.Count >= 0 Then
'                Dim bt As New Bitmap(lstImages.SelectedItem.ToString)
'                Me.picPreview.Image = bt
'                'Me.lblImagePath.Text = lstImages.SelectedItem.ToString
'            End If
'        Catch ex As Exception
'            Return
'        End Try
'    End Sub

'    Private Sub lstImages_Click(sender As Object, e As EventArgs) Handles lstImages.Click
'        ShowSelectedItemOnCheckedListBox()
'    End Sub

'    Private Sub lstImages_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstImages.SelectedIndexChanged

'        ShowSelectedItemOnCheckedListBox()
'        'If lstImages.SelectedIndex >= 0 Then
'        '    picPreview.Image = scannedImages(lstImages.SelectedIndex)
'        'End If
'    End Sub

'    Private Sub btnPDFPath_Click(sender As Object, e As EventArgs) Handles btnPDFPath.Click
'        Dim FBD As New FolderBrowserDialog
'        If FBD.ShowDialog = DialogResult.OK Then
'            My.Settings.PDFFolderPath = FBD.SelectedPath
'            My.Settings.Save()
'        End If
'    End Sub

'    Private Sub TriggerScanner()
'        Me.lstImages.Items.Add(xPath)
'        Me.lblStatus.Text = lstImages.Items.Count
'        'Me.lblImagePath.Text = xPath
'        If MessageBox.Show("هل تود عمل مسح آخر ؟", "رسالة تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
'            btnScan.PerformClick()
'        End If
'    End Sub

'    Private Sub btnSavePDF_Click(sender As Object, e As EventArgs) Handles btnSavePDF.Click
'        Try
'            If lstImages.Items.Count = 0 Then Return
'            If MyTextNull(txtFileName, "اسم الملف الذي تريد انشائه") Then Return
'            Dim CreatedPDFPath As String = My.Settings.PDFFolderPath
'            If String.IsNullOrEmpty(CreatedPDFPath) Then
'                Msg = "قم بإضافة مسار حفظ ملفات الـ " & vbNewLine & "PDF" & "من شاشة متغيرات الإرشيف"
'                MessageBox.Show(Msg, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                Return
'            End If
'            If My.Computer.FileSystem.FileExists(CreatedPDFPath) = False Then Directory.CreateDirectory(CreatedPDFPath)

'            Dim CreatedFilePath As String = CreatedPDFPath & "\" & txtFileName.Text.Trim() & ".pdf"

'            If My.Computer.FileSystem.FileExists(CreatedFilePath) = True Then
'                Msg = "ان اسم الملف " & vbNewLine & txtFileName.Text.Trim() & vbNewLine &
'                      " موجود مسبقا في مسار الحفظ .. يرجى كتابه اسم اخر للملف ؟ "
'                MessageBox.Show(Msg, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                txtFileName.Focus()
'                Return
'            End If


'            PDFCLS.ConvertImagesToPDF(lstImages, CreatedFilePath)
'        Catch ex As Exception
'            MsgBox("خطأ حفظ PDF: " & ex.Message)
'        End Try


'    End Sub



'    Private Sub btnCrop_Click(sender As Object, e As EventArgs) Handles btnCrop.Click
'        If lstImages.SelectedIndex < 0 Then Return

'        Dim img = scannedImages(lstImages.SelectedIndex)
'        Dim rect As New Rectangle(50, 50, img.Width - 100, img.Height - 100)

'        Dim bmp As New Bitmap(rect.Width, rect.Height)
'        Using g As Graphics = Graphics.FromImage(bmp)
'            g.DrawImage(img, New Rectangle(0, 0, bmp.Width, bmp.Height), rect, GraphicsUnit.Pixel)
'        End Using

'        scannedImages(lstImages.SelectedIndex) = bmp
'        picPreview.Image = bmp

'        lblStatus.Text = "✔ تم القص"
'    End Sub

'    Private Sub btnScanSettings_Click(sender As Object, e As EventArgs) Handles btnScanSettings.Click
'        ImgSettingsForm.ShowDialog()
'    End Sub

'    Private Sub btnDefault_Click(sender As Object, e As EventArgs) Handles btnDefault.Click
'        My.Settings.DefaultScaner = GetScanSource()
'        My.Settings.Save()
'        MessageBox.Show("تم الحفظ")
'    End Sub
'End Class


Imports System.Drawing.Printing
Imports System.IO
Imports System.Windows.Interop
Imports ALquhaliLibrary
Imports TwainLib
Public Class ScannerForm

    Private scannedImages As New List(Of Image)
    Private selectedScanner As String = My.Settings.DefaultScaner
    Private Msg As String = ""


    Private Sub ScannerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblStatus.Text = "جاهز"
        If selectedScanner <> "" Then
            lblStatus.Text = selectedScanner & " الماسح الافتراضي: "
            btnScan.Enabled = True
        End If
    End Sub


    ' -----------------------------------------------------------
    '  ❶ زر اختيار الماسح (يفتح واجهة TWAIN الأصلية)
    ' -----------------------------------------------------------
    Private Sub btnSelectScanner_Click(sender As Object, e As EventArgs) Handles btnSelectScanner.Click
        Try
            lblStatus.Text = "فتح واجهة اختيار الماسح..."
            Application.DoEvents()

            ' هذا السطر يفتح واجهة TWAIN ويعيد اسم الماسح المختار بعد الإغلاق
            selectedScanner = TwainOperations.GetScanSource()

            If Not String.IsNullOrWhiteSpace(selectedScanner) Then
                lblStatus.Text = "الماسح المحدد: " & selectedScanner
                btnScan.Enabled = True
            Else
                lblStatus.Text = "لم يتم اختيار ماسح"
                btnScan.Enabled = False
            End If

        Catch ex As Exception
            MsgBox("خطأ في اختيار الماسح: " & ex.Message)
        End Try
    End Sub




    'Private Sub btnSelectScanner_Click(sender As Object, e As EventArgs) Handles btnSelectScanner.Click
    '    Try
    '        lblStatus.Text = "فتح واجهة اختيار الماسح..."

    '        ' فتح واجهة TWAIN
    '        TwainOperations.GetScanSource()

    '        ' الحصول على الماسح المختار
    '        selectedScanner = TwainOperations.GetScanSource()

    '        If selectedScanner.Trim() <> "" Then
    '            lblStatus.Text = "الماسح المحدد: " & selectedScanner
    '            btnScan.Enabled = True
    '        Else
    '            lblStatus.Text = "لم يتم اختيار ماسح"
    '            btnScan.Enabled = False
    '        End If

    '    Catch ex As Exception
    '        MsgBox("خطأ في اختيار الماسح: " & ex.Message)
    '    End Try
    'End Sub


    ' -----------------------------------------------------------
    '  ❷ زر البدء في المسح
    ' -----------------------------------------------------------
    Private Sub btnScan_Click(sender As Object, e As EventArgs) Handles btnScan.Click
        Try
            lblStatus.Text = "جارِ المسح..."
            lstImages.Items.Clear()
            scannedImages.Clear()

            Dim files = TwainOperations.ScanImages(".jpg", True, selectedScanner)

            For Each f In files
                Dim img = Image.FromFile(f)
                scannedImages.Add(img)
                lstImages.Items.Add(f)
            Next

            If scannedImages.Count > 0 Then
                picPreview.Image = scannedImages(0)
                btnSavePDF.Enabled = True
            End If

            lblStatus.Text = "تم المسح بنجاح"

        Catch ex As Exception
            MsgBox("خطأ أثناء المسح: " & ex.Message)
        End Try
    End Sub


    ' -----------------------------------------------------------
    '  ❸ عرض الصورة المختارة
    ' -----------------------------------------------------------
    Private Sub lstImages_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstImages.SelectedIndexChanged
        Try
            If lstImages.SelectedIndex < 0 Then Return
            picPreview.Image = scannedImages(lstImages.SelectedIndex)
        Catch
        End Try
    End Sub


    ' -----------------------------------------------------------
    '  ❹ زر حفظ PDF (محسّن)
    ' -----------------------------------------------------------
    Private Sub btnSavePDF_Click(sender As Object, e As EventArgs) Handles btnSavePDF.Click
        Try
            If scannedImages.Count = 0 Then Return
            If lstImages.Items.Count = 0 Then Return
            If MyTextNull(txtFileName, "اسم الملف الذي تريد انشائه") Then Return

            Dim CreatedPDFPath As String = My.Settings.PDFFolderPath
            If String.IsNullOrEmpty(CreatedPDFPath) Then
                Msg = "قم بإضافة مسار حفظ ملفات الـ " & vbNewLine & "PDF" & "من شاشة متغيرات الإرشيف"
                MessageBox.Show(Msg, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If Not Directory.Exists(CreatedPDFPath) Then
                Directory.CreateDirectory(CreatedPDFPath)
            End If

            Dim CreatedFilePath As String = Path.Combine(CreatedPDFPath, txtFileName.Text.Trim() & ".pdf")

            If File.Exists(CreatedFilePath) Then
                Msg = "ان اسم الملف " & vbNewLine & txtFileName.Text.Trim() & vbNewLine &
                  " موجود مسبقاً.. يرجى كتابة اسم آخر."
                MessageBox.Show(Msg, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtFileName.Focus()
                Return
            End If

            ' 🔹 إنشاء PDF
            PDFCLS.ConvertImagesToPDF(lstImages, CreatedFilePath)

            ' 🔹 سؤال المستخدم عن الأرشفة
            Dim result = MessageBox.Show("هل تود أرشفة الملف؟", "أرشفة", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                SendFileToArchiveForm(CreatedFilePath)
                Me.Close()
            End If

            lblStatus.Text = "✔ تم حفظ الملف بنجاح"

        Catch ex As Exception
            MessageBox.Show("خطأ أثناء حفظ PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub




    'Private Sub btnSavePDF_Click(sender As Object, e As EventArgs) Handles btnSavePDF.Click
    '    Try
    '        If scannedImages.Count = 0 Then Return

    '        If lstImages.Items.Count = 0 Then Return
    '        If MyTextNull(txtFileName, "اسم الملف الذي تريد انشائه") Then Return

    '        Dim CreatedPDFPath As String = My.Settings.PDFFolderPath
    '        If String.IsNullOrEmpty(CreatedPDFPath) Then
    '            Msg = "قم بإضافة مسار حفظ ملفات الـ " & vbNewLine & "PDF" & "من شاشة متغيرات الإرشيف"
    '            MessageBox.Show(Msg, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Return
    '        End If

    '        If My.Computer.FileSystem.DirectoryExists(CreatedPDFPath) = False Then
    '            Directory.CreateDirectory(CreatedPDFPath)
    '        End If

    '        Dim CreatedFilePath As String = CreatedPDFPath & "\" & txtFileName.Text.Trim() & ".pdf"

    '        If My.Computer.FileSystem.FileExists(CreatedFilePath) = True Then
    '            Msg = "ان اسم الملف " & vbNewLine & txtFileName.Text.Trim() & vbNewLine &
    '              " موجود مسبقا في مسار الحفظ .. يرجى كتابه اسم اخر للملف ؟ "
    '            MessageBox.Show(Msg, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            txtFileName.Focus()
    '            Return
    '        End If

    '        ' 🔹 حفظ ملف PDF
    '        PDFCLS.ConvertImagesToPDF(lstImages, CreatedFilePath)

    '        ' ----------------------------------------------------
    '        ' 🔹 رسالة الأرشفة — الجزء الجديد
    '        ' ----------------------------------------------------
    '        Dim result = MessageBox.Show("هل تود أرشفة الملف ؟", "أرشفة", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

    '        If result = DialogResult.Yes Then
    '            ' إذا كان النموذج مغلقًا، قم بفتحه
    '            Dim newArchiveForm As ArchiveForm = TryCast(Application.OpenForms("ArchiveForm"), ArchiveForm)
    '            If newArchiveForm IsNot Nothing Then

    '            Else
    '                newArchiveForm.txtOriginalPath.Text = CreatedFilePath
    '                newArchiveForm.Show()
    '            End If

    '        End If
    '        ' ----------------------------------------------------

    '        lblStatus.Text = "✔ تم حفظ الملف بنجاح"

    '    Catch ex As Exception
    '        MessageBox.Show("خطأ حفظ PDF: " & ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '    End Try
    'End Sub

    Private Sub SendFileToArchiveForm(filePath As String)

        Dim fileName As String = IO.Path.GetFileNameWithoutExtension(filePath)
        Dim fileExt As String = IO.Path.GetExtension(filePath)
        Dim originalPath As String = filePath   ' المسار الأصلي للملف

        ' البحث عن نموذج ArchiveForm
        Dim archiveForm As ArchiveForm = TryCast(Application.OpenForms("ArchiveForm"), ArchiveForm)

        If archiveForm IsNot Nothing Then

            ' تعبئة الحقول
            archiveForm.txtFilepath.Text = filePath
            archiveForm.txtFilename.Text = fileName
            archiveForm.txtFileType.Text = fileExt
            archiveForm.txtOriginalPath.Text = originalPath

            archiveForm.Show()
            archiveForm.BringToFront()

            ' التحديث اليدوي لضمان عرض الملف
            archiveForm.LoadFileIntoViewer()

        Else

            ' في حالة عدم فتح النموذج
            Dim main As MainForm = TryCast(Application.OpenForms("MainForm"), MainForm)

            If main IsNot Nothing Then
                Dim newArchiveForm As New ArchiveForm()
                newArchiveForm.MdiParent = main
                newArchiveForm.Show()

                ' تعبئة البيانات بعد الفتح
                newArchiveForm.txtFilepath.Text = filePath
                newArchiveForm.txtFilename.Text = fileName
                newArchiveForm.txtFileType.Text = fileExt
                newArchiveForm.txtOriginalPath.Text = originalPath

                ' عرض الملف مباشرة
                newArchiveForm.LoadFileIntoViewer()

                newArchiveForm.BringToFront()
            Else
                MessageBox.Show("لا يمكن العثور على الفورم الرئيسي.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        End If

    End Sub





    'Private Sub btnSavePDF_Click(sender As Object, e As EventArgs) Handles btnSavePDF.Click
    '    Try
    '        If scannedImages.Count = 0 Then Return

    '        If lstImages.Items.Count = 0 Then Return
    '        If MyTextNull(txtFileName, "اسم الملف الذي تريد انشائه") Then Return
    '        Dim CreatedPDFPath As String = My.Settings.PDFFolderPath
    '        If String.IsNullOrEmpty(CreatedPDFPath) Then
    '            Msg = "قم بإضافة مسار حفظ ملفات الـ " & vbNewLine & "PDF" & "من شاشة متغيرات الإرشيف"
    '            MessageBox.Show(Msg, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Return
    '        End If
    '        If My.Computer.FileSystem.FileExists(CreatedPDFPath) = False Then Directory.CreateDirectory(CreatedPDFPath)

    '        Dim CreatedFilePath As String = CreatedPDFPath & "\" & txtFileName.Text.Trim() & ".pdf"

    '        If My.Computer.FileSystem.FileExists(CreatedFilePath) = True Then
    '            Msg = "ان اسم الملف " & vbNewLine & txtFileName.Text.Trim() & vbNewLine &
    '                  " موجود مسبقا في مسار الحفظ .. يرجى كتابه اسم اخر للملف ؟ "
    '            MessageBox.Show(Msg, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            txtFileName.Focus()
    '            Return
    '        End If

    '        PDFCLS.ConvertImagesToPDF(lstImages, CreatedFilePath)

    '    Catch ex As Exception
    '        MessageBox.Show("خطأ حفظ PDF: " & ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '    End Try
    'End Sub

    ' -----------------------------------------------------------
    '  ❼ زر جعل الماسح افتراضيًا
    ' -----------------------------------------------------------
    Private Sub btnDefaultScanner_Click(sender As Object, e As EventArgs) Handles btnDefaultScanner.Click
        If selectedScanner = "" Then
            MessageBox.Show("لم يتم اختيار ماسح", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        My.Settings.DefaultScaner = selectedScanner
        My.Settings.Save()

        lblStatus.Text = "تم حفظ الماسح الافتراضي"
    End Sub

    Private Sub btnPDFPath_Click(sender As Object, e As EventArgs) Handles btnPDFPath.Click
        Dim FBD As New FolderBrowserDialog
        If FBD.ShowDialog = DialogResult.OK Then
            My.Settings.PDFFolderPath = FBD.SelectedPath
            My.Settings.Save()
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            PrintImage(picPreview)
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

#Region "************************* Print *********************************"
    Private Sub PrintPage(ByVal sender As Object, ByVal e As PrintPageEventArgs)
        Try
            e.Graphics.DrawImage(picPreview.Image, 0, 0, picPreview.Width, picPreview.Height)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
    End Sub
    Public Sub PrintImage(ByVal Pb As PictureBox)
        Try
            If Pb.Image Is Nothing Then Return
            If MessageBox.Show("هل تود طباعة الصورة ؟", "رسالة تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Pb.SizeMode = PictureBoxSizeMode.AutoSize
                Dim pd As New PrintDialog
                Dim doc As New PrintDocument
                AddHandler doc.PrintPage, AddressOf PrintPage
                pd.Document = doc
                If pd.ShowDialog() = DialogResult.OK Then doc.Print()
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub txtFileName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFileName.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSavePDF.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

#End Region
    ' -----------------------------------------------------------
    '  ❺ دالة حفظ PDF (بدون مكتبات خارجية – تعمل 100%)
    ' -----------------------------------------------------------
    'Private Sub SaveImagesAsPDF(images As List(Of Image), path As String)
    '    Dim doc As New iTextSharp.text.Document()
    '    Dim writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, New FileStream(path, FileMode.Create))
    '    doc.Open()

    '    For Each img As Image In images
    '        Dim ms As New MemoryStream()
    '        img.Save(ms, Imaging.ImageFormat.Jpeg)
    '        Dim pdfImg = iTextSharp.text.Image.GetInstance(ms.ToArray())
    '        pdfImg.ScaleToFit(doc.PageSize.Width - 20, doc.PageSize.Height - 20)
    '        pdfImg.Alignment = iTextSharp.text.Image.ALIGN_CENTER
    '        doc.Add(pdfImg)
    '    Next

    '    doc.Close()
    '    writer.Close()
    'End Sub


End Class

