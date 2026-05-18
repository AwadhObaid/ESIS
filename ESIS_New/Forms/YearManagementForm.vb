Imports System.Data.SqlClient

Public Class YearManagementForm

#Region "🔹 Load Data"

    Private Sub ManageYearsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGrid()
        EnableDoubleBuffering(dgvYears) ' 🔥 هنا الحل
        LoadYears()

        'FormatGrid()
        'ColorRows()

    End Sub

    Private Sub LoadYears()

        Try
            Dim dt As New DataTable()

            Using con As SqlConnection = DatabaseHelper.GetConnection()
                Using cmd As New SqlCommand("
                SELECT 
                    Year,
                    IsClosed,
                    CASE 
                        WHEN IsClosed = 1 THEN N'مغلقة'
                        ELSE N'مفتوحة'
                    END AS StatusText
                FROM TbYears
                ORDER BY Year DESC
            ", con)

                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)

                End Using
            End Using

            dgvYears.DataSource = dt

            FormatGrid()

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل السنوات: " & ex.Message)
        End Try

    End Sub


    'Private Sub LoadYears()

    '    Try
    '        Dim dt As New DataTable()

    '        Using con As SqlConnection = DatabaseHelper.GetConnection()
    '            Using cmd As New SqlCommand("SELECT Year, IsClosed FROM TbYears ORDER BY Year DESC", con)

    '                Dim da As New SqlDataAdapter(cmd)
    '                da.Fill(dt)

    '            End Using
    '        End Using

    '        dgvYears.DataSource = dt

    '        FormatGrid()

    '    Catch ex As Exception
    '        MessageBox.Show("خطأ في تحميل السنوات: " & ex.Message)
    '    End Try

    'End Sub

#End Region

#Region "🎨 Grid Format"

    Private Sub SetupGrid()

        With dgvYears

            ' 🧱 الشكل العام
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .BackgroundColor = Color.White

            ' 📌 إخفاء رؤوس الصفوف
            .RowHeadersVisible = False
            .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing

            ' 🎯 الهيدر
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None

            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(22, 245, 254)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
            .ColumnHeadersDefaultCellStyle.Font = New Font("Cairo", 10, FontStyle.Bold)
            .ColumnHeadersHeight = 40

            ' 🧾 الصفوف
            .DefaultCellStyle.Font = New Font("Cairo", 10)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(76, 255, 0)
            .DefaultCellStyle.SelectionForeColor = Color.White

            ' 🔁 صفوف متناوبة
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(0, 46, 214)

            ' 📏 الحجم
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

            ' 🚫 منع التعديل المباشر (اختياري)
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            '' ✨ تحسين الأداء
            '.DoubleBuffered(True)

        End With

    End Sub

    Private Sub ColorRows()

        For Each row As DataGridViewRow In dgvYears.Rows

            If row.Cells("IsClosed").Value IsNot Nothing Then

                Dim isClosed As Boolean = Convert.ToBoolean(row.Cells("IsClosed").Value)

                If isClosed Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238) ' أحمر فاتح
                Else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233) ' أخضر فاتح
                End If

            End If

        Next

    End Sub

    Private Sub EnableDoubleBuffering(dgv As DataGridView)

        Try
            Dim dgvType As Type = dgv.GetType()

            Dim pi As Reflection.PropertyInfo =
            dgvType.GetProperty("DoubleBuffered",
            Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)

            If pi IsNot Nothing Then
                pi.SetValue(dgv, True, Nothing)
            End If

        Catch ex As Exception
            ' تجاهل أي خطأ
        End Try

    End Sub

    Private Sub FormatGrid()

        If dgvYears.Columns.Count = 0 Then Return

        dgvYears.Columns("Year").HeaderText = "السنة"

        dgvYears.Columns("StatusText").HeaderText = "الحالة"

        ' ❌ إخفاء العمود الحقيقي
        dgvYears.Columns("IsClosed").Visible = False

        dgvYears.Columns("Year").Width = 120
        dgvYears.Columns("StatusText").Width = 120

        ' 🎨 تلوين الصفوف
        For Each row As DataGridViewRow In dgvYears.Rows

            If row.Cells("IsClosed").Value IsNot Nothing Then

                Dim isClosed As Boolean = Convert.ToBoolean(row.Cells("IsClosed").Value)

                If isClosed Then
                    row.DefaultCellStyle.BackColor = Color.LightCoral
                Else
                    row.DefaultCellStyle.BackColor = Color.LightGreen
                End If

            End If

        Next

    End Sub

    'Private Sub FormatGrid()

    '    If dgvYears.Columns.Count = 0 Then Return

    '    dgvYears.Columns("Year").HeaderText = "السنة"
    '    dgvYears.Columns("IsClosed").HeaderText = "الحالة"

    '    dgvYears.Columns("Year").Width = 120
    '    dgvYears.Columns("IsClosed").Width = 120

    '    ' تحويل 0/1 إلى نص
    '    For Each row As DataGridViewRow In dgvYears.Rows

    '        If row.Cells("IsClosed").Value IsNot Nothing Then

    '            Dim val = Convert.ToBoolean(row.Cells("IsClosed").Value)

    '            row.Cells("IsClosed").Value = If(val, "مغلقة", "مفتوحة")

    '            ' 🎨 تلوين
    '            If val Then
    '                row.DefaultCellStyle.BackColor = Color.LightCoral
    '            Else
    '                row.DefaultCellStyle.BackColor = Color.LightGreen
    '            End If

    '        End If

    '    Next

    'End Sub

