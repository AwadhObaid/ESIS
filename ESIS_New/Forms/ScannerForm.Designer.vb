<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ScannerForm
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnSelectScanner = New System.Windows.Forms.Button()
        Me.btnScan = New System.Windows.Forms.Button()
        Me.btnSavePDF = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lstImages = New System.Windows.Forms.CheckedListBox()
        Me.picPreview = New System.Windows.Forms.PictureBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnScanSettings = New System.Windows.Forms.Button()
        Me.btnPDFPath = New System.Windows.Forms.Button()
        Me.btnDefaultScanner = New System.Windows.Forms.Button()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.lblFileName = New System.Windows.Forms.Label()
        Me.PanelTitle = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.PanelFileName = New System.Windows.Forms.Panel()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.PanelStatus = New System.Windows.Forms.Panel()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.PanelTitle.SuspendLayout()
        Me.PanelFileName.SuspendLayout()
        Me.PanelStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSelectScanner
        '
        Me.btnSelectScanner.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSelectScanner.Font = New System.Drawing.Font("Cairo Medium", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectScanner.Location = New System.Drawing.Point(3, 3)
        Me.btnSelectScanner.Name = "btnSelectScanner"
        Me.btnSelectScanner.Size = New System.Drawing.Size(137, 38)
        Me.btnSelectScanner.TabIndex = 6
        Me.btnSelectScanner.Text = "🔍 اختيار الماسح (TWAIN)"
        '
        'btnScan
        '
        Me.btnScan.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnScan.Enabled = False
        Me.btnScan.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnScan.Location = New System.Drawing.Point(146, 3)
        Me.btnScan.Name = "btnScan"
        Me.btnScan.Size = New System.Drawing.Size(137, 38)
        Me.btnScan.TabIndex = 5
        Me.btnScan.Text = "📄 فتح واجهة المسح"
        '
        'btnSavePDF
        '
        Me.btnSavePDF.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSavePDF.Enabled = False
        Me.btnSavePDF.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSavePDF.Location = New System.Drawing.Point(289, 3)
        Me.btnSavePDF.Name = "btnSavePDF"
        Me.btnSavePDF.Size = New System.Drawing.Size(137, 38)
        Me.btnSavePDF.TabIndex = 4
        Me.btnSavePDF.Text = "💾 حفظ PDF"
        '
        'lblStatus
        '
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblStatus.Font = New System.Drawing.Font("Cairo", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(860, 40)
        Me.lblStatus.TabIndex = 7
        Me.lblStatus.Text = "الحالة: جاهز"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 127)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstImages)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.picPreview)
        Me.SplitContainer1.Size = New System.Drawing.Size(860, 705)
        Me.SplitContainer1.SplitterDistance = 217
        Me.SplitContainer1.TabIndex = 8
        '
        'lstImages
        '
        Me.lstImages.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.lstImages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstImages.FormattingEnabled = True
        Me.lstImages.Location = New System.Drawing.Point(0, 0)
        Me.lstImages.Name = "lstImages"
        Me.lstImages.Size = New System.Drawing.Size(217, 705)
        Me.lstImages.TabIndex = 2
        '
        'picPreview
        '
        Me.picPreview.BackColor = System.Drawing.Color.White
        Me.picPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picPreview.Location = New System.Drawing.Point(0, 0)
        Me.picPreview.Name = "picPreview"
        Me.picPreview.Size = New System.Drawing.Size(639, 705)
        Me.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picPreview.TabIndex = 1
        Me.picPreview.TabStop = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 6
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnSelectScanner, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnScan, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnSavePDF, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnScanSettings, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnPDFPath, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDefaultScanner, 5, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 42)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(860, 44)
        Me.TableLayoutPanel1.TabIndex = 9
        '
        'btnScanSettings
        '
        Me.btnScanSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnScanSettings.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnScanSettings.Location = New System.Drawing.Point(432, 3)
        Me.btnScanSettings.Name = "btnScanSettings"
        Me.btnScanSettings.Size = New System.Drawing.Size(137, 38)
        Me.btnScanSettings.TabIndex = 7
        Me.btnScanSettings.Text = "إعدادات الماسح ⚙️"
        '
        'btnPDFPath
        '
        Me.btnPDFPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnPDFPath.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPDFPath.Location = New System.Drawing.Point(575, 3)
        Me.btnPDFPath.Name = "btnPDFPath"
        Me.btnPDFPath.Size = New System.Drawing.Size(137, 38)
        Me.btnPDFPath.TabIndex = 8
        Me.btnPDFPath.Text = " مسار حفظ الملفات 📁"
        '
        'btnDefaultScanner
        '
        Me.btnDefaultScanner.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDefaultScanner.Font = New System.Drawing.Font("Cairo Medium", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDefaultScanner.Location = New System.Drawing.Point(718, 3)
        Me.btnDefaultScanner.Name = "btnDefaultScanner"
        Me.btnDefaultScanner.Size = New System.Drawing.Size(139, 38)
        Me.btnDefaultScanner.TabIndex = 13
        Me.btnDefaultScanner.Text = "الماسح الافتراضي 📋"
        Me.btnDefaultScanner.UseVisualStyleBackColor = True
        '
        'txtFileName
        '
        Me.txtFileName.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileName.Location = New System.Drawing.Point(168, 4)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(429, 32)
        Me.txtFileName.TabIndex = 10
        Me.txtFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        AddHandler Me.txtFileName.KeyDown, AddressOf Me.txtFileName_KeyDown
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.Location = New System.Drawing.Point(603, 8)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.Size = New System.Drawing.Size(89, 24)
        Me.lblFileName.TabIndex = 12
        Me.lblFileName.Text = ": إسم الملف"
        '
        'PanelTitle
        '
        Me.PanelTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.PanelTitle.Controls.Add(Me.lblTitle)
        Me.PanelTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTitle.Location = New System.Drawing.Point(0, 0)
        Me.PanelTitle.Name = "PanelTitle"
        Me.PanelTitle.Size = New System.Drawing.Size(860, 42)
        Me.PanelTitle.TabIndex = 14
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(339, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(183, 24)
        Me.lblTitle.TabIndex = 13
        Me.lblTitle.Text = "واجهة المسح – TWAIN 🪪"
        '
        'PanelFileName
        '
        Me.PanelFileName.Controls.Add(Me.btnPrint)
        Me.PanelFileName.Controls.Add(Me.lblFileName)
        Me.PanelFileName.Controls.Add(Me.txtFileName)
        Me.PanelFileName.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelFileName.Location = New System.Drawing.Point(0, 86)
        Me.PanelFileName.Name = "PanelFileName"
        Me.PanelFileName.Size = New System.Drawing.Size(860, 41)
        Me.PanelFileName.TabIndex = 15
        '
        'btnPrint
        '
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnPrint.Font = New System.Drawing.Font("Cairo Medium", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(749, 5)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(108, 30)
        Me.btnPrint.TabIndex = 13
        Me.btnPrint.Text = "🖨️ طباعة الصورة"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'PanelStatus
        '
        Me.PanelStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.PanelStatus.Controls.Add(Me.lblStatus)
        Me.PanelStatus.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelStatus.Location = New System.Drawing.Point(0, 832)
        Me.PanelStatus.Name = "PanelStatus"
        Me.PanelStatus.Size = New System.Drawing.Size(860, 40)
        Me.PanelStatus.TabIndex = 2
        '
        'ScannerForm
        '
        Me.ClientSize = New System.Drawing.Size(860, 872)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.PanelFileName)
        Me.Controls.Add(Me.PanelStatus)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.PanelTitle)
        Me.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ScannerForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "واجهة المسح – TWAIN"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.PanelTitle.ResumeLayout(False)
        Me.PanelTitle.PerformLayout()
        Me.PanelFileName.ResumeLayout(False)
        Me.PanelFileName.PerformLayout()
        Me.PanelStatus.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnSelectScanner As Button
    Friend WithEvents btnScan As Button
    Friend WithEvents btnSavePDF As Button
    Friend WithEvents picPreview As PictureBox
    Friend WithEvents lblStatus As Label
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents txtFileName As TextBox
    Friend WithEvents lblFileName As Label
    Friend WithEvents lstImages As CheckedListBox
    Friend WithEvents btnScanSettings As Button
    Friend WithEvents btnPDFPath As Button
    Friend WithEvents btnDefaultScanner As Button
    Friend WithEvents PanelTitle As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents PanelFileName As Panel
    Friend WithEvents PanelStatus As Panel
    Friend WithEvents btnPrint As Button
End Class
