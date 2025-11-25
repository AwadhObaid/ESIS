Imports System.Net.Mail
Imports System.Net
Imports System.IO

Public Class EMailForm
    ''' <summary>
    ''' هذا المتغير خاص بتضمين المسار الكامل للمرفق داخل البريد الالكتروني
    ''' </summary>
    Public AttachPath As String = Nothing

    Private Sub ClearMessageControls()


        Clear_Controls_Inside_Panel(Me.Panel1)
        Clear_Controls_Inside_Panel(Me.Panel3)
        Clear_Controls_Inside_Panel(Me.Panel5)
        Me.AtatchName.Tag = AttachPath
        Me.AtatchName.Text = Path.GetFileName(AttachPath)
        Me.btnSend.Enabled = True
        GetDefaultEmailInfo()


    End Sub

    Private Sub GetDefaultEmailInfo()
        Dim DT As New DataTable
        Dim Sqlst = "SELECT DefualtSenderEmail,EmailAppPassword,SMTPServer,PortNumber FROM ArchiveVariables"
        FillDataTable(DT, Sqlst)
        If DT.Rows.Count > 0 Then
            Me.SenderAddress.Text = DT.Rows(0).Item("DefualtSenderEmail").ToString
            Me.SenderAccPassword.Text = DT.Rows(0).Item("EmailAppPassword").ToString
            Me.CombPorts.Text = DT.Rows(0).Item("PortNumber").ToString
            Me.CombSMTPServers.Text = DT.Rows(0).Item("SMTPServer").ToString
            Me.ReceiverAddress.Focus()
        Else
            Me.CombSMTPServers.SelectedIndex = 0
            Me.SenderAddress.Focus()
        End If
        DT.Dispose()
    End Sub

    Public Sub SendEmailMessage()
        Try

            If MyTextNull(Me.SenderAddress, "المرسل") Then Return
            If MyTextNull(Me.SenderAccPassword, "كلمة المرور") Then Return
            If MyTextNull(Me.ReceiverAddress, "المستقبل") Then Return
            If MyCombIndexNull(Me.CombSMTPServers, "السيرفر") Then Return
            If MyCombIndexNull(Me.CombPorts, "البورت") Then Return
            If MyTextNull(Me.txtSubject, "عنوان الرساله") Then Return
            If MyTextNull(Me.txtMessageBody, "نص الرساله") Then Return


            Dim smtp = New SmtpClient(Me.CombSMTPServers.Text, Me.CombPorts.Text.Trim)
            smtp.EnableSsl = True
            smtp.UseDefaultCredentials = False
            smtp.Credentials = New NetworkCredential(Me.SenderAddress.Text, Me.SenderAccPassword.Text)
            Dim MailMessage = New MailMessage(Me.SenderAddress.Text, Me.ReceiverAddress.Text, Me.txtSubject.Text, Me.txtMessageBody.Text)
            MailMessage.Priority = MailPriority.High
            If Ck1.Checked And Me.txtCopyTo.Text.Trim <> String.Empty Then MailMessage.CC.Add(Me.txtCopyTo.Text)
            If Ck2.Checked And Me.txtHidenCopyTo.Text.Trim <> String.Empty Then MailMessage.Bcc.Add(Me.txtHidenCopyTo.Text)

            If String.IsNullOrEmpty(Me.AtatchName.Tag) = False Then
                Dim attach = New Attachment(Me.AtatchName.Tag) 'إرفاق ملف
                MailMessage.Attachments.Add(attach)
            End If
            MailMessage.IsBodyHtml = True
            smtp.Send(MailMessage)

            MessageBox.Show("تم إرسال الرساله بنجاح", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبيه", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        ClearMessageControls()
        'GetUserPermissionsInsideForm(Me.btnSend, 10901)
    End Sub

    Private Sub CombSMTPServers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CombSMTPServers.SelectedIndexChanged
        If CombSMTPServers.SelectedIndex = 0 Then
            CombPorts.SelectedIndex = 0
        Else
            CombPorts.SelectedIndex = 1
        End If
    End Sub


    Private Sub Ck1_CheckedChanged(sender As Object, e As EventArgs) Handles Ck1.CheckedChanged
        If Ck1.Checked Then
            Me.txtCopyTo.Enabled = True
            Me.txtCopyTo.Focus()
            Me.txtCopyTo.BackColor = Color.FromName("White")
        Else
            Me.txtCopyTo.Enabled = False
            Me.txtCopyTo.BackColor = Color.FromName("Control")
        End If
    End Sub

    Private Sub Ck2_CheckedChanged(sender As Object, e As EventArgs) Handles Ck2.CheckedChanged
        If Ck2.Checked Then
            Me.txtHidenCopyTo.Enabled = True
            Me.txtHidenCopyTo.Focus()
            Me.txtHidenCopyTo.BackColor = Color.FromName("White")
        Else
            Me.txtHidenCopyTo.Enabled = False
            Me.txtHidenCopyTo.BackColor = Color.FromName("Control")
        End If
    End Sub

    Private Sub SenderAdderss_Enter(sender As Object, e As EventArgs) Handles SenderAddress.Enter
        ChangeToEnglish()
    End Sub

    Private Sub SenderAccPassword_Enter(sender As Object, e As EventArgs) Handles SenderAccPassword.Enter
        ChangeToEnglish()
    End Sub

    Private Sub CkshowHide_CheckedChanged(sender As Object, e As EventArgs) Handles CkshowHide.CheckedChanged
        If CkshowHide.Checked = True Then
            Me.SenderAccPassword.UseSystemPasswordChar = False
            Me.CkshowHide.Text = "إخفاء"
        Else
            Me.SenderAccPassword.UseSystemPasswordChar = True
            Me.CkshowHide.Text = "إظهار"
        End If
    End Sub

    Private Sub btnAddAttaches_Click(sender As Object, e As EventArgs) Handles btnAddAttaches.Click
        Dim OFD = New OpenFileDialog
        If OFD.ShowDialog() = DialogResult.OK Then
            OFD.InitialDirectory = My.Settings.ARCHIVE_DEFAULT_PATH
            AtatchName.Tag = OFD.FileName
            AtatchName.Text = Path.GetFileName(OFD.FileName)
        End If
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        SendEmailMessage()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub SenderAddress_MouseClick(sender As Object, e As MouseEventArgs) Handles SenderAddress.MouseClick
        ChangeToEnglish()
    End Sub

    Private Sub SenderAddress_KeyPress(sender As Object, e As KeyPressEventArgs) Handles SenderAddress.KeyPress
        ChangeToEnglish()
    End Sub

    Private Sub SenderAddress_TextChanged(sender As Object, e As EventArgs) Handles SenderAddress.TextChanged
        ChangeToEnglish()
    End Sub
End Class