<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InstallWizardForm
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
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.ddd = New System.Windows.Forms.ToolStripLabel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btn_Next = New System.Windows.Forms.Button()
        Me.btnFinish = New System.Windows.Forms.Button()
        Me.btn_Prev = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbEndDevice = New System.Windows.Forms.RadioButton()
        Me.rbServer = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Rb_Permission = New System.Windows.Forms.RadioButton()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnSkip = New System.Windows.Forms.Button()
        Me.txtDataBaseName = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.CKShow1 = New System.Windows.Forms.CheckBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.chkWindowsAuthentication = New System.Windows.Forms.CheckBox()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.CombServers = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ToolStrip2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(648, 44)
        Me.Panel1.TabIndex = 434
        '
        'ToolStrip2
        '
        Me.ToolStrip2.AutoSize = False
        Me.ToolStrip2.BackColor = System.Drawing.Color.SteelBlue
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStrip2.Font = New System.Drawing.Font("Sakkal Majalla", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ddd})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip2.Size = New System.Drawing.Size(646, 42)
        Me.ToolStrip2.TabIndex = 3
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'ddd
        '
        Me.ddd.AutoSize = False
        Me.ddd.Font = New System.Drawing.Font("Sakkal Majalla", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ddd.ForeColor = System.Drawing.Color.White
        Me.ddd.Name = "ddd"
        Me.ddd.Size = New System.Drawing.Size(600, 32)
        Me.ddd.Text = "معالج إعداد وتهيئة برنامج الإرشيف الإلكتروني"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.btn_Next)
        Me.Panel2.Controls.Add(Me.btnFinish)
        Me.Panel2.Controls.Add(Me.btn_Prev)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 328)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(648, 39)
        Me.Panel2.TabIndex = 435
        '
        'btn_Next
        '
        Me.btn_Next.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Next.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Next.Location = New System.Drawing.Point(126, 4)
        Me.btn_Next.Name = "btn_Next"
        Me.btn_Next.Size = New System.Drawing.Size(100, 30)
        Me.btn_Next.TabIndex = 336
        Me.btn_Next.Text = "التالي"
        Me.btn_Next.UseVisualStyleBackColor = True
        '
        'btnFinish
        '
        Me.btnFinish.Enabled = False
        Me.btnFinish.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFinish.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFinish.Location = New System.Drawing.Point(20, 4)
        Me.btnFinish.Name = "btnFinish"
        Me.btnFinish.Size = New System.Drawing.Size(100, 30)
        Me.btnFinish.TabIndex = 315
        Me.btnFinish.Text = "إنهاء"
        Me.btnFinish.UseVisualStyleBackColor = True
        '
        'btn_Prev
        '
        Me.btn_Prev.Enabled = False
        Me.btn_Prev.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Prev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Prev.Location = New System.Drawing.Point(232, 4)
        Me.btn_Prev.Name = "btn_Prev"
        Me.btn_Prev.Size = New System.Drawing.Size(100, 30)
        Me.btn_Prev.TabIndex = 322
        Me.btn_Prev.Text = "السابق"
        Me.btn_Prev.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.rbEndDevice)
        Me.Panel3.Controls.Add(Me.rbServer)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.Rb_Permission)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 44)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(648, 284)
        Me.Panel3.TabIndex = 436
        '
        'rbEndDevice
        '
        Me.rbEndDevice.AutoSize = True
        Me.rbEndDevice.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbEndDevice.Location = New System.Drawing.Point(211, 211)
        Me.rbEndDevice.Name = "rbEndDevice"
        Me.rbEndDevice.Size = New System.Drawing.Size(237, 26)
        Me.rbEndDevice.TabIndex = 440
        Me.rbEndDevice.TabStop = True
        Me.rbEndDevice.Text = "إعداد النسخه للعمل كـ نقطة طرفيه"
        Me.rbEndDevice.UseVisualStyleBackColor = True
        '
        'rbServer
        '
        Me.rbServer.AutoSize = True
        Me.rbServer.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbServer.Location = New System.Drawing.Point(197, 168)
        Me.rbServer.Name = "rbServer"
        Me.rbServer.Size = New System.Drawing.Size(251, 26)
        Me.rbServer.TabIndex = 440
        Me.rbServer.TabStop = True
        Me.rbServer.Text = "إعداد النسخه للعمل كـ سيرفر رئيسي"
        Me.rbServer.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(3, 131)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(643, 28)
        Me.Label2.TabIndex = 439
        Me.Label2.Text = "الخطوه الاولى : قم بإختيار نوع الإعداد والتهئيه للنسخه"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(4, 74)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(639, 57)
        Me.Label8.TabIndex = 438
        Me.Label8.Text = "تنبيه! قبل البدء في عمليه الإعدات يجب تثبيت محرك قواعد بيانات سكوال سيرفر" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "   (SQ" &
    "L Server Management Studio 2008 R2) او احد الاصدارات الاحدث منه  "
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("PT Bold Heading", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(644, 74)
        Me.Label3.TabIndex = 437
        Me.Label3.Text = "مرحباً بك في معالج إعداد وتهيئة برنامج الإرشيف الإلكتروني" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "لإتمام عملية الإعداد ب" &
    "شكل صحيح يجب إتباع الخطوات الاتيه :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Rb_Permission
        '
        Me.Rb_Permission.AutoSize = True
        Me.Rb_Permission.Checked = True
        Me.Rb_Permission.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Rb_Permission.Location = New System.Drawing.Point(28, 313)
        Me.Rb_Permission.Name = "Rb_Permission"
        Me.Rb_Permission.Size = New System.Drawing.Size(111, 19)
        Me.Rb_Permission.TabIndex = 436
        Me.Rb_Permission.TabStop = True
        Me.Rb_Permission.Text = "Adminstrator"
        Me.Rb_Permission.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.GroupBox1)
        Me.Panel4.Controls.Add(Me.GroupBox3)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 44)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(648, 284)
        Me.Panel4.TabIndex = 440
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btnSkip)
        Me.GroupBox1.Controls.Add(Me.txtDataBaseName)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Location = New System.Drawing.Point(71, 168)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(521, 108)
        Me.GroupBox1.TabIndex = 448
        Me.GroupBox1.TabStop = False
        '
        'btnSkip
        '
        Me.btnSkip.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSkip.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSkip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSkip.Location = New System.Drawing.Point(16, 75)
        Me.btnSkip.Name = "btnSkip"
        Me.btnSkip.Size = New System.Drawing.Size(59, 30)
        Me.btnSkip.TabIndex = 315
        Me.btnSkip.Text = "تخطي"
        Me.ToolTip1.SetToolTip(Me.btnSkip, "تخطي عمليه إنشاء قاعدة البيانات")
        Me.btnSkip.UseVisualStyleBackColor = True
        '
        'txtDataBaseName
        '
        Me.txtDataBaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataBaseName.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDataBaseName.Location = New System.Drawing.Point(16, 33)
        Me.txtDataBaseName.Name = "txtDataBaseName"
        Me.txtDataBaseName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDataBaseName.Size = New System.Drawing.Size(383, 26)
        Me.txtDataBaseName.TabIndex = 174
        '
        'Label11
        '
        Me.Label11.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(389, 35)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(132, 20)
        Me.Label11.TabIndex = 173
        Me.Label11.Text = "اسم قاعدة البيانات :"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.CKShow1)
        Me.GroupBox3.Controls.Add(Me.txtPassword)
        Me.GroupBox3.Controls.Add(Me.btnTest)
        Me.GroupBox3.Controls.Add(Me.chkWindowsAuthentication)
        Me.GroupBox3.Controls.Add(Me.txtUserName)
        Me.GroupBox3.Controls.Add(Me.CombServers)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Location = New System.Drawing.Point(71, 36)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(521, 128)
        Me.GroupBox3.TabIndex = 447
        Me.GroupBox3.TabStop = False
        '
        'CKShow1
        '
        Me.CKShow1.AutoSize = True
        Me.CKShow1.BackColor = System.Drawing.Color.White
        Me.CKShow1.Font = New System.Drawing.Font("Arial Rounded MT Bold", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CKShow1.Location = New System.Drawing.Point(345, 80)
        Me.CKShow1.Name = "CKShow1"
        Me.CKShow1.Size = New System.Drawing.Size(54, 16)
        Me.CKShow1.TabIndex = 449
        Me.CKShow1.Text = "Show"
        Me.ToolTip1.SetToolTip(Me.CKShow1, "إضهار/ إخفاء " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "   الباسورد")
        Me.CKShow1.UseVisualStyleBackColor = False
        '
        'txtPassword
        '
        Me.txtPassword.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.ForeColor = System.Drawing.Color.Black
        Me.txtPassword.Location = New System.Drawing.Point(24, 77)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPassword.Size = New System.Drawing.Size(380, 23)
        Me.txtPassword.TabIndex = 345
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'btnTest
        '
        Me.btnTest.FlatAppearance.BorderSize = 0
        Me.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTest.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnTest.Location = New System.Drawing.Point(301, 102)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(103, 23)
        Me.btnTest.TabIndex = 340
        Me.btnTest.Text = "فحص الاتصال"
        Me.btnTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolTip1.SetToolTip(Me.btnTest, "فحص الاتصال بالسيرفر")
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'chkWindowsAuthentication
        '
        Me.chkWindowsAuthentication.AutoSize = True
        Me.chkWindowsAuthentication.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.chkWindowsAuthentication.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkWindowsAuthentication.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkWindowsAuthentication.ForeColor = System.Drawing.Color.Black
        Me.chkWindowsAuthentication.Location = New System.Drawing.Point(24, 106)
        Me.chkWindowsAuthentication.Name = "chkWindowsAuthentication"
        Me.chkWindowsAuthentication.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkWindowsAuthentication.Size = New System.Drawing.Size(188, 18)
        Me.chkWindowsAuthentication.TabIndex = 346
        Me.chkWindowsAuthentication.Text = "Use Windows Authentication"
        Me.chkWindowsAuthentication.UseVisualStyleBackColor = False
        '
        'txtUserName
        '
        Me.txtUserName.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserName.ForeColor = System.Drawing.Color.Black
        Me.txtUserName.Location = New System.Drawing.Point(24, 49)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUserName.Size = New System.Drawing.Size(380, 23)
        Me.txtUserName.TabIndex = 344
        '
        'CombServers
        '
        Me.CombServers.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CombServers.BackColor = System.Drawing.Color.White
        Me.CombServers.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CombServers.ForeColor = System.Drawing.Color.Black
        Me.CombServers.FormattingEnabled = True
        Me.CombServers.Location = New System.Drawing.Point(23, 19)
        Me.CombServers.Name = "CombServers"
        Me.CombServers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CombServers.Size = New System.Drawing.Size(380, 27)
        Me.CombServers.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(410, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(104, 22)
        Me.Label6.TabIndex = 173
        Me.Label6.Text = "كلمة المرور"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(410, 47)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(104, 22)
        Me.Label5.TabIndex = 173
        Me.Label5.Text = "اسم المستخدم"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(410, 18)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(104, 22)
        Me.Label7.TabIndex = 173
        Me.Label7.Text = "اسم السيرفر "
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(2, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(643, 28)
        Me.Label1.TabIndex = 446
        Me.Label1.Text = "الخطوه الثانيه : قم بانشاء قاعدة البيانات من خلال ملف الاسكربت "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.Label4)
        Me.Panel5.Controls.Add(Me.Label9)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(0, 44)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(648, 284)
        Me.Panel5.TabIndex = 441
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("PT Bold Heading", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(0, 91)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(648, 76)
        Me.Label4.TabIndex = 448
        Me.Label4.Text = "اسم المستخدم / admin" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "كلمة المـــــرور / admin"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label9.Font = New System.Drawing.Font("PT Bold Heading", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(0, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(648, 91)
        Me.Label9.TabIndex = 447
        Me.Label9.Text = "شكراً لك لقد وصلت الى الخطوه الاخيره لاعداد وتهيئة برنامج  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " الإرشيف الإلكتروني " &
    "يمكنك الدخول الى البرنامج من خلال بيانات المستخدم الافتراضيه"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'InstallWizardForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.ClientSize = New System.Drawing.Size(648, 367)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InstallWizardForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "معالج إعداد البرنامج"
        Me.Panel1.ResumeLayout(False)
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents ddd As System.Windows.Forms.ToolStripLabel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btn_Next As System.Windows.Forms.Button
    Friend WithEvents btnFinish As System.Windows.Forms.Button
    Friend WithEvents btn_Prev As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Rb_Permission As System.Windows.Forms.RadioButton
    Friend WithEvents rbEndDevice As System.Windows.Forms.RadioButton
    Friend WithEvents rbServer As System.Windows.Forms.RadioButton
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents CombServers As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnSkip As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtDataBaseName As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Private WithEvents txtPassword As System.Windows.Forms.TextBox
    Private WithEvents chkWindowsAuthentication As System.Windows.Forms.CheckBox
    Private WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents CKShow1 As System.Windows.Forms.CheckBox
End Class