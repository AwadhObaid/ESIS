Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Public Class ControlScaler
    ' تخزين الأحجام الافتراضية للعناصر
    Private DefaultControlSizes As New Dictionary(Of Control, CtrlInfo)
    Private FormDefaultClientSize As Size

    ' تهيئة الكلاس مع النموذج المراد تعديل عناصره
    Public Sub New(ByVal form As Form)
        ' تعيين الحالة الافتراضية للنموذج
        form.WindowState = FormWindowState.Normal
        form.MinimumSize = form.Size
        FormDefaultClientSize = form.ClientSize

        ' جمع معلومات العناصر داخل النموذج
        Dim ctrl As Control = form.GetNextControl(form, True)
        While ctrl IsNot Nothing
            If TypeOf ctrl Is ListBox Then DirectCast(ctrl, ListBox).IntegralHeight = False
            DefaultControlSizes.Add(ctrl, New CtrlInfo(ctrl.Bounds, ctrl.Font.Size))
            ctrl = form.GetNextControl(ctrl, True)
        End While
    End Sub

    ' تعديل أحجام العناصر بناءً على حجم النموذج الحالي
    Public Sub ScaleControls(ByVal form As Form)
        If form.WindowState <> FormWindowState.Minimized Then
            For Each kvp As KeyValuePair(Of Control, CtrlInfo) In DefaultControlSizes
                Dim ctrl As Control = kvp.Key

                ' حساب نسبة التحجيم
                Dim Xscl As Double = form.ClientSize.Width / FormDefaultClientSize.Width
                Dim Yscl As Double = form.ClientSize.Height / FormDefaultClientSize.Height

                '' تعديل حجم الخط
                'Dim fntscl As Single = CSng(kvp.Value.cFontSize * Yscl)
                'ctrl.Font = New Font(ctrl.Font.FontFamily, fntscl, ctrl.Font.Style, ctrl.Font.Unit)

                ' تعديل أبعاد وموقع العنصر
                ctrl.Width = CInt(kvp.Value.cBounds.Width * Xscl)
                ctrl.Height = CInt(kvp.Value.cBounds.Height * Yscl)
                ctrl.Left = CInt(kvp.Value.cBounds.X * Xscl)
                ctrl.Top = CInt(kvp.Value.cBounds.Y * Yscl)
            Next
        End If
    End Sub

    ' كلاس داخلي لتخزين معلومات العنصر
    Public Class CtrlInfo
        Public cBounds As Rectangle
        Public cFontSize As Single

        Public Sub New(ByVal Bnds As Rectangle, ByVal FntSize As Single)
            cBounds = Bnds
            cFontSize = FntSize
        End Sub
    End Class
End Class