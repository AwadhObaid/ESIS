Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms

Public Class DataHelper
    ' --- عدادات الأرشيف ---
    Private Shared _archiveCount As Integer = -1
    Public Shared ReadOnly Property ArchiveCount As Integer
        Get
            If _archiveCount = -1 Then UpdateArchiveCount()
            Return _archiveCount
        End Get
    End Property

    ' --- عدادات الصادر ---
    Private Shared _saderCount As Integer = -1
    Private Shared _deletedArchiveCount As Integer = -1
    Private Shared _unarchivedSaderCount As Integer = -1

    Public Shared ReadOnly Property SaderCount As Integer
        Get
            If _saderCount = -1 Then UpdateSaderCount()
            Return _saderCount
        End Get
    End Property

    ' --- عدادات السجلات المحذوفة ---
    Public Shared ReadOnly Property DeletedArchiveCount As Integer
        Get
            If _deletedArchiveCount = -1 Then UpdateDeletedArchiveCount()
            Return _deletedArchiveCount
        End Get
    End Property

    ' --- عدادات السجلات غير المؤرشفة (الإشعارات) ---
    Public Shared ReadOnly Property UnarchivedSaderCount As Integer
        Get
            If _unarchivedSaderCount = -1 Then UpdateUnarchivedSaderCount()
            Return _unarchivedSaderCount
        End Get
    End Property

    ' --- الأحداث ---
    Public Shared Event ArchiveCountChanged As EventHandler(Of DataCountChangedEventArgs)
    Public Shared Event SaderCountChanged As EventHandler(Of DataCountChangedEventArgs)
    Public Shared Event DeletedArchiveCountChanged As EventHandler(Of DataCountChangedEventArgs)
    Public Shared Event UnarchivedSaderCountChanged As EventHandler(Of DataCountChangedEventArgs)

    ' --- مؤقت المراقبة ---
    Private Shared unarchivedTimer As New System.Windows.Forms.Timer()

    ' تعريف الحدث
    Public Shared Event FileSelectedForArchive(filePath As String, fileName As String, fileExtension As String)

    ' دالة لرفع الحدث
    Public Shared Sub RaiseFileSelectedForArchive(filePath As String, fileName As String, fileExtension As String)
        RaiseEvent FileSelectedForArchive(filePath, fileName, fileExtension)
    End Sub

    ''' <summary>
    ''' تحديث عداد الأرشيف
    ''' </summary>
    Public Shared Sub UpdateArchiveCount()
        Try
            ' استخدام DatabaseHelper لجلب العدد من قاعدة البيانات
            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE IsDeleted = 0"
            Dim newCount As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))

            ' حفظ القيمة القديمة قبل التحديث
            Dim oldCount As Integer = _archiveCount

            ' تحديث العداد
            _archiveCount = newCount

            ' رفع حدث التغيير مع تمرير جميع المعاملات المطلوبة
            Dim args As New DataCountChangedEventArgs(oldCount, newCount, "Archive")
            RaiseEvent ArchiveCountChanged(Nothing, args)
        Catch ex As Exception
            ' عرض رسالة خطأ للمستخدم
            MessageBox.Show($"خطأ في تحديث عداد الأرشيف: {ex.Message}",
                           "خطأ قاعدة بيانات",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error)

            ' تسجيل الخطأ في نافذة التصحيح
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الأرشيف: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' دالة للحصول على العدد الحالي
    ''' </summary>
    Public Shared Function GetCurrentArchiveCount() As Integer
        If _archiveCount = -1 Then
            UpdateArchiveCount()
        End If
        Return _archiveCount
    End Function

    ''' <summary>
    ''' تحديث عداد الصادر
    ''' </summary>
    Public Shared Sub UpdateSaderCount()
        Try
            ' استخدام DatabaseHelper لجلب العدد من قاعدة البيانات
            Dim query As String = "SELECT COUNT(*) FROM TbSader"
            Dim newCount As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))

            ' حفظ القيمة القديمة قبل التحديث
            Dim oldCount As Integer = _saderCount

            ' تحديث العداد
            _saderCount = newCount

            ' رفع حدث التغيير مع تمرير جميع المعاملات المطلوبة
            Dim args As New DataCountChangedEventArgs(oldCount, newCount, "Sader")
            RaiseEvent SaderCountChanged(Nothing, args)
        Catch ex As Exception
            ' عرض رسالة خطأ للمستخدم
            MessageBox.Show($"خطأ في تحديث عداد الصادر: {ex.Message}",
                           "خطأ قاعدة بيانات",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error)

            ' تسجيل الخطأ في نافذة التصحيح
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الصادر: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' دالة للحصول على العدد الحالي
    ''' </summary>
    Public Shared Function GetCurrentSaderCount() As Integer
        If _saderCount = -1 Then
            UpdateSaderCount()
        End If
        Return _saderCount
    End Function

    ''' <summary>
    ''' تحديث عداد الأرشيفات المحذوفة
    ''' </summary>
    Public Shared Sub UpdateDeletedArchiveCount()
        Try
            ' استخدام DatabaseHelper لجلب العدد من قاعدة البيانات
            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE IsDeleted = 1"
            Dim newCount As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))

            ' حفظ القيمة القديمة قبل التحديث
            Dim oldCount As Integer = _deletedArchiveCount

            ' تحديث العداد
            _deletedArchiveCount = newCount

            ' رفع حدث التغيير مع تمرير جميع المعاملات المطلوبة
            Dim args As New DataCountChangedEventArgs(oldCount, newCount, "DeletedArchive")
            RaiseEvent DeletedArchiveCountChanged(Nothing, args)
        Catch ex As Exception
            ' عرض رسالة خطأ للمستخدم
            MessageBox.Show($"خطأ في تحديث عداد الأرشيفات المحذوفة: {ex.Message}",
                           "خطأ قاعدة بيانات",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error)

            ' تسجيل الخطأ في نافذة التصحيح
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الأرشيفات المحذوفة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' دالة للحصول على العدد الحالي للأرشيفات المحذوفة
    ''' </summary>
    Public Shared Function GetCurrentDeletedArchiveCount() As Integer
        If _deletedArchiveCount = -1 Then
            UpdateDeletedArchiveCount()
        End If
        Return _deletedArchiveCount
    End Function

    ''' <summary>
    ''' دالة لحساب عدد السجلات غير المرتبطة بالأرشيف
    ''' </summary>
    Public Shared Function GetUnarchivedSaderCount() As Integer
        Try
            Dim query As String = "SELECT COUNT(*) FROM TbSader WHERE ArchiveID IS NULL OR ArchiveID = 0"
            Return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))
        Catch ex As Exception
            MessageBox.Show("خطأ في جلب عدد السجلات غير المرتبطة بالأرشيف: " & ex.Message,
                           "خطأ قاعدة بيانات",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error)
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' دالة لتحديث العدد غير المرتبط
    ''' </summary>
    Public Shared Sub UpdateUnarchivedSaderCount()
        Try
            Dim newCount As Integer = GetUnarchivedSaderCount()
            Dim oldCount As Integer = GetCurrentUnarchivedSaderCount()

            ' إذا تغير العدد، أطلق الحدث
            If newCount <> oldCount Then
                _unarchivedSaderCount = newCount
                Dim args As New DataCountChangedEventArgs(oldCount, newCount, "UnarchivedSader")
                RaiseEvent UnarchivedSaderCountChanged(Nothing, args)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد السجلات غير المؤرشفة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' دالة لاسترجاع العدد الحالي (مع التخزين المؤقت)
    ''' </summary>
    Public Shared Function GetCurrentUnarchivedSaderCount() As Integer
        If _unarchivedSaderCount = -1 Then
            _unarchivedSaderCount = GetUnarchivedSaderCount()
        End If
        Return _unarchivedSaderCount
    End Function

    ''' <summary>
    ''' بدء مراقبة السجلات غير المؤرشفة بشكل دوري
    ''' </summary>
    Public Shared Sub StartMonitoringUnarchivedRecords(intervalInSeconds As Integer)
        ' إيقاف المؤقت إذا كان يعمل بالفعل
        StopMonitoringUnarchivedRecords()

        ' تعيين الفاصل الزمني بالمللي ثانية
        unarchivedTimer.Interval = intervalInSeconds * 1000

        ' إعداد معالج الحدث
        AddHandler unarchivedTimer.Tick, AddressOf UnarchivedTimer_Tick

        ' بدء المؤقت
        unarchivedTimer.Start()

        ' تحديث العدد فورًا
        UpdateUnarchivedSaderCount()
    End Sub

    ''' <summary>
    ''' معالج حدث المؤقت للتحديث الدوري
    ''' </summary>
    Private Shared Sub UnarchivedTimer_Tick(sender As Object, e As EventArgs)
        UpdateUnarchivedSaderCount()
    End Sub

    ''' <summary>
    ''' إيقاف مراقبة السجلات غير المؤرشفة
    ''' </summary>
    Public Shared Sub StopMonitoringUnarchivedRecords()
        unarchivedTimer.Stop()
        RemoveHandler unarchivedTimer.Tick, AddressOf UnarchivedTimer_Tick
    End Sub

    ''' <summary>
    ''' فئة متداخلة لتوفير بيانات لحدث تغيير العداد
    ''' </summary>
    Public Class DataCountChangedEventArgs
        Inherits EventArgs

        Public Property OldCount As Integer
        Public Property NewCount As Integer
        Public Property TableName As String

        Public Sub New(oldCount As Integer, newCount As Integer, tableName As String)
            Me.OldCount = oldCount
            Me.NewCount = newCount
            Me.TableName = tableName
        End Sub
    End Class

End Class

'''' <summary>
'''' فئة لتوفير بيانات لحدث تغيير العداد
'''' </summary>
'Public Class DataCountChangedEventArgs
'    Inherits EventArgs

