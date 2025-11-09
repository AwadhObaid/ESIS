Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.Sql

Module DatabaseHelper

    ' القيم الافتراضية
    Public Const DEFAULT_SERVER As String = "localhost"
    Public Const DEFAULT_DATABASE As String = "ESIS"
    Public Const DEFAULT_PORT As String = "1433"

    ''' <summary>
    ''' دالة اتصال قاعدة البيانات الرئيسية
    ''' </summary>
    Public Function Con() As SqlConnection
        Dim conn As New SqlConnection

        Try
            If My.Settings.LoginMothed = 0 Then
                ' Connection Statement by Windows Authentication
                conn = New SqlConnection("Data Source ='" & My.Settings.ServerName & "';Initial Catalog ='" & My.Settings.DataBaseName & "';Integrated Security=True")
            ElseIf My.Settings.LoginMothed = 1 Then
                ' Connection Statement by SQL Serve Authentication
                conn = New SqlConnection("Data Source ='" & My.Settings.ServerName & "';Initial Catalog ='" & My.Settings.DataBaseName & "';User ID =" & My.Settings.LoginID & "; Password=" & My.Settings.LogPassword & "")
            ElseIf My.Settings.LoginMothed = 2 Then
                ' Connection Statement throgh Network
                conn = New SqlConnection("Data Source =" & My.Settings.ServerIP & "," & My.Settings.LoginPort & "; Network Library=DBMSSOCN; Initial Catalog =" & My.Settings.DataBaseName & ";user ID =" & My.Settings.LoginID & "; Password=" & My.Settings.LogPassword & "")
            ElseIf My.Settings.LoginMothed = 3 Then
                ' Connection Statement throgh WAN hosting
                conn = New SqlConnection("" & My.Settings.WANConString & "")
            Else
                ' الاتصال الافتراضي إذا كانت القيمة غير معروفة
                conn = New SqlConnection("Data Source ='" & DEFAULT_SERVER & "';Initial Catalog ='" & DEFAULT_DATABASE & "';Integrated Security=True")
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء سلسلة الاتصال: {ex.Message}")
            ' محاولة الاتصال الافتراضي
            conn = New SqlConnection("Data Source ='" & DEFAULT_SERVER & "';Initial Catalog ='" & DEFAULT_DATABASE & "';Integrated Security=True")
        End Try

        Return conn
    End Function

    ''' <summary>
    ''' الحصول على اتصال جديد بقاعدة البيانات
    ''' </summary>
    ''' <returns>SqlConnection</returns>
    Public Function GetConnection() As SqlConnection
        Return Con()
    End Function

    ''' <summary>
    ''' الحصول على سلسلة الاتصال المحفوظة من ملف التكوين
    ''' </summary>
    Public Function GetSavedConnectionString() As String
        Try
            Dim configPath As String = Path.Combine(Application.StartupPath, "Connection.config")
            If File.Exists(configPath) Then
                Return File.ReadAllText(configPath).Trim()
            End If

            ' السلسلة الافتراضية إذا لم توجد إعدادات محفوظة
            Return $"Server={DEFAULT_SERVER};Database={DEFAULT_DATABASE};Integrated Security=True;"

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على سلسلة الاتصال: {ex.Message}")
            Return $"Server={DEFAULT_SERVER};Database={DEFAULT_DATABASE};Integrated Security=True;"
        End Try
    End Function

    ''' <summary>
    ''' حفظ سلسلة الاتصال
    ''' </summary>
    Public Function SaveConnectionString(connectionString As String) As Boolean
        Try
            Dim configPath As String = Path.Combine(Application.StartupPath, "Connection.config")
            File.WriteAllText(configPath, connectionString)
            Return True
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في حفظ سلسلة الاتصال: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' بناء سلسلة اتصال بناءً على المعلمات المقدمة
    ''' </summary>
    Public Function BuildConnectionString(server As String, port As String, database As String, useWindowsAuth As Boolean, Optional username As String = "", Optional password As String = "") As String
        Try
            ' إذا كان البورت فارغًا، استخدم القيمة الافتراضية
            If String.IsNullOrEmpty(port) Then port = DEFAULT_PORT

            ' إذا كان الاتصال عبر الشبكة المحلية، قد لا يكون هناك بورت
            Dim serverWithPort As String = server
            If Not String.IsNullOrEmpty(port) AndAlso port <> DEFAULT_PORT Then
                serverWithPort = $"{server},{port}"
            End If

            ' بناء سلسلة الاتصال بناءً على نوع المصادقة
            If useWindowsAuth Then
                Return $"Server={serverWithPort};Database={database};Integrated Security=True;"
            Else
                Return $"Server={serverWithPort};Database={database};User ID={username};Password={password};"
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في بناء سلسلة الاتصال: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' استخراج اسم السيرفر من سلسلة الاتصال
    ''' </summary>
    Public Function GetServerNameFromConnectionString(connectionString As String) As String
        Try
            ' استخدام تعبير عادي لاستخراج اسم السيرفر
            Dim match As Match = Regex.Match(connectionString, "Server\s*=\s*([^;]+)", RegexOptions.IgnoreCase)
            If match.Success Then
                Dim serverPart As String = match.Groups(1).Value

                ' استخراج جزء السيرفر فقط (بدون البورت)
                Dim serverName As String = serverPart.Split(","c)(0).Trim()
                Return serverName
            End If

            Return DEFAULT_SERVER

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في استخراج اسم السيرفر: {ex.Message}")
            Return DEFAULT_SERVER
        End Try
    End Function

    ''' <summary>
    ''' استخراج رقم البورت من سلسلة الاتصال
    ''' </summary>
    Public Function GetPortFromConnectionString(connectionString As String) As String
        Try
            ' استخدام تعبير عادي لاستخراج اسم السيرفر
            Dim match As Match = Regex.Match(connectionString, "Server\s*=\s*([^;]+)", RegexOptions.IgnoreCase)
            If match.Success Then
                Dim serverPart As String = match.Groups(1).Value

                ' التحقق من وجود بورت
                If serverPart.Contains(",") Then
                    Dim port As String = serverPart.Split(","c)(1).Trim()
                    Return port
                End If
            End If

            Return DEFAULT_PORT

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في استخراج رقم البورت: {ex.Message}")
            Return DEFAULT_PORT
        End Try
    End Function

    ''' <summary>
    ''' التحقق مما إذا كانت المصادقة مدمجة (Windows Authentication)
    ''' </summary>
    Public Function IsIntegratedSecurity(connectionString As String) As Boolean
        Try
            Return connectionString.IndexOf("Integrated Security=True", StringComparison.OrdinalIgnoreCase) >= 0
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في التحقق من نوع المصادقة: {ex.Message}")
            Return True ' الافتراضي هو Windows Authentication
        End Try
    End Function

    ''' <summary>
    ''' اكتشاف السيرفرات المتاحة على الشبكة
    ''' </summary>
    Public Function DiscoverAvailableServers() As List(Of String)
        Dim servers As New List(Of String)()

        Try
            ' استخدام SqlDataSourceEnumerator لاكتشاف السيرفرات
            Dim instance As SqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance
            Dim table As DataTable = instance.GetDataSources()

            For Each row As DataRow In table.Rows
                Dim serverName As String = row("ServerName").ToString()
                Dim instanceName As String = row("InstanceName").ToString()

                ' إذا كان هناك اسم مثيل، أضفه إلى اسم السيرفر
                If Not String.IsNullOrEmpty(instanceName) AndAlso instanceName <> "MSSQLSERVER" Then
                    servers.Add($"{serverName}\{instanceName}")
                Else
                    servers.Add(serverName)
                End If
            Next

            ' إذا لم يتم العثور على أي سيرفر، أضف الافتراضي
            If servers.Count = 0 Then
                servers.Add(DEFAULT_SERVER)
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في اكتشاف السيرفرات: {ex.Message}")

            ' إضافة السيرفرات الافتراضية
            servers.Add(".")
            servers.Add("(local)")
            servers.Add("localhost")
            servers.Add("127.0.0.1")
        End Try

        Return servers
    End Function

    ''' <summary>
    ''' جلب قواعد البيانات من سيرفر معين
    ''' </summary>
    Public Function GetDatabases(server As String, Optional port As String = DEFAULT_PORT, Optional useWindowsAuth As Boolean = True, Optional username As String = "", Optional password As String = "") As List(Of String)
        Dim databases As New List(Of String)()

        Try
            ' بناء سلسلة اتصال مؤقتة بدون تحديد قاعدة بيانات
            Dim tempConnectionString As String = ""

            ' معالجة خاصة لـ localhost
            If server.Equals("localhost", StringComparison.OrdinalIgnoreCase) OrElse server.Equals("(local)", StringComparison.OrdinalIgnoreCase) OrElse server.Equals(".", StringComparison.OrdinalIgnoreCase) Then
                server = "."
            End If

            ' معالجة خاصة للاتصال عبر البورت
            If Not String.IsNullOrEmpty(port) AndAlso port <> DEFAULT_PORT Then
                ' استخدام صيغة صحيحة للاتصال عبر البورت
                tempConnectionString = $"Server={server},{port};"
            Else
                tempConnectionString = $"Server={server};"
            End If

            ' إضافة معلومات المصادقة
            If useWindowsAuth Then
                tempConnectionString &= "Integrated Security=True;"
            Else
                If String.IsNullOrWhiteSpace(username) OrElse String.IsNullOrWhiteSpace(password) Then
                    Throw New Exception("اسم المستخدم وكلمة المرور مطلوبان عند استخدام مصادقة SQL Server")
                End If
                tempConnectionString &= $"User ID={username};Password={password};"
            End If

            ' إضافة وقت الانتظار للاتصال
            tempConnectionString &= "Connect Timeout=5;"

            ' تسجيل سلسلة الاتصال (بدون كلمة المرور للخصوصية)
            Dim logConnectionString As String = tempConnectionString
            If Not useWindowsAuth Then
                logConnectionString = logConnectionString.Replace(password, "******")
            End If
            System.Diagnostics.Debug.WriteLine($"محاولة الاتصال بسلسلة: {logConnectionString}")

            ' محاولة الاتصال
            Using connection As New SqlConnection(tempConnectionString)
                Try
                    connection.Open()

                    ' تنفيذ استعلام للحصول على جميع قواعد البيانات
                    Dim query As String = "SELECT name FROM sys.databases WHERE state = 0 ORDER BY name"
                    Using command As New SqlCommand(query, connection)
                        Using reader As SqlDataReader = command.ExecuteReader()
                            While reader.Read()
                                Dim dbName As String = reader("name").ToString()
                                ' تجنب إضافة قواعد البيانات النظامية غير الضرورية
                                If Not String.IsNullOrEmpty(dbName) AndAlso Not databases.Contains(dbName) Then
                                    databases.Add(dbName)
                                End If
                            End While
                        End Using
                    End Using
                Catch ex As SqlException
                    System.Diagnostics.Debug.WriteLine($"خطأ في الاتصال بـ {server}: {ex.Message}")

                    ' محاولة استخدام استعلام بديل
                    Try
                        connection.Close()

                        ' إعادة فتح الاتصال مع قاعدة بيانات رئيسية
                        tempConnectionString = $"{tempConnectionString}Database=master;"
                        Using masterConnection As New SqlConnection(tempConnectionString)
                            masterConnection.Open()

                            ' استخدام استعلام بديل
                            Dim altQuery As String = "SELECT name FROM master.dbo.sysdatabases ORDER BY name"
                            Using command As New SqlCommand(altQuery, masterConnection)
                                Using reader As SqlDataReader = command.ExecuteReader()
                                    While reader.Read()
                                        Dim dbName As String = reader("name").ToString()
                                        If Not String.IsNullOrEmpty(dbName) AndAlso Not databases.Contains(dbName) Then
                                            databases.Add(dbName)
                                        End If
                                    End While
                                End Using
                            End Using
                        End Using
                    Catch altEx As Exception
                        System.Diagnostics.Debug.WriteLine($"فشل الاستعلام البديل: {altEx.Message}")
                        Throw
                    End Try
                End Try
            End Using

            ' إذا لم يتم العثور على قواعد بيانات، إضافة قواعد بيانات افتراضية
            If databases.Count = 0 Then
                databases.Add(DEFAULT_DATABASE)
                databases.Add("master")
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في جلب قواعد البيانات من {server}: {ex.Message}")

            ' إضافة تفاصيل الخطأ إلى رسالة الاستثناء
            Throw New Exception($"فشل الاتصال بالسيرفر: {server}{vbCrLf}التفاصيل: {ex.Message}")
        End Try

        Return databases
    End Function

    Public Sub OpenConnection()
        Try
            ' لا حاجة لهذه الدالة بعد الآن لأننا نستخدم دالة Con() مباشرة
        Catch ex As Exception
            Throw New Exception("فشل الاتصال بقاعدة البيانات: " & ex.Message)
        End Try
    End Sub

    Public Sub CloseConnection()
        Try
            ' لا حاجة لهذه الدالة بعد الآن لأننا نستخدم Using statements التي تغلق الاتصال تلقائيًا
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إغلاق الاتصال: {ex.Message}")
        End Try
    End Sub

    ' دالة تنفيذ استعلام مع معاملات وإرجاع DataTable
    Public Function GetData(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True) As DataTable
        Dim dt As New DataTable
        Try
            Using conn As SqlConnection = Con()
                conn.Open()
                Using cmd As New SqlCommand(cmdText, conn)
                    If isStoredProc Then
                        cmd.CommandType = CommandType.StoredProcedure
                    End If

                    If params IsNot Nothing Then
                        cmd.Parameters.AddRange(params)
                    End If

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("فشل في جلب البيانات: " & ex.Message)
        End Try
        Return dt
    End Function

    ''' <summary>
    ''' تنفيذ استعلام SELECT وإرجاع DataTable
    ''' </summary>
    ''' <param name="query">الاستعلام المراد تنفيذه</param>
    ''' <param name="parameters">المعاملات (اختيارية)</param>
    ''' <returns>DataTable</returns>
    Public Function ExecuteQuery(query As String, Optional parameters As SqlParameter() = Nothing) As DataTable
        Dim dataTable As New DataTable()

        Try
            Using connection As SqlConnection = Con()
                Using command As New SqlCommand(query, connection)
                    If parameters IsNot Nothing Then
                        command.Parameters.AddRange(parameters)
                    End If

                    connection.Open()
                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(dataTable)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
        End Try

        Return dataTable
    End Function

    ''' <summary>
    ''' تنفيذ استعلام INSERT, UPDATE, DELETE
    ''' </summary>
    ''' <param name="query">الاستعلام المراد تنفيذه</param>
    ''' <param name="parameters">المعاملات (اختيارية)</param>
    ''' <returns>عدد الصفوف المتأثرة</returns>
    Public Function ExecuteNonQuery(query As String, Optional parameters As SqlParameter() = Nothing) As Integer
        Dim rowsAffected As Integer = 0

        Try
            Using connection As SqlConnection = Con()
                Using command As New SqlCommand(query, connection)
                    If parameters IsNot Nothing Then
                        command.Parameters.AddRange(parameters)
                    End If

                    connection.Open()
                    rowsAffected = command.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
        End Try

        Return rowsAffected
    End Function

    ' دالة تنفيذ أمر (غير استعلام) مثل insert, update, delete مع معاملات
    Public Sub ExecuteNonQuery(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True)
        Try
            Using conn As SqlConnection = Con()
                conn.Open()
                Using cmd As New SqlCommand(cmdText, conn)
                    If isStoredProc Then
                        cmd.CommandType = CommandType.StoredProcedure
                    End If

                    If params IsNot Nothing Then
                        cmd.Parameters.AddRange(params)
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("فشل في تنفيذ الأمر: " & ex.Message)
        End Try
    End Sub

    '''' <summary>
    '''' تنفيذ إجراء مخزن وإرجاع DataTable
    '''' </summary>
    '''' <param name="procedureName">اسم الإجراء المخزن</param>
    '''' <param name="parameters">المعاملات (اختيارية)</param>
    '''' <returns>DataTable</returns>
    'Public Function ExecuteStoredProcedure(procedureName As String, Optional parameters As SqlParameter() = Nothing) As DataTable
    '    Dim dataTable As New DataTable()

    '    Try
    '        Using connection As SqlConnection = Con()
    '            Using command As New SqlCommand(procedureName, connection)
    '                command.CommandType = CommandType.StoredProcedure

    '                If parameters IsNot Nothing Then
    '                    command.Parameters.AddRange(parameters)
    '                End If

    '                connection.Open()
    '                Using adapter As New SqlDataAdapter(command)
    '                    adapter.Fill(dataTable)
    '                End Using
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
    '    End Try

    '    Return dataTable
    'End Function

    ' إضافة الدالة الجديدة لتنفيذ الإجراءات المخزنة التي تعيد بيانات
    ''' <summary>
    ''' تنفيذ إجراء مخزن وإرجاع DataTable
    ''' </summary>
    ''' <param name="procedureName">اسم الإجراء المخزن</param>
    ''' <param name="parameters">المعاملات (اختيارية)</param>
    ''' <param name="connection">الاتصال المستخدم (اختياري)</param>
    ''' <param name="transaction">المعاملة (اختيارية)</param>
    ''' <returns>DataTable</returns>
    Public Function ExecuteStoredProcedure(
    procedureName As String,
    Optional parameters As SqlParameter() = Nothing,
    Optional connection As SqlConnection = Nothing,
    Optional transaction As SqlTransaction = Nothing
) As DataTable
        Dim dataTable As New DataTable()

        Try
            ' إذا لم يتم توفير اتصال، استخدم اتصالًا جديدًا
            If connection Is Nothing Then
                connection = Con()
            End If

            Using command As New SqlCommand(procedureName, connection)
                command.CommandTimeout = 60 ' زيادة وقت التنفيذ إلى 60 ثانية
                command.CommandType = CommandType.StoredProcedure

                If parameters IsNot Nothing Then
                    command.Parameters.AddRange(parameters)
                End If

                If transaction IsNot Nothing Then
                    ' التحقق من أن المعاملة مرتبطة بالاتصال الحالي
                    If transaction.Connection Is Nothing OrElse transaction.Connection IsNot connection Then
                        Throw New InvalidOperationException("المعاملة غير مقترنة بالاتصال الحالي")
                    End If
                    command.Transaction = transaction
                End If

                ' فتح الاتصال فقط إذا لم يكن مفتوحًا بالفعل
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If

                Using adapter As New SqlDataAdapter(command)
                    adapter.Fill(dataTable)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
        End Try

        Return dataTable
    End Function


    Public Function ExecuteStoredProcedureQuery(storedProcedureName As String, ParamArray parameters As SqlParameter()) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = Con()
                conn.Open()
                Using cmd As New SqlCommand(storedProcedureName, conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                        cmd.Parameters.AddRange(parameters)
                    End If
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
            Return dt
        Catch ex As Exception
            Throw New Exception($"خطأ في تنفيذ الإجراء المخزن {storedProcedureName}: {ex.Message}")
        End Try
    End Function


    ''' <summary>
    ''' تنفيذ إجراء مخزن بدون إرجاع بيانات
    ''' </summary>
    ''' <param name="procedureName">اسم الإجراء المخزن</param>
    ''' <param name="parameters">المعاملات (اختيارية)</param>
    ''' <param name="connection">الاتصال (اختياري)</param>
    ''' <param name="transaction">المعاملة (اختيارية)</param>
    ''' <returns>عدد الصفوف المتأثرة</returns>
    Public Function ExecuteStoredProcedureNonQuery(
    procedureName As String,
    Optional parameters As SqlParameter() = Nothing,
    Optional connection As SqlConnection = Nothing,
    Optional transaction As SqlTransaction = Nothing
) As Integer
        Dim rowsAffected As Integer = 0

        Try
            ' إذا لم يتم توفير اتصال، استخدم اتصالًا جديدًا
            If connection Is Nothing Then
                connection = Con()
            End If

            Using command As New SqlCommand(procedureName, connection)
                command.CommandType = CommandType.StoredProcedure

                If parameters IsNot Nothing Then
                    command.Parameters.AddRange(parameters)
                End If

                ' إذا تم توفير معاملة، استخدمها
                If transaction IsNot Nothing Then
                    ' التحقق من أن المعاملة مرتبطة بالاتصال الحالي
                    If transaction.Connection Is Nothing OrElse transaction.Connection IsNot connection Then
                        Throw New InvalidOperationException("المعاملة غير مقترنة بالاتصال الحالي")
                    End If
                    command.Transaction = transaction
                End If

                ' فتح الاتصال فقط إذا لم يكن مفتوحًا بالفعل
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If

                rowsAffected = command.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
        End Try

        Return rowsAffected
    End Function


    ''' <summary>
    ''' تنفيذ إجراء مخزن بدون إرجاع بيانات
    ''' </summary>
    ''' <param name="procedureName">اسم الإجراء المخزن</param>
    ''' <param name="parameters">المعاملات (اختيارية)</param>
    ''' <returns>عدد الصفوف المتأثرة</returns>
    Public Function ExecuteStoredProcedureNonQuery(procedureName As String, Optional parameters As SqlParameter() = Nothing) As Integer
        Dim rowsAffected As Integer = 0

        Try
            Using connection As SqlConnection = Con()
                Using command As New SqlCommand(procedureName, connection)
                    command.CommandType = CommandType.StoredProcedure

                    If parameters IsNot Nothing Then
                        command.Parameters.AddRange(parameters)
                    End If

                    connection.Open()
                    rowsAffected = command.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
        End Try

        Return rowsAffected
    End Function

    ''' <summary>
    ''' تنفيذ استعلام وإرجاع قيمة واحدة
    ''' </summary>
    ''' <param name="query">الاستعلام المراد تنفيذه</param>
    ''' <param name="parameters">المعاملات (اختيارية)</param>
    ''' <returns>القيمة المرجعة</returns>
    Public Function ExecuteScalar(query As String, Optional parameters As SqlParameter() = Nothing) As Object
        Dim result As Object = Nothing

        Try
            Using connection As SqlConnection = Con()
                Using command As New SqlCommand(query, connection)
                    If parameters IsNot Nothing Then
                        command.Parameters.AddRange(parameters)
                    End If

                    connection.Open()
                    result = command.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
        End Try

        Return result
    End Function

    ' دالة تنفيذ استعلام مع معاملات وإرجاع قيمة وحيدة
    Public Function ExecuteScalar(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True) As Object
        Dim result As Object = Nothing
        Try
            Using conn As SqlConnection = Con()
                conn.Open()
                Using cmd As New SqlCommand(cmdText, conn)
                    If isStoredProc Then
                        cmd.CommandType = CommandType.StoredProcedure
                    End If

                    If params IsNot Nothing Then
                        cmd.Parameters.AddRange(params)
                    End If

                    result = cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("فشل في تنفيذ العملية: " & ex.Message)
        End Try
        Return result
    End Function

    ''' <summary>
    ''' اختبار الاتصال بقاعدة البيانات
    ''' </summary>
    ''' <returns>True إذا كان الاتصال ناجح، False إذا فشل</returns>
    Public Function TestConnection() As Boolean
        Try
            Using connection As SqlConnection = Con()
                connection.Open()
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' إنشاء معامل SQL
    ''' </summary>
    ''' <param name="parameterName">اسم المعامل</param>
    ''' <param name="value">قيمة المعامل</param>
    ''' <returns>SqlParameter</returns>
    Public Function CreateParameter(parameterName As String, value As Object) As SqlParameter
        Return New SqlParameter(parameterName, If(value, DBNull.Value))
    End Function

    ''' <summary>
    ''' إنشاء معامل SQL بنوع وحجم محددين
    ''' </summary>
    ''' <param name="parameterName">اسم المعامل</param>
    ''' <param name="value">قيمة المعامل</param>
    ''' <param name="dbType">نوع البيانات في قاعدة البيانات</param>
    ''' <param name="size">حجم البيانات (اختياري)</param>
    ''' <returns>SqlParameter</returns>
    Public Function CreateParameter(parameterName As String, value As Object, dbType As SqlDbType, Optional size As Integer = 0) As SqlParameter
        Dim param As New SqlParameter(parameterName, dbType)
        If value Is Nothing Then
            param.Value = DBNull.Value
        Else
            param.Value = value
        End If
        If size > 0 Then
            param.Size = size
        End If
        Return param
    End Function

    ''' <summary>
    ''' إنشاء مجموعة من المعاملات
    ''' </summary>
    ''' <param name="parameterPairs">أزواج اسم المعامل والقيمة</param>
    ''' <returns>مصفوفة من SqlParameter</returns>
    Public Function CreateParameters(ParamArray parameterPairs As Object()) As SqlParameter()
        If parameterPairs.Length Mod 2 <> 0 Then
            Throw New ArgumentException("عدد المعاملات يجب أن يكون زوجي (اسم، قيمة)")
        End If

        Dim parameters(parameterPairs.Length \ 2 - 1) As SqlParameter

        For i As Integer = 0 To parameterPairs.Length - 1 Step 2
            parameters(i \ 2) = CreateParameter(parameterPairs(i).ToString(), parameterPairs(i + 1))
        Next

        Return parameters
    End Function

    ' تعبئة ComboBox
    Public Sub fillCombo(ByVal cmb As ComboBox, ByVal sql As String, ByVal displayvalue As String)
        Try
            Using adp As New SqlDataAdapter(sql, Con())
                Dim DS As New DataSet
                adp.Fill(DS)
                Dim dt = DS.Tables(0)
                cmb.Items.Clear()

                For Each row As DataRow In dt.Rows
                    cmb.Items.Add(row(displayvalue).ToString())
                Next

                ' إزالة التكرارات
                Dim uniqueItems = cmb.Items.Cast(Of String).Distinct().ToList()
                cmb.Items.Clear()
                cmb.Items.AddRange(uniqueItems.ToArray())
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' دالة عامة للبحث في جدول قاعدة البيانات باستخدام حقل معين
    ''' </summary>
    ''' <param name="tableName">اسم الجدول</param>
    ''' <param name="searchColumn">اسم الحقل المراد البحث فيه</param>
    ''' <param name="searchTerm">القيمة المراد البحث عنها</param>
    ''' <param name="connection">الاتصال بقاعدة البيانات</param>
    ''' <returns>DataTable يحتوي على النتائج</returns>
    Public Function SearchInDatabase(
        ByVal tableName As String,
        ByVal searchColumn As String,
        ByVal searchTerm As String,
        ByVal connection As SqlConnection) As DataTable

        Dim dt As New DataTable()

        ' التحقق من صحة المدخلات
        If String.IsNullOrEmpty(tableName) OrElse String.IsNullOrEmpty(searchColumn) Then
            MessageBox.Show("اسم الجدول أو الحقل غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return dt
        End If

        If String.IsNullOrEmpty(searchTerm) Then
            Return dt ' إرجاع جدول فارغ إذا كان البحث فارغًا
        End If

        Try
            Using cmd As New SqlCommand()
                cmd.Connection = connection
                cmd.CommandText = $"SELECT * FROM {tableName} WHERE {searchColumn} LIKE @SearchTerm"
                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm.Trim() & "%")

                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        Catch ex As SqlException
            MessageBox.Show("حدث خطأ في قاعدة البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("حدث خطأ عام: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return dt
    End Function
End Module



'Imports System.Data.SqlClient
'Imports System.Configuration
'Imports System.IO
'Imports System.Text.RegularExpressions
'Imports System.Data.Sql

'Module DatabaseHelper

'    ' سلسلة الاتصال بقاعدة البيانات
'    Private connectionString As String = ""
'    Private conn As SqlConnection

'    ' القيم الافتراضية
'    Public Const DEFAULT_SERVER As String = "localhost"
'    Public Const DEFAULT_DATABASE As String = "ESIS"
'    Public Const DEFAULT_PORT As String = "1433"

'    ''' <summary>
'    ''' الحصول على اتصال جديد بقاعدة البيانات
'    ''' </summary>
'    ''' <returns>SqlConnection</returns>
'    Public Function GetConnection() As SqlConnection
'        Try
'            If String.IsNullOrEmpty(connectionString) Then
'                connectionString = GetSavedConnectionString()
'            End If

'            Return New SqlConnection(connectionString)
'        Catch ex As Exception
'            Throw New Exception("فشل إنشاء اتصال قاعدة البيانات: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' الحصول على سلسلة الاتصال المحفوظة من ملف التكوين
'    ''' </summary>
'    Public Function GetSavedConnectionString() As String
'        Try
'            Dim configPath As String = Path.Combine(Application.StartupPath, "Connection.config")
'            If File.Exists(configPath) Then
'                Return File.ReadAllText(configPath).Trim()
'            End If

'            ' السلسلة الافتراضية إذا لم توجد إعدادات محفوظة
'            Return $"Server={DEFAULT_SERVER};Database={DEFAULT_DATABASE};Integrated Security=True;"

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على سلسلة الاتصال: {ex.Message}")
'            Return $"Server={DEFAULT_SERVER};Database={DEFAULT_DATABASE};Integrated Security=True;"
'        End Try
'    End Function

'    ''' <summary>
'    ''' حفظ سلسلة الاتصال
'    ''' </summary>
'    Public Function SaveConnectionString(connectionString As String) As Boolean
'        Try
'            Dim configPath As String = Path.Combine(Application.StartupPath, "Connection.config")
'            File.WriteAllText(configPath, connectionString)
'            Return True
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في حفظ سلسلة الاتصال: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' بناء سلسلة اتصال بناءً على المعلمات المقدمة
'    ''' </summary>
'    Public Function BuildConnectionString(server As String, port As String, database As String, useWindowsAuth As Boolean, Optional username As String = "", Optional password As String = "") As String
'        Try
'            ' إذا كان البورت فارغًا، استخدم القيمة الافتراضية
'            If String.IsNullOrEmpty(port) Then port = DEFAULT_PORT

'            ' إذا كان الاتصال عبر الشبكة المحلية، قد لا يكون هناك بورت
'            Dim serverWithPort As String = server
'            If Not String.IsNullOrEmpty(port) AndAlso port <> DEFAULT_PORT Then
'                serverWithPort = $"{server},{port}"
'            End If

'            ' بناء سلسلة الاتصال بناءً على نوع المصادقة
'            If useWindowsAuth Then
'                Return $"Server={serverWithPort};Database={database};Integrated Security=True;"
'            Else
'                Return $"Server={serverWithPort};Database={database};User ID={username};Password={password};"
'            End If

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في بناء سلسلة الاتصال: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' استخراج اسم السيرفر من سلسلة الاتصال
'    ''' </summary>
'    Public Function GetServerNameFromConnectionString(connectionString As String) As String
'        Try
'            ' استخدام تعبير عادي لاستخراج اسم السيرفر
'            Dim match As Match = Regex.Match(connectionString, "Server\s*=\s*([^;]+)", RegexOptions.IgnoreCase)
'            If match.Success Then
'                Dim serverPart As String = match.Groups(1).Value

'                ' استخراج جزء السيرفر فقط (بدون البورت)
'                Dim serverName As String = serverPart.Split(","c)(0).Trim()
'                Return serverName
'            End If

'            Return DEFAULT_SERVER

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في استخراج اسم السيرفر: {ex.Message}")
'            Return DEFAULT_SERVER
'        End Try
'    End Function

'    ''' <summary>
'    ''' استخراج رقم البورت من سلسلة الاتصال
'    ''' </summary>
'    Public Function GetPortFromConnectionString(connectionString As String) As String
'        Try
'            ' استخدام تعبير عادي لاستخراج اسم السيرفر
'            Dim match As Match = Regex.Match(connectionString, "Server\s*=\s*([^;]+)", RegexOptions.IgnoreCase)
'            If match.Success Then
'                Dim serverPart As String = match.Groups(1).Value

'                ' التحقق من وجود بورت
'                If serverPart.Contains(",") Then
'                    Dim port As String = serverPart.Split(","c)(1).Trim()
'                    Return port
'                End If
'            End If

'            Return DEFAULT_PORT

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في استخراج رقم البورت: {ex.Message}")
'            Return DEFAULT_PORT
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق مما إذا كانت المصادقة مدمجة (Windows Authentication)
'    ''' </summary>
'    Public Function IsIntegratedSecurity(connectionString As String) As Boolean
'        Try
'            Return connectionString.IndexOf("Integrated Security=True", StringComparison.OrdinalIgnoreCase) >= 0
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في التحقق من نوع المصادقة: {ex.Message}")
'            Return True ' الافتراضي هو Windows Authentication
'        End Try
'    End Function

'    ''' <summary>
'    ''' اكتشاف السيرفرات المتاحة على الشبكة
'    ''' </summary>
'    Public Function DiscoverAvailableServers() As List(Of String)
'        Dim servers As New List(Of String)()

'        Try
'            ' استخدام SqlDataSourceEnumerator لاكتشاف السيرفرات
'            Dim instance As SqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance
'            Dim table As DataTable = instance.GetDataSources()

'            For Each row As DataRow In table.Rows
'                Dim serverName As String = row("ServerName").ToString()
'                Dim instanceName As String = row("InstanceName").ToString()

'                ' إذا كان هناك اسم مثيل، أضفه إلى اسم السيرفر
'                If Not String.IsNullOrEmpty(instanceName) AndAlso instanceName <> "MSSQLSERVER" Then
'                    servers.Add($"{serverName}\{instanceName}")
'                Else
'                    servers.Add(serverName)
'                End If
'            Next

'            ' إذا لم يتم العثور على أي سيرفر، أضف الافتراضي
'            If servers.Count = 0 Then
'                servers.Add(DEFAULT_SERVER)
'            End If

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في اكتشاف السيرفرات: {ex.Message}")

'            ' إضافة السيرفرات الافتراضية
'            servers.Add(".")
'            servers.Add("(local)")
'            servers.Add("localhost")
'            servers.Add("127.0.0.1")
'        End Try

'        Return servers
'    End Function

'    '''' <summary>
'    '''' جلب قواعد البيانات من سيرفر معين
'    '''' </summary>
'    'Public Function GetDatabases(server As String, Optional port As String = DEFAULT_PORT, Optional useWindowsAuth As Boolean = True, Optional username As String = "", Optional password As String = "") As List(Of String)
'    '    Dim databases As New List(Of String)()
'    '    Dim tempConnectionString As String = ""

'    '    Try
'    '        ' بناء سلسلة اتصال مؤقتة بدون تحديد قاعدة بيانات
'    '        If String.IsNullOrEmpty(port) OrElse port = DEFAULT_PORT Then
'    '            tempConnectionString = $"Server={server};Integrated Security={useWindowsAuth.ToString()};"
'    '        Else
'    '            tempConnectionString = $"Server={server},{port};Integrated Security={useWindowsAuth.ToString()};"
'    '        End If

'    '        ' إذا كانت المصادقة ليست Windows Authentication، أضف اسم المستخدم وكلمة المرور
'    '        If Not useWindowsAuth Then
'    '            tempConnectionString = $"Server={server},{port};User ID={username};Password={password};"
'    '        End If

'    '        ' إضافة قاعدة البيانات الرئيسية كقاعدة بيانات أولية
'    '        databases.Add("master")

'    '        Using connection As New SqlConnection(tempConnectionString)
'    '            connection.Open()

'    '            ' تنفيذ استعلام للحصول على جميع قواعد البيانات
'    '            Dim query As String = "SELECT name FROM sys.databases ORDER BY name"
'    '            Using command As New SqlCommand(query, connection)
'    '                Using reader As SqlDataReader = command.ExecuteReader()
'    '                    While reader.Read()
'    '                        Dim dbName As String = reader("name").ToString()
'    '                        ' تجنب إضافة قواعد البيانات النظامية غير الضرورية
'    '                        If Not databases.Contains(dbName) AndAlso Not String.IsNullOrEmpty(dbName) Then
'    '                            databases.Add(dbName)
'    '                        End If
'    '                    End While
'    '                End Using
'    '            End Using
'    '        End Using

'    '    Catch ex As Exception
'    '        System.Diagnostics.Debug.WriteLine($"فشل في جلب قواعد البيانات من {server}: {ex.Message}")

'    '        ' إضافة قواعد البيانات الافتراضية
'    '        databases.Clear()
'    '        databases.Add(DEFAULT_DATABASE)
'    '        databases.Add("master")
'    '    End Try

'    '    Return databases
'    'End Function

'    Public Sub OpenConnection()
'        Try
'            ' إذا كان المتغير فارغاً أو تم التخلص منه
'            If conn Is Nothing Then
'                conn = New SqlConnection(connectionString)
'            End If

'            If conn.State = ConnectionState.Closed Then
'                conn.Open()
'            End If
'        Catch ex As Exception
'            Throw New Exception("فشل الاتصال بقاعدة البيانات: " & ex.Message)
'        End Try
'    End Sub

'    Public Sub CloseConnection()
'        If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
'            conn.Close()
'        End If
'    End Sub

'    ' دالة تنفيذ استعلام مع معاملات وإرجاع DataTable
'    Public Function GetData(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True) As DataTable
'        Dim dt As New DataTable
'        Try
'            OpenConnection()
'            Using cmd As New SqlCommand(cmdText, conn)
'                If isStoredProc Then
'                    cmd.CommandType = CommandType.StoredProcedure
'                End If

'                If params IsNot Nothing Then
'                    cmd.Parameters.AddRange(params)
'                End If

'                Using da As New SqlDataAdapter(cmd)
'                    da.Fill(dt)
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("فشل في جلب البيانات: " & ex.Message)
'        Finally
'            CloseConnection()
'        End Try
'        Return dt
'    End Function

'    ''' <summary>
'    ''' تنفيذ استعلام SELECT وإرجاع DataTable
'    ''' </summary>
'    ''' <param name="query">الاستعلام المراد تنفيذه</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>DataTable</returns>
'    Public Function ExecuteQuery(query As String, Optional parameters As SqlParameter() = Nothing) As DataTable
'        Dim dataTable As New DataTable()

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(query, connection)
'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    Using adapter As New SqlDataAdapter(command)
'                        adapter.Fill(dataTable)
'                    End Using
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
'        End Try

'        Return dataTable
'    End Function

'    ''' <summary>
'    ''' تنفيذ استعلام INSERT, UPDATE, DELETE
'    ''' </summary>
'    ''' <param name="query">الاستعلام المراد تنفيذه</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>عدد الصفوف المتأثرة</returns>
'    Public Function ExecuteNonQuery(query As String, Optional parameters As SqlParameter() = Nothing) As Integer
'        Dim rowsAffected As Integer = 0

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(query, connection)
'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    rowsAffected = command.ExecuteNonQuery()
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
'        End Try

'        Return rowsAffected
'    End Function

'    ' دالة تنفيذ أمر (غير استعلام) مثل insert, update, delete مع معاملات
'    Public Sub ExecuteNonQuery(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True)
'        Try
'            OpenConnection()
'            Using cmd As New SqlCommand(cmdText, conn)
'                If isStoredProc Then
'                    cmd.CommandType = CommandType.StoredProcedure
'                End If

'                If params IsNot Nothing Then
'                    cmd.Parameters.AddRange(params)
'                End If

'                cmd.ExecuteNonQuery()
'            End Using
'        Catch ex As Exception
'            Throw New Exception("فشل في تنفيذ الأمر: " & ex.Message)
'        Finally
'            CloseConnection()
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تنفيذ إجراء مخزن وإرجاع DataTable
'    ''' </summary>
'    ''' <param name="procedureName">اسم الإجراء المخزن</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>DataTable</returns>
'    Public Function ExecuteStoredProcedure(procedureName As String, Optional parameters As SqlParameter() = Nothing) As DataTable
'        Dim dataTable As New DataTable()

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(procedureName, connection)
'                    command.CommandType = CommandType.StoredProcedure

'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    Using adapter As New SqlDataAdapter(command)
'                        adapter.Fill(dataTable)
'                    End Using
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
'        End Try

'        Return dataTable
'    End Function

'    ' إضافة الدالة الجديدة لتنفيذ الإجراءات المخزنة التي تعيد بيانات
'    Public Function ExecuteStoredProcedureQuery(storedProcedureName As String, ParamArray parameters As SqlParameter()) As DataTable
'        Dim dt As New DataTable()
'        Try
'            Using conn As New SqlConnection(connectionString)
'                conn.Open()
'                Using cmd As New SqlCommand(storedProcedureName, conn)
'                    cmd.CommandType = CommandType.StoredProcedure
'                    If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
'                        cmd.Parameters.AddRange(parameters)
'                    End If
'                    Using adapter As New SqlDataAdapter(cmd)
'                        adapter.Fill(dt)
'                    End Using
'                End Using
'            End Using
'            Return dt
'        Catch ex As Exception
'            Throw New Exception($"خطأ في تنفيذ الإجراء المخزن {storedProcedureName}: {ex.Message}")
'        End Try
'    End Function

'    ''' <summary>
'    ''' تنفيذ إجراء مخزن بدون إرجاع بيانات
'    ''' </summary>
'    ''' <param name="procedureName">اسم الإجراء المخزن</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>عدد الصفوف المتأثرة</returns>
'    Public Function ExecuteStoredProcedureNonQuery(procedureName As String, Optional parameters As SqlParameter() = Nothing) As Integer
'        Dim rowsAffected As Integer = 0

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(procedureName, connection)
'                    command.CommandType = CommandType.StoredProcedure

'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    rowsAffected = command.ExecuteNonQuery()
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
'        End Try

'        Return rowsAffected
'    End Function

'    ''' <summary>
'    ''' تنفيذ استعلام وإرجاع قيمة واحدة
'    ''' </summary>
'    ''' <param name="query">الاستعلام المراد تنفيذه</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>القيمة المرجعة</returns>
'    Public Function ExecuteScalar(query As String, Optional parameters As SqlParameter() = Nothing) As Object
'        Dim result As Object = Nothing

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(query, connection)
'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    result = command.ExecuteScalar()
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
'        End Try

'        Return result
'    End Function

'    ' دالة تنفيذ استعلام مع معاملات وإرجاع قيمة وحيدة
'    Public Function ExecuteScalar(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True) As Object
'        Dim result As Object = Nothing
'        Try
'            OpenConnection()
'            Using cmd As New SqlCommand(cmdText, conn)
'                If isStoredProc Then
'                    cmd.CommandType = CommandType.StoredProcedure
'                End If

'                If params IsNot Nothing Then
'                    cmd.Parameters.AddRange(params)
'                End If

'                result = cmd.ExecuteScalar()
'            End Using
'        Catch ex As Exception
'            Throw New Exception("فشل في تنفيذ العملية: " & ex.Message)
'        Finally
'            CloseConnection()
'        End Try
'        Return result
'    End Function

'    ''' <summary>
'    ''' اختبار الاتصال بقاعدة البيانات
'    ''' </summary>
'    ''' <returns>True إذا كان الاتصال ناجح، False إذا فشل</returns>
'    Public Function TestConnection() As Boolean
'        Try
'            Using connection As SqlConnection = GetConnection()
'                connection.Open()
'                Return True
'            End Using
'        Catch ex As Exception
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' إنشاء معامل SQL
'    ''' </summary>
'    ''' <param name="parameterName">اسم المعامل</param>
'    ''' <param name="value">قيمة المعامل</param>
'    ''' <returns>SqlParameter</returns>
'    Public Function CreateParameter(parameterName As String, value As Object) As SqlParameter
'        Return New SqlParameter(parameterName, If(value, DBNull.Value))
'    End Function

'    ''' <summary>
'    ''' إنشاء معامل SQL بنوع وحجم محددين
'    ''' </summary>
'    ''' <param name="parameterName">اسم المعامل</param>
'    ''' <param name="value">قيمة المعامل</param>
'    ''' <param name="dbType">نوع البيانات في قاعدة البيانات</param>
'    ''' <param name="size">حجم البيانات (اختياري)</param>
'    ''' <returns>SqlParameter</returns>
'    Public Function CreateParameter(parameterName As String, value As Object, dbType As SqlDbType, Optional size As Integer = 0) As SqlParameter
'        Dim param As New SqlParameter(parameterName, dbType)
'        If value Is Nothing Then
'            param.Value = DBNull.Value
'        Else
'            param.Value = value
'        End If
'        If size > 0 Then
'            param.Size = size
'        End If
'        Return param
'    End Function

'    ''' <summary>
'    ''' إنشاء مجموعة من المعاملات
'    ''' </summary>
'    ''' <param name="parameterPairs">أزواج اسم المعامل والقيمة</param>
'    ''' <returns>مصفوفة من SqlParameter</returns>
'    Public Function CreateParameters(ParamArray parameterPairs As Object()) As SqlParameter()
'        If parameterPairs.Length Mod 2 <> 0 Then
'            Throw New ArgumentException("عدد المعاملات يجب أن يكون زوجي (اسم، قيمة)")
'        End If

'        Dim parameters(parameterPairs.Length \ 2 - 1) As SqlParameter

'        For i As Integer = 0 To parameterPairs.Length - 1 Step 2
'            parameters(i \ 2) = CreateParameter(parameterPairs(i).ToString(), parameterPairs(i + 1))
'        Next

'        Return parameters
'    End Function

'    ' تعبئة ComboBox
'    Public Sub fillCombo(ByVal cmb As ComboBox, ByVal sql As String, ByVal displayvalue As String)
'        Try
'            Using adp As New SqlDataAdapter(sql, GetConnection)
'                Dim DS As New DataSet
'                adp.Fill(DS)
'                Dim dt = DS.Tables(0)
'                cmb.Items.Clear()

'                For Each row As DataRow In dt.Rows
'                    cmb.Items.Add(row(displayvalue).ToString())
'                Next

'                ' إزالة التكرارات
'                Dim uniqueItems = cmb.Items.Cast(Of String).Distinct().ToList()
'                cmb.Items.Clear()
'                cmb.Items.AddRange(uniqueItems.ToArray())
'            End Using
'        Catch ex As Exception
'            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' دالة عامة للبحث في جدول قاعدة البيانات باستخدام حقل معين
'    ''' </summary>
'    ''' <param name="tableName">اسم الجدول</param>
'    ''' <param name="searchColumn">اسم الحقل المراد البحث فيه</param>
'    ''' <param name="searchTerm">القيمة المراد البحث عنها</param>
'    ''' <param name="connection">الاتصال بقاعدة البيانات</param>
'    ''' <returns>DataTable يحتوي على النتائج</returns>
'    Public Function SearchInDatabase(
'        ByVal tableName As String,
'        ByVal searchColumn As String,
'        ByVal searchTerm As String,
'        ByVal connection As SqlConnection) As DataTable

'        Dim dt As New DataTable()

'        ' التحقق من صحة المدخلات
'        If String.IsNullOrEmpty(tableName) OrElse String.IsNullOrEmpty(searchColumn) Then
'            MessageBox.Show("اسم الجدول أو الحقل غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Return dt
'        End If

'        If String.IsNullOrEmpty(searchTerm) Then
'            Return dt ' إرجاع جدول فارغ إذا كان البحث فارغًا
'        End If

'        Try
'            Using cmd As New SqlCommand()
'                cmd.Connection = connection
'                cmd.CommandText = $"SELECT * FROM {tableName} WHERE {searchColumn} LIKE @SearchTerm"
'                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm.Trim() & "%")

'                Using da As New SqlDataAdapter(cmd)
'                    da.Fill(dt)
'                End Using
'            End Using
'        Catch ex As SqlException
'            MessageBox.Show("حدث خطأ في قاعدة البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        Catch ex As Exception
'            MessageBox.Show("حدث خطأ عام: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try

'        Return dt
'    End Function


'    ''' <summary>
'    ''' جلب قواعد البيانات من سيرفر معين مع معالجة أفضل للأخطاء
'    ''' </summary>
'    Public Function GetDatabases(server As String, Optional port As String = DEFAULT_PORT, Optional useWindowsAuth As Boolean = True, Optional username As String = "", Optional password As String = "") As List(Of String)
'        Dim databases As New List(Of String)()

'        Try
'            ' تهيئة متغيرات لعرض تفاصيل الخطأ
'            Dim connectionDetails As String = ""

'            Try
'                ' بناء سلسلة اتصال مؤقتة بدون تحديد قاعدة بيانات
'                Dim tempConnectionString As String = ""

'                ' تهيئة متغير لعرض تفاصيل الاتصال
'                connectionDetails = $"السيرفر: {server}, البورت: {port}, المصادقة: {If(useWindowsAuth, "Windows", "SQL Server")}"

'                ' معالجة خاصة لـ localhost
'                If server.Equals("localhost", StringComparison.OrdinalIgnoreCase) OrElse server.Equals("(local)", StringComparison.OrdinalIgnoreCase) OrElse server.Equals(".", StringComparison.OrdinalIgnoreCase) Then
'                    server = "."
'                End If

'                ' معالجة خاصة للاتصال عبر البورت
'                If Not String.IsNullOrEmpty(port) AndAlso port <> DEFAULT_PORT Then
'                    ' استخدام صيغة صحيحة للاتصال عبر البورت
'                    tempConnectionString = $"Server={server},{port};"
'                Else
'                    tempConnectionString = $"Server={server};"
'                End If

'                ' إضافة معلومات المصادقة
'                If useWindowsAuth Then
'                    tempConnectionString &= "Integrated Security=True;"
'                Else
'                    If String.IsNullOrWhiteSpace(username) OrElse String.IsNullOrWhiteSpace(password) Then
'                        Throw New Exception("اسم المستخدم وكلمة المرور مطلوبان عند استخدام مصادقة SQL Server")
'                    End If
'                    tempConnectionString &= $"User ID={username};Password={password};"
'                End If

'                ' إضافة وقت الانتظار للاتصال
'                tempConnectionString &= "Connect Timeout=5;"

'                ' تسجيل سلسلة الاتصال (بدون كلمة المرور للخصوصية)
'                Dim logConnectionString As String = tempConnectionString
'                If Not useWindowsAuth Then
'                    logConnectionString = logConnectionString.Replace(password, "******")
'                End If
'                System.Diagnostics.Debug.WriteLine($"محاولة الاتصال بسلسلة: {logConnectionString}")

'                ' محاولة الاتصال
'                Using connection As New SqlConnection(tempConnectionString)
'                    Try
'                        connection.Open()

'                        ' تنفيذ استعلام للحصول على جميع قواعد البيانات
'                        Dim query As String = "SELECT name FROM sys.databases WHERE state = 0 ORDER BY name"
'                        Using command As New SqlCommand(query, connection)
'                            Using reader As SqlDataReader = command.ExecuteReader()
'                                While reader.Read()
'                                    Dim dbName As String = reader("name").ToString()
'                                    ' تجنب إضافة قواعد البيانات النظامية غير الضرورية
'                                    If Not String.IsNullOrEmpty(dbName) AndAlso Not databases.Contains(dbName) Then
'                                        databases.Add(dbName)
'                                    End If
'                                End While
'                            End Using
'                        End Using
'                    Catch ex As SqlException
'                        System.Diagnostics.Debug.WriteLine($"خطأ في الاتصال بـ {server}: {ex.Message}")

'                        ' محاولة استخدام استعلام بديل
'                        Try
'                            connection.Close()

'                            ' إعادة فتح الاتصال مع قاعدة بيانات رئيسية
'                            tempConnectionString = $"{tempConnectionString}Database=master;"
'                            Using masterConnection As New SqlConnection(tempConnectionString)
'                                masterConnection.Open()

'                                ' استخدام استعلام بديل
'                                Dim altQuery As String = "SELECT name FROM master.dbo.sysdatabases ORDER BY name"
'                                Using command As New SqlCommand(altQuery, masterConnection)
'                                    Using reader As SqlDataReader = command.ExecuteReader()
'                                        While reader.Read()
'                                            Dim dbName As String = reader("name").ToString()
'                                            If Not String.IsNullOrEmpty(dbName) AndAlso Not databases.Contains(dbName) Then
'                                                databases.Add(dbName)
'                                            End If
'                                        End While
'                                    End Using
'                                End Using
'                            End Using
'                        Catch altEx As Exception
'                            System.Diagnostics.Debug.WriteLine($"فشل الاستعلام البديل: {altEx.Message}")
'                            Throw
'                        End Try
'                    End Try
'                End Using

'                ' إذا لم يتم العثور على قواعد بيانات، إضافة قواعد بيانات افتراضية
'                If databases.Count = 0 Then
'                    databases.Add(DEFAULT_DATABASE)
'                    databases.Add("master")
'                End If

'            Catch ex As Exception
'                System.Diagnostics.Debug.WriteLine($"فشل في جلب قواعد البيانات من {server}: {ex.Message}")

'                ' إضافة تفاصيل الخطأ إلى رسالة الاستثناء
'                Throw New Exception($"فشل الاتصال بالسيرفر: {connectionDetails}{vbCrLf}التفاصيل: {ex.Message}")
'            End Try

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في GetDatabases: {ex.Message}")

'            ' إعداد قائمة افتراضية مع رسالة خطأ
'            databases.Clear()
'            databases.Add($"خطأ: {ex.Message}")
'            databases.Add(DEFAULT_DATABASE)
'            databases.Add("master")
'        End Try

'        Return databases
'    End Function

'End Module






'Imports System.Data.SqlClient
'Imports System.Configuration
'Imports System.IO
'Imports System.Text.RegularExpressions

'Module DatabaseHelper

'    ' سلسلة الاتصال بقاعدة البيانات
'    Private connectionString As String = ""
'    Private conn As SqlConnection

'    ' القيم الافتراضية
'    Public Const DEFAULT_SERVER As String = "localhost"
'    Public Const DEFAULT_DATABASE As String = "ESIS"
'    Public Const DEFAULT_PORT As String = "1433"

'    ''' <summary>
'    ''' الحصول على اتصال جديد بقاعدة البيانات
'    ''' </summary>
'    ''' <returns>SqlConnection</returns>
'    Public Function GetConnection() As SqlConnection
'        Try
'            If String.IsNullOrEmpty(connectionString) Then
'                connectionString = GetSavedConnectionString()
'            End If

'            Return New SqlConnection(connectionString)
'        Catch ex As Exception
'            Throw New Exception("فشل إنشاء اتصال قاعدة البيانات: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' الحصول على سلسلة الاتصال المحفوظة من ملف التكوين
'    ''' </summary>
'    Public Function GetSavedConnectionString() As String
'        Try
'            Dim configPath As String = Path.Combine(Application.StartupPath, "Connection.config")
'            If File.Exists(configPath) Then
'                Return File.ReadAllText(configPath).Trim()
'            End If

'            ' السلسلة الافتراضية إذا لم توجد إعدادات محفوظة
'            Return $"Server={DEFAULT_SERVER};Database={DEFAULT_DATABASE};Integrated Security=True;"

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على سلسلة الاتصال: {ex.Message}")
'            Return $"Server={DEFAULT_SERVER};Database={DEFAULT_DATABASE};Integrated Security=True;"
'        End Try
'    End Function

'    ''' <summary>
'    ''' حفظ سلسلة الاتصال
'    ''' </summary>
'    Public Function SaveConnectionString(connectionString As String) As Boolean
'        Try
'            Dim configPath As String = Path.Combine(Application.StartupPath, "Connection.config")
'            File.WriteAllText(configPath, connectionString)
'            Return True
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في حفظ سلسلة الاتصال: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' بناء سلسلة اتصال بناءً على المعلمات المقدمة
'    ''' </summary>
'    Public Function BuildConnectionString(server As String, port As String, database As String, useWindowsAuth As Boolean, Optional username As String = "", Optional password As String = "") As String
'        Try
'            ' إذا كان البورت فارغًا، استخدم القيمة الافتراضية
'            If String.IsNullOrEmpty(port) Then port = DEFAULT_PORT

'            ' إذا كان الاتصال عبر الشبكة المحلية، قد لا يكون هناك بورت
'            Dim serverWithPort As String = server
'            If Not String.IsNullOrEmpty(port) AndAlso port <> DEFAULT_PORT Then
'                serverWithPort = $"{server},{port}"
'            End If

'            ' بناء سلسلة الاتصال بناءً على نوع المصادقة
'            If useWindowsAuth Then
'                Return $"Server={serverWithPort};Database={database};Integrated Security=True;"
'            Else
'                Return $"Server={serverWithPort};Database={database};User ID={username};Password={password};"
'            End If

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في بناء سلسلة الاتصال: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' استخراج اسم السيرفر من سلسلة الاتصال
'    ''' </summary>
'    Public Function GetServerNameFromConnectionString(connectionString As String) As String
'        Try
'            ' استخدام تعبير عادي لاستخراج اسم السيرفر
'            Dim match As Match = Regex.Match(connectionString, "Server\s*=\s*([^;]+)", RegexOptions.IgnoreCase)
'            If match.Success Then
'                Dim serverPart As String = match.Groups(1).Value

'                ' استخراج جزء السيرفر فقط (بدون البورت)
'                Dim serverName As String = serverPart.Split(","c)(0).Trim()
'                Return serverName
'            End If

'            Return DEFAULT_SERVER

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في استخراج اسم السيرفر: {ex.Message}")
'            Return DEFAULT_SERVER
'        End Try
'    End Function

'    ''' <summary>
'    ''' استخراج رقم البورت من سلسلة الاتصال
'    ''' </summary>
'    Public Function GetPortFromConnectionString(connectionString As String) As String
'        Try
'            ' استخدام تعبير عادي لاستخراج اسم السيرفر
'            Dim match As Match = Regex.Match(connectionString, "Server\s*=\s*([^;]+)", RegexOptions.IgnoreCase)
'            If match.Success Then
'                Dim serverPart As String = match.Groups(1).Value

'                ' التحقق من وجود بورت
'                If serverPart.Contains(",") Then
'                    Dim port As String = serverPart.Split(","c)(1).Trim()
'                    Return port
'                End If
'            End If

'            Return DEFAULT_PORT

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في استخراج رقم البورت: {ex.Message}")
'            Return DEFAULT_PORT
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق مما إذا كانت المصادقة مدمجة (Windows Authentication)
'    ''' </summary>
'    Public Function IsIntegratedSecurity(connectionString As String) As Boolean
'        Try
'            Return connectionString.IndexOf("Integrated Security=True", StringComparison.OrdinalIgnoreCase) >= 0
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في التحقق من نوع المصادقة: {ex.Message}")
'            Return True ' الافتراضي هو Windows Authentication
'        End Try
'    End Function

'    ''' <summary>
'    ''' اكتشاف السيرفرات المتاحة على الشبكة
'    ''' </summary>
'    Public Function DiscoverAvailableServers() As List(Of String)
'        Dim servers As New List(Of String)()

'        Try
'            ' إضافة السيرفرات الافتراضية
'            servers.Add(".")
'            servers.Add("(local)")
'            servers.Add("localhost")
'            servers.Add("127.0.0.1")

'            ' يمكنك هنا إضافة كود لاكتشاف السيرفرات الفعلية على الشبكة
'            ' هذا مثال بسيط، يمكنك تحسينه لاحقًا

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"فشل في اكتشاف السيرفرات: {ex.Message}")
'        End Try

