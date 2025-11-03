Public Class DateDifferenceForm

    Private Sub BtnCalc_Click(sender As Object, e As EventArgs) Handles btnCalc.Click

        Try
            ' التحقق من صحة التواريخ
            If dtpStartDate.Value > dtpEndDate.Value Then
                MessageBox.Show("تاريخ البدء يجب أن يكون قبل أو يساوي تاريخ الانتهاء", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' عرض النتيجة
            ShowDateDifference(dtpStartDate.Value, dtpEndDate.Value,
                              lblYears, lblMonths, lblDays,
                              lblTotalDays, lblFridays,
                              lblSaturdays, lblHolidays)

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء الحساب: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try




        '' إنشاء كائن من الكلاس DateDifferenceCalculator


        '' الحصول على التواريخ من DateTimePicker
        'Dim startDate As Date = Me.startDate.Value
        'Dim endDate As Date = Me.endDate.Value

        '' عرض الفرق بين التاريخين في Labels
        'ShowDateDifference(startDate, endDate, lblYears, lblMonths, lblDays, lblTotalDays)

    End Sub

    Private Sub endDate_KeyDown(sender As Object, e As KeyEventArgs) Handles dtpEndDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnCalc.PerformClick()
        End If
    End Sub


End Class