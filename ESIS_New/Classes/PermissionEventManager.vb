''' <summary>
''' مدير أحداث الصلاحيات
''' </summary>
Public Class PermissionEventManager

    ' تعريف نوع الحدث بشكل صحيح
    Public Delegate Sub PermissionsChangedEventHandler(sender As Object, e As EventArgs)

    ' تعريف الحدث باستخدام الديليجات المعرف
    Public Shared Event PermissionsChanged As PermissionsChangedEventHandler

    ''' <summary>
    ''' رفع حدث تغيير الصلاحيات
    ''' </summary>
    Public Shared Sub RaisePermissionsChanged()
        RaiseEvent PermissionsChanged(Nothing, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' تسجيل حدث تغيير الصلاحيات
    ''' </summary>
    Public Shared Sub RegisterPermissionsChangedHandler(handler As PermissionsChangedEventHandler)
        AddHandler PermissionsChanged, handler
    End Sub

    ''' <summary>
    ''' إلغاء تسجيل حدث تغيير الصلاحيات
    ''' </summary>
    Public Shared Sub UnregisterPermissionsChangedHandler(handler As PermissionsChangedEventHandler)
        RemoveHandler PermissionsChanged, handler
    End Sub
End Class



'Imports System.Collections.Generic

'''' <summary>
'''' مدير الأحداث للصلاحيات - يدير تحديث الصلاحيات ديناميكياً في جميع أنحاء التطبيق
'''' </summary>
'Public Class PermissionEventManager

'    ' Singleton Pattern لضمان وجود نسخة واحدة فقط
'    Private Shared _instance As PermissionEventManager
'    Private Shared ReadOnly _lock As New Object()

'    ' قائمة المستمعين للأحداث
'    Private _permissionChangedHandlers As New List(Of Action)()

'    ''' <summary>
'    ''' الحصول على النسخة الوحيدة من المدير
'    ''' </summary>
'    Public Shared ReadOnly Property Instance As PermissionEventManager
'        Get
'            If _instance Is Nothing Then
'                SyncLock _lock
'                    If _instance Is Nothing Then
'                        _instance = New PermissionEventManager()
'                    End If
'                End SyncLock
'            End If
'            Return _instance
'        End Get
'    End Property

'    ' منع إنشاء نسخ جديدة من الخارج
'    Private Sub New()
'    End Sub

'    ''' <summary>
'    ''' تسجيل مستمع لأحداث تغيير الصلاحيات
'    ''' </summary>
'    ''' <param name="handler">الدالة التي ستُستدعى عند تغيير الصلاحيات</param>
'    Public Sub RegisterPermissionChangedHandler(handler As Action)
'        SyncLock _lock
'            If Not _permissionChangedHandlers.Contains(handler) Then
'                _permissionChangedHandlers.Add(handler)
'            End If
'        End SyncLock
'    End Sub

'    ''' <summary>
'    ''' إلغاء تسجيل مستمع لأحداث تغيير الصلاحيات
'    ''' </summary>
'    ''' <param name="handler">الدالة المراد إلغاء تسجيلها</param>
'    Public Sub UnregisterPermissionChangedHandler(handler As Action)
'        SyncLock _lock
'            _permissionChangedHandlers.Remove(handler)
'        End SyncLock
'    End Sub

'    ''' <summary>
'    ''' إشعار جميع المستمعين بتغيير الصلاحيات
'    ''' </summary>
'    Public Sub NotifyPermissionsChanged()
'        SyncLock _lock
'            ' إنشاء نسخة من القائمة لتجنب مشاكل التزامن
'            Dim handlers As New List(Of Action)(_permissionChangedHandlers)

'            ' تنفيذ جميع المعالجات في Thread منفصل لتجنب تعليق الواجهة
'            Threading.Tasks.Task.Run(Sub()
'                                         For Each handler In handlers
'                                             Try
'                                                 ' تنفيذ المعالج في UI Thread
'                                                 If Application.OpenForms.Count > 0 Then
'                                                     Dim mainForm As Form = Application.OpenForms(0)
'                                                     If mainForm.InvokeRequired Then
'                                                         mainForm.Invoke(handler)
'                                                     Else
'                                                         handler.Invoke()
'                                                     End If
'                                                 End If
'                                             Catch ex As Exception
'                                                 ' تسجيل الخطأ دون إيقاف باقي المعالجات
'                                                 System.Diagnostics.Debug.WriteLine($"خطأ في تنفيذ معالج تغيير الصلاحيات: {ex.Message}")
'                                             End Try
'                                         Next
'                                     End Sub)
'        End SyncLock
'    End Sub

'    ''' <summary>
'    ''' تنظيف جميع المستمعين (يُستخدم عند إغلاق التطبيق)
'    ''' </summary>
'    Public Sub ClearAllHandlers()
'        SyncLock _lock
'            _permissionChangedHandlers.Clear()
'        End SyncLock
'    End Sub

'    ''' <summary>
'    ''' الحصول على عدد المستمعين المسجلين
'    ''' </summary>
'    Public ReadOnly Property RegisteredHandlersCount As Integer
'        Get
'            SyncLock _lock
'                Return _permissionChangedHandlers.Count
'            End SyncLock
'        End Get
'    End Property


'    ''' <summary>
'    ''' إشعار جميع النماذج بتحديث صلاحيات مستخدم معين
'    ''' </summary>
'    Public Sub NotifyPermissionsUpdated(userID As Integer)
'        Try
'            ' إذا كان المستخدم الحالي هو من تم تحديث صلاحياته
'            If userID = GlobalData.LoggedInUserID Then
'                ' تحديث الصلاحيات الحالية للمستخدم
'                PermissionHelper.RefreshCurrentUserPermissions()

'                ' إشعار جميع النماذج المسجلة
'                For Each handler In _permissionChangedHandlers
'                    Try
'                        handler.Invoke()
'                    Catch ex As Exception
'                        System.Diagnostics.Debug.WriteLine($"خطأ في إشعار النموذج: {ex.Message}")
'                    End Try
'                Next
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في إشعار تحديث الصلاحيات: {ex.Message}")
'        End Try
'    End Sub

'End Class