'        Return servers
'    End Function

'    Public Sub OpenConnection()
'        Try
'            ' إذا كان المتغير فارغاً أو تم التخلص منه
'            If conn Is Nothing Then
'                conn = New SqlConnection(connectionString)
'            End If

'            If conn.State = ConnectionState.Closed Then
'                conn.Open()
'            End If
'        Catch ex As Exception
'            Throw New Exception("فشل الاتصال بقاعدة البيانات: " & ex.Message)
'        End Try
'    End Sub

'    Public Sub CloseConnection()
'        If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
'            conn.Close()
'        End If
'    End Sub

'    ' دالة تنفيذ استعلام مع معاملات وإرجاع DataTable
'    Public Function GetData(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True) As DataTable
'        Dim dt As New DataTable
'        Try
'            OpenConnection()
'            Using cmd As New SqlCommand(cmdText, conn)
'                If isStoredProc Then
'                    cmd.CommandType = CommandType.StoredProcedure
'                End If

'                If params IsNot Nothing Then
'                    cmd.Parameters.AddRange(params)
'                End If

'                Using da As New SqlDataAdapter(cmd)
'                    da.Fill(dt)
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("فشل في جلب البيانات: " & ex.Message)
'        Finally
'            CloseConnection()
'        End Try
'        Return dt
'    End Function

