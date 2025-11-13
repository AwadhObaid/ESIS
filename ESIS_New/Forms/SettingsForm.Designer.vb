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
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtdaysToKeep = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnBrowseArchiveDefaultPath = New System.Windows.Forms.Button()
        Me.txtlastSavedNumber = New System.Windows.Forms.TextBox()
        Me.txtSearchDefaultPath = New System.Windows.Forms.TextBox()
        Me.txtArchiveDefaultPath = New System.Windows.Forms.TextBox()
        Me.btnBrowseSearchDefaultPath = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.nudNotificationInterval = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.nudNotificationInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
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
        Me.GroupBox1.Size = New System.Drawing.Size(442, 162)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "المسار الإفتراضي"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(149, 44)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(183, 29)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "العد التنازلي للحذف التلقائي"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtdaysToKeep
        '
        Me.txtdaysToKeep.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold)
        Me.txtdaysToKeep.Location = New System.Drawing.Point(14, 47)
        Me.txtdaysToKeep.Name = "txtdaysToKeep"
        Me.txtdaysToKeep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtdaysToKeep.Size = New System.Drawing.Size(126, 23)
        Me.txtdaysToKeep.TabIndex = 7
        Me.txtdaysToKeep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(327, 116)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(109, 29)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "رقــــم الكـتـاب :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(328, 32)
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
        Me.btnBrowseArchiveDefaultPath.Font = New System.Drawing.Font("Cairo SemiBold", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseArchiveDefaultPath.ForeColor = System.Drawing.Color.Cornsilk
        Me.btnBrowseArchiveDefaultPath.Location = New System.Drawing.Point(10, 32)
        Me.btnBrowseArchiveDefaultPath.Name = "btnBrowseArchiveDefaultPath"
        Me.btnBrowseArchiveDefaultPath.Size = New System.Drawing.Size(63, 29)
        Me.btnBrowseArchiveDefaultPath.TabIndex = 2
        Me.btnBrowseArchiveDefaultPath.Text = "إستعراض"
        Me.btnBrowseArchiveDefaultPath.UseVisualStyleBackColor = False
        '
        'txtlastSavedNumber
        '
        Me.txtlastSavedNumber.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold)
        Me.txtlastSavedNumber.Location = New System.Drawing.Point(170, 120)
        Me.txtlastSavedNumber.Name = "txtlastSavedNumber"
        Me.txtlastSavedNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtlastSavedNumber.Size = New System.Drawing.Size(144, 23)
        Me.txtlastSavedNumber.TabIndex = 5
        Me.txtlastSavedNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSearchDefaultPath
        '
        Me.txtSearchDefaultPath.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold)
        Me.txtSearchDefaultPath.Location = New System.Drawing.Point(79, 79)
        Me.txtSearchDefaultPath.Name = "txtSearchDefaultPath"
        Me.txtSearchDefaultPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSearchDefaultPath.Size = New System.Drawing.Size(235, 23)
        Me.txtSearchDefaultPath.TabIndex = 0
        '
        'txtArchiveDefaultPath
        '
        Me.txtArchiveDefaultPath.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArchiveDefaultPath.Location = New System.Drawing.Point(79, 35)
        Me.txtArchiveDefaultPath.Name = "txtArchiveDefaultPath"
        Me.txtArchiveDefaultPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtArchiveDefaultPath.Size = New System.Drawing.Size(235, 23)
        Me.txtArchiveDefaultPath.TabIndex = 0
        '
        'btnBrowseSearchDefaultPath
        '
        Me.btnBrowseSearchDefaultPath.BackColor = System.Drawing.Color.CornflowerBlue
        Me.btnBrowseSearchDefaultPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowseSearchDefaultPath.Font = New System.Drawing.Font("Cairo SemiBold", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseSearchDefaultPath.ForeColor = System.Drawing.Color.Cornsilk
        Me.btnBrowseSearchDefaultPath.Location = New System.Drawing.Point(10, 76)
        Me.btnBrowseSearchDefaultPath.Name = "btnBrowseSearchDefaultPath"
        Me.btnBrowseSearchDefaultPath.Size = New System.Drawing.Size(63, 29)
        Me.btnBrowseSearchDefaultPath.TabIndex = 4
        Me.btnBrowseSearchDefaultPath.Text = "إستعراض"
        Me.btnBrowseSearchDefaultPath.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(327, 74)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(109, 29)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "مســـار البــحث :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Location = New System.Drawing.Point(353, 10)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(110, 43)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "حفظ وإغلاق"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(817, 41)
        Me.Panel1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(351, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(114, 29)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "شاشة الإعدادات"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(182, 82)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(150, 29)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "تحديد إغلاق الاشعارات"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'nudNotificationInterval
        '
        Me.nudNotificationInterval.Font = New System.Drawing.Font("Cairo SemiBold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudNotificationInterval.Location = New System.Drawing.Point(73, 81)
        Me.nudNotificationInterval.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudNotificationInterval.Name = "nudNotificationInterval"
        Me.nudNotificationInterval.Size = New System.Drawing.Size(67, 30)
        Me.nudNotificationInterval.TabIndex = 11
        Me.nudNotificationInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.nudNotificationInterval)
        Me.GroupBox2.Controls.Add(Me.txtdaysToKeep)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Location = New System.Drawing.Point(469, 47)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(343, 162)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "إعدادات التوقيت"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnSave)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 215)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(817, 63)
        Me.Panel2.TabIndex = 3
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 29.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(817, 278)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 7, 4, 7)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(833, 317)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(833, 317)
        Me.Name = "SettingsForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "شاشة الإعدادات"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.nudNotificationInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Panel2.ResumeLayout(False)
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
    Friend WithEvents Label6 As Label
    Friend WithEvents nudNotificationInterval As NumericUpDown
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Panel2 As Panel
End Class
