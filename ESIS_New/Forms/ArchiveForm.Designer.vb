<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ArchiveForm : Inherits BaseFormWithPermissions

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblWarning = New System.Windows.Forms.Label()
        Me.lblDeletedRecordCount = New System.Windows.Forms.Label()
        Me.lblRecordCount = New System.Windows.Forms.Label()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblArchiveWillMaster = New System.Windows.Forms.Label()
        Me.lblArchiveWillSub = New System.Windows.Forms.Label()
        Me.lblArchiveDate = New System.Windows.Forms.Label()
        Me.txtArchiveWillMaster = New System.Windows.Forms.TextBox()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.txtArchiveWillSub = New System.Windows.Forms.TextBox()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.dtpArchiveDate = New System.Windows.Forms.DateTimePicker()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.CbArchiveNum = New System.Windows.Forms.CheckBox()
        Me.lblArchiveFolder = New System.Windows.Forms.Label()
        Me.lblArchiveSubject = New System.Windows.Forms.Label()
        Me.txtArchiveSubject = New System.Windows.Forms.TextBox()
        Me.txtArchiveNum = New System.Windows.Forms.TextBox()
        Me.txtFileType = New System.Windows.Forms.TextBox()
        Me.lblFileType = New System.Windows.Forms.Label()
        Me.ComboArchiveFolder = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnBrowseFile = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnSelectArchive = New System.Windows.Forms.Button()
        Me.txtFilepath = New System.Windows.Forms.TextBox()
        Me.lblFilepath = New System.Windows.Forms.Label()
        Me.txtFilename = New System.Windows.Forms.TextBox()
        Me.lblFilename = New System.Windows.Forms.Label()
        Me.txtArchiveNotes = New System.Windows.Forms.TextBox()
        Me.lblArchiveNotes = New System.Windows.Forms.Label()
        Me.txtOriginalPath = New System.Windows.Forms.TextBox()
        Me.cmbFileType = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSearchByDate = New System.Windows.Forms.Button()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.dgvArchive = New System.Windows.Forms.DataGridView()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PicMsg = New System.Windows.Forms.PictureBox()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.TimerMsg = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvArchive, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.PicMsg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblWarning)
        Me.GroupBox1.Controls.Add(Me.lblDeletedRecordCount)
        Me.GroupBox1.Controls.Add(Me.lblRecordCount)
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanel3)
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanel2)
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBox1.Controls.Add(Me.btnSelectArchive)
        Me.GroupBox1.Controls.Add(Me.txtFilepath)
        Me.GroupBox1.Controls.Add(Me.lblFilepath)
        Me.GroupBox1.Controls.Add(Me.txtFilename)
        Me.GroupBox1.Controls.Add(Me.lblFilename)
        Me.GroupBox1.Controls.Add(Me.txtArchiveNotes)
        Me.GroupBox1.Controls.Add(Me.lblArchiveNotes)
        Me.GroupBox1.Controls.Add(Me.txtOriginalPath)
        Me.GroupBox1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(11, 11)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(764, 379)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "بيانات الأرشيف"
        '
        'lblWarning
        '
        Me.lblWarning.AutoSize = True
        Me.lblWarning.Font = New System.Drawing.Font("Cairo SemiBold", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblWarning.Location = New System.Drawing.Point(350, 305)
        Me.lblWarning.Name = "lblWarning"
        Me.lblWarning.Size = New System.Drawing.Size(24, 26)
        Me.lblWarning.TabIndex = 31
        Me.lblWarning.Text = "...."
        '
        'lblDeletedRecordCount
        '
        Me.lblDeletedRecordCount.Font = New System.Drawing.Font("Cairo SemiBold", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblDeletedRecordCount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.lblDeletedRecordCount.Location = New System.Drawing.Point(233, 193)
        Me.lblDeletedRecordCount.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblDeletedRecordCount.Name = "lblDeletedRecordCount"
        Me.lblDeletedRecordCount.Size = New System.Drawing.Size(238, 23)
        Me.lblDeletedRecordCount.TabIndex = 6
        Me.lblDeletedRecordCount.Text = "عدد المحذوفات: 0"
        Me.lblDeletedRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRecordCount
        '
        Me.lblRecordCount.Font = New System.Drawing.Font("Cairo SemiBold", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblRecordCount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.lblRecordCount.Location = New System.Drawing.Point(8, 193)
        Me.lblRecordCount.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblRecordCount.Name = "lblRecordCount"
        Me.lblRecordCount.Size = New System.Drawing.Size(238, 23)
        Me.lblRecordCount.TabIndex = 6
        Me.lblRecordCount.Text = "..."
        Me.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.50685!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.49315!))
        Me.TableLayoutPanel3.Controls.Add(Me.lblArchiveWillMaster, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.lblArchiveWillSub, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.lblArchiveDate, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.txtArchiveWillMaster, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.txtUserName, 1, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.txtArchiveWillSub, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.lblUserName, 0, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.dtpArchiveDate, 1, 2)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(5, 24)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 4
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(366, 161)
        Me.TableLayoutPanel3.TabIndex = 30
        '
        'lblArchiveWillMaster
        '
        Me.lblArchiveWillMaster.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblArchiveWillMaster.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArchiveWillMaster.Location = New System.Drawing.Point(253, 1)
        Me.lblArchiveWillMaster.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblArchiveWillMaster.Name = "lblArchiveWillMaster"
        Me.lblArchiveWillMaster.Size = New System.Drawing.Size(110, 39)
        Me.lblArchiveWillMaster.TabIndex = 8
        Me.lblArchiveWillMaster.Text = " البوليصة الرئيسية:"
        Me.lblArchiveWillMaster.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblArchiveWillSub
        '
        Me.lblArchiveWillSub.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblArchiveWillSub.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArchiveWillSub.Location = New System.Drawing.Point(253, 41)
        Me.lblArchiveWillSub.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblArchiveWillSub.Name = "lblArchiveWillSub"
        Me.lblArchiveWillSub.Size = New System.Drawing.Size(110, 39)
        Me.lblArchiveWillSub.TabIndex = 10
        Me.lblArchiveWillSub.Text = "البوليصة الفرعية:"
        Me.lblArchiveWillSub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblArchiveDate
        '
        Me.lblArchiveDate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblArchiveDate.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArchiveDate.Location = New System.Drawing.Point(253, 81)
        Me.lblArchiveDate.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblArchiveDate.Name = "lblArchiveDate"
        Me.lblArchiveDate.Size = New System.Drawing.Size(110, 38)
        Me.lblArchiveDate.TabIndex = 6
        Me.lblArchiveDate.Text = "تاريخ الأرشيف:"
        Me.lblArchiveDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtArchiveWillMaster
        '
        Me.txtArchiveWillMaster.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtArchiveWillMaster.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArchiveWillMaster.Location = New System.Drawing.Point(3, 5)
        Me.txtArchiveWillMaster.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtArchiveWillMaster.Name = "txtArchiveWillMaster"
        Me.txtArchiveWillMaster.Size = New System.Drawing.Size(245, 30)
        Me.txtArchiveWillMaster.TabIndex = 9
        Me.txtArchiveWillMaster.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtUserName
        '
        Me.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtUserName.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserName.Location = New System.Drawing.Point(3, 124)
        Me.txtUserName.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.ReadOnly = True
        Me.txtUserName.Size = New System.Drawing.Size(245, 30)
        Me.txtUserName.TabIndex = 21
        Me.txtUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtArchiveWillSub
        '
        Me.txtArchiveWillSub.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtArchiveWillSub.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArchiveWillSub.Location = New System.Drawing.Point(3, 45)
        Me.txtArchiveWillSub.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtArchiveWillSub.Name = "txtArchiveWillSub"
        Me.txtArchiveWillSub.Size = New System.Drawing.Size(245, 30)
        Me.txtArchiveWillSub.TabIndex = 11
        Me.txtArchiveWillSub.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblUserName
        '
        Me.lblUserName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblUserName.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.Location = New System.Drawing.Point(253, 120)
        Me.lblUserName.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(110, 40)
        Me.lblUserName.TabIndex = 20
        Me.lblUserName.Text = "اسم المستخدم:"
        Me.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpArchiveDate
        '
        Me.dtpArchiveDate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtpArchiveDate.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpArchiveDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpArchiveDate.Location = New System.Drawing.Point(3, 85)
        Me.dtpArchiveDate.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.dtpArchiveDate.Name = "dtpArchiveDate"
        Me.dtpArchiveDate.Size = New System.Drawing.Size(245, 30)
        Me.dtpArchiveDate.TabIndex = 7
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.09278!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.90722!))
        Me.TableLayoutPanel2.Controls.Add(Me.CbArchiveNum, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblArchiveFolder, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblArchiveSubject, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.txtArchiveSubject, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.txtArchiveNum, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.txtFileType, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblFileType, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.ComboArchiveFolder, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(377, 24)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 4
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(379, 160)
        Me.TableLayoutPanel2.TabIndex = 29
        '
        'CbArchiveNum
        '
        Me.CbArchiveNum.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CbArchiveNum.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.CbArchiveNum.Location = New System.Drawing.Point(276, 82)
        Me.CbArchiveNum.Name = "CbArchiveNum"
        Me.CbArchiveNum.Size = New System.Drawing.Size(99, 32)
        Me.CbArchiveNum.TabIndex = 33
        Me.CbArchiveNum.Text = "رقم الأرشيف:"
        Me.CbArchiveNum.UseVisualStyleBackColor = True
        '
        'lblArchiveFolder
        '
        Me.lblArchiveFolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblArchiveFolder.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArchiveFolder.Location = New System.Drawing.Point(275, 1)
        Me.lblArchiveFolder.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblArchiveFolder.Name = "lblArchiveFolder"
        Me.lblArchiveFolder.Size = New System.Drawing.Size(101, 38)
        Me.lblArchiveFolder.TabIndex = 4
        Me.lblArchiveFolder.Text = "مجلد الأرشيف:"
        Me.lblArchiveFolder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblArchiveSubject
        '
        Me.lblArchiveSubject.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblArchiveSubject.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArchiveSubject.Location = New System.Drawing.Point(275, 40)
        Me.lblArchiveSubject.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblArchiveSubject.Name = "lblArchiveSubject"
        Me.lblArchiveSubject.Size = New System.Drawing.Size(101, 38)
        Me.lblArchiveSubject.TabIndex = 2
        Me.lblArchiveSubject.Text = "موضوع الأرشيف:"
        Me.lblArchiveSubject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtArchiveSubject
        '
        Me.txtArchiveSubject.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtArchiveSubject.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArchiveSubject.Location = New System.Drawing.Point(3, 44)
        Me.txtArchiveSubject.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtArchiveSubject.Name = "txtArchiveSubject"
        Me.txtArchiveSubject.Size = New System.Drawing.Size(267, 30)
        Me.txtArchiveSubject.TabIndex = 3
        Me.txtArchiveSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtArchiveNum
        '
        Me.txtArchiveNum.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtArchiveNum.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArchiveNum.Location = New System.Drawing.Point(3, 83)
        Me.txtArchiveNum.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtArchiveNum.Name = "txtArchiveNum"
        Me.txtArchiveNum.ReadOnly = True
        Me.txtArchiveNum.Size = New System.Drawing.Size(267, 30)
        Me.txtArchiveNum.TabIndex = 1
        Me.txtArchiveNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtFileType
        '
        Me.txtFileType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFileType.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileType.Location = New System.Drawing.Point(3, 122)
        Me.txtFileType.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtFileType.Name = "txtFileType"
        Me.txtFileType.ReadOnly = True
        Me.txtFileType.Size = New System.Drawing.Size(267, 30)
        Me.txtFileType.TabIndex = 17
        Me.txtFileType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblFileType
        '
        Me.lblFileType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFileType.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileType.Location = New System.Drawing.Point(275, 118)
        Me.lblFileType.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblFileType.Name = "lblFileType"
        Me.lblFileType.Size = New System.Drawing.Size(101, 41)
        Me.lblFileType.TabIndex = 16
        Me.lblFileType.Text = "نوع الملف:"
        Me.lblFileType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboArchiveFolder
        '
        Me.ComboArchiveFolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboArchiveFolder.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboArchiveFolder.FormattingEnabled = True
        Me.ComboArchiveFolder.Location = New System.Drawing.Point(4, 4)
        Me.ComboArchiveFolder.Name = "ComboArchiveFolder"
        Me.ComboArchiveFolder.Size = New System.Drawing.Size(265, 31)
        Me.ComboArchiveFolder.TabIndex = 18
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnNew, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnAdd, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnBrowseFile, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnUpdate, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 3, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(2, 334)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(760, 41)
        Me.TableLayoutPanel1.TabIndex = 28
        '
        'btnNew
        '
        Me.btnNew.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnNew.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNew.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.ForeColor = System.Drawing.Color.White
        Me.btnNew.Location = New System.Drawing.Point(610, 4)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(148, 33)
        Me.btnNew.TabIndex = 27
        Me.btnNew.Text = "جديد"
        Me.btnNew.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ForeColor = System.Drawing.Color.White
        Me.btnAdd.Location = New System.Drawing.Point(458, 4)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(148, 33)
        Me.btnAdd.TabIndex = 22
        Me.btnAdd.Text = "إضافة"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnBrowseFile
        '
        Me.btnBrowseFile.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnBrowseFile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnBrowseFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowseFile.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseFile.ForeColor = System.Drawing.Color.White
        Me.btnBrowseFile.Location = New System.Drawing.Point(2, 4)
        Me.btnBrowseFile.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnBrowseFile.Name = "btnBrowseFile"
        Me.btnBrowseFile.Size = New System.Drawing.Size(148, 33)
        Me.btnBrowseFile.TabIndex = 25
        Me.btnBrowseFile.Text = "استعراض ملف"
        Me.btnBrowseFile.UseVisualStyleBackColor = False
        '
        'btnUpdate
        '
        Me.btnUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.btnUpdate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnUpdate.Enabled = False
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdate.ForeColor = System.Drawing.Color.White
        Me.btnUpdate.Location = New System.Drawing.Point(306, 4)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(148, 33)
        Me.btnUpdate.TabIndex = 23
        Me.btnUpdate.Text = "تعديل"
        Me.btnUpdate.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDelete.Enabled = False
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ForeColor = System.Drawing.Color.White
        Me.btnDelete.Location = New System.Drawing.Point(154, 4)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(148, 33)
        Me.btnDelete.TabIndex = 24
        Me.btnDelete.Text = "حذف"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnSelectArchive
        '
        Me.btnSelectArchive.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(121, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnSelectArchive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelectArchive.Font = New System.Drawing.Font("Cairo SemiBold", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectArchive.ForeColor = System.Drawing.Color.White
        Me.btnSelectArchive.Location = New System.Drawing.Point(8, 304)
        Me.btnSelectArchive.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnSelectArchive.Name = "btnSelectArchive"
        Me.btnSelectArchive.Size = New System.Drawing.Size(104, 23)
        Me.btnSelectArchive.TabIndex = 26
        Me.btnSelectArchive.Text = "فتح موقع الملف المحدد"
        Me.btnSelectArchive.UseVisualStyleBackColor = False
        '
        'txtFilepath
        '
        Me.txtFilepath.Font = New System.Drawing.Font("Consolas", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFilepath.Location = New System.Drawing.Point(4, 223)
        Me.txtFilepath.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtFilepath.Name = "txtFilepath"
        Me.txtFilepath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFilepath.Size = New System.Drawing.Size(541, 23)
        Me.txtFilepath.TabIndex = 19
        '
        'lblFilepath
        '
        Me.lblFilepath.AutoSize = True
        Me.lblFilepath.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilepath.Location = New System.Drawing.Point(470, 193)
        Me.lblFilepath.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblFilepath.Name = "lblFilepath"
        Me.lblFilepath.Size = New System.Drawing.Size(73, 23)
        Me.lblFilepath.TabIndex = 18
        Me.lblFilepath.Text = "مسار الملف:"
        '
        'txtFilename
        '
        Me.txtFilename.Font = New System.Drawing.Font("Cairo SemiBold", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFilename.Location = New System.Drawing.Point(4, 274)
        Me.txtFilename.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtFilename.Name = "txtFilename"
        Me.txtFilename.Size = New System.Drawing.Size(541, 27)
        Me.txtFilename.TabIndex = 15
        Me.txtFilename.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblFilename
        '
        Me.lblFilename.AutoSize = True
        Me.lblFilename.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilename.Location = New System.Drawing.Point(474, 249)
        Me.lblFilename.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblFilename.Name = "lblFilename"
        Me.lblFilename.Size = New System.Drawing.Size(68, 23)
        Me.lblFilename.TabIndex = 14
        Me.lblFilename.Text = "اسم الملف:"
        '
        'txtArchiveNotes
        '
        Me.txtArchiveNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtArchiveNotes.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArchiveNotes.Location = New System.Drawing.Point(549, 220)
        Me.txtArchiveNotes.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtArchiveNotes.Multiline = True
        Me.txtArchiveNotes.Name = "txtArchiveNotes"
        Me.txtArchiveNotes.Size = New System.Drawing.Size(207, 81)
        Me.txtArchiveNotes.TabIndex = 13
        Me.txtArchiveNotes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblArchiveNotes
        '
        Me.lblArchiveNotes.AutoSize = True
        Me.lblArchiveNotes.Font = New System.Drawing.Font("Cairo SemiBold", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArchiveNotes.Location = New System.Drawing.Point(693, 193)
        Me.lblArchiveNotes.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblArchiveNotes.Name = "lblArchiveNotes"
        Me.lblArchiveNotes.Size = New System.Drawing.Size(59, 23)
        Me.lblArchiveNotes.TabIndex = 12
        Me.lblArchiveNotes.Text = "ملاحظات:"
        '
        'txtOriginalPath
        '
        Me.txtOriginalPath.Font = New System.Drawing.Font("Consolas", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOriginalPath.Location = New System.Drawing.Point(4, 223)
        Me.txtOriginalPath.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtOriginalPath.Name = "txtOriginalPath"
        Me.txtOriginalPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOriginalPath.Size = New System.Drawing.Size(541, 23)
        Me.txtOriginalPath.TabIndex = 32
        Me.txtOriginalPath.Visible = False
        '
        'cmbFileType
        '
        Me.cmbFileType.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFileType.FormattingEnabled = True
        Me.cmbFileType.Location = New System.Drawing.Point(610, 50)
        Me.cmbFileType.Name = "cmbFileType"
        Me.cmbFileType.Size = New System.Drawing.Size(135, 25)
        Me.cmbFileType.TabIndex = 28
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.cmbFileType)
        Me.GroupBox2.Controls.Add(Me.btnSearchByDate)
        Me.GroupBox2.Controls.Add(Me.dtpEndDate)
        Me.GroupBox2.Controls.Add(Me.lblEndDate)
        Me.GroupBox2.Controls.Add(Me.dtpStartDate)
        Me.GroupBox2.Controls.Add(Me.lblStartDate)
        Me.GroupBox2.Controls.Add(Me.btnSearch)
        Me.GroupBox2.Controls.Add(Me.txtSearch)
        Me.GroupBox2.Controls.Add(Me.lblSearch)
        Me.GroupBox2.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(11, 395)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.GroupBox2.Size = New System.Drawing.Size(760, 85)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "البحث"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(610, 27)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(135, 19)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "تصنيف بحسب نوع الملف:"
        '
        'btnSearchByDate
        '
        Me.btnSearchByDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(66, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.btnSearchByDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchByDate.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearchByDate.ForeColor = System.Drawing.Color.White
        Me.btnSearchByDate.Location = New System.Drawing.Point(323, 20)
        Me.btnSearchByDate.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnSearchByDate.Name = "btnSearchByDate"
        Me.btnSearchByDate.Size = New System.Drawing.Size(273, 26)
        Me.btnSearchByDate.TabIndex = 7
        Me.btnSearchByDate.Text = "بحث بالتاريخ"
        Me.btnSearchByDate.UseVisualStyleBackColor = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(323, 50)
        Me.dtpEndDate.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(105, 25)
        Me.dtpEndDate.TabIndex = 6
        '
        'lblEndDate
        '
        Me.lblEndDate.AutoSize = True
        Me.lblEndDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.lblEndDate.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndDate.ForeColor = System.Drawing.Color.White
        Me.lblEndDate.Location = New System.Drawing.Point(428, 54)
        Me.lblEndDate.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.Size = New System.Drawing.Size(27, 19)
        Me.lblEndDate.TabIndex = 5
        Me.lblEndDate.Text = "إلى:"
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(462, 50)
        Me.dtpStartDate.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(105, 25)
        Me.dtpStartDate.TabIndex = 4
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.lblStartDate.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.Color.White
        Me.lblStartDate.Location = New System.Drawing.Point(567, 54)
        Me.lblStartDate.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.Size = New System.Drawing.Size(27, 19)
        Me.lblStartDate.TabIndex = 3
        Me.lblStartDate.Text = "من:"
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(181, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearch.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.ForeColor = System.Drawing.Color.White
        Me.btnSearch.Location = New System.Drawing.Point(4, 20)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(221, 26)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "بحث"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(4, 52)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(221, 25)
        Me.txtSearch.TabIndex = 1
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.Location = New System.Drawing.Point(229, 55)
        Me.lblSearch.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(58, 19)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "البحث عن:"
        '
        'dgvArchive
        '
        Me.dgvArchive.AllowUserToAddRows = False
        Me.dgvArchive.AllowUserToDeleteRows = False
        Me.dgvArchive.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvArchive.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvArchive.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvArchive.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvArchive.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvArchive.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvArchive.Location = New System.Drawing.Point(0, 519)
        Me.dgvArchive.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.dgvArchive.MultiSelect = False
        Me.dgvArchive.Name = "dgvArchive"
        Me.dgvArchive.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvArchive.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvArchive.RowHeadersWidth = 51
        Me.dgvArchive.RowTemplate.Height = 24
        Me.dgvArchive.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvArchive.Size = New System.Drawing.Size(1440, 183)
        Me.dgvArchive.TabIndex = 3
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Location = New System.Drawing.Point(936, 1141)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(87, 52)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "إغلاق"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(3, 28)
        Me.WebBrowser1.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(17, 30)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(654, 472)
        Me.WebBrowser1.TabIndex = 5
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.WebBrowser1)
        Me.GroupBox3.Location = New System.Drawing.Point(780, 9)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(660, 503)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "مستعرض الملفات"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.PicMsg)
        Me.Panel1.Controls.Add(Me.lblMessage)
        Me.Panel1.Location = New System.Drawing.Point(11, 482)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(762, 35)
        Me.Panel1.TabIndex = 33
        '
        'PicMsg
        '
        Me.PicMsg.Dock = System.Windows.Forms.DockStyle.Right
        Me.PicMsg.Location = New System.Drawing.Point(732, 0)
        Me.PicMsg.Name = "PicMsg"
        Me.PicMsg.Size = New System.Drawing.Size(30, 35)
        Me.PicMsg.TabIndex = 1
        Me.PicMsg.TabStop = False
        '
        'lblMessage
        '
        Me.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblMessage.Location = New System.Drawing.Point(0, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(762, 35)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimerMsg
        '
        Me.TimerMsg.Interval = 3000
        '
        'ArchiveForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1440, 702)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.dgvArchive)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.Name = "ArchiveForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إدارة الأرشيف - نظام ESIS"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.dgvArchive, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.PicMsg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents txtUserName As TextBox
    Friend WithEvents lblUserName As Label
    Friend WithEvents txtFilepath As TextBox
    Friend WithEvents lblFilepath As Label
    Friend WithEvents txtFileType As TextBox
    Friend WithEvents lblFileType As Label
    Friend WithEvents txtFilename As TextBox
    Friend WithEvents lblFilename As Label
    Friend WithEvents txtArchiveNotes As TextBox
    Friend WithEvents lblArchiveNotes As Label
    Friend WithEvents txtArchiveWillSub As TextBox
    Friend WithEvents lblArchiveWillSub As Label
    Friend WithEvents txtArchiveWillMaster As TextBox
    Friend WithEvents lblArchiveWillMaster As Label
    Friend WithEvents dtpArchiveDate As DateTimePicker
    Friend WithEvents lblArchiveDate As Label
    Friend WithEvents lblArchiveFolder As Label
    Friend WithEvents txtArchiveSubject As TextBox
    Friend WithEvents lblArchiveSubject As Label
    Friend WithEvents txtArchiveNum As TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btnSearchByDate As Button
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents lblEndDate As Label
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents lblStartDate As Label
    Friend WithEvents btnSearch As Button
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents dgvArchive As DataGridView
    Friend WithEvents btnClose As Button
    Friend WithEvents WebBrowser1 As WebBrowser
    Friend WithEvents btnBrowseFile As Button
    Friend WithEvents btnSelectArchive As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents cmbFileType As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents ComboArchiveFolder As ComboBox
    Friend WithEvents lblRecordCount As Label
    Friend WithEvents lblWarning As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents txtOriginalPath As TextBox
    Friend WithEvents lblDeletedRecordCount As Label
    Friend WithEvents CbArchiveNum As CheckBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TimerMsg As Timer
    Friend WithEvents PicMsg As PictureBox
    Friend WithEvents lblMessage As Label
End Class