'    ''' <summary>
'    ''' تنفيذ استعلام SELECT وإرجاع DataTable
'    ''' </summary>
'    ''' <param name="query">الاستعلام المراد تنفيذه</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>DataTable</returns>
'    Public Function ExecuteQuery(query As String, Optional parameters As SqlParameter() = Nothing) As DataTable
'        Dim dataTable As New DataTable()

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(query, connection)
'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    Using adapter As New SqlDataAdapter(command)
'                        adapter.Fill(dataTable)
'                    End Using
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
'        End Try

'        Return dataTable
'    End Function

'    ''' <summary>
'    ''' تنفيذ استعلام INSERT, UPDATE, DELETE
'    ''' </summary>
'    ''' <param name="query">الاستعلام المراد تنفيذه</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>عدد الصفوف المتأثرة</returns>
'    Public Function ExecuteNonQuery(query As String, Optional parameters As SqlParameter() = Nothing) As Integer
'        Dim rowsAffected As Integer = 0

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(query, connection)
'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    rowsAffected = command.ExecuteNonQuery()
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
'        End Try

'        Return rowsAffected
'    End Function

'    ' دالة تنفيذ أمر (غير استعلام) مثل insert, update, delete مع معاملات
'    Public Sub ExecuteNonQuery(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True)
'        Try
'            OpenConnection()
'            Using cmd As New SqlCommand(cmdText, conn)
'                If isStoredProc Then
'                    cmd.CommandType = CommandType.StoredProcedure
'                End If

