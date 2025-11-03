''' <summary>
''' كلاس مركزي لمراقبة اتصال الشبكة والإنترنت
''' </summary>
Public Class NetworkMonitor
    Private Shared _instance As NetworkMonitor
    Private _localNetwork As NetworkConnection
    Private _internetConnection As InternetConnection
    Private _isInitialized As Boolean = False

    ''' <summary>
    ''' الحصول على مثيل وحيد من NetworkMonitor
    ''' </summary>
    Public Shared ReadOnly Property Instance As NetworkMonitor
        Get
            If _instance Is Nothing Then
                _instance = New NetworkMonitor()
            End If
            Return _instance
        End Get
    End Property

    ''' <summary>
    ''' الباني الخاص
    ''' </summary>
    Private Sub New()
        _localNetwork = New NetworkConnection()
        _internetConnection = New InternetConnection()
    End Sub

    ''' <summary>
    ''' تهيئة المراقب
    ''' </summary>
    Public Sub Initialize()
        If _isInitialized Then
            Return
        End If

        ' الاشتراك في أحداث تغيير الحالة
        AddHandler _localNetwork.ConnectionStatusChanged, AddressOf OnLocalNetworkStatusChanged
        AddHandler _internetConnection.ConnectionStatusChanged, AddressOf OnInternetStatusChanged

        ' بدء المراقبة
        _internetConnection.StartMonitoring()

        _isInitialized = True
    End Sub

    ''' <summary>
    ''' التحقق من حالة الاتصال بالشبكة المحلية
    ''' </summary>
    Public ReadOnly Property IsConnectedToLocalNetwork As Boolean
        Get
            Return _localNetwork.IsConnected
        End Get
    End Property

    ''' <summary>
    ''' التحقق من حالة الاتصال بالإنترنت
    ''' </summary>
    Public ReadOnly Property IsConnectedToInternet As Boolean
        Get
            Return _internetConnection.IsConnected
        End Get
    End Property

    ''' <summary>
    ''' التحقق من حالة الاتصال بالخادم
    ''' </summary>
    Public ReadOnly Property IsServerReachable As Boolean
        Get
            ' التحقق من اتصال الشبكة المحلية والإنترنت
            Return _localNetwork.IsConnected AndAlso _internetConnection.IsConnected
        End Get
    End Property

    ''' <summary>
    ''' الحصول على عنوان IP المحلي
    ''' </summary>
    Public ReadOnly Property LocalIPAddress As String
        Get
            Return _localNetwork.GetLocalIPAddress()
        End Get
    End Property

    ''' <summary>
    ''' الحصول على اسم المضيف
    ''' </summary>
    Public ReadOnly Property HostName As String
        Get
            Return _localNetwork.GetHostName()
        End Get
    End Property

    ''' <summary>
    ''' التحقق من اتصال خادم معين
    ''' </summary>
    Public Function IsServerReachableByName(serverName As String) As Boolean
        Return _localNetwork.IsServerReachable(serverName) AndAlso
           _internetConnection.IsDatabaseServerReachable(serverName)
    End Function

    ''' <summary>
    ''' الحصول على سرعة الإنترنت
    ''' </summary>
    Public ReadOnly Property InternetSpeed As Long
        Get
            Return _internetConnection.GetInternetSpeed()
        End Get
    End Property

    ''' <summary>
    ''' حدث يتم إطلاقه عند تغيير حالة الاتصال بالشبكة المحلية
    ''' </summary>
    Public Event LocalNetworkStatusChanged(sender As Object, e As ConnectionStatusChangedEventArgs)

    ''' <summary>
    ''' حدث يتم إطلاقه عند تغيير حالة الاتصال بالإنترنت
    ''' </summary>
    Public Event InternetStatusChanged(sender As Object, e As ConnectionStatusChangedEventArgs)

    ''' <summary>
    ''' معالجة تغيير حالة الاتصال بالشبكة المحلية
    ''' </summary>
    Private Sub OnLocalNetworkStatusChanged(sender As Object, e As ConnectionStatusChangedEventArgs)
        RaiseEvent LocalNetworkStatusChanged(sender, e)
    End Sub

    ''' <summary>
    ''' معالجة تغيير حالة الاتصال بالإنترنت
    ''' </summary>
    Private Sub OnInternetStatusChanged(sender As Object, e As ConnectionStatusChangedEventArgs)
        RaiseEvent InternetStatusChanged(sender, e)
    End Sub

    ''' <summary>
    ''' التحقق من جميع حالات الاتصال
    ''' </summary>
    Public Sub CheckAllConnections()
        _localNetwork.CheckConnectionStatus()
        _internetConnection.CheckConnectionStatus()
    End Sub

    ''' <summary>
    ''' التحقق من حالة الاتصال بالخادم
    ''' </summary>
    Public Function CheckServerConnection(serverAddress As String, Optional databasePort As Integer = 1433) As Boolean
        Return _localNetwork.IsServerReachable(serverAddress) AndAlso
               _internetConnection.IsDatabaseServerReachable(serverAddress, databasePort)
    End Function

    ''' <summary>
    ''' الحصول على معلومات الاتصال الكاملة
    ''' </summary>
    Public Function GetConnectionInfo() As ConnectionInfo
        Return New ConnectionInfo With {
            .IsConnectedToLocalNetwork = _localNetwork.IsConnected,
            .IsConnectedToInternet = _internetConnection.IsConnected,
            .LocalIPAddress = _localNetwork.GetLocalIPAddress(),
            .HostName = _localNetwork.GetHostName(),
            .InternetSpeed = _internetConnection.GetInternetSpeed(),
            .LastLocalCheck = _localNetwork.LastCheckTime,
            .LastInternetCheck = _internetConnection.LastCheckTime
        }
    End Function

    ''' <summary>
    ''' التوقف عن المراقبة
    ''' </summary>
    Public Sub Dispose()
        If _isInitialized Then
            _internetConnection.StopMonitoring()

            RemoveHandler _localNetwork.ConnectionStatusChanged, AddressOf OnLocalNetworkStatusChanged
            RemoveHandler _internetConnection.ConnectionStatusChanged, AddressOf OnInternetStatusChanged

            _isInitialized = False
        End If
    End Sub
End Class

''' <summary>
''' فئة لاحتواء معلومات الاتصال
''' </summary>
Public Class ConnectionInfo
    Public Property IsConnectedToLocalNetwork As Boolean
    Public Property IsConnectedToInternet As Boolean
    Public Property LocalIPAddress As String
    Public Property HostName As String
    Public Property InternetSpeed As Long
    Public Property LastLocalCheck As DateTime
    Public Property LastInternetCheck As DateTime
End Class