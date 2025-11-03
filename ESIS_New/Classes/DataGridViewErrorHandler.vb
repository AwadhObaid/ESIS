''' <summary>
''' كلاس مساعد لمعالجة أخطاء DataGridView وتحسين الأداء
''' </summary>
Public Class DataGridViewErrorHandler

    ''' <summary>
    ''' إعداد معالج أخطاء DataGridView
    ''' </summary>
    ''' <param name="dgv">DataGridView المراد إعداده</param>
    Public Shared Sub SetupErrorHandling(dgv As DataGridView)
        Try
            If dgv Is Nothing Then Return

            ' إضافة معالج حدث DataError
            AddHandler dgv.DataError, AddressOf HandleDataError

            ' إعدادات إضافية لتحسين الأداء
            dgv.AllowUserToAddRows = False
            dgv.AllowUserToDeleteRows = False
            dgv.VirtualMode = False
            dgv.AutoGenerateColumns = True

            ' تحسين العرض
            dgv.RowHeadersVisible = False
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgv.MultiSelect = False
            dgv.ReadOnly = True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إعداد معالج أخطاء DataGridView: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالج حدث DataError
    ''' </summary>
    ''' <param name="sender">DataGridView</param>
    ''' <param name="e">معلومات الخطأ</param>
    Private Shared Sub HandleDataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Try
            Dim dgv As DataGridView = CType(sender, DataGridView)

            ' تسجيل تفاصيل الخطأ للتشخيص
            System.Diagnostics.Debug.WriteLine($"DataGridView Error:")
            System.Diagnostics.Debug.WriteLine($"  - Row Index: {e.RowIndex}")
            System.Diagnostics.Debug.WriteLine($"  - Column Index: {e.ColumnIndex}")
            System.Diagnostics.Debug.WriteLine($"  - Context: {e.Context}")
            System.Diagnostics.Debug.WriteLine($"  - Exception: {e.Exception?.Message}")

            ' معالجة أنواع مختلفة من الأخطاء
            Select Case e.Context
                Case DataGridViewDataErrorContexts.Commit
                    HandleCommitError(dgv, e)

                Case DataGridViewDataErrorContexts.CurrentCellChange
                    HandleCurrentCellChangeError(dgv, e)

                Case DataGridViewDataErrorContexts.Parsing
                    HandleParsingError(dgv, e)

                Case DataGridViewDataErrorContexts.LeaveControl
                    HandleLeaveControlError(dgv, e)

                Case DataGridViewDataErrorContexts.Display
                    HandleDisplayError(dgv, e)

                Case Else
                    HandleGenericError(dgv, e)
            End Select

            ' منع عرض رسالة الخطأ الافتراضية
            e.ThrowException = False

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالج DataError: {ex.Message}")
            e.ThrowException = False
        End Try
    End Sub

    ''' <summary>
    ''' معالجة خطأ Commit
    ''' </summary>
    Private Shared Sub HandleCommitError(dgv As DataGridView, e As DataGridViewDataErrorEventArgs)
        Try
            ' محاولة إصلاح البيانات أو تجاهل التغيير
            If e.RowIndex >= 0 AndAlso e.RowIndex < dgv.Rows.Count Then
                If e.ColumnIndex >= 0 AndAlso e.ColumnIndex < dgv.Columns.Count Then
                    ' إعادة تعيين القيمة إلى القيمة الأصلية
                    dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Nothing
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في HandleCommitError: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة خطأ تغيير الخلية الحالية
    ''' </summary>
    Private Shared Sub HandleCurrentCellChangeError(dgv As DataGridView, e As DataGridViewDataErrorEventArgs)
        Try
            ' محاولة تحديد خلية صالحة
            If dgv.Rows.Count > 0 AndAlso dgv.Columns.Count > 0 Then
                ' الانتقال إلى الخلية الأولى الصالحة
                dgv.CurrentCell = dgv.Rows(0).Cells(0)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في HandleCurrentCellChangeError: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة خطأ التحليل (Parsing)
    ''' </summary>
    Private Shared Sub HandleParsingError(dgv As DataGridView, e As DataGridViewDataErrorEventArgs)
        Try
            ' تعيين قيمة افتراضية للخلية
            If e.RowIndex >= 0 AndAlso e.RowIndex < dgv.Rows.Count Then
                If e.ColumnIndex >= 0 AndAlso e.ColumnIndex < dgv.Columns.Count Then
                    dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = ""
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في HandleParsingError: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة خطأ مغادرة التحكم
    ''' </summary>
    Private Shared Sub HandleLeaveControlError(dgv As DataGridView, e As DataGridViewDataErrorEventArgs)
        Try
            ' تجاهل الخطأ والسماح بمغادرة التحكم
            ' لا حاجة لإجراء خاص
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في HandleLeaveControlError: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة خطأ العرض
    ''' </summary>
    Private Shared Sub HandleDisplayError(dgv As DataGridView, e As DataGridViewDataErrorEventArgs)
        Try
            ' تعيين قيمة افتراضية للعرض
            If e.RowIndex >= 0 AndAlso e.RowIndex < dgv.Rows.Count Then
                If e.ColumnIndex >= 0 AndAlso e.ColumnIndex < dgv.Columns.Count Then
                    dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = "[خطأ في البيانات]"
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في HandleDisplayError: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الأخطاء العامة
    ''' </summary>
    Private Shared Sub HandleGenericError(dgv As DataGridView, e As DataGridViewDataErrorEventArgs)
        Try
            ' تسجيل الخطأ وتجاهله
            System.Diagnostics.Debug.WriteLine($"خطأ عام في DataGridView: {e.Exception?.Message}")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في HandleGenericError: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من صحة البيانات قبل ربطها بـ DataGridView
    ''' </summary>
    ''' <param name="dataTable">جدول البيانات</param>
    ''' <returns>جدول البيانات المنظف أو جدول فارغ</returns>
    Public Shared Function ValidateAndCleanDataTable(dataTable As DataTable) As DataTable
        Try
            If dataTable Is Nothing Then
                Return CreateEmptyDataTable()
            End If

            ' إنشاء جدول جديد منظف
            Dim cleanTable As DataTable = dataTable.Clone()

            ' نسخ الصفوف مع التحقق من صحتها
            For Each row As DataRow In dataTable.Rows
                Try
                    ' التحقق من أن الصف ليس محذوفاً
                    If row.RowState <> DataRowState.Deleted Then
                        Dim newRow As DataRow = cleanTable.NewRow()

                        ' نسخ القيم مع معالجة القيم الفارغة
                        For i As Integer = 0 To Math.Min(row.ItemArray.Length - 1, cleanTable.Columns.Count - 1)
                            Try
                                If row.ItemArray(i) Is Nothing OrElse IsDBNull(row.ItemArray(i)) Then
                                    newRow(i) = ""
                                Else
                                    newRow(i) = row.ItemArray(i)
                                End If
                            Catch ex As Exception
                                newRow(i) = ""
                                System.Diagnostics.Debug.WriteLine($"خطأ في نسخ القيمة في العمود {i}: {ex.Message}")
                            End Try
                        Next

                        cleanTable.Rows.Add(newRow)
                    End If
                Catch ex As Exception
                    System.Diagnostics.Debug.WriteLine($"خطأ في معالجة الصف: {ex.Message}")
                    Continue For
                End Try
            Next

            Return cleanTable

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ValidateAndCleanDataTable: {ex.Message}")
            Return CreateEmptyDataTable()
        End Try
    End Function

    ''' <summary>
    ''' إنشاء جدول بيانات فارغ للحالات الطارئة
    ''' </summary>
    ''' <returns>جدول بيانات فارغ</returns>
    Private Shared Function CreateEmptyDataTable() As DataTable
        Try
            Dim emptyTable As New DataTable()
            emptyTable.Columns.Add("ID", GetType(Integer))
            emptyTable.Columns.Add("NumberBook", GetType(String))
            emptyTable.Columns.Add("DateBook", GetType(DateTime))
            emptyTable.Columns.Add("SubjBook", GetType(String))
            emptyTable.Columns.Add("WillMaster", GetType(String))
            emptyTable.Columns.Add("WillSub", GetType(String))
            emptyTable.Columns.Add("UserName", GetType(String))
            emptyTable.Columns.Add("ArchiveID", GetType(Integer))

            Return emptyTable
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء جدول فارغ: {ex.Message}")
            Return New DataTable()
        End Try
    End Function

    ''' <summary>
    ''' ربط البيانات بـ DataGridView بشكل آمن
    ''' </summary>
    ''' <param name="dgv">DataGridView</param>
    ''' <param name="dataTable">جدول البيانات</param>
    Public Shared Sub SafeBindData(dgv As DataGridView, dataTable As DataTable)
        Try
            If dgv Is Nothing Then Return

            ' تعطيل الأحداث مؤقتاً
            dgv.SuspendLayout()

            ' تنظيف البيانات
            Dim cleanData As DataTable = ValidateAndCleanDataTable(dataTable)

            ' ربط البيانات
            dgv.DataSource = cleanData

            ' إعادة تفعيل الأحداث
            dgv.ResumeLayout()

            ' تحديث العرض
            dgv.Refresh()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SafeBindData: {ex.Message}")

            ' في حالة الفشل، ربط جدول فارغ
            Try
                dgv.DataSource = CreateEmptyDataTable()
            Catch innerEx As Exception
                System.Diagnostics.Debug.WriteLine($"خطأ في ربط الجدول الفارغ: {innerEx.Message}")
            End Try
        End Try
    End Sub

End Class