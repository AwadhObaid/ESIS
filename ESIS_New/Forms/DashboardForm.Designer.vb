<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DashboardForm
    Inherits BaseFormWithPermissions

    Private components As System.ComponentModel.IContainer

    Friend WithEvents PanelMain As Panel
    Friend WithEvents PanelHeader As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblStatus As Label

    Friend WithEvents PanelCards As FlowLayoutPanel

    Friend WithEvents cardOutgoing As Panel
    Friend WithEvents lblOutgoingCount As Label
    Friend WithEvents lblOutgoingTitle As Label

    Friend WithEvents cardArchived As Panel
    Friend WithEvents lblArchivedCount As Label
    Friend WithEvents lblArchivedTitle As Label

    Friend WithEvents cardUnarchived As Panel
    Friend WithEvents lblUnarchivedCount As Label
    Friend WithEvents lblUnarchivedTitle As Label

    Friend WithEvents cardDeleted As Panel
    Friend WithEvents lblDeletedCount As Label
    Friend WithEvents lblDeletedTitle As Label

    Friend WithEvents PanelCharts As TableLayoutPanel
    Friend WithEvents chartArchiveOverview As LiveCharts.WinForms.PieChart
    Friend WithEvents chartArchiveByFolder As LiveCharts.WinForms.CartesianChart
    Friend WithEvents chartMonthlyTrend As LiveCharts.WinForms.CartesianChart

    Friend WithEvents TimerRefresh As Timer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PanelMain = New System.Windows.Forms.Panel()
        Me.PanelCharts = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelChart2 = New System.Windows.Forms.Panel()
        Me.lblChart2Title = New System.Windows.Forms.Label()
        Me.chartArchiveByFolder = New LiveCharts.WinForms.CartesianChart()
        Me.PanelChart1 = New System.Windows.Forms.Panel()
        Me.lblChart1Title = New System.Windows.Forms.Label()
        Me.chartArchiveOverview = New LiveCharts.WinForms.PieChart()
        Me.PanelChart3 = New System.Windows.Forms.Panel()
        Me.lblChart3Title = New System.Windows.Forms.Label()
        Me.chartMonthlyTrend = New LiveCharts.WinForms.CartesianChart()
        Me.PanelSummary = New System.Windows.Forms.Panel()
        Me.PanelDeleted = New System.Windows.Forms.Panel()
        Me.lblDeletedCount = New System.Windows.Forms.Label()
        Me.lblDeletedTitle = New System.Windows.Forms.Label()
        Me.PanelOutgoing = New System.Windows.Forms.Panel()
        Me.lblOutgoingCount = New System.Windows.Forms.Label()
        Me.lblOutgoingTitle = New System.Windows.Forms.Label()
        Me.PanelArchived = New System.Windows.Forms.Panel()
        Me.lblArchivedCount = New System.Windows.Forms.Label()
        Me.lblArchivedTitle = New System.Windows.Forms.Label()
        Me.PanelUnarchived = New System.Windows.Forms.Panel()
        Me.lblUnarchivedCount = New System.Windows.Forms.Label()
        Me.lblUnarchivedTitle = New System.Windows.Forms.Label()
        Me.PanelTitle = New System.Windows.Forms.Panel()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.PanelMain.SuspendLayout()
        Me.PanelCharts.SuspendLayout()
        Me.PanelChart2.SuspendLayout()
        Me.PanelChart1.SuspendLayout()
        Me.PanelChart3.SuspendLayout()
        Me.PanelSummary.SuspendLayout()
        Me.PanelDeleted.SuspendLayout()
        Me.PanelOutgoing.SuspendLayout()
        Me.PanelArchived.SuspendLayout()
        Me.PanelUnarchived.SuspendLayout()
        Me.PanelTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelMain
        '
        Me.PanelMain.Controls.Add(Me.PanelCharts)
        Me.PanelMain.Controls.Add(Me.PanelSummary)
        Me.PanelMain.Controls.Add(Me.PanelTitle)
        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelMain.Location = New System.Drawing.Point(0, 0)
        Me.PanelMain.Name = "PanelMain"
        Me.PanelMain.Size = New System.Drawing.Size(1284, 701)
        Me.PanelMain.TabIndex = 0
        '
        'PanelCharts
        '
        Me.PanelCharts.ColumnCount = 3
        Me.PanelCharts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.PanelCharts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.PanelCharts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.PanelCharts.Controls.Add(Me.PanelChart2, 1, 0)
        Me.PanelCharts.Controls.Add(Me.PanelChart1, 0, 0)
        Me.PanelCharts.Controls.Add(Me.PanelChart3, 2, 0)
        Me.PanelCharts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelCharts.Location = New System.Drawing.Point(0, 150)
        Me.PanelCharts.Name = "PanelCharts"
        Me.PanelCharts.RowCount = 1
        Me.PanelCharts.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.PanelCharts.Size = New System.Drawing.Size(1284, 551)
        Me.PanelCharts.TabIndex = 2
        '
        'PanelChart2
        '
        Me.PanelChart2.Controls.Add(Me.lblChart2Title)
        Me.PanelChart2.Controls.Add(Me.chartArchiveByFolder)
        Me.PanelChart2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelChart2.Location = New System.Drawing.Point(431, 3)
        Me.PanelChart2.Name = "PanelChart2"
        Me.PanelChart2.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelChart2.Size = New System.Drawing.Size(422, 545)
        Me.PanelChart2.TabIndex = 1
        '
        'lblChart2Title
        '
        Me.lblChart2Title.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblChart2Title.Font = New System.Drawing.Font("Cairo", 11.25!)
        Me.lblChart2Title.Location = New System.Drawing.Point(10, 10)
        Me.lblChart2Title.Name = "lblChart2Title"
        Me.lblChart2Title.Size = New System.Drawing.Size(402, 30)
        Me.lblChart2Title.TabIndex = 1
        Me.lblChart2Title.Text = "توزيع الأرشيف حسب المجلدات"
        Me.lblChart2Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chartArchiveByFolder
        '
        Me.chartArchiveByFolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chartArchiveByFolder.Font = New System.Drawing.Font("Cairo Medium", 11.25!, System.Drawing.FontStyle.Bold)
        Me.chartArchiveByFolder.Location = New System.Drawing.Point(10, 10)
        Me.chartArchiveByFolder.Name = "chartArchiveByFolder"
        Me.chartArchiveByFolder.Size = New System.Drawing.Size(402, 525)
        Me.chartArchiveByFolder.TabIndex = 0
        '
        'PanelChart1
        '
        Me.PanelChart1.Controls.Add(Me.lblChart1Title)
        Me.PanelChart1.Controls.Add(Me.chartArchiveOverview)
        Me.PanelChart1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelChart1.Location = New System.Drawing.Point(859, 3)
        Me.PanelChart1.Name = "PanelChart1"
        Me.PanelChart1.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelChart1.Size = New System.Drawing.Size(422, 545)
        Me.PanelChart1.TabIndex = 0
        '
        'lblChart1Title
        '
        Me.lblChart1Title.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblChart1Title.Font = New System.Drawing.Font("Cairo", 11.25!)
        Me.lblChart1Title.Location = New System.Drawing.Point(10, 10)
        Me.lblChart1Title.Name = "lblChart1Title"
        Me.lblChart1Title.Size = New System.Drawing.Size(402, 30)
        Me.lblChart1Title.TabIndex = 1
        Me.lblChart1Title.Text = "نظرة عامة على الأرشيف"
        Me.lblChart1Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chartArchiveOverview
        '
        Me.chartArchiveOverview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chartArchiveOverview.Font = New System.Drawing.Font("Cairo Medium", 11.25!, System.Drawing.FontStyle.Bold)
        Me.chartArchiveOverview.Location = New System.Drawing.Point(10, 10)
        Me.chartArchiveOverview.Name = "chartArchiveOverview"
        Me.chartArchiveOverview.Size = New System.Drawing.Size(402, 525)
        Me.chartArchiveOverview.TabIndex = 0
        '
        'PanelChart3
        '
        Me.PanelChart3.Controls.Add(Me.lblChart3Title)
        Me.PanelChart3.Controls.Add(Me.chartMonthlyTrend)
        Me.PanelChart3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelChart3.Location = New System.Drawing.Point(3, 3)
        Me.PanelChart3.Name = "PanelChart3"
        Me.PanelChart3.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelChart3.Size = New System.Drawing.Size(422, 545)
        Me.PanelChart3.TabIndex = 2
        '
        'lblChart3Title
        '
        Me.lblChart3Title.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblChart3Title.Font = New System.Drawing.Font("Cairo", 11.25!)
        Me.lblChart3Title.Location = New System.Drawing.Point(10, 10)
        Me.lblChart3Title.Name = "lblChart3Title"
        Me.lblChart3Title.Size = New System.Drawing.Size(402, 30)
        Me.lblChart3Title.TabIndex = 1
        Me.lblChart3Title.Text = "الاتجاه الشهري للأرشيف"
        Me.lblChart3Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chartMonthlyTrend
        '
        Me.chartMonthlyTrend.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chartMonthlyTrend.Font = New System.Drawing.Font("Cairo Medium", 11.25!, System.Drawing.FontStyle.Bold)
        Me.chartMonthlyTrend.Location = New System.Drawing.Point(10, 10)
        Me.chartMonthlyTrend.Name = "chartMonthlyTrend"
        Me.chartMonthlyTrend.Size = New System.Drawing.Size(402, 525)
        Me.chartMonthlyTrend.TabIndex = 0
        '
        'PanelSummary
        '
        Me.PanelSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(162, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(216, Byte), Integer))
        Me.PanelSummary.Controls.Add(Me.PanelDeleted)
        Me.PanelSummary.Controls.Add(Me.PanelOutgoing)
        Me.PanelSummary.Controls.Add(Me.PanelArchived)
        Me.PanelSummary.Controls.Add(Me.PanelUnarchived)
        Me.PanelSummary.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelSummary.Location = New System.Drawing.Point(0, 50)
        Me.PanelSummary.Name = "PanelSummary"
        Me.PanelSummary.Size = New System.Drawing.Size(1284, 100)
        Me.PanelSummary.TabIndex = 1
        '
        'PanelDeleted
        '
        Me.PanelDeleted.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelDeleted.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.PanelDeleted.Controls.Add(Me.lblDeletedCount)
        Me.PanelDeleted.Controls.Add(Me.lblDeletedTitle)
        Me.PanelDeleted.Location = New System.Drawing.Point(-3, -1)
        Me.PanelDeleted.Name = "PanelDeleted"
        Me.PanelDeleted.Size = New System.Drawing.Size(325, 100)
        Me.PanelDeleted.TabIndex = 3
        '
        'lblDeletedCount
        '
        Me.lblDeletedCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblDeletedCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDeletedCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblDeletedCount.ForeColor = System.Drawing.Color.Red
        Me.lblDeletedCount.Location = New System.Drawing.Point(0, 30)
        Me.lblDeletedCount.Name = "lblDeletedCount"
        Me.lblDeletedCount.Size = New System.Drawing.Size(325, 70)
        Me.lblDeletedCount.TabIndex = 1
        Me.lblDeletedCount.Text = "0"
        Me.lblDeletedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDeletedTitle
        '
        Me.lblDeletedTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblDeletedTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblDeletedTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblDeletedTitle.Name = "lblDeletedTitle"
        Me.lblDeletedTitle.Size = New System.Drawing.Size(325, 30)
        Me.lblDeletedTitle.TabIndex = 0
        Me.lblDeletedTitle.Text = "المحذوفة"
        Me.lblDeletedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelOutgoing
        '
        Me.PanelOutgoing.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelOutgoing.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PanelOutgoing.Controls.Add(Me.lblOutgoingCount)
        Me.PanelOutgoing.Controls.Add(Me.lblOutgoingTitle)
        Me.PanelOutgoing.Location = New System.Drawing.Point(957, -1)
        Me.PanelOutgoing.Name = "PanelOutgoing"
        Me.PanelOutgoing.Size = New System.Drawing.Size(325, 100)
        Me.PanelOutgoing.TabIndex = 0
        '
        'lblOutgoingCount
        '
        Me.lblOutgoingCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblOutgoingCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblOutgoingCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblOutgoingCount.ForeColor = System.Drawing.Color.Blue
        Me.lblOutgoingCount.Location = New System.Drawing.Point(0, 30)
        Me.lblOutgoingCount.Name = "lblOutgoingCount"
        Me.lblOutgoingCount.Size = New System.Drawing.Size(325, 70)
        Me.lblOutgoingCount.TabIndex = 1
        Me.lblOutgoingCount.Text = "0"
        Me.lblOutgoingCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblOutgoingTitle
        '
        Me.lblOutgoingTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblOutgoingTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblOutgoingTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblOutgoingTitle.Name = "lblOutgoingTitle"
        Me.lblOutgoingTitle.Size = New System.Drawing.Size(325, 30)
        Me.lblOutgoingTitle.TabIndex = 0
        Me.lblOutgoingTitle.Text = "الصادرة"
        Me.lblOutgoingTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelArchived
        '
        Me.PanelArchived.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelArchived.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(201, Byte), Integer))
        Me.PanelArchived.Controls.Add(Me.lblArchivedCount)
        Me.PanelArchived.Controls.Add(Me.lblArchivedTitle)
        Me.PanelArchived.Location = New System.Drawing.Point(637, -1)
        Me.PanelArchived.Name = "PanelArchived"
        Me.PanelArchived.Size = New System.Drawing.Size(325, 100)
        Me.PanelArchived.TabIndex = 1
        '
        'lblArchivedCount
        '
        Me.lblArchivedCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblArchivedCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblArchivedCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblArchivedCount.ForeColor = System.Drawing.Color.Green
        Me.lblArchivedCount.Location = New System.Drawing.Point(0, 30)
        Me.lblArchivedCount.Name = "lblArchivedCount"
        Me.lblArchivedCount.Size = New System.Drawing.Size(325, 70)
        Me.lblArchivedCount.TabIndex = 1
        Me.lblArchivedCount.Text = "0"
        Me.lblArchivedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblArchivedTitle
        '
        Me.lblArchivedTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblArchivedTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblArchivedTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblArchivedTitle.Name = "lblArchivedTitle"
        Me.lblArchivedTitle.Size = New System.Drawing.Size(325, 30)
        Me.lblArchivedTitle.TabIndex = 0
        Me.lblArchivedTitle.Text = "المؤرشفة"
        Me.lblArchivedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelUnarchived
        '
        Me.PanelUnarchived.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelUnarchived.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(186, Byte), Integer))
        Me.PanelUnarchived.Controls.Add(Me.lblUnarchivedCount)
        Me.PanelUnarchived.Controls.Add(Me.lblUnarchivedTitle)
        Me.PanelUnarchived.Location = New System.Drawing.Point(317, -1)
        Me.PanelUnarchived.Name = "PanelUnarchived"
        Me.PanelUnarchived.Size = New System.Drawing.Size(325, 100)
        Me.PanelUnarchived.TabIndex = 2
        '
        'lblUnarchivedCount
        '
        Me.lblUnarchivedCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(186, Byte), Integer))
        Me.lblUnarchivedCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblUnarchivedCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblUnarchivedCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblUnarchivedCount.ForeColor = System.Drawing.Color.Orange
        Me.lblUnarchivedCount.Location = New System.Drawing.Point(0, 30)
        Me.lblUnarchivedCount.Name = "lblUnarchivedCount"
        Me.lblUnarchivedCount.Size = New System.Drawing.Size(325, 70)
        Me.lblUnarchivedCount.TabIndex = 1
        Me.lblUnarchivedCount.Text = "0"
        Me.lblUnarchivedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUnarchivedTitle
        '
        Me.lblUnarchivedTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(186, Byte), Integer))
        Me.lblUnarchivedTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblUnarchivedTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblUnarchivedTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblUnarchivedTitle.Name = "lblUnarchivedTitle"
        Me.lblUnarchivedTitle.Size = New System.Drawing.Size(325, 30)
        Me.lblUnarchivedTitle.TabIndex = 0
        Me.lblUnarchivedTitle.Text = "غير المؤرشفة"
        Me.lblUnarchivedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelTitle
        '
        Me.PanelTitle.BackColor = System.Drawing.Color.SteelBlue
        Me.PanelTitle.Controls.Add(Me.lblStatus)
        Me.PanelTitle.Controls.Add(Me.lblTitle)
        Me.PanelTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTitle.Location = New System.Drawing.Point(0, 0)
        Me.PanelTitle.Name = "PanelTitle"
        Me.PanelTitle.Size = New System.Drawing.Size(1284, 50)
        Me.PanelTitle.TabIndex = 0
        '
        'lblStatus
        '
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Left
        Me.lblStatus.Font = New System.Drawing.Font("Cairo", 11.25!)
        Me.lblStatus.ForeColor = System.Drawing.Color.White
        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(479, 50)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "جاري تحميل البيانات..."
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblStatus.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Cairo", 15.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(554, 7)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(177, 37)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "لوحة التحكم - ESIS"
        '
        'TimerRefresh
        '
        Me.TimerRefresh.Interval = 60000
        '
        'DashboardForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1284, 701)
        Me.Controls.Add(Me.PanelMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1300, 735)
        Me.Name = "DashboardForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "لوحة التحكم - ESIS"
        Me.PanelMain.ResumeLayout(False)
        Me.PanelCharts.ResumeLayout(False)
        Me.PanelChart2.ResumeLayout(False)
        Me.PanelChart1.ResumeLayout(False)
        Me.PanelChart3.ResumeLayout(False)
        Me.PanelSummary.ResumeLayout(False)
        Me.PanelDeleted.ResumeLayout(False)
        Me.PanelOutgoing.ResumeLayout(False)
        Me.PanelArchived.ResumeLayout(False)
        Me.PanelUnarchived.ResumeLayout(False)
        Me.PanelTitle.ResumeLayout(False)
        Me.PanelTitle.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelSummary As Panel
    Friend WithEvents PanelOutgoing As Panel
    Friend WithEvents PanelArchived As Panel
    Friend WithEvents PanelUnarchived As Panel
    Friend WithEvents PanelDeleted As Panel
    Friend WithEvents PanelTitle As Panel
    Friend WithEvents PanelChart2 As Panel
    Friend WithEvents lblChart2Title As Label
    Friend WithEvents PanelChart1 As Panel
    Friend WithEvents lblChart1Title As Label
    Friend WithEvents PanelChart3 As Panel
    Friend WithEvents lblChart3Title As Label
