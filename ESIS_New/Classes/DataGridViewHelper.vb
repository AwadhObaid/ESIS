''' <summary>
''' كلاس مساعد للتعامل مع DataGridView وإدارة التحديد والتحقق من الصفوف
''' </summary>
Public Class DataGridViewHelper

    ''' <summary>
    ''' التحقق من وجود صف محدد صالح في DataGridView
    ''' </summary>
    ''' <param name="dgv">DataGridView المراد فحصه</param>
    ''' <param name="requiredColumns">أسماء الأعمدة المطلوبة (اختياري)</param>
    ''' <returns>True إذا كان هناك صف محدد صالح</returns>
    Public Shared Function IsValidRowSelected(dgv As DataGridView, Optional requiredColumns As String() = Nothing) As Boolean
        Try
            ' التحقق من أن DataGridView ليس Nothing
            If dgv Is Nothing Then
                Return False
            End If

            ' التحقق من وجود صفوف في DataGridView
            If dgv.Rows.Count = 0 Then
                Return False
            End If

            ' التحقق من وجود صفوف محددة
            If dgv.SelectedRows.Count = 0 Then
                Return False
            End If

            ' التحقق من صحة الصف المحدد
            Dim selectedRow As DataGridViewRow = dgv.SelectedRows(0)
            If selectedRow Is Nothing Then
                Return False
            End If

            ' التحقق من أن الصف ليس صف جديد
            If selectedRow.IsNewRow Then
                Return False
            End If

            ' التحقق من وجود الأعمدة المطلوبة إذا تم تحديدها
            If requiredColumns IsNot Nothing Then
                For Each columnName As String In requiredColumns
                    If Not dgv.Columns.Contains(columnName) Then
                        Return False
                    End If

                    ' التحقق من وجود قيمة في العمود
                    If selectedRow.Cells(columnName) Is Nothing OrElse
                       selectedRow.Cells(columnName).Value Is Nothing OrElse
                       IsDBNull(selectedRow.Cells(columnName).Value) Then
                        Return False
                    End If
                Next
            End If

            Return True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في IsValidRowSelected: {ex.Message}")
            Return False
        End Try
    End Function


    ''' <summary>
    ''' الحصول على قيمة من الصف المحدد مع التحقق من الأمان
    ''' </summary>
    ''' <param name="dgv">DataGridView</param>
    ''' <param name="columnName">اسم العمود</param>
    ''' <param name="defaultValue">القيمة الافتراضية في حالة عدم وجود قيمة</param>
    ''' <returns>قيمة العمود أو القيمة الافتراضية</returns>
    Public Shared Function GetSelectedRowValue(Of T)(dgv As DataGridView, columnName As String, defaultValue As T) As T
        Try
            If Not IsValidRowSelected(dgv, {columnName}) Then
                Return defaultValue
            End If

            Dim selectedRow As DataGridViewRow = dgv.SelectedRows(0)
            Dim cellValue As Object = selectedRow.Cells(columnName).Value

            If cellValue Is Nothing OrElse IsDBNull(cellValue) Then
                Return defaultValue
            End If

            ' محاولة تحويل القيمة إلى النوع المطلوب
            If TypeOf cellValue Is T Then
                Return CType(cellValue, T)
            Else
                Return CType(Convert.ChangeType(cellValue, GetType(T)), T)
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetSelectedRowValue: {ex.Message}")
            Return defaultValue
        End Try
    End Function

    ''' <summary>
    ''' الحصول على معرف الصف المحدد (ID)
    ''' </summary>
    ''' <param name="dgv">DataGridView</param>
    ''' <param name="idColumnName">اسم عمود المعرف (افتراضي: "ID")</param>
    ''' <returns>معرف الصف أو -1 في حالة عدم وجود تحديد صالح</returns>
    Public Shared Function GetSelectedRowId(dgv As DataGridView, Optional idColumnName As String = "ID") As Integer
        Try
            Dim id As Integer = GetSelectedRowValue(dgv, idColumnName, -1)

            ' التحقق من أن المعرف صالح
            If id <= 0 Then
                Return -1
            End If

            Return id

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetSelectedRowId: {ex.Message}")
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' تحديد صف معين في DataGridView
    ''' </summary>
    ''' <param name="dgv">DataGridView</param>
    ''' <param name="rowIndex">فهرس الصف</param>
    ''' <param name="clearOtherSelections">مسح التحديدات الأخرى (افتراضي: True)</param>
    ''' <returns>True إذا تم التحديد بنجاح</returns>
    Public Shared Function SelectRow(dgv As DataGridView, rowIndex As Integer, Optional clearOtherSelections As Boolean = True) As Boolean
        Try
            If dgv Is Nothing OrElse rowIndex < 0 OrElse rowIndex >= dgv.Rows.Count Then
                Return False
            End If

            If clearOtherSelections Then
                dgv.ClearSelection()
            End If

            dgv.Rows(rowIndex).Selected = True
            dgv.CurrentCell = dgv.Rows(rowIndex).Cells(0)

            Return True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SelectRow: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' البحث عن صف بناءً على قيمة في عمود معين
    ''' </summary>
    ''' <param name="dgv">DataGridView</param>
    ''' <param name="columnName">اسم العمود</param>
    ''' <param name="searchValue">القيمة المراد البحث عنها</param>
    ''' <param name="selectIfFound">تحديد الصف إذا تم العثور عليه (افتراضي: True)</param>
    ''' <returns>فهرس الصف إذا تم العثور عليه، -1 إذا لم يتم العثور عليه</returns>
    Public Shared Function FindRowByValue(dgv As DataGridView, columnName As String, searchValue As Object, Optional selectIfFound As Boolean = True) As Integer
        Try
            If dgv Is Nothing OrElse String.IsNullOrEmpty(columnName) OrElse searchValue Is Nothing Then
                Return -1
            End If

            If Not dgv.Columns.Contains(columnName) Then
                Return -1
            End If

            For i As Integer = 0 To dgv.Rows.Count - 1
                Dim row As DataGridViewRow = dgv.Rows(i)
                If row.IsNewRow Then Continue For

                Dim cellValue As Object = row.Cells(columnName).Value
                If cellValue IsNot Nothing AndAlso Not IsDBNull(cellValue) Then
                    If cellValue.ToString().Equals(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) Then
                        If selectIfFound Then
                            SelectRow(dgv, i)
                        End If
                        Return i
                    End If
                End If
            Next

            Return -1

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في FindRowByValue: {ex.Message}")
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' تطبيق تنسيق على الصفوف بناءً على شرط معين
    ''' </summary>
    ''' <param name="dgv">DataGridView</param>
    ''' <param name="columnName">اسم العمود للتحقق من الشرط</param>
    ''' <param name="condition">دالة الشرط</param>
    ''' <param name="backColor">لون الخلفية</param>
    ''' <param name="foreColor">لون النص</param>
    Public Shared Sub ApplyConditionalFormatting(dgv As DataGridView, columnName As String, condition As Func(Of Object, Boolean), backColor As Color, foreColor As Color)
        Try
            If dgv Is Nothing OrElse String.IsNullOrEmpty(columnName) OrElse condition Is Nothing Then
                Return
            End If

            If Not dgv.Columns.Contains(columnName) Then
                Return
            End If

            For Each row As DataGridViewRow In dgv.Rows
                If row.IsNewRow Then Continue For

                Dim cellValue As Object = row.Cells(columnName).Value
                If cellValue IsNot Nothing AndAlso condition(cellValue) Then
                    row.DefaultCellStyle.BackColor = backColor
                    row.DefaultCellStyle.ForeColor = foreColor
                End If
            Next

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ApplyConditionalFormatting: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' إعداد DataGridView بالخصائص الأساسية للتطبيقات العربية
    ''' </summary>
    ''' <param name="dgv">DataGridView</param>
    Public Shared Sub SetupArabicDataGridView(dgv As DataGridView)
        Try
            If dgv Is Nothing Then Return

            ' إعدادات اللغة العربية
            dgv.RightToLeft = RightToLeft.Yes
            'dgv.RightToLeftLayout = True

            ' إعدادات التحديد
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgv.MultiSelect = False

            ' إعدادات المظهر
            dgv.AllowUserToAddRows = False
            dgv.AllowUserToDeleteRows = False
            dgv.ReadOnly = True
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            ' إعدادات الألوان
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
            dgv.DefaultCellStyle.SelectionBackColor = Color.DarkBlue
            dgv.DefaultCellStyle.SelectionForeColor = Color.White

            ' إعدادات الخط
            dgv.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Regular)
            dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SetupArabicDataGridView: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تصدير بيانات DataGridView إلى CSV
    ''' </summary>
    ''' <param name="dgv">DataGridView</param>
    ''' <param name="filePath">مسار الملف</param>
    ''' <param name="includeHeaders">تضمين رؤوس الأعمدة (افتراضي: True)</param>
    ''' <returns>True إذا تم التصدير بنجاح</returns>
    Public Shared Function ExportToCSV(dgv As DataGridView, filePath As String, Optional includeHeaders As Boolean = True) As Boolean
        Try
            If dgv Is Nothing OrElse String.IsNullOrEmpty(filePath) Then
                Return False
            End If

            Using writer As New IO.StreamWriter(filePath, False, System.Text.Encoding.UTF8)
                ' كتابة رؤوس الأعمدة
                If includeHeaders Then
                    Dim headers As New List(Of String)
                    For Each column As DataGridViewColumn In dgv.Columns
                        If column.Visible Then
                            headers.Add($"""{column.HeaderText}""")
                        End If
                    Next
                    writer.WriteLine(String.Join(",", headers))
                End If

                ' كتابة البيانات
                For Each row As DataGridViewRow In dgv.Rows
                    If row.IsNewRow Then Continue For

                    Dim values As New List(Of String)
                    For Each column As DataGridViewColumn In dgv.Columns
                        If column.Visible Then
                            Dim cellValue As Object = row.Cells(column.Index).Value
                            Dim valueStr As String = If(cellValue?.ToString(), "")
                            values.Add($"""{valueStr}""")
                        End If
                    Next
                    writer.WriteLine(String.Join(",", values))
                Next
            End Using

            Return True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ExportToCSV: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' دالة عامة لإضافة عمود زر حذف إلى DataGridView
    ''' </summary>
    ''' <param name="dataGridView">عنوان DataGridView</param>
    ''' <param name="columnName">اسم العمود (سيتم استخدامه كمعرّف)</param>
    ''' <param name="headerText">نص عنوان العمود</param>
    ''' <param name="buttonText">نص الزر</param>
    ''' <param name="permissionForm">اسم النموذج للتحقق من الصلاحيات</param>
    ''' <param name="permissionAction">نوع الصلاحية (مثل "Delete")</param>
    ''' <param name="confirmMessage">رسالة التأكيد عند النقر على الزر</param>
    ''' <param name="successMessage">رسالة النجاح بعد الحذف</param>
    ''' <param name="deleteAction">إجراء الحذف (دالة أو إجراء)</param>
    ''' <param name="width">عرض العمود (اختياري)</param>
    Public Sub AddDeleteButtonColumn(
        ByVal dataGridView As DataGridView,
        ByVal columnName As String,
        ByVal headerText As String,
        ByVal buttonText As String,
        ByVal permissionForm As String,
        ByVal permissionAction As String,
        ByVal confirmMessage As String,
        ByVal successMessage As String,
        ByVal deleteAction As Action(Of Integer),
        Optional ByVal width As Integer = 60)

        Try
            ' التحقق من أن DataGridView غير فارغ
            If dataGridView Is Nothing Then
                Throw New ArgumentException("DataGridView لا يمكن أن يكون فارغًا")
            End If

            ' التحقق من وجود صلاحيات الحذف
            Dim hasDeletePermission As Boolean = PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, permissionForm, permissionAction)

            ' إزالة العمود إذا كان موجودًا بالفعل
            If dataGridView.Columns.Contains(columnName) Then
                dataGridView.Columns.Remove(columnName)
            End If

            ' إنشاء عمود زر جديد
            Dim deleteButtonColumn As New DataGridViewButtonColumn()
            With deleteButtonColumn
                .Name = columnName
                .HeaderText = headerText
                .Text = buttonText
                .UseColumnTextForButtonValue = True
                .Width = width
                .DisplayIndex = 8 ' وضع العمود في البداية
                .Visible = hasDeletePermission ' جعل العمود مرئيًا فقط إذا كان لدى المستخدم الصلاحية
            End With

            ' إضافة العمود إلى DataGridView
            dataGridView.Columns.Add(deleteButtonColumn)

            ' إضافة معالج حدث النقر على الزر
            AddHandler dataGridView.CellClick, Sub(sender As Object, e As DataGridViewCellEventArgs)
                                                   Try
                                                       ' التحقق من أن النقر كان على زر الحذف
                                                       If e.ColumnIndex = dataGridView.Columns(columnName).Index AndAlso e.RowIndex >= 0 Then
                                                           ' التحقق من صلاحيات الحذف
                                                           If Not PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, permissionForm, permissionAction) Then
                                                               MessageBox.Show("ليس لديك صلاحية الحذف. يرجى الاتصال بمدير النظام.", "صلاحيات محدودة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                               Return
                                                           End If

                                                           ' التحقق من وجود صف محدد
                                                           If dataGridView.Rows(e.RowIndex).IsNewRow Then
                                                               MessageBox.Show("يرجى اختيار صف صالح للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                               Return
                                                           End If

                                                           ' عرض رسالة تأكيد
                                                           Dim result As DialogResult = MessageBox.Show(confirmMessage, "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                                           If result = DialogResult.No Then
                                                               Return
                                                           End If

                                                           ' الحصول على معرف السجل من الخلية المناسبة (افترض أن العمود الأول يحتوي على المعرف)
                                                           Dim id As Integer = Convert.ToInt32(dataGridView.Rows(e.RowIndex).Cells(1).Value) ' تعديل رقم الخلية حسب الحاجة

                                                           ' تنفيذ إجراء الحذف
                                                           deleteAction.Invoke(id)

                                                           ' عرض رسالة نجاح
                                                           MessageBox.Show(successMessage, "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                                                           ' تحديث DataGridView
                                                           RefreshDataGridView(dataGridView)
                                                       End If
                                                   Catch ex As Exception
                                                       MessageBox.Show($"حدث خطأ أثناء الحذف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                                   End Try
                                               End Sub

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء إضافة عمود الحذف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' دالة مساعدة لتحديث DataGridView بعد الحذف
    ''' </summary>
    Private Sub RefreshDataGridView(dataGridView As DataGridView)
        Try
            ' حفظ الصف المحدد (إذا وجد)
            Dim selectedRowIndex As Integer = -1
            If dataGridView.SelectedRows.Count > 0 Then
                selectedRowIndex = dataGridView.SelectedRows(0).Index
            End If

            ' تحديث البيانات
            If TypeOf dataGridView.DataSource Is DataTable Then
                Dim dataTable As DataTable = DirectCast(dataGridView.DataSource, DataTable)
                dataTable.AcceptChanges()
            End If

            ' إعادة تحديد الصف (إذا كان ممكنًا)
            If selectedRowIndex >= 0 AndAlso selectedRowIndex < dataGridView.Rows.Count Then
                dataGridView.Rows(selectedRowIndex).Selected = True
                dataGridView.FirstDisplayedScrollingRowIndex = selectedRowIndex
            End If
        Catch ex As Exception
            ' تجاهل الأخطاء في التحديث
        End Try
    End Sub

End Class