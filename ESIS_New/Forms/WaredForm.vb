Imports System.Data.SqlClient

Public Class WaredForm
    Private waredClass As New WaredClass()

    Private Sub WaredForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "إدارة الوارد - نظام ESIS"
        Me.RightToLeft = RightToLeft.Yes
        Me.RightToLeftLayout = True
        dtpDateBook.Value = DateTime.Now
        dtpStartDate.Value = DateTime.Now.AddDays(-30)
        dtpEndDate.Value = DateTime.Now
        LoadWared()
        txtUserName.Text = GlobalData.LoggedInUserName  ' تعبئة اسم المستخدم
        txtUserName.ReadOnly = True ' جعل حقل اسم المستخدم للقراءة فقط
    End Sub

    Private Sub LoadWared()
        Try
            Dim dt As DataTable = waredClass.GetAllWared()
            dgvWared.DataSource = dt

            ' تخصيص أسماء الأعمدة
            If dgvWared.Columns.Count > 0 Then
                dgvWared.Columns("ID").HeaderText = "المعرف"
                dgvWared.Columns("NumberBook").HeaderText = "رقم الكتاب"
                dgvWared.Columns("DateBook").HeaderText = "تاريخ الكتاب"
                dgvWared.Columns("SubjBook").HeaderText = "موضوع الكتاب"
                dgvWared.Columns("WillMaster").HeaderText = "رقم البوليصة الرئيسية"
                dgvWared.Columns("WillSub").HeaderText = "رقم البوليصة الفرعية"
                dgvWared.Columns("UserName").HeaderText = "اسم المستخدم"
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            If ValidateInput() Then
                Dim numberBook As String = txtNumberBook.Text.Trim()
                Dim dateBook As Date = dtpDateBook.Value.Date
                Dim subjBook As String = txtSubjBook.Text.Trim()
                Dim willMaster As String = txtWillMaster.Text.Trim()
                Dim willSub As String = txtWillSub.Text.Trim()
                Dim userName As String = txtUserName.Text.Trim()

                If waredClass.AddWared(numberBook, dateBook, subjBook, willMaster, willSub, userName) Then
                    MessageBox.Show("تم إضافة الوارد بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ClearFields()
                    LoadWared()
                Else
                    MessageBox.Show("فشل في إضافة الوارد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            If dgvWared.SelectedRows.Count > 0 AndAlso ValidateInput() Then
                Dim id As Integer = Convert.ToInt32(dgvWared.SelectedRows(0).Cells("ID").Value)
                Dim numberBook As String = txtNumberBook.Text.Trim()
                Dim dateBook As Date = dtpDateBook.Value.Date
                Dim subjBook As String = txtSubjBook.Text.Trim()
                Dim willMaster As String = txtWillMaster.Text.Trim()
                Dim willSub As String = txtWillSub.Text.Trim()
                Dim userName As String = txtUserName.Text.Trim()

                If waredClass.UpdateWared(id, numberBook, dateBook, subjBook, willMaster, willSub, userName) Then
                    MessageBox.Show("تم تحديث الوارد بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ClearFields()
                    LoadWared()
                Else
                    MessageBox.Show("فشل في تحديث الوارد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("يرجى اختيار وارد للتحديث", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If dgvWared.SelectedRows.Count > 0 Then
                If MessageBox.Show("هل تريد حذف هذا الوارد؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Dim id As Integer = Convert.ToInt32(dgvWared.SelectedRows(0).Cells("ID").Value)

                    If waredClass.DeleteWared(id) Then
                        MessageBox.Show("تم حذف الوارد بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ClearFields()
                        LoadWared()
                    Else
                        MessageBox.Show("فشل في حذف الوارد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            Else
                MessageBox.Show("يرجى اختيار وارد للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            Dim searchTerm As String = txtSearch.Text.Trim()
            If Not String.IsNullOrEmpty(searchTerm) Then
                Dim dt As DataTable = waredClass.SearchWared(searchTerm)
                dgvWared.DataSource = dt
            Else
                LoadWared()
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء البحث: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearchByDate_Click(sender As Object, e As EventArgs) Handles btnSearchByDate.Click
        Try
            Dim startDate As Date = dtpStartDate.Value.Date
            Dim endDate As Date = dtpEndDate.Value.Date
            Dim dt As DataTable = waredClass.SearchWaredByDateRange(startDate, endDate)
            dgvWared.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء البحث بالتاريخ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvWared_SelectionChanged(sender As Object, e As EventArgs) Handles dgvWared.SelectionChanged
        Try
            If dgvWared.SelectedRows.Count > 0 Then
                Dim row As DataGridViewRow = dgvWared.SelectedRows(0)
                txtNumberBook.Text = row.Cells("NumberBook").Value.ToString()
                dtpDateBook.Value = Convert.ToDateTime(row.Cells("DateBook").Value)
                txtSubjBook.Text = row.Cells("SubjBook").Value.ToString()
                txtWillMaster.Text = row.Cells("WillMaster").Value.ToString()
                txtWillSub.Text = row.Cells("WillSub").Value.ToString()
                txtUserName.Text = row.Cells("UserName").Value.ToString()
            End If
        Catch ex As Exception
            ' تجاهل الأخطاء في هذا الحدث
        End Try
    End Sub

    Private Function ValidateInput() As Boolean
        If String.IsNullOrEmpty(txtNumberBook.Text.Trim()) Then
            MessageBox.Show("يرجى إدخال رقم الكتاب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNumberBook.Focus()
            Return False
        End If

        If String.IsNullOrEmpty(txtSubjBook.Text.Trim()) Then
            MessageBox.Show("يرجى إدخال موضوع الكتاب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSubjBook.Focus()
            Return False
        End If

        If String.IsNullOrEmpty(txtUserName.Text.Trim()) Then
            MessageBox.Show("يرجى إدخال اسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUserName.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearFields()
        txtNumberBook.Clear()
        txtSubjBook.Clear()
        txtWillMaster.Clear()
        txtWillSub.Clear()
        txtUserName.Clear()
        txtSearch.Clear()
        dtpDateBook.Value = DateTime.Now
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class

