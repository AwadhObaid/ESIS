Public Class NewSubjForm
    Private Cls As New SaderClass

    Private Sub txtAddNew_TextChanged(sender As Object, e As EventArgs) Handles txtAddNew.TextChanged
        Try
            Dim y = TryCast(Application.OpenForms("SaderForm"), SaderForm)
            y.ComboSubjBook.Text = txtAddNew.Text
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        Try
            ''التحقق من الصلاحية
            'If Not PermissionHelper.CanAdd() Then
            '    PermissionHelper.ShowNoPermissionMessage("الإضافة")
            '    Return
            'End If

            If String.IsNullOrEmpty(txtAddNew.Text.Trim()) Then
                MessageBox.Show("يرجى إدخال اسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAddNew.Focus()
            End If

            Dim subjBook As String = txtAddNew.Text.Trim()

            If Cls.AddSubjBook(subjBook) Then
                MessageBox.Show("تم إضافة موضوع جديد بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.Close()
            Else
                MessageBox.Show("فشل في إضافة موضوع جديد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
End Class