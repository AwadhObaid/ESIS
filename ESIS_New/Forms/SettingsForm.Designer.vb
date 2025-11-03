<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtlastSavedNumber = New System.Windows.Forms.TextBox()
        Me.btnBrowseSearchDefaultPath = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnBrowseArchiveDefaultPath = New System.Windows.Forms.Button()
        Me.txtSearchDefaultPath = New System.Windows.Forms.TextBox()
        Me.txtArchiveDefaultPath = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtdaysToKeep = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.txtdaysToKeep)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.btnBrowseArchiveDefaultPath)
        Me.GroupBox1.Controls.Add(Me.txtlastSavedNumber)
        Me.GroupBox1.Controls.Add(Me.txtSearchDefaultPath)
        Me.GroupBox1.Controls.Add(Me.txtArchiveDefaultPath)
        Me.GroupBox1.Controls.Add(Me.btnBrowseSearchDefaultPath)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 47)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(568, 241)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "المسار الإفتراضي"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(466, 127)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(85, 29)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "رقم الكتاب :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtlastSavedNumber
        '
        Me.txtlastSavedNumber.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold)
        Me.txtlastSavedNumber.Location = New System.Drawing.Point(76, 130)
        Me.txtlastSavedNumber.Name = "txtlastSavedNumber"
        Me.txtlastSavedNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtlastSavedNumber.Size = New System.Drawing.Size(382, 23)
        Me.txtlastSavedNumber.TabIndex = 5
        Me.txtlastSavedNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnBrowseSearchDefaultPath
        '
        Me.btnBrowseSearchDefaultPath.BackColor = System.Drawing.Color.CornflowerBlue
        Me.btnBrowseSearchDefaultPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowseSearchDefaultPath.Font = New System.Drawing.Font("Cairo SemiBold", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseSearchDefaultPath.ForeColor = System.Drawing.Color.Cornsilk
        Me.btnBrowseSearchDefaultPath.Location = New System.Drawing.Point(4, 84)
        Me.btnBrowseSearchDefaultPath.Name = "btnBrowseSearchDefaultPath"
        Me.btnBrowseSearchDefaultPath.Size = New System.Drawing.Size(63, 32)
        Me.btnBrowseSearchDefaultPath.TabIndex = 4
        Me.btnBrowseSearchDefaultPath.Text = "إستعراض"
        Me.btnBrowseSearchDefaultPath.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(466, 86)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(89, 29)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "مسار البحث :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(458, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(106, 29)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "مسار الارشيف :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnBrowseArchiveDefaultPath
        '
        Me.btnBrowseArchiveDefaultPath.BackColor = System.Drawing.Color.CornflowerBlue
        Me.btnBrowseArchiveDefaultPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowseArchiveDefaultPath.Font = New System.Drawing.Font("Cairo SemiBold", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseArchiveDefaultPath.ForeColor = System.Drawing.Color.Cornsilk
        Me.btnBrowseArchiveDefaultPath.Location = New System.Drawing.Point(7, 30)
        Me.btnBrowseArchiveDefaultPath.Name = "btnBrowseArchiveDefaultPath"
        Me.btnBrowseArchiveDefaultPath.Size = New System.Drawing.Size(63, 32)
        Me.btnBrowseArchiveDefaultPath.TabIndex = 2
        Me.btnBrowseArchiveDefaultPath.Text = "إستعراض"
        Me.btnBrowseArchiveDefaultPath.UseVisualStyleBackColor = False
        '
        'txtSearchDefaultPath
        '
        Me.txtSearchDefaultPath.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold)
        Me.txtSearchDefaultPath.Location = New System.Drawing.Point(76, 89)
        Me.txtSearchDefaultPath.Name = "txtSearchDefaultPath"
        Me.txtSearchDefaultPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSearchDefaultPath.Size = New System.Drawing.Size(382, 23)
        Me.txtSearchDefaultPath.TabIndex = 0
        '
        'txtArchiveDefaultPath
        '
        Me.txtArchiveDefaultPath.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArchiveDefaultPath.Location = New System.Drawing.Point(76, 35)
        Me.txtArchiveDefaultPath.Name = "txtArchiveDefaultPath"
        Me.txtArchiveDefaultPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtArchiveDefaultPath.Size = New System.Drawing.Size(382, 23)
        Me.txtArchiveDefaultPath.TabIndex = 0
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Location = New System.Drawing.Point(270, 381)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(85, 43)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "حفظ"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(595, 41)
        Me.Panel1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(240, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(114, 29)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "شاشة الإعدادات"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(368, 176)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(183, 29)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "العد التنازلي للحذف التلقائي"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtdaysToKeep
        '
        Me.txtdaysToKeep.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold)
        Me.txtdaysToKeep.Location = New System.Drawing.Point(76, 176)
        Me.txtdaysToKeep.Name = "txtdaysToKeep"
        Me.txtdaysToKeep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtdaysToKeep.Size = New System.Drawing.Size(246, 23)
        Me.txtdaysToKeep.TabIndex = 7
        Me.txtdaysToKeep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 29.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(595, 461)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnSave)
        Me.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 7, 4, 7)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(611, 500)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(611, 317)
        Me.Name = "SettingsForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "شاشة الإعدادات"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtArchiveDefaultPath As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents btnBrowseArchiveDefaultPath As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents txtSearchDefaultPath As TextBox
    Friend WithEvents btnBrowseSearchDefaultPath As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents txtlastSavedNumber As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtdaysToKeep As TextBox
End Class