End Class





'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
'Partial Class DashboardForm
'    Inherits BaseFormWithPermissions

'    Private components As System.ComponentModel.IContainer

'    Friend WithEvents PanelMain As Panel
'    Friend WithEvents PanelHeader As Panel
'    Friend WithEvents lblTitle As Label
'    Friend WithEvents lblStatus As Label

'    Friend WithEvents PanelCards As FlowLayoutPanel

'    Friend WithEvents cardOutgoing As Panel
'    Friend WithEvents lblOutgoingCount As Label
'    Friend WithEvents lblOutgoingTitle As Label

'    Friend WithEvents cardArchived As Panel
'    Friend WithEvents lblArchivedCount As Label
'    Friend WithEvents lblArchivedTitle As Label

'    Friend WithEvents cardUnarchived As Panel
'    Friend WithEvents lblUnarchivedCount As Label
'    Friend WithEvents lblUnarchivedTitle As Label

'    Friend WithEvents cardDeleted As Panel
'    Friend WithEvents lblDeletedCount As Label
'    Friend WithEvents lblDeletedTitle As Label

'    Friend WithEvents PanelCharts As TableLayoutPanel
'    Friend WithEvents chartArchiveOverview As LiveCharts.WinForms.PieChart
'    Friend WithEvents chartArchiveByFolder As LiveCharts.WinForms.CartesianChart
'    Friend WithEvents chartMonthlyTrend As LiveCharts.WinForms.CartesianChart