'    ''' <summary>
'    ''' العدد القديم
'    ''' </summary>
'    Public Property OldCount As Integer

'    ''' <summary>
'    ''' العدد الجديد
'    ''' </summary>
'    Public Property NewCount As Integer

'    ''' <summary>
'    ''' اسم الجدول الذي تم تحديث عدده
'    ''' </summary>
'    Public Property TableName As String

'    ''' <summary>
'    ''' المُنشئ
'    ''' </summary>
'    Public Sub New(oldCount As Integer, newCount As Integer, tableName As String)
'        Me.OldCount = oldCount
'        Me.NewCount = newCount
'        Me.TableName = tableName
'    End Sub
'End Class




'Imports System.Data.SqlClient

'Public Class DataHelper
'    ' الحدث الذي سيطلق عند تغيير العدد في جدول الصادرة
'    Public Shared Event SaderCountChanged As EventHandler(Of DataCountChangedEventArgs)

'    ' الحدث الذي سيطلق عند تغيير العدد في جدول الأرشيف
'    Public Shared Event ArchiveCountChanged As EventHandler(Of DataCountChangedEventArgs)

'    ' حدث لتغيير العدد
'    Public Shared Event DeletedArchiveCountChanged As EventHandler(Of DataCountChangedEventArgs)

'    ' المتغيرات المشتركة لتخزين العدد الحالي
'    Private Shared _saderCount As Integer = -1
'    Private Shared _archiveCount As Integer = -1
'    Private Shared _deletedArchiveCount As Integer = -1

