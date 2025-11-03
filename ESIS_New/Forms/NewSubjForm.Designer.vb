<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NewSubjForm
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
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnAddNew = New System.Windows.Forms.Button()
        Me.txtAddNew = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("LBC", 10.0!)
        Me.btnClose.Location = New System.Drawing.Point(226, 132)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(150, 29)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "إغلاق"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("LBC", 12.0!)
        Me.Label1.Location = New System.Drawing.Point(122, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(200, 21)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "إضافة موضوع جديد للقائمة"
        '
        'btnAddNew
        '
        Me.btnAddNew.Font = New System.Drawing.Font("LBC", 10.0!)
        Me.btnAddNew.Location = New System.Drawing.Point(69, 132)
        Me.btnAddNew.Name = "btnAddNew"
        Me.btnAddNew.Size = New System.Drawing.Size(150, 29)
        Me.btnAddNew.TabIndex = 5
        Me.btnAddNew.Text = "إضـــــــــافة"
        Me.btnAddNew.UseVisualStyleBackColor = True
        '
        'txtAddNew
        '
        Me.txtAddNew.Font = New System.Drawing.Font("LBC", 10.0!)
        Me.txtAddNew.Location = New System.Drawing.Point(59, 92)
        Me.txtAddNew.Name = "txtAddNew"
        Me.txtAddNew.Size = New System.Drawing.Size(326, 25)
        Me.txtAddNew.TabIndex = 4
        '
        'NewSubjForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(444, 214)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnAddNew)
        Me.Controls.Add(Me.txtAddNew)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewSubjForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إضافة موضوع جديد للقائمة"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnAddNew As Button
    Public WithEvents txtAddNew As TextBox
End Class
