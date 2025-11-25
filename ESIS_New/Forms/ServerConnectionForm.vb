
Public Class ServerConnectionForm
    Dim Cls As New ServerConnectionManager

    Private Sub CombAuth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CombAuth.SelectedIndexChanged
        If CombAuth.SelectedIndex = 1 Then
            Me.UserId.Enabled = True
            Me.UserPassword.Enabled = True
            UserId.Focus()
        Else
            Me.UserId.Enabled = False
            Me.UserPassword.Enabled = False
        End If
    End Sub

    Private Sub rbLocal_CheckedChanged(sender As Object, e As EventArgs) Handles rbLocal.CheckedChanged
        If rbLocal.Checked = True Then
            Me.GroupBox1.Enabled = True
            Me.GroupBox4.Enabled = False
            Me.GroupBox2.Enabled = False
        End If
    End Sub

    Private Sub rbNetwork_CheckedChanged(sender As Object, e As EventArgs) Handles rbNetwork.CheckedChanged
        If rbNetwork.Checked = True Then
            Me.GroupBox2.Enabled = True
            Me.GroupBox4.Enabled = False
            Me.GroupBox1.Enabled = False
        End If
    End Sub

    Private Sub rbWAN_CheckedChanged(sender As Object, e As EventArgs) Handles rbWAN.CheckedChanged
        If rbWAN.Checked = True Then
            Me.GroupBox4.Enabled = True
            Me.GroupBox1.Enabled = False
            Me.GroupBox2.Enabled = False
        End If
    End Sub

    Private Sub CKShow1_CheckedChanged(sender As Object, e As EventArgs) Handles CKShow1.CheckedChanged
        If CKShow1.Checked = True Then
            Me.UserPassword.UseSystemPasswordChar = False
        Else
            Me.UserPassword.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub CKShow2_CheckedChanged(sender As Object, e As EventArgs) Handles CKShow2.CheckedChanged
        If CKShow2.Checked = True Then
            Me.txtPassword.UseSystemPasswordChar = False
        Else
            Me.txtPassword.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub FrmServerConnection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not BackgroundWorker1.IsBusy Then
            BackgroundWorker1.RunWorkerAsync()
        End If
    End Sub

    Private Sub CombServers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CombServers.SelectedIndexChanged
        Try
            ' التحقق من وجود عنصر محدد في CombServers
            If CombServers.SelectedItem IsNot Nothing Then
                ' استدعاء الإجراء FillCombByDataBase مع تمرير اسم السيرفر المحدد و ComboBox الخاص بقواعد البيانات
                Cls.FillCombByDataBases(CombServers.SelectedItem.ToString(), ComboDBs)
            End If
        Catch ex As Exception
            ' عرض رسالة خطأ في حالة حدوث استثناء
            MessageBox.Show(String.Format("حدث خطأ أثناء تحميل قواعد البيانات: {0}", ex.Message), "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        Cls.TestSQLServerConnection(Me)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Cls.Save_Conection_Settings(Me)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Cls.ResetSavedSettingsData(Me)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            If My.Settings.LoginMothed <> 3 Then
                Cls.LoadInstalledServers(Me.CombServers)
                Dim ServerName As String = If(My.Settings.LoginMothed = 2, My.Settings.ServerIP, My.Settings.ServerName)
                Cls.FillCombByDataBases(ServerName, Me.ComboDBs)
            End If
            Cls.ShowSavedSettings(Me)
        Catch ex As Exception
            ' عرض رسالة خطأ في حالة حدوث استثناء
            MessageBox.Show(String.Format("حدث خطأ أثناء تحميل قواعد البيانات: {0}", ex.Message), "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub FrmServerConnection_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If BackgroundWorker1.IsBusy Then
            BackgroundWorker1.CancelAsync()
        End If
    End Sub

    Private Sub BtnAccessPassword_Click(sender As Object, e As EventArgs) Handles BtnAccessPassword.Click
        ChangeAccessFrmServerConnectionPassword()
    End Sub
End Class