'    Friend WithEvents TimerRefresh As Timer

'    <System.Diagnostics.DebuggerStepThrough()>
'    Private Sub InitializeComponent()
'        Me.components = New System.ComponentModel.Container()
'        Me.PanelMain = New System.Windows.Forms.Panel()
'        Me.Panel1 = New System.Windows.Forms.Panel()
'        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
'        Me.cardOutgoing = New System.Windows.Forms.Panel()
'        Me.cardArchived = New System.Windows.Forms.Panel()
'        Me.cardUnarchived = New System.Windows.Forms.Panel()
'        Me.cardDeleted = New System.Windows.Forms.Panel()
'        Me.Panel2 = New System.Windows.Forms.Panel()
'        Me.PanelCharts = New System.Windows.Forms.TableLayoutPanel()
'        Me.chartArchiveOverview = New LiveCharts.WinForms.PieChart()
'        Me.chartArchiveByFolder = New LiveCharts.WinForms.CartesianChart()
'        Me.chartMonthlyTrend = New LiveCharts.WinForms.CartesianChart()
'        Me.PanelHeader = New System.Windows.Forms.Panel()
'        Me.PanelCards = New System.Windows.Forms.FlowLayoutPanel()
'        Me.lblTitle = New System.Windows.Forms.Label()
'        Me.lblStatus = New System.Windows.Forms.Label()
'        Me.lblOutgoingCount = New System.Windows.Forms.Label()
'        Me.lblOutgoingTitle = New System.Windows.Forms.Label()
'        Me.lblArchivedCount = New System.Windows.Forms.Label()
'        Me.lblArchivedTitle = New System.Windows.Forms.Label()
'        Me.lblUnarchivedCount = New System.Windows.Forms.Label()
'        Me.lblUnarchivedTitle = New System.Windows.Forms.Label()
'        Me.lblDeletedCount = New System.Windows.Forms.Label()
'        Me.lblDeletedTitle = New System.Windows.Forms.Label()
'        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
'        Me.PanelMain.SuspendLayout()
'        Me.Panel1.SuspendLayout()
'        Me.TableLayoutPanel1.SuspendLayout()
'        Me.Panel2.SuspendLayout()
'        Me.PanelCharts.SuspendLayout()
'        Me.PanelHeader.SuspendLayout()
'        Me.SuspendLayout()
'        '
'        'PanelMain
'        '
'        Me.PanelMain.AutoScroll = True
'        Me.PanelMain.Controls.Add(Me.Panel1)
'        Me.PanelMain.Controls.Add(Me.Panel2)
'        Me.PanelMain.Controls.Add(Me.PanelHeader)
'        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.PanelMain.Location = New System.Drawing.Point(0, 0)
'        Me.PanelMain.Name = "PanelMain"
'        Me.PanelMain.Size = New System.Drawing.Size(1298, 622)
'        Me.PanelMain.TabIndex = 0
'        '
'        'Panel1
'        '
'        Me.Panel1.Controls.Add(Me.TableLayoutPanel1)
'        Me.Panel1.Location = New System.Drawing.Point(0, 60)
'        Me.Panel1.Name = "Panel1"
'        Me.Panel1.Size = New System.Drawing.Size(1298, 87)
'        Me.Panel1.TabIndex = 4
'        '
'        'TableLayoutPanel1
'        '
'        Me.TableLayoutPanel1.ColumnCount = 4
'        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
'        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
'        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
'        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
'        Me.TableLayoutPanel1.Controls.Add(Me.cardOutgoing, 0, 0)
'        Me.TableLayoutPanel1.Controls.Add(Me.cardArchived, 1, 0)
'        Me.TableLayoutPanel1.Controls.Add(Me.cardUnarchived, 2, 0)
'        Me.TableLayoutPanel1.Controls.Add(Me.cardDeleted, 3, 0)
'        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
'        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
'        Me.TableLayoutPanel1.RowCount = 1
'        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
'        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1298, 87)
'        Me.TableLayoutPanel1.TabIndex = 0
'        '
'        'cardOutgoing
'        '
'        Me.cardOutgoing.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.cardOutgoing.Location = New System.Drawing.Point(3, 3)
'        Me.cardOutgoing.Name = "cardOutgoing"
'        Me.cardOutgoing.Size = New System.Drawing.Size(318, 81)
'        Me.cardOutgoing.TabIndex = 0
'        '
'        'cardArchived
'        '
'        Me.cardArchived.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.cardArchived.Location = New System.Drawing.Point(327, 3)
'        Me.cardArchived.Name = "cardArchived"
'        Me.cardArchived.Size = New System.Drawing.Size(318, 81)
'        Me.cardArchived.TabIndex = 1
'        '
'        'cardUnarchived
'        '
'        Me.cardUnarchived.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.cardUnarchived.Location = New System.Drawing.Point(651, 3)
'        Me.cardUnarchived.Name = "cardUnarchived"
'        Me.cardUnarchived.Size = New System.Drawing.Size(318, 81)
'        Me.cardUnarchived.TabIndex = 2
'        '
'        'cardDeleted
'        '
'        Me.cardDeleted.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.cardDeleted.Location = New System.Drawing.Point(975, 3)
'        Me.cardDeleted.Name = "cardDeleted"
'        Me.cardDeleted.Size = New System.Drawing.Size(320, 81)
'        Me.cardDeleted.TabIndex = 3
'        '
'        'Panel2
'        '
'        Me.Panel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
'            Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.Panel2.AutoScroll = True
'        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
'        Me.Panel2.Controls.Add(Me.PanelCharts)
'        Me.Panel2.Location = New System.Drawing.Point(0, 153)
'        Me.Panel2.Name = "Panel2"
'        Me.Panel2.Size = New System.Drawing.Size(1298, 469)
'        Me.Panel2.TabIndex = 5
'        '
'        'PanelCharts
'        '
'        Me.PanelCharts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.PanelCharts.AutoScroll = True
'        Me.PanelCharts.ColumnCount = 1
'        Me.PanelCharts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
'        Me.PanelCharts.Controls.Add(Me.chartArchiveOverview, 0, 0)
'        Me.PanelCharts.Controls.Add(Me.chartArchiveByFolder, 0, 2)
'        Me.PanelCharts.Controls.Add(Me.chartMonthlyTrend, 0, 1)
'        Me.PanelCharts.Location = New System.Drawing.Point(35, 93)
'        Me.PanelCharts.Name = "PanelCharts"
'        Me.PanelCharts.RowCount = 3
'        Me.PanelCharts.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
'        Me.PanelCharts.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
'        Me.PanelCharts.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
'        Me.PanelCharts.Size = New System.Drawing.Size(1156, 1112)
'        Me.PanelCharts.TabIndex = 0
'        '
'        'chartArchiveOverview
'        '
'        Me.chartArchiveOverview.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.chartArchiveOverview.Location = New System.Drawing.Point(3, 3)
'        Me.chartArchiveOverview.Name = "chartArchiveOverview"
'        Me.chartArchiveOverview.Size = New System.Drawing.Size(1150, 364)
'        Me.chartArchiveOverview.TabIndex = 0
'        '
'        'chartArchiveByFolder
'        '
'        Me.chartArchiveByFolder.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.chartArchiveByFolder.Location = New System.Drawing.Point(3, 743)
'        Me.chartArchiveByFolder.Name = "chartArchiveByFolder"
'        Me.chartArchiveByFolder.Size = New System.Drawing.Size(1150, 366)
'        Me.chartArchiveByFolder.TabIndex = 1
'        '
'        'chartMonthlyTrend
'        '
'        Me.chartMonthlyTrend.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.chartMonthlyTrend.Location = New System.Drawing.Point(3, 373)
'        Me.chartMonthlyTrend.Name = "chartMonthlyTrend"
'        Me.chartMonthlyTrend.Size = New System.Drawing.Size(1150, 364)
'        Me.chartMonthlyTrend.TabIndex = 2
'        '
'        'PanelHeader
'        '
'        Me.PanelHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
'        Me.PanelHeader.Controls.Add(Me.PanelCards)
'        Me.PanelHeader.Controls.Add(Me.lblTitle)
'        Me.PanelHeader.Controls.Add(Me.lblStatus)
'        Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top
'        Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
'        Me.PanelHeader.Name = "PanelHeader"
'        Me.PanelHeader.Size = New System.Drawing.Size(1298, 54)
'        Me.PanelHeader.TabIndex = 2
'        '
'        'PanelCards
'        '
'        Me.PanelCards.BackColor = System.Drawing.Color.Yellow
'        Me.PanelCards.Location = New System.Drawing.Point(165, 23)
'        Me.PanelCards.Name = "PanelCards"
'        Me.PanelCards.Padding = New System.Windows.Forms.Padding(10)
'        Me.PanelCards.Size = New System.Drawing.Size(102, 19)
'        Me.PanelCards.TabIndex = 1
'        '
'        'lblTitle
'        '
'        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.lblTitle.Font = New System.Drawing.Font("Cairo", 16.0!, System.Drawing.FontStyle.Bold)
'        Me.lblTitle.ForeColor = System.Drawing.Color.White
'        Me.lblTitle.Location = New System.Drawing.Point(300, 0)
'        Me.lblTitle.Name = "lblTitle"
'        Me.lblTitle.Size = New System.Drawing.Size(998, 54)
'        Me.lblTitle.TabIndex = 0
'        Me.lblTitle.Text = "لوحة التحكم"
'        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'lblStatus
'        '
'        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Left
'        Me.lblStatus.ForeColor = System.Drawing.Color.White
'        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
'        Me.lblStatus.Name = "lblStatus"
'        Me.lblStatus.Size = New System.Drawing.Size(300, 54)
'        Me.lblStatus.TabIndex = 1
'        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
'        '
'        'lblOutgoingCount
'        '
'        Me.lblOutgoingCount.Location = New System.Drawing.Point(0, 0)
'        Me.lblOutgoingCount.Name = "lblOutgoingCount"
'        Me.lblOutgoingCount.Size = New System.Drawing.Size(100, 23)
'        Me.lblOutgoingCount.TabIndex = 0
'        '
'        'lblOutgoingTitle
'        '
'        Me.lblOutgoingTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblOutgoingTitle.Name = "lblOutgoingTitle"
'        Me.lblOutgoingTitle.Size = New System.Drawing.Size(100, 23)
'        Me.lblOutgoingTitle.TabIndex = 0
'        '
'        'lblArchivedCount
'        '
'        Me.lblArchivedCount.Location = New System.Drawing.Point(0, 0)
'        Me.lblArchivedCount.Name = "lblArchivedCount"
'        Me.lblArchivedCount.Size = New System.Drawing.Size(100, 23)
'        Me.lblArchivedCount.TabIndex = 0
'        '
'        'lblArchivedTitle
'        '
'        Me.lblArchivedTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblArchivedTitle.Name = "lblArchivedTitle"
'        Me.lblArchivedTitle.Size = New System.Drawing.Size(100, 23)
'        Me.lblArchivedTitle.TabIndex = 0
'        '
'        'lblUnarchivedCount
'        '
'        Me.lblUnarchivedCount.Location = New System.Drawing.Point(0, 0)
'        Me.lblUnarchivedCount.Name = "lblUnarchivedCount"
'        Me.lblUnarchivedCount.Size = New System.Drawing.Size(100, 23)
'        Me.lblUnarchivedCount.TabIndex = 0
'        '
'        'lblUnarchivedTitle
'        '
'        Me.lblUnarchivedTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblUnarchivedTitle.Name = "lblUnarchivedTitle"
'        Me.lblUnarchivedTitle.Size = New System.Drawing.Size(100, 23)
'        Me.lblUnarchivedTitle.TabIndex = 0
'        '
'        'lblDeletedCount
'        '
'        Me.lblDeletedCount.Location = New System.Drawing.Point(0, 0)
'        Me.lblDeletedCount.Name = "lblDeletedCount"
'        Me.lblDeletedCount.Size = New System.Drawing.Size(100, 23)
'        Me.lblDeletedCount.TabIndex = 0
'        '
'        'lblDeletedTitle
'        '
'        Me.lblDeletedTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblDeletedTitle.Name = "lblDeletedTitle"
'        Me.lblDeletedTitle.Size = New System.Drawing.Size(100, 23)
'        Me.lblDeletedTitle.TabIndex = 0
'        '
'        'TimerRefresh
'        '
'        Me.TimerRefresh.Interval = 10000
'        '
'        'DashboardForm
'        '
'        Me.AutoScroll = True
'        Me.ClientSize = New System.Drawing.Size(1298, 622)
'        Me.Controls.Add(Me.PanelMain)
'        Me.Name = "DashboardForm"
'        Me.Text = "لوحة التحكم"
'        Me.PanelMain.ResumeLayout(False)
'        Me.Panel1.ResumeLayout(False)
'        Me.TableLayoutPanel1.ResumeLayout(False)
'        Me.Panel2.ResumeLayout(False)
'        Me.PanelCharts.ResumeLayout(False)
'        Me.PanelHeader.ResumeLayout(False)
'        Me.ResumeLayout(False)

