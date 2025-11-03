Imports System.Drawing.Printing
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Windows
Imports System.Windows.Media
Imports ClosedXML.Excel
Imports DocumentFormat.OpenXml.Office2010.PowerPoint
Imports DocumentFormat.OpenXml.Wordprocessing
Imports ESIS_New.ESIS_New
Imports iTextSharp.text
Imports iTextSharp.text.pdf


Imports DrawingFont = System.Drawing.Font
Imports DrawingFontStyle = System.Drawing.FontStyle
Imports DrawingBrushes = System.Drawing.Brushes
Imports ITextFont = iTextSharp.text.Font

''' <summary>
''' نموذج لإدارة وعرض سجلات حركة المستخدمين في نظام ESIS
''' </summary>
Public Class UserActivityLogForm
    Inherits BaseFormWithPermissions

    Private activityClass As New ActivityLogClass()
    Private isDataLoaded As Boolean = False
    ' متغيرات السحب والنقل للنافذة
    Private Drag As Boolean = False
    Private MouseX As Integer
    Private MouseY As Integer

    ''' <summary>
    ''' تحميل النموذج
    ''' </summary>
    Private Sub UserActivityLogForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تعيين خصائص النموذج
            Me.Text = "سجلات الحركة - نظام إدارة الوثائق الإلكتروني"
            Me.lblTitle.Text = "سجلات الحركة"

            ' تطبيق الصلاحيات
            ApplyPermissions()

            ' تهيئة النموذج
            SetupForm()

            ' تحميل البيانات
            LoadActivityLogs()
            cmbActionType.SelectedIndex = 0
            ' بدء مؤقت التحديث التلقائي
            TimerRefresh.Start()

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل نموذج سجلات الحركة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '''' <summary>
    '''' تطبيق الصلاحيات على عناصر النموذج
    '''' </summary>
    'Protected Overrides Sub ApplyCustomPermissions()
    '    Try
    '        ' تطبيق الصلاحيات على الأزرار
    '        btnExportExcel.Enabled = PermissionsManager.Instance.CanAdd(Me.GetType().Name)
    '        btnUpdate.Enabled = PermissionsManager.Instance.CanEdit(Me.GetType().Name)
    '        btnDelete.Enabled = PermissionsManager.Instance.CanDelete(Me.GetType().Name)
    '        btnSearch.Enabled = PermissionsManager.Instance.CanSearch(Me.GetType().Name)
    '        btnPrint.Enabled = PermissionsManager.Instance.CanPrint(Me.GetType().Name)

    '        ' عناصر إضافية خاصة بهذا النموذج
    '        ' ...

    '        ' عرض رسالة إذا لم تكن هناك صلاحيات كافية
    '        If Not (btnAdd.Enabled OrElse btnUpdate.Enabled OrElse btnDelete.Enabled) Then
    '            lblWarning.Text = "ملاحظة: لا تملك صلاحيات كافية لتعديل البيانات"
    '            lblWarning.ForeColor = Color.Red
    '            lblWarning.Visible = True
    '        Else
    '            lblWarning.Visible = False
    '        End If

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' تهيئة النموذج
    ''' </summary>
    Private Sub SetupForm()
        Try
            ' إعداد DataGridView
            SetupDataGridView()

            ' تعيين قيم افتراضية للبحث باستخدام تاريخ اليوم فقط
            dtpStartDate.Value = DateTime.Now.Date.AddDays(-7)
            dtpEndDate.Value = DateTime.Now.Date

            ' تعيين حدود البحث
            dtpStartDate.MaxDate = DateTime.Now.Date
            dtpEndDate.MaxDate = DateTime.Now.Date

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة النموذج: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' إعداد جدول العرض
    ''' </summary>
    Private Sub SetupDataGridView()
        Try
            ' إعداد الخصائص الأساسية
            dgvActivityLog.AutoGenerateColumns = False
            dgvActivityLog.DataSource = Nothing

            ' تنظيف الأعمدة القديمة
            dgvActivityLog.Columns.Clear()

            ' إضافة الأعمدة
            AddTextBoxColumn("LogID", "معرّف العملية", 50, DataGridViewContentAlignment.MiddleCenter)
            AddTextBoxColumn("UserName", "المستخدم", 60, DataGridViewContentAlignment.MiddleCenter)
            AddTextBoxColumn("ActionType", "العملية", 80, DataGridViewContentAlignment.MiddleCenter)
            AddTextBoxColumn("TableNameArabic", "اسم الجدول", 80, DataGridViewContentAlignment.MiddleCenter) ' ← استخدام العمود الجديد
            AddTextBoxColumn("RecordID", "المعرّف", 50, DataGridViewContentAlignment.MiddleCenter)
            AddTextBoxColumn("ActionDetails", "التفاصيل", 600, DataGridViewContentAlignment.MiddleCenter)
            AddTextBoxColumn("IPAddress", "عنوان IP", 90, DataGridViewContentAlignment.MiddleCenter)
            AddTextBoxColumn("ActionDate", "التاريخ", 130, DataGridViewContentAlignment.MiddleCenter)

            ' إعداد الخصائص العامة
            dgvActivityLog.EnableHeadersVisualStyles = False

            ' استخدام System.Drawing.Colors و System.Drawing.Font بشكل صريح
            dgvActivityLog.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue
            dgvActivityLog.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White
            dgvActivityLog.ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Cairo", 9, System.Drawing.FontStyle.Bold)
            dgvActivityLog.DefaultCellStyle.Font = New System.Drawing.Font("Cairo", 9)
            dgvActivityLog.GridColor = System.Drawing.Color.LightGray
            dgvActivityLog.BorderStyle = BorderStyle.None

            ' ربط قائمة السياق
            dgvActivityLog.ContextMenuStrip = ContextMenuStrip1

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إعداد جدول العرض: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' إضافة عمود نصي إلى جدول العرض
    ''' </summary>
    Private Sub AddTextBoxColumn(columnName As String, headerText As String, width As Integer, alignment As DataGridViewContentAlignment)
        Try
            Dim column As New DataGridViewTextBoxColumn()
            column.DataPropertyName = columnName
            column.HeaderText = headerText
            column.Name = columnName
            column.Width = width
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            column.DefaultCellStyle.Alignment = alignment
            column.SortMode = DataGridViewColumnSortMode.Automatic
            dgvActivityLog.Columns.Add(column)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إضافة العمود {columnName}: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' ترجمة نوع العملية من الإنجليزية إلى العربية
    ''' </summary>
    Private Function TranslateActionType(actionType As String) As String
        Select Case actionType?.Trim().ToLower()
            Case "add"
                Return "إضافة"
            Case "edit", "update"
                Return "تعديل"
            Case "delete"
                Return "حذف"
            Case "search"
                Return "بحث"
            Case "print"
                Return "طباعة"
            Case "login"
                Return "تسجيل دخول"
            Case "logout"
                Return "تسجيل خروج"
            Case "failed_add"
                Return "فشل في الإضافة"
            Case "failed_edit"
                Return "فشل في التعديل"
            Case "failed_delete"
                Return "فشل في الحذف"
            Case Else
                Return If(String.IsNullOrEmpty(actionType), "غير معروف", actionType)
        End Select
    End Function


    ''' <summary>
    ''' تحميل سجلات الحركة
    ''' </summary>
    Private Async Sub LoadActivityLogs()
        Try
            ' عرض حالة التحميل
            SetLoadingState(True)

            ' تحميل البيانات بشكل غير متزامن
            Dim dataTable As DataTable = Await Task.Run(AddressOf activityClass.GetActivityLogs)

            ' عرض البيانات
            DisplayActivityLogs(dataTable)

            ' تحديث حالة التحميل
            isDataLoaded = True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل سجلات الحركة: {ex.Message}")
            MessageBox.Show($"فشل في تحميل سجلات الحركة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetLoadingState(False)
        End Try
    End Sub

    '''' <summary>
    '''' عرض سجلات الحركة مع الترجمة
    '''' </summary>
    'Private Sub DisplayActivityLogs(dataTable As DataTable)
    '    Try
    '        If dataTable IsNot Nothing AndAlso dataTable.Rows.Count > 0 Then
    '            ' إنشاء جدول مؤقت للعرض
    '            Dim displayTable As New DataTable()

    '            ' نسخ الهيكل مع تغيير عنوان العمود
    '            For Each column As DataColumn In dataTable.Columns
    '                Dim newColumn As New DataColumn(column.ColumnName, column.DataType)
    '                displayTable.Columns.Add(newColumn)
    '            Next

    '            ' تعديل عنوان عمود ActionType
    '            displayTable.Columns("ActionType").ColumnName = "نوع_العملية"
    '            displayTable.Columns("ActionType").Caption = "نوع العملية"

    '            ' ملء البيانات مع الترجمة
    '            For Each row As DataRow In dataTable.Rows
    '                Dim newRow As DataRow = displayTable.NewRow()
    '                For Each column As DataColumn In dataTable.Columns
    '                    If column.ColumnName = "ActionType" Then
    '                        newRow(column.ColumnName) = TranslateActionType(row(column).ToString())
    '                    Else
    '                        newRow(column.ColumnName) = row(column)
    '                    End If
    '                Next
    '                displayTable.Rows.Add(newRow)
    '            Next

    '            ' ربط الجدول بـ DataGridView
    '            dgvActivityLog.DataSource = displayTable

    '            ' تحديث عدد السجلات
    '            UpdateRecordCount(displayTable.Rows.Count)

    '            ' تمكين أزرار التصدير
    '            EnableExportButtons(True)
    '        Else
    '            dgvActivityLog.DataSource = Nothing
    '            UpdateRecordCount(0)
    '            EnableExportButtons(False)
    '        End If

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في عرض سجلات الحركة: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' عرض سجلات الحركة مع الترجمة
    ''' </summary>
    Private Sub DisplayActivityLogs(dataTable As DataTable)
        Try
            If dataTable IsNot Nothing AndAlso dataTable.Rows.Count > 0 Then
                ' إنشاء جدول مؤقت للعرض
                Dim displayTable As New DataTable()

                ' نسخ الهيكل مع التحقق من أسماء الأعمدة
                For Each column As DataColumn In dataTable.Columns
                    Dim newColumn As New DataColumn(column.ColumnName, column.DataType)
                    displayTable.Columns.Add(newColumn)
                Next

                ' ملء البيانات مع الترجمة
                For Each row As DataRow In dataTable.Rows
                    Dim newRow As DataRow = displayTable.NewRow()
                    For Each column As DataColumn In dataTable.Columns
                        If column.ColumnName = "ActionType" OrElse column.ColumnName.ToLower() = "actiontype" Then
                            newRow(column.ColumnName) = TranslateActionType(row(column).ToString())
                        Else
                            newRow(column.ColumnName) = row(column)
                        End If
                    Next
                    displayTable.Rows.Add(newRow)
                Next

                ' ربط الجدول بـ DataGridView
                dgvActivityLog.DataSource = displayTable

                ' تحديث عدد السجلات
                UpdateRecordCount(displayTable.Rows.Count)

                ' تمكين أزرار التصدير
                EnableExportButtons(True)
            Else
                dgvActivityLog.DataSource = Nothing
                UpdateRecordCount(0)
                EnableExportButtons(False)
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في عرض سجلات الحركة: {ex.Message}")
        End Try
    End Sub




    ''' <summary>
    ''' تصفية السجلات بناءً على معايير البحث
    ''' </summary>
    Private Async Sub FilterActivityLogs()
        Try
            ' عرض حالة التحميل
            SetLoadingState(True)

            ' جمع معايير البحث
            Dim criteria As New ActivitySearchCriteria With {
                .StartDate = dtpStartDate.Value.Date,
                .EndDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1),
                .UserName = txtUserName.Text.Trim(),
                .RecordID = If(String.IsNullOrEmpty(txtRecordID.Text), 0, CInt(Val(txtRecordID.Text))),
                .TableName = txtTableName.Text.Trim(),
                .ActionType = If(cmbActionType.SelectedIndex = 0, "", cmbActionType.SelectedItem.ToString())
            }

            ' تصفية البيانات بشكل غير متزامن
            Dim filteredData As DataTable = Await Task.Run(Function() activityClass.FilterActivityLogs(criteria))

            ' عرض النتائج
            DisplayActivityLogs(filteredData)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تصفية سجلات الحركة: {ex.Message}")
            MessageBox.Show($"فشل في تصفية السجلات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetLoadingState(False)
        End Try
    End Sub

    ''' <summary>
    ''' تعيين حالة التحميل
    ''' </summary>
    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            ' تعطيل/تمكين عناصر التحكم
            btnRefresh.Enabled = Not isLoading
            btnClearFilter.Enabled = Not isLoading
            btnExportExcel.Enabled = Not isLoading

            ' تحديث المؤشر
            If isLoading Then
                Me.Cursor = Cursors.WaitCursor
            Else
                Me.Cursor = Cursors.Default
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين حالة التحميل: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث عدد السجلات
    ''' </summary>
    Private Sub UpdateRecordCount(count As Integer)
        Try
            lblTotalRecords.Text = $"إجمالي السجلات: {count:N0}"

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عدد السجلات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تمكين أزرار التصدير
    ''' </summary>
    Private Sub EnableExportButtons(enable As Boolean)
        Try
            btnExportExcel.Enabled = enable

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تمكين أزرار التصدير: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر التحديث
    ''' </summary>
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            LoadActivityLogs()

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحديث السجلات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر مسح الفلاتر
    ''' </summary>
    Private Sub btnClearFilter_Click(sender As Object, e As EventArgs) Handles btnClearFilter.Click
        Try
            ' إعادة تعيين معايير البحث
            dtpStartDate.Value = DateTime.Now.Date.AddDays(-7)
            dtpEndDate.Value = DateTime.Now.Date
            txtUserName.Text = ""
            txtRecordID.Text = ""
            txtTableName.Text = ""
            cmbActionType.SelectedIndex = 0

            ' إعادة تحميل جميع السجلات
            LoadActivityLogs()

        Catch ex As Exception
            MessageBox.Show($"خطأ في مسح الفلاتر: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر التصدير إلى Excel
    ''' </summary>
    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        Try
            ExportToExcelFile()

        Catch ex As Exception
            MessageBox.Show($"خطأ في التصدير إلى Excel: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    ''' <summary>
    ''' تصدير البيانات إلى ملف Excel (XLSX)
    ''' </summary>
    Private Sub ExportToExcelFile()
        Try
            Using saveFileDialog As New SaveFileDialog()
                saveFileDialog.Filter = "ملفات Excel (*.xlsx)|*.xlsx|جميع الملفات (*.*)|*.*"
                saveFileDialog.FilterIndex = 1
                saveFileDialog.RestoreDirectory = True
                saveFileDialog.FileName = $"سجلات_الحركة_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"

                If saveFileDialog.ShowDialog() = DialogResult.OK Then
                    Dim filePath As String = saveFileDialog.FileName

                    Using workbook As New XLWorkbook()
                        Dim worksheet = workbook.Worksheets.Add("سجلات الحركة")

                        ' تحويل مصدر البيانات إلى DataTable
                        Dim dataTable As DataTable
                        If TypeOf dgvActivityLog.DataSource Is DataTable Then
                            dataTable = DirectCast(dgvActivityLog.DataSource, DataTable)
                        ElseIf TypeOf dgvActivityLog.DataSource Is DataView Then
                            dataTable = DirectCast(dgvActivityLog.DataSource, DataView).ToTable()
                        Else
                            Throw New ArgumentException("نوع المصدر غير مدعوم")
                        End If

                        ' تعبئة البيانات
                        worksheet.Cell(1, 1).InsertTable(dataTable)

                        ' تنسيق الرأس
                        Dim headerRow = worksheet.Row(1)
                        headerRow.Style.Font.Bold = True
                        headerRow.Style.Fill.SetBackgroundColor(XLColor.FromColor(System.Drawing.Color.SteelBlue))
                        headerRow.Style.Font.FontColor = XLColor.White

                        ' ضبط عرض الأعمدة
                        worksheet.Columns().AdjustToContents()

                        ' حفظ الملف
                        workbook.SaveAs(filePath)

                        MessageBox.Show($"تم تصدير السجلات بنجاح إلى:{vbNewLine}{filePath}",
                                  "تصدير ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show($"فشل في التصدير إلى Excel: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '''' <summary>
    '''' تصدير البيانات إلى CSV مع دعم اللغة العربية
    '''' </summary>
    'Private Sub ExportToExcel(dataSource As Object)
    '    Try
    '        Using saveFileDialog As New SaveFileDialog()
    '            saveFileDialog.Filter = "ملفات CSV (*.csv)|*.csv|جميع الملفات (*.*)|*.*"
    '            saveFileDialog.FilterIndex = 1
    '            saveFileDialog.RestoreDirectory = True
    '            saveFileDialog.FileName = $"سجلات_الحركة_{DateTime.Now:yyyyMMdd_HHmmss}.csv"

    '            If saveFileDialog.ShowDialog() = DialogResult.OK Then
    '                Dim filePath As String = saveFileDialog.FileName

    '                ' استخدام ترميز UTF-8 مع BOM لدعم اللغة العربية
    '                Using writer As New StreamWriter(filePath, False, New UTF8Encoding(True))
    '                    ' كتابة رؤوس الأعمدة
    '                    For i As Integer = 0 To dgvActivityLog.Columns.Count - 1
    '                        If i > 0 Then
    '                            writer.Write(",")
    '                        End If

    '                        Dim headerText As String = dgvActivityLog.Columns(i).HeaderText
    '                        ' وضع النص بين علامتي اقتباس
    '                        writer.Write("""" & headerText.Replace("""", """""") & """")
    '                    Next
    '                    writer.WriteLine()

    '                    ' كتابة بيانات الصفوف
    '                    For Each row As DataGridViewRow In dgvActivityLog.Rows
    '                        For i As Integer = 0 To dgvActivityLog.Columns.Count - 1
    '                            If i > 0 Then
    '                                writer.Write(",")
    '                            End If

    '                            Dim value As String = ""
    '                            If row.Cells(i).Value IsNot Nothing Then
    '                                value = row.Cells(i).Value.ToString()
    '                            End If

    '                            ' وضع القيم بين علامتي اقتباس إذا احتوت على فواصل أو كانت باللغة العربية
    '                            If value.Contains(",") Or value.Contains(vbCrLf) Or IsArabicText(value) Then
    '                                writer.Write("""" & value.Replace("""", """""") & """")
    '                            Else
    '                                writer.Write(value)
    '                            End If
    '                        Next
    '                        writer.WriteLine()
    '                    Next
    '                End Using

    '                ' إظهار رسالة النجاح
    '                MessageBox.Show($"تم تصدير السجلات بنجاح إلى:{vbNewLine}{filePath}",
    '                          "تصدير ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            End If
    '        End Using

    '    Catch ex As Exception
    '        Throw New Exception($"فشل في تصدير البيانات إلى CSV: {ex.Message}")
    '    End Try
    'End Sub

    '''' <summary>
    '''' تصدير البيانات إلى ملف PDF
    '''' </summary>
    'Private Sub ExportToPDF()
    '    Try
    '        Using saveFileDialog As New SaveFileDialog()
    '            saveFileDialog.Filter = "ملفات PDF (*.pdf)|*.pdf|جميع الملفات (*.*)|*.*"
    '            saveFileDialog.FilterIndex = 1
    '            saveFileDialog.RestoreDirectory = True
    '            saveFileDialog.FileName = $"سجلات_الحركة_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"

    '            If saveFileDialog.ShowDialog() = DialogResult.OK Then
    '                Dim filePath As String = saveFileDialog.FileName

    '                Using document As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate())
    '                    Using writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(filePath, FileMode.Create))
    '                        document.Open()

    '                        ' استخدام iTextSharp.text.Paragraph بشكل صريح
    '                        Dim titleFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE)
    '                        Dim titleParagraph As New iTextSharp.text.Paragraph("سجلات الحركة", titleFont)
    '                        titleParagraph.Alignment = Element.ALIGN_CENTER
    '                        titleParagraph.SpacingAfter = 20
    '                        document.Add(titleParagraph)

    '                        ' إضافة تاريخ التصدير
    '                        Dim dateFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.GRAY)
    '                        Dim dateParagraph As New iTextSharp.text.Paragraph($"تاريخ التصدير: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", dateFont)
    '                        dateParagraph.Alignment = Element.ALIGN_RIGHT
    '                        dateParagraph.SpacingAfter = 20
    '                        document.Add(dateParagraph)

    '                        ' إنشاء جدول
    '                        Dim table As New PdfPTable(dgvActivityLog.Columns.Count)
    '                        table.WidthPercentage = 100

    '                        ' إضافة رؤوس الأعمدة
    '                        For Each column As DataGridViewColumn In dgvActivityLog.Columns
    '                            Dim headerCell As New PdfPCell(New Phrase(column.HeaderText, New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)))
    '                            headerCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY
    '                            headerCell.HorizontalAlignment = Element.ALIGN_CENTER
    '                            table.AddCell(headerCell)
    '                        Next

    '                        ' إضافة بيانات الصفوف
    '                        For Each row As DataGridViewRow In dgvActivityLog.Rows
    '                            For i As Integer = 0 To dgvActivityLog.Columns.Count - 1
    '                                Dim cellValue As String = If(row.Cells(i).Value?.ToString(), "")
    '                                Dim cell As New PdfPCell(New Phrase(cellValue, New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL)))
    '                                cell.HorizontalAlignment = Element.ALIGN_CENTER
    '                                table.AddCell(cell)
    '                            Next
    '                        Next

    '                        document.Add(table)
    '                        document.Close()
    '                    End Using
    '                End Using

    '                MessageBox.Show($"تم تصدير السجلات بنجاح إلى:{vbNewLine}{filePath}",
    '                          "تصدير ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            End If
    '        End Using

    '    Catch ex As Exception
    '        MessageBox.Show($"فشل في التصدير إلى PDF: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub


    ''' <summary>
    ''' تصدير البيانات إلى ملف PDF
    ''' </summary>
    Private Sub ExportToPDF()
        Try
            Using saveFileDialog As New SaveFileDialog()
                saveFileDialog.Filter = "ملفات PDF (*.pdf)|*.pdf|جميع الملفات (*.*)|*.*"
                saveFileDialog.FilterIndex = 1
                saveFileDialog.RestoreDirectory = True
                saveFileDialog.FileName = $"سجلات_الحركة_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"

                If saveFileDialog.ShowDialog() = DialogResult.OK Then
                    Dim filePath As String = saveFileDialog.FileName

                    Using document As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate())
                        Using writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(filePath, FileMode.Create))
                            document.Open()

                            ' إضافة عنوان
                            Dim titleFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE)
                            Dim titleParagraph As New iTextSharp.text.Paragraph("سجلات الحركة", titleFont)
                            titleParagraph.Alignment = Element.ALIGN_CENTER
                            titleParagraph.SpacingAfter = 20
                            document.Add(titleParagraph)

                            ' إضافة تاريخ التصدير
                            Dim dateFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.GRAY)
                            Dim dateParagraph As New iTextSharp.text.Paragraph($"تاريخ التصدير: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", dateFont)
                            dateParagraph.Alignment = Element.ALIGN_RIGHT
                            dateParagraph.SpacingAfter = 20
                            document.Add(dateParagraph)

                            ' ✅ استخدام مصدر البيانات الأصلي للحصول على الأعمدة والصفوف
                            Dim dataTable As DataTable = Nothing

                            If TypeOf dgvActivityLog.DataSource Is DataTable Then
                                dataTable = DirectCast(dgvActivityLog.DataSource, DataTable)
                            ElseIf TypeOf dgvActivityLog.DataSource Is DataView Then
                                dataTable = DirectCast(dgvActivityLog.DataSource, DataView).Table
                            End If

                            ' التحقق من وجود البيانات
                            If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
                                MessageBox.Show("لا توجد بيانات لتصديرها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Return
                            End If

                            ' ✅ إنشاء جدول بناءً على عدد أعمدة المصدر
                            Dim table As New PdfPTable(dataTable.Columns.Count)
                            table.WidthPercentage = 100

                            ' ✅ إضافة رؤوس الأعمدة من المصدر
                            For Each column As DataColumn In dataTable.Columns
                                Dim headerText As String = ""

                                ' الحصول على النص المعروض في DataGridView إذا كان متاحًا
                                Dim dgvColumn As DataGridViewColumn = dgvActivityLog.Columns.Cast(Of DataGridViewColumn)() _
                                                                  .FirstOrDefault(Function(c) c.DataPropertyName = column.ColumnName)

                                If dgvColumn IsNot Nothing AndAlso Not String.IsNullOrEmpty(dgvColumn.HeaderText) Then
                                    headerText = TranslateActionTypeIfNeeded(dgvColumn.HeaderText, column.ColumnName)
                                Else
                                    headerText = TranslateActionTypeIfNeeded(column.ColumnName, column.ColumnName)
                                End If

                                Dim headerCell As New PdfPCell(New Phrase(headerText, New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)))
                                headerCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY
                                headerCell.HorizontalAlignment = Element.ALIGN_CENTER
                                table.AddCell(headerCell)
                            Next

                            ' ✅ إضافة بيانات الصفوف من المصدر
                            For Each row As DataRow In dataTable.Rows
                                For i As Integer = 0 To dataTable.Columns.Count - 1
                                    Dim cellValue As String = If(row(i) IsNot Nothing, row(i).ToString(), "")

                                    ' ترجمة نوع العملية إذا لزم الأمر
                                    If dataTable.Columns(i).ColumnName = "ActionType" Then
                                        cellValue = TranslateActionType(cellValue)
                                    End If

                                    Dim cell As New PdfPCell(New Phrase(cellValue, New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL)))
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER
                                    table.AddCell(cell)
                                Next
                            Next

                            document.Add(table)
                            document.Close()
                        End Using
                    End Using

                    MessageBox.Show($"تم تصدير السجلات بنجاح إلى:{vbNewLine}{filePath}",
                              "تصدير ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show($"فشل في التصدير إلى PDF: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' ترجمة النص إذا كان العمود هو ActionType
    ''' </summary>
    Private Function TranslateActionTypeIfNeeded(displayText As String, columnName As String) As String
        If columnName = "ActionType" Then
            Return TranslateActionType(displayText)
        Else
            Return displayText
        End If
    End Function

    ''' <summary>
    ''' طباعة DataGridView مباشرة
    ''' </summary>
    Private Sub PrintDataGridView()
        Try
            Dim printDocument As New PrintDocument()
            AddHandler printDocument.PrintPage, AddressOf PrintPageHandler

            ' إعداد طابعة
            Dim printDialog As New PrintDialog()
            printDialog.Document = printDocument

            If printDialog.ShowDialog() = DialogResult.OK Then
                printDocument.Print()
            End If

        Catch ex As Exception
            MessageBox.Show($"فشل في الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالج صفحة الطباعة
    ''' </summary>
    Private Sub PrintPageHandler(sender As Object, e As PrintPageEventArgs)
        Try
            ' استخدام System.Drawing بشكل صريح
            Dim font As New System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular)
            Dim boldFont As New System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
            Dim headerFont As New System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)

            ' تحديد بداية الطباعة من اليمين
            Dim startX As Single = e.MarginBounds.Right  ' بدء من اليمين بدلًا من اليسار
            Dim startY As Single = e.MarginBounds.Top
            Dim offsetY As Single = 0

            ' إضافة عنوان (يجب أن يكون في اليمين)
            Dim titleFormat As New StringFormat() With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center
        }
            e.Graphics.DrawString("سجلات الحركة", headerFont, System.Drawing.Brushes.Blue,
                            New RectangleF(e.MarginBounds.Left, startY + offsetY, e.MarginBounds.Width, 30),
                            titleFormat)
            offsetY += 30

            ' إضافة تاريخ الطباعة (في اليمين)
            Dim dateFormat As New StringFormat() With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center
        }
            e.Graphics.DrawString($"تاريخ الطباعة: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", font, System.Drawing.Brushes.Gray,
                            New RectangleF(e.MarginBounds.Left, startY + offsetY, e.MarginBounds.Width, 20),
                            dateFormat)
            offsetY += 30

            ' حساب عرض الأعمدة
            Dim totalWidth As Single = 0
            Dim columnWidths As New List(Of Single)

            For Each column As DataGridViewColumn In dgvActivityLog.Columns
                Dim width As Single = column.Width * 0.75F ' تحويل من بكسل إلى نقطة
                columnWidths.Add(width)
                totalWidth += width
            Next

            ' تعديل العروض إذا كانت أكبر من الصفحة
            If totalWidth > e.MarginBounds.Width Then
                Dim scale As Single = e.MarginBounds.Width / totalWidth
                For i As Integer = 0 To columnWidths.Count - 1
                    columnWidths(i) *= scale
                Next
            End If

            ' رسم رؤوس الأعمدة من اليمين إلى اليسار
            Dim currentX As Single = startX
            For i As Integer = 0 To dgvActivityLog.Columns.Count - 1
                ' تقليل X لبدء من اليمين
                currentX -= columnWidths(i)

                ' استخدام System.Drawing.Brushes
                e.Graphics.FillRectangle(System.Drawing.Brushes.LightGray, currentX, startY + offsetY, columnWidths(i), 25)
                e.Graphics.DrawRectangle(System.Drawing.Pens.Black, currentX, startY + offsetY, columnWidths(i), 25)

                ' محاذاة النص إلى الوسط
                Dim format As New StringFormat() With {
                .Alignment = StringAlignment.Center,
                .LineAlignment = StringAlignment.Center
            }

                e.Graphics.DrawString(dgvActivityLog.Columns(i).HeaderText, boldFont, System.Drawing.Brushes.Black,
                                New RectangleF(currentX, startY + offsetY, columnWidths(i), 25),
                                format)
            Next

            offsetY += 25

            ' رسم صفوف البيانات من اليمين إلى اليسار
            For Each row As DataGridViewRow In dgvActivityLog.Rows
                currentX = startX
                For i As Integer = 0 To dgvActivityLog.Columns.Count - 1
                    currentX -= columnWidths(i)

                    Dim cellValue As String = If(row.Cells(i).Value?.ToString(), "")

                    e.Graphics.DrawRectangle(System.Drawing.Pens.Black, currentX, startY + offsetY, columnWidths(i), 20)

                    Dim format As New StringFormat() With {
                    .Alignment = StringAlignment.Center,
                    .LineAlignment = StringAlignment.Center
                }

                    e.Graphics.DrawString(cellValue, font, System.Drawing.Brushes.Black,
                                    New RectangleF(currentX, startY + offsetY, columnWidths(i), 20),
                                    format)
                Next
                offsetY += 20

                ' التحقق من أن الصفحة لم تمتلئ
                If startY + offsetY > e.MarginBounds.Bottom Then
                    e.HasMorePages = True
                    Return
                End If
            Next

            e.HasMorePages = False

        Catch ex As Exception
            MessageBox.Show($"خطأ في الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ''' <summary>
    ''' التحقق مما إذا كانت السلسلة تحتوي على نص عربي
    ''' </summary>
    Private Function IsArabicText(text As String) As Boolean
        If String.IsNullOrEmpty(text) Then Return False

        For Each c As Char In text
            If c >= ChrW(&H600) AndAlso c <= ChrW(&H6FF) Then ' نطاق الحروف العربية
                Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' معالجة مؤقت التحديث التلقائي
    ''' </summary>
    Private Sub TimerRefresh_Tick(sender As Object, e As EventArgs) Handles TimerRefresh.Tick
        Try
            ' تحديث البيانات كل 5 دقائق إذا كانت البيانات قد تم تحميلها بالفعل
            If isDataLoaded Then
                LoadActivityLogs()
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث السجلات التلقائي: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر الإغلاق
    ''' </summary>
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Try
            Me.Close()

        Catch ex As Exception
            MessageBox.Show($"خطأ في إغلاق النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة النقر بزر الماوس الأيمن
    ''' </summary>
    Private Sub dgvActivityLog_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvActivityLog.MouseDown
        Try
            If e.Button = MouseButtons.Right Then
                Dim hitTestInfo = dgvActivityLog.HitTest(e.X, e.Y)

                If hitTestInfo.RowIndex >= 0 Then
                    dgvActivityLog.ClearSelection()
                    dgvActivityLog.Rows(hitTestInfo.RowIndex).Selected = True
                End If
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة النقر بزر الماوس الأيمن: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة أمر نسخ السطر المحدد
    ''' </summary>
    Private Sub mnuCopySelected_Click(sender As Object, e As EventArgs) Handles mnuCopySelected.Click
        Try
            If dgvActivityLog.SelectedRows.Count > 0 Then
                Dim selectedRow = dgvActivityLog.SelectedRows(0)
                Dim rowText As New StringBuilder()

                For i As Integer = 0 To selectedRow.Cells.Count - 1
                    If i > 0 Then rowText.Append(vbTab)
                    rowText.Append(selectedRow.Cells(i).Value?.ToString())
                Next

                Clipboard.SetText(rowText.ToString())
                MessageBox.Show("تم نسخ السطر المحدد إلى الحافظة", "نسخ ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في نسخ السطر: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة أمر نسخ الكل
    ''' </summary>
    Private Sub mnuCopyAll_Click(sender As Object, e As EventArgs) Handles mnuCopyAll.Click
        Try
            If dgvActivityLog.Rows.Count > 0 Then
                Dim sb As New StringBuilder()

                ' إضافة رؤوس الأعمدة
                For i As Integer = 0 To dgvActivityLog.Columns.Count - 1
                    If i > 0 Then sb.Append(vbTab)
                    sb.Append(dgvActivityLog.Columns(i).HeaderText)
                Next
                sb.AppendLine()

                ' إضافة بيانات الصفوف
                For Each row As DataGridViewRow In dgvActivityLog.Rows
                    For i As Integer = 0 To dgvActivityLog.Columns.Count - 1
                        If i > 0 Then sb.Append(vbTab)
                        sb.Append(row.Cells(i).Value?.ToString())
                    Next
                    sb.AppendLine()
                Next

                Clipboard.SetText(sb.ToString())
                MessageBox.Show("تم نسخ جميع البيانات إلى الحافظة", "نسخ ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في نسخ الكل: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة أمر تصدير المحدد إلى Excel
    ''' </summary>
    Private Sub mnuExportSelected_Click(sender As Object, e As EventArgs) Handles mnuExportSelected.Click
        Try
            If dgvActivityLog.SelectedRows.Count > 0 Then
                ' إنشاء DataTable تحتوي فقط على الصفوف المحددة
                Dim selectedData As New DataTable()

                ' إضافة الأعمدة
                For Each column As DataGridViewColumn In dgvActivityLog.Columns
                    selectedData.Columns.Add(column.Name, GetType(String))
                Next

                ' إضافة الصفوف المحددة
                For Each row As DataGridViewRow In dgvActivityLog.SelectedRows
                    Dim rowData(selectedData.Columns.Count - 1) As Object
                    For i As Integer = 0 To selectedData.Columns.Count - 1
                        rowData(i) = row.Cells(i).Value?.ToString()
                    Next
                    selectedData.Rows.Add(rowData)
                Next

                ExportToExcelFile()
            Else
                MessageBox.Show("يرجى تحديد صفوف للتصدير", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في تصدير المحدد: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة أمر تصدير الكل إلى Excel
    ''' </summary>
    Private Sub mnuExportAll_Click(sender As Object, e As EventArgs) Handles mnuExportAll.Click
        Try
            ExportToExcelFile()

        Catch ex As Exception
            MessageBox.Show($"خطأ في تصدير الكل: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تنظيف الموارد عند إغلاق النموذج
    ''' </summary>
    Private Sub UserActivityLogForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            ' إيقاف المؤقت
            TimerRefresh.Stop()

            ' تفريغ البيانات
            If dgvActivityLog.DataSource IsNot Nothing Then
                Dim dt As DataTable = TryCast(dgvActivityLog.DataSource, DataTable)
                If dt IsNot Nothing Then
                    dt.Dispose()
                End If
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تنظيف الموارد: {ex.Message}")
        End Try
    End Sub

    Private Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        ExportToPDF()
    End Sub

    Private Sub btnPrintTable_Click(sender As Object, e As EventArgs) Handles btnPrintTable.Click
        PrintDataGridView()
    End Sub

    ' أحداث السحب والنقل للنافذة
    Private Sub PanelTitle_MouseDown(sender As Object, e As MouseEventArgs) Handles PanelTitle.MouseDown
        Drag = True
        MouseX = Cursor.Position.X - Me.Left
        MouseY = Cursor.Position.Y - Me.Top
    End Sub

    Private Sub PanelTitle_MouseMove(sender As Object, e As MouseEventArgs) Handles PanelTitle.MouseMove
        If Drag Then
            Me.Top = Cursor.Position.Y - MouseY
            Me.Left = Cursor.Position.X - MouseX
        End If
    End Sub

    Private Sub PanelTitle_MouseUp(sender As Object, e As MouseEventArgs) Handles PanelTitle.MouseUp
        Drag = False
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر عرض التقرير
    ''' </summary>
    Private Sub btnReprt_Click(sender As Object, e As EventArgs) Handles btnReprt.Click
        Try
            ' التحقق من صلاحية عرض التقارير
            If Not CheckPermission("ViewReports", "عرض التقارير") Then
                MessageBox.Show("ليس لديك صلاحية عرض التقارير. يرجى الاتصال بمدير النظام.",
                          "صلاحيات محدودة",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information)
                Return
            End If

            ' التحقق مما إذا كان النموذج مفتوحًا بالفعل
            For Each form As Form In Me.MdiChildren
                If TypeOf form Is UserActivityLogReportForm Then
                    form.BringToFront()
                    Return
                End If
            Next

            ' إنشاء نموذج التقرير
            Dim reportForm As New UserActivityLogReportForm()
            reportForm.MdiParent = Me.MdiParent
            reportForm.WindowState = FormWindowState.Maximized
            reportForm.Show()

        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح تقرير سجلات الحركة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FilterActivityLogs()
    End Sub
End Class