'                If params IsNot Nothing Then
'                    cmd.Parameters.AddRange(params)
'                End If

'                cmd.ExecuteNonQuery()
'            End Using
'        Catch ex As Exception
'            Throw New Exception("فشل في تنفيذ الأمر: " & ex.Message)
'        Finally
'            CloseConnection()
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تنفيذ إجراء مخزن وإرجاع DataTable
'    ''' </summary>
'    ''' <param name="procedureName">اسم الإجراء المخزن</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>DataTable</returns>
'    Public Function ExecuteStoredProcedure(procedureName As String, Optional parameters As SqlParameter() = Nothing) As DataTable
'        Dim dataTable As New DataTable()

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(procedureName, connection)
'                    command.CommandType = CommandType.StoredProcedure

'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    Using adapter As New SqlDataAdapter(command)
'                        adapter.Fill(dataTable)
'                    End Using
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
'        End Try

'        Return dataTable
'    End Function

'    ' إضافة الدالة الجديدة لتنفيذ الإجراءات المخزنة التي تعيد بيانات
'    Public Function ExecuteStoredProcedureQuery(storedProcedureName As String, ParamArray parameters As SqlParameter()) As DataTable
'        Dim dt As New DataTable()
'        Try
'            Using conn As New SqlConnection(connectionString)
'                conn.Open()
'                Using cmd As New SqlCommand(storedProcedureName, conn)
'                    cmd.CommandType = CommandType.StoredProcedure
'                    If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
'                        cmd.Parameters.AddRange(parameters)
'                    End If
'                    Using adapter As New SqlDataAdapter(cmd)
'                        adapter.Fill(dt)
'                    End Using
'                End Using
'            End Using
'            Return dt
'        Catch ex As Exception
'            Throw New Exception($"خطأ في تنفيذ الإجراء المخزن {storedProcedureName}: {ex.Message}")
'        End Try
'    End Function