'    ' تعريف الحدث
'    Public Shared Event FileSelectedForArchive(filePath As String, fileName As String, fileExtension As String)

'    ' دالة لرفع الحدث
'    Public Shared Sub RaiseFileSelectedForArchive(filePath As String, fileName As String, fileExtension As String)
'        RaiseEvent FileSelectedForArchive(filePath, fileName, fileExtension)
'    End Sub

'    ' الخاصية للوصول إلى العدد
'    Public Shared ReadOnly Property DeletedArchiveCount As Integer
'        Get
'            If _deletedArchiveCount = -1 Then
'                UpdateDeletedArchiveCount()
'            End If
'            Return _deletedArchiveCount
'        End Get
'    End Property

'    ' دالة تحديث العدد
'    Public Shared Sub UpdateDeletedArchiveCount()
'        Try
'            ' استخدام DatabaseHelper لجلب العدد من قاعدة البيانات
'            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE IsDeleted = 1"
'            Dim newCount As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))

'            ' حفظ القيمة القديمة قبل التحديث
'            Dim oldCount As Integer = _deletedArchiveCount

'            ' تحديث العداد
'            _deletedArchiveCount = newCount

'            ' رفع حدث التغيير مع تمرير جميع المعاملات المطلوبة
'            Dim args As New DataCountChangedEventArgs(oldCount, newCount, "DeletedArchive")
'            RaiseEvent DeletedArchiveCountChanged(Nothing, args)
'        Catch ex As Exception
'            ' عرض رسالة خطأ للمستخدم
'            MessageBox.Show($"خطأ في تحديث عداد الأرشيفات المحذوفة: {ex.Message}",
'                       "خطأ قاعدة بيانات",
'                       MessageBoxButtons.OK,
'                       MessageBoxIcon.Error)

