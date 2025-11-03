Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Net

''' <summary>
''' كلاس للتحقق من اتصال الشبكة المحلية
''' </summary>
Public Class NetworkConnection
    Private _lastCheckTime As DateTime = DateTime.MinValue
    Private _isConnected As Boolean = False
    Private _lastConnectionStatus As Boolean = False
    Private _connectionStatusChanged As Boolean = False

    ''' <summary>
    ''' حدث يتم إطلاقه عند تغيير حالة الاتصال بالشبكة
    ''' </summary>
    Public Event ConnectionStatusChanged(sender As Object, e As ConnectionStatusChangedEventArgs)

    ''' <summary>
    ''' التحقق من اتصال الشبكة المحلية
    ''' </summary>
    ''' <returns>True إذا كان الجهاز متصلًا بالشبكة المحلية</returns>
    Public Function IsConnectedToLocalNetwork() As Boolean
        Try
            ' التحقق من حالة الاتصال باستخدام NetworkInterface
            Dim interfaces As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
            For Each adapter As NetworkInterface In interfaces
                ' تجاهل المحولات غير الفعلية أو المعطلة
                If adapter.OperationalStatus = OperationalStatus.Up AndAlso
                   adapter.NetworkInterfaceType <> NetworkInterfaceType.Loopback AndAlso
                   adapter.NetworkInterfaceType <> NetworkInterfaceType.Tunnel Then

                    ' التحقق من وجود عنوان IP
                    Dim properties As IPInterfaceProperties = adapter.GetIPProperties()
                    If properties.UnicastAddresses.Count > 0 Then
                        ' التحقق من أن هناك عنوان IP صالح (ليس 169.254.x.x)
                        For Each unicast As UnicastIPAddressInformation In properties.UnicastAddresses
                            Dim ip As IPAddress = unicast.Address
                            If ip.AddressFamily = AddressFamily.InterNetwork Then
                                Dim ipBytes As Byte() = ip.GetAddressBytes()
                                ' تجنب عناوين APIPA (169.254.x.x)
                                If Not (ipBytes(0) = 169 AndAlso ipBytes(1) = 254) Then
                                    Return True
                                End If
                            End If
                        Next
                    End If
                End If
            Next

            Return False
        Catch ex As Exception
            ' في حالة حدوث خطأ، نعتبر أن الجهاز غير متصل
            Return False
        End Try
    End Function

    ''' <summary>
    ''' الحصول على عنوان IP الخاص بالجهاز
    ''' </summary>
    ''' <returns>عنوان IP أو فارغ إذا لم يكن متصلاً</returns>
    Public Function GetLocalIPAddress() As String
        Try
            Dim host As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
            For Each ip As IPAddress In host.AddressList
                If ip.AddressFamily = AddressFamily.InterNetwork Then
                    ' تجنب عناوين APIPA
                    Dim ipBytes As Byte() = ip.GetAddressBytes()
                    If Not (ipBytes(0) = 169 AndAlso ipBytes(1) = 254) Then
                        Return ip.ToString()
                    End If
                End If
            Next
            Return String.Empty
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' الحصول على اسم المضيف (HostName)
    ''' </summary>
    ''' <returns>اسم المضيف</returns>
    Public Function GetHostName() As String
        Try
            Return Dns.GetHostName()
        Catch ex As Exception
            Return "Unknown"
        End Try
    End Function

    ''' <summary>
    ''' التحقق من اتصال جهاز معين في الشبكة المحلية
    ''' </summary>
    ''' <param name="ipAddress">عنوان IP للجهاز</param>
    ''' <param name="timeout">الوقت المستغرق للانتظار (بالميلي ثانية)</param>
    ''' <returns>True إذا كان الجهاز متصلًا</returns>
    Public Function IsHostReachable(ipAddress As String, Optional timeout As Integer = 1000) As Boolean
        Try
            Using ping As New Ping()
                Dim reply As PingReply = ping.Send(ipAddress, timeout)
                Return reply.Status = IPStatus.Success
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من اتصال خادم معين في الشبكة المحلية
    ''' </summary>
    ''' <param name="serverName">اسم الخادم</param>
    ''' <param name="timeout">الوقت المستغرق للانتظار (بالميلي ثانية)</param>
    ''' <returns>True إذا كان الخادم متصلًا</returns>
    Public Function IsServerReachable(serverName As String, Optional timeout As Integer = 1000) As Boolean
        Try
            Dim ipAddresses As IPAddress() = Dns.GetHostAddresses(serverName)
            For Each ip As IPAddress In ipAddresses
                If ip.AddressFamily = AddressFamily.InterNetwork Then
                    Return IsHostReachable(ip.ToString(), timeout)
                End If
            Next
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' الحصول على قائمة بجميع الأجهزة المتصلة في الشبكة
    ''' </summary>
    ''' <returns>قائمة بعناوين IP للأجهزة المتصلة</returns>
    Public Function GetConnectedDevices() As List(Of String)
        Dim devices As New List(Of String)
        Try
            ' الحصول على عنوان الشبكة الخاص بالجهاز
            Dim localIP As String = GetLocalIPAddress()
            If String.IsNullOrEmpty(localIP) Then
                Return devices
            End If

            ' تحديد نطاق الشبكة (بافتراض /24)
            Dim ipParts() As String = localIP.Split("."c)
            If ipParts.Length = 4 Then
                Dim networkPrefix As String = $"{ipParts(0)}.{ipParts(1)}.{ipParts(2)}."

                ' التحقق من الأجهزة في النطاق (1-254)
                Parallel.For(1, 255, Sub(i)
                                         Dim ipAddress As String = $"{networkPrefix}{i}"
                                         ' تجنب عنوان الجهاز نفسه
                                         If ipAddress <> localIP AndAlso IsHostReachable(ipAddress, 100) Then
                                             devices.Add(ipAddress)
                                         End If
                                     End Sub)
            End If
        Catch ex As Exception
            ' تجاهل الأخطاء وعودة القائمة الحالية
        End Try
        Return devices
    End Function

    ''' <summary>
    ''' التحقق من حالة الاتصال وتحديث الحدث إذا تغيرت الحالة
    ''' </summary>
    Public Sub CheckConnectionStatus()
        Dim isConnected As Boolean = IsConnectedToLocalNetwork()

        ' تحديث حالة الاتصال فقط إذا تغيرت
        If _lastConnectionStatus <> isConnected Then
            _lastConnectionStatus = isConnected
            _connectionStatusChanged = True

            ' إطلاق حدث تغيير الحالة
            RaiseEvent ConnectionStatusChanged(Me, New ConnectionStatusChangedEventArgs(isConnected, ConnectionType.LocalNetwork))
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
End Class