'    ''' <summary>
'    ''' تنفيذ إجراء مخزن بدون إرجاع بيانات
'    ''' </summary>
'    ''' <param name="procedureName">اسم الإجراء المخزن</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>عدد الصفوف المتأثرة</returns>
'    Public Function ExecuteStoredProcedureNonQuery(procedureName As String, Optional parameters As SqlParameter() = Nothing) As Integer
'        Dim rowsAffected As Integer = 0

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(procedureName, connection)
'                    command.CommandType = CommandType.StoredProcedure

'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    rowsAffected = command.ExecuteNonQuery()
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
'        End Try

'        Return rowsAffected
'    End Function

'    ''' <summary>
'    ''' تنفيذ استعلام وإرجاع قيمة واحدة
'    ''' </summary>
'    ''' <param name="query">الاستعلام المراد تنفيذه</param>
'    ''' <param name="parameters">المعاملات (اختيارية)</param>
'    ''' <returns>القيمة المرجعة</returns>
'    Public Function ExecuteScalar(query As String, Optional parameters As SqlParameter() = Nothing) As Object
'        Dim result As Object = Nothing

'        Try
'            Using connection As SqlConnection = GetConnection()
'                Using command As New SqlCommand(query, connection)
'                    If parameters IsNot Nothing Then
'                        command.Parameters.AddRange(parameters)
'                    End If

