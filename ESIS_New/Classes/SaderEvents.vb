Public Class SaderEvents
    ' حدث يتم تشغيله عند تحديث بيانات الصادرة
    Public Shared Event SaderDataChanged As EventHandler(Of SaderDataChangedEventArgs)

    ' دالة لتشغيل الحدث
    Public Shared Sub RaiseSaderDataChanged(actionType As SaderDataAction, affectedCount As Integer, sourceForm As Form)
        RaiseEvent SaderDataChanged(Nothing, New SaderDataChangedEventArgs(actionType, affectedCount, sourceForm))
    End Sub
End Class

' كلاس لنقل معلومات الحدث
Public Class SaderDataChangedEventArgs
    Inherits EventArgs

    Public Property ActionType As SaderDataAction
    Public Property AffectedCount As Integer
    Public Property SourceForm As Form
    Public Property TotalCount As Integer

    Public Sub New(actionType As SaderDataAction, affectedCount As Integer, sourceForm As Form)
        Me.ActionType = actionType
        Me.AffectedCount = affectedCount
        Me.SourceForm = sourceForm
    End Sub




End Class

' أنواع إجراءات البيانات
Public Enum SaderDataAction
    Added
    Updated
    Deleted
    Refreshed
End Enum