'    End Sub

'    ' ================= CARD SETUP =================
'    Private Sub SetupCard(panel As Panel, lblValue As Label, lblTitle As Label, title As String, color As Color)

'        panel.Width = 250
'        panel.Height = 90
'        panel.BackColor = color
'        panel.Margin = New Padding(10)

'        lblTitle.Text = title
'        lblTitle.Dock = DockStyle.Top
'        lblTitle.ForeColor = Color.White
'        lblTitle.TextAlign = ContentAlignment.MiddleCenter

'        lblValue.Dock = DockStyle.Fill
'        lblValue.TextAlign = ContentAlignment.MiddleCenter
'        lblValue.Font = New Font("Cairo", 14, FontStyle.Bold)
'        lblValue.ForeColor = Color.White

'        panel.Controls.Add(lblValue)
'        panel.Controls.Add(lblTitle)

'    End Sub

'    Friend WithEvents Panel1 As Panel
'    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
'    Friend WithEvents Panel2 As Panel
'End Class

'=================================================================================================

'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
'Partial Class DashboardForm
'    Inherits BaseFormWithPermissions

'    Private components As System.ComponentModel.IContainer

'    Friend WithEvents PanelMain As Panel
'    Friend WithEvents PanelHeader As Panel
'    Friend WithEvents lblTitle As Label
'    Friend WithEvents lblStatus As Label

'    Friend WithEvents cardOutgoing As Panel
'    Friend WithEvents lblOutgoingCount As Label
'    Friend WithEvents lblOutgoingTitle As Label

'    Friend WithEvents cardArchived As Panel
'    Friend WithEvents lblArchivedCount As Label
'    Friend WithEvents lblArchivedTitle As Label

'    Friend WithEvents cardUnarchived As Panel
'    Friend WithEvents lblUnarchivedCount As Label
'    Friend WithEvents lblUnarchivedTitle As Label

'    Friend WithEvents cardDeleted As Panel
'    Friend WithEvents lblDeletedCount As Label
'    Friend WithEvents lblDeletedTitle As Label

