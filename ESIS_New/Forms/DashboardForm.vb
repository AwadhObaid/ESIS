
Imports System.Windows.Media
Imports LiveCharts
Imports LiveCharts.Defaults
Imports LiveCharts.Wpf

''' <summary>
''' نموذج لوحة التحكم والإحصائيات لنظام ESIS باستخدام الحزمة القديمة من LiveCharts
''' </summary>
Public Class DashboardForm
    Inherits BaseFormWithPermissions

    Private statisticsData As DataTable
    Private foldersData As DataTable
    Private monthlyData As DataTable
    Private isDataLoaded As Boolean = False
    Private pieSeries As PieSeries
    Private columnSeries As ColumnSeries
    Private lineSeries As LineSeries
    Private archiveClass As New ArchiveClass

    ''' <summary>
    ''' تحميل النموذج
    ''' </summary>
    Private Sub DashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تعيين خصائص النموذج
            Me.Text = "لوحة التحكم - نظام إدارة الوثائق الإلكتروني"
            Me.lblTitle.Text = "لوحة التحكم"

            ' تطبيق الصلاحيات
            ApplyPermissions()

            ' تهيئة الرسوم البيانية
            SetupCharts()

            ' تحميل البيانات الإحصائية
            LoadDashboardData()

            ' بدء مؤقت التحديث
            TimerRefresh.Start()

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل لوحة التحكم: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '''' <summary>
    '''' تطبيق الصلاحيات على النموذج
    '''' </summary>
    'Protected Overrides Sub ApplyPermissions()
    '    Try
    '        ' التحقق من صلاحية عرض لوحة التحكم
    '        If Not CheckPermission("ViewDashboard", "عرض لوحة التحكم") Then
    '            MessageBox.Show("ليس لديك صلاحية عرض لوحة التحكم. يرجى الاتصال بمدير النظام.",
    '                          "صلاحيات محدودة",
    '                          MessageBoxButtons.OK,
    '                          MessageBoxIcon.Information)
    '            Me.Close()
    '            Return
    '        End If

    '        ' تطبيق الصلاحيات الأخرى إذا لزم
    '        MyBase.ApplyPermissions()

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات: {ex.Message}")
    '    End Try
    'End Sub

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

    ''' <summary>
    ''' تحميل البيانات الإحصائية
    ''' </summary>
    Private Async Sub LoadDashboardData()
        Try
            ' عرض حالة التحميل
            SetLoadingState(True)

            ' تحميل البيانات بشكل غير متزامن
            Await Task.Run(AddressOf LoadStatisticsData)

            ' تحديث الواجهة
            UpdateUI()

            ' عرض البيانات
            isDataLoaded = True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل البيانات: {ex.Message}")
            MessageBox.Show($"فشل في تحميل البيانات الإحصائية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetLoadingState(False)
        End Try
    End Sub

    ''' <summary>
    ''' تحميل البيانات الإحصائية من قاعدة البيانات
    ''' </summary>
    Private Sub LoadStatisticsData()
        Try
            ' تحميل الإحصائيات الأساسية
            statisticsData = archiveClass.GetArchiveStatistics()

            ' تحميل توزيع الأرشيف حسب المجلدات
            foldersData = archiveClass.GetArchiveByFolder()

            ' تحميل الاتجاه الشهري
            monthlyData = archiveClass.GetMonthlyArchiveTrend()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل البيانات الإحصائية: {ex.Message}")
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' تحديث واجهة المستخدم بالبيانات
    ''' </summary>
    Private Sub UpdateUI()
        Try
            ' تحديث العدادات
            UpdateSummaryCounters()

            ' تحديث الرسوم البيانية
            UpdateArchiveOverviewChart()
            UpdateArchiveByFolderChart()
            UpdateMonthlyTrendChart()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث واجهة المستخدم: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث العدادات في الجزء العلوي
    ''' </summary>
    Private Sub UpdateSummaryCounters()
        Try
            ' التحقق من وجود البيانات
            If statisticsData IsNot Nothing AndAlso statisticsData.Rows.Count > 0 Then
                Dim row As DataRow = statisticsData.Rows(0)

                ' تحديث العدادات
                lblOutgoingCount.Text = row("OutgoingCount").ToString()
                lblArchivedCount.Text = row("ArchivedCount").ToString()
                lblUnarchivedCount.Text = row("UnarchivedCount").ToString()
                lblDeletedCount.Text = row("DeletedCount").ToString()
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث العدادات: {ex.Message}")
        End Try
    End Sub

    '''' <summary>
    '''' تحديث رسم نظرة عامة على الأرشيف باستخدام PieSeries
    '''' </summary>
    'Private Sub UpdateArchiveOverviewChart()
    '    Try
    '        ' التحقق من وجود البيانات
    '        If statisticsData Is Nothing OrElse statisticsData.Rows.Count = 0 Then
    '            Return
    '        End If

    '        Dim row As DataRow = statisticsData.Rows(0)
    '        Dim archivedCount As Double = Convert.ToDouble(row("ArchivedCount"))
    '        Dim unarchivedCount As Double = Convert.ToDouble(row("UnarchivedCount"))
    '        Dim deletedCount As Double = Convert.ToDouble(row("DeletedCount"))
    '        Dim totalCount As Double = archivedCount + unarchivedCount + deletedCount

    '        ' تنظيف السلسلة الحالية
    '        pieSeries.Values.Clear()

    '        ' إعداد القيم
    '        pieSeries.Values.Add(archivedCount)
    '        pieSeries.Values.Add(unarchivedCount)
    '        pieSeries.Values.Add(deletedCount)

    '        ' تحديث عنوان الرسم البياني
    '        If Me.InvokeRequired Then
    '            Me.Invoke(Sub()
    '                          lblChart1Title.Text = $"نظرة عامة على الأرشيف (الإجمالي: {totalCount:N0})"
    '                      End Sub)
    '        Else
    '            lblChart1Title.Text = $"نظرة عامة على الأرشيف (الإجمالي: {totalCount:N0})"
    '        End If

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تحديث رسم نظرة عامة: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' تحديث رسم نظرة عامة على الأرشيف
    ''' </summary>
    Private Sub UpdateArchiveOverviewChart()
        Try
            ' التحقق من وجود البيانات
            If statisticsData Is Nothing OrElse statisticsData.Rows.Count = 0 Then
                Return
            End If

            Dim row As DataRow = statisticsData.Rows(0)
            Dim archivedCount As Double = Convert.ToDouble(row("ArchivedCount"))
            Dim unarchivedCount As Double = Convert.ToDouble(row("UnarchivedCount"))
            Dim deletedCount As Double = Convert.ToDouble(row("DeletedCount"))

            ' تنظيف السلسلة الحالية
            chartArchiveOverview.Series.Clear()

            ' إعداد السلسلة
            Dim values As New ChartValues(Of Double) From {archivedCount, unarchivedCount, deletedCount}

            Dim labels As String() = {"المؤرشفة", "غير المؤرشفة", "المحذوفة"}
            Dim colors As Color() = {
            Color.FromRgb(76, 175, 80),   ' الأخضر
            Color.FromRgb(255, 152, 0),   ' البرتقالي
            Color.FromRgb(244, 67, 54)    ' الأحمر
        }

            For i As Integer = 0 To values.Count - 1
                Dim series As New PieSeries() With {
                .Title = labels(i),
                .Values = New ChartValues(Of Double) From {values(i)},
                .DataLabels = True,
                .LabelPoint = Function(point) $"{point.Y:N0} ({point.Participation:P1})"
            }

                If i < colors.Length Then
                    series.Fill = New SolidColorBrush(colors(i))
                End If

                series.Stroke = New SolidColorBrush(colors.FirstOrDefault)
                series.StrokeThickness = 2

                chartArchiveOverview.Series.Add(series)
            Next

            ' إعداد الخصائص
            chartArchiveOverview.LegendLocation = LegendLocation.Bottom
            chartArchiveOverview.DisableAnimations = False

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث رسم نظرة عامة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث رسم توزيع الأرشيف حسب المجلدات
    ''' </summary>
    Private Sub UpdateArchiveByFolderChart()
        Try
            ' التحقق من وجود البيانات
            If foldersData Is Nothing OrElse foldersData.Rows.Count = 0 Then
                Return
            End If

            ' تنظيف السلسلة الحالية
            columnSeries.Values.Clear()
            chartArchiveByFolder.AxisX.Clear()
            chartArchiveByFolder.AxisY.Clear()

            ' إعداد المحور X
            Dim labels As New List(Of String)()
            For Each row As DataRow In foldersData.Rows
                labels.Add(row("ArchiveFolder").ToString())
            Next

            ' إعداد المحور X
            chartArchiveByFolder.AxisX.Add(New Axis() With {
                .Labels = labels,
                .Separator = New Separator() With {.Step = 1}
            })

            ' إعداد المحور Y
            chartArchiveByFolder.AxisY.Add(New Axis() With {
                .MinValue = 0,
                .LabelFormatter = Function(value) value.ToString("N0")
            })

            ' إعداد القيم
            For Each row As DataRow In foldersData.Rows
                columnSeries.Values.Add(Convert.ToDouble(row("Count")))
            Next

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث رسم توزيع المجلدات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث رسم الاتجاه الشهري
    ''' </summary>
    Private Sub UpdateMonthlyTrendChart()
        Try
            ' التحقق من وجود البيانات
            If monthlyData Is Nothing OrElse monthlyData.Rows.Count = 0 Then
                Return
            End If

            ' تنظيف السلسلة الحالية
            lineSeries.Values.Clear()
            chartMonthlyTrend.AxisX.Clear()
            chartMonthlyTrend.AxisY.Clear()

            ' إعداد المحور X
            Dim labels As New List(Of String)()
            For Each row As DataRow In monthlyData.Rows
                labels.Add(row("MonthName").ToString())
            Next

            ' إعداد المحور X
            chartMonthlyTrend.AxisX.Add(New Axis() With {
                .Labels = labels,
                .Separator = New Separator() With {.Step = 1}
            })

            ' إعداد المحور Y
            chartMonthlyTrend.AxisY.Add(New Axis() With {
                .MinValue = 0,
                .LabelFormatter = Function(value) value.ToString("N0")
            })

            ' إعداد القيم
            For Each row As DataRow In monthlyData.Rows
                lineSeries.Values.Add(Convert.ToDouble(row("Count")))
            Next

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث رسم الاتجاه الشهري: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تعيين حالة التحميل
    ''' </summary>
    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(Sub() SetLoadingState(isLoading))
                Return
            End If

            ' تحديث النصوص
            lblStatus.Visible = isLoading
            If isLoading Then
                lblStatus.Text = "جاري تحميل البيانات الإحصائية..."
            Else
                lblStatus.Text = "تم تحميل البيانات بنجاح"
            End If

            ' تحديث الواجهة
            Me.Refresh()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين حالة التحميل: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر الإغلاق
    ''' </summary>
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Try
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في إغلاق النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة مؤقت التحديث
    ''' </summary>
    Private Sub TimerRefresh_Tick(sender As Object, e As EventArgs) Handles TimerRefresh.Tick
        Try
            ' تحديث البيانات كل 5 دقائق
            If isDataLoaded Then
                LoadDashboardData()
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث البيانات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تنظيف الموارد عند إغلاق النموذج
    ''' </summary>
    Private Sub DashboardForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            ' إيقاف المؤقت
            TimerRefresh.Stop()

            ' تفريغ البيانات
            statisticsData = Nothing
            foldersData = Nothing
            monthlyData = Nothing

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تنظيف الموارد: {ex.Message}")
        End Try
    End Sub

    Private Sub lblOutgoingCount_Click(sender As Object, e As EventArgs) Handles lblOutgoingCount.Click

        ' إذا كان النموذج مغلقًا، قم بفتحه
        Dim main As MainForm = TryCast(Application.OpenForms("MainForm"), MainForm)
        If main IsNot Nothing Then
            Dim newsaderForm As New SaderForm()
            newsaderForm.MdiParent = main
            newsaderForm.Show()
        Else
            MessageBox.Show("لا يمكن العثور على الفورم الرئيسي")
        End If
    End Sub

    Private Sub lblDeletedCount_Click(sender As Object, e As EventArgs) Handles lblDeletedCount.Click
        ' إذا كان النموذج مغلقًا، قم بفتحه
        Dim main As MainForm = TryCast(Application.OpenForms("MainForm"), MainForm)
        If main IsNot Nothing Then
            Dim newdeletedArchiveForm As New DeletedArchiveForm()
            newdeletedArchiveForm.MdiParent = main
            newdeletedArchiveForm.Show()
        Else
            MessageBox.Show("لا يمكن العثور على الفورم الرئيسي")
        End If
    End Sub

    Private Sub lblUnarchivedCount_Click(sender As Object, e As EventArgs) Handles lblUnarchivedCount.Click
        ' إذا كان النموذج مغلقًا، قم بفتحه
        Dim main As MainForm = TryCast(Application.OpenForms("MainForm"), MainForm)
        If main IsNot Nothing Then
            Dim newunarchivedRecordsForm As New UnarchivedRecordsForm()
            newunarchivedRecordsForm.MdiParent = main
            newunarchivedRecordsForm.Show()
        Else
            MessageBox.Show("لا يمكن العثور على الفورم الرئيسي")
        End If
    End Sub

    Private Sub lblArchivedCount_Click(sender As Object, e As EventArgs) Handles lblArchivedCount.Click
        ' إذا كان النموذج مغلقًا، قم بفتحه
        Dim main As MainForm = TryCast(Application.OpenForms("MainForm"), MainForm)
        If main IsNot Nothing Then
            Dim newArchiveForm As New ArchiveForm()
            newArchiveForm.MdiParent = main
            newArchiveForm.Show()
        Else
            MessageBox.Show("لا يمكن العثور على الفورم الرئيسي")
        End If
    End Sub
End Class