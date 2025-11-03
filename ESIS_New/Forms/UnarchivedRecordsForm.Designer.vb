<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnarchivedRecordsForm
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
        Me.dgvUnarchivedRecords = New System.Windows.Forms.DataGridView()
        Me.lblRecordCount = New System.Windows.Forms.Label()
        Me.btnArchiveSelected = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me.dgvUnarchivedRecords, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvUnarchivedRecords
        '
        Me.dgvUnarchivedRecords.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvUnarchivedRecords.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.dgvUnarchivedRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUnarchivedRecords.Location = New System.Drawing.Point(9, 39)
        Me.dgvUnarchivedRecords.Name = "dgvUnarchivedRecords"
        Me.dgvUnarchivedRecords.Size = New System.Drawing.Size(806, 250)
        Me.dgvUnarchivedRecords.TabIndex = 0
        '
        'lblRecordCount
        '
        Me.lblRecordCount.AutoSize = True
        Me.lblRecordCount.Location = New System.Drawing.Point(12, 9)
        Me.lblRecordCount.Name = "lblRecordCount"
        Me.lblRecordCount.Size = New System.Drawing.Size(171, 24)
        Me.lblRecordCount.TabIndex = 1
        Me.lblRecordCount.Text = "عدد السجلات غير المؤرشفة: 0"
        '
        'btnArchiveSelected
        '
        Me.btnArchiveSelected.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnArchiveSelected.FlatAppearance.BorderSize = 0
        Me.btnArchiveSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnArchiveSelected.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnArchiveSelected.Location = New System.Drawing.Point(590, 7)
        Me.btnArchiveSelected.Name = "btnArchiveSelected"
        Me.btnArchiveSelected.Size = New System.Drawing.Size(182, 34)
        Me.btnArchiveSelected.TabIndex = 2
        Me.btnArchiveSelected.Text = "فتح نموذج الصادر للارشفة"
        Me.btnArchiveSelected.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(170, Byte), Integer))
        Me.btnRefresh.FlatAppearance.BorderSize = 0
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnRefresh.Location = New System.Drawing.Point(476, 7)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 34)
        Me.btnRefresh.TabIndex = 4
        Me.btnRefresh.Text = "تحديث"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnRefresh)
        Me.Panel1.Controls.Add(Me.btnArchiveSelected)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 313)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(819, 49)
        Me.Panel1.TabIndex = 5
        '
        'UnarchivedRecordsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(819, 362)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.dgvUnarchivedRecords)
        Me.Controls.Add(Me.lblRecordCount)
        Me.Font = New System.Drawing.Font("Cairo SemiBold", 9.749999!, System.Drawing.FontStyle.Bold)
        Me.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(835, 401)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(835, 401)
        Me.Name = "UnarchivedRecordsForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "نموذج السجلات الغير مؤرشفة"
        CType(Me.dgvUnarchivedRecords, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvUnarchivedRecords As DataGridView
    Friend WithEvents lblRecordCount As Label
    Friend WithEvents btnArchiveSelected As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents Panel1 As Panel
End Class
