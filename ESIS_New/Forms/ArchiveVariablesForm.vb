Imports ALquhaliLibrary.TPro

Public Class ArchiveVariablesForm


    Private Sub btnFolderBrows1_Click(sender As Object, e As EventArgs) Handles btnFolderBrows1.Click
        Dim FBD As New FolderBrowserDialog
        FBD.RootFolder = Environment.SpecialFolder.MyComputer
        If FBD.ShowDialog = DialogResult.OK Then
            Me.ExternalSoredPath.Text = FBD.SelectedPath
        End If
    End Sub

    Private Sub btnFolderBrows2_Click(sender As Object, e As EventArgs) Handles btnFolderBrows2.Click
        Dim FBD As New FolderBrowserDialog
        FBD.RootFolder = Environment.SpecialFolder.MyComputer
        If FBD.ShowDialog = DialogResult.OK Then
            Me.ScandImagePath.Text = FBD.SelectedPath
        End If
    End Sub

    Private Sub btnFolderBrows3_Click(sender As Object, e As EventArgs) Handles btnFolderBrows3.Click
        Dim FBD As New FolderBrowserDialog
        FBD.RootFolder = Environment.SpecialFolder.MyComputer
        If FBD.ShowDialog = DialogResult.OK Then
            Me.PDFStoredPath.Text = FBD.SelectedPath
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ClearConfirmMsgTools(Me.lblConfirmMsg, Me.PicMsg, Timer1)
    End Sub

    Private Sub DefualtSenderEmail_Enter(sender As Object, e As EventArgs) Handles DefualtSenderEmail.Enter
        ChangeToEnglish()
    End Sub

    Private Sub CkshowHide_CheckedChanged(sender As Object, e As EventArgs) Handles CkshowHide.CheckedChanged
        If CkshowHide.Checked = True Then
            Me.EmailAppPassword.UseSystemPasswordChar = False
            Me.CkshowHide.Text = "إخفاء"
        Else
            Me.EmailAppPassword.UseSystemPasswordChar = True
            Me.CkshowHide.Text = "إظهار"
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        ClearContolOfFrmArchiveVariables(Me)
        'GetUserPermissionsInsideForm(Me.btnSave, 10101, Me.btnUpdate, 10102, Me.btnDelete, 10103)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveArchiveVariables(Me)
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        UpdateArchiveVariables(Me)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        MessageBox.Show("معذرة ‘ لايمكن حذف متغيرات الإرشيف يمكن تعديلها فقط", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub DefualtSenderEmail_Leave(sender As Object, e As EventArgs) Handles DefualtSenderEmail.Leave
        EmailAddressValidation(DefualtSenderEmail)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Me.txtPassCon.UseSystemPasswordChar = False
            Me.CheckBox1.Text = "إخفاء"
        Else
            Me.txtPassCon.UseSystemPasswordChar = True
            Me.CheckBox1.Text = "إظهار"
        End If
    End Sub


End Class