'            ' تسجيل الخطأ في نافذة التصحيح
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الأرشيفات المحذوفة: {ex.Message}")
'        End Try
'    End Sub



'    ' دالة للحصول على العدد الحالي
'    Public Shared Function GetCurrentDeletedArchiveCount() As Integer
'        If _deletedArchiveCount = -1 Then
'            UpdateDeletedArchiveCount()
'        End If
'        Return _deletedArchiveCount
'    End Function

'    ' دالة لاسترجاع العدد الحالي للصادرة (مع التخزين المؤقت)
'    Public Shared Function GetCurrentSaderCount() As Integer
'        If _saderCount = -1 Then
'            _saderCount = GetActualCountFromDatabase("TbSader")
'        End If
'        Return _saderCount
'    End Function

'    ' دالة لاسترجاع العدد الحالي للأرشيف (مع التخزين المؤقت)
'    Public Shared Function GetCurrentArchiveCount() As Integer
'        If _archiveCount = -1 Then
'            _archiveCount = GetActualCountFromDatabase("TbArchive")
'        End If
'        Return _archiveCount
'    End Function

'    ' دالة لجلب العدد الفعلي من قاعدة البيانات
'    Private Shared Function GetActualCountFromDatabase(tableName As String) As Integer
'        Try
'            ' التحقق من وجود عمود IsDeleted في الجدول
'            Dim hasIsDeletedColumn As Boolean = CheckIfTableHasColumn(tableName, "IsDeleted")

'            ' بناء الاستعلام بناءً على وجود العمود
'            Dim query As String
'            If hasIsDeletedColumn Then
'                query = $"SELECT COUNT(*) FROM {tableName} WHERE IsDeleted = 0"
'            Else
'                query = $"SELECT COUNT(*) FROM {tableName}"
'            End If

'            Return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))
'        Catch ex As Exception
'            MessageBox.Show($"خطأ في جلب عدد السجلات من {tableName}: " & ex.Message,
'                       "خطأ قاعدة بيانات",
'                       MessageBoxButtons.OK,
'                       MessageBoxIcon.Error)
'            Return -1
'        End Try
'    End Function

'    Private Shared Function CheckIfTableHasColumn(tableName As String, columnName As String) As Boolean
'        Try
'            Dim query As String = "SELECT COUNT(*) " &
'                              "FROM INFORMATION_SCHEMA.COLUMNS " &
'                              "WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName"

'            Using connection As SqlConnection = DatabaseHelper.GetConnection
'                connection.Open()
'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@TableName", tableName)
'                    command.Parameters.AddWithValue("@ColumnName", columnName)

'                    Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
'                    Return count > 0
'                End Using
'            End Using
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من وجود العمود: {ex.Message}")
'            ' في حالة حدوث خطأ، نفترض أن العمود غير موجود
'            Return False
'        End Try
'    End Function


'    '' دالة لجلب العدد الفعلي من قاعدة البيانات
'    'Private Shared Function GetActualCountFromDatabase(tableName As String) As Integer
'    '    Try
'    '        Dim query As String = $"SELECT COUNT(*) FROM {tableName}"
'    '        Return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))
'    '    Catch ex As Exception
'    '        MessageBox.Show($"خطأ في جلب عدد السجلات من {tableName}: " & ex.Message,
'    '                       "خطأ قاعدة بيانات",
'    '                       MessageBoxButtons.OK,
'    '                       MessageBoxIcon.Error)
'    '        Return -1
'    '    End Try
'    'End Function

'    ' دالة لتحديث العدد للصادرة يدويًا (تستخدم بعد العمليات)
'    'Public Shared Sub UpdateSaderCount()
'    '    Dim newCount As Integer = GetActualCountFromDatabase("TbSader")

'    '    ' إذا تغير العدد، أطلق الحدث
'    '    If newCount <> _saderCount Then
'    '        Dim oldCount As Integer = _saderCount
'    '        _saderCount = newCount

'    '        ' إطلاق الحدث مع إرسال القيم القديمة والجديدة
'    '        RaiseEvent SaderCountChanged(Nothing,
'    '            New DataCountChangedEventArgs(oldCount, newCount, "Sader"))
'    '    End If
'    'End Sub