'    Friend WithEvents TimerRefresh As Timer

'    Friend WithEvents PanelCharts As TableLayoutPanel
'    Friend WithEvents chartArchiveOverview As LiveCharts.WinForms.PieChart
'    Friend WithEvents chartArchiveByFolder As LiveCharts.WinForms.CartesianChart
'    Friend WithEvents chartMonthlyTrend As LiveCharts.WinForms.CartesianChart

'    <System.Diagnostics.DebuggerStepThrough()>
'    Private Sub InitializeComponent()
'        Me.components = New System.ComponentModel.Container()
'        Me.PanelMain = New System.Windows.Forms.Panel()
'        Me.Panel1 = New System.Windows.Forms.Panel()
'        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
'        Me.cardOutgoing = New System.Windows.Forms.Panel()
'        Me.cardDeleted = New System.Windows.Forms.Panel()
'        Me.cardArchived = New System.Windows.Forms.Panel()
'        Me.cardUnarchived = New System.Windows.Forms.Panel()
'        Me.PanelHeader = New System.Windows.Forms.Panel()
'        Me.lblTitle = New System.Windows.Forms.Label()
'        Me.lblStatus = New System.Windows.Forms.Label()
'        Me.Panel2 = New System.Windows.Forms.Panel()
'        Me.PanelCharts = New System.Windows.Forms.TableLayoutPanel()
'        Me.chartArchiveOverview = New LiveCharts.WinForms.PieChart()
'        Me.chartArchiveByFolder = New LiveCharts.WinForms.CartesianChart()
'        Me.chartMonthlyTrend = New LiveCharts.WinForms.CartesianChart()
'        Me.lblOutgoingCount = New System.Windows.Forms.Label()
'        Me.lblOutgoingTitle = New System.Windows.Forms.Label()
'        Me.lblArchivedCount = New System.Windows.Forms.Label()
'        Me.lblArchivedTitle = New System.Windows.Forms.Label()
'        Me.lblUnarchivedCount = New System.Windows.Forms.Label()
'        Me.lblUnarchivedTitle = New System.Windows.Forms.Label()
'        Me.lblDeletedCount = New System.Windows.Forms.Label()
'        Me.lblDeletedTitle = New System.Windows.Forms.Label()
'        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
'        Me.PanelMain.SuspendLayout()
'        Me.Panel1.SuspendLayout()
'        Me.TableLayoutPanel1.SuspendLayout()
'        Me.PanelHeader.SuspendLayout()
'        Me.Panel2.SuspendLayout()
'        Me.PanelCharts.SuspendLayout()
'        Me.SuspendLayout()
'        '
'        'PanelMain
'        '
'        Me.PanelMain.Controls.Add(Me.Panel1)
'        Me.PanelMain.Controls.Add(Me.PanelHeader)
'        Me.PanelMain.Controls.Add(Me.Panel2)
'        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.PanelMain.Location = New System.Drawing.Point(0, 0)
'        Me.PanelMain.Name = "PanelMain"
'        Me.PanelMain.Size = New System.Drawing.Size(1300, 700)
'        Me.PanelMain.TabIndex = 0
'        '
'        'Panel1
'        '
'        Me.Panel1.Controls.Add(Me.TableLayoutPanel1)
'        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
'        Me.Panel1.Location = New System.Drawing.Point(0, 60)
'        Me.Panel1.Name = "Panel1"
'        Me.Panel1.Size = New System.Drawing.Size(1300, 97)
'        Me.Panel1.TabIndex = 5
'        '
'        'TableLayoutPanel1
'        '
'        Me.TableLayoutPanel1.ColumnCount = 4
'        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
'        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
'        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
'        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
'        Me.TableLayoutPanel1.Controls.Add(Me.cardOutgoing, 3, 0)
'        Me.TableLayoutPanel1.Controls.Add(Me.cardDeleted, 0, 0)
'        Me.TableLayoutPanel1.Controls.Add(Me.cardArchived, 2, 0)
'        Me.TableLayoutPanel1.Controls.Add(Me.cardUnarchived, 1, 0)
'        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
'        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
'        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
'        Me.TableLayoutPanel1.RowCount = 1
'        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
'        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1300, 91)
'        Me.TableLayoutPanel1.TabIndex = 4
'        '
'        'cardOutgoing
'        '
'        Me.cardOutgoing.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.cardOutgoing.Location = New System.Drawing.Point(3, 3)
'        Me.cardOutgoing.Name = "cardOutgoing"
'        Me.cardOutgoing.Size = New System.Drawing.Size(319, 85)
'        Me.cardOutgoing.TabIndex = 0
'        '
'        'cardDeleted
'        '
'        Me.cardDeleted.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.cardDeleted.Location = New System.Drawing.Point(978, 3)
'        Me.cardDeleted.Name = "cardDeleted"
'        Me.cardDeleted.Size = New System.Drawing.Size(319, 85)
'        Me.cardDeleted.TabIndex = 3
'        '
'        'cardArchived
'        '
'        Me.cardArchived.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.cardArchived.Location = New System.Drawing.Point(328, 3)
'        Me.cardArchived.Name = "cardArchived"
'        Me.cardArchived.Size = New System.Drawing.Size(319, 85)
'        Me.cardArchived.TabIndex = 1
'        '
'        'cardUnarchived
'        '
'        Me.cardUnarchived.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.cardUnarchived.Location = New System.Drawing.Point(653, 3)
'        Me.cardUnarchived.Name = "cardUnarchived"
'        Me.cardUnarchived.Size = New System.Drawing.Size(319, 85)
'        Me.cardUnarchived.TabIndex = 2
'        '
'        'PanelHeader
'        '
'        Me.PanelHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
'        Me.PanelHeader.Controls.Add(Me.lblTitle)
'        Me.PanelHeader.Controls.Add(Me.lblStatus)
'        Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top
'        Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
'        Me.PanelHeader.Name = "PanelHeader"
'        Me.PanelHeader.Size = New System.Drawing.Size(1300, 60)
'        Me.PanelHeader.TabIndex = 2
'        '
'        'lblTitle
'        '
'        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.lblTitle.Font = New System.Drawing.Font("Cairo", 16.0!, System.Drawing.FontStyle.Bold)
'        Me.lblTitle.ForeColor = System.Drawing.Color.White
'        Me.lblTitle.Location = New System.Drawing.Point(300, 0)
'        Me.lblTitle.Name = "lblTitle"
'        Me.lblTitle.Size = New System.Drawing.Size(1000, 60)
'        Me.lblTitle.TabIndex = 0
'        Me.lblTitle.Text = "لوحة التحكم"
'        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'lblStatus
'        '
'        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Left
'        Me.lblStatus.ForeColor = System.Drawing.Color.White
'        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
'        Me.lblStatus.Name = "lblStatus"
'        Me.lblStatus.Size = New System.Drawing.Size(300, 60)
'        Me.lblStatus.TabIndex = 1
'        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
'        '
'        'Panel2
'        '
'        Me.Panel2.Controls.Add(Me.PanelCharts)
'        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
'        Me.Panel2.Location = New System.Drawing.Point(0, 274)
'        Me.Panel2.Name = "Panel2"
'        Me.Panel2.Size = New System.Drawing.Size(1300, 426)
'        Me.Panel2.TabIndex = 6
'        '
'        'PanelCharts
'        '
'        Me.PanelCharts.ColumnCount = 3
'        Me.PanelCharts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.0!))
'        Me.PanelCharts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.0!))
'        Me.PanelCharts.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.0!))
'        Me.PanelCharts.Controls.Add(Me.chartArchiveOverview, 0, 0)
'        Me.PanelCharts.Controls.Add(Me.chartArchiveByFolder, 1, 0)
'        Me.PanelCharts.Controls.Add(Me.chartMonthlyTrend, 2, 0)
'        Me.PanelCharts.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.PanelCharts.Location = New System.Drawing.Point(0, 0)
'        Me.PanelCharts.Name = "PanelCharts"
'        Me.PanelCharts.RowCount = 1
'        Me.PanelCharts.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
'        Me.PanelCharts.Size = New System.Drawing.Size(1300, 426)
'        Me.PanelCharts.TabIndex = 0
'        '
'        'chartArchiveOverview
'        '
'        Me.chartArchiveOverview.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.chartArchiveOverview.Location = New System.Drawing.Point(874, 3)
'        Me.chartArchiveOverview.Name = "chartArchiveOverview"
'        Me.chartArchiveOverview.Size = New System.Drawing.Size(423, 420)
'        Me.chartArchiveOverview.TabIndex = 0
'        '
'        'chartArchiveByFolder
'        '
'        Me.chartArchiveByFolder.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.chartArchiveByFolder.Location = New System.Drawing.Point(432, 3)
'        Me.chartArchiveByFolder.Name = "chartArchiveByFolder"
'        Me.chartArchiveByFolder.Size = New System.Drawing.Size(436, 420)
'        Me.chartArchiveByFolder.TabIndex = 1
'        '
'        'chartMonthlyTrend
'        '
'        Me.chartMonthlyTrend.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.chartMonthlyTrend.Location = New System.Drawing.Point(3, 3)
'        Me.chartMonthlyTrend.Name = "chartMonthlyTrend"
'        Me.chartMonthlyTrend.Size = New System.Drawing.Size(423, 420)
'        Me.chartMonthlyTrend.TabIndex = 2
'        '
'        'lblOutgoingCount
'        '
'        Me.lblOutgoingCount.Location = New System.Drawing.Point(0, 0)
'        Me.lblOutgoingCount.Name = "lblOutgoingCount"
'        Me.lblOutgoingCount.Size = New System.Drawing.Size(100, 23)
'        Me.lblOutgoingCount.TabIndex = 0
'        '
'        'lblOutgoingTitle
'        '
'        Me.lblOutgoingTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblOutgoingTitle.Name = "lblOutgoingTitle"
'        Me.lblOutgoingTitle.Size = New System.Drawing.Size(100, 23)
'        Me.lblOutgoingTitle.TabIndex = 0
'        '
'        'lblArchivedCount
'        '
'        Me.lblArchivedCount.Location = New System.Drawing.Point(0, 0)
'        Me.lblArchivedCount.Name = "lblArchivedCount"
'        Me.lblArchivedCount.Size = New System.Drawing.Size(100, 23)
'        Me.lblArchivedCount.TabIndex = 0
'        '
'        'lblArchivedTitle
'        '
'        Me.lblArchivedTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblArchivedTitle.Name = "lblArchivedTitle"
'        Me.lblArchivedTitle.Size = New System.Drawing.Size(100, 23)
'        Me.lblArchivedTitle.TabIndex = 0
'        '
'        'lblUnarchivedCount
'        '
'        Me.lblUnarchivedCount.Location = New System.Drawing.Point(0, 0)
'        Me.lblUnarchivedCount.Name = "lblUnarchivedCount"
'        Me.lblUnarchivedCount.Size = New System.Drawing.Size(100, 23)
'        Me.lblUnarchivedCount.TabIndex = 0
'        '
'        'lblUnarchivedTitle
'        '
'        Me.lblUnarchivedTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblUnarchivedTitle.Name = "lblUnarchivedTitle"
'        Me.lblUnarchivedTitle.Size = New System.Drawing.Size(100, 23)
'        Me.lblUnarchivedTitle.TabIndex = 0
'        '
'        'lblDeletedCount
'        '
'        Me.lblDeletedCount.Location = New System.Drawing.Point(0, 0)
'        Me.lblDeletedCount.Name = "lblDeletedCount"
'        Me.lblDeletedCount.Size = New System.Drawing.Size(100, 23)
'        Me.lblDeletedCount.TabIndex = 0
'        '
'        'lblDeletedTitle
'        '
'        Me.lblDeletedTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblDeletedTitle.Name = "lblDeletedTitle"
'        Me.lblDeletedTitle.Size = New System.Drawing.Size(100, 23)
'        Me.lblDeletedTitle.TabIndex = 0
'        '
'        'DashboardForm
'        '
'        Me.ClientSize = New System.Drawing.Size(1300, 700)
'        Me.Controls.Add(Me.PanelMain)
'        Me.Name = "DashboardForm"
'        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.RightToLeftLayout = True
'        Me.Text = "لوحة التحكم"
'        Me.PanelMain.ResumeLayout(False)
'        Me.Panel1.ResumeLayout(False)
'        Me.TableLayoutPanel1.ResumeLayout(False)
'        Me.PanelHeader.ResumeLayout(False)
'        Me.Panel2.ResumeLayout(False)
'        Me.PanelCharts.ResumeLayout(False)
'        Me.ResumeLayout(False)

