
Public Class FrmAlerts

    Private alertClass As New AlertsClass()
    Private selectedID As Integer = 0
    Private alertsTable As DataTable

#Region "تحميل البيانات والتنسيقات"
    Private Sub FrmAlerts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBoxType()
        LoadAlerts()

    End Sub

    Private Sub FrmAlerts_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        txtTitle.Focus()
    End Sub

    Private Sub LoadAlerts()
        alertsTable = alertClass.GetAllAlerts
        dgvAlerts.DataSource = alertsTable
        SetupAlertsGrid()
        FormatAlertsGrid()
        BeautifyAlertsGrid()
        SetupFilters()
        HighlightAlerts()
        ApplyFilters() ' 🔥 مهم جداً
        UpdateRecordCount()
        ClearForm()
        LoadStatsFromDB()

    End Sub
    Private Function ValidateInputs() As Boolean

        If String.IsNullOrWhiteSpace(txtTitle.Text) Then
            MessageBox.Show("أدخل عنوان التنبيه ⚠️")
            txtTitle.Focus()
            Return False
        End If

        If cmbType.SelectedIndex = -1 Then
            MessageBox.Show("اختر نوع التنبيه ⚠️")
            Return False
        End If

        If cmbType.Text = "أسبوعي" AndAlso cmbDayOfWeek.SelectedIndex = -1 Then
            MessageBox.Show("اختر يوم الأسبوع ⚠️")
            Return False
        End If

        If cmbType.Text = "شهري" AndAlso numDayOfMonth.Value = 0 Then
            MessageBox.Show("حدد يوم من الشهر ⚠️")
            Return False
        End If

        Return True

    End Function

    Private Sub ClearForm()

        txtTitle.Clear()
        txtDescription.Clear()

        cmbType.SelectedIndex = 0
        cmbDayOfWeek.SelectedIndex = -1

        numDayOfMonth.Value = 1
        numRepeatInterval.Value = 2

        dtpDate.Value = Date.Today
        dtpTime.Value = Date.Now

        chkActive.Checked = True
        chkRepeat.Checked = False

        selectedID = 0
        txtTitle.Focus()

    End Sub
#End Region

