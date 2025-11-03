Imports System.Data.SqlClient

Public Class ChangePasswordForm
    Private Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        Try
            ' التحقق من تطابق كلمتي المرور
            If txtNewPassword.Text <> txtConfirmPassword.Text Then
                MessageBox.Show("كلمتا المرور الجديدتان غير متطابقتين")
                Return
            End If

            ' التحقق من كلمة المرور الحالية
            If Not VerifyCurrentPassword() Then
                MessageBox.Show("كلمة المرور الحالية غير صحيحة")
                Return
            End If

            ' تغيير كلمة المرور
            ChangePassword(txtNewPassword.Text)
            MessageBox.Show("تم تغيير كلمة المرور بنجاح")
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("فشل في تغيير كلمة المرور: " & ex.Message)
        End Try
    End Sub

    Private Function VerifyCurrentPassword() As Boolean
        Dim query As String = "SELECT COUNT(*) FROM TbUsers 
                              WHERE ID = @UserID AND Password = @Password"

        Dim params() As SqlParameter = {
            New SqlParameter("@UserID", GlobalData.LoggedInUserID),
            New SqlParameter("@Password", txtCurrentPassword.Text)
        }

        Dim result As Integer = CInt(DatabaseHelper.ExecuteScalar(query, params, False))
        Return result > 0
    End Function

    Private Sub ChangePassword(newPassword As String)
        Dim query As String = "UPDATE TbUsers SET Password = @Password 
                              WHERE ID = @UserID"

        Dim params() As SqlParameter = {
            New SqlParameter("@Password", newPassword),
            New SqlParameter("@UserID", GlobalData.LoggedInUserID)
        }

        DatabaseHelper.ExecuteNonQuery(query, params, False)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class