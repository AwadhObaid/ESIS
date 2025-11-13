Public Class SettingsForm
    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtArchiveDefaultPath.Text = My.Settings.ARCHIVE_DEFAULT_PATH
        txtSearchDefaultPath.Text = My.Settings.DefaultPathSearch
        txtlastSavedNumber.Text = My.Settings.LastSaderNumber
        txtdaysToKeep.Text = My.Settings.daysToKeep
        nudNotificationInterval.Value = My.Settings.NotificationsFormInterval

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        My.Settings.ARCHIVE_DEFAULT_PATH = txtArchiveDefaultPath.Text.Trim()
        My.Settings.DefaultPathSearch = txtSearchDefaultPath.Text.Trim()
        My.Settings.LastSaderNumber = txtlastSavedNumber.Text.Trim()
        My.Settings.daysToKeep = txtdaysToKeep.Text.Trim()
        My.Settings.NotificationsFormInterval = CInt(nudNotificationInterval.Value)
        My.Settings.Save()
        MessageBox.Show("تم حفظ المسار بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Me.Close()
    End Sub

    Private Sub btnBrowseArchiveDefaultPath_Click(sender As Object, e As EventArgs) Handles btnBrowseArchiveDefaultPath.Click
        Using folderDlg As New FolderBrowserDialog()
            folderDlg.SelectedPath = txtArchiveDefaultPath.Text
            If folderDlg.ShowDialog() = DialogResult.OK Then
                txtArchiveDefaultPath.Text = folderDlg.SelectedPath
            End If
        End Using
    End Sub

    Private Sub btnBrowseSearchDefaultPath_Click(sender As Object, e As EventArgs) Handles btnBrowseSearchDefaultPath.Click
        Using folderDlg As New FolderBrowserDialog()
            folderDlg.SelectedPath = txtSearchDefaultPath.Text
            If folderDlg.ShowDialog() = DialogResult.OK Then
                txtSearchDefaultPath.Text = folderDlg.SelectedPath
            End If
        End Using
    End Sub
End Class