#End Region

#Region "🔘 Add Year"

    Private Sub btnAddYear_Click(sender As Object, e As EventArgs) Handles btnAddYear.Click

        Try
            If String.IsNullOrWhiteSpace(txtYear.Text) Then
                MessageBox.Show("أدخل السنة")
                Return
            End If

            Dim year As Integer = Convert.ToInt32(txtYear.Text)

            Using con As SqlConnection = DatabaseHelper.GetConnection()
                Using cmd As New SqlCommand("
                    IF NOT EXISTS (SELECT 1 FROM TbYears WHERE Year = @Year)
                    INSERT INTO TbYears (Year, IsClosed) VALUES (@Year, 0)
                ", con)

                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year

                    con.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

            LoadYears()
            txtYear.Clear()

        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message)
        End Try

    End Sub

#End Region

#Region "🔒 Close Year"

    Private Sub btnCloseYear_Click(sender As Object, e As EventArgs) Handles btnCloseYear.Click

        Try
            If dgvYears.SelectedRows.Count = 0 Then
                MessageBox.Show("اختر سنة أولاً")
                Return
            End If

            Dim year As Integer = Convert.ToInt32(dgvYears.SelectedRows(0).Cells("Year").Value)

            Using con As SqlConnection = DatabaseHelper.GetConnection()
                Using cmd As New SqlCommand("
                    UPDATE TbYears
                    SET IsClosed = 1
                    WHERE Year = @Year
                ", con)

                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year

                    con.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

            LoadYears()

        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message)
        End Try

    End Sub

#End Region

#Region "🔓 Open Year"

    Private Sub btnOpenYear_Click(sender As Object, e As EventArgs) Handles btnOpenYear.Click

        Try
            If dgvYears.SelectedRows.Count = 0 Then
                MessageBox.Show("اختر سنة أولاً")
                Return
            End If

            Dim year As Integer = Convert.ToInt32(dgvYears.SelectedRows(0).Cells("Year").Value)

            Using con As SqlConnection = DatabaseHelper.GetConnection()
                Using cmd As New SqlCommand("
                    UPDATE TbYears
                    SET IsClosed = 0
                    WHERE Year = @Year
                ", con)

                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year

                    con.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

            LoadYears()

        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message)
        End Try

    End Sub

#End Region

#Region "❌ Delete Year (اختياري)"

    Private Sub btnDeleteYear_Click(sender As Object, e As EventArgs) Handles btnDeleteYear.Click

        Try
            If dgvYears.SelectedRows.Count = 0 Then Return

            Dim year As Integer = Convert.ToInt32(dgvYears.SelectedRows(0).Cells("Year").Value)

            If MessageBox.Show("هل تريد حذف السنة؟", "تأكيد", MessageBoxButtons.YesNo) = DialogResult.No Then Return

            Using con As SqlConnection = DatabaseHelper.GetConnection()
                Using cmd As New SqlCommand("DELETE FROM TbYears WHERE Year=@Year", con)

                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year

                    con.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

            LoadYears()

        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message)
        End Try

    End Sub

#End Region

End Class

'Imports System.Data.SqlClient

'Public Class YearManagementForm
'    Private Sub ShowStatus(message As String, color As Color)

'        lblStatus.Text = message
'        lblStatus.ForeColor = color

'        TimerStatus.Stop()
'        TimerStatus.Start()

'    End Sub

'    Private Sub TimerStatus_Tick(sender As Object, e As EventArgs) Handles TimerStatus.Tick

'        lblStatus.Text = ""
'        TimerStatus.Stop()

'    End Sub

'    Private Sub LoadYears()

