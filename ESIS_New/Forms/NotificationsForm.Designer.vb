'Namespace ESIS.Forms
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class NotificationsForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the list of components.
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

        'Required by the Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Form Designer
        'It can be modified using the Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.lblMessage = New System.Windows.Forms.Label()
            Me.btnArchiveAll = New System.Windows.Forms.Button()
            Me.btnClose = New System.Windows.Forms.Button()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.pnlHeader.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.Panel2.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(185, Byte), Integer))
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Size = New System.Drawing.Size(325, 46)
            Me.pnlHeader.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Font = New System.Drawing.Font("Cairo SemiBold", 12.0!, System.Drawing.FontStyle.Bold)
            Me.lblTitle.ForeColor = System.Drawing.Color.White
            Me.lblTitle.Location = New System.Drawing.Point(90, 8)
            Me.lblTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(144, 30)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "سجلات غير مؤرشفة"
            '
            'lblMessage
            '
            Me.lblMessage.AutoSize = True
            Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Hand
            Me.lblMessage.Font = New System.Drawing.Font("Cairo SemiBold", 12.0!, System.Drawing.FontStyle.Bold)
            Me.lblMessage.Location = New System.Drawing.Point(66, 5)
            Me.lblMessage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.lblMessage.Name = "lblMessage"
            Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.lblMessage.Size = New System.Drawing.Size(197, 30)
            Me.lblMessage.TabIndex = 1
            Me.lblMessage.Text = "لديك 5 سجلات غير مؤرشفة."
            Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnArchiveAll
            '
            Me.btnArchiveAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(185, Byte), Integer))
            Me.btnArchiveAll.FlatAppearance.BorderSize = 0
            Me.btnArchiveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnArchiveAll.Font = New System.Drawing.Font("Cairo SemiBold", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnArchiveAll.ForeColor = System.Drawing.Color.White
            Me.btnArchiveAll.Location = New System.Drawing.Point(187, 7)
            Me.btnArchiveAll.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.btnArchiveAll.Name = "btnArchiveAll"
            Me.btnArchiveAll.Size = New System.Drawing.Size(106, 30)
            Me.btnArchiveAll.TabIndex = 2
            Me.btnArchiveAll.Text = "أرشفة الكل"
            Me.btnArchiveAll.UseVisualStyleBackColor = False
            '
            'btnClose
            '
            Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(76, Byte), Integer), CType(CType(60, Byte), Integer))
            Me.btnClose.FlatAppearance.BorderSize = 0
            Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClose.Font = New System.Drawing.Font("Cairo SemiBold", 10.0!, System.Drawing.FontStyle.Bold)
            Me.btnClose.ForeColor = System.Drawing.Color.White
            Me.btnClose.Location = New System.Drawing.Point(31, 7)
            Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(106, 30)
            Me.btnClose.TabIndex = 3
            Me.btnClose.Text = "إغلاق"
            Me.btnClose.UseVisualStyleBackColor = False
            '
            'Panel1
            '
            Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(200, Byte), Integer))
            Me.Panel1.Controls.Add(Me.btnArchiveAll)
            Me.Panel1.Controls.Add(Me.btnClose)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.Panel1.Location = New System.Drawing.Point(0, 199)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(325, 45)
            Me.Panel1.TabIndex = 4
            '
            'Panel2
            '
            Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
            Me.Panel2.Controls.Add(Me.lblMessage)
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel2.Location = New System.Drawing.Point(0, 46)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(325, 153)
            Me.Panel2.TabIndex = 5
            '
            'NotificationsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 30.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(325, 244)
            Me.Controls.Add(Me.Panel2)
            Me.Controls.Add(Me.Panel1)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New System.Drawing.Font("Cairo SemiBold", 12.0!, System.Drawing.FontStyle.Bold)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.Name = "NotificationsForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.Text = "الإشعارات"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.Panel1.ResumeLayout(False)
            Me.Panel2.ResumeLayout(False)
            Me.Panel2.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlHeader As Panel
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblMessage As Label
        Friend WithEvents btnArchiveAll As Button
        Friend WithEvents btnClose As Button
        Friend WithEvents Panel1 As Panel
        Friend WithEvents Panel2 As Panel
    End Class
'End Namespace