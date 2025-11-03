Imports System.Security
Imports MicroVisionSerial.Encryption.Activation
Imports Security.FingerPrint
Public Class ActivationForm

    Private Async Sub btnActivate_Click(sender As Object, e As EventArgs) Handles btnActivate.Click
        ' تعطيل الزر لمنع النقر المتكرر
        btnActivate.Enabled = False
        btnActivate.Text = "جاري التفعيل..."

        Try
            ' التحقق من أن الحقول ليست فارغة
            If String.IsNullOrWhiteSpace(txtSerial.Text) OrElse String.IsNullOrWhiteSpace(txtActivation.Text) Then
                MessageBox.Show("يرجى إدخال الرقم التسلسلي ومفتاح التفعيل.", "معلومة ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من صحة مفتاح التفعيل
            If Not CheckActivationKey(txtSerial.Text.Trim(), txtActivation.Text.Trim()) Then
                MessageBox.Show("مفتاح التفعيل غير صحيح. يرجى التأكد من إدخال القيم بشكل دقيق.", "مفتاح خاطئ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtActivation.SelectAll()
                txtActivation.Focus()
                Return
            End If

            ' محاولة كتابة مفتاح التفعيل في التسجيل
            If WriteValue(txtActivation.Text.Trim()) Then
                MessageBox.Show("تم تفعيل البرنامج بنجاح! سيتم إعادة تشغيل التطبيق الآن.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' استخدام Task.Delay قبل إعادة التشغيل للسماح بعرض الرسالة
                Await Task.Delay(500)
                Application.Restart()
            Else
                MessageBox.Show("فشل في حفظ بيانات التفعيل. قد تكون هناك مشكلة في الصلاحيات أو نظام التسجيل.", "خطأ في التفعيل", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As SecurityException
            MessageBox.Show("يحتاج البرنامج لصلاحيات مدير (Administrator) لتفعيله. يرجى تشغيل البرنامج كمسؤول ثم أعد المحاولة.", "صلاحيات غير كافية", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As UnauthorizedAccessException
            MessageBox.Show("غير مصرح بالوصول إلى مسار التسجيل المطلوب. تأكد من صلاحياتك وحاول مرة أخرى.", "وصول غير مصرح به", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ غير متوقع أثناء عملية التفعيل: {ex.Message}", "خطأ عام", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' إعادة تمكين الزر في جميع الحالات
            btnActivate.Enabled = True
            btnActivate.Text = "تفعيل"
        End Try
    End Sub
    Private Sub ActivationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtSerial.Text = Value()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub
End Class