'                    connection.Open()
'                    result = command.ExecuteScalar()
'                End Using
'            End Using
'        Catch ex As Exception
'            Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
'        End Try

'        Return result
'    End Function

'    ' دالة تنفيذ استعلام مع معاملات وإرجاع قيمة وحيدة
'    Public Function ExecuteScalar(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True) As Object
'        Dim result As Object = Nothing
'        Try
'            OpenConnection()
'            Using cmd As New SqlCommand(cmdText, conn)
'                If isStoredProc Then
'                    cmd.CommandType = CommandType.StoredProcedure
'                End If

'                If params IsNot Nothing Then
'                    cmd.Parameters.AddRange(params)
'                End If

'                result = cmd.ExecuteScalar()
'            End Using
'        Catch ex As Exception
'            Throw New Exception("فشل في تنفيذ العملية: " & ex.Message)
'        Finally
'            CloseConnection()
'        End Try
'        Return result
'    End Function

'    ''' <summary>
'    ''' اختبار الاتصال بقاعدة البيانات
'    ''' </summary>
'    ''' <returns>True إذا كان الاتصال ناجح، False إذا فشل</returns>
'    Public Function TestConnection() As Boolean
'        Try
'            Using connection As SqlConnection = GetConnection()
'                connection.Open()
'                Return True
'            End Using
'        Catch ex As Exception
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' إنشاء معامل SQL
'    ''' </summary>
'    ''' <param name="parameterName">اسم المعامل</param>
'    ''' <param name="value">قيمة المعامل</param>
'    ''' <returns>SqlParameter</returns>
'    Public Function CreateParameter(parameterName As String, value As Object) As SqlParameter
'        Return New SqlParameter(parameterName, If(value, DBNull.Value))
'    End Function