'        Dim query As String = "
'    SELECT 
'        Year, 
'        IsClosed,
'        CASE 
'            WHEN IsClosed = 1 THEN N'مغلقة' 
'            ELSE N'مفتوحة' 
'        END AS StatusText,
'        CreatedDate 
'    FROM SystemYears 
'    ORDER BY Year DESC"

'        Dim dt As DataTable = DatabaseHelper.ExecuteDataTable(query)

'        dgvYears.DataSource = dt

'        ' إخفاء العمود الحقيقي
'        dgvYears.Columns("IsClosed").Visible = False

'        dgvYears.Columns("Year").HeaderText = "السنة"
'        dgvYears.Columns("StatusText").HeaderText = "الحالة"
'        dgvYears.Columns("CreatedDate").HeaderText = "تاريخ الإنشاء"

'        dgvYears.Columns("Year").DisplayIndex = 0
'        dgvYears.Columns("StatusText").DisplayIndex = 1
'        dgvYears.Columns("CreatedDate").DisplayIndex = 2

'        dgvYears.Columns("CreatedDate").DefaultCellStyle.Format = "yyyy/MM/dd hh:mm tt"

'        ' =========================
'        ' إضافة عمود أيقونة
'        ' =========================
'        If Not dgvYears.Columns.Contains("StatusIcon") Then

'            Dim imgCol As New DataGridViewImageColumn()
'            imgCol.Name = "StatusIcon"
'            imgCol.HeaderText = ""
'            imgCol.Width = 40
'            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom

'            dgvYears.Columns.Insert(0, imgCol)

'        End If

'        '' =========================
'        '' إضافة زر
'        '' =========================
'        'If Not dgvYears.Columns.Contains("Action") Then

'        '    Dim btnCol As New DataGridViewButtonColumn()
'        '    btnCol.Name = "Action"
'        '    btnCol.HeaderText = "إجراء"
'        '    btnCol.Text = "تبديل"
'        '    btnCol.UseColumnTextForButtonValue = True

'        '    dgvYears.Columns.Add(btnCol)

'        'End If

'        ' =========================
'        ' تعبئة الأيقونات
'        ' =========================
'        For Each row As DataGridViewRow In dgvYears.Rows

'            If row.IsNewRow Then Continue For

'            Dim isClosed As Boolean = False

'            If row.Cells("IsClosed").Value IsNot Nothing Then
'                Boolean.TryParse(row.Cells("IsClosed").Value.ToString(), isClosed)
'            End If

'            row.Cells("StatusIcon").Value =
'            If(isClosed, My.Resources.lock, My.Resources.unlock)

'        Next

'    End Sub

'    Private Sub SetupGrid()

'        With dgvYears

'            .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
'            .RowHeadersVisible = False

'            .BorderStyle = BorderStyle.None
'            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal

'            .EnableHeadersVisualStyles = False
'            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None

'            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 245, 254)
'            .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black

'            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
'            .DefaultCellStyle.SelectionForeColor = Color.White

'            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245)

'            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

'        End With

'    End Sub

'    Private Sub YearManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

'        SetupGrid()   ' إعداد التصميم
'        LoadYears()   ' تحميل البيانات

'        lblStatus.BackColor = Color.FromArgb(30, 30, 30)
'        lblStatus.ForeColor = Color.White
'        lblStatus.Font = New Font("Cairo", 9, FontStyle.Bold)

'    End Sub

'    Private Sub btnAddYear_Click(sender As Object, e As EventArgs) Handles btnAddYear.Click

'        Dim newYear As Integer = Date.Now.Year

'        ' ✅ رسالة تأكيد
'        Dim result = MessageBox.Show(
'        $"هل تريد إضافة السنة {newYear}؟",
'        "تأكيد",
'        MessageBoxButtons.YesNo,
'        MessageBoxIcon.Question)

'        If result <> DialogResult.Yes Then Exit Sub

'        Dim query As String =
'    "IF NOT EXISTS (SELECT 1 FROM SystemYears WHERE Year = @Year)
'     INSERT INTO SystemYears (Year, IsClosed) VALUES (@Year, 0)"

'        Dim parameters() As SqlParameter = {
'        DatabaseHelper.CreateParameter("@Year", newYear)
'    }

'        DatabaseHelper.ExecuteNonQuery(query, parameters)

'        ShowStatus("تم إضافة السنة: " & newYear & " ✅", Color.Green)

'        LoadYears()

'    End Sub

'    'Private Sub btnCloseYear_Click(sender As Object, e As EventArgs) Handles btnCloseYear.Click

'    '    If dgvYears.CurrentRow Is Nothing Then Exit Sub

