Imports System.Data.SqlClient
Imports System.IO

' افتراض: وجود كلاس لإدارة الاتصال بقاعدة البيانات (مثل GlobalData.ConnectionString)

Public Class ArchiveClass_ForScanner

    ' *****************************************************************
    ' * دالة لإضافة سجل أرشيف جديد وربطه برقم الصادر
    ' * (هذه دالة وهمية Mock Function، يجب استبدالها بالدالة الحقيقية)
    ' *****************************************************************
    ''' <summary>
    ''' تضيف سجل أرشيف جديد إلى قاعدة البيانات وتربطه بسجل الصادر.
    ''' </summary>
    ''' <param name="filePath">المسار النهائي للملف المحفوظ.</param>
    ''' <param name="fileName">اسم الملف.</param>
    ''' <param name="fileType">نوع الملف (مثل JPG).</param>
    ''' <param name="saderID">معرف سجل الصادر المراد ربط الأرشيف به.</param>
    ''' <returns>معرف الأرشيف الجديد (ArchiveID) أو -1 في حالة الفشل.</returns>
    Public Shared Function AddArchive(ByVal filePath As String, ByVal fileName As String, ByVal fileType As String, ByVal saderID As Integer) As Integer
        ' في النظام الحقيقي، سيتم استخدام اتصال قاعدة البيانات
        ' وتنفيذ إجراء مخزن لإضافة السجل والحصول على الـ ID الجديد.

        Try
            ' *****************************************************************
            ' * 1. حفظ الملف على القرص (تم بالفعل في ScannerForm، لكن نتحقق)
            ' *****************************************************************
            If Not File.Exists(filePath) Then
                ' في التنفيذ الحقيقي، يجب أن يكون الملف قد تم نقله بالفعل
                ' أو حفظه في المسار النهائي قبل استدعاء هذه الدالة.
                Throw New FileNotFoundException($"الملف غير موجود في المسار: {filePath}")
            End If

            ' *****************************************************************
            ' * 2. إضافة السجل إلى قاعدة البيانات (افتراض)
            ' *****************************************************************

            ' هنا يتم تنفيذ SQL Command أو Stored Procedure
            ' مثال على SQL (افتراضي):
            ' INSERT INTO ArchiveTable (FilePath, FileName, FileType, SaderID, ArchiveDate)
            ' VALUES (@FilePath, @FileName, @FileType, @SaderID, GETDATE());
            ' SELECT SCOPE_IDENTITY();

            ' لغرض التكامل والاختبار، سنفترض أن العملية نجحت
            ' ونعيد معرف أرشيف وهمي (Mock Archive ID)
            Dim newArchiveID As Integer = New Random().Next(1000, 9999)

            ' *****************************************************************
            ' * 3. تحديث سجل الصادر برقم الأرشيف الجديد (افتراض)
            ' *****************************************************************
            ' يجب أن تكون هناك دالة في SaderClass لتحديث حقل ArchiveID في جدول الصادر
            ' SaderClass.UpdateArchiveID(saderID, newArchiveID)

            ' بما أننا لا نملك SaderClass، سنفترض أن التحديث تم بنجاح.

            Return newArchiveID

        Catch ex As Exception
            Console.WriteLine($"خطأ في إضافة سجل الأرشيف: {ex.Message}")
            Return -1 ' فشل العملية
        End Try
    End Function

End Class