'    ''' <summary>
'    ''' إنشاء معامل SQL بنوع وحجم محددين
'    ''' </summary>
'    ''' <param name="parameterName">اسم المعامل</param>
'    ''' <param name="value">قيمة المعامل</param>
'    ''' <param name="dbType">نوع البيانات في قاعدة البيانات</param>
'    ''' <param name="size">حجم البيانات (اختياري)</param>
'    ''' <returns>SqlParameter</returns>
'    Public Function CreateParameter(parameterName As String, value As Object, dbType As SqlDbType, Optional size As Integer = 0) As SqlParameter
'        Dim param As New SqlParameter(parameterName, dbType)
'        If value Is Nothing Then
'            param.Value = DBNull.Value
'        Else
'            param.Value = value
'        End If
'        If size > 0 Then
'            param.Size = size
'        End If
'        Return param
'    End Function

'    ''' <summary>
'    ''' إنشاء مجموعة من المعاملات
'    ''' </summary>
'    ''' <param name="parameterPairs">أزواج اسم المعامل والقيمة</param>
'    ''' <returns>مصفوفة من SqlParameter</returns>
'    Public Function CreateParameters(ParamArray parameterPairs As Object()) As SqlParameter()
'        If parameterPairs.Length Mod 2 <> 0 Then
'            Throw New ArgumentException("عدد المعاملات يجب أن يكون زوجي (اسم، قيمة)")
'        End If

'        Dim parameters(parameterPairs.Length \ 2 - 1) As SqlParameter

'        For i As Integer = 0 To parameterPairs.Length - 1 Step 2
'            parameters(i \ 2) = CreateParameter(parameterPairs(i).ToString(), parameterPairs(i + 1))
'        Next

'        Return parameters
'    End Function

'    ' تعبئة ComboBox
'    Public Sub fillCombo(ByVal cmb As ComboBox, ByVal sql As String, ByVal displayvalue As String)
'        Try
'            Using adp As New SqlDataAdapter(sql, GetConnection)
'                Dim DS As New DataSet
'                adp.Fill(DS)
'                Dim dt = DS.Tables(0)
'                cmb.Items.Clear()

'                For Each row As DataRow In dt.Rows
'                    cmb.Items.Add(row(displayvalue).ToString())
'                Next

'                ' إزالة التكرارات
'                Dim uniqueItems = cmb.Items.Cast(Of String).Distinct().ToList()
'                cmb.Items.Clear()
'                cmb.Items.AddRange(uniqueItems.ToArray())
'            End Using
'        Catch ex As Exception
'            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' دالة عامة للبحث في جدول قاعدة البيانات باستخدام حقل معين
'    ''' </summary>
'    ''' <param name="tableName">اسم الجدول</param>
'    ''' <param name="searchColumn">اسم الحقل المراد البحث فيه</param>
'    ''' <param name="searchTerm">القيمة المراد البحث عنها</param>
'    ''' <param name="connection">الاتصال بقاعدة البيانات</param>
'    ''' <returns>DataTable يحتوي على النتائج</returns>
'    Public Function SearchInDatabase(
'        ByVal tableName As String,
'        ByVal searchColumn As String,
'        ByVal searchTerm As String,
'        ByVal connection As SqlConnection) As DataTable

'        Dim dt As New DataTable()

'        ' التحقق من صحة المدخلات
'        If String.IsNullOrEmpty(tableName) OrElse String.IsNullOrEmpty(searchColumn) Then
'            MessageBox.Show("اسم الجدول أو الحقل غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Return dt
'        End If

'        If String.IsNullOrEmpty(searchTerm) Then
'            Return dt ' إرجاع جدول فارغ إذا كان البحث فارغًا
'        End If

'        Try
'            Using cmd As New SqlCommand()
'                cmd.Connection = connection
'                cmd.CommandText = $"SELECT * FROM {tableName} WHERE {searchColumn} LIKE @SearchTerm"
'                cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm.Trim() & "%")

'                Using da As New SqlDataAdapter(cmd)
'                    da.Fill(dt)
'                End Using
'            End Using
'        Catch ex As SqlException
'            MessageBox.Show("حدث خطأ في قاعدة البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        Catch ex As Exception
'            MessageBox.Show("حدث خطأ عام: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try

'        Return dt
'    End Function
'End Module






'Imports System.Data.SqlClient
'Imports System.Configuration

'Module DatabaseHelper

'' سلسلة الاتصال بقاعدة البيانات
''Private ReadOnly connectionString As String = "Data Source=localhost;Initial Catalog=ESIS;Integrated Security=True"
'Private ReadOnly connectionString As String = "Server=" & My.Settings.ServerName & ";Database=" & My.Settings.DataBaseName & ";Trusted_Connection=True;"
'Private conn As SqlConnection ' تم إزالة التهيئة هنا

'''' <summary>
'''' الحصول على اتصال جديد بقاعدة البيانات
'''' </summary>
'''' <returns>SqlConnection</returns>
'Public Function GetConnection() As SqlConnection
'    Return New SqlConnection(connectionString)
'End Function

'Public Sub OpenConnection()
'    Try
'        ' إذا كان المتغير فارغاً أو تم التخلص منه
'        If conn Is Nothing Then
'            conn = New SqlConnection(connectionString)
'        End If

'        If conn.State = ConnectionState.Closed Then
'            conn.Open()
'        End If
'    Catch ex As Exception
'        Throw New Exception("فشل الاتصال بقاعدة البيانات: " & ex.Message)
'    End Try
'End Sub

'Public Sub CloseConnection()
'    If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
'        conn.Close()
'    End If
'End Sub
'' دالة تنفيذ استعلام مع معاملات وإرجاع DataTable
'Public Function GetData(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True) As DataTable
'    Dim dt As New DataTable
'    Try
'        OpenConnection()
'        Using cmd As New SqlCommand(cmdText, conn)
'            If isStoredProc Then
'                cmd.CommandType = CommandType.StoredProcedure
'            End If

'            If params IsNot Nothing Then
'                cmd.Parameters.AddRange(params)
'            End If

'            Using da As New SqlDataAdapter(cmd)
'                da.Fill(dt)
'            End Using
'        End Using
'    Catch ex As Exception
'        Throw New Exception("فشل في جلب البيانات: " & ex.Message)
'    Finally
'        CloseConnection()
'    End Try
'    Return dt
'End Function



'''' <summary>
'''' تنفيذ استعلام SELECT وإرجاع DataTable
'''' </summary>
'''' <param name="query">الاستعلام المراد تنفيذه</param>
'''' <param name="parameters">المعاملات (اختيارية)</param>
'''' <returns>DataTable</returns>
'Public Function ExecuteQuery(query As String, Optional parameters As SqlParameter() = Nothing) As DataTable
'    Dim dataTable As New DataTable()

'    Try
'        Using connection As SqlConnection = GetConnection()
'            Using command As New SqlCommand(query, connection)
'                If parameters IsNot Nothing Then
'                    command.Parameters.AddRange(parameters)
'                End If

'                connection.Open()
'                Using adapter As New SqlDataAdapter(command)
'                    adapter.Fill(dataTable)
'                End Using
'            End Using
'        End Using
'    Catch ex As Exception
'        Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
'    End Try

'    Return dataTable
'End Function

'''' <summary>
'''' تنفيذ استعلام INSERT, UPDATE, DELETE
'''' </summary>
'''' <param name="query">الاستعلام المراد تنفيذه</param>
'''' <param name="parameters">المعاملات (اختيارية)</param>
'''' <returns>عدد الصفوف المتأثرة</returns>
'Public Function ExecuteNonQuery(query As String, Optional parameters As SqlParameter() = Nothing) As Integer
'    Dim rowsAffected As Integer = 0

