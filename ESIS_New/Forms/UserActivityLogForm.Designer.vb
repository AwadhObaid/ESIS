

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UserActivityLogForm
    Inherits BaseFormWithPermissions

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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PanelMain = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.PanelSearch = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblDateRange = New System.Windows.Forms.Label()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.lblActionType = New System.Windows.Forms.Label()
        Me.cmbActionType = New System.Windows.Forms.ComboBox()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.txtTableName = New System.Windows.Forms.TextBox()
        Me.txtRecordID = New System.Windows.Forms.TextBox()
        Me.lblTableName = New System.Windows.Forms.Label()
        Me.lblRecordID = New System.Windows.Forms.Label()
        Me.btnReprt = New System.Windows.Forms.Button()
        Me.btnPrintTable = New System.Windows.Forms.Button()
        Me.btnExportPdf = New System.Windows.Forms.Button()
        Me.lblTotalRecords = New System.Windows.Forms.Label()
        Me.btnExportExcel = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnClearFilter = New System.Windows.Forms.Button()
        Me.PanelGrid = New System.Windows.Forms.Panel()
        Me.dgvActivityLog = New System.Windows.Forms.DataGridView()
        Me.PanelTitle = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCopySelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCopyAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExportSelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExportAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.PanelMain.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.PanelSearch.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.PanelGrid.SuspendLayout()
        CType(Me.dgvActivityLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelTitle.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelMain
        '
        Me.PanelMain.Controls.Add(Me.SplitContainer1)
        Me.PanelMain.Controls.Add(Me.PanelTitle)
        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelMain.Location = New System.Drawing.Point(0, 0)
        Me.PanelMain.Name = "PanelMain"
        Me.PanelMain.Size = New System.Drawing.Size(1439, 700)
        Me.PanelMain.TabIndex = 0
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 36)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.PanelSearch)
        Me.SplitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.PanelGrid)
        Me.SplitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.SplitContainer1.Size = New System.Drawing.Size(1439, 664)
        Me.SplitContainer1.SplitterDistance = 302
        Me.SplitContainer1.TabIndex = 2
        '
        'PanelSearch
        '
        Me.PanelSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(228, Byte), Integer))
        Me.PanelSearch.Controls.Add(Me.Button1)
        Me.PanelSearch.Controls.Add(Me.TableLayoutPanel1)
        Me.PanelSearch.Controls.Add(Me.btnReprt)
        Me.PanelSearch.Controls.Add(Me.btnPrintTable)
        Me.PanelSearch.Controls.Add(Me.btnExportPdf)
        Me.PanelSearch.Controls.Add(Me.lblTotalRecords)
        Me.PanelSearch.Controls.Add(Me.btnExportExcel)
        Me.PanelSearch.Controls.Add(Me.btnRefresh)
        Me.PanelSearch.Controls.Add(Me.btnClearFilter)
        Me.PanelSearch.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelSearch.Location = New System.Drawing.Point(0, 0)
        Me.PanelSearch.Name = "PanelSearch"
        Me.PanelSearch.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelSearch.Size = New System.Drawing.Size(302, 664)
        Me.PanelSearch.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(7, 219)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(282, 30)
        Me.Button1.TabIndex = 20
        Me.Button1.Text = "تصفية السجلات "
        Me.Button1.UseVisualStyleBackColor = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.9589!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.0411!))
        Me.TableLayoutPanel1.Controls.Add(Me.lblDateRange, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.dtpStartDate, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblTo, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.dtpEndDate, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblActionType, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.cmbActionType, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblUserName, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txtUserName, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txtTableName, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.txtRecordID, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.lblTableName, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.lblRecordID, 0, 4)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(7, 13)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(292, 200)
        Me.TableLayoutPanel1.TabIndex = 19
        '
        'lblDateRange
        '
        Me.lblDateRange.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDateRange.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateRange.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(121, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblDateRange.Location = New System.Drawing.Point(191, 0)
        Me.lblDateRange.Name = "lblDateRange"
        Me.lblDateRange.Size = New System.Drawing.Size(98, 32)
        Me.lblDateRange.TabIndex = 6
        Me.lblDateRange.Text = "نطاق التاريخ:"
        Me.lblDateRange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(3, 3)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(182, 25)
        Me.dtpStartDate.TabIndex = 7
        '
        'lblTo
        '
        Me.lblTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTo.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(121, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblTo.Location = New System.Drawing.Point(191, 32)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(98, 35)
        Me.lblTo.TabIndex = 8
        Me.lblTo.Text = "إلى:"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(3, 35)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(182, 25)
        Me.dtpEndDate.TabIndex = 9
        '
        'lblActionType
        '
        Me.lblActionType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblActionType.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActionType.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(121, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblActionType.Location = New System.Drawing.Point(191, 67)
        Me.lblActionType.Name = "lblActionType"
        Me.lblActionType.Size = New System.Drawing.Size(98, 35)
        Me.lblActionType.TabIndex = 10
        Me.lblActionType.Text = "نوع العملية:"
        Me.lblActionType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbActionType
        '
        Me.cmbActionType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbActionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActionType.FormattingEnabled = True
        Me.cmbActionType.Items.AddRange(New Object() {"الكل", "Add", "Edit", "Delete", "Search", "Print", "Login", "Logout"})
        Me.cmbActionType.Location = New System.Drawing.Point(3, 70)
        Me.cmbActionType.Name = "cmbActionType"
        Me.cmbActionType.Size = New System.Drawing.Size(182, 25)
        Me.cmbActionType.TabIndex = 11
        '
        'lblUserName
        '
        Me.lblUserName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblUserName.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(121, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblUserName.Location = New System.Drawing.Point(191, 102)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(98, 32)
        Me.lblUserName.TabIndex = 4
        Me.lblUserName.Text = "اسم المستخدم:"
        Me.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtUserName
        '
        Me.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtUserName.Location = New System.Drawing.Point(3, 105)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(182, 25)
        Me.txtUserName.TabIndex = 5
        '
        'txtTableName
        '
        Me.txtTableName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTableName.Location = New System.Drawing.Point(3, 168)
        Me.txtTableName.Name = "txtTableName"
        Me.txtTableName.Size = New System.Drawing.Size(182, 25)
        Me.txtTableName.TabIndex = 1
        '
        'txtRecordID
        '
        Me.txtRecordID.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtRecordID.Location = New System.Drawing.Point(3, 137)
        Me.txtRecordID.Name = "txtRecordID"
        Me.txtRecordID.Size = New System.Drawing.Size(182, 25)
        Me.txtRecordID.TabIndex = 3
        '
        'lblTableName
        '
        Me.lblTableName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTableName.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTableName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(121, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblTableName.Location = New System.Drawing.Point(191, 165)
        Me.lblTableName.Name = "lblTableName"
        Me.lblTableName.Size = New System.Drawing.Size(98, 35)
        Me.lblTableName.TabIndex = 0
        Me.lblTableName.Text = "اسم الجدول:"
        Me.lblTableName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRecordID
        '
        Me.lblRecordID.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblRecordID.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecordID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(121, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblRecordID.Location = New System.Drawing.Point(191, 134)
        Me.lblRecordID.Name = "lblRecordID"
        Me.lblRecordID.Size = New System.Drawing.Size(98, 31)
        Me.lblRecordID.TabIndex = 2
        Me.lblRecordID.Text = "معرف السجل:"
        Me.lblRecordID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnReprt
        '
        Me.btnReprt.BackColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnReprt.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnReprt.FlatAppearance.BorderSize = 0
        Me.btnReprt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReprt.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReprt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReprt.Location = New System.Drawing.Point(10, 474)
        Me.btnReprt.Name = "btnReprt"
        Me.btnReprt.Size = New System.Drawing.Size(282, 30)
        Me.btnReprt.TabIndex = 18
        Me.btnReprt.Text = "تصدير التقرير بواسطة الكريستال"
        Me.btnReprt.UseVisualStyleBackColor = False
        '
        'btnPrintTable
        '
        Me.btnPrintTable.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnPrintTable.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnPrintTable.FlatAppearance.BorderSize = 0
        Me.btnPrintTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrintTable.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrintTable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrintTable.Location = New System.Drawing.Point(10, 504)
        Me.btnPrintTable.Name = "btnPrintTable"
        Me.btnPrintTable.Size = New System.Drawing.Size(282, 30)
        Me.btnPrintTable.TabIndex = 17
        Me.btnPrintTable.Text = "تصدير الجدول الداتا قريد"
        Me.btnPrintTable.UseVisualStyleBackColor = False
        '
        'btnExportPdf
        '
        Me.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(15, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnExportPdf.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnExportPdf.FlatAppearance.BorderSize = 0
        Me.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportPdf.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportPdf.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.btnExportPdf.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.file
        Me.btnExportPdf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportPdf.Location = New System.Drawing.Point(10, 534)
        Me.btnExportPdf.Name = "btnExportPdf"
        Me.btnExportPdf.Size = New System.Drawing.Size(282, 30)
        Me.btnExportPdf.TabIndex = 16
        Me.btnExportPdf.Text = "   تصدير إلى Pdf"
        Me.btnExportPdf.UseVisualStyleBackColor = False
        '
        'lblTotalRecords
        '
        Me.lblTotalRecords.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalRecords.ForeColor = System.Drawing.Color.Blue
        Me.lblTotalRecords.Location = New System.Drawing.Point(7, 431)
        Me.lblTotalRecords.Name = "lblTotalRecords"
        Me.lblTotalRecords.Size = New System.Drawing.Size(282, 30)
        Me.lblTotalRecords.TabIndex = 15
        Me.lblTotalRecords.Text = "إجمالي السجلات: 0"
        Me.lblTotalRecords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnExportExcel
        '
        Me.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(CType(CType(61, Byte), Integer), CType(CType(201, Byte), Integer), CType(CType(98, Byte), Integer))
        Me.btnExportExcel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnExportExcel.FlatAppearance.BorderSize = 0
        Me.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportExcel.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportExcel.Location = New System.Drawing.Point(10, 564)
        Me.btnExportExcel.Name = "btnExportExcel"
        Me.btnExportExcel.Size = New System.Drawing.Size(282, 30)
        Me.btnExportExcel.TabIndex = 14
        Me.btnExportExcel.Text = "   تصدير إلى Excel"
        Me.btnExportExcel.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.btnRefresh.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnRefresh.FlatAppearance.BorderSize = 0
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRefresh.Location = New System.Drawing.Point(10, 594)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(282, 30)
        Me.btnRefresh.TabIndex = 13
        Me.btnRefresh.Text = "   تحديث"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnClearFilter
        '
        Me.btnClearFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnClearFilter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnClearFilter.FlatAppearance.BorderSize = 0
        Me.btnClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClearFilter.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClearFilter.Location = New System.Drawing.Point(10, 624)
        Me.btnClearFilter.Name = "btnClearFilter"
        Me.btnClearFilter.Size = New System.Drawing.Size(282, 30)
        Me.btnClearFilter.TabIndex = 12
        Me.btnClearFilter.Text = "   مسح الفلاتر"
        Me.btnClearFilter.UseVisualStyleBackColor = False
        '
        'PanelGrid
        '
        Me.PanelGrid.Controls.Add(Me.dgvActivityLog)
        Me.PanelGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelGrid.Location = New System.Drawing.Point(0, 0)
        Me.PanelGrid.Name = "PanelGrid"
        Me.PanelGrid.Size = New System.Drawing.Size(1133, 664)
        Me.PanelGrid.TabIndex = 0
        '
        'dgvActivityLog
        '
        Me.dgvActivityLog.AllowUserToAddRows = False
        Me.dgvActivityLog.AllowUserToDeleteRows = False
        Me.dgvActivityLog.AllowUserToOrderColumns = True
        Me.dgvActivityLog.AllowUserToResizeRows = False
        Me.dgvActivityLog.BackgroundColor = System.Drawing.Color.White
        Me.dgvActivityLog.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvActivityLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.dgvActivityLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvActivityLog.DefaultCellStyle = DataGridViewCellStyle8
        Me.dgvActivityLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvActivityLog.Location = New System.Drawing.Point(0, 0)
        Me.dgvActivityLog.MultiSelect = False
        Me.dgvActivityLog.Name = "dgvActivityLog"
        Me.dgvActivityLog.ReadOnly = True
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvActivityLog.RowHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.dgvActivityLog.RowHeadersWidth = 51
        Me.dgvActivityLog.RowTemplate.Height = 29
        Me.dgvActivityLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvActivityLog.Size = New System.Drawing.Size(1133, 664)
        Me.dgvActivityLog.TabIndex = 0
        '
        'PanelTitle
        '
        Me.PanelTitle.BackColor = System.Drawing.Color.SteelBlue
        Me.PanelTitle.Controls.Add(Me.lblTitle)
        Me.PanelTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTitle.Location = New System.Drawing.Point(0, 0)
        Me.PanelTitle.Name = "PanelTitle"
        Me.PanelTitle.Size = New System.Drawing.Size(1439, 36)
        Me.PanelTitle.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Cairo", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(652, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(134, 36)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "سجلات الحركة"
        '
        'TimerRefresh
        '
        Me.TimerRefresh.Interval = 300000
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCopySelected, Me.mnuCopyAll, Me.ToolStripSeparator1, Me.mnuExportSelected, Me.mnuExportAll})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(192, 98)
        '
        'mnuCopySelected
        '
        Me.mnuCopySelected.Name = "mnuCopySelected"
        Me.mnuCopySelected.Size = New System.Drawing.Size(191, 22)
        Me.mnuCopySelected.Text = "نسخ السطر المحدد"
        '
        'mnuCopyAll
        '
        Me.mnuCopyAll.Name = "mnuCopyAll"
        Me.mnuCopyAll.Size = New System.Drawing.Size(191, 22)
        Me.mnuCopyAll.Text = "نسخ الكل"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(188, 6)
        '
        'mnuExportSelected
        '
        Me.mnuExportSelected.Name = "mnuExportSelected"
        Me.mnuExportSelected.Size = New System.Drawing.Size(191, 22)
        Me.mnuExportSelected.Text = "تصدير المحدد إلى Excel"
        '
        'mnuExportAll
        '
        Me.mnuExportAll.Name = "mnuExportAll"
        Me.mnuExportAll.Size = New System.Drawing.Size(191, 22)
        Me.mnuExportAll.Text = "تصدير الكل إلى Excel"
        '
        'UserActivityLogForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1439, 700)
        Me.Controls.Add(Me.PanelMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UserActivityLogForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "UserActivityLogForm"
        Me.PanelMain.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.PanelSearch.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.PanelGrid.ResumeLayout(False)
        CType(Me.dgvActivityLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelTitle.ResumeLayout(False)
        Me.PanelTitle.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelMain As Panel
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents PanelSearch As Panel
    Friend WithEvents txtTableName As TextBox
    Friend WithEvents lblTableName As Label
    Friend WithEvents txtRecordID As TextBox
    Friend WithEvents lblRecordID As Label
    Friend WithEvents txtUserName As TextBox
    Friend WithEvents lblUserName As Label
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents lblTo As Label
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents lblDateRange As Label
    Friend WithEvents cmbActionType As ComboBox
    Friend WithEvents lblActionType As Label
    Friend WithEvents btnClearFilter As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnExportExcel As Button
    Friend WithEvents PanelGrid As Panel
    Friend WithEvents dgvActivityLog As DataGridView
    Friend WithEvents PanelTitle As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents TimerRefresh As Timer
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents mnuCopySelected As ToolStripMenuItem
    Friend WithEvents mnuCopyAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents mnuExportSelected As ToolStripMenuItem
    Friend WithEvents mnuExportAll As ToolStripMenuItem
    Friend WithEvents lblTotalRecords As Label
    Friend WithEvents btnExportPdf As Button
    Friend WithEvents btnPrintTable As Button
    Friend WithEvents btnReprt As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Button1 As Button
End Class