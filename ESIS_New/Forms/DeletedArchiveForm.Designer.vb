<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DeletedArchiveForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvDeletedArchive = New System.Windows.Forms.DataGridView()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnDefaultSettings = New System.Windows.Forms.Button()
        Me.btnStartCleanup = New System.Windows.Forms.Button()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.btnPermanentDelete = New System.Windows.Forms.Button()
        Me.txtDaysToKeep = New System.Windows.Forms.TextBox()
        Me.lblRecordCount = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblCountdown = New System.Windows.Forms.Label()
        CType(Me.dgvDeletedArchive, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvDeletedArchive
        '
        Me.dgvDeletedArchive.AllowUserToAddRows = False
        Me.dgvDeletedArchive.AllowUserToDeleteRows = False
        Me.dgvDeletedArchive.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvDeletedArchive.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDeletedArchive.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvDeletedArchive.ColumnHeadersHeight = 30
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvDeletedArchive.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvDeletedArchive.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvDeletedArchive.Location = New System.Drawing.Point(0, 137)
        Me.dgvDeletedArchive.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.dgvDeletedArchive.MultiSelect = False
        Me.dgvDeletedArchive.Name = "dgvDeletedArchive"
        Me.dgvDeletedArchive.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDeletedArchive.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvDeletedArchive.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.dgvDeletedArchive.RowTemplate.Height = 24
        Me.dgvDeletedArchive.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDeletedArchive.Size = New System.Drawing.Size(1328, 373)
        Me.dgvDeletedArchive.TabIndex = 4
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnDefaultSettings, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnStartCleanup, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnRestore, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnPermanentDelete, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.txtDaysToKeep, 4, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 94)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1328, 43)
        Me.TableLayoutPanel1.TabIndex = 23
        '
        'btnDefaultSettings
        '
        Me.btnDefaultSettings.BackColor = System.Drawing.Color.FromArgb(CType(CType(202, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnDefaultSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDefaultSettings.FlatAppearance.BorderSize = 0
        Me.btnDefaultSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDefaultSettings.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDefaultSettings.ForeColor = System.Drawing.Color.White
        Me.btnDefaultSettings.Location = New System.Drawing.Point(270, 4)
        Me.btnDefaultSettings.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnDefaultSettings.Name = "btnDefaultSettings"
        Me.btnDefaultSettings.Size = New System.Drawing.Size(261, 35)
        Me.btnDefaultSettings.TabIndex = 28
        Me.btnDefaultSettings.Text = "إعدادات إفتراضية"
        Me.btnDefaultSettings.UseVisualStyleBackColor = False
        '
        'btnStartCleanup
        '
        Me.btnStartCleanup.BackColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnStartCleanup.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnStartCleanup.FlatAppearance.BorderSize = 0
        Me.btnStartCleanup.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnStartCleanup.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStartCleanup.ForeColor = System.Drawing.Color.White
        Me.btnStartCleanup.Location = New System.Drawing.Point(535, 4)
        Me.btnStartCleanup.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnStartCleanup.Name = "btnStartCleanup"
        Me.btnStartCleanup.Size = New System.Drawing.Size(261, 35)
        Me.btnStartCleanup.TabIndex = 29
        Me.btnStartCleanup.Text = "حذف بعد مدة "
        Me.btnStartCleanup.UseVisualStyleBackColor = False
        '
        'btnRestore
        '
        Me.btnRestore.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnRestore.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnRestore.FlatAppearance.BorderSize = 0
        Me.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRestore.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRestore.ForeColor = System.Drawing.Color.White
        Me.btnRestore.Location = New System.Drawing.Point(1065, 4)
        Me.btnRestore.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(261, 35)
        Me.btnRestore.TabIndex = 14
        Me.btnRestore.Text = "إستعادة"
        Me.btnRestore.UseVisualStyleBackColor = False
        '
        'btnPermanentDelete
        '
        Me.btnPermanentDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnPermanentDelete.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnPermanentDelete.FlatAppearance.BorderSize = 0
        Me.btnPermanentDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPermanentDelete.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPermanentDelete.ForeColor = System.Drawing.Color.White
        Me.btnPermanentDelete.Location = New System.Drawing.Point(800, 4)
        Me.btnPermanentDelete.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnPermanentDelete.Name = "btnPermanentDelete"
        Me.btnPermanentDelete.Size = New System.Drawing.Size(261, 35)
        Me.btnPermanentDelete.TabIndex = 16
        Me.btnPermanentDelete.Text = "حذف نهائي"
        Me.btnPermanentDelete.UseVisualStyleBackColor = False
        '
        'txtDaysToKeep
        '
        Me.txtDaysToKeep.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDaysToKeep.Font = New System.Drawing.Font("Cairo SemiBold", 12.0!, System.Drawing.FontStyle.Bold)
        Me.txtDaysToKeep.Location = New System.Drawing.Point(3, 3)
        Me.txtDaysToKeep.Name = "txtDaysToKeep"
        Me.txtDaysToKeep.Size = New System.Drawing.Size(262, 37)
        Me.txtDaysToKeep.TabIndex = 28
        Me.txtDaysToKeep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblRecordCount
        '
        Me.lblRecordCount.AutoSize = True
        Me.lblRecordCount.Location = New System.Drawing.Point(1291, 18)
        Me.lblRecordCount.Name = "lblRecordCount"
        Me.lblRecordCount.Size = New System.Drawing.Size(19, 24)
        Me.lblRecordCount.TabIndex = 24
        Me.lblRecordCount.Text = "..."
        Me.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Cairo SemiBold", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(575, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(179, 45)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "سلة المحذوفات"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(1079, 59)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(244, 32)
        Me.txtSearch.TabIndex = 26
        Me.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Cairo SemiBold", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(880, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(195, 30)
        Me.Label2.TabIndex = 27
        Me.Label2.Text = "البحث في سلة المحذوفات :"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblRecordCount)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1328, 53)
        Me.Panel1.TabIndex = 28
        '
        'lblCountdown
        '
        Me.lblCountdown.AutoSize = True
        Me.lblCountdown.Font = New System.Drawing.Font("Cairo SemiBold", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblCountdown.ForeColor = System.Drawing.Color.Red
        Me.lblCountdown.Location = New System.Drawing.Point(12, 58)
        Me.lblCountdown.Name = "lblCountdown"
        Me.lblCountdown.Size = New System.Drawing.Size(25, 30)
        Me.lblCountdown.TabIndex = 27
        Me.lblCountdown.Text = "..."
        '
        'DeletedArchiveForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1328, 510)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblCountdown)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.dgvDeletedArchive)
        Me.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold)
        Me.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DeletedArchiveForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "سلة المحذوفات"
        CType(Me.dgvDeletedArchive, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvDeletedArchive As DataGridView
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents btnRestore As Button
    Friend WithEvents btnPermanentDelete As Button
    Friend WithEvents lblRecordCount As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtDaysToKeep As TextBox
    Friend WithEvents btnStartCleanup As Button
    Friend WithEvents btnDefaultSettings As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblCountdown As Label
End Class