#Region "الأزرار"

    ' ================= ADD =================
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        If Not ValidateInputs() Then Return

        Dim alertType = cmbType.Text

        Dim alertDate As Date? = Nothing
        Dim dayOfWeek As Integer? = Nothing
        Dim dayOfMonth As Integer? = Nothing
        Dim alertTime As TimeSpan? = dtpTime.Value.TimeOfDay

        ' 🔁 التكرار
        Dim repeatInterval As Integer? =
        If(chkRepeat.Checked, CInt(numRepeatInterval.Value), CType(Nothing, Integer?))

        ' تحديد القيم حسب النوع
        Select Case alertType

            Case "مرة واحدة"
                alertDate = dtpDate.Value

            Case "يومي"
                alertDate = Date.Today

            Case "أسبوعي"
                If cmbDayOfWeek.SelectedIndex >= 0 Then
                    dayOfWeek = cmbDayOfWeek.SelectedIndex
                End If

            Case "شهري"
                dayOfMonth = CInt(numDayOfMonth.Value)

        End Select

        ' 🔥 الحفظ
        alertClass.InsertAlert(
        txtTitle.Text,
        txtDescription.Text,
        alertType,
        alertDate,
        dayOfWeek,
        dayOfMonth,
        GlobalData.LoggedInUserID,
        chkActive.Checked,
        alertTime,
        repeatInterval
    )

        LoadAlerts()
        MessageBox.Show("تمت الإضافة بنجاح ✅")
    End Sub

    ' ================= UPDATE =================
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        If Not ValidateInputs() Then Return

        If selectedID = 0 Then
            MessageBox.Show("اختر تنبيه أولاً ⚠️")
            Return
        End If

        Dim alertType = cmbType.Text

        Dim alertDate As Date? = Nothing
        Dim dayOfWeek As Integer? = Nothing
        Dim dayOfMonth As Integer? = Nothing
        Dim alertTime As TimeSpan? = dtpTime.Value.TimeOfDay

        ' 🔁 التكرار
        Dim repeatInterval As Integer? =
        If(chkRepeat.Checked, CInt(numRepeatInterval.Value), CType(Nothing, Integer?))

        Select Case alertType

            Case "مرة واحدة"
                alertDate = dtpDate.Value

            Case "يومي"
                alertDate = Date.Today

            Case "أسبوعي"
                If cmbDayOfWeek.SelectedIndex >= 0 Then
                    dayOfWeek = cmbDayOfWeek.SelectedIndex
                End If

            Case "شهري"
                dayOfMonth = CInt(numDayOfMonth.Value)

        End Select

        alertClass.UpdateAlert(
        selectedID,
        txtTitle.Text,
        txtDescription.Text,
        alertType,
        alertDate,
        dayOfWeek,
        dayOfMonth,
        chkActive.Checked,
        alertTime,
        repeatInterval
    )

        LoadAlerts()

        MessageBox.Show("تم التعديل بنجاح ✅")

    End Sub

    ' ================= DELETE =================
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MessageBox.Show("هل أنت متأكد من الحذف؟", "تأكيد",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
        If selectedID = 0 Then Return

        alertClass.DeleteAlert(selectedID)
        LoadAlerts()
        ClearForm()
        MessageBox.Show("تم الحذف بنجاح ✅")

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        Try
            LoadAlerts()

        Catch ex As Exception
            MessageBox.Show("خطأ أثناء التحديث: " & ex.Message)
        End Try

    End Sub


#End Region

#Region "تنسيقات القريد"

    Private Sub SetupAlertsGrid()

        With dgvAlerts

            ' 🎯 الشكل العام
            .RightToLeft = RightToLeft.Yes
            .RowHeadersVisible = False
            .BorderStyle = BorderStyle.Fixed3D
            .CellBorderStyle = DataGridViewCellBorderStyle.Single

            ' 🎨 الهيدر
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Cairo", 10, FontStyle.Bold)

            ' 🎨 الصفوف
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .DefaultCellStyle.Font = New Font("Cairo", 9)

            ' 🎨 صفوف متناوبة
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245)

            ' 📏 إعدادات
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            ' 🚫 منع التعديل والإضافة
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False

        End With

    End Sub

    Private Sub FormatAlertsGrid()

        With dgvAlerts

            If .Columns.Count = 0 Then Exit Sub

            .Columns("ID").HeaderText = "رقم"
            .Columns("Title").HeaderText = "عنوان التنبيه"
            .Columns("Description").HeaderText = "التفاصيل"
            .Columns("AlertDate").HeaderText = "التاريخ"
            .Columns("AlertType").HeaderText = "نوع التنبيه"
            .Columns("AlertTime").HeaderText = "وقت التنبيه"
            .Columns("DayOfWeek").HeaderText = "اليوم"
            .Columns("DayOfMonth").HeaderText = "يوم الشهر"
            .Columns("UserName").HeaderText = "المستخدم"
            .Columns("IsActive").HeaderText = "نشط"
            .Columns("RepeatInterval").HeaderText = "تكرار"

            .Columns("ID").Visible = False

            .Columns("DayOfWeek").Width = 60
            .Columns("DayOfMonth").Width = 80
            .Columns("IsActive").Width = 60
            .Columns("Description").Width = 150
            .Columns("Title").Width = 100
            .Columns("UserName").Width = 60
            .Columns("AlertType").Width = 80
            .Columns("AlertTime").Width = 100
            .Columns("AlertDate").Width = 80
            .Columns("RepeatInterval").Width = 60

        End With

    End Sub

    Private Sub BeautifyAlertsGrid()

        With dgvAlerts

            If .Columns.Contains("AlertDate") Then
                .Columns("AlertDate").DefaultCellStyle.Format = "yyyy/MM/dd"
            End If

        End With

    End Sub

    Private Sub HighlightAlerts()

        For Each row As DataGridViewRow In dgvAlerts.Rows

            Dim cellValue = row.Cells("AlertDate").Value

            ' ✅ التحقق الصحيح من DBNull
            If cellValue Is Nothing OrElse IsDBNull(cellValue) Then
                Continue For
            End If

            Dim alertDate As Date

            ' ✅ تحويل آمن
            If Not Date.TryParse(cellValue.ToString(), alertDate) Then
                Continue For
            End If

            ' 🎨 التلوين
            If alertDate < Date.Today Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210) ' 🔴 متأخر
                row.DefaultCellStyle.ForeColor = Color.DarkRed
            ElseIf alertDate = Date.Today Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196) ' 🟡 اليوم
                row.DefaultCellStyle.ForeColor = Color.DarkOrange
            Else
                row.DefaultCellStyle.BackColor = Color.FromArgb(200, 230, 201) ' 🟢 قادم
                row.DefaultCellStyle.ForeColor = Color.DarkGreen
            End If

        Next

    End Sub

    Private Sub dgvAlerts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAlerts.CellClick

        If e.RowIndex < 0 Then Return

        Dim row = dgvAlerts.Rows(e.RowIndex)

        ' ================= ID =================
        selectedID = Convert.ToInt32(row.Cells("ID").Value)

        ' ================= نصوص =================
        txtTitle.Text = If(row.Cells("Title").Value?.ToString(), "")
        txtDescription.Text = If(row.Cells("Description").Value?.ToString(), "")

        ' ================= النوع =================
        Dim type As String = If(row.Cells("AlertType").Value?.ToString(), "")
        cmbType.Text = type

        ' ================= التاريخ =================
        Dim dateVal = row.Cells("AlertDate").Value

        If dateVal IsNot Nothing AndAlso Not IsDBNull(dateVal) Then
            dtpDate.Value = Convert.ToDateTime(dateVal)
        Else
            dtpDate.Value = Date.Today
        End If

        ' ================= الوقت =================
        If dgvAlerts.Columns.Contains("AlertTime") Then

            Dim timeVal = row.Cells("AlertTime").Value

            If timeVal IsNot Nothing AndAlso Not IsDBNull(timeVal) Then
                dtpTime.Value = Date.Today.Add(CType(timeVal, TimeSpan))
            Else
                dtpTime.Value = Date.Now.ToShortTimeString
            End If

        End If

        ' ================= يوم الأسبوع =================
        If dgvAlerts.Columns.Contains("DayOfWeek") Then

            Dim dayVal = row.Cells("DayOfWeek").Value

            If dayVal IsNot Nothing AndAlso Not IsDBNull(dayVal) Then
                cmbDayOfWeek.SelectedIndex = Convert.ToInt32(dayVal)
            Else
                cmbDayOfWeek.SelectedIndex = -1
            End If

        End If

        ' ================= يوم الشهر =================

        If dgvAlerts.Columns.Contains("DayOfMonth") Then

            Dim dayMonthVal = row.Cells("DayOfMonth").Value

            If dayMonthVal IsNot Nothing AndAlso Not IsDBNull(dayMonthVal) Then
                numDayOfMonth.Value = Convert.ToDecimal(dayMonthVal)
            Else
                numDayOfMonth.Value = 1
            End If

        End If

        ' ================= التفعيل =================
        If dgvAlerts.Columns.Contains("IsActive") Then

            Dim activeVal = row.Cells("IsActive").Value

            If activeVal IsNot Nothing AndAlso Not IsDBNull(activeVal) Then
                chkActive.Checked = Convert.ToBoolean(activeVal)
            Else
                chkActive.Checked = True
            End If

        End If

        ' ================= التكرار =================
        If dgvAlerts.Columns.Contains("RepeatInterval") Then

            Dim repeatVal = row.Cells("RepeatInterval").Value

            If repeatVal IsNot Nothing AndAlso Not IsDBNull(repeatVal) Then
                chkRepeat.Checked = Convert.ToBoolean(repeatVal)
            Else
                chkRepeat.Checked = False
            End If

        End If

        ' ================= 🔥 تحديث UX =================
        cmbType_SelectedIndexChanged(Nothing, Nothing)

    End Sub

    '    Private Sub dgvAlerts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAlerts.CellClick

    '        If e.RowIndex < 0 Then Return

    '        With dgvAlerts.Rows(e.RowIndex)
    '            txtTitle.Text = .Cells("Title").Value.ToString()
    '            txtDescription.Text = .Cells("Description").Value.ToString()
    '            dtpDate.Value = Convert.ToDateTime(.Cells("AlertDate").Value)
    '        End With

    '    End Sub

    ' 🔥 التحويل للعرض فقط بدون كسر البيانات

    Private Sub dgvAlerts_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvAlerts.CellFormatting

        If dgvAlerts.Columns(e.ColumnIndex).Name = "AlertType" Then

            If e.Value IsNot Nothing Then

                Select Case e.Value.ToString()
                    Case "Once" : e.Value = "مرة واحدة"
                    Case "Daily" : e.Value = "يومي"
                    Case "Weekly" : e.Value = "أسبوعي"
                    Case "Monthly" : e.Value = "شهري"
                End Select

                e.FormattingApplied = True

            End If

        End If
        If dgvAlerts.Columns(e.ColumnIndex).Name = "DayOfWeek" Then

            If e.Value IsNot Nothing Then

                Dim days = {"الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت"}

                Dim index As Integer
                If Integer.TryParse(e.Value.ToString(), index) AndAlso index >= 0 AndAlso index <= 6 Then
                    e.Value = days(index)
                    e.FormattingApplied = True
                End If

            End If

        End If

    End Sub

    Private Sub dgvAlerts_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvAlerts.DataError
        e.ThrowException = False
    End Sub

