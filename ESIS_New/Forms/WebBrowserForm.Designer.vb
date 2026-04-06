<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class WebBrowserForm
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
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.btnBack = New System.Windows.Forms.ToolStripButton()
        Me.btnForward = New System.Windows.Forms.ToolStripButton()
        Me.btnRefresh = New System.Windows.Forms.ToolStripButton()
        Me.btnHome = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.txtUrl = New System.Windows.Forms.ToolStripTextBox()
        Me.btnGo = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnPrint = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnOpenInBrowser = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnShowHistory = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnAddToFavorites = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnShowFavorites = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.statusBar = New System.Windows.Forms.StatusStrip()
        Me.statusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.pageLoader = New System.Windows.Forms.ProgressBar()
        Me.ToolStrip.SuspendLayout()
        Me.statusBar.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip
        '
        Me.ToolStrip.Font = New System.Drawing.Font("Cairo SemiBold", 12.0!, System.Drawing.FontStyle.Bold)
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnBack, Me.btnForward, Me.btnRefresh, Me.btnHome, Me.ToolStripSeparator1, Me.txtUrl, Me.btnGo, Me.ToolStripSeparator2, Me.btnPrint, Me.ToolStripSeparator4, Me.btnOpenInBrowser, Me.ToolStripSeparator5, Me.btnClose, Me.btnShowHistory, Me.ToolStripSeparator6, Me.btnAddToFavorites, Me.ToolStripSeparator7, Me.btnShowFavorites, Me.ToolStripSeparator8})
        Me.ToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(1298, 37)
        Me.ToolStrip.TabIndex = 1
        Me.ToolStrip.Text = "ToolStrip"
        '
        'btnBack
        '
        Me.btnBack.AutoSize = False
        Me.btnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnBack.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.back
        Me.btnBack.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(34, 34)
        Me.btnBack.Text = "العودة للصفحة السابقة"
        '
        'btnForward
        '
        Me.btnForward.AutoSize = False
        Me.btnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnForward.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.arrow
        Me.btnForward.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnForward.Name = "btnForward"
        Me.btnForward.Size = New System.Drawing.Size(34, 34)
        Me.btnForward.Text = "التقدم للصفحة التالية"
        '
        'btnRefresh
        '
        Me.btnRefresh.AutoSize = False
        Me.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnRefresh.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.refresh_button
        Me.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(34, 34)
        Me.btnRefresh.Text = "تحديث الصفحة"
        '
        'btnHome
        '
        Me.btnHome.AutoSize = False
        Me.btnHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnHome.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.home
        Me.btnHome.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnHome.Name = "btnHome"
        Me.btnHome.Size = New System.Drawing.Size(34, 34)
        Me.btnHome.Text = "الصفحة الرئيسية"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 37)
        '
        'txtUrl
        '
        Me.txtUrl.AutoSize = False
        Me.txtUrl.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUrl.Name = "txtUrl"
        Me.txtUrl.Size = New System.Drawing.Size(700, 37)
        Me.txtUrl.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnGo
        '
        Me.btnGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnGo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(55, 34)
        Me.btnGo.Text = "اذهب"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 37)
        '
        'btnPrint
        '
        Me.btnPrint.AutoSize = False
        Me.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnPrint.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.printer
        Me.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(34, 34)
        Me.btnPrint.Text = "طباعة "
        Me.btnPrint.ToolTipText = "طباعة | F8"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 37)
        '
        'btnOpenInBrowser
        '
        Me.btnOpenInBrowser.AutoSize = False
        Me.btnOpenInBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnOpenInBrowser.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.open_in_browser
        Me.btnOpenInBrowser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnOpenInBrowser.Name = "btnOpenInBrowser"
        Me.btnOpenInBrowser.Size = New System.Drawing.Size(34, 34)
        Me.btnOpenInBrowser.Text = "فتح في المتصفح الافتراضي"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 37)
        '
        'btnClose
        '
        Me.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnClose.AutoSize = False
        Me.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnClose.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.close_50px
        Me.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(34, 34)
        Me.btnClose.Text = "إغلاق"
        '
        'btnShowHistory
        '
        Me.btnShowHistory.AutoSize = False
        Me.btnShowHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnShowHistory.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.history
        Me.btnShowHistory.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnShowHistory.Name = "btnShowHistory"
        Me.btnShowHistory.Size = New System.Drawing.Size(34, 34)
        Me.btnShowHistory.Text = "عرض سجل التصفح"
        Me.btnShowHistory.ToolTipText = "عرض سجل التصفح | Ctrl + H"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 37)
        '
        'btnAddToFavorites
        '
        Me.btnAddToFavorites.AutoSize = False
        Me.btnAddToFavorites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAddToFavorites.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.star
        Me.btnAddToFavorites.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAddToFavorites.Name = "btnAddToFavorites"
        Me.btnAddToFavorites.Size = New System.Drawing.Size(34, 34)
        Me.btnAddToFavorites.Text = "إضافة للمفضلة"
        Me.btnAddToFavorites.ToolTipText = "إضافة للمفضلة | Ctrl + D"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 37)
        '
        'btnShowFavorites
        '
        Me.btnShowFavorites.AutoSize = False
        Me.btnShowFavorites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnShowFavorites.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.favorite
        Me.btnShowFavorites.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnShowFavorites.Name = "btnShowFavorites"
        Me.btnShowFavorites.Size = New System.Drawing.Size(34, 34)
        Me.btnShowFavorites.Text = "عرض المفضلة"
        Me.btnShowFavorites.ToolTipText = "عرض المفضلة | Ctrl + B"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 37)
        '
        'statusBar
        '
        Me.statusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.statusLabel})
        Me.statusBar.Location = New System.Drawing.Point(0, 682)
        Me.statusBar.Name = "statusBar"
        Me.statusBar.Size = New System.Drawing.Size(1298, 22)
        Me.statusBar.TabIndex = 2
        Me.statusBar.Text = "StatusStrip1"
        '
        'statusLabel
        '
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(0, 17)
        '
        'TabControl1
        '
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 37)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1298, 645)
        Me.TabControl1.TabIndex = 3
        '
        'pageLoader
        '
        Me.pageLoader.BackColor = System.Drawing.Color.DodgerBlue
        Me.pageLoader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pageLoader.Location = New System.Drawing.Point(0, 37)
        Me.pageLoader.Name = "pageLoader"
        Me.pageLoader.Size = New System.Drawing.Size(1298, 3)
        Me.pageLoader.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pageLoader.TabIndex = 4
        Me.pageLoader.Visible = False
        '
        'WebBrowserForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1298, 704)
        Me.Controls.Add(Me.pageLoader)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.ToolStrip)
        Me.Controls.Add(Me.statusBar)
        Me.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "WebBrowserForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "متصفح الويب"
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.statusBar.ResumeLayout(False)
        Me.statusBar.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip As ToolStrip
    Friend WithEvents btnBack As ToolStripButton
    Friend WithEvents btnForward As ToolStripButton
    Friend WithEvents btnRefresh As ToolStripButton
    Friend WithEvents btnHome As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents txtUrl As ToolStripTextBox
    Friend WithEvents btnGo As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents btnPrint As ToolStripButton
    Friend WithEvents btnOpenInBrowser As ToolStripButton
    Friend WithEvents btnClose As ToolStripButton
    Friend WithEvents statusBar As StatusStrip
    Friend WithEvents statusLabel As ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents pageLoader As ProgressBar
    Friend WithEvents btnShowHistory As ToolStripButton
    Friend WithEvents btnAddToFavorites As ToolStripButton
    Friend WithEvents btnShowFavorites As ToolStripButton
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
End Class