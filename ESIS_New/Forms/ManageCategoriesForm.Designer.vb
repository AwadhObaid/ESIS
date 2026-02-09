<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ManageCategoriesForm
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
        Me.lstCategories = New System.Windows.Forms.ListBox()
        Me.txtNewCategory = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnAddCategory = New System.Windows.Forms.Button()
        Me.btnSaveCategories = New System.Windows.Forms.Button()
        Me.btnDeleteCategory = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnEditCategory = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSearchCategory = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstCategories
        '
        Me.lstCategories.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstCategories.FormattingEnabled = True
        Me.lstCategories.ItemHeight = 24
        Me.lstCategories.Location = New System.Drawing.Point(3, 28)
        Me.lstCategories.Name = "lstCategories"
        Me.lstCategories.ScrollAlwaysVisible = True
        Me.lstCategories.Size = New System.Drawing.Size(312, 351)
        Me.lstCategories.TabIndex = 0
        '
        'txtNewCategory
        '
        Me.txtNewCategory.Location = New System.Drawing.Point(20, 134)
        Me.txtNewCategory.Name = "txtNewCategory"
        Me.txtNewCategory.Size = New System.Drawing.Size(493, 32)
        Me.txtNewCategory.TabIndex = 1
        Me.txtNewCategory.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(172, 24)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "إضافة او تعديل التصنيف :"
        '
        'btnAddCategory
        '
        Me.btnAddCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(93, Byte), Integer), CType(CType(191, Byte), Integer), CType(CType(158, Byte), Integer))
        Me.btnAddCategory.FlatAppearance.BorderSize = 0
        Me.btnAddCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.btnAddCategory.Location = New System.Drawing.Point(393, 19)
        Me.btnAddCategory.Name = "btnAddCategory"
        Me.btnAddCategory.Size = New System.Drawing.Size(115, 31)
        Me.btnAddCategory.TabIndex = 3
        Me.btnAddCategory.Text = "إضافة تصنيف"
        Me.btnAddCategory.UseVisualStyleBackColor = False
        '
        'btnSaveCategories
        '
        Me.btnSaveCategories.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(201, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.btnSaveCategories.FlatAppearance.BorderSize = 0
        Me.btnSaveCategories.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSaveCategories.ForeColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(67, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.btnSaveCategories.Location = New System.Drawing.Point(266, 19)
        Me.btnSaveCategories.Name = "btnSaveCategories"
        Me.btnSaveCategories.Size = New System.Drawing.Size(115, 31)
        Me.btnSaveCategories.TabIndex = 4
        Me.btnSaveCategories.Text = "حفظ التغييرات"
        Me.btnSaveCategories.UseVisualStyleBackColor = False
        '
        'btnDeleteCategory
        '
        Me.btnDeleteCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(67, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.btnDeleteCategory.FlatAppearance.BorderSize = 0
        Me.btnDeleteCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDeleteCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.btnDeleteCategory.Location = New System.Drawing.Point(12, 19)
        Me.btnDeleteCategory.Name = "btnDeleteCategory"
        Me.btnDeleteCategory.Size = New System.Drawing.Size(115, 31)
        Me.btnDeleteCategory.TabIndex = 5
        Me.btnDeleteCategory.Text = "حذف تصنيف"
        Me.btnDeleteCategory.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(839, 37)
        Me.Panel1.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(366, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 24)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "إدارة التصنيفات"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lstCategories)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Right
        Me.GroupBox1.Location = New System.Drawing.Point(521, 37)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(318, 382)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "قائمة التصنيفات والمجلدات"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnEditCategory)
        Me.Panel2.Controls.Add(Me.btnSaveCategories)
        Me.Panel2.Controls.Add(Me.btnAddCategory)
        Me.Panel2.Controls.Add(Me.btnDeleteCategory)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 351)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(521, 68)
        Me.Panel2.TabIndex = 8
        '
        'btnEditCategory
        '
        Me.btnEditCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(101, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEditCategory.FlatAppearance.BorderSize = 0
        Me.btnEditCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.btnEditCategory.Location = New System.Drawing.Point(139, 19)
        Me.btnEditCategory.Name = "btnEditCategory"
        Me.btnEditCategory.Size = New System.Drawing.Size(115, 31)
        Me.btnEditCategory.TabIndex = 6
        Me.btnEditCategory.Text = "تعديل تصنيف"
        Me.btnEditCategory.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(150, 24)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "🔎 بحث عن التصنيف :"
        '
        'txtSearchCategory
        '
        Me.txtSearchCategory.Location = New System.Drawing.Point(20, 67)
        Me.txtSearchCategory.Name = "txtSearchCategory"
        Me.txtSearchCategory.Size = New System.Drawing.Size(235, 32)
        Me.txtSearchCategory.TabIndex = 9
        Me.txtSearchCategory.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label4.Location = New System.Drawing.Point(66, 224)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(395, 74)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "عند إضافة تصنيف جديد " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "أولا قم بكتابة التصنيف الجديد ثم إضغط علة زر إضافة تصنيف " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ثانيا قم بالضغط على زر حفظ التغييرات"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ManageCategoriesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(839, 419)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtSearchCategory)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNewCategory)
        Me.Font = New System.Drawing.Font("Cairo Medium", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.Name = "ManageCategoriesForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إدارة التصنيفات"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstCategories As ListBox
    Friend WithEvents txtNewCategory As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnAddCategory As Button
    Friend WithEvents btnSaveCategories As Button
    Friend WithEvents btnDeleteCategory As Button
    Friend WithEvents btnEditCategory As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents txtSearchCategory As TextBox
    Friend WithEvents Label4 As Label
End Class