'    Try
'        Using connection As SqlConnection = GetConnection()
'            Using command As New SqlCommand(query, connection)
'                If parameters IsNot Nothing Then
'                    command.Parameters.AddRange(parameters)
'                End If

'                connection.Open()
'                rowsAffected = command.ExecuteNonQuery()
'            End Using
'        End Using
'    Catch ex As Exception
'        Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
'    End Try

'    Return rowsAffected
'End Function

'' دالة تنفيذ أمر (غير استعلام) مثل insert, update, delete مع معاملات
'Public Sub ExecuteNonQuery(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True)
'    Try
'        OpenConnection()
'        Using cmd As New SqlCommand(cmdText, conn)
'            If isStoredProc Then
'                cmd.CommandType = CommandType.StoredProcedure
'            End If

'            If params IsNot Nothing Then
'                cmd.Parameters.AddRange(params)
'            End If

'            cmd.ExecuteNonQuery()
'        End Using
'    Catch ex As Exception
'        Throw New Exception("فشل في تنفيذ الأمر: " & ex.Message)
'    Finally
'        CloseConnection()
'    End Try
'End Sub

'''' <summary>
'''' تنفيذ إجراء مخزن وإرجاع DataTable
'''' </summary>
'''' <param name="procedureName">اسم الإجراء المخزن</param>
'''' <param name="parameters">المعاملات (اختيارية)</param>
'''' <returns>DataTable</returns>
'Public Function ExecuteStoredProcedure(procedureName As String, Optional parameters As SqlParameter() = Nothing) As DataTable
'    Dim dataTable As New DataTable()

'    Try
'        Using connection As SqlConnection = GetConnection()
'            Using command As New SqlCommand(procedureName, connection)
'                command.CommandType = CommandType.StoredProcedure

'                If parameters IsNot Nothing Then
'                    command.Parameters.AddRange(parameters)
'                End If

'                connection.Open()
'                Using adapter As New SqlDataAdapter(command)
'                    adapter.Fill(dataTable)
'                End Using
'            End Using
'        End Using
'    Catch ex As Exception
'        Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
'    End Try

'    Return dataTable
'End Function

'' إضافة الدالة الجديدة لتنفيذ الإجراءات المخزنة التي تعيد بيانات
'Public Function ExecuteStoredProcedureQuery(storedProcedureName As String, ParamArray parameters As SqlParameter()) As DataTable
'    Dim dt As New DataTable()
'    Try
'        Using conn As New SqlConnection(connectionString)
'            conn.Open()
'            Using cmd As New SqlCommand(storedProcedureName, conn)
'                cmd.CommandType = CommandType.StoredProcedure
'                If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
'                    cmd.Parameters.AddRange(parameters)
'                End If
'                Using adapter As New SqlDataAdapter(cmd)
'                    adapter.Fill(dt)
'                End Using
'            End Using
'        End Using
'        Return dt
'    Catch ex As Exception
'        Throw New Exception($"خطأ في تنفيذ الإجراء المخزن {storedProcedureName}: {ex.Message}")
'    End Try
'End Function



'''' <summary>
'''' تنفيذ إجراء مخزن بدون إرجاع بيانات
'''' </summary>
'''' <param name="procedureName">اسم الإجراء المخزن</param>
'''' <param name="parameters">المعاملات (اختيارية)</param>
'''' <returns>عدد الصفوف المتأثرة</returns>
'Public Function ExecuteStoredProcedureNonQuery(procedureName As String, Optional parameters As SqlParameter() = Nothing) As Integer
'    Dim rowsAffected As Integer = 0

'    Try
'        Using connection As SqlConnection = GetConnection()
'            Using command As New SqlCommand(procedureName, connection)
'                command.CommandType = CommandType.StoredProcedure

'                If parameters IsNot Nothing Then
'                    command.Parameters.AddRange(parameters)
'                End If

'                connection.Open()
'                rowsAffected = command.ExecuteNonQuery()
'            End Using
'        End Using
'    Catch ex As Exception
'        Throw New Exception("خطأ في تنفيذ الإجراء المخزن: " & ex.Message)
'    End Try

'    Return rowsAffected
'End Function

'''' <summary>
'''' تنفيذ استعلام وإرجاع قيمة واحدة
'''' </summary>
'''' <param name="query">الاستعلام المراد تنفيذه</param>
'''' <param name="parameters">المعاملات (اختيارية)</param>
'''' <returns>القيمة المرجعة</returns>
'Public Function ExecuteScalar(query As String, Optional parameters As SqlParameter() = Nothing) As Object
'    Dim result As Object = Nothing

'    Try
'        Using connection As SqlConnection = GetConnection()
'            Using command As New SqlCommand(query, connection)
'                If parameters IsNot Nothing Then
'                    command.Parameters.AddRange(parameters)
'                End If

'                connection.Open()
'                result = command.ExecuteScalar()
'            End Using
'        End Using
'    Catch ex As Exception
'        Throw New Exception("خطأ في تنفيذ الاستعلام: " & ex.Message)
'    End Try

'    Return result
'End Function

'' دالة تنفيذ استعلام مع معاملات وإرجاع قيمة وحيدة
'Public Function ExecuteScalar(cmdText As String, params As SqlParameter(), Optional isStoredProc As Boolean = True) As Object
'    Dim result As Object = Nothing
'    Try
'        OpenConnection()
'        Using cmd As New SqlCommand(cmdText, conn)
'            If isStoredProc Then
'                cmd.CommandType = CommandType.StoredProcedure
'            End If

'            If params IsNot Nothing Then
'                cmd.Parameters.AddRange(params)
'            End If

'            result = cmd.ExecuteScalar()
'        End Using
'    Catch ex As Exception
'        Throw New Exception("فشل في تنفيذ العملية: " & ex.Message)
'    Finally
'        CloseConnection()
'    End Try
'    Return result
'End Function


'''' <summary>
'''' اختبار الاتصال بقاعدة البيانات
'''' </summary>
'''' <returns>True إذا كان الاتصال ناجح، False إذا فشل</returns>
'Public Function TestConnection() As Boolean
'    Try
'        Using connection As SqlConnection = GetConnection()
'            connection.Open()
'            Return True
'        End Using
'    Catch ex As Exception
'        Return False
'    End Try
'End Function

'''' <summary>
'''' إنشاء معامل SQL
'''' </summary>
'''' <param name="parameterName">اسم المعامل</param>
'''' <param name="value">قيمة المعامل</param>
'''' <returns>SqlParameter</returns>
'Public Function CreateParameter(parameterName As String, value As Object) As SqlParameter
'    Return New SqlParameter(parameterName, If(value, DBNull.Value))
'End Function

'''' <summary>
'''' إنشاء معامل SQL بنوع وحجم محددين
'''' </summary>
'''' <param name="parameterName">اسم المعامل</param>
'''' <param name="value">قيمة المعامل</param>
'''' <param name="dbType">نوع البيانات في قاعدة البيانات</param>
'''' <param name="size">حجم البيانات (اختياري)</param>
'''' <returns>SqlParameter</returns>
'Public Function CreateParameter(parameterName As String, value As Object, dbType As SqlDbType, Optional size As Integer = 0) As SqlParameter
'    Dim param As New SqlParameter(parameterName, dbType)
'    If value Is Nothing Then
'        param.Value = DBNull.Value
'    Else
'        param.Value = value
'    End If
'    If size > 0 Then
'        param.Size = size
'    End If
'    Return param
'End Function

'''' <summary>
'''' إنشاء مجموعة من المعاملات
'''' </summary>
'''' <param name="parameterPairs">أزواج اسم المعامل والقيمة</param>
'''' <returns>مصفوفة من SqlParameter</returns>
'Public Function CreateParameters(ParamArray parameterPairs As Object()) As SqlParameter()
'    If parameterPairs.Length Mod 2 <> 0 Then
'        Throw New ArgumentException("عدد المعاملات يجب أن يكون زوجي (اسم، قيمة)")
'    End If

'    Dim parameters(parameterPairs.Length \ 2 - 1) As SqlParameter

'    For i As Integer = 0 To parameterPairs.Length - 1 Step 2
'        parameters(i \ 2) = CreateParameter(parameterPairs(i).ToString(), parameterPairs(i + 1))
'    Next

'    Return parameters
'End Function

'' تعبئة ComboBox
'Public Sub fillCombo(ByVal cmb As ComboBox, ByVal sql As String, ByVal displayvalue As String)
'    Try
'        Using adp As New SqlDataAdapter(sql, GetConnection)
'            Dim DS As New DataSet
'            adp.Fill(DS)
'            Dim dt = DS.Tables(0)
'            cmb.Items.Clear()

'            For Each row As DataRow In dt.Rows
'                cmb.Items.Add(row(displayvalue).ToString())
'            Next

'            ' إزالة التكرارات
'            Dim uniqueItems = cmb.Items.Cast(Of String).Distinct().ToList()
'            cmb.Items.Clear()
'            cmb.Items.AddRange(uniqueItems.ToArray())
'        End Using
'    Catch ex As Exception
'        MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    End Try
'End Sub

'''' <summary>
'''' دالة عامة للبحث في جدول قاعدة البيانات باستخدام حقل معين
'''' </summary>
'''' <param name="tableName">اسم الجدول</param>
'''' <param name="searchColumn">اسم الحقل المراد البحث فيه</param>
'''' <param name="searchTerm">القيمة المراد البحث عنها</param>
'''' <param name="connection">الاتصال بقاعدة البيانات</param>
'''' <returns>DataTable يحتوي على النتائج</returns>
'Public Function SearchInDatabase(
'    ByVal tableName As String,
'    ByVal searchColumn As String,
'    ByVal searchTerm As String,
'    ByVal connection As SqlConnection) As DataTable

'    Dim dt As New DataTable()

'    ' التحقق من صحة المدخلات
'    If String.IsNullOrEmpty(tableName) OrElse String.IsNullOrEmpty(searchColumn) Then
'        MessageBox.Show("اسم الجدول أو الحقل غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        Return dt
'    End If

'    If String.IsNullOrEmpty(searchTerm) Then
'        Return dt ' إرجاع جدول فارغ إذا كان البحث فارغًا
'    End If

'    Try
'        Using cmd As New SqlCommand()
'            cmd.Connection = connection
'            cmd.CommandText = $"SELECT * FROM {tableName} WHERE {searchColumn} LIKE @SearchTerm"
'            cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm.Trim() & "%")

'            Using da As New SqlDataAdapter(cmd)
'                da.Fill(dt)
'            End Using
'        End Using
'    Catch ex As SqlException
'        MessageBox.Show("حدث خطأ في قاعدة البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    Catch ex As Exception
'        MessageBox.Show("حدث خطأ عام: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    End Try

'    Return dt
'End Function



'End Module

