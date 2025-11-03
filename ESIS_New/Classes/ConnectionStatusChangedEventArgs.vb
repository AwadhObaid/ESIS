Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Threading
Imports System.Threading.Tasks

''' <summary>
''' نوع الاتصال
''' </summary>
Public Enum ConnectionType
    LocalNetwork
    Internet
End Enum

''' <summary>
''' فئة لأحداث تغيير حالة الاتصال
''' </summary>
Public Class ConnectionStatusChangedEventArgs
    Inherits EventArgs

    Public Property IsConnected As Boolean
    Public Property ConnectionType As ConnectionType
    Public Property CheckTime As DateTime

    Public Sub New(isConnected As Boolean, connectionType As ConnectionType)
        Me.IsConnected = isConnected
        Me.ConnectionType = connectionType
        Me.CheckTime = DateTime.Now
    End Sub
End Class

''' <summary>
''' كلاس للتحقق من اتصال الإنترنت
''' </summary>
Public Class InternetConnection
    Private _lastCheckTime As DateTime = DateTime.MinValue
    Private _isConnected As Boolean = False
    Private _lastConnectionStatus As Boolean = False
    Private _connectionStatusChanged As Boolean = False
    Private _checkInterval As Integer = 5000 ' 5 ثوانٍ
    Private _isMonitoring As Boolean = False
    Private _monitoringThread As Thread

    ''' <summary>
    ''' حدث يتم إطلاقه عند تغيير حالة الاتصال بالإنترنت
    ''' </summary>
    Public Event ConnectionStatusChanged(sender As Object, e As ConnectionStatusChangedEventArgs)

    ''' <summary>
    ''' التحقق من اتصال الإنترنت باستخدام طريقة متعددة المراحل
    ''' </summary>
    ''' <returns>True إذا كان الجهاز متصلًا بالإنترنت</returns>
    Public Function IsConnectedToInternet() As Boolean
        Try
            ' المرحلة 1: التحقق من الاتصال باستخدام DNS
            If Not IsDnsReachable() Then
                Return False
            End If

            ' المرحلة 2: التحقق من الاتصال بخادم ويب معروف
            If Not IsWebServerReachable() Then
                Return False
            End If

            ' المرحلة 3: التحقق من الوصول إلى عنوان URL معين (اختياري)
            If Not IsUrlReachable("https://www.google.com") Then
                Return False
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من إمكانية الوصول إلى خوادم DNS
    ''' </summary>
    Private Function IsDnsReachable() As Boolean
        Try
            ' محاولة حل عنوان IP لخادم معروف
            Dim hostEntry As IPHostEntry = Dns.GetHostEntry("www.google.com")
            Return hostEntry.AddressList.Length > 0
        Catch ex As SocketException
            ' خطأ في DNS أو الشبكة
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من إمكانية الوصول إلى خادم ويب
    ''' </summary>
    Private Function IsWebServerReachable() As Boolean
        Try
            ' استخدام Ping لخادم معروف
            Using ping As New Ping()
                Dim reply As PingReply = ping.Send("8.8.8.8", 3000) ' Google DNS
                Return reply.Status = IPStatus.Success
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من إمكانية الوصول إلى عنوان URL معين
    ''' </summary>
    ''' <param name="url">العنوان المراد التحقق منه</param>
    Private Function IsUrlReachable(url As String) As Boolean
        Try
            ' إنشاء طلب HTTP
            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
            request.Method = "HEAD"
            request.Timeout = 5000 ' 5 ثوانٍ
            request.AllowAutoRedirect = True

            Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Return response.StatusCode = HttpStatusCode.OK
            End Using
        Catch ex As WebException
            ' التحقق من نوع الخطأ
            If ex.Status = WebExceptionStatus.Timeout OrElse
               ex.Status = WebExceptionStatus.NameResolutionFailure OrElse
               ex.Status = WebExceptionStatus.ConnectFailure Then
                Return False
            Else
                ' قد يكون الخادم يعمل ولكن يرفض الطلب
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من سرعة الاتصال بالإنترنت
    ''' </summary>
    ''' <returns>السرعة التقريبية بالبايت/الثانية</returns>
    Public Function GetInternetSpeed() As Long
        Try
            ' قياس الوقت اللازم لتحميل ملف صغير
            Dim startTime As DateTime = DateTime.Now
            Dim request As HttpWebRequest = DirectCast(WebRequest.Create("https://httpbin.org/bytes/1024"), HttpWebRequest)
            request.Timeout = 10000

            Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Using stream As Stream = response.GetResponseStream()
                    Dim buffer(1023) As Byte
                    stream.Read(buffer, 0, 1024)
                End Using
            End Using

            Dim endTime As DateTime = DateTime.Now
            Dim duration As Double = (endTime - startTime).TotalSeconds

            If duration > 0 Then
                Return CLng(1024 / duration) ' بايت/ثانية
            Else
                Return 0
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' بدء مراقبة حالة الاتصال بالإنترنت
    ''' </summary>
    Public Sub StartMonitoring()
        If _isMonitoring Then
            Return
        End If

        _isMonitoring = True
        _monitoringThread = New Thread(AddressOf MonitoringLoop)
        _monitoringThread.IsBackground = True
        _monitoringThread.Start()
    End Sub

    ''' <summary>
    ''' إيقاف مراقبة حالة الاتصال
    ''' </summary>
    Public Sub StopMonitoring()
        _isMonitoring = False
        If _monitoringThread IsNot Nothing AndAlso _monitoringThread.IsAlive Then
            _monitoringThread.Join(1000) ' انتظر لمدة 1 ثانية كحد أقصى
        End If
    End Sub

    ''' <summary>
    ''' حلقة المراقبة
    ''' </summary>
    Private Sub MonitoringLoop()
        Try
            While _isMonitoring
                CheckConnectionStatus()
                Thread.Sleep(_checkInterval)
            End While
        Catch ex As ThreadAbortException
            ' تم إنهاء الخيط بشكل طبيعي
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في مراقبة الاتصال: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من حالة الاتصال وتحديث الحدث إذا تغيرت الحالة
    ''' </summary>
    Public Sub CheckConnectionStatus()
        Dim isConnected As Boolean = IsConnectedToInternet()

        ' تحديث حالة الاتصال فقط إذا تغيرت
        If _lastConnectionStatus <> isConnected Then
            _lastConnectionStatus = isConnected
            _connectionStatusChanged = True

            ' إطلاق حدث تغيير الحالة
            RaiseEvent ConnectionStatusChanged(Me, New ConnectionStatusChangedEventArgs(isConnected, ConnectionType.Internet))
        End If

        _lastCheckTime = DateTime.Now
    End Sub

    ''' <summary>
    ''' الحصول على وقت آخر تحقق من الحالة
    ''' </summary>
    Public ReadOnly Property LastCheckTime As DateTime
        Get
            Return _lastCheckTime
        End Get
    End Property

    ''' <summary>
    ''' الحصول على حالة الاتصال الحالية
    ''' </summary>
    Public ReadOnly Property IsConnected As Boolean
        Get
            Return _lastConnectionStatus
        End Get
    End Property

    ''' <summary>
    ''' الحصول على فاصل الوقت بين عمليات التحقق
    ''' </summary>
    Public Property CheckInterval As Integer
        Get
            Return _checkInterval
        End Get
        Set(value As Integer)
            If value > 0 Then
                _checkInterval = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' التحقق مما إذا كانت المراقبة نشطة
    ''' </summary>
    Public ReadOnly Property IsMonitoring As Boolean
        Get
            Return _isMonitoring
        End Get
    End Property

    ''' <summary>
    ''' التحقق من إمكانية الوصول إلى خادم قاعدة البيانات
    ''' </summary>
    ''' <param name="serverAddress">عنوان الخادم</param>
    ''' <param name="port">منفذ قاعدة البيانات (عادة 1433 لـ SQL Server)</param>
    Public Function IsDatabaseServerReachable(serverAddress As String, Optional port As Integer = 1433) As Boolean
        Try
            Using client As New TcpClient()
                Dim result As IAsyncResult = client.BeginConnect(serverAddress, port, Nothing, Nothing)
                Dim success As Boolean = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5))

                If success Then
                    client.EndConnect(result)
                    Return client.Connected
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
