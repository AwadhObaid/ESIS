<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EMailForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnhelp = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CkshowHide = New System.Windows.Forms.CheckBox()
        Me.Ck2 = New System.Windows.Forms.CheckBox()
        Me.Ck1 = New System.Windows.Forms.CheckBox()
        Me.txtHidenCopyTo = New System.Windows.Forms.TextBox()
        Me.txtCopyTo = New System.Windows.Forms.TextBox()
        Me.CombPorts = New System.Windows.Forms.ComboBox()
        Me.CombSMTPServers = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SenderAccPassword = New System.Windows.Forms.TextBox()
        Me.ReceiverAddress = New System.Windows.Forms.TextBox()
        Me.SenderAddress = New System.Windows.Forms.TextBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.btnAddAttaches = New System.Windows.Forms.Button()
        Me.AtatchName = New System.Windows.Forms.TextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.txtMessageBody = New System.Windows.Forms.RichTextBox()
        Me.txtSubject = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ToolStrip1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(603, 43)
        Me.Panel2.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.SteelBlue
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnhelp, Me.ToolStripLabel1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip1.Size = New System.Drawing.Size(603, 43)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnhelp
        '
        Me.btnhelp.AutoSize = False
        Me.btnhelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnhelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnhelp.Name = "btnhelp"
        Me.btnhelp.Size = New System.Drawing.Size(35, 35)
        Me.btnhelp.Text = "إرسال ملف"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Font = New System.Drawing.Font("Sakkal Majalla", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripLabel1.ForeColor = System.Drawing.Color.White
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(500, 40)
        Me.ToolStripLabel1.Text = "إرسال بريد الكتروني"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CkshowHide)
        Me.Panel1.Controls.Add(Me.Ck2)
        Me.Panel1.Controls.Add(Me.Ck1)
        Me.Panel1.Controls.Add(Me.txtHidenCopyTo)
        Me.Panel1.Controls.Add(Me.txtCopyTo)
        Me.Panel1.Controls.Add(Me.CombPorts)
        Me.Panel1.Controls.Add(Me.CombSMTPServers)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.SenderAccPassword)
        Me.Panel1.Controls.Add(Me.ReceiverAddress)
        Me.Panel1.Controls.Add(Me.SenderAddress)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Enabled = False
        Me.Panel1.Location = New System.Drawing.Point(0, 43)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(603, 256)
        Me.Panel1.TabIndex = 2
        '
        'CkshowHide
        '
        Me.CkshowHide.AutoSize = True
        Me.CkshowHide.BackColor = System.Drawing.SystemColors.Window
        Me.CkshowHide.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkshowHide.Location = New System.Drawing.Point(71, 48)
        Me.CkshowHide.Name = "CkshowHide"
        Me.CkshowHide.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.CkshowHide.Size = New System.Drawing.Size(65, 28)
        Me.CkshowHide.TabIndex = 14
        Me.CkshowHide.Text = "إظهار"
        Me.CkshowHide.UseVisualStyleBackColor = False
        Me.CkshowHide.Visible = False
        '
        'Ck2
        '
        Me.Ck2.AutoSize = True
        Me.Ck2.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ck2.Location = New System.Drawing.Point(492, 227)
        Me.Ck2.Name = "Ck2"
        Me.Ck2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Ck2.Size = New System.Drawing.Size(113, 28)
        Me.Ck2.TabIndex = 13
        Me.Ck2.Text = "نسخه مخفيه "
        Me.Ck2.UseVisualStyleBackColor = True
        '
        'Ck1
        '
        Me.Ck1.AutoSize = True
        Me.Ck1.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ck1.Location = New System.Drawing.Point(492, 191)
        Me.Ck1.Name = "Ck1"
        Me.Ck1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Ck1.Size = New System.Drawing.Size(91, 28)
        Me.Ck1.TabIndex = 12
        Me.Ck1.Text = "نسخه إلى "
        Me.Ck1.UseVisualStyleBackColor = True
        '
        'txtHidenCopyTo
        '
        Me.txtHidenCopyTo.BackColor = System.Drawing.SystemColors.Control
        Me.txtHidenCopyTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtHidenCopyTo.Enabled = False
        Me.txtHidenCopyTo.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHidenCopyTo.Location = New System.Drawing.Point(69, 222)
        Me.txtHidenCopyTo.Multiline = True
        Me.txtHidenCopyTo.Name = "txtHidenCopyTo"
        Me.txtHidenCopyTo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtHidenCopyTo.Size = New System.Drawing.Size(417, 27)
        Me.txtHidenCopyTo.TabIndex = 10
        '
        'txtCopyTo
        '
        Me.txtCopyTo.BackColor = System.Drawing.SystemColors.Control
        Me.txtCopyTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCopyTo.Enabled = False
        Me.txtCopyTo.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCopyTo.Location = New System.Drawing.Point(69, 187)
        Me.txtCopyTo.Multiline = True
        Me.txtCopyTo.Name = "txtCopyTo"
        Me.txtCopyTo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCopyTo.Size = New System.Drawing.Size(417, 29)
        Me.txtCopyTo.TabIndex = 11
        '
        'CombPorts
        '
        Me.CombPorts.BackColor = System.Drawing.Color.AliceBlue
        Me.CombPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CombPorts.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CombPorts.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CombPorts.FormattingEnabled = True
        Me.CombPorts.Items.AddRange(New Object() {"587", "465", "25"})
        Me.CombPorts.Location = New System.Drawing.Point(69, 80)
        Me.CombPorts.Name = "CombPorts"
        Me.CombPorts.Size = New System.Drawing.Size(153, 32)
        Me.CombPorts.TabIndex = 9
        '
        'CombSMTPServers
        '
        Me.CombSMTPServers.BackColor = System.Drawing.Color.AliceBlue
        Me.CombSMTPServers.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CombSMTPServers.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CombSMTPServers.FormattingEnabled = True
        Me.CombSMTPServers.Items.AddRange(New Object() {"smtp.gmail.com", "smtp.live.com", "smtp.yahoo.com", "plus.smtp.mail.yahoo.com", "mail.gmx.com"})
        Me.CombSMTPServers.Location = New System.Drawing.Point(227, 81)
        Me.CombSMTPServers.Name = "CombSMTPServers"
        Me.CombSMTPServers.Size = New System.Drawing.Size(259, 32)
        Me.CombSMTPServers.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(489, 87)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 24)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "نوع البورت"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(489, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 24)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "كلمة المرور"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label4.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(490, 120)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 24)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "المستلم"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(489, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 24)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "المرسل"
        '
        'SenderAccPassword
        '
        Me.SenderAccPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SenderAccPassword.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SenderAccPassword.Location = New System.Drawing.Point(69, 46)
        Me.SenderAccPassword.Name = "SenderAccPassword"
        Me.SenderAccPassword.Size = New System.Drawing.Size(417, 32)
        Me.SenderAccPassword.TabIndex = 5
        Me.SenderAccPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.SenderAccPassword.UseSystemPasswordChar = True
        '
        'ReceiverAddress
        '
        Me.ReceiverAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ReceiverAddress.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ReceiverAddress.Location = New System.Drawing.Point(69, 113)
        Me.ReceiverAddress.Multiline = True
        Me.ReceiverAddress.Name = "ReceiverAddress"
        Me.ReceiverAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ReceiverAddress.Size = New System.Drawing.Size(417, 67)
        Me.ReceiverAddress.TabIndex = 6
        '
        'SenderAddress
        '
        Me.SenderAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SenderAddress.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SenderAddress.Location = New System.Drawing.Point(69, 10)
        Me.SenderAddress.Name = "SenderAddress"
        Me.SenderAddress.Size = New System.Drawing.Size(417, 32)
        Me.SenderAddress.TabIndex = 6
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.btnClose)
        Me.Panel4.Controls.Add(Me.btnSend)
        Me.Panel4.Controls.Add(Me.btnNew)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 607)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(603, 52)
        Me.Panel4.TabIndex = 4
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.btnAddAttaches)
        Me.Panel5.Controls.Add(Me.AtatchName)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel5.Enabled = False
        Me.Panel5.Location = New System.Drawing.Point(0, 569)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(603, 38)
        Me.Panel5.TabIndex = 0
        '
        'btnAddAttaches
        '
        Me.btnAddAttaches.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnAddAttaches.Location = New System.Drawing.Point(486, 4)
        Me.btnAddAttaches.Name = "btnAddAttaches"
        Me.btnAddAttaches.Size = New System.Drawing.Size(108, 29)
        Me.btnAddAttaches.TabIndex = 7
        Me.btnAddAttaches.Text = "إضافة مرفق"
        Me.btnAddAttaches.UseVisualStyleBackColor = True
        '
        'AtatchName
        '
        Me.AtatchName.BackColor = System.Drawing.SystemColors.Menu
        Me.AtatchName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.AtatchName.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold)
        Me.AtatchName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.AtatchName.Location = New System.Drawing.Point(65, 2)
        Me.AtatchName.Name = "AtatchName"
        Me.AtatchName.Size = New System.Drawing.Size(417, 32)
        Me.AtatchName.TabIndex = 6
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.txtMessageBody)
        Me.Panel3.Controls.Add(Me.txtSubject)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Enabled = False
        Me.Panel3.Location = New System.Drawing.Point(0, 299)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(603, 270)
        Me.Panel3.TabIndex = 5
        '
        'txtMessageBody
        '
        Me.txtMessageBody.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold)
        Me.txtMessageBody.Location = New System.Drawing.Point(69, 39)
        Me.txtMessageBody.Name = "txtMessageBody"
        Me.txtMessageBody.Size = New System.Drawing.Size(417, 210)
        Me.txtMessageBody.TabIndex = 9
        Me.txtMessageBody.Text = ""
        '
        'txtSubject
        '
        Me.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSubject.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold)
        Me.txtSubject.Location = New System.Drawing.Point(69, 6)
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.Size = New System.Drawing.Size(417, 32)
        Me.txtSubject.TabIndex = 6
        Me.txtSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label6.Location = New System.Drawing.Point(492, 106)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 24)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "الرساله"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label5.Location = New System.Drawing.Point(489, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(68, 24)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "الموضوع"
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnClose.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.Close_16x16
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(69, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(85, 35)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "إغلاق"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSend
        '
        Me.btnSend.Enabled = False
        Me.btnSend.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnSend.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.Mail_16x16__3_
        Me.btnSend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSend.Location = New System.Drawing.Point(160, 5)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(226, 35)
        Me.btnSend.TabIndex = 7
        Me.btnSend.Text = "إرسال"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnNew.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.AddItem_1616
        Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNew.Location = New System.Drawing.Point(392, 5)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(94, 35)
        Me.btnNew.TabIndex = 7
        Me.btnNew.Text = "جديد"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'EMailForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.ClientSize = New System.Drawing.Size(603, 659)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EMailForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إرسال البريد الإلكتروني"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnhelp As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents CombPorts As System.Windows.Forms.ComboBox
    Friend WithEvents CombSMTPServers As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents SenderAccPassword As System.Windows.Forms.TextBox
    Friend WithEvents ReceiverAddress As System.Windows.Forms.TextBox
    Friend WithEvents SenderAddress As System.Windows.Forms.TextBox
    Friend WithEvents txtMessageBody As System.Windows.Forms.RichTextBox
    Friend WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnAddAttaches As System.Windows.Forms.Button
    Friend WithEvents AtatchName As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Ck2 As CheckBox
    Friend WithEvents Ck1 As CheckBox
    Friend WithEvents txtHidenCopyTo As TextBox
    Friend WithEvents txtCopyTo As TextBox
    Friend WithEvents CkshowHide As CheckBox
End Class
