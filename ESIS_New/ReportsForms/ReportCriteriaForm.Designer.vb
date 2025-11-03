<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ReportCriteriaForm
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
        Me.gbCriteria = New System.Windows.Forms.GroupBox()
        Me.pnlArchiveFolder = New System.Windows.Forms.Panel()
        Me.lblArchiveFolder = New System.Windows.Forms.Label()
        Me.cmbArchiveFolder = New System.Windows.Forms.ComboBox()
        Me.pnlWillSub = New System.Windows.Forms.Panel()
        Me.lblWillSub = New System.Windows.Forms.Label()
        Me.cmbWillSub = New System.Windows.Forms.ComboBox()
        Me.pnlWillMaster = New System.Windows.Forms.Panel()
        Me.lblWillMaster = New System.Windows.Forms.Label()
        Me.cmbWillMaster = New System.Windows.Forms.ComboBox()
        Me.pnlBookCategory = New System.Windows.Forms.Panel()
        Me.lblBookCategory = New System.Windows.Forms.Label()
        Me.cmbBookCategory = New System.Windows.Forms.ComboBox()
        Me.pnlDateRange = New System.Windows.Forms.Panel()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.lblToDate = New System.Windows.Forms.Label()
        Me.lblFromDate = New System.Windows.Forms.Label()
        Me.pnlReportTitle = New System.Windows.Forms.Panel()
        Me.txtReportTitle = New System.Windows.Forms.TextBox()
        Me.lblReportTitle = New System.Windows.Forms.Label()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnConfirm = New System.Windows.Forms.Button()
        Me.pnlMain.SuspendLayout()
        Me.gbCriteria.SuspendLayout()
        Me.pnlArchiveFolder.SuspendLayout()
        Me.pnlWillSub.SuspendLayout()
        Me.pnlWillMaster.SuspendLayout()
        Me.pnlBookCategory.SuspendLayout()
        Me.pnlDateRange.SuspendLayout()
        Me.pnlReportTitle.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.gbCriteria)
        Me.pnlMain.Controls.Add(Me.pnlButtons)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlMain.Size = New System.Drawing.Size(521, 450)
        Me.pnlMain.TabIndex = 0
        '
        'gbCriteria
        '
        Me.gbCriteria.Controls.Add(Me.pnlArchiveFolder)
        Me.gbCriteria.Controls.Add(Me.pnlWillSub)
        Me.gbCriteria.Controls.Add(Me.pnlWillMaster)
        Me.gbCriteria.Controls.Add(Me.pnlBookCategory)
        Me.gbCriteria.Controls.Add(Me.pnlDateRange)
        Me.gbCriteria.Controls.Add(Me.pnlReportTitle)
        Me.gbCriteria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbCriteria.Location = New System.Drawing.Point(10, 10)
        Me.gbCriteria.Name = "gbCriteria"
        Me.gbCriteria.Size = New System.Drawing.Size(501, 380)
        Me.gbCriteria.TabIndex = 1
        Me.gbCriteria.TabStop = False
        Me.gbCriteria.Text = "معاير التقرير"
        '
        'pnlArchiveFolder
        '
        Me.pnlArchiveFolder.Controls.Add(Me.lblArchiveFolder)
        Me.pnlArchiveFolder.Controls.Add(Me.cmbArchiveFolder)
        Me.pnlArchiveFolder.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlArchiveFolder.Location = New System.Drawing.Point(3, 251)
        Me.pnlArchiveFolder.Name = "pnlArchiveFolder"
        Me.pnlArchiveFolder.Size = New System.Drawing.Size(495, 50)
        Me.pnlArchiveFolder.TabIndex = 5
        Me.pnlArchiveFolder.Visible = False
        '
        'lblArchiveFolder
        '
        Me.lblArchiveFolder.AutoSize = True
        Me.lblArchiveFolder.Location = New System.Drawing.Point(380, 15)
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
        Me.cmbArchiveFolder.Size = New System.Drawing.Size(350, 25)
        Me.cmbArchiveFolder.TabIndex = 0
        '
        'pnlWillSub
        '
        Me.pnlWillSub.Controls.Add(Me.lblWillSub)
        Me.pnlWillSub.Controls.Add(Me.cmbWillSub)
        Me.pnlWillSub.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlWillSub.Location = New System.Drawing.Point(3, 201)
        Me.pnlWillSub.Name = "pnlWillSub"
        Me.pnlWillSub.Size = New System.Drawing.Size(495, 50)
        Me.pnlWillSub.TabIndex = 4
        Me.pnlWillSub.Visible = False
        '
        'lblWillSub
        '
        Me.lblWillSub.AutoSize = True
        Me.lblWillSub.Location = New System.Drawing.Point(380, 15)
        Me.lblWillSub.Name = "lblWillSub"
        Me.lblWillSub.Size = New System.Drawing.Size(102, 17)
        Me.lblWillSub.TabIndex = 1
        Me.lblWillSub.Text = "البوليصة الفرعية :"
        '
        'cmbWillSub
        '
        Me.cmbWillSub.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbWillSub.FormattingEnabled = True
        Me.cmbWillSub.Location = New System.Drawing.Point(20, 12)
        Me.cmbWillSub.Name = "cmbWillSub"
        Me.cmbWillSub.Size = New System.Drawing.Size(350, 25)
        Me.cmbWillSub.TabIndex = 0
        '
        'pnlWillMaster
        '
        Me.pnlWillMaster.Controls.Add(Me.lblWillMaster)
        Me.pnlWillMaster.Controls.Add(Me.cmbWillMaster)
        Me.pnlWillMaster.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlWillMaster.Location = New System.Drawing.Point(3, 151)
        Me.pnlWillMaster.Name = "pnlWillMaster"
        Me.pnlWillMaster.Size = New System.Drawing.Size(495, 50)
        Me.pnlWillMaster.TabIndex = 3
        Me.pnlWillMaster.Visible = False
        '
        'lblWillMaster
        '
        Me.lblWillMaster.AutoSize = True
        Me.lblWillMaster.Location = New System.Drawing.Point(380, 15)
        Me.lblWillMaster.Name = "lblWillMaster"
        Me.lblWillMaster.Size = New System.Drawing.Size(106, 17)
        Me.lblWillMaster.TabIndex = 1
        Me.lblWillMaster.Text = "البوليصة الرئيسية :"
        '
        'cmbWillMaster
        '
        Me.cmbWillMaster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbWillMaster.FormattingEnabled = True
        Me.cmbWillMaster.Location = New System.Drawing.Point(20, 12)
        Me.cmbWillMaster.Name = "cmbWillMaster"
        Me.cmbWillMaster.Size = New System.Drawing.Size(350, 25)
        Me.cmbWillMaster.TabIndex = 0
        '
        'pnlBookCategory
        '
        Me.pnlBookCategory.Controls.Add(Me.lblBookCategory)
        Me.pnlBookCategory.Controls.Add(Me.cmbBookCategory)
        Me.pnlBookCategory.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlBookCategory.Location = New System.Drawing.Point(3, 101)
        Me.pnlBookCategory.Name = "pnlBookCategory"
        Me.pnlBookCategory.Size = New System.Drawing.Size(495, 50)
        Me.pnlBookCategory.TabIndex = 2
        Me.pnlBookCategory.Visible = False
        '
        'lblBookCategory
        '
        Me.lblBookCategory.AutoSize = True
        Me.lblBookCategory.Location = New System.Drawing.Point(380, 15)
        Me.lblBookCategory.Name = "lblBookCategory"
        Me.lblBookCategory.Size = New System.Drawing.Size(86, 17)
        Me.lblBookCategory.TabIndex = 1
        Me.lblBookCategory.Text = "تصنيف الكتاب:"
        '
        'cmbBookCategory
        '
        Me.cmbBookCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBookCategory.FormattingEnabled = True
        Me.cmbBookCategory.Location = New System.Drawing.Point(20, 12)
        Me.cmbBookCategory.Name = "cmbBookCategory"
        Me.cmbBookCategory.Size = New System.Drawing.Size(350, 25)
        Me.cmbBookCategory.TabIndex = 0
        '
        'pnlDateRange
        '
        Me.pnlDateRange.Controls.Add(Me.dtpEndDate)
        Me.pnlDateRange.Controls.Add(Me.dtpStartDate)
        Me.pnlDateRange.Controls.Add(Me.lblToDate)
        Me.pnlDateRange.Controls.Add(Me.lblFromDate)
        Me.pnlDateRange.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlDateRange.Location = New System.Drawing.Point(3, 51)
        Me.pnlDateRange.Name = "pnlDateRange"
        Me.pnlDateRange.Size = New System.Drawing.Size(495, 50)
        Me.pnlDateRange.TabIndex = 1
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(20, 12)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(150, 25)
        Me.dtpEndDate.TabIndex = 3
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(220, 12)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(150, 25)
        Me.dtpStartDate.TabIndex = 2
        '
        'lblToDate
        '
        Me.lblToDate.AutoSize = True
        Me.lblToDate.Location = New System.Drawing.Point(180, 15)
        Me.lblToDate.Name = "lblToDate"
        Me.lblToDate.Size = New System.Drawing.Size(29, 17)
        Me.lblToDate.TabIndex = 1
        Me.lblToDate.Text = "إلى:"
        '
        'lblFromDate
        '
        Me.lblFromDate.AutoSize = True
        Me.lblFromDate.Location = New System.Drawing.Point(380, 15)
        Me.lblFromDate.Name = "lblFromDate"
        Me.lblFromDate.Size = New System.Drawing.Size(28, 17)
        Me.lblFromDate.TabIndex = 0
        Me.lblFromDate.Text = "من:"
        '
        'pnlReportTitle
        '
        Me.pnlReportTitle.Controls.Add(Me.txtReportTitle)
        Me.pnlReportTitle.Controls.Add(Me.lblReportTitle)
        Me.pnlReportTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlReportTitle.Location = New System.Drawing.Point(3, 21)
        Me.pnlReportTitle.Name = "pnlReportTitle"
        Me.pnlReportTitle.Size = New System.Drawing.Size(495, 30)
        Me.pnlReportTitle.TabIndex = 0
        '
        'txtReportTitle
        '
        Me.txtReportTitle.Location = New System.Drawing.Point(20, 2)
        Me.txtReportTitle.Name = "txtReportTitle"
        Me.txtReportTitle.Size = New System.Drawing.Size(350, 25)
        Me.txtReportTitle.TabIndex = 1
        Me.txtReportTitle.Text = "تقرير"
        '
        'lblReportTitle
        '
        Me.lblReportTitle.AutoSize = True
        Me.lblReportTitle.Location = New System.Drawing.Point(380, 5)
        Me.lblReportTitle.Name = "lblReportTitle"
        Me.lblReportTitle.Size = New System.Drawing.Size(79, 17)
        Me.lblReportTitle.TabIndex = 0
        Me.lblReportTitle.Text = "عنوان التقرير:"
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.btnCancel)
        Me.pnlButtons.Controls.Add(Me.btnConfirm)
        Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlButtons.Location = New System.Drawing.Point(10, 390)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(501, 50)
        Me.pnlButtons.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(151, 10)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "إلغاء"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnConfirm
        '
        Me.btnConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConfirm.Location = New System.Drawing.Point(261, 10)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(100, 30)
        Me.btnConfirm.TabIndex = 0
        Me.btnConfirm.Text = "تأكيد"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'ReportCriteriaForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 450)
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ReportCriteriaForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "معاير التقرير"
        Me.pnlMain.ResumeLayout(False)
        Me.gbCriteria.ResumeLayout(False)
        Me.pnlArchiveFolder.ResumeLayout(False)
        Me.pnlArchiveFolder.PerformLayout()
        Me.pnlWillSub.ResumeLayout(False)
        Me.pnlWillSub.PerformLayout()
        Me.pnlWillMaster.ResumeLayout(False)
        Me.pnlWillMaster.PerformLayout()
        Me.pnlBookCategory.ResumeLayout(False)
        Me.pnlBookCategory.PerformLayout()
        Me.pnlDateRange.ResumeLayout(False)
        Me.pnlDateRange.PerformLayout()
        Me.pnlReportTitle.ResumeLayout(False)
        Me.pnlReportTitle.PerformLayout()
        Me.pnlButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlMain As Panel
    Friend WithEvents gbCriteria As GroupBox
    Friend WithEvents pnlButtons As Panel
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnConfirm As Button
    Friend WithEvents pnlReportTitle As Panel
    Friend WithEvents txtReportTitle As TextBox
    Friend WithEvents lblReportTitle As Label
    Friend WithEvents pnlDateRange As Panel
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents lblToDate As Label
    Friend WithEvents lblFromDate As Label
    Friend WithEvents pnlBookCategory As Panel
    Friend WithEvents lblBookCategory As Label
    Friend WithEvents cmbBookCategory As ComboBox
    Friend WithEvents pnlWillMaster As Panel
    Friend WithEvents lblWillMaster As Label
    Friend WithEvents cmbWillMaster As ComboBox
    Friend WithEvents pnlWillSub As Panel
    Friend WithEvents lblWillSub As Label
    Friend WithEvents cmbWillSub As ComboBox
    Friend WithEvents pnlArchiveFolder As Panel
    Friend WithEvents lblArchiveFolder As Label
    Friend WithEvents cmbArchiveFolder As ComboBox
End Class