'    '    Dim row = dgvYears.CurrentRow
'    '    Dim year As Integer = Convert.ToInt32(row.Cells("Year").Value)

'    '    ' 🔥 التحقق إذا السنة مقفلة مسبقاً
'    '    Dim isClosed As Boolean = False

'    '    If row.Cells("IsClosed").Value IsNot Nothing Then
'    '        Boolean.TryParse(row.Cells("IsClosed").Value.ToString(), isClosed)
'    '    End If

'    '    If isClosed Then
'    '        ShowStatus("السنة مقفلة مسبقاً ⚠", Color.Orange)
'    '        Exit Sub
'    '    End If

'    '    ' ✅ رسالة التأكيد (بعد التحقق)
'    '    Dim result = MessageBox.Show(
'    '    $"هل تريد قفل السنة {year}؟ 🔒",
'    '    "تأكيد",
'    '    MessageBoxButtons.YesNo,
'    '    MessageBoxIcon.Warning)

'    '    If result <> DialogResult.Yes Then Exit Sub

'    '    ' 🔥 تنفيذ القفل
'    '    Dim query As String = "UPDATE SystemYears SET IsClosed = 1 WHERE Year = @Year"

'    '    Dim parameters() As SqlParameter = {
'    '    DatabaseHelper.CreateParameter("@Year", year)
'    '}

'    '    DatabaseHelper.ExecuteNonQuery(query, parameters)

'    '    ShowStatus("تم قفل السنة: " & year & " 🔒", Color.Red)

'    '    LoadYears()

'    'End Sub

'    'Private Sub btnOpenYear_Click(sender As Object, e As EventArgs) Handles btnOpenYear.Click

'    '    If dgvYears.CurrentRow Is Nothing Then Exit Sub

'    '    Dim row = dgvYears.CurrentRow
'    '    Dim year As Integer = Convert.ToInt32(row.Cells("Year").Value)

'    '    ' 🔥 التحقق من حالة السنة
'    '    Dim isClosed As Boolean = False

'    '    If row.Cells("IsClosed").Value IsNot Nothing Then
'    '        Boolean.TryParse(row.Cells("IsClosed").Value.ToString(), isClosed)
'    '    End If

'    '    ' ❌ إذا كانت مفتوحة مسبقاً
'    '    If Not isClosed Then
'    '        ShowStatus("السنة مفتوحة مسبقاً ⚠", Color.Orange)
'    '        Exit Sub
'    '    End If

'    '    ' ✅ رسالة تأكيد
'    '    Dim result = MessageBox.Show(
'    '    $"هل تريد فتح السنة {year}؟ 🔓",
'    '    "تأكيد",
'    '    MessageBoxButtons.YesNo,
'    '    MessageBoxIcon.Question)

'    '    If result <> DialogResult.Yes Then Exit Sub

'    '    ' 🔥 تنفيذ الفتح
'    '    Dim query As String = "UPDATE SystemYears SET IsClosed = 0 WHERE Year = @Year"

'    '    Dim parameters() As SqlParameter = {
'    '    DatabaseHelper.CreateParameter("@Year", year)
'    '}

'    '    DatabaseHelper.ExecuteNonQuery(query, parameters)

'    '    ShowStatus("تم فتح السنة: " & year & " 🔓", Color.Green)

'    '    LoadYears()

'    'End Sub

'    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
'        LoadYears()
'        SetupGrid()
'    End Sub

'    Private Sub dgvYears_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvYears.RowPrePaint

'        If e.RowIndex < 0 Then Exit Sub

'        Dim row = dgvYears.Rows(e.RowIndex)

'        Dim year As Integer = Convert.ToInt32(row.Cells("Year").Value)

'        If year = Date.Now.Year Then
'            row.DefaultCellStyle.Font = New Font(dgvYears.Font, FontStyle.Bold)
'        End If

'    End Sub

'    Private Sub dgvYears_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvYears.CellClick

'        If e.RowIndex < 0 Then Exit Sub

'        If dgvYears.Columns(e.ColumnIndex).Name = "Action" Then

'            Dim row = dgvYears.Rows(e.RowIndex)

'            Dim year As Integer = Convert.ToInt32(row.Cells("Year").Value)

'            Dim isClosed As Boolean = False

'            If row.Cells("IsClosed").Value IsNot Nothing Then
'                Boolean.TryParse(row.Cells("IsClosed").Value.ToString(), isClosed)
'            End If

'            Dim newStatus As Integer = If(isClosed, 0, 1)

'            Dim query As String = "UPDATE SystemYears SET IsClosed = @Status WHERE Year = @Year"