'    Public Shared Sub UpdateSaderCount()
'        Try
'            ' استخدام DatabaseHelper لجلب العدد من قاعدة البيانات (بدون شرط IsDeleted)
'            Dim query As String = "SELECT COUNT(*) FROM TbSader"
'            Dim newCount As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))

'            ' حفظ القيمة القديمة قبل التحديث
'            Dim oldCount As Integer = _saderCount

'            ' تحديث العداد
'            _saderCount = newCount

'            ' رفع حدث التغيير مع تمرير جميع المعاملات المطلوبة
'            Dim args As New DataCountChangedEventArgs(oldCount, newCount, "Sader")
'            RaiseEvent SaderCountChanged(Nothing, args)
'        Catch ex As Exception
'            ' عرض رسالة خطأ للمستخدم
'            MessageBox.Show($"خطأ في تحديث عداد الصادر: {ex.Message}",
'                       "خطأ قاعدة بيانات",
'                       MessageBoxButtons.OK,
'                       MessageBoxIcon.Error)

'            ' تسجيل الخطأ في نافذة التصحيح
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الصادر: {ex.Message}")
'        End Try
'    End Sub

'    Public Shared Sub UpdateArchiveCount()
'        Try
'            ' استخدام DatabaseHelper لجلب العدد من قاعدة البيانات
'            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE IsDeleted = 0"
'            Dim newCount As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))

'            ' حفظ القيمة القديمة قبل التحديث
'            Dim oldCount As Integer = _archiveCount

'            ' تحديث العداد
'            _archiveCount = newCount

'            ' رفع حدث التغيير مع تمرير جميع المعاملات المطلوبة
'            Dim args As New DataCountChangedEventArgs(oldCount, newCount, "Archive")
'            RaiseEvent ArchiveCountChanged(Nothing, args)
'        Catch ex As Exception
'            ' عرض رسالة خطأ للمستخدم
'            MessageBox.Show($"خطأ في تحديث عداد الأرشيف: {ex.Message}",
'                       "خطأ قاعدة بيانات",
'                       MessageBoxButtons.OK,
'                       MessageBoxIcon.Error)

'            ' تسجيل الخطأ في نافذة التصحيح
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الأرشيف: {ex.Message}")
'        End Try
'    End Sub


'    '' دالة لتحديث العدد للأرشيف يدويًا (تستخدم بعد العمليات)
'    'Public Shared Sub UpdateArchiveCount()
'    '    Dim newCount As Integer = GetActualCountFromDatabase("TbArchive")

'    '    ' إذا تغير العدد، أطلق الحدث
'    '    If newCount <> _archiveCount Then
'    '        Dim oldCount As Integer = _archiveCount
'    '        _archiveCount = newCount

'    '        ' إطلاق الحدث مع إرسال القيم القديمة والجديدة
'    '        RaiseEvent ArchiveCountChanged(Nothing,
'    '            New DataCountChangedEventArgs(oldCount, newCount, "Archive"))
'    '    End If
'    'End Sub

'    ' دالة لحساب عدد السجلات غير المرتبطة بالأرشيف
'    Public Shared Function GetUnarchivedSaderCount() As Integer
'        Try
'            Dim query As String = "SELECT COUNT(*) FROM TbSader WHERE ArchiveID IS NULL OR ArchiveID = 0"
'            Return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))
'        Catch ex As Exception
'            MessageBox.Show("خطأ في جلب عدد السجلات غير المرتبطة بالأرشيف: " & ex.Message,
'                           "خطأ قاعدة بيانات",
'                           MessageBoxButtons.OK,
'                           MessageBoxIcon.Error)
'            Return -1
'        End Try
'    End Function

'    ' دالة لتحديث العدد غير المرتبط
'    Public Shared Sub UpdateUnarchivedSaderCount()
'        Dim newCount As Integer = GetUnarchivedSaderCount()
'        Dim oldCount As Integer = GetCurrentUnarchivedSaderCount()

'        ' إذا تغير العدد، أطلق الحدث
'        If newCount <> oldCount Then
'            _unarchivedSaderCount = newCount
'            RaiseEvent UnarchivedSaderCountChanged(Nothing,
'                New DataCountChangedEventArgs(oldCount, newCount, "UnarchivedSader"))
'        End If
'    End Sub