'    End Sub

'    ' =========================
'    ' CARD BUILDER (مسموح في Designer لأنه UI فقط)
'    ' =========================
'    Private Sub SetupCard(panel As Panel, lblValue As Label, lblTitle As Label, title As String, color As Color)

'        panel.Width = 250
'        panel.Height = 90
'        panel.BackColor = color
'        panel.Margin = New Padding(10)

'        lblTitle.Text = title
'        lblTitle.Dock = DockStyle.Top
'        lblTitle.ForeColor = Color.White
'        lblTitle.TextAlign = ContentAlignment.MiddleCenter

'        lblValue.Dock = DockStyle.Fill
'        lblValue.TextAlign = ContentAlignment.MiddleCenter
'        lblValue.Font = New Font("Cairo", 14, FontStyle.Bold)
'        lblValue.ForeColor = Color.White

'        panel.Controls.Add(lblValue)
'        panel.Controls.Add(lblTitle)

'    End Sub

'    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
'    Friend WithEvents Panel1 As Panel
'    Friend WithEvents Panel2 As Panel
'End Class

'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
'Partial Class DashboardForm
'    Inherits BaseFormWithPermissions

'    'Form overrides dispose to clean up the list of components.
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
'        Me.components = New System.ComponentModel.Container()
'        Me.PanelMain = New System.Windows.Forms.Panel()
'        Me.PanelCharts = New System.Windows.Forms.Panel()
'        Me.PanelChart2 = New System.Windows.Forms.Panel()
'        Me.lblChart2Title = New System.Windows.Forms.Label()
'        Me.chartArchiveByFolder = New LiveCharts.WinForms.CartesianChart()
'        Me.PanelChart1 = New System.Windows.Forms.Panel()
'        Me.lblChart1Title = New System.Windows.Forms.Label()
'        Me.chartArchiveOverview = New LiveCharts.WinForms.PieChart()
'        Me.PanelChart3 = New System.Windows.Forms.Panel()
'        Me.lblChart3Title = New System.Windows.Forms.Label()
'        Me.chartMonthlyTrend = New LiveCharts.WinForms.CartesianChart()
'        Me.PanelSummary = New System.Windows.Forms.Panel()
'        Me.PanelDeleted = New System.Windows.Forms.Panel()
'        Me.lblDeletedCount = New System.Windows.Forms.Label()
'        Me.lblDeletedTitle = New System.Windows.Forms.Label()
'        Me.PanelUnarchived = New System.Windows.Forms.Panel()
'        Me.lblUnarchivedCount = New System.Windows.Forms.Label()
'        Me.lblUnarchivedTitle = New System.Windows.Forms.Label()
'        Me.PanelArchived = New System.Windows.Forms.Panel()
'        Me.lblArchivedCount = New System.Windows.Forms.Label()
'        Me.lblArchivedTitle = New System.Windows.Forms.Label()
'        Me.PanelOutgoing = New System.Windows.Forms.Panel()
'        Me.lblOutgoingCount = New System.Windows.Forms.Label()
'        Me.lblOutgoingTitle = New System.Windows.Forms.Label()
'        Me.PanelTitle = New System.Windows.Forms.Panel()
'        Me.lblTitle = New System.Windows.Forms.Label()
'        Me.lblStatus = New System.Windows.Forms.Label()
'        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
'        Me.PanelMain.SuspendLayout()
'        Me.PanelCharts.SuspendLayout()
'        Me.PanelChart2.SuspendLayout()
'        Me.PanelChart1.SuspendLayout()
'        Me.PanelChart3.SuspendLayout()
'        Me.PanelSummary.SuspendLayout()
'        Me.PanelDeleted.SuspendLayout()
'        Me.PanelUnarchived.SuspendLayout()
'        Me.PanelArchived.SuspendLayout()
'        Me.PanelOutgoing.SuspendLayout()
'        Me.PanelTitle.SuspendLayout()
'        Me.SuspendLayout()
'        '
'        'PanelMain
'        '
'        Me.PanelMain.Controls.Add(Me.PanelCharts)
'        Me.PanelMain.Controls.Add(Me.PanelSummary)
'        Me.PanelMain.Controls.Add(Me.PanelTitle)
'        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.PanelMain.Location = New System.Drawing.Point(0, 0)
'        Me.PanelMain.Name = "PanelMain"
'        Me.PanelMain.Size = New System.Drawing.Size(1270, 696)
'        Me.PanelMain.TabIndex = 0
'        '
'        'PanelCharts
'        '
'        Me.PanelCharts.Controls.Add(Me.PanelChart2)
'        Me.PanelCharts.Controls.Add(Me.PanelChart1)
'        Me.PanelCharts.Controls.Add(Me.PanelChart3)
'        Me.PanelCharts.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.PanelCharts.Location = New System.Drawing.Point(0, 150)
'        Me.PanelCharts.Name = "PanelCharts"
'        Me.PanelCharts.Size = New System.Drawing.Size(1270, 546)
'        Me.PanelCharts.TabIndex = 2
'        '
'        'PanelChart2
'        '
'        Me.PanelChart2.Controls.Add(Me.lblChart2Title)
'        Me.PanelChart2.Controls.Add(Me.chartArchiveByFolder)
'        Me.PanelChart2.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.PanelChart2.Location = New System.Drawing.Point(428, 0)
'        Me.PanelChart2.Name = "PanelChart2"
'        Me.PanelChart2.Padding = New System.Windows.Forms.Padding(10)
'        Me.PanelChart2.Size = New System.Drawing.Size(414, 546)
'        Me.PanelChart2.TabIndex = 1
'        '
'        'lblChart2Title
'        '
'        Me.lblChart2Title.Dock = System.Windows.Forms.DockStyle.Top
'        Me.lblChart2Title.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblChart2Title.Location = New System.Drawing.Point(10, 10)
'        Me.lblChart2Title.Name = "lblChart2Title"
'        Me.lblChart2Title.Size = New System.Drawing.Size(394, 30)
'        Me.lblChart2Title.TabIndex = 1
'        Me.lblChart2Title.Text = "توزيع الأرشيف حسب المجلدات"
'        Me.lblChart2Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'chartArchiveByFolder
'        '
'        Me.chartArchiveByFolder.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.chartArchiveByFolder.Font = New System.Drawing.Font("Cairo Medium", 11.25!, System.Drawing.FontStyle.Bold)
'        Me.chartArchiveByFolder.Location = New System.Drawing.Point(10, 10)
'        Me.chartArchiveByFolder.Name = "chartArchiveByFolder"
'        Me.chartArchiveByFolder.Size = New System.Drawing.Size(394, 526)
'        Me.chartArchiveByFolder.TabIndex = 0
'        Me.chartArchiveByFolder.Text = "cartesianChart2"
'        '
'        'PanelChart1
'        '
'        Me.PanelChart1.Controls.Add(Me.lblChart1Title)
'        Me.PanelChart1.Controls.Add(Me.chartArchiveOverview)
'        Me.PanelChart1.Dock = System.Windows.Forms.DockStyle.Left
'        Me.PanelChart1.Location = New System.Drawing.Point(0, 0)
'        Me.PanelChart1.Name = "PanelChart1"
'        Me.PanelChart1.Padding = New System.Windows.Forms.Padding(10)
'        Me.PanelChart1.Size = New System.Drawing.Size(428, 546)
'        Me.PanelChart1.TabIndex = 0
'        '
'        'lblChart1Title
'        '
'        Me.lblChart1Title.Dock = System.Windows.Forms.DockStyle.Top
'        Me.lblChart1Title.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblChart1Title.Location = New System.Drawing.Point(10, 10)
'        Me.lblChart1Title.Name = "lblChart1Title"
'        Me.lblChart1Title.Size = New System.Drawing.Size(408, 30)
'        Me.lblChart1Title.TabIndex = 1
'        Me.lblChart1Title.Text = "نظرة عامة على الأرشيف"
'        Me.lblChart1Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'chartArchiveOverview
'        '
'        Me.chartArchiveOverview.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.chartArchiveOverview.Font = New System.Drawing.Font("Cairo Medium", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.chartArchiveOverview.Location = New System.Drawing.Point(10, 10)
'        Me.chartArchiveOverview.Name = "chartArchiveOverview"
'        Me.chartArchiveOverview.Size = New System.Drawing.Size(408, 526)
'        Me.chartArchiveOverview.TabIndex = 0
'        Me.chartArchiveOverview.Text = "pieChart1"
'        '
'        'PanelChart3
'        '
'        Me.PanelChart3.Controls.Add(Me.lblChart3Title)
'        Me.PanelChart3.Controls.Add(Me.chartMonthlyTrend)
'        Me.PanelChart3.Dock = System.Windows.Forms.DockStyle.Right
'        Me.PanelChart3.Location = New System.Drawing.Point(842, 0)
'        Me.PanelChart3.Name = "PanelChart3"
'        Me.PanelChart3.Padding = New System.Windows.Forms.Padding(10)
'        Me.PanelChart3.Size = New System.Drawing.Size(428, 546)
'        Me.PanelChart3.TabIndex = 2
'        '
'        'lblChart3Title
'        '
'        Me.lblChart3Title.Dock = System.Windows.Forms.DockStyle.Top
'        Me.lblChart3Title.Font = New System.Drawing.Font("Cairo", 11.25!)
'        Me.lblChart3Title.Location = New System.Drawing.Point(10, 10)
'        Me.lblChart3Title.Name = "lblChart3Title"
'        Me.lblChart3Title.Size = New System.Drawing.Size(408, 30)
'        Me.lblChart3Title.TabIndex = 1
'        Me.lblChart3Title.Text = "الاتجاه الشهري للأرشيف"
'        Me.lblChart3Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'chartMonthlyTrend
'        '
'        Me.chartMonthlyTrend.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.chartMonthlyTrend.Font = New System.Drawing.Font("Cairo Medium", 11.25!, System.Drawing.FontStyle.Bold)
'        Me.chartMonthlyTrend.Location = New System.Drawing.Point(10, 10)
'        Me.chartMonthlyTrend.Name = "chartMonthlyTrend"
'        Me.chartMonthlyTrend.Size = New System.Drawing.Size(408, 526)
'        Me.chartMonthlyTrend.TabIndex = 0
'        Me.chartMonthlyTrend.Text = "cartesianChart3"
'        '
'        'PanelSummary
'        '
'        Me.PanelSummary.Controls.Add(Me.PanelDeleted)
'        Me.PanelSummary.Controls.Add(Me.PanelUnarchived)
'        Me.PanelSummary.Controls.Add(Me.PanelArchived)
'        Me.PanelSummary.Controls.Add(Me.PanelOutgoing)
'        Me.PanelSummary.Dock = System.Windows.Forms.DockStyle.Top
'        Me.PanelSummary.Location = New System.Drawing.Point(0, 50)
'        Me.PanelSummary.Name = "PanelSummary"
'        Me.PanelSummary.Size = New System.Drawing.Size(1270, 100)
'        Me.PanelSummary.TabIndex = 1
'        '
'        'PanelDeleted
'        '
'        Me.PanelDeleted.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
'        Me.PanelDeleted.Controls.Add(Me.lblDeletedCount)
'        Me.PanelDeleted.Controls.Add(Me.lblDeletedTitle)
'        Me.PanelDeleted.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.PanelDeleted.Location = New System.Drawing.Point(642, 0)
'        Me.PanelDeleted.Name = "PanelDeleted"
'        Me.PanelDeleted.Size = New System.Drawing.Size(327, 100)
'        Me.PanelDeleted.TabIndex = 3
'        '
'        'lblDeletedCount
'        '
'        Me.lblDeletedCount.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.lblDeletedCount.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.lblDeletedCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblDeletedCount.ForeColor = System.Drawing.Color.Red
'        Me.lblDeletedCount.Location = New System.Drawing.Point(0, 30)
'        Me.lblDeletedCount.Name = "lblDeletedCount"
'        Me.lblDeletedCount.Size = New System.Drawing.Size(327, 70)
'        Me.lblDeletedCount.TabIndex = 1
'        Me.lblDeletedCount.Text = "0"
'        Me.lblDeletedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'lblDeletedTitle
'        '
'        Me.lblDeletedTitle.Dock = System.Windows.Forms.DockStyle.Top
'        Me.lblDeletedTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblDeletedTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblDeletedTitle.Name = "lblDeletedTitle"
'        Me.lblDeletedTitle.Size = New System.Drawing.Size(327, 30)
'        Me.lblDeletedTitle.TabIndex = 0
'        Me.lblDeletedTitle.Text = "المحذوفة"
'        Me.lblDeletedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'PanelUnarchived
'        '
'        Me.PanelUnarchived.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(186, Byte), Integer))
'        Me.PanelUnarchived.Controls.Add(Me.lblUnarchivedCount)
'        Me.PanelUnarchived.Controls.Add(Me.lblUnarchivedTitle)
'        Me.PanelUnarchived.Dock = System.Windows.Forms.DockStyle.Right
'        Me.PanelUnarchived.Location = New System.Drawing.Point(969, 0)
'        Me.PanelUnarchived.Name = "PanelUnarchived"
'        Me.PanelUnarchived.Size = New System.Drawing.Size(301, 100)
'        Me.PanelUnarchived.TabIndex = 2
'        '
'        'lblUnarchivedCount
'        '
'        Me.lblUnarchivedCount.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.lblUnarchivedCount.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.lblUnarchivedCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblUnarchivedCount.ForeColor = System.Drawing.Color.Orange
'        Me.lblUnarchivedCount.Location = New System.Drawing.Point(0, 30)
'        Me.lblUnarchivedCount.Name = "lblUnarchivedCount"
'        Me.lblUnarchivedCount.Size = New System.Drawing.Size(301, 70)
'        Me.lblUnarchivedCount.TabIndex = 1
'        Me.lblUnarchivedCount.Text = "0"
'        Me.lblUnarchivedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'lblUnarchivedTitle
'        '
'        Me.lblUnarchivedTitle.Dock = System.Windows.Forms.DockStyle.Top
'        Me.lblUnarchivedTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblUnarchivedTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblUnarchivedTitle.Name = "lblUnarchivedTitle"
'        Me.lblUnarchivedTitle.Size = New System.Drawing.Size(301, 30)
'        Me.lblUnarchivedTitle.TabIndex = 0
'        Me.lblUnarchivedTitle.Text = "غير المؤرشفة"
'        Me.lblUnarchivedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'PanelArchived
'        '
'        Me.PanelArchived.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(201, Byte), Integer))
'        Me.PanelArchived.Controls.Add(Me.lblArchivedCount)
'        Me.PanelArchived.Controls.Add(Me.lblArchivedTitle)
'        Me.PanelArchived.Dock = System.Windows.Forms.DockStyle.Left
'        Me.PanelArchived.Location = New System.Drawing.Point(321, 0)
'        Me.PanelArchived.Name = "PanelArchived"
'        Me.PanelArchived.Size = New System.Drawing.Size(321, 100)
'        Me.PanelArchived.TabIndex = 1
'        '
'        'lblArchivedCount
'        '
'        Me.lblArchivedCount.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.lblArchivedCount.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.lblArchivedCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblArchivedCount.ForeColor = System.Drawing.Color.Green
'        Me.lblArchivedCount.Location = New System.Drawing.Point(0, 30)
'        Me.lblArchivedCount.Name = "lblArchivedCount"
'        Me.lblArchivedCount.Size = New System.Drawing.Size(321, 70)
'        Me.lblArchivedCount.TabIndex = 1
'        Me.lblArchivedCount.Text = "0"
'        Me.lblArchivedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'lblArchivedTitle
'        '
'        Me.lblArchivedTitle.Dock = System.Windows.Forms.DockStyle.Top
'        Me.lblArchivedTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblArchivedTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblArchivedTitle.Name = "lblArchivedTitle"
'        Me.lblArchivedTitle.Size = New System.Drawing.Size(321, 30)
'        Me.lblArchivedTitle.TabIndex = 0
'        Me.lblArchivedTitle.Text = "المؤرشفة"
'        Me.lblArchivedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'PanelOutgoing
'        '
'        Me.PanelOutgoing.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer))
'        Me.PanelOutgoing.Controls.Add(Me.lblOutgoingCount)
'        Me.PanelOutgoing.Controls.Add(Me.lblOutgoingTitle)
'        Me.PanelOutgoing.Dock = System.Windows.Forms.DockStyle.Left
'        Me.PanelOutgoing.Location = New System.Drawing.Point(0, 0)
'        Me.PanelOutgoing.Name = "PanelOutgoing"
'        Me.PanelOutgoing.Size = New System.Drawing.Size(321, 100)
'        Me.PanelOutgoing.TabIndex = 0
'        '
'        'lblOutgoingCount
'        '
'        Me.lblOutgoingCount.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.lblOutgoingCount.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.lblOutgoingCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblOutgoingCount.ForeColor = System.Drawing.Color.Blue
'        Me.lblOutgoingCount.Location = New System.Drawing.Point(0, 30)
'        Me.lblOutgoingCount.Name = "lblOutgoingCount"
'        Me.lblOutgoingCount.Size = New System.Drawing.Size(321, 70)
'        Me.lblOutgoingCount.TabIndex = 1
'        Me.lblOutgoingCount.Text = "0"
'        Me.lblOutgoingCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'lblOutgoingTitle
'        '
'        Me.lblOutgoingTitle.Dock = System.Windows.Forms.DockStyle.Top
'        Me.lblOutgoingTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblOutgoingTitle.Location = New System.Drawing.Point(0, 0)
'        Me.lblOutgoingTitle.Name = "lblOutgoingTitle"
'        Me.lblOutgoingTitle.Size = New System.Drawing.Size(321, 30)
'        Me.lblOutgoingTitle.TabIndex = 0
'        Me.lblOutgoingTitle.Text = "الصادرة"
'        Me.lblOutgoingTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
'        '
'        'PanelTitle
'        '
'        Me.PanelTitle.BackColor = System.Drawing.Color.SteelBlue
'        Me.PanelTitle.Controls.Add(Me.lblTitle)
'        Me.PanelTitle.Controls.Add(Me.lblStatus)
'        Me.PanelTitle.Dock = System.Windows.Forms.DockStyle.Top
'        Me.PanelTitle.Location = New System.Drawing.Point(0, 0)
'        Me.PanelTitle.Name = "PanelTitle"
'        Me.PanelTitle.Size = New System.Drawing.Size(1270, 50)
'        Me.PanelTitle.TabIndex = 0
'        '
'        'lblTitle
'        '
'        Me.lblTitle.AutoSize = True
'        Me.lblTitle.Font = New System.Drawing.Font("Cairo", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblTitle.ForeColor = System.Drawing.Color.White
'        Me.lblTitle.Location = New System.Drawing.Point(539, 7)
'        Me.lblTitle.Name = "lblTitle"
'        Me.lblTitle.Size = New System.Drawing.Size(123, 37)
'        Me.lblTitle.TabIndex = 0
'        Me.lblTitle.Text = "لوحة التحكم"
'        '
'        'lblStatus
'        '
'        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Left
'        Me.lblStatus.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblStatus.ForeColor = System.Drawing.Color.White
'        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
'        Me.lblStatus.Name = "lblStatus"
'        Me.lblStatus.Size = New System.Drawing.Size(479, 50)
'        Me.lblStatus.TabIndex = 2
'        Me.lblStatus.Text = "جاري تحميل البيانات..."
'        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
'        Me.lblStatus.Visible = False
'        '
'        'TimerRefresh
'        '
'        Me.TimerRefresh.Interval = 60000
'        '
'        'DashboardForm
'        '
'        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
'        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
'        Me.ClientSize = New System.Drawing.Size(1270, 696)
'        Me.Controls.Add(Me.PanelMain)
'        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.Margin = New System.Windows.Forms.Padding(4)
'        Me.MaximizeBox = False
'        Me.MinimizeBox = False
'        Me.Name = "DashboardForm"
'        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
'        Me.RightToLeftLayout = True
'        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
'        Me.Text = "لوحة التحكم"
'        Me.PanelMain.ResumeLayout(False)
'        Me.PanelCharts.ResumeLayout(False)
'        Me.PanelChart2.ResumeLayout(False)
'        Me.PanelChart1.ResumeLayout(False)
'        Me.PanelChart3.ResumeLayout(False)
'        Me.PanelSummary.ResumeLayout(False)
'        Me.PanelDeleted.ResumeLayout(False)
'        Me.PanelUnarchived.ResumeLayout(False)
'        Me.PanelArchived.ResumeLayout(False)
'        Me.PanelOutgoing.ResumeLayout(False)
'        Me.PanelTitle.ResumeLayout(False)
'        Me.PanelTitle.PerformLayout()
'        Me.ResumeLayout(False)