#End Region

#Region "وظائف التنبيه"
    Private Sub ComboBoxType()
        cmbType.Items.AddRange({
        "مرة واحدة",
        "يومي",
        "أسبوعي",
        "شهري"
    })

        cmbDayOfWeek.Items.AddRange({
            "الأحد", "الاثنين", "الثلاثاء",
            "الأربعاء", "الخميس", "الجمعة", "السبت"
        })

        cmbType.SelectedIndex = 0

    End Sub

    Private Sub cmbType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType.SelectedIndexChanged

        ' 🔄 إعادة تعيين القيم
        cmbDayOfWeek.SelectedIndex = -1
        numDayOfMonth.Value = 1

        Select Case cmbType.Text

            Case "مرة واحدة"
                dtpDate.Enabled = True

                cmbDayOfWeek.Visible = False
                Label9.Visible = False

                numDayOfMonth.Visible = False
                Label4.Visible = False

            Case "يومي"
                dtpDate.Enabled = False

                cmbDayOfWeek.Visible = False
                Label9.Visible = False

                numDayOfMonth.Visible = False
                Label4.Visible = False

            Case "أسبوعي"
                dtpDate.Enabled = False

                cmbDayOfWeek.Visible = True
                Label9.Visible = True

                numDayOfMonth.Visible = False
                Label4.Visible = False

            Case "شهري"
                dtpDate.Enabled = False

                cmbDayOfWeek.Visible = False
                Label9.Visible = False

                numDayOfMonth.Visible = True
                Label4.Visible = True

        End Select

    End Sub