'    ' المتغير المخزن لعدد السجلات غير المرتبطة
'    Private Shared _unarchivedSaderCount As Integer = -1

'    ' دالة لاسترجاع العدد الحالي (مع التخزين المؤقت)
'    Public Shared Function GetCurrentUnarchivedSaderCount() As Integer
'        If _unarchivedSaderCount = -1 Then
'            _unarchivedSaderCount = GetUnarchivedSaderCount()
'        End If
'        Return _unarchivedSaderCount
'    End Function

'    ' الحدث الذي سيطلق عند تغيير العدد غير المرتبط
'    Public Shared Event UnarchivedSaderCountChanged As EventHandler(Of DataCountChangedEventArgs)

'    ''' <summary>
'    ''' فئة لتوفير بيانات لحدث تغيير العداد
'    ''' </summary>
'    Public Class DataCountChangedEventArgs
'        Inherits EventArgs

'        ''' <summary>
'        ''' العدد القديم
'        ''' </summary>
'        Public Property OldCount As Integer

'        ''' <summary>
'        ''' العدد الجديد
'        ''' </summary>
'        Public Property NewCount As Integer

'        ''' <summary>
'        ''' اسم الجدول الذي تم تحديث عدده
'        ''' </summary>
'        Public Property TableName As String

'        ''' <summary>
'        ''' المُنشئ
'        ''' </summary>
'        Public Sub New(oldCount As Integer, newCount As Integer, tableName As String)
'            Me.OldCount = oldCount
'            Me.NewCount = newCount
'            Me.TableName = tableName
'        End Sub
'    End Class



'    '' فئة لEventArgs المخصصة
'    'Public Class DataCountChangedEventArgs
'    '    Inherits EventArgs

'    '    Public Property OldCount As Integer
'    '    Public Property NewCount As Integer
'    '    Public Property TableName As String

'    'Public Sub New(oldCount As Integer, newCount As Integer, tableName As String)
'    '        Me.OldCount = oldCount
'    '        Me.NewCount = newCount
'    '        Me.TableName = tableName
'    '    End Sub
'    'End Class
'End Class



'Public Class DataHelper
'    ' الحدث الذي سيطلق عند تغيير العدد
'    Public Shared Event SaderCountChanged As EventHandler(Of SaderCountChangedEventArgs)

'    ' المتغير المشترك لتخزين العدد الحالي
'    Private Shared _currentCount As Integer = -1

'    ' دالة لاسترجاع العدد الحالي (مع التخزين المؤقت)
'    Public Shared Function GetCurrentCount() As Integer
'        If _currentCount = -1 Then
'            _currentCount = GetActualCountFromDatabase()
'        End If
'        Return _currentCount
'    End Function

'    ' دالة لجلب العدد الفعلي من قاعدة البيانات
'    Private Shared Function GetActualCountFromDatabase() As Integer
'        Try
'            Dim query As String = "SELECT COUNT(*) FROM TbSader"
'            Return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query))
'        Catch ex As Exception
'            MessageBox.Show("خطأ في جلب عدد السجلات: " & ex.Message,
'                           "خطأ قاعدة بيانات",
'                           MessageBoxButtons.OK,
'                           MessageBoxIcon.Error)
'            Return -1
'        End Try
'    End Function

'    ' دالة لتحديث العدد يدويًا (تستخدم بعد العمليات)
'    Public Shared Sub UpdateCount()
'        Dim newCount As Integer = GetActualCountFromDatabase()

'        ' إذا تغير العدد، أطلق الحدث
'        If newCount <> _currentCount Then
'            Dim oldCount As Integer = _currentCount
'            _currentCount = newCount

'            ' إطلاق الحدث مع إرسال القيم القديمة والجديدة
'            RaiseEvent SaderCountChanged(Nothing,
'                New SaderCountChangedEventArgs(oldCount, newCount))
'        End If
'    End Sub

'    ' فئة لEventArgs المخصصة
'    Public Class SaderCountChangedEventArgs
'        Inherits EventArgs

'        Public Property OldCount As Integer
'        Public Property NewCount As Integer

'        Public Sub New(oldCount As Integer, newCount As Integer)
'            Me.OldCount = oldCount
'            Me.NewCount = newCount
'        End Sub
'    End Class
'End Class