'    End Sub

'    Friend WithEvents PanelMain As Panel
'    Friend WithEvents PanelSummary As Panel
'    Friend WithEvents PanelOutgoing As Panel
'    Friend WithEvents lblOutgoingCount As Label
'    Friend WithEvents lblOutgoingTitle As Label
'    Friend WithEvents PanelArchived As Panel
'    Friend WithEvents lblArchivedCount As Label
'    Friend WithEvents lblArchivedTitle As Label
'    Friend WithEvents PanelUnarchived As Panel
'    Friend WithEvents lblUnarchivedCount As Label
'    Friend WithEvents lblUnarchivedTitle As Label
'    Friend WithEvents PanelDeleted As Panel
'    Friend WithEvents lblDeletedCount As Label
'    Friend WithEvents lblDeletedTitle As Label
'    Friend WithEvents PanelTitle As Panel
'    Friend WithEvents lblTitle As Label
'    Friend WithEvents PanelCharts As Panel
'    Friend WithEvents PanelChart1 As Panel
'    Friend WithEvents chartArchiveOverview As LiveCharts.WinForms.PieChart
'    Friend WithEvents PanelChart2 As Panel
'    Friend WithEvents chartArchiveByFolder As LiveCharts.WinForms.CartesianChart
'    Friend WithEvents PanelChart3 As Panel
'    Friend WithEvents chartMonthlyTrend As LiveCharts.WinForms.CartesianChart
'    Friend WithEvents TimerRefresh As Timer
'    Friend WithEvents lblChart1Title As Label
'    Friend WithEvents lblChart2Title As Label
'    Friend WithEvents lblChart3Title As Label
'    Friend WithEvents lblStatus As Label

'End Class