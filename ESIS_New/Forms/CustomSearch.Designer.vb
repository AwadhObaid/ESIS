<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CustomSearch
    Inherits System.Windows.Forms.Form

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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnCancel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnOpenLoction = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnPrint = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.searchTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.LabelFileName = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnChooseFolder = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TxtMsg = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.progressLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.backgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.resultsTreeView = New System.Windows.Forms.TreeView()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.pdfViewer = New PdfiumViewer.PdfViewer()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1327, 37)
        Me.Panel1.TabIndex = 11
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ToolStrip1.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCancel, Me.ToolStripSeparator1, Me.btnOpenLoction, Me.ToolStripSeparator2, Me.btnPrint, Me.ToolStripSeparator3, Me.ToolStripLabel1, Me.searchTextBox, Me.ToolStripSeparator4, Me.ToolStripLabel2, Me.ToolStripSeparator6, Me.LabelFileName, Me.ToolStripSeparator5, Me.btnChooseFolder})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1327, 42)
        Me.ToolStrip1.TabIndex = 7
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnCancel
        '
        Me.btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnCancel.Font = New System.Drawing.Font("Cairo", 7.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Padding = New System.Windows.Forms.Padding(8)
        Me.btnCancel.Size = New System.Drawing.Size(77, 39)
        Me.btnCancel.Text = "إلغاء البحث"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 42)
        '
        'btnOpenLoction
        '
        Me.btnOpenLoction.Font = New System.Drawing.Font("Cairo", 7.0!, System.Drawing.FontStyle.Bold)
        Me.btnOpenLoction.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnOpenLoction.Name = "btnOpenLoction"
        Me.btnOpenLoction.Padding = New System.Windows.Forms.Padding(8)
        Me.btnOpenLoction.Size = New System.Drawing.Size(104, 39)
        Me.btnOpenLoction.Text = "فتح موقع الملف"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 42)
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Cairo", 7.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Padding = New System.Windows.Forms.Padding(8)
        Me.btnPrint.Size = New System.Drawing.Size(93, 39)
        Me.btnPrint.Text = "معاينة وطباعة"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 42)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Font = New System.Drawing.Font("Cairo", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(76, 39)
        Me.ToolStripLabel1.Text = "مربع البحث"
        '
        'searchTextBox
        '
        Me.searchTextBox.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.searchTextBox.Name = "searchTextBox"
        Me.searchTextBox.Size = New System.Drawing.Size(150, 42)
        Me.searchTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 42)
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Font = New System.Drawing.Font("Cairo", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(84, 39)
        Me.ToolStripLabel2.Text = "مســار الملف"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 42)
        '
        'LabelFileName
        '
        Me.LabelFileName.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelFileName.Name = "LabelFileName"
        Me.LabelFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelFileName.Size = New System.Drawing.Size(42, 39)
        Me.LabelFileName.Text = "المسار"
        Me.LabelFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 42)
        '
        'btnChooseFolder
        '
        Me.btnChooseFolder.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnChooseFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnChooseFolder.Font = New System.Drawing.Font("Cairo", 7.0!, System.Drawing.FontStyle.Bold)
        Me.btnChooseFolder.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.iconfinder_Open_file_132300
        Me.btnChooseFolder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnChooseFolder.Name = "btnChooseFolder"
        Me.btnChooseFolder.Size = New System.Drawing.Size(87, 39)
        Me.btnChooseFolder.Text = "اختيار مجلد البحث"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.StatusStrip1.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.TxtMsg, Me.ProgressBar1, Me.progressLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 9)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1327, 23)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.ForeColor = System.Drawing.Color.DarkBlue
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(73, 18)
        Me.ToolStripStatusLabel1.Text = "حالة البحث"
        '
        'TxtMsg
        '
        Me.TxtMsg.ForeColor = System.Drawing.Color.Crimson
        Me.TxtMsg.Name = "TxtMsg"
        Me.TxtMsg.Size = New System.Drawing.Size(32, 18)
        Me.TxtMsg.Text = "..."
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(200, 17)
        '
        'progressLabel
        '
        Me.progressLabel.AutoSize = False
        Me.progressLabel.Name = "progressLabel"
        Me.progressLabel.Size = New System.Drawing.Size(50, 18)
        Me.progressLabel.Text = "0"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.StatusStrip1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 614)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1327, 32)
        Me.Panel4.TabIndex = 10
        '
        'resultsTreeView
        '
        Me.resultsTreeView.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.resultsTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.resultsTreeView.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.resultsTreeView.ItemHeight = 40
        Me.resultsTreeView.Location = New System.Drawing.Point(0, 0)
        Me.resultsTreeView.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.resultsTreeView.Name = "resultsTreeView"
        Me.resultsTreeView.RightToLeftLayout = True
        Me.resultsTreeView.Size = New System.Drawing.Size(592, 577)
        Me.resultsTreeView.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.resultsTreeView)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(592, 577)
        Me.Panel2.TabIndex = 4
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 37)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel2)
        Me.SplitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.SplitContainer1.Panel1MinSize = 420
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.pdfViewer)
        Me.SplitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.SplitContainer1.Size = New System.Drawing.Size(1327, 577)
        Me.SplitContainer1.SplitterDistance = 592
        Me.SplitContainer1.TabIndex = 9
        '
        'pdfViewer
        '
        Me.pdfViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pdfViewer.Location = New System.Drawing.Point(0, 0)
        Me.pdfViewer.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pdfViewer.MinimumSize = New System.Drawing.Size(20, 13)
        Me.pdfViewer.Name = "pdfViewer"
        Me.pdfViewer.Size = New System.Drawing.Size(731, 577)
        Me.pdfViewer.TabIndex = 3
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'CustomSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1327, 646)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Name = "CustomSearch"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "بحث مخصص"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    ' تعريفات العناصر
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend Shadows WithEvents btnCancel As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents btnOpenLoction As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents btnPrint As ToolStripButton
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents searchTextBox As ToolStripTextBox
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripLabel2 As ToolStripLabel
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents LabelFileName As ToolStripLabel
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents TxtMsg As ToolStripStatusLabel
    Friend WithEvents ProgressBar1 As ToolStripProgressBar
    Friend WithEvents progressLabel As ToolStripStatusLabel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents backgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents resultsTreeView As TreeView
    Friend WithEvents Panel2 As Panel
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents pdfViewer As PdfiumViewer.PdfViewer
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents btnChooseFolder As ToolStripButton
End Class
