<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAlerts
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblAppTitle = New System.Windows.Forms.Label()
        Me.pnlFilter = New System.Windows.Forms.Panel()
        Me.pnlFilterHdr = New System.Windows.Forms.Panel()
        Me.lblFilterHdr = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblFilterStatus = New System.Windows.Forms.Label()
        Me.cmbFilterStatus = New System.Windows.Forms.ComboBox()
        Me.lblFilterType = New System.Windows.Forms.Label()
        Me.cmbFilterType = New System.Windows.Forms.ComboBox()
        Me.pnlFilterDiv = New System.Windows.Forms.Panel()
        Me.lblSummaryHdr = New System.Windows.Forms.Label()
        Me.lblTotalLbl = New System.Windows.Forms.Label()
        Me.lblTotalVal = New System.Windows.Forms.Label()
        Me.lblActiveLbl = New System.Windows.Forms.Label()
        Me.lblActiveVal = New System.Windows.Forms.Label()
        Me.lblInactiveLbl = New System.Windows.Forms.Label()
        Me.lblInactiveVal = New System.Windows.Forms.Label()
        Me.pnlInput = New System.Windows.Forms.Panel()
        Me.pnlInputHdr = New System.Windows.Forms.Panel()
        Me.lblInputHdr = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.pnlDiv1 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbDayOfWeek = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.numDayOfMonth = New System.Windows.Forms.NumericUpDown()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.dtpTime = New System.Windows.Forms.DateTimePicker()
        Me.pnlDiv2 = New System.Windows.Forms.Panel()
        Me.chkActive = New System.Windows.Forms.CheckBox()
        Me.chkRepeat = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.numRepeatInterval = New System.Windows.Forms.NumericUpDown()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.pnlGridHdr = New System.Windows.Forms.Panel()
        Me.lblGridHdr = New System.Windows.Forms.Label()
        Me.lblRecordCount = New System.Windows.Forms.Label()
        Me.dgvAlerts = New System.Windows.Forms.DataGridView()
        Me.pnlHeader.SuspendLayout()
        Me.pnlFilter.SuspendLayout()
        Me.pnlFilterHdr.SuspendLayout()
        Me.pnlInput.SuspendLayout()
        Me.pnlInputHdr.SuspendLayout()
        CType(Me.numDayOfMonth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numRepeatInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGrid.SuspendLayout()
        Me.pnlGridHdr.SuspendLayout()
        CType(Me.dgvAlerts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.lblAppTitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1443, 46)
        Me.pnlHeader.TabIndex = 0
        '
        'lblAppTitle
        '
        Me.lblAppTitle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAppTitle.AutoSize = True
        Me.lblAppTitle.Font = New System.Drawing.Font("Cairo", 11.0!, System.Drawing.FontStyle.Bold)
        Me.lblAppTitle.ForeColor = System.Drawing.Color.White
        Me.lblAppTitle.Location = New System.Drawing.Point(631, 9)
        Me.lblAppTitle.Name = "lblAppTitle"
        Me.lblAppTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblAppTitle.Size = New System.Drawing.Size(137, 29)
        Me.lblAppTitle.TabIndex = 0
        Me.lblAppTitle.Text = "إدارة التنبيهات 🔔"
        '
        'pnlFilter
        '
        Me.pnlFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlFilter.BackColor = System.Drawing.Color.White
        Me.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlFilter.Controls.Add(Me.pnlFilterHdr)
        Me.pnlFilter.Controls.Add(Me.lblSearch)
        Me.pnlFilter.Controls.Add(Me.txtSearch)
        Me.pnlFilter.Controls.Add(Me.lblFilterStatus)
        Me.pnlFilter.Controls.Add(Me.cmbFilterStatus)
        Me.pnlFilter.Controls.Add(Me.lblFilterType)
        Me.pnlFilter.Controls.Add(Me.cmbFilterType)
        Me.pnlFilter.Controls.Add(Me.pnlFilterDiv)
        Me.pnlFilter.Controls.Add(Me.lblSummaryHdr)
        Me.pnlFilter.Controls.Add(Me.lblTotalLbl)
        Me.pnlFilter.Controls.Add(Me.lblTotalVal)
        Me.pnlFilter.Controls.Add(Me.lblActiveLbl)
        Me.pnlFilter.Controls.Add(Me.lblActiveVal)
        Me.pnlFilter.Controls.Add(Me.lblInactiveLbl)
        Me.pnlFilter.Controls.Add(Me.lblInactiveVal)
        Me.pnlFilter.Location = New System.Drawing.Point(8, 54)
        Me.pnlFilter.Name = "pnlFilter"
        Me.pnlFilter.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.pnlFilter.Size = New System.Drawing.Size(187, 517)
        Me.pnlFilter.TabIndex = 1
        '
        'pnlFilterHdr
        '
        Me.pnlFilterHdr.BackColor = System.Drawing.Color.FromArgb(CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.pnlFilterHdr.Controls.Add(Me.lblFilterHdr)
        Me.pnlFilterHdr.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlFilterHdr.Location = New System.Drawing.Point(0, 0)
        Me.pnlFilterHdr.Name = "pnlFilterHdr"
        Me.pnlFilterHdr.Size = New System.Drawing.Size(185, 34)
        Me.pnlFilterHdr.TabIndex = 0
        '
        'lblFilterHdr
        '
        Me.lblFilterHdr.AutoSize = True
        Me.lblFilterHdr.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblFilterHdr.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.lblFilterHdr.Location = New System.Drawing.Point(40, 4)
        Me.lblFilterHdr.Name = "lblFilterHdr"
        Me.lblFilterHdr.Size = New System.Drawing.Size(128, 26)
        Me.lblFilterHdr.TabIndex = 0
        Me.lblFilterHdr.Text = "🔍  البحث والفلترة"
        '
        'lblSearch
        '
        Me.lblSearch.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblSearch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.lblSearch.Location = New System.Drawing.Point(10, 44)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(172, 18)
        Me.lblSearch.TabIndex = 1
        Me.lblSearch.Text = "بحث"
        Me.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.txtSearch.Location = New System.Drawing.Point(10, 64)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(172, 32)
        Me.txtSearch.TabIndex = 1
        Me.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblFilterStatus
        '
        Me.lblFilterStatus.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblFilterStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.lblFilterStatus.Location = New System.Drawing.Point(10, 102)
        Me.lblFilterStatus.Name = "lblFilterStatus"
        Me.lblFilterStatus.Size = New System.Drawing.Size(172, 18)
        Me.lblFilterStatus.TabIndex = 2
        Me.lblFilterStatus.Text = "الحالة"
        Me.lblFilterStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbFilterStatus
        '
        Me.cmbFilterStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFilterStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbFilterStatus.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.cmbFilterStatus.Location = New System.Drawing.Point(10, 122)
        Me.cmbFilterStatus.Name = "cmbFilterStatus"
        Me.cmbFilterStatus.Size = New System.Drawing.Size(172, 32)
        Me.cmbFilterStatus.TabIndex = 2
        '
        'lblFilterType
        '
        Me.lblFilterType.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblFilterType.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.lblFilterType.Location = New System.Drawing.Point(10, 160)
        Me.lblFilterType.Name = "lblFilterType"
        Me.lblFilterType.Size = New System.Drawing.Size(172, 18)
        Me.lblFilterType.TabIndex = 3
        Me.lblFilterType.Text = "النوع"
        Me.lblFilterType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbFilterType
        '
        Me.cmbFilterType.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.cmbFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFilterType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbFilterType.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.cmbFilterType.Location = New System.Drawing.Point(10, 180)
        Me.cmbFilterType.Name = "cmbFilterType"
        Me.cmbFilterType.Size = New System.Drawing.Size(172, 32)
        Me.cmbFilterType.TabIndex = 3
        '
        'pnlFilterDiv
        '
        Me.pnlFilterDiv.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.pnlFilterDiv.Location = New System.Drawing.Point(10, 220)
        Me.pnlFilterDiv.Name = "pnlFilterDiv"
        Me.pnlFilterDiv.Size = New System.Drawing.Size(188, 1)
        Me.pnlFilterDiv.TabIndex = 4
        '
        'lblSummaryHdr
        '
        Me.lblSummaryHdr.Font = New System.Drawing.Font("Cairo", 8.5!, System.Drawing.FontStyle.Bold)
        Me.lblSummaryHdr.ForeColor = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(181, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.lblSummaryHdr.Location = New System.Drawing.Point(10, 228)
        Me.lblSummaryHdr.Name = "lblSummaryHdr"
        Me.lblSummaryHdr.Size = New System.Drawing.Size(172, 18)
        Me.lblSummaryHdr.TabIndex = 5
        Me.lblSummaryHdr.Text = "ملخص"
        Me.lblSummaryHdr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalLbl
        '
        Me.lblTotalLbl.Font = New System.Drawing.Font("Cairo", 9.5!)
        Me.lblTotalLbl.ForeColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.lblTotalLbl.Location = New System.Drawing.Point(81, 258)
        Me.lblTotalLbl.Name = "lblTotalLbl"
        Me.lblTotalLbl.Size = New System.Drawing.Size(95, 30)
        Me.lblTotalLbl.TabIndex = 6
        Me.lblTotalLbl.Text = "إجمالي التنبيهات"
        Me.lblTotalLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTotalVal
        '
        Me.lblTotalVal.Font = New System.Drawing.Font("Cairo", 11.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalVal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.lblTotalVal.Location = New System.Drawing.Point(10, 258)
        Me.lblTotalVal.Name = "lblTotalVal"
        Me.lblTotalVal.Size = New System.Drawing.Size(65, 30)
        Me.lblTotalVal.TabIndex = 7
        Me.lblTotalVal.Text = "0"
        Me.lblTotalVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblActiveLbl
        '
        Me.lblActiveLbl.Font = New System.Drawing.Font("Cairo", 9.5!)
        Me.lblActiveLbl.ForeColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.lblActiveLbl.Location = New System.Drawing.Point(81, 304)
        Me.lblActiveLbl.Name = "lblActiveLbl"
        Me.lblActiveLbl.Size = New System.Drawing.Size(95, 30)
        Me.lblActiveLbl.TabIndex = 8
        Me.lblActiveLbl.Text = "مفعّل"
        Me.lblActiveLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblActiveVal
        '
        Me.lblActiveVal.Font = New System.Drawing.Font("Cairo", 11.0!, System.Drawing.FontStyle.Bold)
        Me.lblActiveVal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.lblActiveVal.Location = New System.Drawing.Point(10, 304)
        Me.lblActiveVal.Name = "lblActiveVal"
        Me.lblActiveVal.Size = New System.Drawing.Size(65, 30)
        Me.lblActiveVal.TabIndex = 9
        Me.lblActiveVal.Text = "0"
        Me.lblActiveVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblInactiveLbl
        '
        Me.lblInactiveLbl.Font = New System.Drawing.Font("Cairo", 9.5!)
        Me.lblInactiveLbl.ForeColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.lblInactiveLbl.Location = New System.Drawing.Point(81, 350)
        Me.lblInactiveLbl.Name = "lblInactiveLbl"
        Me.lblInactiveLbl.Size = New System.Drawing.Size(95, 30)
        Me.lblInactiveLbl.TabIndex = 10
        Me.lblInactiveLbl.Text = "موقوف"
        Me.lblInactiveLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblInactiveVal
        '
        Me.lblInactiveVal.Font = New System.Drawing.Font("Cairo", 11.0!, System.Drawing.FontStyle.Bold)
        Me.lblInactiveVal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(148, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.lblInactiveVal.Location = New System.Drawing.Point(10, 350)
        Me.lblInactiveVal.Name = "lblInactiveVal"
        Me.lblInactiveVal.Size = New System.Drawing.Size(65, 30)
        Me.lblInactiveVal.TabIndex = 11
        Me.lblInactiveVal.Text = "0"
        Me.lblInactiveVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlInput
        '
        Me.pnlInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlInput.BackColor = System.Drawing.Color.White
        Me.pnlInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlInput.Controls.Add(Me.pnlInputHdr)
        Me.pnlInput.Controls.Add(Me.Label1)
        Me.pnlInput.Controls.Add(Me.txtTitle)
        Me.pnlInput.Controls.Add(Me.Label2)
        Me.pnlInput.Controls.Add(Me.txtDescription)
        Me.pnlInput.Controls.Add(Me.pnlDiv1)
        Me.pnlInput.Controls.Add(Me.Label3)
        Me.pnlInput.Controls.Add(Me.cmbType)
        Me.pnlInput.Controls.Add(Me.Label9)
        Me.pnlInput.Controls.Add(Me.cmbDayOfWeek)
        Me.pnlInput.Controls.Add(Me.Label4)
        Me.pnlInput.Controls.Add(Me.numDayOfMonth)
        Me.pnlInput.Controls.Add(Me.Label7)
        Me.pnlInput.Controls.Add(Me.dtpDate)
        Me.pnlInput.Controls.Add(Me.Label8)
        Me.pnlInput.Controls.Add(Me.dtpTime)
        Me.pnlInput.Controls.Add(Me.pnlDiv2)
        Me.pnlInput.Controls.Add(Me.chkActive)
        Me.pnlInput.Controls.Add(Me.chkRepeat)
        Me.pnlInput.Controls.Add(Me.Label12)
        Me.pnlInput.Controls.Add(Me.numRepeatInterval)
        Me.pnlInput.Controls.Add(Me.btnAdd)
        Me.pnlInput.Controls.Add(Me.btnUpdate)
        Me.pnlInput.Controls.Add(Me.btnDelete)
        Me.pnlInput.Controls.Add(Me.btnRefresh)
        Me.pnlInput.Location = New System.Drawing.Point(201, 54)
        Me.pnlInput.Name = "pnlInput"
        Me.pnlInput.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.pnlInput.Size = New System.Drawing.Size(401, 517)
        Me.pnlInput.TabIndex = 2
        '
        'pnlInputHdr
        '
        Me.pnlInputHdr.BackColor = System.Drawing.Color.FromArgb(CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.pnlInputHdr.Controls.Add(Me.lblInputHdr)
        Me.pnlInputHdr.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlInputHdr.Location = New System.Drawing.Point(0, 0)
        Me.pnlInputHdr.Name = "pnlInputHdr"
        Me.pnlInputHdr.Size = New System.Drawing.Size(399, 34)
        Me.pnlInputHdr.TabIndex = 0
        '
        'lblInputHdr
        '
        Me.lblInputHdr.AutoSize = True
        Me.lblInputHdr.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblInputHdr.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.lblInputHdr.Location = New System.Drawing.Point(140, 4)
        Me.lblInputHdr.Name = "lblInputHdr"
        Me.lblInputHdr.Size = New System.Drawing.Size(120, 26)
        Me.lblInputHdr.TabIndex = 0
        Me.lblInputHdr.Text = "✏️  بيانات التنبيه"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(10, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(382, 18)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "عنوان التنبيه"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTitle
        '
        Me.txtTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTitle.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.txtTitle.Location = New System.Drawing.Point(10, 64)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(382, 32)
        Me.txtTitle.TabIndex = 1
        Me.txtTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(10, 102)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(382, 18)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "تفاصيل التنبيه"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDescription
        '
        Me.txtDescription.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDescription.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.txtDescription.Location = New System.Drawing.Point(10, 122)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(382, 62)
        Me.txtDescription.TabIndex = 2
        Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'pnlDiv1
        '
        Me.pnlDiv1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.pnlDiv1.Location = New System.Drawing.Point(10, 192)
        Me.pnlDiv1.Name = "pnlDiv1"
        Me.pnlDiv1.Size = New System.Drawing.Size(382, 1)
        Me.pnlDiv1.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(202, 200)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(190, 18)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "نوع التنبيه"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbType
        '
        Me.cmbType.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbType.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.cmbType.Location = New System.Drawing.Point(202, 220)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(190, 32)
        Me.cmbType.TabIndex = 4
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(10, 200)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(184, 18)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "يوم الأسبوع"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbDayOfWeek
        '
        Me.cmbDayOfWeek.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.cmbDayOfWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDayOfWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbDayOfWeek.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.cmbDayOfWeek.Location = New System.Drawing.Point(10, 220)
        Me.cmbDayOfWeek.Name = "cmbDayOfWeek"
        Me.cmbDayOfWeek.Size = New System.Drawing.Size(184, 32)
        Me.cmbDayOfWeek.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(10, 258)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(184, 18)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "يوم من الشهر"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'numDayOfMonth
        '
        Me.numDayOfMonth.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.numDayOfMonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numDayOfMonth.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.numDayOfMonth.Location = New System.Drawing.Point(10, 278)
        Me.numDayOfMonth.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me.numDayOfMonth.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numDayOfMonth.Name = "numDayOfMonth"
        Me.numDayOfMonth.Size = New System.Drawing.Size(184, 32)
        Me.numDayOfMonth.TabIndex = 6
        Me.numDayOfMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.numDayOfMonth.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(202, 258)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(190, 18)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "التاريخ"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpDate
        '
        Me.dtpDate.CalendarFont = New System.Drawing.Font("Cairo", 10.0!)
        Me.dtpDate.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDate.Location = New System.Drawing.Point(202, 278)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(190, 32)
        Me.dtpDate.TabIndex = 7
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(10, 316)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(382, 18)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "الوقت"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTime
        '
        Me.dtpTime.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpTime.Location = New System.Drawing.Point(10, 336)
        Me.dtpTime.Name = "dtpTime"
        Me.dtpTime.ShowUpDown = True
        Me.dtpTime.Size = New System.Drawing.Size(382, 32)
        Me.dtpTime.TabIndex = 8
        '
        'pnlDiv2
        '
        Me.pnlDiv2.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.pnlDiv2.Location = New System.Drawing.Point(10, 374)
        Me.pnlDiv2.Name = "pnlDiv2"
        Me.pnlDiv2.Size = New System.Drawing.Size(382, 1)
        Me.pnlDiv2.TabIndex = 9
        '
        'chkActive
        '
        Me.chkActive.BackColor = System.Drawing.Color.FromArgb(CType(CType(219, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.chkActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkActive.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.chkActive.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.chkActive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(187, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.chkActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkActive.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkActive.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.chkActive.Location = New System.Drawing.Point(202, 382)
        Me.chkActive.Name = "chkActive"
        Me.chkActive.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkActive.Size = New System.Drawing.Size(190, 30)
        Me.chkActive.TabIndex = 10
        Me.chkActive.Text = "تفعيل التنبيه"
        Me.chkActive.UseVisualStyleBackColor = False
        '
        'chkRepeat
        '
        Me.chkRepeat.BackColor = System.Drawing.Color.FromArgb(CType(CType(219, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.chkRepeat.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRepeat.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.chkRepeat.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.chkRepeat.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.chkRepeat.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkRepeat.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkRepeat.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.chkRepeat.Location = New System.Drawing.Point(202, 418)
        Me.chkRepeat.Name = "chkRepeat"
        Me.chkRepeat.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkRepeat.Size = New System.Drawing.Size(190, 30)
        Me.chkRepeat.TabIndex = 11
        Me.chkRepeat.Text = "تكرار التنبيه"
        Me.chkRepeat.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(10, 382)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(184, 18)
        Me.Label12.TabIndex = 12
        Me.Label12.Text = "التكرار كل (دقيقة)"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'numRepeatInterval
        '
        Me.numRepeatInterval.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.numRepeatInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numRepeatInterval.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.numRepeatInterval.Location = New System.Drawing.Point(10, 418)
        Me.numRepeatInterval.Maximum = New Decimal(New Integer() {1440, 0, 0, 0})
        Me.numRepeatInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numRepeatInterval.Name = "numRepeatInterval"
        Me.numRepeatInterval.Size = New System.Drawing.Size(184, 32)
        Me.numRepeatInterval.TabIndex = 12
        Me.numRepeatInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.numRepeatInterval.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAdd.FlatAppearance.BorderSize = 0
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnAdd.ForeColor = System.Drawing.Color.White
        Me.btnAdd.Location = New System.Drawing.Point(298, 468)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(90, 36)
        Me.btnAdd.TabIndex = 13
        Me.btnAdd.Text = "+  إضافة"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnUpdate
        '
        Me.btnUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUpdate.FlatAppearance.BorderSize = 0
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnUpdate.ForeColor = System.Drawing.Color.White
        Me.btnUpdate.Location = New System.Drawing.Point(202, 468)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(88, 36)
        Me.btnUpdate.TabIndex = 14
        Me.btnUpdate.Text = "✏  تعديل"
        Me.btnUpdate.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(68, Byte), Integer))
        Me.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDelete.FlatAppearance.BorderSize = 0
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnDelete.ForeColor = System.Drawing.Color.White
        Me.btnDelete.Location = New System.Drawing.Point(106, 468)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(88, 36)
        Me.btnDelete.TabIndex = 15
        Me.btnDelete.Text = "🗑  حذف"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(148, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRefresh.FlatAppearance.BorderSize = 0
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.Location = New System.Drawing.Point(10, 468)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(88, 36)
        Me.btnRefresh.TabIndex = 16
        Me.btnRefresh.Text = "↺  تحديث"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'pnlGrid
        '
        Me.pnlGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlGrid.BackColor = System.Drawing.Color.White
        Me.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlGrid.Controls.Add(Me.pnlGridHdr)
        Me.pnlGrid.Controls.Add(Me.dgvAlerts)
        Me.pnlGrid.Location = New System.Drawing.Point(608, 54)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.pnlGrid.Size = New System.Drawing.Size(827, 517)
        Me.pnlGrid.TabIndex = 3
        '
        'pnlGridHdr
        '
        Me.pnlGridHdr.BackColor = System.Drawing.Color.FromArgb(CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.pnlGridHdr.Controls.Add(Me.lblGridHdr)
        Me.pnlGridHdr.Controls.Add(Me.lblRecordCount)
        Me.pnlGridHdr.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlGridHdr.Location = New System.Drawing.Point(0, 0)
        Me.pnlGridHdr.Name = "pnlGridHdr"
        Me.pnlGridHdr.Size = New System.Drawing.Size(825, 34)
        Me.pnlGridHdr.TabIndex = 0
        '
        'lblGridHdr
        '
        Me.lblGridHdr.AutoSize = True
        Me.lblGridHdr.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblGridHdr.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.lblGridHdr.Location = New System.Drawing.Point(8, 4)
        Me.lblGridHdr.Name = "lblGridHdr"
        Me.lblGridHdr.Size = New System.Drawing.Size(139, 26)
        Me.lblGridHdr.TabIndex = 0
        Me.lblGridHdr.Text = "📋  قائمة التنبيهات"
        '
        'lblRecordCount
        '
        Me.lblRecordCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblRecordCount.Font = New System.Drawing.Font("Cairo", 8.5!, System.Drawing.FontStyle.Bold)
        Me.lblRecordCount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblRecordCount.Location = New System.Drawing.Point(685, 0)
        Me.lblRecordCount.Name = "lblRecordCount"
        Me.lblRecordCount.Padding = New System.Windows.Forms.Padding(4, 2, 4, 2)
        Me.lblRecordCount.Size = New System.Drawing.Size(140, 34)
        Me.lblRecordCount.TabIndex = 1
        Me.lblRecordCount.Text = "0 سجل"
        Me.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvAlerts
        '
        Me.dgvAlerts.AllowUserToAddRows = False
        Me.dgvAlerts.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(253, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(219, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(254, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.dgvAlerts.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvAlerts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvAlerts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvAlerts.BackgroundColor = System.Drawing.Color.White
        Me.dgvAlerts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvAlerts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvAlerts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(250, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Cairo", 10.0!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(250, Byte), Integer))
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAlerts.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvAlerts.ColumnHeadersHeight = 38
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Cairo", 10.0!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(58, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(122, Byte), Integer))
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(219, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(254, Byte), Integer))
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(95, Byte), Integer))
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAlerts.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvAlerts.EnableHeadersVisualStyles = False
        Me.dgvAlerts.GridColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.dgvAlerts.Location = New System.Drawing.Point(0, 34)
        Me.dgvAlerts.Name = "dgvAlerts"
        Me.dgvAlerts.ReadOnly = True
        Me.dgvAlerts.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Cairo", 10.0!)
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvAlerts.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvAlerts.RowHeadersVisible = False
        Me.dgvAlerts.RowTemplate.Height = 36
        Me.dgvAlerts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAlerts.Size = New System.Drawing.Size(825, 490)
        Me.dgvAlerts.TabIndex = 1
        '
        'FrmAlerts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1443, 583)
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.pnlInput)
        Me.Controls.Add(Me.pnlFilter)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.MinimumSize = New System.Drawing.Size(1210, 594)
        Me.Name = "FrmAlerts"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إدارة التنبيهات"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlFilter.ResumeLayout(False)
        Me.pnlFilter.PerformLayout()
        Me.pnlFilterHdr.ResumeLayout(False)
        Me.pnlFilterHdr.PerformLayout()
        Me.pnlInput.ResumeLayout(False)
        Me.pnlInput.PerformLayout()
        Me.pnlInputHdr.ResumeLayout(False)
        Me.pnlInputHdr.PerformLayout()
        CType(Me.numDayOfMonth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numRepeatInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGrid.ResumeLayout(False)
        Me.pnlGridHdr.ResumeLayout(False)
        Me.pnlGridHdr.PerformLayout()
        CType(Me.dgvAlerts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    ' =====================================================================
    '  Control Declarations
    ' =====================================================================

    '  Header
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblAppTitle As System.Windows.Forms.Label

    '  Filter panel
    Friend WithEvents pnlFilter As System.Windows.Forms.Panel
    Friend WithEvents pnlFilterHdr As System.Windows.Forms.Panel
    Friend WithEvents lblFilterHdr As System.Windows.Forms.Label
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents lblFilterStatus As System.Windows.Forms.Label
    Friend WithEvents cmbFilterStatus As System.Windows.Forms.ComboBox
    Friend WithEvents lblFilterType As System.Windows.Forms.Label
    Friend WithEvents cmbFilterType As System.Windows.Forms.ComboBox
    Friend WithEvents pnlFilterDiv As System.Windows.Forms.Panel
    Friend WithEvents lblSummaryHdr As System.Windows.Forms.Label
    Friend WithEvents lblTotalLbl As System.Windows.Forms.Label
    Friend WithEvents lblTotalVal As System.Windows.Forms.Label
    Friend WithEvents lblActiveLbl As System.Windows.Forms.Label
    Friend WithEvents lblActiveVal As System.Windows.Forms.Label
    Friend WithEvents lblInactiveLbl As System.Windows.Forms.Label
    Friend WithEvents lblInactiveVal As System.Windows.Forms.Label

    '  Input panel
    Friend WithEvents pnlInput As System.Windows.Forms.Panel
    Friend WithEvents pnlInputHdr As System.Windows.Forms.Panel
    Friend WithEvents lblInputHdr As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents pnlDiv1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbDayOfWeek As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents numDayOfMonth As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents dtpDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents dtpTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents pnlDiv2 As System.Windows.Forms.Panel
    Friend WithEvents chkActive As System.Windows.Forms.CheckBox
    Friend WithEvents chkRepeat As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents numRepeatInterval As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnRefresh As System.Windows.Forms.Button

    '  Grid panel
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents pnlGridHdr As System.Windows.Forms.Panel
    Friend WithEvents lblGridHdr As System.Windows.Forms.Label
    Friend WithEvents lblRecordCount As System.Windows.Forms.Label
    Friend WithEvents dgvAlerts As System.Windows.Forms.DataGridView

End Class


'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
'Partial Class FrmAlerts
'    Inherits System.Windows.Forms.Form

'    Private components As System.ComponentModel.IContainer

'    <System.Diagnostics.DebuggerStepThrough()>
'    Private Sub InitializeComponent()
'        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
'        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
'        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmAlerts))
'        Me.dgvAlerts = New System.Windows.Forms.DataGridView()
'        Me.txtTitle = New System.Windows.Forms.TextBox()
'        Me.txtDescription = New System.Windows.Forms.TextBox()
'        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
'        Me.dtpTime = New System.Windows.Forms.DateTimePicker()
'        Me.cmbType = New System.Windows.Forms.ComboBox()
'        Me.cmbDayOfWeek = New System.Windows.Forms.ComboBox()
'        Me.numDayOfMonth = New System.Windows.Forms.NumericUpDown()
'        Me.chkActive = New System.Windows.Forms.CheckBox()
'        Me.btnAdd = New System.Windows.Forms.Button()
'        Me.btnUpdate = New System.Windows.Forms.Button()
'        Me.btnDelete = New System.Windows.Forms.Button()
'        Me.btnRefresh = New System.Windows.Forms.Button()
'        Me.Label1 = New System.Windows.Forms.Label()
'        Me.Label2 = New System.Windows.Forms.Label()
'        Me.Label3 = New System.Windows.Forms.Label()
'        Me.Label4 = New System.Windows.Forms.Label()
'        Me.Label7 = New System.Windows.Forms.Label()
'        Me.Label8 = New System.Windows.Forms.Label()
'        Me.Label9 = New System.Windows.Forms.Label()
'        Me.Label5 = New System.Windows.Forms.Label()
'        Me.Panel1 = New System.Windows.Forms.Panel()
'        Me.cmbFilterType = New System.Windows.Forms.ComboBox()
'        Me.cmbFilterStatus = New System.Windows.Forms.ComboBox()
'        Me.txtSearch = New System.Windows.Forms.TextBox()
'        Me.panelInput = New System.Windows.Forms.Panel()
'        Me.Label12 = New System.Windows.Forms.Label()
'        Me.numRepeatInterval = New System.Windows.Forms.NumericUpDown()
'        Me.chkRepeat = New System.Windows.Forms.CheckBox()
'        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
'        Me.Label11 = New System.Windows.Forms.Label()
'        Me.Label10 = New System.Windows.Forms.Label()
'        Me.Label6 = New System.Windows.Forms.Label()
'        CType(Me.dgvAlerts, System.ComponentModel.ISupportInitialize).BeginInit()
'        CType(Me.numDayOfMonth, System.ComponentModel.ISupportInitialize).BeginInit()
'        Me.Panel1.SuspendLayout()
'        Me.panelInput.SuspendLayout()
'        CType(Me.numRepeatInterval, System.ComponentModel.ISupportInitialize).BeginInit()
'        Me.GroupBox1.SuspendLayout()
'        Me.SuspendLayout()
'        '
'        'dgvAlerts
'        '
'        Me.dgvAlerts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
'        Me.dgvAlerts.BackgroundColor = System.Drawing.Color.White
'        Me.dgvAlerts.BorderStyle = System.Windows.Forms.BorderStyle.None
'        Me.dgvAlerts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
'        Me.dgvAlerts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
'        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
'        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(250, Byte), Integer))
'        DataGridViewCellStyle3.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
'        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(90, Byte), Integer))
'        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
'        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
'        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
'        Me.dgvAlerts.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
'        Me.dgvAlerts.EnableHeadersVisualStyles = False
'        Me.dgvAlerts.Location = New System.Drawing.Point(677, 66)
'        Me.dgvAlerts.Name = "dgvAlerts"
'        Me.dgvAlerts.ReadOnly = True
'        Me.dgvAlerts.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.dgvAlerts.RowHeadersVisible = False
'        DataGridViewCellStyle4.BackColor = System.Drawing.Color.White
'        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(248, Byte), Integer))
'        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
'        Me.dgvAlerts.RowsDefaultCellStyle = DataGridViewCellStyle4
'        Me.dgvAlerts.Size = New System.Drawing.Size(738, 490)
'        Me.dgvAlerts.TabIndex = 2
'        '
'        'txtTitle
'        '
'        Me.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
'        Me.txtTitle.Location = New System.Drawing.Point(20, 40)
'        Me.txtTitle.Name = "txtTitle"
'        Me.txtTitle.Size = New System.Drawing.Size(380, 32)
'        Me.txtTitle.TabIndex = 1
'        Me.txtTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        '
'        'txtDescription
'        '
'        Me.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
'        Me.txtDescription.Location = New System.Drawing.Point(20, 105)
'        Me.txtDescription.Multiline = True
'        Me.txtDescription.Name = "txtDescription"
'        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
'        Me.txtDescription.Size = New System.Drawing.Size(380, 70)
'        Me.txtDescription.TabIndex = 3
'        Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        '
'        'dtpDate
'        '
'        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
'        Me.dtpDate.Location = New System.Drawing.Point(220, 335)
'        Me.dtpDate.Name = "dtpDate"
'        Me.dtpDate.Size = New System.Drawing.Size(180, 32)
'        Me.dtpDate.TabIndex = 11
'        '
'        'dtpTime
'        '
'        Me.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
'        Me.dtpTime.Location = New System.Drawing.Point(20, 335)
'        Me.dtpTime.Name = "dtpTime"
'        Me.dtpTime.ShowUpDown = True
'        Me.dtpTime.Size = New System.Drawing.Size(180, 32)
'        Me.dtpTime.TabIndex = 13
'        '
'        'cmbType
'        '
'        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
'        Me.cmbType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.cmbType.Location = New System.Drawing.Point(220, 210)
'        Me.cmbType.Name = "cmbType"
'        Me.cmbType.Size = New System.Drawing.Size(180, 32)
'        Me.cmbType.TabIndex = 5
'        '
'        'cmbDayOfWeek
'        '
'        Me.cmbDayOfWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
'        Me.cmbDayOfWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.cmbDayOfWeek.Location = New System.Drawing.Point(20, 210)
'        Me.cmbDayOfWeek.Name = "cmbDayOfWeek"
'        Me.cmbDayOfWeek.Size = New System.Drawing.Size(180, 32)
'        Me.cmbDayOfWeek.TabIndex = 7
'        '
'        'numDayOfMonth
'        '
'        Me.numDayOfMonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
'        Me.numDayOfMonth.Location = New System.Drawing.Point(220, 275)
'        Me.numDayOfMonth.Name = "numDayOfMonth"
'        Me.numDayOfMonth.Size = New System.Drawing.Size(180, 32)
'        Me.numDayOfMonth.TabIndex = 9
'        Me.numDayOfMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        '
'        'chkActive
'        '
'        Me.chkActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
'        Me.chkActive.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.chkActive.Location = New System.Drawing.Point(220, 373)
'        Me.chkActive.Name = "chkActive"
'        Me.chkActive.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.chkActive.Size = New System.Drawing.Size(180, 31)
'        Me.chkActive.TabIndex = 15
'        Me.chkActive.Text = "تفعيل التنبيه"
'        '
'        'btnAdd
'        '
'        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(76, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(80, Byte), Integer))
'        Me.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.btnAdd.FlatAppearance.BorderSize = 0
'        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnAdd.ForeColor = System.Drawing.Color.White
'        Me.btnAdd.Location = New System.Drawing.Point(323, 453)
'        Me.btnAdd.Name = "btnAdd"
'        Me.btnAdd.Size = New System.Drawing.Size(75, 37)
'        Me.btnAdd.TabIndex = 16
'        Me.btnAdd.Text = "إضافة"
'        Me.btnAdd.UseVisualStyleBackColor = False
'        '
'        'btnUpdate
'        '
'        Me.btnUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(243, Byte), Integer))
'        Me.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.btnUpdate.FlatAppearance.BorderSize = 0
'        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnUpdate.ForeColor = System.Drawing.Color.White
'        Me.btnUpdate.Location = New System.Drawing.Point(223, 453)
'        Me.btnUpdate.Name = "btnUpdate"
'        Me.btnUpdate.Size = New System.Drawing.Size(75, 37)
'        Me.btnUpdate.TabIndex = 17
'        Me.btnUpdate.Text = "تعديل"
'        Me.btnUpdate.UseVisualStyleBackColor = False
'        '
'        'btnDelete
'        '
'        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(67, Byte), Integer), CType(CType(54, Byte), Integer))
'        Me.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.btnDelete.FlatAppearance.BorderSize = 0
'        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnDelete.ForeColor = System.Drawing.Color.White
'        Me.btnDelete.Location = New System.Drawing.Point(123, 453)
'        Me.btnDelete.Name = "btnDelete"
'        Me.btnDelete.Size = New System.Drawing.Size(75, 37)
'        Me.btnDelete.TabIndex = 18
'        Me.btnDelete.Text = "حذف"
'        Me.btnDelete.UseVisualStyleBackColor = False
'        '
'        'btnRefresh
'        '
'        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(158, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(158, Byte), Integer))
'        Me.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.btnRefresh.FlatAppearance.BorderSize = 0
'        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnRefresh.ForeColor = System.Drawing.Color.White
'        Me.btnRefresh.Location = New System.Drawing.Point(23, 453)
'        Me.btnRefresh.Name = "btnRefresh"
'        Me.btnRefresh.Size = New System.Drawing.Size(75, 37)
'        Me.btnRefresh.TabIndex = 19
'        Me.btnRefresh.Text = "تحديث"
'        Me.btnRefresh.UseVisualStyleBackColor = False
'        '
'        'Label1
'        '
'        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label1.Location = New System.Drawing.Point(300, 15)
'        Me.Label1.Name = "Label1"
'        Me.Label1.Size = New System.Drawing.Size(100, 23)
'        Me.Label1.TabIndex = 0
'        Me.Label1.Text = "عنوان التنبيه"
'        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label2
'        '
'        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label2.Location = New System.Drawing.Point(241, 80)
'        Me.Label2.Name = "Label2"
'        Me.Label2.Size = New System.Drawing.Size(159, 23)
'        Me.Label2.TabIndex = 2
'        Me.Label2.Text = "تفاصيل التنبيه"
'        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label3
'        '
'        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label3.Location = New System.Drawing.Point(300, 185)
'        Me.Label3.Name = "Label3"
'        Me.Label3.Size = New System.Drawing.Size(100, 23)
'        Me.Label3.TabIndex = 4
'        Me.Label3.Text = "نوع التنبيه"
'        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label4
'        '
'        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label4.Location = New System.Drawing.Point(300, 250)
'        Me.Label4.Name = "Label4"
'        Me.Label4.Size = New System.Drawing.Size(100, 23)
'        Me.Label4.TabIndex = 8
'        Me.Label4.Text = "يوم من الشهر"
'        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label7
'        '
'        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label7.Location = New System.Drawing.Point(337, 310)
'        Me.Label7.Name = "Label7"
'        Me.Label7.Size = New System.Drawing.Size(63, 23)
'        Me.Label7.TabIndex = 10
'        Me.Label7.Text = "التاريخ"
'        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label8
'        '
'        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label8.Location = New System.Drawing.Point(130, 309)
'        Me.Label8.Name = "Label8"
'        Me.Label8.Size = New System.Drawing.Size(68, 23)
'        Me.Label8.TabIndex = 12
'        Me.Label8.Text = "الوقت"
'        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label9
'        '
'        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label9.Location = New System.Drawing.Point(100, 185)
'        Me.Label9.Name = "Label9"
'        Me.Label9.Size = New System.Drawing.Size(100, 23)
'        Me.Label9.TabIndex = 6
'        Me.Label9.Text = "يوم الأسبوع"
'        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label5
'        '
'        Me.Label5.AutoSize = True
'        Me.Label5.Font = New System.Drawing.Font("Cairo", 14.0!, System.Drawing.FontStyle.Bold)
'        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(90, Byte), Integer))
'        Me.Label5.Location = New System.Drawing.Point(640, 8)
'        Me.Label5.Name = "Label5"
'        Me.Label5.Size = New System.Drawing.Size(135, 36)
'        Me.Label5.TabIndex = 0
'        Me.Label5.Text = "إدارة التنبيهات"
'        '
'        'Panel1
'        '
'        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(248, Byte), Integer))
'        Me.Panel1.Controls.Add(Me.Label5)
'        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
'        Me.Panel1.Location = New System.Drawing.Point(0, 0)
'        Me.Panel1.Name = "Panel1"
'        Me.Panel1.Size = New System.Drawing.Size(1427, 50)
'        Me.Panel1.TabIndex = 0
'        '
'        'cmbFilterType
'        '
'        Me.cmbFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
'        Me.cmbFilterType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.cmbFilterType.Location = New System.Drawing.Point(28, 184)
'        Me.cmbFilterType.Name = "cmbFilterType"
'        Me.cmbFilterType.Size = New System.Drawing.Size(180, 32)
'        Me.cmbFilterType.TabIndex = 7
'        '
'        'cmbFilterStatus
'        '
'        Me.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
'        Me.cmbFilterStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.cmbFilterStatus.Location = New System.Drawing.Point(28, 119)
'        Me.cmbFilterStatus.Name = "cmbFilterStatus"
'        Me.cmbFilterStatus.Size = New System.Drawing.Size(180, 32)
'        Me.cmbFilterStatus.TabIndex = 6
'        '
'        'txtSearch
'        '
'        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
'        Me.txtSearch.Location = New System.Drawing.Point(28, 54)
'        Me.txtSearch.Name = "txtSearch"
'        Me.txtSearch.Size = New System.Drawing.Size(180, 32)
'        Me.txtSearch.TabIndex = 2
'        Me.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        '
'        'panelInput
'        '
'        Me.panelInput.BackColor = System.Drawing.Color.White
'        Me.panelInput.Controls.Add(Me.Label12)
'        Me.panelInput.Controls.Add(Me.numRepeatInterval)
'        Me.panelInput.Controls.Add(Me.chkRepeat)
'        Me.panelInput.Controls.Add(Me.Label1)
'        Me.panelInput.Controls.Add(Me.txtTitle)
'        Me.panelInput.Controls.Add(Me.Label2)
'        Me.panelInput.Controls.Add(Me.txtDescription)
'        Me.panelInput.Controls.Add(Me.Label3)
'        Me.panelInput.Controls.Add(Me.cmbType)
'        Me.panelInput.Controls.Add(Me.Label9)
'        Me.panelInput.Controls.Add(Me.cmbDayOfWeek)
'        Me.panelInput.Controls.Add(Me.Label4)
'        Me.panelInput.Controls.Add(Me.numDayOfMonth)
'        Me.panelInput.Controls.Add(Me.Label7)
'        Me.panelInput.Controls.Add(Me.dtpDate)
'        Me.panelInput.Controls.Add(Me.Label8)
'        Me.panelInput.Controls.Add(Me.dtpTime)
'        Me.panelInput.Controls.Add(Me.chkActive)
'        Me.panelInput.Controls.Add(Me.btnAdd)
'        Me.panelInput.Controls.Add(Me.btnUpdate)
'        Me.panelInput.Controls.Add(Me.btnDelete)
'        Me.panelInput.Controls.Add(Me.btnRefresh)
'        Me.panelInput.Location = New System.Drawing.Point(243, 66)
'        Me.panelInput.Name = "panelInput"
'        Me.panelInput.Size = New System.Drawing.Size(428, 500)
'        Me.panelInput.TabIndex = 1
'        '
'        'Label12
'        '
'        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label12.Location = New System.Drawing.Point(23, 373)
'        Me.Label12.Name = "Label12"
'        Me.Label12.Size = New System.Drawing.Size(175, 31)
'        Me.Label12.TabIndex = 22
'        Me.Label12.Text = "التكرار كل (دقيقة)"
'        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'numRepeatInterval
'        '
'        Me.numRepeatInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
'        Me.numRepeatInterval.Location = New System.Drawing.Point(20, 406)
'        Me.numRepeatInterval.Maximum = New Decimal(New Integer() {1440, 0, 0, 0})
'        Me.numRepeatInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
'        Me.numRepeatInterval.Name = "numRepeatInterval"
'        Me.numRepeatInterval.Size = New System.Drawing.Size(180, 32)
'        Me.numRepeatInterval.TabIndex = 21
'        Me.numRepeatInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        Me.numRepeatInterval.Value = New Decimal(New Integer() {2, 0, 0, 0})
'        '
'        'chkRepeat
'        '
'        Me.chkRepeat.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
'        Me.chkRepeat.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.chkRepeat.Location = New System.Drawing.Point(220, 407)
'        Me.chkRepeat.Name = "chkRepeat"
'        Me.chkRepeat.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.chkRepeat.Size = New System.Drawing.Size(180, 31)
'        Me.chkRepeat.TabIndex = 20
'        Me.chkRepeat.Text = "تكرار التنبيه"
'        '
'        'GroupBox1
'        '
'        Me.GroupBox1.BackColor = System.Drawing.Color.White
'        Me.GroupBox1.Controls.Add(Me.Label11)
'        Me.GroupBox1.Controls.Add(Me.Label10)
'        Me.GroupBox1.Controls.Add(Me.Label6)
'        Me.GroupBox1.Controls.Add(Me.cmbFilterType)
'        Me.GroupBox1.Controls.Add(Me.txtSearch)
'        Me.GroupBox1.Controls.Add(Me.cmbFilterStatus)
'        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.GroupBox1.Location = New System.Drawing.Point(0, 66)
'        Me.GroupBox1.Name = "GroupBox1"
'        Me.GroupBox1.Size = New System.Drawing.Size(237, 242)
'        Me.GroupBox1.TabIndex = 4
'        Me.GroupBox1.TabStop = False
'        Me.GroupBox1.Text = "البحث والفلترة 🔍"
'        '
'        'Label11
'        '
'        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label11.Location = New System.Drawing.Point(108, 156)
'        Me.Label11.Name = "Label11"
'        Me.Label11.Size = New System.Drawing.Size(100, 23)
'        Me.Label11.TabIndex = 10
'        Me.Label11.Text = "النوع"
'        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label10
'        '
'        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label10.Location = New System.Drawing.Point(108, 91)
'        Me.Label10.Name = "Label10"
'        Me.Label10.Size = New System.Drawing.Size(100, 23)
'        Me.Label10.TabIndex = 9
'        Me.Label10.Text = "الحالة"
'        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label6
'        '
'        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.Label6.Location = New System.Drawing.Point(108, 26)
'        Me.Label6.Name = "Label6"
'        Me.Label6.Size = New System.Drawing.Size(100, 23)
'        Me.Label6.TabIndex = 8
'        Me.Label6.Text = "البحث"
'        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'FrmAlerts
'        '
'        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(250, Byte), Integer))
'        Me.ClientSize = New System.Drawing.Size(1427, 568)
'        Me.Controls.Add(Me.GroupBox1)
'        Me.Controls.Add(Me.Panel1)
'        Me.Controls.Add(Me.panelInput)
'        Me.Controls.Add(Me.dgvAlerts)
'        Me.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
'        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
'        Me.Name = "FrmAlerts"
'        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.RightToLeftLayout = True
'        CType(Me.dgvAlerts, System.ComponentModel.ISupportInitialize).EndInit()
'        CType(Me.numDayOfMonth, System.ComponentModel.ISupportInitialize).EndInit()
'        Me.Panel1.ResumeLayout(False)
'        Me.Panel1.PerformLayout()
'        Me.panelInput.ResumeLayout(False)
'        Me.panelInput.PerformLayout()
'        CType(Me.numRepeatInterval, System.ComponentModel.ISupportInitialize).EndInit()
'        Me.GroupBox1.ResumeLayout(False)
'        Me.GroupBox1.PerformLayout()
'        Me.ResumeLayout(False)

'    End Sub

'    Friend WithEvents dgvAlerts As DataGridView
'    Friend WithEvents txtTitle As TextBox
'    Friend WithEvents txtDescription As TextBox
'    Friend WithEvents dtpDate As DateTimePicker
'    Friend WithEvents cmbType As ComboBox
'    Friend WithEvents cmbDayOfWeek As ComboBox
'    Friend WithEvents numDayOfMonth As NumericUpDown
'    Friend WithEvents chkActive As CheckBox
'    Friend WithEvents btnAdd As Button
'    Friend WithEvents btnUpdate As Button
'    Friend WithEvents btnDelete As Button
'    Friend WithEvents btnRefresh As Button
'    Friend WithEvents Label1 As Label
'    Friend WithEvents Label2 As Label
'    Friend WithEvents Label3 As Label
'    Friend WithEvents Label4 As Label
'    Friend WithEvents Label7 As Label
'    Friend WithEvents Label8 As Label
'    Friend WithEvents Label9 As Label
'    Friend WithEvents Panel1 As Panel
'    Friend WithEvents Label5 As Label
'    Friend WithEvents dtpTime As DateTimePicker
'    Friend WithEvents panelInput As Panel
'    Friend WithEvents cmbFilterType As ComboBox
'    Friend WithEvents cmbFilterStatus As ComboBox
'    Friend WithEvents txtSearch As TextBox
'    Friend WithEvents GroupBox1 As GroupBox
'    Friend WithEvents Label11 As Label
'    Friend WithEvents Label10 As Label
'    Friend WithEvents Label6 As Label
'    Friend WithEvents chkRepeat As CheckBox
'    Friend WithEvents Label12 As Label
'    Friend WithEvents numRepeatInterval As NumericUpDown
'End Class


'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
'Partial Class FrmAlerts
'    Inherits System.Windows.Forms.Form

'    Private components As System.ComponentModel.IContainer

'    <System.Diagnostics.DebuggerStepThrough()>
'    Private Sub InitializeComponent()
'        Me.dgvAlerts = New System.Windows.Forms.DataGridView()
'        Me.txtTitle = New System.Windows.Forms.TextBox()
'        Me.txtDescription = New System.Windows.Forms.TextBox()
'        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
'        Me.dtpTime = New System.Windows.Forms.DateTimePicker()
'        Me.cmbType = New System.Windows.Forms.ComboBox()
'        Me.cmbDayOfWeek = New System.Windows.Forms.ComboBox()
'        Me.numDayOfMonth = New System.Windows.Forms.NumericUpDown()
'        Me.chkActive = New System.Windows.Forms.CheckBox()
'        Me.btnAdd = New System.Windows.Forms.Button()
'        Me.btnUpdate = New System.Windows.Forms.Button()
'        Me.btnDelete = New System.Windows.Forms.Button()
'        Me.btnRefresh = New System.Windows.Forms.Button()
'        Me.Label1 = New System.Windows.Forms.Label()
'        Me.Label2 = New System.Windows.Forms.Label()
'        Me.Label3 = New System.Windows.Forms.Label()
'        Me.Label4 = New System.Windows.Forms.Label()
'        Me.Label7 = New System.Windows.Forms.Label()
'        Me.Label8 = New System.Windows.Forms.Label()
'        Me.Label9 = New System.Windows.Forms.Label()
'        Me.Label5 = New System.Windows.Forms.Label()
'        Me.Panel1 = New System.Windows.Forms.Panel()
'        Me.cmbFilterType = New System.Windows.Forms.ComboBox()
'        Me.cmbFilterStatus = New System.Windows.Forms.ComboBox()
'        Me.txtSearch = New System.Windows.Forms.TextBox()
'        Me.panelInput = New System.Windows.Forms.Panel()
'        Me.Label12 = New System.Windows.Forms.Label()
'        Me.numRepeatInterval = New System.Windows.Forms.NumericUpDown()
'        Me.chkRepeat = New System.Windows.Forms.CheckBox()
'        Me.lblActive = New System.Windows.Forms.Label()
'        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
'        Me.Label11 = New System.Windows.Forms.Label()
'        Me.Label10 = New System.Windows.Forms.Label()
'        Me.Label6 = New System.Windows.Forms.Label()
'        CType(Me.dgvAlerts, System.ComponentModel.ISupportInitialize).BeginInit()
'        CType(Me.numDayOfMonth, System.ComponentModel.ISupportInitialize).BeginInit()
'        Me.Panel1.SuspendLayout()
'        Me.panelInput.SuspendLayout()
'        CType(Me.numRepeatInterval, System.ComponentModel.ISupportInitialize).BeginInit()
'        Me.GroupBox1.SuspendLayout()
'        Me.SuspendLayout()
'        '
'        'dgvAlerts
'        '
'        Me.dgvAlerts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
'            Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.dgvAlerts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
'        Me.dgvAlerts.BackgroundColor = System.Drawing.Color.White
'        Me.dgvAlerts.Location = New System.Drawing.Point(677, 56)
'        Me.dgvAlerts.Name = "dgvAlerts"
'        Me.dgvAlerts.ReadOnly = True
'        Me.dgvAlerts.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.dgvAlerts.Size = New System.Drawing.Size(750, 500)
'        Me.dgvAlerts.TabIndex = 2
'        '
'        'txtTitle
'        '
'        Me.txtTitle.Location = New System.Drawing.Point(20, 40)
'        Me.txtTitle.Name = "txtTitle"
'        Me.txtTitle.Size = New System.Drawing.Size(380, 32)
'        Me.txtTitle.TabIndex = 1
'        Me.txtTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        '
'        'txtDescription
'        '
'        Me.txtDescription.Location = New System.Drawing.Point(20, 105)
'        Me.txtDescription.Multiline = True
'        Me.txtDescription.Name = "txtDescription"
'        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
'        Me.txtDescription.Size = New System.Drawing.Size(380, 70)
'        Me.txtDescription.TabIndex = 3
'        Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        '
'        'dtpDate
'        '
'        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
'        Me.dtpDate.Location = New System.Drawing.Point(220, 335)
'        Me.dtpDate.Name = "dtpDate"
'        Me.dtpDate.Size = New System.Drawing.Size(180, 32)
'        Me.dtpDate.TabIndex = 11
'        '
'        'dtpTime
'        '
'        Me.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
'        Me.dtpTime.Location = New System.Drawing.Point(20, 335)
'        Me.dtpTime.Name = "dtpTime"
'        Me.dtpTime.ShowUpDown = True
'        Me.dtpTime.Size = New System.Drawing.Size(180, 32)
'        Me.dtpTime.TabIndex = 13
'        '
'        'cmbType
'        '
'        Me.cmbType.Location = New System.Drawing.Point(220, 210)
'        Me.cmbType.Name = "cmbType"
'        Me.cmbType.Size = New System.Drawing.Size(180, 32)
'        Me.cmbType.TabIndex = 5
'        '
'        'cmbDayOfWeek
'        '
'        Me.cmbDayOfWeek.Location = New System.Drawing.Point(20, 210)
'        Me.cmbDayOfWeek.Name = "cmbDayOfWeek"
'        Me.cmbDayOfWeek.Size = New System.Drawing.Size(180, 32)
'        Me.cmbDayOfWeek.TabIndex = 7
'        '
'        'numDayOfMonth
'        '
'        Me.numDayOfMonth.Location = New System.Drawing.Point(220, 275)
'        Me.numDayOfMonth.Name = "numDayOfMonth"
'        Me.numDayOfMonth.Size = New System.Drawing.Size(180, 32)
'        Me.numDayOfMonth.TabIndex = 9
'        Me.numDayOfMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        '
'        'chkActive
'        '
'        Me.chkActive.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
'        Me.chkActive.Location = New System.Drawing.Point(220, 373)
'        Me.chkActive.Name = "chkActive"
'        Me.chkActive.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.chkActive.Size = New System.Drawing.Size(31, 31)
'        Me.chkActive.TabIndex = 15
'        '
'        'btnAdd
'        '
'        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
'        Me.btnAdd.ForeColor = System.Drawing.Color.White
'        Me.btnAdd.Location = New System.Drawing.Point(323, 453)
'        Me.btnAdd.Name = "btnAdd"
'        Me.btnAdd.Size = New System.Drawing.Size(75, 37)
'        Me.btnAdd.TabIndex = 16
'        Me.btnAdd.Text = "إضافة"
'        Me.btnAdd.UseVisualStyleBackColor = False
'        '
'        'btnUpdate
'        '
'        Me.btnUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
'        Me.btnUpdate.ForeColor = System.Drawing.Color.White
'        Me.btnUpdate.Location = New System.Drawing.Point(223, 453)
'        Me.btnUpdate.Name = "btnUpdate"
'        Me.btnUpdate.Size = New System.Drawing.Size(75, 37)
'        Me.btnUpdate.TabIndex = 17
'        Me.btnUpdate.Text = "تعديل"
'        Me.btnUpdate.UseVisualStyleBackColor = False
'        '
'        'btnDelete
'        '
'        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
'        Me.btnDelete.ForeColor = System.Drawing.Color.White
'        Me.btnDelete.Location = New System.Drawing.Point(123, 453)
'        Me.btnDelete.Name = "btnDelete"
'        Me.btnDelete.Size = New System.Drawing.Size(75, 37)
'        Me.btnDelete.TabIndex = 18
'        Me.btnDelete.Text = "حذف"
'        Me.btnDelete.UseVisualStyleBackColor = False
'        '
'        'btnRefresh
'        '
'        Me.btnRefresh.BackColor = System.Drawing.Color.Gray
'        Me.btnRefresh.ForeColor = System.Drawing.Color.White
'        Me.btnRefresh.Location = New System.Drawing.Point(23, 453)
'        Me.btnRefresh.Name = "btnRefresh"
'        Me.btnRefresh.Size = New System.Drawing.Size(75, 37)
'        Me.btnRefresh.TabIndex = 19
'        Me.btnRefresh.Text = "تحديث"
'        Me.btnRefresh.UseVisualStyleBackColor = False
'        '
'        'Label1
'        '
'        Me.Label1.Location = New System.Drawing.Point(300, 15)
'        Me.Label1.Name = "Label1"
'        Me.Label1.Size = New System.Drawing.Size(100, 23)
'        Me.Label1.TabIndex = 0
'        Me.Label1.Text = "عنوان التنبيه"
'        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label2
'        '
'        Me.Label2.Location = New System.Drawing.Point(241, 80)
'        Me.Label2.Name = "Label2"
'        Me.Label2.Size = New System.Drawing.Size(159, 23)
'        Me.Label2.TabIndex = 2
'        Me.Label2.Text = "تفاصيل التنبيه"
'        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label3
'        '
'        Me.Label3.Location = New System.Drawing.Point(300, 185)
'        Me.Label3.Name = "Label3"
'        Me.Label3.Size = New System.Drawing.Size(100, 23)
'        Me.Label3.TabIndex = 4
'        Me.Label3.Text = "نوع التنبيه"
'        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label4
'        '
'        Me.Label4.Location = New System.Drawing.Point(300, 250)
'        Me.Label4.Name = "Label4"
'        Me.Label4.Size = New System.Drawing.Size(100, 23)
'        Me.Label4.TabIndex = 8
'        Me.Label4.Text = "يوم من الشهر"
'        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label7
'        '
'        Me.Label7.Location = New System.Drawing.Point(337, 310)
'        Me.Label7.Name = "Label7"
'        Me.Label7.Size = New System.Drawing.Size(63, 23)
'        Me.Label7.TabIndex = 10
'        Me.Label7.Text = "التاريخ"
'        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label8
'        '
'        Me.Label8.Location = New System.Drawing.Point(130, 309)
'        Me.Label8.Name = "Label8"
'        Me.Label8.Size = New System.Drawing.Size(68, 23)
'        Me.Label8.TabIndex = 12
'        Me.Label8.Text = "الوقت"
'        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label9
'        '
'        Me.Label9.Location = New System.Drawing.Point(100, 185)
'        Me.Label9.Name = "Label9"
'        Me.Label9.Size = New System.Drawing.Size(100, 23)
'        Me.Label9.TabIndex = 6
'        Me.Label9.Text = "يوم الأسبوع"
'        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label5
'        '
'        Me.Label5.AutoSize = True
'        Me.Label5.Font = New System.Drawing.Font("Cairo", 13.0!, System.Drawing.FontStyle.Bold)
'        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
'        Me.Label5.Location = New System.Drawing.Point(520, 10)
'        Me.Label5.Name = "Label5"
'        Me.Label5.Size = New System.Drawing.Size(130, 34)
'        Me.Label5.TabIndex = 0
'        Me.Label5.Text = "إدارة التنبيهات"
'        '
'        'Panel1
'        '
'        Me.Panel1.BackColor = System.Drawing.Color.White
'        Me.Panel1.Controls.Add(Me.Label5)
'        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
'        Me.Panel1.Location = New System.Drawing.Point(0, 0)
'        Me.Panel1.Name = "Panel1"
'        Me.Panel1.Size = New System.Drawing.Size(1427, 50)
'        Me.Panel1.TabIndex = 0
'        '
'        'cmbFilterType
'        '
'        Me.cmbFilterType.Location = New System.Drawing.Point(28, 184)
'        Me.cmbFilterType.Name = "cmbFilterType"
'        Me.cmbFilterType.Size = New System.Drawing.Size(180, 32)
'        Me.cmbFilterType.TabIndex = 7
'        '
'        'cmbFilterStatus
'        '
'        Me.cmbFilterStatus.Location = New System.Drawing.Point(28, 119)
'        Me.cmbFilterStatus.Name = "cmbFilterStatus"
'        Me.cmbFilterStatus.Size = New System.Drawing.Size(180, 32)
'        Me.cmbFilterStatus.TabIndex = 6
'        '
'        'txtSearch
'        '
'        Me.txtSearch.Location = New System.Drawing.Point(28, 54)
'        Me.txtSearch.Name = "txtSearch"
'        Me.txtSearch.Size = New System.Drawing.Size(180, 32)
'        Me.txtSearch.TabIndex = 2
'        Me.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        '
'        'panelInput
'        '
'        Me.panelInput.BackColor = System.Drawing.Color.White
'        Me.panelInput.Controls.Add(Me.Label12)
'        Me.panelInput.Controls.Add(Me.numRepeatInterval)
'        Me.panelInput.Controls.Add(Me.chkRepeat)
'        Me.panelInput.Controls.Add(Me.Label1)
'        Me.panelInput.Controls.Add(Me.txtTitle)
'        Me.panelInput.Controls.Add(Me.Label2)
'        Me.panelInput.Controls.Add(Me.txtDescription)
'        Me.panelInput.Controls.Add(Me.Label3)
'        Me.panelInput.Controls.Add(Me.cmbType)
'        Me.panelInput.Controls.Add(Me.Label9)
'        Me.panelInput.Controls.Add(Me.cmbDayOfWeek)
'        Me.panelInput.Controls.Add(Me.Label4)
'        Me.panelInput.Controls.Add(Me.numDayOfMonth)
'        Me.panelInput.Controls.Add(Me.Label7)
'        Me.panelInput.Controls.Add(Me.dtpDate)
'        Me.panelInput.Controls.Add(Me.Label8)
'        Me.panelInput.Controls.Add(Me.dtpTime)
'        Me.panelInput.Controls.Add(Me.lblActive)
'        Me.panelInput.Controls.Add(Me.chkActive)
'        Me.panelInput.Controls.Add(Me.btnAdd)
'        Me.panelInput.Controls.Add(Me.btnUpdate)
'        Me.panelInput.Controls.Add(Me.btnDelete)
'        Me.panelInput.Controls.Add(Me.btnRefresh)
'        Me.panelInput.Location = New System.Drawing.Point(243, 56)
'        Me.panelInput.Name = "panelInput"
'        Me.panelInput.Size = New System.Drawing.Size(428, 500)
'        Me.panelInput.TabIndex = 1
'        '
'        'Label12
'        '
'        Me.Label12.Location = New System.Drawing.Point(23, 373)
'        Me.Label12.Name = "Label12"
'        Me.Label12.Size = New System.Drawing.Size(175, 31)
'        Me.Label12.TabIndex = 22
'        Me.Label12.Text = "التكرار كل (دقيقة)"
'        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'numRepeatInterval
'        '
'        Me.numRepeatInterval.Location = New System.Drawing.Point(20, 406)
'        Me.numRepeatInterval.Maximum = New Decimal(New Integer() {1440, 0, 0, 0})
'        Me.numRepeatInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
'        Me.numRepeatInterval.Name = "numRepeatInterval"
'        Me.numRepeatInterval.Size = New System.Drawing.Size(180, 32)
'        Me.numRepeatInterval.TabIndex = 21
'        Me.numRepeatInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
'        Me.numRepeatInterval.Value = New Decimal(New Integer() {2, 0, 0, 0})
'        '
'        'chkRepeat
'        '
'        Me.chkRepeat.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
'        Me.chkRepeat.Location = New System.Drawing.Point(223, 407)
'        Me.chkRepeat.Name = "chkRepeat"
'        Me.chkRepeat.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.chkRepeat.Size = New System.Drawing.Size(177, 31)
'        Me.chkRepeat.TabIndex = 20
'        Me.chkRepeat.Text = "تكرار التنبيه"
'        '
'        'lblActive
'        '
'        Me.lblActive.Location = New System.Drawing.Point(300, 373)
'        Me.lblActive.Name = "lblActive"
'        Me.lblActive.Size = New System.Drawing.Size(100, 31)
'        Me.lblActive.TabIndex = 14
'        Me.lblActive.Text = "تفعيل التنبيه"
'        Me.lblActive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'GroupBox1
'        '
'        Me.GroupBox1.Controls.Add(Me.Label11)
'        Me.GroupBox1.Controls.Add(Me.Label10)
'        Me.GroupBox1.Controls.Add(Me.Label6)
'        Me.GroupBox1.Controls.Add(Me.cmbFilterType)
'        Me.GroupBox1.Controls.Add(Me.txtSearch)
'        Me.GroupBox1.Controls.Add(Me.cmbFilterStatus)
'        Me.GroupBox1.Location = New System.Drawing.Point(0, 56)
'        Me.GroupBox1.Name = "GroupBox1"
'        Me.GroupBox1.Size = New System.Drawing.Size(237, 307)
'        Me.GroupBox1.TabIndex = 4
'        Me.GroupBox1.TabStop = False
'        Me.GroupBox1.Text = "البحث والفلترة 🔍"
'        '
'        'Label11
'        '
'        Me.Label11.Location = New System.Drawing.Point(108, 156)
'        Me.Label11.Name = "Label11"
'        Me.Label11.Size = New System.Drawing.Size(100, 23)
'        Me.Label11.TabIndex = 10
'        Me.Label11.Text = "النوع"
'        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label10
'        '
'        Me.Label10.Location = New System.Drawing.Point(108, 91)
'        Me.Label10.Name = "Label10"
'        Me.Label10.Size = New System.Drawing.Size(100, 23)
'        Me.Label10.TabIndex = 9
'        Me.Label10.Text = "الحالة"
'        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'Label6
'        '
'        Me.Label6.Location = New System.Drawing.Point(108, 26)
'        Me.Label6.Name = "Label6"
'        Me.Label6.Size = New System.Drawing.Size(100, 23)
'        Me.Label6.TabIndex = 8
'        Me.Label6.Text = "البحث"
'        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'FrmAlerts
'        '
'        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
'        Me.ClientSize = New System.Drawing.Size(1427, 568)
'        Me.Controls.Add(Me.GroupBox1)
'        Me.Controls.Add(Me.Panel1)
'        Me.Controls.Add(Me.panelInput)
'        Me.Controls.Add(Me.dgvAlerts)
'        Me.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
'        Me.Name = "FrmAlerts"
'        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.RightToLeftLayout = True
'        CType(Me.dgvAlerts, System.ComponentModel.ISupportInitialize).EndInit()
'        CType(Me.numDayOfMonth, System.ComponentModel.ISupportInitialize).EndInit()
'        Me.Panel1.ResumeLayout(False)
'        Me.Panel1.PerformLayout()
'        Me.panelInput.ResumeLayout(False)
'        Me.panelInput.PerformLayout()
'        CType(Me.numRepeatInterval, System.ComponentModel.ISupportInitialize).EndInit()
'        Me.GroupBox1.ResumeLayout(False)
'        Me.GroupBox1.PerformLayout()
'        Me.ResumeLayout(False)

'    End Sub

'    Friend WithEvents dgvAlerts As DataGridView
'    Friend WithEvents txtTitle As TextBox
'    Friend WithEvents txtDescription As TextBox
'    Friend WithEvents dtpDate As DateTimePicker

'    Friend WithEvents cmbType As ComboBox
'    Friend WithEvents cmbDayOfWeek As ComboBox
'    Friend WithEvents numDayOfMonth As NumericUpDown

'    Friend WithEvents chkActive As CheckBox

'    Friend WithEvents btnAdd As Button
'    Friend WithEvents btnUpdate As Button
'    Friend WithEvents btnDelete As Button
'    Friend WithEvents btnRefresh As Button
'    Friend WithEvents Label1 As Label
'    Friend WithEvents Label2 As Label
'    Friend WithEvents Label3 As Label
'    Friend WithEvents Label4 As Label
'    Friend WithEvents Label7 As Label
'    Friend WithEvents Label8 As Label
'    Friend WithEvents Label9 As Label
'    Friend WithEvents Panel1 As Panel
'    Friend WithEvents Label5 As Label
'    Friend WithEvents dtpTime As DateTimePicker
'    Friend WithEvents panelInput As Panel
'    Friend WithEvents lblActive As Label
'    Friend WithEvents cmbFilterType As ComboBox
'    Friend WithEvents cmbFilterStatus As ComboBox
'    Friend WithEvents txtSearch As TextBox
'    Friend WithEvents GroupBox1 As GroupBox
'    Friend WithEvents Label11 As Label
'    Friend WithEvents Label10 As Label
'    Friend WithEvents Label6 As Label
'    Friend WithEvents chkRepeat As CheckBox
'    Friend WithEvents Label12 As Label
'    Friend WithEvents numRepeatInterval As NumericUpDown
'End Class