'            Dim parameters() As SqlParameter = {
'            DatabaseHelper.CreateParameter("@Status", newStatus),
'            DatabaseHelper.CreateParameter("@Year", year)
'        }
'            If isClosed Then
'                ShowStatus("السنة مقفلة مسبقاً ⚠", Color.Orange)
'                Exit Sub
'            ElseIf Not isClosed Then
'                ShowStatus("السنة مفتوحة مسبقاً ⚠", Color.Orange)
'                Exit Sub
'            End If

'            If Not isClosed Then
'                Dim result = MessageBox.Show($"هل تريد قفل السنة {year}؟", "تأكيد", MessageBoxButtons.YesNo)

'                If result <> DialogResult.Yes Then Exit Sub
'            End If

'            ' تنفيذ التحديث
'            DatabaseHelper.ExecuteNonQuery(query, parameters)

'            ' 🔥 هنا الرسالة (المكان الصحيح)
'            If newStatus = 1 Then
'                ShowStatus($"تم قفل السنة {year} 🔒", Color.Red)
'            Else
'                ShowStatus($"تم فتح السنة {year} 🔓", Color.Green)
'            End If

'            ' تحديث الجدول
'            LoadYears()

'        End If

'    End Sub

'    Private Sub dgvYears_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvYears.CellMouseEnter

'        If e.RowIndex >= 0 Then
'            dgvYears.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.FromArgb(220, 240, 255)
'        End If

'    End Sub

'    Private Sub dgvYears_CellMouseLeave(sender As Object, e As DataGridViewCellEventArgs) Handles dgvYears.CellMouseLeave
'        dgvYears.Invalidate()
'    End Sub

'    Private Sub btnToggleYear_Click(sender As Object, e As EventArgs) Handles btnToggleYear.Click

'        If dgvYears.CurrentRow Is Nothing Then
'            ShowStatus("يرجى اختيار سنة أولاً ⚠", Color.Orange)
'            Exit Sub
'        End If

'        Dim row = dgvYears.CurrentRow
'        Dim year As Integer = Convert.ToInt32(row.Cells("Year").Value)

'        ' قراءة الحالة
'        Dim isClosed As Boolean = False

'        If row.Cells("IsClosed").Value IsNot Nothing Then
'            Boolean.TryParse(row.Cells("IsClosed").Value.ToString(), isClosed)
'        End If

'        ' =========================
'        ' تحديد العملية
'        ' =========================
'        Dim newStatus As Integer
'        Dim actionText As String
'        Dim icon As String

'        If isClosed Then
'            newStatus = 0
'            actionText = "فتح"
'            icon = "🔓"
'        Else
'            newStatus = 1
'            actionText = "قفل"
'            icon = "🔒"
'        End If

'        ' =========================
'        ' رسالة التأكيد
'        ' =========================
'        Dim result = MessageBox.Show(
'            $"هل تريد {actionText} السنة {year}؟ {icon}",
'            "تأكيد",
'            MessageBoxButtons.YesNo,
'            MessageBoxIcon.Question)

'        If result <> DialogResult.Yes Then Exit Sub

'        ' =========================
'        ' تنفيذ العملية
'        ' =========================
'        Dim query As String = "UPDATE SystemYears SET IsClosed = @Status WHERE Year = @Year"

'        Dim parameters() As SqlParameter = {
'            DatabaseHelper.CreateParameter("@Status", newStatus),
'            DatabaseHelper.CreateParameter("@Year", year)
'        }

'        DatabaseHelper.ExecuteNonQuery(query, parameters)

'        ' =========================
'        ' رسالة الحالة
'        ' =========================
'        If newStatus = 1 Then
'            ShowStatus($"تم قفل السنة {year} 🔒", Color.Red)
'        Else
'            ShowStatus($"تم فتح السنة {year} 🔓", Color.Green)
'        End If

'        LoadYears()

'    End Sub

'    Private Sub dgvYears_SelectionChanged(sender As Object, e As EventArgs) Handles dgvYears.SelectionChanged

'        If dgvYears.CurrentRow Is Nothing Then Exit Sub

'        Dim isClosed As Boolean = False

'        If dgvYears.CurrentRow.Cells("IsClosed").Value IsNot Nothing Then
'            Boolean.TryParse(dgvYears.CurrentRow.Cells("IsClosed").Value.ToString(), isClosed)
'        End If

'        If isClosed Then
'            btnToggleYear.Text = "فتح السنة 🔓"
'            btnToggleYear.BackColor = Color.Green
'        Else
'            btnToggleYear.Text = "قفل السنة 🔒"
'            btnToggleYear.BackColor = Color.Red
'        End If

'    End Sub

'End Class