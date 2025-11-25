Imports System.IO

Public Class ImgSettingsForm

    Private Sub FrmImgSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CombExtension.SelectedIndex = My.Settings.ImageFormat
        Me.defualtName.Text = My.Settings.DefualtImgName
        If My.Settings.ImgFolderPath = String.Empty Then Me.ScandImageFolder.Text = Path.GetTempPath() Else Me.ScandImageFolder.Text = My.Settings.ImgFolderPath
    End Sub

    Private Sub btnSelectFolder_Click(sender As Object, e As EventArgs) Handles btnSelectFolder.Click
        Dim FBD As New FolderBrowserDialog
        If FBD.ShowDialog = DialogResult.OK Then
            ScandImageFolder.Text = FBD.SelectedPath
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        My.Settings.ImageFormat = Me.CombExtension.SelectedIndex
        My.Settings.DefualtImgName = Me.defualtName.Text.Trim
        My.Settings.ImgFolderPath = Me.ScandImageFolder.Text
        My.Settings.Save()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class