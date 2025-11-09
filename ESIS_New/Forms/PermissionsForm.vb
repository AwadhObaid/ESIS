Imports System.Data.SqlClient

''' <summary>
''' نموذج لإدارة صلاحيات المستخدمين باستخدام العرض الشجري
''' </summary>
Public Class PermissionsForm
    Inherits BaseFormWithPermissions

    Private _currentUserNodeId As String = ""
    Private _originalPermissions As Dictionary(Of String, Boolean)
    Private _isDirty As Boolean = False
    Private _formsList As List(Of FormInfo)
    Private isSaving As Boolean = False ' متغير للتحكم في التنفيذ


    ' معلومات عن النماذج في النظام
    Private Class FormInfo
        Public Property FormName As String
        Public Property FormTitle As String
        Public Property Description As String
        Public Property ParentCategory As String
        Public Property Order As Integer
    End Class

    ''' <summary>
    ''' تحميل النموذج
    ''' </summary>
    Private Sub PermissionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تعيين خصائص النموذج
            Me.Text = "إدارة الصلاحيات - نظام إدارة الوثائق الإلكتروني"
            Me.lblTitle.Text = "إدارة الصلاحيات"

            ' تطبيق الصلاحيات
            ApplyPermissions()

            ' تهيئة النموذج
            SetupForm()

            ' تحميل البيانات
            LoadUserData()

            ' تفعيل مؤقت التحديث التلقائي
            TimerRefresh.Start()

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل نموذج الصلاحيات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '''' <summary>
    '''' تطبيق الصلاحيات على النموذج
    '''' </summary>
    'Protected Overrides Sub ApplyPermissions()
    '    Try
    '        ' التحقق من صلاحية إدارة الصلاحيات
    '        If Not CheckPermission("ManagePermissions", "إدارة الصلاحيات") Then
    '            MessageBox.Show("ليس لديك صلاحية إدارة الصلاحيات. يرجى الاتصال بمدير النظام.",
    '                          "صلاحيات محدودة",
    '                          MessageBoxButtons.OK,
    '                          MessageBoxIcon.Information)
    '            Me.Close()
    '            Return
    '        End If

    '        ' تطبيق الصلاحيات الأخرى إذا لزم
    '        MyBase.ApplyPermissions()

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' تهيئة النموذج
    ''' </summary>
    Private Sub SetupForm()
        Try
            ' تهيئة الأشجار
            SetupTreeViews()

            ' تهيئة الأزرار
            SetupButtons()

            ' ربط أحداث النقر بزر الماوس الأيمن
            AddHandler TreeViewUsers.MouseClick, AddressOf TreeViewUsers_MouseClick
            AddHandler TreeViewPermissions.MouseClick, AddressOf TreeViewPermissions_MouseClick

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة النموذج: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تهيئة أشجار العرض
    ''' </summary>
    Private Sub SetupTreeViews()
        Try
            ' تنظيف الأشجار
            TreeViewUsers.Nodes.Clear()
            TreeViewPermissions.Nodes.Clear()

            ' إعداد قائمة النماذج
            InitializeFormsList()

            ' بناء هيكل النماذج
            BuildFormsTree()

            ' تمكين السحب والإفلات
            TreeViewUsers.AllowDrop = True
            TreeViewPermissions.AllowDrop = True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة أشجار العرض: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تهيئة الأزرار
    ''' </summary>
    Private Sub SetupButtons()
        Try
            ' تعطيل زر الحفظ في البداية
            btnSave.Enabled = False
            btnCancel.Enabled = False

            ' ربط الأحداث
            AddHandler btnSave.Click, AddressOf btnSave_Click
            AddHandler btnRefresh.Click, AddressOf btnRefresh_Click
            AddHandler btnCancel.Click, AddressOf btnCancel_Click

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة الأزرار: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تهيئة قائمة النماذج
    ''' </summary>
    Private Sub InitializeFormsList()
        Try
            _formsList = New List(Of FormInfo) From {
                New FormInfo With {.FormName = "ArchiveForm", .FormTitle = "الأرشيف", .Description = "إدارة السجلات المؤرشفة", .ParentCategory = "التسجيل", .Order = 1},
                New FormInfo With {.FormName = "SaderForm", .FormTitle = "الصادر", .Description = "إدارة الكتب الصادرة", .ParentCategory = "التسجيل", .Order = 2},
                New FormInfo With {.FormName = "WaredForm", .FormTitle = "الوارد", .Description = "إدارة الكتب الواردة", .ParentCategory = "التسجيل", .Order = 3},
                New FormInfo With {.FormName = "UsersForm", .FormTitle = "المستخدمين", .Description = "إدارة حسابات المستخدمين", .ParentCategory = "النظام", .Order = 1},
                New FormInfo With {.FormName = "PermissionsForm", .FormTitle = "الصلاحيات", .Description = "إدارة صلاحيات المستخدمين", .ParentCategory = "النظام", .Order = 2},
                New FormInfo With {.FormName = "BackupForm", .FormTitle = "النسخ الاحتياطي", .Description = "إنشاء نسخ احتياطية", .ParentCategory = "النظام", .Order = 3},
                New FormInfo With {.FormName = "DashboardForm", .FormTitle = "لوحة التحكم", .Description = "عرض الإحصائيات العامة", .ParentCategory = "التقارير", .Order = 1},
                New FormInfo With {.FormName = "UserActivityLogForm", .FormTitle = "سجلات الحركة", .Description = "تتبع أنشطة المستخدمين", .ParentCategory = "التقارير", .Order = 2}
            }

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة قائمة النماذج: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' بناء هيكل شجرة النماذج
    ''' </summary>
    Private Sub BuildFormsTree()
        Try
            ' إنشاء فئات رئيسية
            Dim categories As New Dictionary(Of String, TreeNode)()
            Dim categoryNames As String() = {"التسجيل", "النظام", "التقارير"}

            For Each categoryName In categoryNames
                Dim node As New TreeNode(categoryName)
                node.ImageIndex = 2 ' مجلد
                node.SelectedImageIndex = 2
                node.Tag = "CATEGORY"
                categories(categoryName) = node
            Next

            ' إضافة النماذج إلى الفئات
            For Each formInfo In _formsList.OrderBy(Function(f) f.ParentCategory & f.Order)
                If categories.ContainsKey(formInfo.ParentCategory) Then
                    Dim formNode As New TreeNode(formInfo.FormTitle)
                    formNode.ImageIndex = 3 ' نموذج
                    formNode.SelectedImageIndex = 3
                    formNode.Tag = formInfo.FormName
                    formNode.ToolTipText = $"{formInfo.Description}"

                    ' إضافة أنواع الصلاحيات كعقد فرعية
                    AddPermissionTypesToNode(formNode)

                    categories(formInfo.ParentCategory).Nodes.Add(formNode)
                End If
            Next

            ' إضافة الفئات إلى الشجرة
            TreeViewPermissions.Nodes.Clear()
            For Each categoryName In categoryNames
                If categories(categoryName).Nodes.Count > 0 Then
                    TreeViewPermissions.Nodes.Add(categories(categoryName))
                End If
            Next

            ' توسيع جميع العقد
            TreeViewPermissions.ExpandAll()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في بناء هيكل شجرة النماذج: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' إضافة أنواع الصلاحيات إلى عقدة النموذج
    ''' </summary>
    Private Sub AddPermissionTypesToNode(node As TreeNode)
        Try
            Dim permissionTypes As String() = {"View", "Add", "Edit", "Delete", "Search", "Print", "Archive"}
            Dim friendlyNames As String() = {"عرض", "إضافة", "تعديل", "حذف", "بحث", "طباعة", "أرشفة"}

            For i As Integer = 0 To permissionTypes.Length - 1
                Dim permissionNode As New TreeNode(friendlyNames(i))
                permissionNode.ImageIndex = 3
                permissionNode.SelectedImageIndex = 3
                permissionNode.Tag = $"PERMISSION_{permissionTypes(i)}"
                permissionNode.Checked = False
                node.Nodes.Add(permissionNode)
            Next

            ' ترتيب العقد الفرعية
            node.Expand()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إضافة أنواع الصلاحيات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحميل بيانات المستخدمين
    ''' </summary>
    Private Async Sub LoadUserData()
        Try
            SetLoadingState(True)

            ' تحميل بيانات المستخدمين بشكل غير متزامن
            Dim usersData As DataTable = Await Task.Run(AddressOf LoadUsersFromDatabase)

            ' بناء شجرة المستخدمين
            BuildUsersTree(usersData)

            ' تحديد أول مستخدم
            If TreeViewUsers.Nodes.Count > 0 AndAlso TreeViewUsers.Nodes(0).Nodes.Count > 0 Then
                TreeViewUsers.Nodes(0).Nodes(0).EnsureVisible()
                TreeViewUsers.SelectedNode = TreeViewUsers.Nodes(0).Nodes(0)
                LoadUserPermissions(TreeViewUsers.Nodes(0).Nodes(0))
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل بيانات المستخدمين: {ex.Message}")
            MessageBox.Show($"فشل في تحميل بيانات المستخدمين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetLoadingState(False)
        End Try
    End Sub

    ''' <summary>
    ''' تحميل بيانات المستخدمين من قاعدة البيانات
    ''' </summary>
    Private Function LoadUsersFromDatabase() As DataTable
        Try
            Dim query As String = "
            SELECT ID, UserName, NumUser, IsAdmin, IsActive 
            FROM TbUsers 
            ORDER BY IsAdmin DESC, UserName"

            Return DatabaseHelper.ExecuteQuery(query)

        Catch ex As Exception
            Throw New Exception("خطأ في جلب بيانات المستخدمين: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' بناء شجرة المستخدمين
    ''' </summary>
    Private Sub BuildUsersTree(usersData As DataTable)
        Try
            TreeViewUsers.BeginUpdate()
            TreeViewUsers.Nodes.Clear()

            ' إنشاء فئتين: المديرين والمستخدمين العاديين
            Dim adminNode As New TreeNode("المديرون")
            adminNode.ImageIndex = 1
            adminNode.SelectedImageIndex = 1
            adminNode.Tag = "ADMIN_CATEGORY"

            Dim userNode As New TreeNode("المستخدمون")
            userNode.ImageIndex = 0
            userNode.SelectedImageIndex = 0
            userNode.Tag = "USER_CATEGORY"

            ' إضافة المستخدمين إلى الفئات المناسبة
            For Each row As DataRow In usersData.Rows
                Dim userName As String = row("UserName").ToString()
                Dim userId As Integer = Convert.ToInt32(row("ID"))
                Dim isAdmin As Boolean = Convert.ToBoolean(row("IsAdmin"))
                Dim isActive As Boolean = Convert.ToBoolean(row("IsActive"))

                Dim userStatus As String = If(isActive, "", " (معطل)")

                Dim userTreeNode As New TreeNode($"{userName}{userStatus}")
                userTreeNode.ImageIndex = If(isAdmin, 1, 0)
                userTreeNode.SelectedImageIndex = If(isAdmin, 1, 0)
                userTreeNode.Tag = $"USER_{userId}"

                If isAdmin Then
                    adminNode.Nodes.Add(userTreeNode)
                Else
                    userNode.Nodes.Add(userTreeNode)
                End If
            Next

            ' إضافة الفئات إلى الشجرة
            If adminNode.Nodes.Count > 0 Then
                TreeViewUsers.Nodes.Add(adminNode)
            End If

            If userNode.Nodes.Count > 0 Then
                TreeViewUsers.Nodes.Add(userNode)
            End If

            TreeViewUsers.EndUpdate()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في بناء شجرة المستخدمين: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تعيين حالة التحميل
    ''' </summary>
    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            btnRefresh.Enabled = Not isLoading
            btnSave.Enabled = False
            btnCancel.Enabled = False

            If isLoading Then
                Me.Cursor = Cursors.WaitCursor
            Else
                Me.Cursor = Cursors.Default
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين حالة التحميل: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة اختيار المستخدم
    ''' </summary>
    Private Sub TreeViewUsers_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeViewUsers.AfterSelect
        Try
            ' منع المعالجة المتكررة
            If e.Action = TreeViewAction.Unknown Then
                Return
            End If

            ' التعامل مع التحقق من عقدة النموذج (العقدة الأم)
            If e.Node.Nodes.Count > 0 AndAlso Not String.IsNullOrEmpty(e.Node.Tag?.ToString()) AndAlso Not e.Node.Tag.ToString().StartsWith("PERMISSION_") Then
                ' هذه عقدة نموذج (الأصل)
                HandleFormNodeCheck(e.Node, e.Node.Checked)
            Else
                ' هذه عقدة إذن فرعي (View, Add, إلخ)
                HandlePermissionNodeCheck(e.Node)
            End If

            ' تحديث حالة النموذج
            UpdateFormState()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تغيير الحالة: {ex.Message}")
        End Try


        'Try
        '    If e.Node Is Nothing OrElse e.Node.Tag Is Nothing Then
        '        Return
        '    End If

        '    Dim tag As String = e.Node.Tag.ToString()

        '    If tag.StartsWith("USER_") Then
        '        LoadUserPermissions(e.Node)
        '    ElseIf tag.EndsWith("_CATEGORY") Then
        '        ' لا شيء، هذه عقدة فئة
        '    End If

        'Catch ex As Exception
        '    System.Diagnostics.Debug.WriteLine($"خطأ في اختيار المستخدم: {ex.Message}")
        'End Try
    End Sub

    ''' <summary>
    ''' معالجة التحقق من عقدة النموذج (العقدة الأم)
    ''' </summary>
    Private Sub HandleFormNodeCheck(formNode As TreeNode, isChecked As Boolean)
        Try
            ' تطبيق الحالة على جميع العقد الفرعية
            For Each permissionNode As TreeNode In formNode.Nodes
                ' تجنب التسبب في حلقة لا نهائية
                RemoveHandler TreeViewPermissions.AfterCheck, AddressOf TreeViewPermissions_AfterCheck

                permissionNode.Checked = isChecked

                AddHandler TreeViewPermissions.AfterCheck, AddressOf TreeViewPermissions_AfterCheck
            Next

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة عقدة النموذج: {ex.Message}")
        End Try
    End Sub


    ''' <summary>
    ''' معالجة التحقق من عقدة الإذن الفرعي
    ''' </summary>
    Private Sub HandlePermissionNodeCheck(permissionNode As TreeNode)
        Try
            ' التحقق مما إذا كانت العقدة الفرعية جزء من عقدة نموذج
            If permissionNode.Parent IsNot Nothing Then
                Dim formNode As TreeNode = permissionNode.Parent

                ' التحقق مما إذا كانت جميع العقد الفرعية محددة
                Dim allChecked As Boolean = True
                Dim anyChecked As Boolean = False

                For Each childNode As TreeNode In formNode.Nodes
                    If childNode.Checked Then
                        anyChecked = True
                    Else
                        allChecked = False
                    End If
                Next

                ' إزالة معالج الحدث مؤقتًا لتجنب الحلقات اللانهائية
                RemoveHandler TreeViewPermissions.AfterCheck, AddressOf TreeViewPermissions_AfterCheck

                ' تحديث حالة العقدة الأم
                If allChecked Then
                    formNode.Checked = True
                    formNode.StateImageIndex = 0 ' استخدام صورة للحالة المكتملة (مثلاً علامة صح)
                ElseIf anyChecked Then
                    formNode.Checked = False
                    formNode.StateImageIndex = 1 ' استخدام صورة للحالة الجزئية (مثلاً مربع جزئي)
                Else
                    formNode.Checked = False
                    formNode.StateImageIndex = -1 ' لا صورة حالة
                End If

                AddHandler TreeViewPermissions.AfterCheck, AddressOf TreeViewPermissions_AfterCheck
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة عقدة الإذن: {ex.Message}")
        End Try
    End Sub


    ''' <summary>
    ''' تحميل صلاحيات المستخدم
    ''' </summary>
    Private Sub LoadUserPermissions(userNode As TreeNode)
        Try
            ' استخراج معرف المستخدم من العلامة
            Dim userId As Integer = Convert.ToInt32(userNode.Tag.ToString().Replace("USER_", ""))

            ' حفظ معرف المستخدم الحالي
            _currentUserNodeId = userNode.Tag.ToString()

            ' تحميل صلاحيات المستخدم
            Dim permissionsData As DataTable = GetUserPermissions(userId)

            ' تخزين الصلاحيات الأصلية للمقارنة
            _originalPermissions = New Dictionary(Of String, Boolean)()

            ' تعيين حالات مربعات الاختيار
            For Each rootNode As TreeNode In TreeViewPermissions.Nodes
                For Each formNode As TreeNode In rootNode.Nodes
                    Dim formName As String = formNode.Tag.ToString()

                    For Each permissionNode As TreeNode In formNode.Nodes
                        Dim permissionType As String = permissionNode.Tag.ToString()
                        Dim permissionKey As String = $"{formName}_{permissionType}"

                        ' البحث عن الصلاحية في البيانات
                        Dim hasPermission As Boolean = False
                        For Each row As DataRow In permissionsData.Rows
                            If row("FormName").ToString() = formName AndAlso row("PermissionType").ToString() = permissionType Then
                                hasPermission = Convert.ToBoolean(row("HasPermission"))
                                Exit For
                            End If
                        Next

                        ' تعيين الحالة
                        permissionNode.Checked = hasPermission

                        ' حفظ الحالة الأصلية
                        _originalPermissions(permissionKey) = hasPermission
                    Next
                Next
            Next

            ' تحديث حالة النموذج
            UpdateFormState()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل صلاحيات المستخدم: {ex.Message}")
            MessageBox.Show($"فشل في تحميل صلاحيات المستخدم: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' الحصول على صلاحيات المستخدم من قاعدة البيانات
    ''' </summary>
    Private Function GetUserPermissions(userId As Integer) As DataTable
        Try
            Dim query As String = "
            SELECT FormName, PermissionType, HasPermission 
            FROM TbPermissions 
            WHERE UserID = @UserID 
            ORDER BY FormName, PermissionType"

            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@UserID", userId)
            }

            Return DatabaseHelper.ExecuteQuery(query, parameters)

        Catch ex As Exception
            Throw New Exception("خطأ في جلب صلاحيات المستخدم: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' معالجة تغيير الحالة في شجرة الصلاحيات
    ''' </summary>
    Private Sub TreeViewPermissions_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles TreeViewPermissions.AfterCheck
        Try
            ' منع المعالجة المتكررة
            If e.Action = TreeViewAction.Unknown Then
                Return
            End If

            ' تحديث حالة النموذج
            UpdateFormState()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تغيير الحالة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث حالة النموذج بناءً على التغييرات
    ''' </summary>
    Private Sub UpdateFormState()
        Try
            ' التحقق مما إذا كانت هناك تغييرات
            _isDirty = HasChanges()

            ' تمكين/تعطيل الأزرار بناءً على الحالة
            btnSave.Enabled = _isDirty
            btnCancel.Enabled = _isDirty

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث حالة النموذج: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' التحقق مما إذا كانت هناك تغييرات في الصلاحيات
    ''' </summary>
    Private Function HasChanges() As Boolean
        Try
            For Each rootNode As TreeNode In TreeViewPermissions.Nodes
                For Each formNode As TreeNode In rootNode.Nodes
                    Dim formName As String = formNode.Tag.ToString()

                    For Each permissionNode As TreeNode In formNode.Nodes
                        Dim permissionType As String = permissionNode.Tag.ToString()
                        Dim permissionKey As String = $"{formName}_{permissionType}"

                        If _originalPermissions.ContainsKey(permissionKey) Then
                            If _originalPermissions(permissionKey) <> permissionNode.Checked Then
                                Return True
                            End If
                        Else
                            If permissionNode.Checked Then
                                Return True
                            End If
                        End If
                    Next
                Next
            Next

            Return False

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من التغييرات: {ex.Message}")
            Return True
        End Try
    End Function

    ''' <summary>
    ''' معالجة الضغط على زر الحفظ
    ''' </summary>
    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            ' منع التنفيذ المتكرر
            If isSaving OrElse Not _isDirty Then
                Return
            End If

            ' تعيين الحالة إلى جاري الحفظ
            isSaving = True

            ' تعطيل الأزرار
            btnSave.Enabled = False
            btnCancel.Enabled = False

            If String.IsNullOrEmpty(_currentUserNodeId) Then
                MessageBox.Show("يرجى اختيار مستخدم أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim userId As Integer = Convert.ToInt32(_currentUserNodeId.Replace("USER_", ""))

            ' عرض مؤشر الانتظار
            Me.Cursor = Cursors.WaitCursor

            Try
                ' تنفيذ عملية الحفظ
                Dim success As Boolean = Await Task.Run(Function() SaveUserPermissions(userId))

                If success Then
                    ' التأكد أننا لا نزال في نفس النموذج
                    If Not Me.IsDisposed AndAlso Me.InvokeRequired Then
                        Me.Invoke(Sub()
                                      MessageBox.Show("تم حفظ الصلاحيات بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                      _isDirty = False
                                      btnSave.Enabled = False
                                      btnCancel.Enabled = False
                                  End Sub)
                    ElseIf Not Me.IsDisposed Then
                        MessageBox.Show("تم حفظ الصلاحيات بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        _isDirty = False
                        btnSave.Enabled = False
                        btnCancel.Enabled = False
                    End If

                    RefreshUsersTree()
                Else
                    ' التأكد من أن النموذج لا يزال صالحًا
                    If Not Me.IsDisposed Then
                        MessageBox.Show("فشل في حفظ الصلاحيات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If

            Catch ex As Exception
                If Not Me.IsDisposed Then
                    MessageBox.Show($"خطأ في حفظ الصلاحيات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Try

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ غير متوقع في.btnSave_Click: {ex.Message}")
        Finally
            ' إعادة الحالة العادية
            isSaving = False
            Me.Cursor = Cursors.Default

            ' إعادة تمكين الأزرار فقط إذا كانت هناك حاجة
            If _isDirty AndAlso Not isSaving Then
                btnSave.Enabled = True
                btnCancel.Enabled = True
            End If
        End Try
    End Sub

    ''' <summary>
    ''' حفظ صلاحيات المستخدم
    ''' </summary>
    Private Function SaveUserPermissions(userId As Integer) As Boolean
        Try
            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using transaction As SqlTransaction = connection.BeginTransaction()
                    Try
                        ' حذف جميع الصلاحيات الحالية للمستخدم
                        Dim deleteQuery As String = "DELETE FROM TbPermissions WHERE UserID = @UserID"
                        Using deleteCommand As New SqlCommand(deleteQuery, connection, transaction)
                            deleteCommand.Parameters.AddWithValue("@UserID", userId)
                            deleteCommand.ExecuteNonQuery()
                        End Using

                        ' إدراج الصلاحيات الجديدة
                        Dim insertQuery As String = "INSERT INTO TbPermissions (UserID, FormName, PermissionType, HasPermission) VALUES (@UserID, @FormName, @PermissionType, @HasPermission)"

                        For Each rootNode As TreeNode In TreeViewPermissions.Nodes
                            For Each formNode As TreeNode In rootNode.Nodes
                                Dim formName As String = formNode.Tag.ToString()

                                For Each permissionNode As TreeNode In formNode.Nodes
                                    Dim permissionType As String = permissionNode.Tag.ToString()
                                    Dim hasPermission As Boolean = permissionNode.Checked

                                    Using insertCommand As New SqlCommand(insertQuery, connection, transaction)
                                        insertCommand.Parameters.AddWithValue("@UserID", userId)
                                        insertCommand.Parameters.AddWithValue("@FormName", formName)
                                        insertCommand.Parameters.AddWithValue("@PermissionType", permissionType)
                                        insertCommand.Parameters.AddWithValue("@HasPermission", hasPermission)
                                        insertCommand.ExecuteNonQuery()
                                    End Using
                                Next
                            Next
                        Next

                        ' تأكيد المعاملة
                        transaction.Commit()

                        ' تسجيل نشاط الحفظ
                        Dim activityLogClass As New ActivityLogClass()
                        activityLogClass.LogUserActivity(
                            GlobalData.LoggedInUserName,
                            "Save",
                            "TbPermissions",
                            userId,
                            $"حفظ صلاحيات المستخدم: {GetUserNameById(userId)}"
                        )

                        Return True
                    Catch ex As Exception
                        ' التراجع عن المعاملة في حالة الخطأ
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ صلاحيات المستخدم: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' الحصول على اسم المستخدم من المعرف
    ''' </summary>
    Private Function GetUserNameById(userId As Integer) As String
        Try
            Dim query As String = "SELECT UserName FROM TbUsers WHERE ID = @UserID"
            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", userId)
                    Dim result As Object = command.ExecuteScalar()
                    Return If(result IsNot Nothing, result.ToString(), "مستخدم غير معروف")
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على اسم المستخدم: {ex.Message}")
            Return "مستخدم غير معروف"
        End Try
    End Function

    ''' <summary>
    ''' معالجة الضغط على زر إلغاء
    ''' </summary>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            If _isDirty Then
                Dim result As DialogResult = MessageBox.Show("هل تريد إلغاء التغييرات؟ سيتم فقدان جميع التغييرات غير المحفوظة.", "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.No Then
                    Return
                End If
            End If

            ' إعادة تحميل صلاحيات المستخدم
            If Not String.IsNullOrEmpty(_currentUserNodeId) Then
                Dim userNode As TreeNode = FindNodeByTag(TreeViewUsers.Nodes, _currentUserNodeId)
                If userNode IsNot Nothing Then
                    LoadUserPermissions(userNode)
                End If
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في إلغاء التغييرات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر تحديث
    ''' </summary>
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            LoadUserData()

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحديث البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' العثور على عقدة بالعلامة
    ''' </summary>
    Private Function FindNodeByTag(nodes As TreeNodeCollection, tag As String) As TreeNode
        For Each node As TreeNode In nodes
            If node.Tag IsNot Nothing AndAlso node.Tag.ToString() = tag Then
                Return node
            End If

            Dim foundNode As TreeNode = FindNodeByTag(node.Nodes, tag)
            If foundNode IsNot Nothing Then
                Return foundNode
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' تحديث شجرة المستخدمين
    ''' </summary>
    Private Sub RefreshUsersTree()
        Try
            Dim selectedNode As TreeNode = TreeViewUsers.SelectedNode
            Dim userData As DataTable = LoadUsersFromDatabase()
            BuildUsersTree(userData)

            ' محاولة تحديد نفس المستخدم
            If selectedNode IsNot Nothing AndAlso selectedNode.Tag IsNot Nothing Then
                Dim userNode As TreeNode = FindNodeByTag(TreeViewUsers.Nodes, selectedNode.Tag.ToString())
                If userNode IsNot Nothing Then
                    TreeViewUsers.SelectedNode = userNode
                End If
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث شجرة المستخدمين: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة النقر بزر الماوس الأيمن على شجرة المستخدمين
    ''' </summary>
    Private Sub TreeViewUsers_MouseClick(sender As Object, e As MouseEventArgs)
        Try
            If e.Button = MouseButtons.Right Then
                Dim hitTestInfo = TreeViewUsers.HitTest(e.X, e.Y)
                If hitTestInfo.Node IsNot Nothing Then
                    TreeViewUsers.SelectedNode = hitTestInfo.Node
                    ContextMenuStripUser.Show(TreeViewUsers, e.Location)
                End If
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة النقر بزر الماوس الأيمن: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة النقر بزر الماوس الأيمن على شجرة الصلاحيات
    ''' </summary>
    Private Sub TreeViewPermissions_MouseClick(sender As Object, e As MouseEventArgs)
        Try
            If e.Button = MouseButtons.Right Then
                Dim hitTestInfo = TreeViewPermissions.HitTest(e.X, e.Y)
                If hitTestInfo.Node IsNot Nothing Then
                    TreeViewPermissions.SelectedNode = hitTestInfo.Node
                    'ContextMenuStripPermissions.Show(TreeViewPermissions, e.Location)
                End If
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة النقر بزر الماوس الأيمن: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة أمر توسيع الكل
    ''' </summary>
    Private Sub mnuExpandAll_Click(sender As Object, e As EventArgs) Handles mnuExpandAll.Click
        Try
            TreeViewUsers.ExpandAll()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في توسيع الكل: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة أمر طي الكل
    ''' </summary>
    Private Sub mnuCollapseAll_Click(sender As Object, e As EventArgs) Handles mnuCollapseAll.Click
        Try
            TreeViewUsers.CollapseAll()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في طي الكل: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة أمر تحديد الكل
    ''' </summary>
    Private Sub mnuSelectAll_Click(sender As Object, e As EventArgs) Handles mnuSelectAll.Click
        Try
            SetAllPermissions(True)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديد الكل: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة أمر إلغاء التحديد
    ''' </summary>
    Private Sub mnuDeselectAll_Click(sender As Object, e As EventArgs) Handles mnuDeselectAll.Click
        Try
            SetAllPermissions(False)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إلغاء التحديد: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تعيين جميع الصلاحيات
    ''' </summary>
    Private Sub SetAllPermissions(checked As Boolean)
        Try
            For Each rootNode As TreeNode In TreeViewPermissions.Nodes
                For Each formNode As TreeNode In rootNode.Nodes
                    For Each permissionNode As TreeNode In formNode.Nodes
                        permissionNode.Checked = checked
                    Next
                Next
            Next

            UpdateFormState()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين جميع الصلاحيات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة مؤقت التحديث التلقائي
    ''' </summary>
    Private Sub TimerRefresh_Tick(sender As Object, e As EventArgs) Handles TimerRefresh.Tick
        Try
            ' تحديث البيانات كل 5 دقائق
            Static counter As Integer = 0
            counter += 1

            If counter >= 60 Then ' كل 5 دقائق (60 * 5 ثواني)
                counter = 0
                LoadUserData()
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث البيانات التلقائي: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة إغلاق النموذج
    ''' </summary>
    Private Sub PermissionsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            ' التحقق من التغييرات غير المحفوظة
            If _isDirty Then
                Dim result As DialogResult = MessageBox.Show("لديك تغييرات غير محفوظة. هل تريد حفظها قبل الإغلاق؟", "تغييرات غير محفوظة", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Select Case result
                    Case DialogResult.Yes
                        ' محاولة الحفظ
                        btnSave.PerformClick()
                        If _isDirty Then ' إذا فشل الحفظ
                            e.Cancel = True
                        End If
                    Case DialogResult.No
                        ' المتابعة بالإغلاق
                    Case DialogResult.Cancel
                        e.Cancel = True
                End Select
            End If

            ' إيقاف المؤقت
            TimerRefresh.Stop()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إغلاق النموذج: {ex.Message}")
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub
End Class