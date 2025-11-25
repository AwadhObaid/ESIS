Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Windows.Forms
Imports WIA
Imports TwainLib

' يجب إضافة مرجع لـ "Microsoft Windows Image Acquisition Library v2.0"
' أو استخدام WIA 1.0 (Interop.WIA.dll)

Module ScannerHelper

    ' تعريف ثابت لـ WIA
    Private Const WIA_SOURCE_FEEDER As Integer = 1
    Private Const WIA_SOURCE_FLATBED As Integer = 2
    Private Const WIA_SOURCE_AUTO As Integer = 4

    ' خصائص WIA
    Private Const WIA_PROPERTY_DPI_X As Integer = 6147
    Private Const WIA_PROPERTY_DPI_Y As Integer = 6148
    Private Const WIA_PROPERTY_COLOR_MODE As Integer = 6146
    Private Const WIA_PROPERTY_BRIGHTNESS As Integer = 6150
    Private Const WIA_PROPERTY_CONTRAST As Integer = 6151

    ' قيم وضع الألوان
    Private Const WIA_COLOR_GRAYSCALE As Integer = 2
    Private Const WIA_COLOR_COLOR As Integer = 4
    Private Const WIA_COLOR_BLACK_WHITE As Integer = 1

    ' كائن WIA DeviceManager
    Private ReadOnly DeviceManager As New WIA.DeviceManager()

    '''' <summary>
    '''' جلب قائمة الماسحات الضوئية المتوفرة
    '''' </summary>
    'Public Function GetAvailableScanners() As List(Of String)

    '    Dim FileName = TwainLib.TwainOperations.ScanImages(".jpg", True)
    '    For Each Item In FileName
    '        TwainLib.TwainOperations.GetScanSource(Item).ToString()
    '    Next
    '    'Try
    '    '    For Each deviceInfo As DeviceInfo In DeviceManager.DeviceInfos
    '    '        If deviceInfo.Type = WiaDeviceType.ScannerDeviceType Then
    '    '            scanners.Add(deviceInfo.Properties.Item("Name").Value.ToString())
    '    '        End If
    '    '    Next
    '    'Catch ex As Exception
    '    '    MessageBox.Show($"خطأ في جلب الماسحات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    'End Try
    '    Return Item
    'End Function




    ''' <summary>
    ''' جلب قائمة الماسحات الضوئية المتوفرة في النظام (بدون TwainLib)
    ''' </summary>


    Public Function GetAvailableScanners() As List(Of String)
        Dim scanners As New List(Of String)
        Dim dm As New DeviceManager()

        For Each info As DeviceInfo In dm.DeviceInfos
            If info.Type = WiaDeviceType.ScannerDeviceType Then
                scanners.Add(info.Properties("Name").Value.ToString())
            End If
        Next

        Return scanners
    End Function

    Public Function ScanWithSpecificScanner(scannerName As String) As List(Of String)
        Return TwainLib.TwainOperations.ScanImages(".jpg", True, scannerName)
    End Function




    ''' <summary>
    ''' مسح ضوئي وحفظ الصورة في ملف
    ''' </summary>
    ''' <param name="scannerName">اسم الماسح الضوئي المختار</param>
    ''' <param name="filePath">المسار الكامل لحفظ الملف</param>
    ''' <param name="dpi">دقة المسح الضوئي (DPI)</param>
    ''' <param name="colorMode">وضع الألوان (Color, Grayscale, BlackAndWhite)</param>
    ''' <param name="source">مصدر المسح (Flatbed, Feeder)</param>
    Public Function ScanAndSave(scannerName As String, filePath As String, dpi As Integer, colorMode As String, source As String) As Boolean
        Dim device As WIA.Device = Nothing
        Dim item As WIA.Item = Nothing

        Try
            ' 1. اختيار الماسح الضوئي
            Dim deviceId As String = ""
            For Each deviceInfo As WIA.DeviceInfo In DeviceManager.DeviceInfos
                If deviceInfo.Type = WIA.WiaDeviceType.ScannerDeviceType AndAlso deviceInfo.Properties.Item("Name").Value.ToString() = scannerName Then
                    deviceId = deviceInfo.DeviceID
                    Exit For
                End If
            Next

            If String.IsNullOrEmpty(deviceId) Then
                MessageBox.Show("لم يتم العثور على الماسح الضوئي المحدد.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            device = DeviceManager.Connect(deviceId)

            ' 2. اختيار مصدر المسح
            Dim wiaSource As Integer = WIA_SOURCE_FLATBED
            If source.ToLower() = "feeder" Then
                wiaSource = WIA_SOURCE_FEEDER
            End If

            ' 3. إعدادات المسح الضوئي
            item = device.Items(1) ' عادةً ما يكون العنصر الأول هو المسح الضوئي

            ' تعيين مصدر المسح
            SetWiaProperty(item.Properties, "Document Handling Select", wiaSource)

            ' تعيين الدقة
            SetWiaProperty(item.Properties, "Horizontal Resolution", dpi)
            SetWiaProperty(item.Properties, "Vertical Resolution", dpi)

            ' تعيين وضع الألوان
            Dim wiaColorMode As Integer = WIA_COLOR_COLOR
            Select Case colorMode.ToLower()
                Case "grayscale"
                    wiaColorMode = WIA_COLOR_GRAYSCALE
                Case "blackandwhite"
                    wiaColorMode = WIA_COLOR_BLACK_WHITE
            End Select
            SetWiaProperty(item.Properties, "Current Intent", wiaColorMode)

            ' 4. بدء المسح الضوئي
            Dim imageFile As WIA.ImageFile = item.Transfer(WIA.FormatID.wiaFormatJPEG) ' يمكن تغييرها إلى TIFF أو PNG

            ' 5. حفظ الملف
            imageFile.SaveFile(filePath)

            Return True

        Catch ex As Exception
            MessageBox.Show($"خطأ أثناء المسح الضوئي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            ' تحرير الموارد
            If item IsNot Nothing Then Marshal.ReleaseComObject(item)
            If device IsNot Nothing Then Marshal.ReleaseComObject(device)
        End Try
    End Function

    ''' <summary>
    ''' دالة مساعدة لتعيين خصائص WIA
    ''' </summary>
    Private Sub SetWiaProperty(ByVal properties As WIA.Properties, ByVal propertyName As String, ByVal propertyValue As Object)
        Try
            For Each prop As WIA.Property In properties
                If prop.Name = propertyName Then
                    prop.Value = propertyValue
                    Exit Sub
                End If
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين خاصية WIA: {propertyName} - {ex.Message}")
        End Try
    End Sub

End Module
