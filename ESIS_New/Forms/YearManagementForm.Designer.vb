<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YearManagementForm
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvYears = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnOpenYear = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnAddYear = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.TimerStatus = New System.Windows.Forms.Timer(Me.components)
        Me.btnCloseYear = New System.Windows.Forms.Button()
        Me.txtYear = New System.Windows.Forms.TextBox()
        Me.btnDeleteYear = New System.Windows.Forms.Button()
        CType(Me.dgvYears, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvYears
        '
        Me.dgvYears.AllowUserToAddRows = False
        Me.dgvYears.AllowUserToDeleteRows = False
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Cairo SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvYears.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvYears.ColumnHeadersHeight = 30
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Cairo SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvYears.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvYears.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvYears.Location = New System.Drawing.Point(0, 91)
        Me.dgvYears.Name = "dgvYears"
        Me.dgvYears.ReadOnly = True
        Me.dgvYears.RightToLeft = System.Windows.Forms.RightToLeft.No
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Cairo SemiBold", 8.249999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvYears.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvYears.Size = New System.Drawing.Size(767, 237)
        Me.dgvYears.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.dgvYears)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(767, 375)
        Me.Panel1.TabIndex = 1
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel4.Controls.Add(Me.txtYear)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(767, 49)
        Me.Panel4.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Font = New System.Drawing.Font("Cairo", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(328, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 32)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "إدارة السنوات"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 52)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(767, 39)
        Me.Panel2.TabIndex = 2
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnDeleteYear, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnCloseYear, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnOpenYear, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnAddYear, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnRefresh, 3, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(767, 39)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'btnOpenYear
        '
        Me.btnOpenYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnOpenYear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnOpenYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOpenYear.Font = New System.Drawing.Font("Cairo", 7.0!, System.Drawing.FontStyle.Bold)
        Me.btnOpenYear.ForeColor = System.Drawing.Color.White
        Me.btnOpenYear.Location = New System.Drawing.Point(463, 4)
        Me.btnOpenYear.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnOpenYear.Name = "btnOpenYear"
        Me.btnOpenYear.Size = New System.Drawing.Size(149, 31)
        Me.btnOpenYear.TabIndex = 59
        Me.btnOpenYear.Text = "🔓 فتح السنة"
        Me.btnOpenYear.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Cairo", 7.0!, System.Drawing.FontStyle.Bold)
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.Location = New System.Drawing.Point(157, 4)
        Me.btnRefresh.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(149, 31)
        Me.btnRefresh.TabIndex = 59
        Me.btnRefresh.Text = "🔄 تحديث"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnAddYear
        '
        Me.btnAddYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnAddYear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnAddYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddYear.Font = New System.Drawing.Font("Cairo", 7.0!, System.Drawing.FontStyle.Bold)
        Me.btnAddYear.ForeColor = System.Drawing.Color.White
        Me.btnAddYear.Location = New System.Drawing.Point(616, 4)
        Me.btnAddYear.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnAddYear.Name = "btnAddYear"
        Me.btnAddYear.Size = New System.Drawing.Size(149, 31)
        Me.btnAddYear.TabIndex = 56
        Me.btnAddYear.Text = "➕ بدء سنة جديدة"
        Me.btnAddYear.UseVisualStyleBackColor = False
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lblStatus)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 328)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(767, 47)
        Me.Panel3.TabIndex = 4
        '
        'lblStatus
        '
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(767, 47)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimerStatus
        '
        Me.TimerStatus.Interval = 3000
        '
        'btnCloseYear
        '
        Me.btnCloseYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnCloseYear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCloseYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCloseYear.Font = New System.Drawing.Font("Cairo", 7.0!, System.Drawing.FontStyle.Bold)
        Me.btnCloseYear.ForeColor = System.Drawing.Color.White
        Me.btnCloseYear.Location = New System.Drawing.Point(310, 4)
        Me.btnCloseYear.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnCloseYear.Name = "btnCloseYear"
        Me.btnCloseYear.Size = New System.Drawing.Size(149, 31)
        Me.btnCloseYear.TabIndex = 60
        Me.btnCloseYear.Text = "🔒 إغلاق السنة"
        Me.btnCloseYear.UseVisualStyleBackColor = False
        '
        'txtYear
        '
        Me.txtYear.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtYear.Location = New System.Drawing.Point(638, 6)
        Me.txtYear.Name = "txtYear"
        Me.txtYear.Size = New System.Drawing.Size(115, 32)
        Me.txtYear.TabIndex = 6
        '
        'btnDeleteYear
        '
        Me.btnDeleteYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.btnDeleteYear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDeleteYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDeleteYear.Font = New System.Drawing.Font("Cairo", 7.0!, System.Drawing.FontStyle.Bold)
        Me.btnDeleteYear.ForeColor = System.Drawing.Color.White
        Me.btnDeleteYear.Location = New System.Drawing.Point(2, 4)
        Me.btnDeleteYear.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnDeleteYear.Name = "btnDeleteYear"
        Me.btnDeleteYear.Size = New System.Drawing.Size(151, 31)
        Me.btnDeleteYear.TabIndex = 61
        Me.btnDeleteYear.Text = "🔄 حذف السة "
        Me.btnDeleteYear.UseVisualStyleBackColor = False
        '
        'YearManagementForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(767, 375)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "YearManagementForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إدارة السنوات"
        CType(Me.dgvYears, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvYears As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnAddYear As Button
    Friend WithEvents TimerStatus As Timer
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblStatus As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents btnOpenYear As Button
    Friend WithEvents btnCloseYear As Button
    Friend WithEvents txtYear As TextBox
    Friend WithEvents btnDeleteYear As Button
End Class
