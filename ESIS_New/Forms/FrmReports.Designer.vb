<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmReports

    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    Friend WithEvents PanelTop As Panel
    Friend WithEvents cmbReportType As ComboBox
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents btnPreview As Button
    Friend WithEvents btnExportPdf As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents pdfViewer As PdfiumViewer.PdfViewer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PanelTop = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.txtSubjBook = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbYear = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbReportType = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtWillMaster = New System.Windows.Forms.TextBox()
        Me.txtNumberBook = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtWillSub = New System.Windows.Forms.TextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnResetFilters = New System.Windows.Forms.Button()
        Me.btnPreview = New System.Windows.Forms.Button()
        Me.btnExportPdf = New System.Windows.Forms.Button()
        Me.ListPrinter = New System.Windows.Forms.CheckedListBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.pdfViewer = New PdfiumViewer.PdfViewer()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtPages = New System.Windows.Forms.TextBox()
        Me.RBPrintCustom = New System.Windows.Forms.RadioButton()
        Me.RBPrintCurrent = New System.Windows.Forms.RadioButton()
        Me.RBPrintAll = New System.Windows.Forms.RadioButton()
        Me.chkDuplex = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.nudCopies = New System.Windows.Forms.NumericUpDown()
        Me.cmbTrays = New System.Windows.Forms.ComboBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.PanelTop.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nudCopies, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelTop
        '
        Me.PanelTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(218, Byte), Integer), CType(CType(227, Byte), Integer), CType(CType(236, Byte), Integer))
        Me.PanelTop.Controls.Add(Me.Panel5)
        Me.PanelTop.Controls.Add(Me.Panel4)
        Me.PanelTop.Controls.Add(Me.Panel3)
        Me.PanelTop.Controls.Add(Me.Panel2)
        Me.PanelTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTop.Location = New System.Drawing.Point(211, 0)
        Me.PanelTop.Name = "PanelTop"
        Me.PanelTop.Size = New System.Drawing.Size(1275, 122)
        Me.PanelTop.TabIndex = 1
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Panel5.Controls.Add(Me.txtSubjBook)
        Me.Panel5.Controls.Add(Me.Label9)
        Me.Panel5.Controls.Add(Me.cmbYear)
        Me.Panel5.Controls.Add(Me.Label7)
        Me.Panel5.Controls.Add(Me.cmbReportType)
        Me.Panel5.Controls.Add(Me.Label1)
        Me.Panel5.Location = New System.Drawing.Point(261, 8)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(359, 107)
        Me.Panel5.TabIndex = 18
        '
        'txtSubjBook
        '
        Me.txtSubjBook.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.txtSubjBook.Location = New System.Drawing.Point(16, 38)
        Me.txtSubjBook.Name = "txtSubjBook"
        Me.txtSubjBook.Size = New System.Drawing.Size(229, 27)
        Me.txtSubjBook.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Label9.Font = New System.Drawing.Font("Cairo", 9.0!)
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(251, 38)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(98, 27)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "موضوع الكتاب :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbYear
        '
        Me.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbYear.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.cmbYear.Location = New System.Drawing.Point(16, 72)
        Me.cmbYear.Name = "cmbYear"
        Me.cmbYear.Size = New System.Drawing.Size(229, 28)
        Me.cmbYear.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Label7.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(265, 73)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 27)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "السنة :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbReportType
        '
        Me.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReportType.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.cmbReportType.Location = New System.Drawing.Point(16, 3)
        Me.cmbReportType.Name = "cmbReportType"
        Me.cmbReportType.Size = New System.Drawing.Size(229, 28)
        Me.cmbReportType.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.cmbReportType, "يمكنك اختيار نوع التقرير المراد طباعتة")
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(265, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 27)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "نوع التقرير"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Controls.Add(Me.txtWillMaster)
        Me.Panel4.Controls.Add(Me.txtNumberBook)
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Controls.Add(Me.Label12)
        Me.Panel4.Controls.Add(Me.txtWillSub)
        Me.Panel4.Location = New System.Drawing.Point(626, 8)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(335, 108)
        Me.Panel4.TabIndex = 18
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Label8.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(234, 10)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(90, 27)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "رقم الكتاب :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWillMaster
        '
        Me.txtWillMaster.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.txtWillMaster.Location = New System.Drawing.Point(14, 43)
        Me.txtWillMaster.Name = "txtWillMaster"
        Me.txtWillMaster.Size = New System.Drawing.Size(220, 27)
        Me.txtWillMaster.TabIndex = 4
        '
        'txtNumberBook
        '
        Me.txtNumberBook.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.txtNumberBook.Location = New System.Drawing.Point(14, 10)
        Me.txtNumberBook.Name = "txtNumberBook"
        Me.txtNumberBook.Size = New System.Drawing.Size(220, 27)
        Me.txtNumberBook.TabIndex = 10
        Me.txtNumberBook.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.txtNumberBook, "اكتب رقم الكتاب المراد طباعة تقريره")
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Label10.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(234, 43)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(90, 27)
        Me.Label10.TabIndex = 13
        Me.Label10.Text = "البوليصة الرئيسية :"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Label12.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.Label12.ForeColor = System.Drawing.Color.Black
        Me.Label12.Location = New System.Drawing.Point(234, 76)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(90, 27)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "البوليصة الفرعية :"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWillSub
        '
        Me.txtWillSub.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.txtWillSub.Location = New System.Drawing.Point(14, 76)
        Me.txtWillSub.Name = "txtWillSub"
        Me.txtWillSub.Size = New System.Drawing.Size(220, 27)
        Me.txtWillSub.TabIndex = 15
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Panel3.Controls.Add(Me.dtpFrom)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.dtpTo)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Location = New System.Drawing.Point(967, 8)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(298, 106)
        Me.Panel3.TabIndex = 18
        '
        'dtpFrom
        '
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFrom.Location = New System.Drawing.Point(15, 17)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(179, 32)
        Me.dtpFrom.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(195, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 27)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "من تاريخ :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpTo
        '
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTo.Location = New System.Drawing.Point(15, 55)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(179, 32)
        Me.dtpTo.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(195, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 27)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "إلى تاريخ :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Panel2.Controls.Add(Me.btnResetFilters)
        Me.Panel2.Controls.Add(Me.btnPreview)
        Me.Panel2.Controls.Add(Me.btnExportPdf)
        Me.Panel2.Location = New System.Drawing.Point(12, 8)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(243, 107)
        Me.Panel2.TabIndex = 17
        '
        'btnResetFilters
        '
        Me.btnResetFilters.BackColor = System.Drawing.Color.DarkSlateGray
        Me.btnResetFilters.FlatAppearance.BorderSize = 0
        Me.btnResetFilters.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnResetFilters.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.btnResetFilters.ForeColor = System.Drawing.Color.White
        Me.btnResetFilters.Location = New System.Drawing.Point(23, 74)
        Me.btnResetFilters.Name = "btnResetFilters"
        Me.btnResetFilters.Size = New System.Drawing.Size(197, 26)
        Me.btnResetFilters.TabIndex = 8
        Me.btnResetFilters.Text = "إعادة تعيين الفلاتر 🔄"
        Me.btnResetFilters.UseVisualStyleBackColor = False
        '
        'btnPreview
        '
        Me.btnPreview.BackColor = System.Drawing.Color.FromArgb(CType(CType(76, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.btnPreview.FlatAppearance.BorderSize = 0
        Me.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnPreview.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.btnPreview.ForeColor = System.Drawing.Color.White
        Me.btnPreview.Location = New System.Drawing.Point(23, 40)
        Me.btnPreview.Name = "btnPreview"
        Me.btnPreview.Size = New System.Drawing.Size(197, 26)
        Me.btnPreview.TabIndex = 6
        Me.btnPreview.Text = "عرض التقرير  📝"
        Me.btnPreview.UseVisualStyleBackColor = False
        '
        'btnExportPdf
        '
        Me.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(67, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnExportPdf.Font = New System.Drawing.Font("Cairo", 8.0!)
        Me.btnExportPdf.ForeColor = System.Drawing.Color.White
        Me.btnExportPdf.Location = New System.Drawing.Point(23, 6)
        Me.btnExportPdf.Name = "btnExportPdf"
        Me.btnExportPdf.Size = New System.Drawing.Size(197, 26)
        Me.btnExportPdf.TabIndex = 7
        Me.btnExportPdf.Text = "تصدير PDF : 📄"
        Me.btnExportPdf.UseVisualStyleBackColor = False
        '
        'ListPrinter
        '
        Me.ListPrinter.FormattingEnabled = True
        Me.ListPrinter.Location = New System.Drawing.Point(5, 159)
        Me.ListPrinter.Name = "ListPrinter"
        Me.ListPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ListPrinter.ScrollAlwaysVisible = True
        Me.ListPrinter.Size = New System.Drawing.Size(205, 193)
        Me.ListPrinter.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.ListPrinter, "إخيار الطابعة المناسبة")
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(82, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.btnPrint.Enabled = False
        Me.btnPrint.FlatAppearance.BorderSize = 0
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.ForeColor = System.Drawing.Color.White
        Me.btnPrint.Location = New System.Drawing.Point(7, 68)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(197, 40)
        Me.btnPrint.TabIndex = 8
        Me.btnPrint.Text = "طباعة التقرير 🖨️"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'pdfViewer
        '
        Me.pdfViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pdfViewer.Location = New System.Drawing.Point(211, 122)
        Me.pdfViewer.Name = "pdfViewer"
        Me.pdfViewer.ShowToolbar = False
        Me.pdfViewer.Size = New System.Drawing.Size(1275, 639)
        Me.pdfViewer.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.chkDuplex)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.nudCopies)
        Me.Panel1.Controls.Add(Me.cmbTrays)
        Me.Panel1.Controls.Add(Me.ListPrinter)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(211, 761)
        Me.Panel1.TabIndex = 2
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label14.ForeColor = System.Drawing.Color.Black
        Me.Label14.Location = New System.Drawing.Point(101, 355)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(107, 30)
        Me.Label14.TabIndex = 16
        Me.Label14.Text = "إعدادات إضافية"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label13.ForeColor = System.Drawing.Color.Black
        Me.Label13.Location = New System.Drawing.Point(5, 415)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(70, 30)
        Me.Label13.TabIndex = 16
        Me.Label13.Text = "نسخة"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Cairo", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(38, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(140, 30)
        Me.Label11.TabIndex = 15
        Me.Label11.Text = "إعدادات الطباعة"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(218, Byte), Integer), CType(CType(227, Byte), Integer), CType(CType(236, Byte), Integer))
        Me.GroupBox1.Controls.Add(Me.txtPages)
        Me.GroupBox1.Controls.Add(Me.RBPrintCustom)
        Me.GroupBox1.Controls.Add(Me.RBPrintCurrent)
        Me.GroupBox1.Controls.Add(Me.RBPrintAll)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 512)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(198, 202)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "خيارات الصـفحات"
        '
        'txtPages
        '
        Me.txtPages.Enabled = False
        Me.txtPages.Location = New System.Drawing.Point(6, 159)
        Me.txtPages.Name = "txtPages"
        Me.txtPages.Size = New System.Drawing.Size(179, 32)
        Me.txtPages.TabIndex = 3
        Me.txtPages.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.txtPages, "يمكن تخصيص صفحات معينة لطباعتها ")
        '
        'RBPrintCustom
        '
        Me.RBPrintCustom.AutoSize = True
        Me.RBPrintCustom.Location = New System.Drawing.Point(55, 118)
        Me.RBPrintCustom.Name = "RBPrintCustom"
        Me.RBPrintCustom.Size = New System.Drawing.Size(130, 30)
        Me.RBPrintCustom.TabIndex = 2
        Me.RBPrintCustom.Text = "تخصيص الصفحات"
        Me.RBPrintCustom.UseVisualStyleBackColor = True
        '
        'RBPrintCurrent
        '
        Me.RBPrintCurrent.AutoSize = True
        Me.RBPrintCurrent.Location = New System.Drawing.Point(59, 77)
        Me.RBPrintCurrent.Name = "RBPrintCurrent"
        Me.RBPrintCurrent.Size = New System.Drawing.Size(126, 30)
        Me.RBPrintCurrent.TabIndex = 1
        Me.RBPrintCurrent.Text = "الصفحـة الحــالية"
        Me.RBPrintCurrent.UseVisualStyleBackColor = True
        '
        'RBPrintAll
        '
        Me.RBPrintAll.AutoSize = True
        Me.RBPrintAll.Checked = True
        Me.RBPrintAll.Location = New System.Drawing.Point(65, 36)
        Me.RBPrintAll.Name = "RBPrintAll"
        Me.RBPrintAll.Size = New System.Drawing.Size(120, 30)
        Me.RBPrintAll.TabIndex = 0
        Me.RBPrintAll.TabStop = True
        Me.RBPrintAll.Text = "كـل الصـفحــات"
        Me.RBPrintAll.UseVisualStyleBackColor = True
        '
        'chkDuplex
        '
        Me.chkDuplex.AutoSize = True
        Me.chkDuplex.Location = New System.Drawing.Point(43, 720)
        Me.chkDuplex.Name = "chkDuplex"
        Me.chkDuplex.Size = New System.Drawing.Size(149, 30)
        Me.chkDuplex.TabIndex = 13
        Me.chkDuplex.Text = "الطباعة على الوجهين"
        Me.chkDuplex.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(142, 126)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 30)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "الطابعات"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(160, 448)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 23)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "الدرج"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(138, 385)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 30)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "عدد النسخ"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'nudCopies
        '
        Me.nudCopies.Location = New System.Drawing.Point(84, 415)
        Me.nudCopies.Name = "nudCopies"
        Me.nudCopies.Size = New System.Drawing.Size(117, 32)
        Me.nudCopies.TabIndex = 10
        Me.nudCopies.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.nudCopies, "إختيار عدد النسخ للطباعة")
        Me.nudCopies.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cmbTrays
        '
        Me.cmbTrays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTrays.Location = New System.Drawing.Point(5, 474)
        Me.cmbTrays.Name = "cmbTrays"
        Me.cmbTrays.Size = New System.Drawing.Size(198, 32)
        Me.cmbTrays.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.cmbTrays, "إختيار درج الورق ")
        '
        'FrmReports
        '
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1486, 761)
        Me.Controls.Add(Me.pdfViewer)
        Me.Controls.Add(Me.PanelTop)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Cairo", 10.0!)
        Me.MinimumSize = New System.Drawing.Size(1502, 800)
        Me.Name = "FrmReports"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "التقارير"
        Me.PanelTop.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nudCopies, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnPrint As Button
    Friend WithEvents ListPrinter As CheckedListBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents cmbTrays As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents nudCopies As NumericUpDown
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents cmbYear As ComboBox
    Friend WithEvents txtNumberBook As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents chkDuplex As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtPages As TextBox
    Friend WithEvents RBPrintCustom As RadioButton
    Friend WithEvents RBPrintCurrent As RadioButton
    Friend WithEvents RBPrintAll As RadioButton
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents txtWillSub As TextBox
    Friend WithEvents txtSubjBook As TextBox
    Friend WithEvents txtWillMaster As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label13 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents btnResetFilters As Button
