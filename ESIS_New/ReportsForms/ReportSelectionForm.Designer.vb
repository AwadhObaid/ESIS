<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ReportSelectionForm
    Inherits System.Windows.Forms.Form

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
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.gbReportType = New System.Windows.Forms.GroupBox()
        Me.cmbReportType = New System.Windows.Forms.ComboBox()
        Me.lblReportType = New System.Windows.Forms.Label()
        Me.gbDateRange = New System.Windows.Forms.GroupBox()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.lblToDate = New System.Windows.Forms.Label()
        Me.lblFromDate = New System.Windows.Forms.Label()
        Me.pnlAdditionalCriteria = New System.Windows.Forms.Panel()
        Me.pnlArchiveFolder = New System.Windows.Forms.Panel()
        Me.lblArchiveFolder = New System.Windows.Forms.Label()
        Me.cmbArchiveFolder = New System.Windows.Forms.ComboBox()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.pnlMain.SuspendLayout()
        Me.gbReportType.SuspendLayout()
        Me.gbDateRange.SuspendLayout()
        Me.pnlAdditionalCriteria.SuspendLayout()
        Me.pnlArchiveFolder.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.gbReportType)
        Me.pnlMain.Controls.Add(Me.gbDateRange)
        Me.pnlMain.Controls.Add(Me.pnlAdditionalCriteria)
        Me.pnlMain.Controls.Add(Me.pnlButtons)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlMain.Size = New System.Drawing.Size(467, 350)
        Me.pnlMain.TabIndex = 0
        '
        'gbReportType
        '
        Me.gbReportType.Controls.Add(Me.cmbReportType)
        Me.gbReportType.Controls.Add(Me.lblReportType)
        Me.gbReportType.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbReportType.Location = New System.Drawing.Point(10, 170)
        Me.gbReportType.Name = "gbReportType"
        Me.gbReportType.Size = New System.Drawing.Size(447, 60)
        Me.gbReportType.TabIndex = 0
        Me.gbReportType.TabStop = False
        Me.gbReportType.Text = "نوع التقرير"
        '
        'cmbReportType
        '
        Me.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReportType.FormattingEnabled = True
        Me.cmbReportType.Location = New System.Drawing.Point(20, 22)
        Me.cmbReportType.Name = "cmbReportType"
        Me.cmbReportType.Size = New System.Drawing.Size(350, 25)
        Me.cmbReportType.TabIndex = 1
        '
        'lblReportType
        '
        Me.lblReportType.AutoSize = True
        Me.lblReportType.Location = New System.Drawing.Point(380, 25)
        Me.lblReportType.Name = "lblReportType"
        Me.lblReportType.Size = New System.Drawing.Size(37, 17)
        Me.lblReportType.TabIndex = 0
        Me.lblReportType.Text = "النوع:"
        '
        'gbDateRange
        '
        Me.gbDateRange.Controls.Add(Me.dtpEndDate)
        Me.gbDateRange.Controls.Add(Me.dtpStartDate)
        Me.gbDateRange.Controls.Add(Me.lblToDate)
        Me.gbDateRange.Controls.Add(Me.lblFromDate)
        Me.gbDateRange.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbDateRange.Location = New System.Drawing.Point(10, 110)
        Me.gbDateRange.Name = "gbDateRange"
        Me.gbDateRange.Size = New System.Drawing.Size(447, 60)
        Me.gbDateRange.TabIndex = 1
        Me.gbDateRange.TabStop = False
        Me.gbDateRange.Text = "نطاق التاريخ"
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(20, 22)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(150, 25)
        Me.dtpEndDate.TabIndex = 3
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(220, 22)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(150, 25)
        Me.dtpStartDate.TabIndex = 2
        '
        'lblToDate
        '
        Me.lblToDate.AutoSize = True
        Me.lblToDate.Location = New System.Drawing.Point(180, 25)
        Me.lblToDate.Name = "lblToDate"
        Me.lblToDate.Size = New System.Drawing.Size(29, 17)
        Me.lblToDate.TabIndex = 1
        Me.lblToDate.Text = "إلى:"
        '
        'lblFromDate
        '
        Me.lblFromDate.AutoSize = True
        Me.lblFromDate.Location = New System.Drawing.Point(380, 25)
        Me.lblFromDate.Name = "lblFromDate"
        Me.lblFromDate.Size = New System.Drawing.Size(28, 17)
        Me.lblFromDate.TabIndex = 0
        Me.lblFromDate.Text = "من:"
        '
        'pnlAdditionalCriteria
        '
        Me.pnlAdditionalCriteria.Controls.Add(Me.pnlArchiveFolder)
        Me.pnlAdditionalCriteria.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlAdditionalCriteria.Location = New System.Drawing.Point(10, 10)
        Me.pnlAdditionalCriteria.Name = "pnlAdditionalCriteria"
        Me.pnlAdditionalCriteria.Size = New System.Drawing.Size(447, 100)
        Me.pnlAdditionalCriteria.TabIndex = 2
        Me.pnlAdditionalCriteria.Visible = False
        '
        'pnlArchiveFolder
        '
        Me.pnlArchiveFolder.Controls.Add(Me.lblArchiveFolder)
        Me.pnlArchiveFolder.Controls.Add(Me.cmbArchiveFolder)
        Me.pnlArchiveFolder.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlArchiveFolder.Location = New System.Drawing.Point(0, 0)
        Me.pnlArchiveFolder.Name = "pnlArchiveFolder"
        Me.pnlArchiveFolder.Size = New System.Drawing.Size(447, 50)
        Me.pnlArchiveFolder.TabIndex = 0
        Me.pnlArchiveFolder.Visible = False
        '
        'lblArchiveFolder
        '
        Me.lblArchiveFolder.AutoSize = True
        Me.lblArchiveFolder.Location = New System.Drawing.Point(350, 15)
        Me.lblArchiveFolder.Name = "lblArchiveFolder"
        Me.lblArchiveFolder.Size = New System.Drawing.Size(84, 17)
        Me.lblArchiveFolder.TabIndex = 1
        Me.lblArchiveFolder.Text = "مجلد الأرشيف:"
        '
        'cmbArchiveFolder
        '
        Me.cmbArchiveFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbArchiveFolder.FormattingEnabled = True
        Me.cmbArchiveFolder.Location = New System.Drawing.Point(20, 12)
        Me.cmbArchiveFolder.Name = "cmbArchiveFolder"
        Me.cmbArchiveFolder.Size = New System.Drawing.Size(320, 25)
        Me.cmbArchiveFolder.TabIndex = 0
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.btnCancel)
        Me.pnlButtons.Controls.Add(Me.btnNext)
        Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlButtons.Location = New System.Drawing.Point(10, 270)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(447, 70)
        Me.pnlButtons.TabIndex = 3
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(167, 20)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "إلغاء"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Location = New System.Drawing.Point(277, 20)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(100, 30)
        Me.btnNext.TabIndex = 0
        Me.btnNext.Text = "التالي"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'ReportSelectionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(467, 350)
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ReportSelectionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "اختيار التقرير"
        Me.pnlMain.ResumeLayout(False)
        Me.gbReportType.ResumeLayout(False)
        Me.gbReportType.PerformLayout()
        Me.gbDateRange.ResumeLayout(False)
        Me.gbDateRange.PerformLayout()
        Me.pnlAdditionalCriteria.ResumeLayout(False)
        Me.pnlArchiveFolder.ResumeLayout(False)
        Me.pnlArchiveFolder.PerformLayout()
        Me.pnlButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlMain As Panel
    Friend WithEvents gbReportType As GroupBox
    Friend WithEvents cmbReportType As ComboBox
    Friend WithEvents lblReportType As Label
    Friend WithEvents gbDateRange As GroupBox
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents lblToDate As Label
    Friend WithEvents lblFromDate As Label
    Friend WithEvents pnlAdditionalCriteria As Panel
    Friend WithEvents pnlArchiveFolder As Panel
    Friend WithEvents lblArchiveFolder As Label
    Friend WithEvents cmbArchiveFolder As ComboBox
    Friend WithEvents pnlButtons As Panel
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnNext As Button
End Class