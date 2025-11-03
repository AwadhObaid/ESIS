<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DateDifferenceForm
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
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTotalDays = New System.Windows.Forms.Label()
        Me.lblMonths = New System.Windows.Forms.Label()
        Me.lblYears = New System.Windows.Forms.Label()
        Me.lblDays = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnCalc = New System.Windows.Forms.Button()
        Me.lblFridays = New System.Windows.Forms.Label()
        Me.lblSaturdays = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblHolidays = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label6
        '
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label6.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.Location = New System.Drawing.Point(274, 104)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(263, 26)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "صافي الأيام (بدون عطلة)"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label7.Font = New System.Drawing.Font("LBC", 20.0!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.Coral
        Me.Label7.Location = New System.Drawing.Point(83, 74)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(654, 73)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "حساب الفرق بين تاريخين بالأيام"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(882, 535)
        Me.Panel1.TabIndex = 13
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel2.Controls.Add(Me.btnCalc)
        Me.Panel2.Location = New System.Drawing.Point(22, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(839, 502)
        Me.Panel2.TabIndex = 12
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.Controls.Add(Me.lblHolidays, 1, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.Label8, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.Label6, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.lblTotalDays, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.lblMonths, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblYears, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblDays, 2, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.dtpStartDate, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.dtpEndDate, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblFridays, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.lblSaturdays, 2, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.Label10, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Label11, 2, 4)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(13, 154)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 8
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.95349!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.48634!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.11475!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.84699!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.7541!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.13661!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(809, 246)
        Me.TableLayoutPanel1.TabIndex = 13
        '
        'lblTotalDays
        '
        Me.lblTotalDays.BackColor = System.Drawing.Color.DodgerBlue
        Me.lblTotalDays.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblTotalDays.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalDays.ForeColor = System.Drawing.Color.White
        Me.lblTotalDays.Location = New System.Drawing.Point(274, 130)
        Me.lblTotalDays.Name = "lblTotalDays"
        Me.lblTotalDays.Size = New System.Drawing.Size(263, 26)
        Me.lblTotalDays.TabIndex = 11
        Me.lblTotalDays.Text = "0"
        Me.lblTotalDays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMonths
        '
        Me.lblMonths.BackColor = System.Drawing.Color.DodgerBlue
        Me.lblMonths.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblMonths.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblMonths.ForeColor = System.Drawing.Color.White
        Me.lblMonths.Location = New System.Drawing.Point(274, 77)
        Me.lblMonths.Name = "lblMonths"
        Me.lblMonths.Size = New System.Drawing.Size(263, 26)
        Me.lblMonths.TabIndex = 8
        Me.lblMonths.Text = "0"
        Me.lblMonths.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblYears
        '
        Me.lblYears.BackColor = System.Drawing.Color.DodgerBlue
        Me.lblYears.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblYears.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblYears.ForeColor = System.Drawing.Color.White
        Me.lblYears.Location = New System.Drawing.Point(543, 77)
        Me.lblYears.Name = "lblYears"
        Me.lblYears.Size = New System.Drawing.Size(263, 26)
        Me.lblYears.TabIndex = 9
        Me.lblYears.Text = "0"
        Me.lblYears.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDays
        '
        Me.lblDays.BackColor = System.Drawing.Color.DodgerBlue
        Me.lblDays.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblDays.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblDays.ForeColor = System.Drawing.Color.White
        Me.lblDays.Location = New System.Drawing.Point(3, 77)
        Me.lblDays.Name = "lblDays"
        Me.lblDays.Size = New System.Drawing.Size(265, 26)
        Me.lblDays.TabIndex = 7
        Me.lblDays.Text = "0"
        Me.lblDays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label5.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.Location = New System.Drawing.Point(543, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(263, 23)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "السنوات"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label4.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(274, 54)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(263, 23)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "الشهور :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(3, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(265, 23)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "الأيام :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(543, 27)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(263, 25)
        Me.dtpStartDate.TabIndex = 1
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(3, 27)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(265, 25)
        Me.dtpEndDate.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(543, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(263, 24)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "من تاريخ :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(265, 24)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "إلى تاريخ :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnCalc
        '
        Me.btnCalc.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCalc.BackColor = System.Drawing.Color.YellowGreen
        Me.btnCalc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCalc.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnCalc.Location = New System.Drawing.Point(44, 452)
        Me.btnCalc.Name = "btnCalc"
        Me.btnCalc.Size = New System.Drawing.Size(746, 34)
        Me.btnCalc.TabIndex = 0
        Me.btnCalc.Text = "إحسب"
        Me.btnCalc.UseVisualStyleBackColor = False
        '
        'lblFridays
        '
        Me.lblFridays.BackColor = System.Drawing.Color.DodgerBlue
        Me.lblFridays.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblFridays.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblFridays.ForeColor = System.Drawing.Color.White
        Me.lblFridays.Location = New System.Drawing.Point(543, 130)
        Me.lblFridays.Name = "lblFridays"
        Me.lblFridays.Size = New System.Drawing.Size(263, 26)
        Me.lblFridays.TabIndex = 13
        Me.lblFridays.Text = "0"
        Me.lblFridays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSaturdays
        '
        Me.lblSaturdays.BackColor = System.Drawing.Color.DodgerBlue
        Me.lblSaturdays.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblSaturdays.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblSaturdays.ForeColor = System.Drawing.Color.White
        Me.lblSaturdays.Location = New System.Drawing.Point(3, 130)
        Me.lblSaturdays.Name = "lblSaturdays"
        Me.lblSaturdays.Size = New System.Drawing.Size(265, 26)
        Me.lblSaturdays.TabIndex = 14
        Me.lblSaturdays.Text = "0"
        Me.lblSaturdays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label10.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label10.Location = New System.Drawing.Point(543, 104)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(263, 26)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "إجمالي الجمع"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label11.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label11.Location = New System.Drawing.Point(3, 104)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(265, 26)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "إجمالي السبت"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHolidays
        '
        Me.lblHolidays.BackColor = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(214, Byte), Integer))
        Me.lblHolidays.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblHolidays.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblHolidays.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblHolidays.Location = New System.Drawing.Point(274, 197)
        Me.lblHolidays.Name = "lblHolidays"
        Me.lblHolidays.Size = New System.Drawing.Size(263, 49)
        Me.lblHolidays.TabIndex = 14
        Me.lblHolidays.Text = "0"
        Me.lblHolidays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label8.Font = New System.Drawing.Font("LBC", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label8.Location = New System.Drawing.Point(274, 174)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(263, 23)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "الإجمالي مع العطل"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateDifferenceForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(882, 535)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Cairo Medium", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.Name = "DateDifferenceForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "حساب الفرق بين تاريخين بالأيام"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents lblTotalDays As Label
    Friend WithEvents lblMonths As Label
    Friend WithEvents lblYears As Label
    Friend WithEvents lblDays As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents btnCalc As Button
    Friend WithEvents lblFridays As Label
    Friend WithEvents lblSaturdays As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents lblHolidays As Label
    Friend WithEvents Label8 As Label
End Class
