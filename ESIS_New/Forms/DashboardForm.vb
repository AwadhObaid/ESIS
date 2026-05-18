
Imports System.Windows.Media
Imports LiveCharts
Imports LiveCharts.Wpf

Public Class DashboardForm
    Inherits BaseFormWithPermissions

    Private statisticsData As DataTable
    Private foldersData As DataTable
    Private monthlyData As DataTable

    Private isDataLoaded As Boolean = False
    Private lastTotal As Integer = 0

    Private pieSeries As PieSeries
    Private columnSeries As ColumnSeries
    Private lineSeries As LineSeries

    Private archiveClass As New ArchiveClass()

    Private scaler As ControlScaler

    Public Sub New()
        ' هذا السطر مطلوب لتهيئة النموذج.
        InitializeComponent()
        ' تهيئة الكلاس مع النموذج الحالي
        scaler = New ControlScaler(Me)
    End Sub

    ' ==============================
    ' تحميل الفورم
    ' ==============================
    Private Async Sub DashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            Me.Text = "لوحة التحكم - ESIS"
            lblTitle.Text = "لوحة التحكم"
            PanelMain.AutoScroll = True
            SetupCharts()

            Await LoadDashboardDataAsync()

            TimerRefresh.Interval = 10000
            TimerRefresh.Start()

            Debug.WriteLine("OutgoingCount Label Parent: " & lblOutgoingCount.Parent?.Name)
            Debug.WriteLine("OutgoingCount Visible: " & lblOutgoingCount.Visible)
            'Debug.WriteLine("OutgoingCount Size: " & lblOutgoingCount.Size)
            'Debug.WriteLine("OutgoingCount Location: " & lblOutgoingCount.Location)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الداشبورد: " & ex.Message)
        End Try


    End Sub

    ' ==============================
    ' تحجيم الفورم
    ' ==============================
    Private Sub DashboardForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ' التحقق من أن الكائن scaler مهيأ قبل استدعائه
        If scaler IsNot Nothing Then
            scaler.ScaleControls(Me)
        End If
    End Sub

    Private Sub lblOutgoingCount_Click(sender As Object, e As EventArgs) Handles lblOutgoingCount.Click
        OpenMdiForm(Of SaderForm)(Me)
    End Sub

    Private Sub lblArchivedCount_Click(sender As Object, e As EventArgs) Handles lblArchivedCount.Click
        OpenMdiForm(Of ArchiveForm)(Me)
    End Sub

    Private Sub lblUnarchivedCount_Click(sender As Object, e As EventArgs) Handles lblUnarchivedCount.Click
        OpenMdiForm(Of UnarchivedRecordsForm)(Me)
    End Sub

    Private Sub lblDeletedCount_Click(sender As Object, e As EventArgs) Handles lblDeletedCount.Click
        OpenMdiForm(Of DeletedArchiveForm)(Me)
    End Sub

    ' ==============================
    ' تحميل البيانات (Async)
    ' ==============================
    Private Async Function LoadDashboardDataAsync() As Task

        Try
            SetLoadingState(True)

            Await Task.Run(AddressOf LoadStatisticsData)

            UpdateUI()

            isDataLoaded = True

        Catch ex As Exception
            ShowStatus("❌ فشل تحميل البيانات", System.Drawing.Color.Red)
        Finally
            SetLoadingState(False)
        End Try

    End Function

    ' ==============================
    ' تحميل البيانات من DB
    ' ==============================
    Private Sub LoadStatisticsData()

        statisticsData = archiveClass.GetArchiveStatistics()
        foldersData = archiveClass.GetArchiveByFolder()
        monthlyData = archiveClass.GetMonthlyArchiveTrend()

    End Sub

    ' ==============================
    ' تحديث الواجهة
    ' ==============================
    Private Sub UpdateUI()

        UpdateSummaryCounters()
        UpdateArchiveOverviewChart()
        UpdateArchiveByFolderChart()
        UpdateMonthlyTrendChart()

        ' 🔥 الذكاء
        CheckAlerts()
        CalculateHealth()
        ShowTopFolder()

    End Sub

    ' ==============================
    ' العدادات
    ' ==============================
    Private Sub UpdateSummaryCounters()

        If statisticsData Is Nothing OrElse statisticsData.Rows.Count = 0 Then Exit Sub

        Dim row = statisticsData.Rows(0)

        lblOutgoingCount.Text = row("OutgoingCount").ToString()
        lblArchivedCount.Text = row("ArchivedCount").ToString()
        lblUnarchivedCount.Text = row("UnarchivedCount").ToString()
        lblDeletedCount.Text = row("DeletedCount").ToString()

    End Sub

    ' ==============================
    ' Pie Chart
    ' ==============================
    Private Sub UpdateArchiveOverviewChart()

        If statisticsData Is Nothing OrElse statisticsData.Rows.Count = 0 Then Exit Sub

        Dim row = statisticsData.Rows(0)

        Dim archived = CDbl(row("ArchivedCount"))
        Dim unarchived = CDbl(row("UnarchivedCount"))
        Dim deleted = CDbl(row("DeletedCount"))

        chartArchiveOverview.Series.Clear()

        chartArchiveOverview.Series = New SeriesCollection From {
            New PieSeries With {
                .Title = "المؤرشفة",
                .Values = New ChartValues(Of Double) From {archived},
                .Fill = New SolidColorBrush(Color.FromRgb(76, 175, 80)),
                .DataLabels = True
            },
            New PieSeries With {
                .Title = "غير المؤرشفة",
                .Values = New ChartValues(Of Double) From {unarchived},
                .Fill = New SolidColorBrush(Color.FromRgb(255, 152, 0)),
                .DataLabels = True
            },
            New PieSeries With {
                .Title = "المحذوفة",
                .Values = New ChartValues(Of Double) From {deleted},
                .Fill = New SolidColorBrush(Color.FromRgb(244, 67, 54)),
                .DataLabels = True
            }
        }

        chartArchiveOverview.LegendLocation = LegendLocation.Right
        chartArchiveOverview.HoverPushOut = 15

    End Sub

    ' ==============================
    ' Column Chart
    ' ==============================
    Private Sub UpdateArchiveByFolderChart()

        If foldersData Is Nothing OrElse foldersData.Rows.Count = 0 Then Exit Sub

        chartArchiveByFolder.Series.Clear()
        chartArchiveByFolder.AxisX.Clear()
        chartArchiveByFolder.AxisY.Clear()

        Dim values As New ChartValues(Of Double)
        Dim labels As New List(Of String)

        For Each row In foldersData.Rows
            values.Add(CDbl(row("Count")))
            labels.Add(row("ArchiveFolder").ToString())
        Next

        chartArchiveByFolder.Series.Add(New ColumnSeries With {
            .values = values,
            .DataLabels = True,
            .MaxColumnWidth = 40
        })

        chartArchiveByFolder.AxisX.Add(New Axis With {.labels = labels})
        chartArchiveByFolder.AxisY.Add(New Axis With {.MinValue = 0})

    End Sub

    ' ==============================
    ' Line Chart
    ' ==============================
    Private Sub UpdateMonthlyTrendChart()

        If monthlyData Is Nothing OrElse monthlyData.Rows.Count = 0 Then Exit Sub

        chartMonthlyTrend.Series.Clear()
        chartMonthlyTrend.AxisX.Clear()
        chartMonthlyTrend.AxisY.Clear()

        Dim values As New ChartValues(Of Double)
        Dim labels As New List(Of String)

        For Each row In monthlyData.Rows
            values.Add(CDbl(row("Count")))
            labels.Add(row("MonthName").ToString())
        Next

        chartMonthlyTrend.Series.Add(New LineSeries With {
            .values = values,
            .LineSmoothness = 0.5,
            .PointGeometrySize = 10
        })

        chartMonthlyTrend.AxisX.Add(New Axis With {.labels = labels})
        chartMonthlyTrend.AxisY.Add(New Axis With {.MinValue = 0})

    End Sub

    ' ==============================
    ' 🔥 تنبيهات ذكية
    ' ==============================
    Private Sub CheckAlerts()

        Dim unarchived As Integer = Val(lblUnarchivedCount.Text)

        If unarchived > 50 Then
            ShowStatus("⚠ يوجد عدد كبير غير مؤرشف", System.Drawing.Color.OrangeRed)
        End If

    End Sub

    ' ==============================
    ' 🔥 تقييم النظام
    ' ==============================
    Private Sub CalculateHealth()

        Dim total = Val(lblOutgoingCount.Text)
        Dim archived = Val(lblArchivedCount.Text)

        If total = 0 Then Exit Sub

        Dim ratio = archived / total

        If ratio >= 0.8 Then
            ShowStatus("✅ النظام ممتاز", System.Drawing.Color.Green)
        ElseIf ratio >= 0.5 Then
            ShowStatus("⚠ النظام متوسط", System.Drawing.Color.Orange)
        Else
            ShowStatus("❌ يحتاج تحسين", System.Drawing.Color.Red)
        End If

    End Sub

    ' ==============================
    ' 🔥 أفضل مجلد
    ' ==============================
    Private Sub ShowTopFolder()

        If foldersData Is Nothing OrElse foldersData.Rows.Count = 0 Then Exit Sub

        Dim top = foldersData.AsEnumerable().
            OrderByDescending(Function(r) r("Count")).First()

        ShowStatus("📁 الأكثر نشاط: " & top("ArchiveFolder").ToString(), System.Drawing.Color.Blue)

    End Sub

    ' ==============================
    ' حالة التحميل
    ' ==============================
    Private Sub SetLoadingState(isLoading As Boolean)

        If Me.InvokeRequired Then
            Me.Invoke(Sub() SetLoadingState(isLoading))
            Return
        End If

        lblStatus.Visible = isLoading

        If isLoading Then
            lblStatus.Text = "⏳ جاري التحميل..."
        Else
            lblStatus.Text = "✔ تم التحديث"
        End If

    End Sub

    ' ==============================
    ' عرض رسالة
    ' ==============================
    Private Sub ShowStatus(msg As String, color As System.Drawing.Color)

        lblStatus.Visible = True
        lblStatus.Text = msg
        lblStatus.ForeColor = color

    End Sub

    ' ==============================
    ' Smart Refresh
    ' ==============================
    Private Function HasDataChanged() As Boolean

        Dim current = Val(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM TbSader"))

        If current <> lastTotal Then
            lastTotal = current
            Return True
        End If

        Return False

    End Function

    ' ==============================
    ' Timer
    ' ==============================
    Private Async Sub TimerRefresh_Tick(sender As Object, e As EventArgs) Handles TimerRefresh.Tick

        If Not Me.Visible Then Exit Sub

        If HasDataChanged() Then
            Await LoadDashboardDataAsync()
        End If

    End Sub

    ''' <summary>
    ''' تهيئة الرسوم البيانية
    ''' </summary>
    Private Sub SetupCharts()
        Try
            ' تهيئة رسم نظرة عامة (PieChart)
            pieSeries = New PieSeries() With {
                .Values = New ChartValues(Of Double)(),
                .DataLabels = True,
                .LabelPoint = Function(point) $"{point.Y:N0} ({point.Participation:P1})",
                .PushOut = 5
            }
            '
            ' تعيين الألوان
            pieSeries.Fill = New SolidColorBrush(Color.FromRgb(76, 175, 80))
            pieSeries.Stroke = New SolidColorBrush(Colors.White)
            pieSeries.StrokeThickness = 2

            ' إعداد السلسلة
            chartArchiveOverview.Series = New SeriesCollection From {pieSeries}

            ' تهيئة رسم توزيع المجلدات (CartesianChart)
            columnSeries = New ColumnSeries() With {
                .Values = New ChartValues(Of Double)(),
                .DataLabels = True,
                .LabelPoint = Function(point) point.Y.ToString("N0")
            }

            ' تعيين الألوان
            columnSeries.Fill = New SolidColorBrush(Color.FromRgb(76, 175, 80))
            columnSeries.Stroke = New SolidColorBrush(Color.FromRgb(33, 150, 243))
            columnSeries.StrokeThickness = 2

            ' إعداد السلسلة
            chartArchiveByFolder.Series = New SeriesCollection From {columnSeries}

            ' تهيئة رسم الاتجاه الشهري (CartesianChart)
            lineSeries = New LineSeries() With {
                .Values = New ChartValues(Of Double)(),
                .DataLabels = True,
                .LabelPoint = Function(point) point.Y.ToString("N0"),
                .PointGeometrySize = 15
            }

            ' تعيين الألوان
            lineSeries.Fill = New SolidColorBrush(Color.FromArgb(50, 76, 175, 80))
            lineSeries.Stroke = New SolidColorBrush(Color.FromRgb(76, 175, 80))
            lineSeries.StrokeThickness = 2

            ' إعداد السلسلة
            chartMonthlyTrend.Series = New SeriesCollection From {lineSeries}

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة الرسوم البيانية: {ex.Message}")
        End Try
    End Sub


End Class
