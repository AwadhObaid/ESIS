Imports ESIS_New.ESIS_New

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UsersForm
    Inherits BaseFormWithPermissions

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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PanelMain = New System.Windows.Forms.Panel()
        Me.PanelButtons = New System.Windows.Forms.Panel()
        Me.btnResetPassword = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnChangePassword = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.PanelUserDetails = New System.Windows.Forms.Panel()
        Me.dgvUsers = New System.Windows.Forms.DataGridView()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.chkIsActive = New System.Windows.Forms.CheckBox()
        Me.chkIsAdmin = New System.Windows.Forms.CheckBox()
        Me.txtConfirmPassword = New System.Windows.Forms.TextBox()
        Me.lblConfirmPassword = New System.Windows.Forms.Label()
        Me.txtNewPassword = New System.Windows.Forms.TextBox()
        Me.lblNewPassword = New System.Windows.Forms.Label()
        Me.txtCurrentPassword = New System.Windows.Forms.TextBox()
        Me.lblCurrentPassword = New System.Windows.Forms.Label()
        Me.btnBrowseImage = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.txtNumUser = New System.Windows.Forms.TextBox()
        Me.lblNumUser = New System.Windows.Forms.Label()
        Me.PanelFormTitle = New System.Windows.Forms.Panel()
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.PanelMain.SuspendLayout()
        Me.PanelButtons.SuspendLayout()
        Me.PanelUserDetails.SuspendLayout()
        CType(Me.dgvUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelFormTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelMain
        '
        Me.PanelMain.Controls.Add(Me.PanelButtons)
        Me.PanelMain.Controls.Add(Me.PanelUserDetails)
        Me.PanelMain.Controls.Add(Me.PanelFormTitle)
        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelMain.Location = New System.Drawing.Point(0, 0)
        Me.PanelMain.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelMain.Name = "PanelMain"
        Me.PanelMain.Padding = New System.Windows.Forms.Padding(10, 14, 10, 14)
        Me.PanelMain.Size = New System.Drawing.Size(1042, 503)
        Me.PanelMain.TabIndex = 0
        '
        'PanelButtons
        '
        Me.PanelButtons.Controls.Add(Me.btnResetPassword)
        Me.PanelButtons.Controls.Add(Me.btnNew)
        Me.PanelButtons.Controls.Add(Me.btnChangePassword)
        Me.PanelButtons.Controls.Add(Me.btnCancel)
        Me.PanelButtons.Controls.Add(Me.btnUpdate)
        Me.PanelButtons.Controls.Add(Me.btnAdd)
        Me.PanelButtons.Controls.Add(Me.btnDelete)
        Me.PanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelButtons.Location = New System.Drawing.Point(10, 425)
        Me.PanelButtons.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelButtons.Name = "PanelButtons"
        Me.PanelButtons.Size = New System.Drawing.Size(1022, 64)
        Me.PanelButtons.TabIndex = 4
        '
        'btnResetPassword
        '
        Me.btnResetPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnResetPassword.Enabled = False
        Me.btnResetPassword.FlatAppearance.BorderSize = 0
        Me.btnResetPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnResetPassword.Location = New System.Drawing.Point(339, 16)
        Me.btnResetPassword.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnResetPassword.Name = "btnResetPassword"
        Me.btnResetPassword.Size = New System.Drawing.Size(173, 32)
        Me.btnResetPassword.TabIndex = 6
        Me.btnResetPassword.Text = "إعادة تعيين كلمة المرور"
        Me.btnResetPassword.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNew.FlatAppearance.BorderSize = 0
        Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNew.Location = New System.Drawing.Point(904, 16)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 32)
        Me.btnNew.TabIndex = 5
        Me.btnNew.Text = "جديد"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnChangePassword
        '
        Me.btnChangePassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnChangePassword.Enabled = False
        Me.btnChangePassword.FlatAppearance.BorderSize = 0
        Me.btnChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnChangePassword.Location = New System.Drawing.Point(208, 16)
        Me.btnChangePassword.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnChangePassword.Name = "btnChangePassword"
        Me.btnChangePassword.Size = New System.Drawing.Size(135, 32)
        Me.btnChangePassword.TabIndex = 4
        Me.btnChangePassword.Text = "تغيير كلمة المرور"
        Me.btnChangePassword.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderSize = 0
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Location = New System.Drawing.Point(108, 16)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 32)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "إلغاء"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdate.Enabled = False
        Me.btnUpdate.FlatAppearance.BorderSize = 0
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Location = New System.Drawing.Point(646, 16)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(100, 32)
        Me.btnUpdate.TabIndex = 2
        Me.btnUpdate.Text = "تحديث"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.FlatAppearance.BorderSize = 0
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Location = New System.Drawing.Point(775, 16)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(100, 32)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "إضافة"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Enabled = False
        Me.btnDelete.FlatAppearance.BorderSize = 0
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Location = New System.Drawing.Point(517, 16)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 32)
        Me.btnDelete.TabIndex = 0
        Me.btnDelete.Text = "حذف"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'PanelUserDetails
        '
        Me.PanelUserDetails.Controls.Add(Me.dgvUsers)
        Me.PanelUserDetails.Controls.Add(Me.CheckBox1)
        Me.PanelUserDetails.Controls.Add(Me.chkIsActive)
        Me.PanelUserDetails.Controls.Add(Me.chkIsAdmin)
        Me.PanelUserDetails.Controls.Add(Me.txtConfirmPassword)
        Me.PanelUserDetails.Controls.Add(Me.lblConfirmPassword)
        Me.PanelUserDetails.Controls.Add(Me.txtNewPassword)
        Me.PanelUserDetails.Controls.Add(Me.lblNewPassword)
        Me.PanelUserDetails.Controls.Add(Me.txtCurrentPassword)
        Me.PanelUserDetails.Controls.Add(Me.lblCurrentPassword)
        Me.PanelUserDetails.Controls.Add(Me.btnBrowseImage)
        Me.PanelUserDetails.Controls.Add(Me.PictureBox1)
        Me.PanelUserDetails.Controls.Add(Me.txtPassword)
        Me.PanelUserDetails.Controls.Add(Me.lblPassword)
        Me.PanelUserDetails.Controls.Add(Me.txtUserName)
        Me.PanelUserDetails.Controls.Add(Me.lblUserName)
        Me.PanelUserDetails.Controls.Add(Me.txtNumUser)
        Me.PanelUserDetails.Controls.Add(Me.lblNumUser)
        Me.PanelUserDetails.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelUserDetails.Location = New System.Drawing.Point(10, 66)
        Me.PanelUserDetails.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelUserDetails.Name = "PanelUserDetails"
        Me.PanelUserDetails.Size = New System.Drawing.Size(1022, 348)
        Me.PanelUserDetails.TabIndex = 2
        '
        'dgvUsers
        '
        Me.dgvUsers.AllowUserToAddRows = False
        Me.dgvUsers.AllowUserToDeleteRows = False
        Me.dgvUsers.AllowUserToOrderColumns = True
        Me.dgvUsers.AllowUserToResizeRows = False
        Me.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvUsers.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Cairo SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvUsers.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Cairo SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvUsers.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvUsers.Dock = System.Windows.Forms.DockStyle.Right
        Me.dgvUsers.EnableHeadersVisualStyles = False
        Me.dgvUsers.GridColor = System.Drawing.Color.Gainsboro
        Me.dgvUsers.Location = New System.Drawing.Point(671, 0)
        Me.dgvUsers.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dgvUsers.MultiSelect = False
        Me.dgvUsers.Name = "dgvUsers"
        Me.dgvUsers.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Cairo SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvUsers.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvUsers.RowHeadersVisible = False
        Me.dgvUsers.RowTemplate.Height = 25
        Me.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvUsers.Size = New System.Drawing.Size(351, 348)
        Me.dgvUsers.TabIndex = 0
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(221, 271)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(194, 28)
        Me.CheckBox1.TabIndex = 15
        Me.CheckBox1.Text = "تغيير / إعادة تعيين كلمة المرور"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'chkIsActive
        '
        Me.chkIsActive.AutoSize = True
        Me.chkIsActive.Location = New System.Drawing.Point(431, 271)
        Me.chkIsActive.Name = "chkIsActive"
        Me.chkIsActive.Size = New System.Drawing.Size(95, 28)
        Me.chkIsActive.TabIndex = 14
        Me.chkIsActive.Text = "تعيين كنشط"
        Me.chkIsActive.UseVisualStyleBackColor = True
        '
        'chkIsAdmin
        '
        Me.chkIsAdmin.AutoSize = True
        Me.chkIsAdmin.Location = New System.Drawing.Point(542, 271)
        Me.chkIsAdmin.Name = "chkIsAdmin"
        Me.chkIsAdmin.Size = New System.Drawing.Size(93, 28)
        Me.chkIsAdmin.TabIndex = 13
        Me.chkIsAdmin.Text = "تعيين كمدير"
        Me.chkIsAdmin.UseVisualStyleBackColor = True
        '
        'txtConfirmPassword
        '
        Me.txtConfirmPassword.Location = New System.Drawing.Point(221, 219)
        Me.txtConfirmPassword.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtConfirmPassword.Name = "txtConfirmPassword"
        Me.txtConfirmPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtConfirmPassword.Size = New System.Drawing.Size(300, 32)
        Me.txtConfirmPassword.TabIndex = 11
        '
        'lblConfirmPassword
        '
        Me.lblConfirmPassword.AutoSize = True
        Me.lblConfirmPassword.Location = New System.Drawing.Point(541, 224)
        Me.lblConfirmPassword.Name = "lblConfirmPassword"
        Me.lblConfirmPassword.Size = New System.Drawing.Size(107, 24)
        Me.lblConfirmPassword.TabIndex = 10
        Me.lblConfirmPassword.Text = "تأكيد كلمة المرور:"
        '
        'txtNewPassword
        '
        Me.txtNewPassword.Location = New System.Drawing.Point(221, 178)
        Me.txtNewPassword.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNewPassword.Name = "txtNewPassword"
        Me.txtNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewPassword.Size = New System.Drawing.Size(300, 32)
        Me.txtNewPassword.TabIndex = 9
        '
        'lblNewPassword
        '
        Me.lblNewPassword.AutoSize = True
        Me.lblNewPassword.Location = New System.Drawing.Point(526, 182)
        Me.lblNewPassword.Name = "lblNewPassword"
        Me.lblNewPassword.Size = New System.Drawing.Size(122, 24)
        Me.lblNewPassword.TabIndex = 8
        Me.lblNewPassword.Text = "كلمة المرور الجديدة:"
        '
        'txtCurrentPassword
        '
        Me.txtCurrentPassword.Location = New System.Drawing.Point(221, 137)
        Me.txtCurrentPassword.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCurrentPassword.Name = "txtCurrentPassword"
        Me.txtCurrentPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtCurrentPassword.Size = New System.Drawing.Size(300, 32)
        Me.txtCurrentPassword.TabIndex = 7
        '
        'lblCurrentPassword
        '
        Me.lblCurrentPassword.AutoSize = True
        Me.lblCurrentPassword.Location = New System.Drawing.Point(532, 141)
        Me.lblCurrentPassword.Name = "lblCurrentPassword"
        Me.lblCurrentPassword.Size = New System.Drawing.Size(116, 24)
        Me.lblCurrentPassword.TabIndex = 6
        Me.lblCurrentPassword.Text = "كلمة المرور الحالية:"
        '
        'btnBrowseImage
        '
        Me.btnBrowseImage.Location = New System.Drawing.Point(20, 232)
        Me.btnBrowseImage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnBrowseImage.Name = "btnBrowseImage"
        Me.btnBrowseImage.Size = New System.Drawing.Size(189, 33)
        Me.btnBrowseImage.TabIndex = 12
        Me.btnBrowseImage.Text = "اختر صورة"
        Me.btnBrowseImage.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Image = Global.ElectronicSystemforIntegratedServices.My.Resources.Resources.UserDefault
        Me.PictureBox1.Location = New System.Drawing.Point(20, 14)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(189, 213)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        Me.PictureBox1.Tag = ""
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(221, 96)
        Me.txtPassword.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(300, 32)
        Me.txtPassword.TabIndex = 2
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(571, 99)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(77, 24)
        Me.lblPassword.TabIndex = 4
        Me.lblPassword.Text = "كلمة المرور:"
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(221, 55)
        Me.txtUserName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(300, 32)
        Me.txtUserName.TabIndex = 1
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Location = New System.Drawing.Point(553, 58)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(95, 24)
        Me.lblUserName.TabIndex = 2
        Me.lblUserName.Text = "اسم المستخدم:"
        '
        'txtNumUser
        '
        Me.txtNumUser.Location = New System.Drawing.Point(221, 14)
        Me.txtNumUser.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNumUser.Name = "txtNumUser"
        Me.txtNumUser.Size = New System.Drawing.Size(300, 32)
        Me.txtNumUser.TabIndex = 0
        '
        'lblNumUser
        '
        Me.lblNumUser.AutoSize = True
        Me.lblNumUser.Location = New System.Drawing.Point(556, 17)
        Me.lblNumUser.Name = "lblNumUser"
        Me.lblNumUser.Size = New System.Drawing.Size(92, 24)
        Me.lblNumUser.TabIndex = 0
        Me.lblNumUser.Text = "رقم المستخدم:"
        '
        'PanelFormTitle
        '
        Me.PanelFormTitle.Controls.Add(Me.lblFormTitle)
        Me.PanelFormTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelFormTitle.Location = New System.Drawing.Point(10, 14)
        Me.PanelFormTitle.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelFormTitle.Name = "PanelFormTitle"
        Me.PanelFormTitle.Size = New System.Drawing.Size(1022, 52)
        Me.PanelFormTitle.TabIndex = 3
        '
        'lblFormTitle
        '
        Me.lblFormTitle.AutoSize = True
        Me.lblFormTitle.Font = New System.Drawing.Font("Cairo Black", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFormTitle.Location = New System.Drawing.Point(375, 8)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(177, 36)
        Me.lblFormTitle.TabIndex = 0
        Me.lblFormTitle.Text = "إدارة المستخدمين"
        '
        'UsersForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1042, 503)
        Me.Controls.Add(Me.PanelMain)
        Me.Font = New System.Drawing.Font("Cairo SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UsersForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "إدارة المستخدمين"
        Me.PanelMain.ResumeLayout(False)
        Me.PanelButtons.ResumeLayout(False)
        Me.PanelUserDetails.ResumeLayout(False)
        Me.PanelUserDetails.PerformLayout()
        CType(Me.dgvUsers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelFormTitle.ResumeLayout(False)
        Me.PanelFormTitle.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelMain As Panel
    Friend WithEvents dgvUsers As DataGridView
    Friend WithEvents PanelUserDetails As Panel
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents txtUserName As TextBox
    Friend WithEvents lblUserName As Label
    Friend WithEvents txtNumUser As TextBox
    Friend WithEvents lblNumUser As Label
    Friend WithEvents PanelFormTitle As Panel
    Friend WithEvents lblFormTitle As Label
    Friend WithEvents PanelButtons As Panel
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnBrowseImage As Button
    Friend WithEvents txtCurrentPassword As TextBox
    Friend WithEvents lblCurrentPassword As Label
    Friend WithEvents txtNewPassword As TextBox
    Friend WithEvents lblNewPassword As Label
    Friend WithEvents txtConfirmPassword As TextBox
    Friend WithEvents lblConfirmPassword As Label
    Friend WithEvents btnChangePassword As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents btnResetPassword As Button
    Friend WithEvents chkIsActive As CheckBox
    Friend WithEvents chkIsAdmin As CheckBox
    Friend WithEvents CheckBox1 As CheckBox
End Class