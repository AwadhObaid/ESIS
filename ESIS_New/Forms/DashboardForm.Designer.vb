


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DashboardForm
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
        Me.PanelMain = New System.Windows.Forms.Panel()
        Me.PanelCharts = New System.Windows.Forms.Panel()
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
        Me.PanelUnarchived = New System.Windows.Forms.Panel()
        Me.lblUnarchivedCount = New System.Windows.Forms.Label()
        Me.lblUnarchivedTitle = New System.Windows.Forms.Label()
        Me.PanelArchived = New System.Windows.Forms.Panel()
        Me.lblArchivedCount = New System.Windows.Forms.Label()
        Me.lblArchivedTitle = New System.Windows.Forms.Label()
        Me.PanelOutgoing = New System.Windows.Forms.Panel()
        Me.lblOutgoingCount = New System.Windows.Forms.Label()
        Me.lblOutgoingTitle = New System.Windows.Forms.Label()
        Me.PanelTitle = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.PanelMain.SuspendLayout()
        Me.PanelCharts.SuspendLayout()
        Me.PanelChart2.SuspendLayout()
        Me.PanelChart1.SuspendLayout()
        Me.PanelChart3.SuspendLayout()
        Me.PanelSummary.SuspendLayout()
        Me.PanelDeleted.SuspendLayout()
        Me.PanelUnarchived.SuspendLayout()
        Me.PanelArchived.SuspendLayout()
        Me.PanelOutgoing.SuspendLayout()
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
        Me.PanelMain.Size = New System.Drawing.Size(1270, 696)
        Me.PanelMain.TabIndex = 0
        '
        'PanelCharts
        '
        Me.PanelCharts.Controls.Add(Me.PanelChart2)
        Me.PanelCharts.Controls.Add(Me.PanelChart1)
        Me.PanelCharts.Controls.Add(Me.PanelChart3)
        Me.PanelCharts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelCharts.Location = New System.Drawing.Point(0, 150)
        Me.PanelCharts.Name = "PanelCharts"
        Me.PanelCharts.Size = New System.Drawing.Size(1270, 546)
        Me.PanelCharts.TabIndex = 2
        '
        'PanelChart2
        '
        Me.PanelChart2.Controls.Add(Me.lblChart2Title)
        Me.PanelChart2.Controls.Add(Me.chartArchiveByFolder)
        Me.PanelChart2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelChart2.Location = New System.Drawing.Point(428, 0)
        Me.PanelChart2.Name = "PanelChart2"
        Me.PanelChart2.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelChart2.Size = New System.Drawing.Size(414, 546)
        Me.PanelChart2.TabIndex = 1
        '
        'lblChart2Title
        '
        Me.lblChart2Title.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblChart2Title.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChart2Title.Location = New System.Drawing.Point(10, 10)
        Me.lblChart2Title.Name = "lblChart2Title"
        Me.lblChart2Title.Size = New System.Drawing.Size(394, 30)
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
        Me.chartArchiveByFolder.Size = New System.Drawing.Size(394, 526)
        Me.chartArchiveByFolder.TabIndex = 0
        Me.chartArchiveByFolder.Text = "cartesianChart2"
        '
        'PanelChart1
        '
        Me.PanelChart1.Controls.Add(Me.lblChart1Title)
        Me.PanelChart1.Controls.Add(Me.chartArchiveOverview)
        Me.PanelChart1.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelChart1.Location = New System.Drawing.Point(0, 0)
        Me.PanelChart1.Name = "PanelChart1"
        Me.PanelChart1.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelChart1.Size = New System.Drawing.Size(428, 546)
        Me.PanelChart1.TabIndex = 0
        '
        'lblChart1Title
        '
        Me.lblChart1Title.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblChart1Title.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChart1Title.Location = New System.Drawing.Point(10, 10)
        Me.lblChart1Title.Name = "lblChart1Title"
        Me.lblChart1Title.Size = New System.Drawing.Size(408, 30)
        Me.lblChart1Title.TabIndex = 1
        Me.lblChart1Title.Text = "نظرة عامة على الأرشيف"
        Me.lblChart1Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chartArchiveOverview
        '
        Me.chartArchiveOverview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chartArchiveOverview.Font = New System.Drawing.Font("Cairo Medium", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chartArchiveOverview.Location = New System.Drawing.Point(10, 10)
        Me.chartArchiveOverview.Name = "chartArchiveOverview"
        Me.chartArchiveOverview.Size = New System.Drawing.Size(408, 526)
        Me.chartArchiveOverview.TabIndex = 0
        Me.chartArchiveOverview.Text = "pieChart1"
        '
        'PanelChart3
        '
        Me.PanelChart3.Controls.Add(Me.lblChart3Title)
        Me.PanelChart3.Controls.Add(Me.chartMonthlyTrend)
        Me.PanelChart3.Dock = System.Windows.Forms.DockStyle.Right
        Me.PanelChart3.Location = New System.Drawing.Point(842, 0)
        Me.PanelChart3.Name = "PanelChart3"
        Me.PanelChart3.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelChart3.Size = New System.Drawing.Size(428, 546)
        Me.PanelChart3.TabIndex = 2
        '
        'lblChart3Title
        '
        Me.lblChart3Title.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblChart3Title.Font = New System.Drawing.Font("Cairo", 11.25!)
        Me.lblChart3Title.Location = New System.Drawing.Point(10, 10)
        Me.lblChart3Title.Name = "lblChart3Title"
        Me.lblChart3Title.Size = New System.Drawing.Size(408, 30)
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
        Me.chartMonthlyTrend.Size = New System.Drawing.Size(408, 526)
        Me.chartMonthlyTrend.TabIndex = 0
        Me.chartMonthlyTrend.Text = "cartesianChart3"
        '
        'PanelSummary
        '
        Me.PanelSummary.Controls.Add(Me.PanelDeleted)
        Me.PanelSummary.Controls.Add(Me.PanelUnarchived)
        Me.PanelSummary.Controls.Add(Me.PanelArchived)
        Me.PanelSummary.Controls.Add(Me.PanelOutgoing)
        Me.PanelSummary.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelSummary.Location = New System.Drawing.Point(0, 50)
        Me.PanelSummary.Name = "PanelSummary"
        Me.PanelSummary.Size = New System.Drawing.Size(1270, 100)
        Me.PanelSummary.TabIndex = 1
        '
        'PanelDeleted
        '
        Me.PanelDeleted.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.PanelDeleted.Controls.Add(Me.lblDeletedCount)
        Me.PanelDeleted.Controls.Add(Me.lblDeletedTitle)
        Me.PanelDeleted.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelDeleted.Location = New System.Drawing.Point(642, 0)
        Me.PanelDeleted.Name = "PanelDeleted"
        Me.PanelDeleted.Size = New System.Drawing.Size(327, 100)
        Me.PanelDeleted.TabIndex = 3
        '
        'lblDeletedCount
        '
        Me.lblDeletedCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblDeletedCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDeletedCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeletedCount.ForeColor = System.Drawing.Color.Red
        Me.lblDeletedCount.Location = New System.Drawing.Point(0, 30)
        Me.lblDeletedCount.Name = "lblDeletedCount"
        Me.lblDeletedCount.Size = New System.Drawing.Size(327, 70)
        Me.lblDeletedCount.TabIndex = 1
        Me.lblDeletedCount.Text = "0"
        Me.lblDeletedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDeletedTitle
        '
        Me.lblDeletedTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblDeletedTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeletedTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblDeletedTitle.Name = "lblDeletedTitle"
        Me.lblDeletedTitle.Size = New System.Drawing.Size(327, 30)
        Me.lblDeletedTitle.TabIndex = 0
        Me.lblDeletedTitle.Text = "المحذوفة"
        Me.lblDeletedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelUnarchived
        '
        Me.PanelUnarchived.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(186, Byte), Integer))
        Me.PanelUnarchived.Controls.Add(Me.lblUnarchivedCount)
        Me.PanelUnarchived.Controls.Add(Me.lblUnarchivedTitle)
        Me.PanelUnarchived.Dock = System.Windows.Forms.DockStyle.Right
        Me.PanelUnarchived.Location = New System.Drawing.Point(969, 0)
        Me.PanelUnarchived.Name = "PanelUnarchived"
        Me.PanelUnarchived.Size = New System.Drawing.Size(301, 100)
        Me.PanelUnarchived.TabIndex = 2
        '
        'lblUnarchivedCount
        '
        Me.lblUnarchivedCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblUnarchivedCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblUnarchivedCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnarchivedCount.ForeColor = System.Drawing.Color.Orange
        Me.lblUnarchivedCount.Location = New System.Drawing.Point(0, 30)
        Me.lblUnarchivedCount.Name = "lblUnarchivedCount"
        Me.lblUnarchivedCount.Size = New System.Drawing.Size(301, 70)
        Me.lblUnarchivedCount.TabIndex = 1
        Me.lblUnarchivedCount.Text = "0"
        Me.lblUnarchivedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUnarchivedTitle
        '
        Me.lblUnarchivedTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblUnarchivedTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnarchivedTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblUnarchivedTitle.Name = "lblUnarchivedTitle"
        Me.lblUnarchivedTitle.Size = New System.Drawing.Size(301, 30)
        Me.lblUnarchivedTitle.TabIndex = 0
        Me.lblUnarchivedTitle.Text = "غير المؤرشفة"
        Me.lblUnarchivedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelArchived
        '
        Me.PanelArchived.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(201, Byte), Integer))
        Me.PanelArchived.Controls.Add(Me.lblArchivedCount)
        Me.PanelArchived.Controls.Add(Me.lblArchivedTitle)
        Me.PanelArchived.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelArchived.Location = New System.Drawing.Point(321, 0)
        Me.PanelArchived.Name = "PanelArchived"
        Me.PanelArchived.Size = New System.Drawing.Size(321, 100)
        Me.PanelArchived.TabIndex = 1
        '
        'lblArchivedCount
        '
        Me.lblArchivedCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblArchivedCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblArchivedCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArchivedCount.ForeColor = System.Drawing.Color.Green
        Me.lblArchivedCount.Location = New System.Drawing.Point(0, 30)
        Me.lblArchivedCount.Name = "lblArchivedCount"
        Me.lblArchivedCount.Size = New System.Drawing.Size(321, 70)
        Me.lblArchivedCount.TabIndex = 1
        Me.lblArchivedCount.Text = "0"
        Me.lblArchivedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblArchivedTitle
        '
        Me.lblArchivedTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblArchivedTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArchivedTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblArchivedTitle.Name = "lblArchivedTitle"
        Me.lblArchivedTitle.Size = New System.Drawing.Size(321, 30)
        Me.lblArchivedTitle.TabIndex = 0
        Me.lblArchivedTitle.Text = "المؤرشفة"
        Me.lblArchivedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelOutgoing
        '
        Me.PanelOutgoing.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PanelOutgoing.Controls.Add(Me.lblOutgoingCount)
        Me.PanelOutgoing.Controls.Add(Me.lblOutgoingTitle)
        Me.PanelOutgoing.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelOutgoing.Location = New System.Drawing.Point(0, 0)
        Me.PanelOutgoing.Name = "PanelOutgoing"
        Me.PanelOutgoing.Size = New System.Drawing.Size(321, 100)
        Me.PanelOutgoing.TabIndex = 0
        '
        'lblOutgoingCount
        '
        Me.lblOutgoingCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblOutgoingCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblOutgoingCount.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutgoingCount.ForeColor = System.Drawing.Color.Blue
        Me.lblOutgoingCount.Location = New System.Drawing.Point(0, 30)
        Me.lblOutgoingCount.Name = "lblOutgoingCount"
        Me.lblOutgoingCount.Size = New System.Drawing.Size(321, 70)
        Me.lblOutgoingCount.TabIndex = 1
        Me.lblOutgoingCount.Text = "0"
        Me.lblOutgoingCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblOutgoingTitle
        '
        Me.lblOutgoingTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblOutgoingTitle.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutgoingTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblOutgoingTitle.Name = "lblOutgoingTitle"
        Me.lblOutgoingTitle.Size = New System.Drawing.Size(321, 30)
        Me.lblOutgoingTitle.TabIndex = 0
        Me.lblOutgoingTitle.Text = "الصادرة"
        Me.lblOutgoingTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelTitle
        '
        Me.PanelTitle.BackColor = System.Drawing.Color.SteelBlue
        Me.PanelTitle.Controls.Add(Me.lblTitle)
        Me.PanelTitle.Controls.Add(Me.lblStatus)
        Me.PanelTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTitle.Location = New System.Drawing.Point(0, 0)
        Me.PanelTitle.Name = "PanelTitle"
        Me.PanelTitle.Size = New System.Drawing.Size(1270, 50)
        Me.PanelTitle.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Cairo", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(539, 7)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(123, 37)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "لوحة التحكم"
        '
        'lblStatus
        '
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Left
        Me.lblStatus.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.White
        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(479, 50)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "جاري تحميل البيانات..."
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblStatus.Visible = False
        '
        'TimerRefresh
        '
        Me.TimerRefresh.Interval = 60000
        '
        'DashboardForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1270, 696)
        Me.Controls.Add(Me.PanelMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DashboardForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "لوحة التحكم"
        Me.PanelMain.ResumeLayout(False)
        Me.PanelCharts.ResumeLayout(False)
        Me.PanelChart2.ResumeLayout(False)
        Me.PanelChart1.ResumeLayout(False)
        Me.PanelChart3.ResumeLayout(False)
        Me.PanelSummary.ResumeLayout(False)
        Me.PanelDeleted.ResumeLayout(False)
        Me.PanelUnarchived.ResumeLayout(False)
        Me.PanelArchived.ResumeLayout(False)
        Me.PanelOutgoing.ResumeLayout(False)
        Me.PanelTitle.ResumeLayout(False)
        Me.PanelTitle.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelMain As Panel
    Friend WithEvents PanelSummary As Panel
    Friend WithEvents PanelOutgoing As Panel
    Friend WithEvents lblOutgoingCount As Label
    Friend WithEvents lblOutgoingTitle As Label
    Friend WithEvents PanelArchived As Panel
    Friend WithEvents lblArchivedCount As Label
    Friend WithEvents lblArchivedTitle As Label
    Friend WithEvents PanelUnarchived As Panel
    Friend WithEvents lblUnarchivedCount As Label
    Friend WithEvents lblUnarchivedTitle As Label
    Friend WithEvents PanelDeleted As Panel
    Friend WithEvents lblDeletedCount As Label
    Friend WithEvents lblDeletedTitle As Label
    Friend WithEvents PanelTitle As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents PanelCharts As Panel
    Friend WithEvents PanelChart1 As Panel
    Friend WithEvents chartArchiveOverview As LiveCharts.WinForms.PieChart
    Friend WithEvents PanelChart2 As Panel
    Friend WithEvents chartArchiveByFolder As LiveCharts.WinForms.CartesianChart
    Friend WithEvents PanelChart3 As Panel
    Friend WithEvents chartMonthlyTrend As LiveCharts.WinForms.CartesianChart
    Friend WithEvents TimerRefresh As Timer
    Friend WithEvents lblChart1Title As Label
    Friend WithEvents lblChart2Title As Label
    Friend WithEvents lblChart3Title As Label
    Friend WithEvents lblStatus As Label

End Class