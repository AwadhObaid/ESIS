<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ReportViewerForm
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
        Me.crystalReportViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsClose = New System.Windows.Forms.ToolStripButton()
        Me.tsPrint = New System.Windows.Forms.ToolStripButton()
        Me.tsExportPDF = New System.Windows.Forms.ToolStripButton()
        Me.ToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'crystalReportViewer
        '
        Me.crystalReportViewer.ActiveViewIndex = -1
        Me.crystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crystalReportViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.crystalReportViewer.DisplayStatusBar = False
        Me.crystalReportViewer.DisplayToolbar = False
        Me.crystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.crystalReportViewer.Location = New System.Drawing.Point(0, 25)
        Me.crystalReportViewer.Name = "crystalReportViewer"
        Me.crystalReportViewer.Size = New System.Drawing.Size(1000, 600)
        Me.crystalReportViewer.TabIndex = 0
        Me.crystalReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        '
        'ToolStrip
        '
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsPrint, Me.tsExportPDF, Me.ToolStripSeparator1, Me.tsClose})
        Me.ToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(1000, 25)
        Me.ToolStrip.TabIndex = 1
        Me.ToolStrip.Text = "ToolStrip"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsClose
        '
        Me.tsClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsClose.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.close_50px
        Me.tsClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsClose.Name = "tsClose"
        Me.tsClose.Size = New System.Drawing.Size(23, 22)
        Me.tsClose.Text = "إغلاق"
        '
        'tsPrint
        '
        Me.tsPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsPrint.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.printer
        Me.tsPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsPrint.Name = "tsPrint"
        Me.tsPrint.Size = New System.Drawing.Size(23, 22)
        Me.tsPrint.Text = "طباعة"
        '
        'tsExportPDF
        '
        Me.tsExportPDF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsExportPDF.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.file
        Me.tsExportPDF.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsExportPDF.Name = "tsExportPDF"
        Me.tsExportPDF.Size = New System.Drawing.Size(23, 22)
        Me.tsExportPDF.Text = "تصدير إلى PDF"
        '
        'ReportViewerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1000, 625)
        Me.Controls.Add(Me.crystalReportViewer)
        Me.Controls.Add(Me.ToolStrip)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ReportViewerForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "عارض التقارير"
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents crystalReportViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents ToolStrip As ToolStrip
    Friend WithEvents tsPrint As ToolStripButton
    Friend WithEvents tsExportPDF As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents tsClose As ToolStripButton
End Class