End Class



'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
'Partial Class frmReports
'    Inherits BaseFormWithPermissions

'    'Form overrides dispose to clean up the component list.
'    <System.Diagnostics.DebuggerNonUserCode()>
'    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
'        Try
'            If disposing AndAlso components IsNot Nothing Then
'                components.Dispose()
'            End If
'        Finally
'            MyBase.Dispose(disposing)
'        End Try
'    End Sub

'    'Required by the Windows Form Designer
'    Private components As System.ComponentModel.IContainer

'    'NOTE: The following procedure is required by the Windows Form Designer
'    'It can be modified using the Windows Form Designer.
'    'Do not modify it using the code editor.
'    <System.Diagnostics.DebuggerStepThrough()>
'    Private Sub InitializeComponent()
'        Me.gbFilters = New System.Windows.Forms.GroupBox()
'        Me.btnRefreshYears = New System.Windows.Forms.Button()
'        Me.cmbYear = New System.Windows.Forms.ComboBox()
'        Me.Label1 = New System.Windows.Forms.Label()
'        Me.tlpButtons = New System.Windows.Forms.TableLayoutPanel()
'        Me.btnSaderAll = New System.Windows.Forms.Button()
'        Me.btnSaderByYear = New System.Windows.Forms.Button()
'        Me.btnArchiveAll = New System.Windows.Forms.Button()
'        Me.btnArchiveByYear = New System.Windows.Forms.Button()
'        Me.btnUnarchived = New System.Windows.Forms.Button()
'        Me.btnExportAll = New System.Windows.Forms.Button()
'        Me.chkPreview = New System.Windows.Forms.CheckBox()
'        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
'        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
'        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
'        Me.gbFilters.SuspendLayout()
'        Me.tlpButtons.SuspendLayout()
'        Me.StatusStrip1.SuspendLayout()
'        Me.SuspendLayout()
'        '
'        'gbFilters
'        '
'        Me.gbFilters.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.gbFilters.Controls.Add(Me.btnRefreshYears)
'        Me.gbFilters.Controls.Add(Me.cmbYear)
'        Me.gbFilters.Controls.Add(Me.Label1)
'        Me.gbFilters.Font = New System.Drawing.Font("Cairo", 9.75!, System.Drawing.FontStyle.Bold)
'        Me.gbFilters.Location = New System.Drawing.Point(12, 12)
'        Me.gbFilters.Name = "gbFilters"
'        Me.gbFilters.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.gbFilters.Size = New System.Drawing.Size(776, 70)
'        Me.gbFilters.TabIndex = 0
'        Me.gbFilters.TabStop = False
'        Me.gbFilters.Text = "خيارات التقرير"
'        '
'        'btnRefreshYears
'        '
'        Me.btnRefreshYears.Anchor = System.Windows.Forms.AnchorStyles.Top
'        Me.btnRefreshYears.BackColor = System.Drawing.Color.SteelBlue
'        Me.btnRefreshYears.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnRefreshYears.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Bold)
'        Me.btnRefreshYears.ForeColor = System.Drawing.Color.White
'        Me.btnRefreshYears.Location = New System.Drawing.Point(446, 28)
'        Me.btnRefreshYears.Name = "btnRefreshYears"
'        Me.btnRefreshYears.Size = New System.Drawing.Size(100, 30)
'        Me.btnRefreshYears.TabIndex = 2
'        Me.btnRefreshYears.Text = "تحديث السنوات"
'        Me.btnRefreshYears.UseVisualStyleBackColor = False
'        '
'        'cmbYear
'        '
'        Me.cmbYear.Anchor = System.Windows.Forms.AnchorStyles.Top
'        Me.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
'        Me.cmbYear.Font = New System.Drawing.Font("Cairo", 9.75!)
'        Me.cmbYear.FormattingEnabled = True
'        Me.cmbYear.Location = New System.Drawing.Point(566, 27)
'        Me.cmbYear.Name = "cmbYear"
'        Me.cmbYear.Size = New System.Drawing.Size(130, 32)
'        Me.cmbYear.TabIndex = 1
'        '
'        'Label1
'        '
'        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Top
'        Me.Label1.AutoSize = True
'        Me.Label1.Font = New System.Drawing.Font("Cairo", 9.75!)
'        Me.Label1.Location = New System.Drawing.Point(702, 31)
'        Me.Label1.Name = "Label1"
'        Me.Label1.Size = New System.Drawing.Size(64, 24)
'        Me.Label1.TabIndex = 0
'        Me.Label1.Text = "اختر السنة:"
'        '
'        'tlpButtons
'        '
'        Me.tlpButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.tlpButtons.ColumnCount = 3
'        Me.tlpButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
'        Me.tlpButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
'        Me.tlpButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
'        Me.tlpButtons.Controls.Add(Me.btnSaderAll, 0, 0)
'        Me.tlpButtons.Controls.Add(Me.btnSaderByYear, 1, 0)
'        Me.tlpButtons.Controls.Add(Me.btnArchiveAll, 2, 0)
'        Me.tlpButtons.Controls.Add(Me.btnArchiveByYear, 0, 1)
'        Me.tlpButtons.Controls.Add(Me.btnUnarchived, 1, 1)
'        Me.tlpButtons.Controls.Add(Me.btnExportAll, 2, 1)
'        Me.tlpButtons.Location = New System.Drawing.Point(12, 100)
'        Me.tlpButtons.Name = "tlpButtons"
'        Me.tlpButtons.RowCount = 2
'        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
'        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
'        Me.tlpButtons.Size = New System.Drawing.Size(776, 100)
'        Me.tlpButtons.TabIndex = 1
'        '
'        'btnSaderAll
'        '
'        Me.btnSaderAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
'        Me.btnSaderAll.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.btnSaderAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnSaderAll.Font = New System.Drawing.Font("Cairo", 12.0!, System.Drawing.FontStyle.Bold)
'        Me.btnSaderAll.ForeColor = System.Drawing.Color.White
'        Me.btnSaderAll.Location = New System.Drawing.Point(521, 3)
'        Me.btnSaderAll.Name = "btnSaderAll"
'        Me.btnSaderAll.Size = New System.Drawing.Size(252, 44)
'        Me.btnSaderAll.TabIndex = 0
'        Me.btnSaderAll.Text = "📄 تقرير الصادر (الكل)"
'        Me.btnSaderAll.UseVisualStyleBackColor = False
'        '
'        'btnSaderByYear
'        '
'        Me.btnSaderByYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(204, Byte), Integer))
'        Me.btnSaderByYear.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.btnSaderByYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnSaderByYear.Font = New System.Drawing.Font("Cairo", 12.0!, System.Drawing.FontStyle.Bold)
'        Me.btnSaderByYear.ForeColor = System.Drawing.Color.White
'        Me.btnSaderByYear.Location = New System.Drawing.Point(263, 3)
'        Me.btnSaderByYear.Name = "btnSaderByYear"
'        Me.btnSaderByYear.Size = New System.Drawing.Size(252, 44)
'        Me.btnSaderByYear.TabIndex = 1
'        Me.btnSaderByYear.Text = "🗓️ تقرير الصادر حسب السنة"
'        Me.btnSaderByYear.UseVisualStyleBackColor = False
'        '
'        'btnArchiveAll
'        '
'        Me.btnArchiveAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
'        Me.btnArchiveAll.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.btnArchiveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnArchiveAll.Font = New System.Drawing.Font("Cairo", 12.0!, System.Drawing.FontStyle.Bold)
'        Me.btnArchiveAll.ForeColor = System.Drawing.Color.White
'        Me.btnArchiveAll.Location = New System.Drawing.Point(3, 3)
'        Me.btnArchiveAll.Name = "btnArchiveAll"
'        Me.btnArchiveAll.Size = New System.Drawing.Size(254, 44)
'        Me.btnArchiveAll.TabIndex = 2
'        Me.btnArchiveAll.Text = "📁 تقرير الأرشيف (الكل)"
'        Me.btnArchiveAll.UseVisualStyleBackColor = False
'        '
'        'btnArchiveByYear
'        '
'        Me.btnArchiveByYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
'        Me.btnArchiveByYear.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.btnArchiveByYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnArchiveByYear.Font = New System.Drawing.Font("Cairo", 12.0!, System.Drawing.FontStyle.Bold)
'        Me.btnArchiveByYear.ForeColor = System.Drawing.Color.White
'        Me.btnArchiveByYear.Location = New System.Drawing.Point(521, 53)
'        Me.btnArchiveByYear.Name = "btnArchiveByYear"
'        Me.btnArchiveByYear.Size = New System.Drawing.Size(252, 44)
'        Me.btnArchiveByYear.TabIndex = 3
'        Me.btnArchiveByYear.Text = "🗓️ تقرير الأرشيف حسب السنة"
'        Me.btnArchiveByYear.UseVisualStyleBackColor = False
'        '
'        'btnUnarchived
'        '
'        Me.btnUnarchived.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(34, Byte), Integer))
'        Me.btnUnarchived.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.btnUnarchived.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnUnarchived.Font = New System.Drawing.Font("Cairo", 12.0!, System.Drawing.FontStyle.Bold)
'        Me.btnUnarchived.ForeColor = System.Drawing.Color.White
'        Me.btnUnarchived.Location = New System.Drawing.Point(263, 53)
'        Me.btnUnarchived.Name = "btnUnarchived"
'        Me.btnUnarchived.Size = New System.Drawing.Size(252, 44)
'        Me.btnUnarchived.TabIndex = 4
'        Me.btnUnarchived.Text = "⚠️ تقرير غير المؤرشف"
'        Me.btnUnarchived.UseVisualStyleBackColor = False
'        '
'        'btnExportAll
'        '
'        Me.btnExportAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
'        Me.btnExportAll.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.btnExportAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
'        Me.btnExportAll.Font = New System.Drawing.Font("Cairo", 12.0!, System.Drawing.FontStyle.Bold)
'        Me.btnExportAll.ForeColor = System.Drawing.Color.White
'        Me.btnExportAll.Location = New System.Drawing.Point(3, 53)
'        Me.btnExportAll.Name = "btnExportAll"
'        Me.btnExportAll.Size = New System.Drawing.Size(254, 44)
'        Me.btnExportAll.TabIndex = 5
'        Me.btnExportAll.Text = "💾 تصدير الكل إلى PDF"
'        Me.btnExportAll.UseVisualStyleBackColor = False
'        '
'        'chkPreview
'        '
'        Me.chkPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.chkPreview.AutoSize = True
'        Me.chkPreview.Checked = True
'        Me.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked
'        Me.chkPreview.Font = New System.Drawing.Font("Cairo", 9.75!)
'        Me.chkPreview.Location = New System.Drawing.Point(612, 215)
'        Me.chkPreview.Name = "chkPreview"
'        Me.chkPreview.Size = New System.Drawing.Size(176, 28)
'        Me.chkPreview.TabIndex = 2
'        Me.chkPreview.Text = "عرض التقرير في المعاينة أولاً"
'        Me.chkPreview.UseVisualStyleBackColor = True
'        '
'        'StatusStrip1
'        '
'        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
'        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus})
'        Me.StatusStrip1.Location = New System.Drawing.Point(0, 279)
'        Me.StatusStrip1.Name = "StatusStrip1"
'        Me.StatusStrip1.Size = New System.Drawing.Size(800, 22)
'        Me.StatusStrip1.TabIndex = 3
'        Me.StatusStrip1.Text = "StatusStrip1"
'        '
'        'lblStatus
'        '
'        Me.lblStatus.Name = "lblStatus"
'        Me.lblStatus.Size = New System.Drawing.Size(102, 17)
'        Me.lblStatus.Text = "جاهز لعرض التقارير"
'        '
'        'ProgressBar1
'        '
'        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.ProgressBar1.Location = New System.Drawing.Point(12, 240)
'        Me.ProgressBar1.Name = "ProgressBar1"
'        Me.ProgressBar1.Size = New System.Drawing.Size(776, 23)
'        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee
'        Me.ProgressBar1.TabIndex = 4
'        Me.ProgressBar1.Visible = False
'        '
'        'frmReports
'        '
'        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 23.0!)
'        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
'        Me.ClientSize = New System.Drawing.Size(800, 301)
'        Me.Controls.Add(Me.ProgressBar1)
'        Me.Controls.Add(Me.StatusStrip1)
'        Me.Controls.Add(Me.chkPreview)
'        Me.Controls.Add(Me.tlpButtons)
'        Me.Controls.Add(Me.gbFilters)
'        Me.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
'        Me.MaximizeBox = False
'        Me.MinimizeBox = False
'        Me.Name = "frmReports"
'        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.RightToLeftLayout = True
'        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
'        Me.Text = "تقارير النظام - ESIS"
'        Me.gbFilters.ResumeLayout(False)
'        Me.gbFilters.PerformLayout()
'        Me.tlpButtons.ResumeLayout(False)
'        Me.StatusStrip1.ResumeLayout(False)
'        Me.StatusStrip1.PerformLayout()
'        Me.ResumeLayout(False)
'        Me.PerformLayout()

'    End Sub

'    Friend WithEvents gbFilters As GroupBox
'    Friend WithEvents btnRefreshYears As Button
'    Friend WithEvents cmbYear As ComboBox
'    Friend WithEvents Label1 As Label
'    Friend WithEvents tlpButtons As TableLayoutPanel
'    Friend WithEvents btnSaderAll As Button
'    Friend WithEvents btnSaderByYear As Button
'    Friend WithEvents btnArchiveAll As Button
'    Friend WithEvents btnArchiveByYear As Button
'    Friend WithEvents btnUnarchived As Button
'    Friend WithEvents btnExportAll As Button
'    Friend WithEvents chkPreview As CheckBox
'    Friend WithEvents StatusStrip1 As StatusStrip
'    Friend WithEvents lblStatus As ToolStripStatusLabel
'    Friend WithEvents ProgressBar1 As ProgressBar

'End Class