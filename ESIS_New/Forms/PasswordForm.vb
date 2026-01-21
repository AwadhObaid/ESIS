Imports ALquhaliLibrary.DBCLS
Public Class PasswordForm
    Public InputText As String = Nothing

    Private Sub ckShow_CheckedChanged(sender As Object, e As EventArgs) Handles ckShow.CheckedChanged
        If ckShow.Checked = True Then
            txtPassword.UseSystemPasswordChar = False
        Else
            txtPassword.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click

        If ValidatePassword("كلمة المرور الذي ادخلتها غير صحيحة") = False Then Return
            InputText = Decrypt(txtPassword.Text)
            Me.Close()


    End Sub

    Private Sub FrmPassword_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtPassword.Focus()
        txtPassword.Select()
    End Sub
End Class