#End Region

#Region "الفلترة والبحث"

    'Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
    '    If txtSearch.Text = "ابحث هنا..." Then
    '        txtSearch.Text = ""
    '    End If
    'End Sub

    'Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
    '    If txtSearch.Text = "" Then
    '        txtSearch.Text = "ابحث هنا..."
    '    End If
    'End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        ApplyFilters()
    End Sub

    Private Sub cmbFilterStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFilterStatus.SelectedIndexChanged
        ApplyFilters()
    End Sub

    Private Sub cmbFilterType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFilterType.SelectedIndexChanged
        ApplyFilters()
    End Sub

    Private Sub chkRepeat_CheckedChanged(sender As Object, e As EventArgs) Handles chkRepeat.CheckedChanged
        numRepeatInterval.Enabled = chkRepeat.Checked
    End Sub

    Private Sub SetupFilters()

        cmbFilterStatus.Items.AddRange({
            "الكل",
            "نشط فقط",
            "غير نشط"
        })

        cmbFilterType.Items.AddRange({
            "الكل",
            "مرة واحدة",
            "يومي",
            "أسبوعي",
            "شهري"
        })

        cmbFilterStatus.SelectedIndex = 0
        cmbFilterType.SelectedIndex = 0

    End Sub

    Private Sub ApplyFilters()

        If alertsTable Is Nothing Then Return

        Dim dv As New DataView(alertsTable)

        Dim filters As New List(Of String)

        ' 🔍 البحث
        If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
            filters.Add($"Title LIKE '%{txtSearch.Text}%' OR Description LIKE '%{txtSearch.Text}%'")
        End If

        ' 📊 الحالة
        Select Case cmbFilterStatus.Text
            Case "نشط فقط"
                filters.Add("IsActive = 1")
            Case "غير نشط"
                filters.Add("IsActive = 0")
        End Select

        ' 🧠 النوع
        If cmbFilterType.Text <> "الكل" Then
            filters.Add($"AlertType = '{cmbFilterType.Text}'")
        End If

        dv.RowFilter = String.Join(" AND ", filters)

        dgvAlerts.DataSource = dv
        UpdateRecordCount()
    End Sub

    Private Sub UpdateAlertStats()

        Dim dt As DataTable = TryCast(dgvAlerts.DataSource, DataTable)

        If dt Is Nothing Then Return

        ' 📊 الإجمالي
        lblTotalVal.Text = dt.Rows.Count.ToString()

        ' 🟢 المفعلة
        Dim activeCount = dt.AsEnumerable().
            Count(Function(r) Not r.IsNull("IsActive") AndAlso Convert.ToBoolean(r("IsActive")) = True)

        lblActiveVal.Text = activeCount.ToString()

        ' 🔴 غير المفعلة
        Dim inactiveCount = dt.AsEnumerable().
            Count(Function(r) Not r.IsNull("IsActive") AndAlso Convert.ToBoolean(r("IsActive")) = False)

        lblInactiveVal.Text = inactiveCount.ToString()

    End Sub


    Private Sub LoadStatsFromDB()

        Dim dt = alertClass.GetAlertsStats()

        If dt.Rows.Count > 0 Then

            lblTotalVal.Text = dt.Rows(0)("Total").ToString()
            lblActiveVal.Text = dt.Rows(0)("Active").ToString()
            lblInactiveVal.Text = dt.Rows(0)("Inactive").ToString()

        End If

    End Sub

    Private Sub UpdateRecordCount()

        ' استبعاد صف الإدخال الجديد
        Dim count As Integer = dgvAlerts.Rows.Cast(Of DataGridViewRow)().
                               Count(Function(r) Not r.IsNewRow)

        lblRecordCount.Text = "عدد السجلات: " & count.ToString()

    End Sub



#End Region

End Class