<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SaderForm : Inherits BaseFormWithPermissions

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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SaderForm))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.PicMsg = New System.Windows.Forms.PictureBox()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnAddCategory = New System.Windows.Forms.Button()
        Me.lblFilterStatus = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.rbUnarchivedSader = New System.Windows.Forms.RadioButton()
        Me.rbNumberBook = New System.Windows.Forms.RadioButton()
        Me.rbWillMaster = New System.Windows.Forms.RadioButton()
        Me.RBDate = New System.Windows.Forms.RadioButton()
        Me.rbSubjBook = New System.Windows.Forms.RadioButton()
        Me.rbWillSub = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpSearchDate = New System.Windows.Forms.DateTimePicker()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.cmbBookCategory = New System.Windows.Forms.ComboBox()
        Me.dgvSader = New System.Windows.Forms.DataGridView()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.AxAcroPDF1 = New AxAcroPDFLib.AxAcroPDF()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.ListPrinter = New System.Windows.Forms.CheckedListBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RBDirectPrint = New System.Windows.Forms.RadioButton()
        Me.RBPreviewBeforePrint = New System.Windows.Forms.RadioButton()
        Me.lblWarning = New System.Windows.Forms.Label()
        Me.TLPControls = New System.Windows.Forms.TableLayoutPanel()
        Me.lblWillMaster = New System.Windows.Forms.Label()
        Me.txtWillMaster = New System.Windows.Forms.TextBox()
        Me.lblArchiveID = New System.Windows.Forms.Label()
        Me.txtArchiveID = New System.Windows.Forms.TextBox()
        Me.lblWillSub = New System.Windows.Forms.Label()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.lblNumberBook = New System.Windows.Forms.Label()
        Me.txtNumberBook = New System.Windows.Forms.TextBox()
        Me.txtWillSub = New System.Windows.Forms.TextBox()
        Me.lblDateBook = New System.Windows.Forms.Label()
        Me.dtpDateBook = New System.Windows.Forms.DateTimePicker()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.ComboSubjBook = New System.Windows.Forms.ComboBox()
        Me.btnAddNewSubj = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnShowUnarchivedOnly = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnSearchByDate = New System.Windows.Forms.Button()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.lblSubjBook = New System.Windows.Forms.Label()
        Me.txtPrintNumberBook = New System.Windows.Forms.TextBox()
        Me.lblRecordCount = New System.Windows.Forms.Label()
        Me.TimerMsg = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.PicMsg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        CType(Me.dgvSader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        CType(Me.AxAcroPDF1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TLPControls.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1379, 678)
        Me.Panel1.TabIndex = 20
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.btnAddCategory)
        Me.Panel2.Controls.Add(Me.lblFilterStatus)
        Me.Panel2.Controls.Add(Me.GroupBox5)
        Me.Panel2.Controls.Add(Me.cmbBookCategory)
        Me.Panel2.Controls.Add(Me.dgvSader)
        Me.Panel2.Controls.Add(Me.GroupBox4)
        Me.Panel2.Controls.Add(Me.GroupBox3)
        Me.Panel2.Controls.Add(Me.GroupBox1)
        Me.Panel2.Controls.Add(Me.lblRecordCount)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1379, 678)
        Me.Panel2.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel3.Controls.Add(Me.PicMsg)
        Me.Panel3.Controls.Add(Me.lblMessage)
        Me.Panel3.Location = New System.Drawing.Point(505, 641)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(864, 37)
        Me.Panel3.TabIndex = 32
        '
        'PicMsg
        '
        Me.PicMsg.Dock = System.Windows.Forms.DockStyle.Right
        Me.PicMsg.Location = New System.Drawing.Point(827, 0)
        Me.PicMsg.Name = "PicMsg"
        Me.PicMsg.Size = New System.Drawing.Size(37, 37)
        Me.PicMsg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PicMsg.TabIndex = 31
        Me.PicMsg.TabStop = False
        '
        'lblMessage
        '
        Me.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblMessage.Font = New System.Drawing.Font("Cairo", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblMessage.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblMessage.Location = New System.Drawing.Point(0, 0)
        Me.lblMessage.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(864, 37)
        Me.lblMessage.TabIndex = 30
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(1259, 11)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 30)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "تصنيف الكتاب:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnAddCategory
        '
        Me.btnAddCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(185, Byte), Integer), CType(CType(193, Byte), Integer), CType(CType(7, Byte), Integer))
        Me.btnAddCategory.FlatAppearance.BorderSize = 0
        Me.btnAddCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddCategory.Font = New System.Drawing.Font("Cairo", 8.999999!)
        Me.btnAddCategory.Location = New System.Drawing.Point(788, 12)
        Me.btnAddCategory.Name = "btnAddCategory"
        Me.btnAddCategory.Size = New System.Drawing.Size(191, 32)
        Me.btnAddCategory.TabIndex = 28
        Me.btnAddCategory.Text = "إدارة التصنيفات"
        Me.btnAddCategory.UseVisualStyleBackColor = False
        '
        'lblFilterStatus
        '
        Me.lblFilterStatus.AutoSize = True
        Me.lblFilterStatus.Location = New System.Drawing.Point(479, 9)
        Me.lblFilterStatus.Name = "lblFilterStatus"
        Me.lblFilterStatus.Size = New System.Drawing.Size(19, 24)
        Me.lblFilterStatus.TabIndex = 27
        Me.lblFilterStatus.Text = "..."
        '
        'GroupBox5
        '
        Me.GroupBox5.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.GroupBox5.Controls.Add(Me.TableLayoutPanel3)
        Me.GroupBox5.Controls.Add(Me.Label1)
        Me.GroupBox5.Controls.Add(Me.dtpSearchDate)
        Me.GroupBox5.Controls.Add(Me.txtSearch)
        Me.GroupBox5.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(237, 39)
        Me.GroupBox5.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Padding = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.GroupBox5.Size = New System.Drawing.Size(257, 290)
        Me.GroupBox5.TabIndex = 21
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "خيارات البحث"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.17742!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.82258!))
        Me.TableLayoutPanel3.Controls.Add(Me.rbUnarchivedSader, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.rbNumberBook, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.rbWillMaster, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.RBDate, 1, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.rbSubjBook, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.rbWillSub, 1, 1)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 30)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 3
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.12195!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 54.87805!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(248, 118)
        Me.TableLayoutPanel3.TabIndex = 26
        '
        'rbUnarchivedSader
        '
        Me.rbUnarchivedSader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rbUnarchivedSader.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.rbUnarchivedSader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rbUnarchivedSader.Location = New System.Drawing.Point(133, 84)
        Me.rbUnarchivedSader.Margin = New System.Windows.Forms.Padding(2)
        Me.rbUnarchivedSader.Name = "rbUnarchivedSader"
        Me.rbUnarchivedSader.Size = New System.Drawing.Size(113, 32)
        Me.rbUnarchivedSader.TabIndex = 54
        Me.rbUnarchivedSader.TabStop = True
        Me.rbUnarchivedSader.Text = "الغير مـــؤرشف"
        Me.rbUnarchivedSader.UseVisualStyleBackColor = True
        '
        'rbNumberBook
        '
        Me.rbNumberBook.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rbNumberBook.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.rbNumberBook.Location = New System.Drawing.Point(134, 6)
        Me.rbNumberBook.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.rbNumberBook.Name = "rbNumberBook"
        Me.rbNumberBook.Size = New System.Drawing.Size(111, 25)
        Me.rbNumberBook.TabIndex = 0
        Me.rbNumberBook.TabStop = True
        Me.rbNumberBook.Text = "رقـــم الكتــاب"
        Me.rbNumberBook.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rbNumberBook.UseVisualStyleBackColor = True
        '
        'rbWillMaster
        '
        Me.rbWillMaster.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rbWillMaster.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.rbWillMaster.Location = New System.Drawing.Point(3, 6)
        Me.rbWillMaster.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.rbWillMaster.Name = "rbWillMaster"
        Me.rbWillMaster.Size = New System.Drawing.Size(125, 25)
        Me.rbWillMaster.TabIndex = 1
        Me.rbWillMaster.TabStop = True
        Me.rbWillMaster.Text = "البوليصة الرئيسية"
        Me.rbWillMaster.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rbWillMaster.UseVisualStyleBackColor = True
        '
        'RBDate
        '
        Me.RBDate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RBDate.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.RBDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RBDate.Location = New System.Drawing.Point(2, 84)
        Me.RBDate.Margin = New System.Windows.Forms.Padding(2)
        Me.RBDate.Name = "RBDate"
        Me.RBDate.Size = New System.Drawing.Size(127, 32)
        Me.RBDate.TabIndex = 49
        Me.RBDate.TabStop = True
        Me.RBDate.Text = "البحـــث بالتـــاريخ"
        Me.RBDate.UseVisualStyleBackColor = True
        '
        'rbSubjBook
        '
        Me.rbSubjBook.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rbSubjBook.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.rbSubjBook.Location = New System.Drawing.Point(134, 43)
        Me.rbSubjBook.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.rbSubjBook.Name = "rbSubjBook"
        Me.rbSubjBook.Size = New System.Drawing.Size(111, 33)
        Me.rbSubjBook.TabIndex = 3
        Me.rbSubjBook.TabStop = True
        Me.rbSubjBook.Text = "موضوع الكتاب"
        Me.rbSubjBook.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rbSubjBook.UseVisualStyleBackColor = True
        '
        'rbWillSub
        '
        Me.rbWillSub.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rbWillSub.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.rbWillSub.Location = New System.Drawing.Point(3, 43)
        Me.rbWillSub.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.rbWillSub.Name = "rbWillSub"
        Me.rbWillSub.Size = New System.Drawing.Size(125, 33)
        Me.rbWillSub.TabIndex = 2
        Me.rbWillSub.TabStop = True
        Me.rbWillSub.Text = "البوليصة الفرعية"
        Me.rbWillSub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rbWillSub.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(6, 157)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(245, 46)
        Me.Label1.TabIndex = 51
        Me.Label1.Text = "عند إختيار البحث بالتاريخ يرجى" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " اختيار التاريخ من اداة التاريخ بالأسفل"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpSearchDate
        '
        Me.dtpSearchDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpSearchDate.Enabled = False
        Me.dtpSearchDate.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.dtpSearchDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpSearchDate.Location = New System.Drawing.Point(7, 212)
        Me.dtpSearchDate.Name = "dtpSearchDate"
        Me.dtpSearchDate.Size = New System.Drawing.Size(242, 30)
        Me.dtpSearchDate.TabIndex = 50
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtSearch.Location = New System.Drawing.Point(3, 250)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(250, 30)
        Me.txtSearch.TabIndex = 1
        '
        'cmbBookCategory
        '
        Me.cmbBookCategory.DropDownHeight = 100
        Me.cmbBookCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBookCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbBookCategory.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.cmbBookCategory.FormattingEnabled = True
        Me.cmbBookCategory.IntegralHeight = False
        Me.cmbBookCategory.Location = New System.Drawing.Point(985, 12)
        Me.cmbBookCategory.Name = "cmbBookCategory"
        Me.cmbBookCategory.Size = New System.Drawing.Size(269, 31)
        Me.cmbBookCategory.TabIndex = 25
        '
        'dgvSader
        '
        Me.dgvSader.AllowUserToAddRows = False
        Me.dgvSader.AllowUserToDeleteRows = False
        Me.dgvSader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvSader.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvSader.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvSader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvSader.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvSader.Location = New System.Drawing.Point(505, 359)
        Me.dgvSader.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.dgvSader.MultiSelect = False
        Me.dgvSader.Name = "dgvSader"
        Me.dgvSader.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvSader.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvSader.RowHeadersWidth = 51
        Me.dgvSader.RowTemplate.Height = 24
        Me.dgvSader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSader.Size = New System.Drawing.Size(862, 278)
        Me.dgvSader.TabIndex = 24
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.AxAcroPDF1)
        Me.GroupBox4.Location = New System.Drawing.Point(8, 327)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.GroupBox4.Size = New System.Drawing.Size(486, 351)
        Me.GroupBox4.TabIndex = 26
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "رقم الصادر"
        '
        'AxAcroPDF1
        '
        Me.AxAcroPDF1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AxAcroPDF1.Enabled = True
        Me.AxAcroPDF1.Location = New System.Drawing.Point(3, 31)
        Me.AxAcroPDF1.Name = "AxAcroPDF1"
        Me.AxAcroPDF1.OcxState = CType(resources.GetObject("AxAcroPDF1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxAcroPDF1.Size = New System.Drawing.Size(480, 314)
        Me.AxAcroPDF1.TabIndex = 1
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.ListPrinter)
        Me.GroupBox3.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GroupBox3.Location = New System.Drawing.Point(8, 39)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.GroupBox3.Size = New System.Drawing.Size(231, 290)
        Me.GroupBox3.TabIndex = 25
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "الطابعات"
        '
        'ListPrinter
        '
        Me.ListPrinter.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ListPrinter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListPrinter.Font = New System.Drawing.Font("Tahoma", 10.0!)
        Me.ListPrinter.FormattingEnabled = True
        Me.ListPrinter.Location = New System.Drawing.Point(3, 26)
        Me.ListPrinter.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.ListPrinter.Name = "ListPrinter"
        Me.ListPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ListPrinter.Size = New System.Drawing.Size(225, 258)
        Me.ListPrinter.TabIndex = 19
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RBDirectPrint)
        Me.GroupBox1.Controls.Add(Me.RBPreviewBeforePrint)
        Me.GroupBox1.Controls.Add(Me.lblWarning)
        Me.GroupBox1.Controls.Add(Me.TLPControls)
        Me.GroupBox1.Controls.Add(Me.ComboSubjBook)
        Me.GroupBox1.Controls.Add(Me.btnAddNewSubj)
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.lblSubjBook)
        Me.GroupBox1.Controls.Add(Me.txtPrintNumberBook)
        Me.GroupBox1.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(501, 51)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(876, 302)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "بيانات الصادر"
        '
        'RBDirectPrint
        '
        Me.RBDirectPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RBDirectPrint.AutoSize = True
        Me.RBDirectPrint.Checked = True
        Me.RBDirectPrint.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.RBDirectPrint.ForeColor = System.Drawing.Color.DarkRed
        Me.RBDirectPrint.Location = New System.Drawing.Point(764, 219)
        Me.RBDirectPrint.Margin = New System.Windows.Forms.Padding(2)
        Me.RBDirectPrint.Name = "RBDirectPrint"
        Me.RBDirectPrint.Size = New System.Drawing.Size(101, 27)
        Me.RBDirectPrint.TabIndex = 51
        Me.RBDirectPrint.TabStop = True
        Me.RBDirectPrint.Text = "طباعة مباشرة"
        Me.RBDirectPrint.UseVisualStyleBackColor = True
        '
        'RBPreviewBeforePrint
        '
        Me.RBPreviewBeforePrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RBPreviewBeforePrint.AutoSize = True
        Me.RBPreviewBeforePrint.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.RBPreviewBeforePrint.ForeColor = System.Drawing.Color.DarkRed
        Me.RBPreviewBeforePrint.Location = New System.Drawing.Point(630, 219)
        Me.RBPreviewBeforePrint.Margin = New System.Windows.Forms.Padding(2)
        Me.RBPreviewBeforePrint.Name = "RBPreviewBeforePrint"
        Me.RBPreviewBeforePrint.Size = New System.Drawing.Size(130, 27)
        Me.RBPreviewBeforePrint.TabIndex = 50
        Me.RBPreviewBeforePrint.TabStop = True
        Me.RBPreviewBeforePrint.Text = "معاينة قبل الطباعة"
        Me.RBPreviewBeforePrint.UseVisualStyleBackColor = True
        '
        'lblWarning
        '
        Me.lblWarning.AutoSize = True
        Me.lblWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWarning.ForeColor = System.Drawing.Color.Red
        Me.lblWarning.Location = New System.Drawing.Point(287, 191)
        Me.lblWarning.Name = "lblWarning"
        Me.lblWarning.Size = New System.Drawing.Size(2, 31)
        Me.lblWarning.TabIndex = 53
        Me.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TLPControls
        '
        Me.TLPControls.ColumnCount = 4
        Me.TLPControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.20374!))
        Me.TLPControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.71137!))
        Me.TLPControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.31749!))
        Me.TLPControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.10696!))
        Me.TLPControls.Controls.Add(Me.lblWillMaster, 0, 0)
        Me.TLPControls.Controls.Add(Me.txtWillMaster, 1, 0)
        Me.TLPControls.Controls.Add(Me.lblArchiveID, 2, 0)
        Me.TLPControls.Controls.Add(Me.txtArchiveID, 3, 0)
        Me.TLPControls.Controls.Add(Me.lblWillSub, 0, 1)
        Me.TLPControls.Controls.Add(Me.txtUserName, 3, 1)
        Me.TLPControls.Controls.Add(Me.lblNumberBook, 2, 2)
        Me.TLPControls.Controls.Add(Me.txtNumberBook, 3, 2)
        Me.TLPControls.Controls.Add(Me.txtWillSub, 1, 1)
        Me.TLPControls.Controls.Add(Me.lblDateBook, 0, 2)
        Me.TLPControls.Controls.Add(Me.dtpDateBook, 1, 2)
        Me.TLPControls.Controls.Add(Me.lblUserName, 2, 1)
        Me.TLPControls.Enabled = False
        Me.TLPControls.Location = New System.Drawing.Point(285, 26)
        Me.TLPControls.Name = "TLPControls"
        Me.TLPControls.RowCount = 3
        Me.TLPControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.07407!))
        Me.TLPControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.79426!))
        Me.TLPControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.92344!))
        Me.TLPControls.Size = New System.Drawing.Size(589, 113)
        Me.TLPControls.TabIndex = 25
        '
        'lblWillMaster
        '
        Me.lblWillMaster.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblWillMaster.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblWillMaster.Location = New System.Drawing.Point(473, 0)
        Me.lblWillMaster.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblWillMaster.Name = "lblWillMaster"
        Me.lblWillMaster.Size = New System.Drawing.Size(114, 36)
        Me.lblWillMaster.TabIndex = 6
        Me.lblWillMaster.Text = " البوليصة الرئيسية:"
        Me.lblWillMaster.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtWillMaster
        '
        Me.txtWillMaster.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtWillMaster.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtWillMaster.Location = New System.Drawing.Point(299, 4)
        Me.txtWillMaster.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtWillMaster.Name = "txtWillMaster"
        Me.txtWillMaster.Size = New System.Drawing.Size(170, 30)
        Me.txtWillMaster.TabIndex = 7
        Me.txtWillMaster.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblArchiveID
        '
        Me.lblArchiveID.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblArchiveID.Location = New System.Drawing.Point(198, 0)
        Me.lblArchiveID.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblArchiveID.Name = "lblArchiveID"
        Me.lblArchiveID.Size = New System.Drawing.Size(97, 30)
        Me.lblArchiveID.TabIndex = 12
        Me.lblArchiveID.Text = "معرف الأرشيف:"
        Me.lblArchiveID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtArchiveID
        '
        Me.txtArchiveID.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtArchiveID.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtArchiveID.Location = New System.Drawing.Point(2, 4)
        Me.txtArchiveID.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtArchiveID.Name = "txtArchiveID"
        Me.txtArchiveID.ReadOnly = True
        Me.txtArchiveID.Size = New System.Drawing.Size(192, 30)
        Me.txtArchiveID.TabIndex = 13
        '
        'lblWillSub
        '
        Me.lblWillSub.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblWillSub.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblWillSub.Location = New System.Drawing.Point(473, 36)
        Me.lblWillSub.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblWillSub.Name = "lblWillSub"
        Me.lblWillSub.Size = New System.Drawing.Size(114, 40)
        Me.lblWillSub.TabIndex = 8
        Me.lblWillSub.Text = " البوليصة الفرعية:"
        Me.lblWillSub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtUserName
        '
        Me.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtUserName.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtUserName.Location = New System.Drawing.Point(2, 40)
        Me.txtUserName.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.ReadOnly = True
        Me.txtUserName.Size = New System.Drawing.Size(192, 30)
        Me.txtUserName.TabIndex = 11
        '
        'lblNumberBook
        '
        Me.lblNumberBook.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblNumberBook.Location = New System.Drawing.Point(198, 76)
        Me.lblNumberBook.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblNumberBook.Name = "lblNumberBook"
        Me.lblNumberBook.Size = New System.Drawing.Size(97, 30)
        Me.lblNumberBook.TabIndex = 0
        Me.lblNumberBook.Text = "رقــم الكتـــــاب:"
        Me.lblNumberBook.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtNumberBook
        '
        Me.txtNumberBook.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtNumberBook.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtNumberBook.Location = New System.Drawing.Point(2, 80)
        Me.txtNumberBook.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtNumberBook.Name = "txtNumberBook"
        Me.txtNumberBook.ReadOnly = True
        Me.txtNumberBook.Size = New System.Drawing.Size(192, 30)
        Me.txtNumberBook.TabIndex = 1
        '
        'txtWillSub
        '
        Me.txtWillSub.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtWillSub.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtWillSub.Location = New System.Drawing.Point(299, 40)
        Me.txtWillSub.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtWillSub.Name = "txtWillSub"
        Me.txtWillSub.Size = New System.Drawing.Size(170, 30)
        Me.txtWillSub.TabIndex = 9
        Me.txtWillSub.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblDateBook
        '
        Me.lblDateBook.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDateBook.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblDateBook.Location = New System.Drawing.Point(473, 76)
        Me.lblDateBook.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblDateBook.Name = "lblDateBook"
        Me.lblDateBook.Size = New System.Drawing.Size(114, 37)
        Me.lblDateBook.TabIndex = 2
        Me.lblDateBook.Text = "تـــــاريخ الكتـــاب:"
        Me.lblDateBook.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpDateBook
        '
        Me.dtpDateBook.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtpDateBook.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.dtpDateBook.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDateBook.Location = New System.Drawing.Point(299, 80)
        Me.dtpDateBook.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.dtpDateBook.Name = "dtpDateBook"
        Me.dtpDateBook.Size = New System.Drawing.Size(170, 30)
        Me.dtpDateBook.TabIndex = 3
        '
        'lblUserName
        '
        Me.lblUserName.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblUserName.Location = New System.Drawing.Point(198, 36)
        Me.lblUserName.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(97, 30)
        Me.lblUserName.TabIndex = 10
        Me.lblUserName.Text = "اسم المستخدم:"
        Me.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboSubjBook
        '
        Me.ComboSubjBook.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ComboSubjBook.DropDownHeight = 400
        Me.ComboSubjBook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboSubjBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboSubjBook.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.ComboSubjBook.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.ComboSubjBook.FormattingEnabled = True
        Me.ComboSubjBook.IntegralHeight = False
        Me.ComboSubjBook.Location = New System.Drawing.Point(428, 146)
        Me.ComboSubjBook.Name = "ComboSubjBook"
        Me.ComboSubjBook.Size = New System.Drawing.Size(329, 31)
        Me.ComboSubjBook.TabIndex = 24
        '
        'btnAddNewSubj
        '
        Me.btnAddNewSubj.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnAddNewSubj.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddNewSubj.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnAddNewSubj.ForeColor = System.Drawing.Color.White
        Me.btnAddNewSubj.Location = New System.Drawing.Point(287, 146)
        Me.btnAddNewSubj.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnAddNewSubj.Name = "btnAddNewSubj"
        Me.btnAddNewSubj.Size = New System.Drawing.Size(136, 31)
        Me.btnAddNewSubj.TabIndex = 8
        Me.btnAddNewSubj.Text = "إضافة موضوع جديد"
        Me.btnAddNewSubj.UseVisualStyleBackColor = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 7
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnShowUnarchivedOnly, 6, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnClose, 5, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnNew, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnAdd, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnPrint, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnUpdate, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 3, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(2, 248)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(872, 50)
        Me.TableLayoutPanel1.TabIndex = 22
        '
        'btnShowUnarchivedOnly
        '
        Me.btnShowUnarchivedOnly.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.btnShowUnarchivedOnly.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnShowUnarchivedOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShowUnarchivedOnly.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnShowUnarchivedOnly.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnShowUnarchivedOnly.Location = New System.Drawing.Point(2, 4)
        Me.btnShowUnarchivedOnly.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnShowUnarchivedOnly.Name = "btnShowUnarchivedOnly"
        Me.btnShowUnarchivedOnly.Size = New System.Drawing.Size(124, 42)
        Me.btnShowUnarchivedOnly.TabIndex = 54
        Me.btnShowUnarchivedOnly.Text = "عرض الغير مؤرشف"
        Me.btnShowUnarchivedOnly.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnClose.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Location = New System.Drawing.Point(130, 4)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(120, 42)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "إغلاق"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnNew
        '
        Me.btnNew.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnNew.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNew.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.ForeColor = System.Drawing.Color.White
        Me.btnNew.Location = New System.Drawing.Point(750, 4)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(120, 42)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "جديد"
        Me.btnNew.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ForeColor = System.Drawing.Color.White
        Me.btnAdd.Location = New System.Drawing.Point(626, 4)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(120, 42)
        Me.btnAdd.TabIndex = 14
        Me.btnAdd.Text = "إضافة"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnPrint.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.ForeColor = System.Drawing.Color.White
        Me.btnPrint.Location = New System.Drawing.Point(254, 4)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(120, 42)
        Me.btnPrint.TabIndex = 18
        Me.btnPrint.Text = "طباعة"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnUpdate
        '
        Me.btnUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.btnUpdate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdate.ForeColor = System.Drawing.Color.White
        Me.btnUpdate.Location = New System.Drawing.Point(502, 4)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(120, 42)
        Me.btnUpdate.TabIndex = 15
        Me.btnUpdate.Text = "تعديل"
        Me.btnUpdate.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ForeColor = System.Drawing.Color.White
        Me.btnDelete.Location = New System.Drawing.Point(378, 4)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 42)
        Me.btnDelete.TabIndex = 16
        Me.btnDelete.Text = "حذف"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnSearchByDate)
        Me.GroupBox2.Controls.Add(Me.dtpEndDate)
        Me.GroupBox2.Controls.Add(Me.lblEndDate)
        Me.GroupBox2.Controls.Add(Me.dtpStartDate)
        Me.GroupBox2.Controls.Add(Me.lblStartDate)
        Me.GroupBox2.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(4, 20)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.GroupBox2.Size = New System.Drawing.Size(276, 114)
        Me.GroupBox2.TabIndex = 23
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "البحث بين تاريخين"
        '
        'btnSearchByDate
        '
        Me.btnSearchByDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnSearchByDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchByDate.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnSearchByDate.ForeColor = System.Drawing.Color.White
        Me.btnSearchByDate.Location = New System.Drawing.Point(15, 74)
        Me.btnSearchByDate.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnSearchByDate.Name = "btnSearchByDate"
        Me.btnSearchByDate.Size = New System.Drawing.Size(250, 34)
        Me.btnSearchByDate.TabIndex = 7
        Me.btnSearchByDate.Text = "بحث بالتاريخ"
        Me.btnSearchByDate.UseVisualStyleBackColor = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(6, 38)
        Me.dtpEndDate.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(103, 30)
        Me.dtpEndDate.TabIndex = 6
        '
        'lblEndDate
        '
        Me.lblEndDate.AutoSize = True
        Me.lblEndDate.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.lblEndDate.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblEndDate.Location = New System.Drawing.Point(111, 42)
        Me.lblEndDate.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.Size = New System.Drawing.Size(30, 23)
        Me.lblEndDate.TabIndex = 5
        Me.lblEndDate.Text = "إلى:"
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(141, 38)
        Me.dtpStartDate.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(102, 30)
        Me.dtpStartDate.TabIndex = 4
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.lblStartDate.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblStartDate.Location = New System.Drawing.Point(245, 42)
        Me.lblStartDate.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.Size = New System.Drawing.Size(30, 23)
        Me.lblStartDate.TabIndex = 3
        Me.lblStartDate.Text = "من:"
        '
        'lblSubjBook
        '
        Me.lblSubjBook.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblSubjBook.Location = New System.Drawing.Point(764, 146)
        Me.lblSubjBook.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSubjBook.Name = "lblSubjBook"
        Me.lblSubjBook.Size = New System.Drawing.Size(107, 30)
        Me.lblSubjBook.TabIndex = 4
        Me.lblSubjBook.Text = "موضوع الكتاب:"
        Me.lblSubjBook.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPrintNumberBook
        '
        Me.txtPrintNumberBook.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtPrintNumberBook.Location = New System.Drawing.Point(551, 147)
        Me.txtPrintNumberBook.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtPrintNumberBook.Name = "txtPrintNumberBook"
        Me.txtPrintNumberBook.Size = New System.Drawing.Size(150, 30)
        Me.txtPrintNumberBook.TabIndex = 52
        '
        'lblRecordCount
        '
        Me.lblRecordCount.Font = New System.Drawing.Font("Cairo SemiBold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecordCount.Location = New System.Drawing.Point(13, 9)
        Me.lblRecordCount.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblRecordCount.Name = "lblRecordCount"
        Me.lblRecordCount.Size = New System.Drawing.Size(278, 29)
        Me.lblRecordCount.TabIndex = 0
        Me.lblRecordCount.Text = "...."
        Me.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimerMsg
        '
        Me.TimerMsg.Interval = 3000
        '
        'SaderForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1379, 678)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.Name = "SaderForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إدارة الصادر - نظام ESIS"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        CType(Me.PicMsg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        CType(Me.dgvSader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        CType(Me.AxAcroPDF1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TLPControls.ResumeLayout(False)
        Me.TLPControls.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnClose As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btnSearchByDate As Button
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents lblEndDate As Label
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents lblStartDate As Label
    Friend WithEvents dgvSader As DataGridView
    Friend WithEvents GroupBox4 As GroupBox
    'Friend WithEvents CRV As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents ListPrinter As CheckedListBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents btnNew As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents rbSubjBook As RadioButton
    Friend WithEvents rbWillSub As RadioButton
    Friend WithEvents rbWillMaster As RadioButton
    Friend WithEvents rbNumberBook As RadioButton
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents txtArchiveID As TextBox
    Friend WithEvents lblArchiveID As Label
    Friend WithEvents txtUserName As TextBox
    Friend WithEvents lblUserName As Label
    Friend WithEvents txtWillSub As TextBox
    Friend WithEvents lblWillSub As Label
    Friend WithEvents txtWillMaster As TextBox
    Friend WithEvents lblWillMaster As Label
    Friend WithEvents lblSubjBook As Label
    Friend WithEvents dtpDateBook As DateTimePicker
    Friend WithEvents lblDateBook As Label
    Friend WithEvents txtNumberBook As TextBox
    Friend WithEvents lblNumberBook As Label
    Friend WithEvents dtpSearchDate As DateTimePicker
    Friend WithEvents RBDate As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbBookCategory As ComboBox
    Friend WithEvents btnAddNewSubj As Button
    Friend WithEvents TLPControls As TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Public WithEvents ComboSubjBook As ComboBox
    Friend WithEvents txtPrintNumberBook As TextBox
    Friend WithEvents lblRecordCount As Label
    Friend WithEvents lblWarning As Label
    Friend WithEvents btnShowUnarchivedOnly As Button
    Friend WithEvents lblFilterStatus As Label
    Friend WithEvents TimerMsg As Timer
    Friend WithEvents AxAcroPDF1 As AxAcroPDFLib.AxAcroPDF
    Friend WithEvents RBDirectPrint As RadioButton
    Friend WithEvents RBPreviewBeforePrint As RadioButton
    Friend WithEvents rbUnarchivedSader As RadioButton
    Friend WithEvents btnAddCategory As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents PicMsg As PictureBox
    Friend WithEvents lblMessage As Label
    Friend WithEvents Panel3 As Panel
End Class

