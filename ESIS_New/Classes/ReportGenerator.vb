Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

''' <summary>
''' كلاس مركزي لإنشاء وإدارة التقارير
''' </summary>
Public Class ReportGenerator
    Private _reportDocument As ReportDocument
    Private _reportType As ReportType
    Private _criteria As ReportCriteria

    ''' <summary>
    ''' أنواع التقارير المدعومة
    ''' </summary>
    Public Enum ReportType
        OutgoingRecords ' السجلات الصادرة
        ArchivedRecords ' السجلات المؤرشفة
        UnarchivedRecords ' السجلات غير المؤرشفة
        DeletedRecords ' السجلات المحذوفة
        Custom ' تقرير مخصص
    End Enum

    ''' <summary>
    ''' فئة لتخزين معايير التقرير
    ''' </summary>
    Public Class ReportCriteria
        Public Property StartDate As DateTime = DateTime.Now.AddMonths(-1)
        Public Property EndDate As DateTime = DateTime.Now
        Public Property ArchiveFolder As String = String.Empty
        Public Property BookCategory As String = String.Empty
        Public Property WillMaster As String = String.Empty
        Public Property WillSub As String = String.Empty
        Public Property SelectedIDs As List(Of Integer)
        Public Property ReportTitle As String = "تقرير"
    End Class

    Public Sub New()
        _criteria = New ReportCriteria()
    End Sub

    ''' <summary>
    ''' تعيين نوع التقرير
    ''' </summary>
    Public Sub SetReportType(reportType As ReportType)
        _reportType = reportType
    End Sub

    ''' <summary>
    ''' تعيين معايير التقرير
    ''' </summary>
    Public Sub SetReportCriteria(criteria As ReportCriteria)
        _criteria = criteria
    End Sub

    ''' <summary>
    ''' إنشاء التقرير
    ''' </summary>
    Public Function GenerateReport() As ReportDocument
        Try
            Select Case _reportType
                Case ReportType.OutgoingRecords
                    Return GenerateOutgoingRecordsReport()
                Case ReportType.ArchivedRecords
                    Return GenerateArchivedRecordsReport()
                Case ReportType.UnarchivedRecords
                    Return GenerateUnarchivedRecordsReport()
                Case ReportType.DeletedRecords
                    Return GenerateDeletedRecordsReport()
                Case ReportType.Custom
                    Return GenerateCustomReport()
                Case Else
                    Throw New ArgumentException("نوع التقرير غير معروف")
            End Select
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء التقرير: {ex.Message}")
            Throw New Exception($"فشل في إنشاء التقرير: {ex.Message}")
        End Try
    End Function

    ''' <summary>
    ''' إنشاء تقرير السجلات الصادرة
    ''' </summary>
    Private Function GenerateOutgoingRecordsReport() As ReportDocument
        Try
            ' تحديد مسار ملف التقرير
            Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "OutgoingRecordsReport.rpt")
            If Not File.Exists(reportPath) Then
                Throw New FileNotFoundException("لم يتم العثور على ملف تقرير السجلات الصادرة", reportPath)
            End If

            ' تحميل التقرير
            _reportDocument = New ReportDocument()
            _reportDocument.Load(reportPath)

            ' تعيين معايير التقرير
            SetReportParameters()
            ApplyDatabaseLogon()
            ApplyRecordSelectionFormula()

            Return _reportDocument
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء تقرير السجلات الصادرة: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' إنشاء تقرير السجلات المؤرشفة
    ''' </summary>
    Private Function GenerateArchivedRecordsReport() As ReportDocument
        Try
            ' تحديد مسار ملف التقرير
            Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "ArchivedRecordsReport.rpt")
            If Not File.Exists(reportPath) Then
                Throw New FileNotFoundException("لم يتم العثور على ملف تقرير السجلات المؤرشفة", reportPath)
            End If

            ' تحميل التقرير
            _reportDocument = New ReportDocument()
            _reportDocument.Load(reportPath)

            ' تعيين معايير التقرير
            SetReportParameters()
            ApplyDatabaseLogon()
            ApplyRecordSelectionFormula()

            Return _reportDocument
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء تقرير السجلات المؤرشفة: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' إنشاء تقرير السجلات غير المؤرشفة
    ''' </summary>
    Private Function GenerateUnarchivedRecordsReport() As ReportDocument
        Try
            ' تحديد مسار ملف التقرير
            Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "UnarchivedRecordsReport.rpt")
            If Not File.Exists(reportPath) Then
                Throw New FileNotFoundException("لم يتم العثور على ملف تقرير السجلات غير المؤرشفة", reportPath)
            End If

            ' تحميل التقرير
            _reportDocument = New ReportDocument()
            _reportDocument.Load(reportPath)

            ' تعيين معايير التقرير
            SetReportParameters()
            ApplyDatabaseLogon()
            ApplyRecordSelectionFormula()

            Return _reportDocument
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء تقرير السجلات غير المؤرشفة: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' إنشاء تقرير السجلات المحذوفة
    ''' </summary>
    Private Function GenerateDeletedRecordsReport() As ReportDocument
        Try
            ' تحديد مسار ملف التقرير
            Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "DeletedRecordsReport.rpt")
            If Not File.Exists(reportPath) Then
                Throw New FileNotFoundException("لم يتم العثور على ملف تقرير السجلات المحذوفة", reportPath)
            End If

            ' تحميل التقرير
            _reportDocument = New ReportDocument()
            _reportDocument.Load(reportPath)

            ' تعيين معايير التقرير
            SetReportParameters()
            ApplyDatabaseLogon()
            ApplyRecordSelectionFormula()

            Return _reportDocument
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء تقرير السجلات المحذوفة: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' إنشاء تقرير مخصص
    ''' </summary>
    Private Function GenerateCustomReport() As ReportDocument
        Try
            ' تحديد مسار ملف التقرير
            Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "CustomReport.rpt")
            If Not File.Exists(reportPath) Then
                Throw New FileNotFoundException("لم يتم العثور على ملف تقرير مخصص", reportPath)
            End If

            ' تحميل التقرير
            _reportDocument = New ReportDocument()
            _reportDocument.Load(reportPath)

            ' تعيين معايير التقرير
            SetReportParameters()
            ApplyDatabaseLogon()
            ApplyRecordSelectionFormula()

            Return _reportDocument
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء تقرير مخصص: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' تطبيق بيانات تسجيل الدخول لقاعدة البيانات
    ''' </summary>
    Private Sub ApplyDatabaseLogon()
        Try
            ' استخدام الإعدادات من ملف الإعدادات
            Dim serverName As String = My.Settings.ServerName
            Dim databaseName As String = My.Settings.DataBaseName
            Dim userID As String = My.Settings.UserName
            Dim password As String = My.Settings.Password
            Dim integratedSecurity As Boolean = My.Settings.UseIntegratedSecurity

            For Each table As Table In _reportDocument.Database.Tables
                Dim tableLogOnInfo As TableLogOnInfo = table.LogOnInfo
                Dim connectionInfo As New CrystalDecisions.Shared.ConnectionInfo()

                ' تعيين معلومات الاتصال الأساسية
                connectionInfo.ServerName = serverName
                connectionInfo.DatabaseName = databaseName

                ' التعامل مع المصادقة المتكاملة (Integrated Security)
                If integratedSecurity Then
                    ' في حالة المصادقة المتكاملة، لا نقوم بتعيين اسم المستخدم وكلمة المرور
                    ' Crystal Reports ستستخدم اتصال Windows تلقائيًا
                    ' لا حاجة لتعيين خاصية IntegratedSecurity
                Else
                    ' في حالة المصادقة العادية، نقوم بتعيين اسم المستخدم وكلمة المرور
                    connectionInfo.UserID = userID
                    connectionInfo.Password = password
                End If

                tableLogOnInfo.ConnectionInfo = connectionInfo
                table.ApplyLogOnInfo(tableLogOnInfo)
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق بيانات تسجيل الدخول: {ex.Message}")
            Throw
        End Try
    End Sub

    '''' <summary>
    '''' تطبيق بيانات تسجيل الدخول لقاعدة البيانات
    '''' </summary>
    'Private Sub ApplyDatabaseLogon()
    '    Try
    '        ' استخدام الإعدادات من ملف الإعدادات
    '        Dim serverName As String = My.Settings.ServerName
    '        Dim databaseName As String = My.Settings.DataBaseName
    '        Dim userID As String = My.Settings.UserName
    '        Dim password As String = My.Settings.Password
    '        Dim integratedSecurity As Boolean = My.Settings.UseIntegratedSecurity

    '        For Each table As Table In _reportDocument.Database.Tables
    '            Dim tableLogOnInfo As TableLogOnInfo = table.LogOnInfo
    '            Dim connectionInfo As New CrystalDecisions.Shared.ConnectionInfo()

    '            ' بناء سلسلة الاتصال
    '            Dim connectionString As String = ""

    '            ' تعيين سلسلة الاتصال بناءً على نوع المصادقة
    '            If integratedSecurity Then
    '                connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=SSPI;"
    '            Else
    '                connectionString = $"Data Source={serverName};Initial Catalog={databaseName};User ID={userID};Password={password};"
    '            End If

    '            ' تعيين سلسلة الاتصال
    '            connectionInfo.Attributes.CollectionString = connectionString

    '            ' تطبيق معلومات الاتصال
    '            tableLogOnInfo.ConnectionInfo = connectionInfo
    '            table.ApplyLogOnInfo(tableLogOnInfo)

    '            ' التحقق من تطبيق الاتصال بنجاح
    '            Dim testConnection As Boolean = table.TestConnectivity()
    '            If Not testConnection Then
    '                System.Diagnostics.Debug.WriteLine($"فشل في الاتصال بجدول {table.Name} باستخدام: {connectionString}")
    '            End If
    '        Next
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق بيانات تسجيل الدخول: {ex.Message}")
    '        Throw
    '    End Try
    'End Sub

    ''' <summary>
    ''' تعيين معلمة إذا كانت موجودة في التقرير
    ''' </summary>
    Private Sub SetParameterIfExists(parameterName As String, parameterValue As Object)
        Try
            ' التحقق من وجود المعلمة في التقرير باستخدام الطريقة الصحيحة
            If ParameterExists(parameterName) Then
                _reportDocument.SetParameterValue(parameterName, parameterValue)
            Else
                System.Diagnostics.Debug.WriteLine($"المعلمة {parameterName} غير موجودة في التقرير")
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين المعلمة {parameterName}: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من وجود معلمة في التقرير
    ''' </summary>
    Private Function ParameterExists(parameterName As String) As Boolean
        Try
            ' الطريقة 1: استخدام Item ومحاولة الوصول إلى المعلمة
            Try
                Dim parameterField As ParameterFieldDefinition = _reportDocument.DataDefinition.ParameterFields.Item(parameterName)
                Return parameterField IsNot Nothing
            Catch ex As Exception
                ' إذا لم تكن المعلمة موجودة، سيتم رمي استثناء
                Return False
            End Try

            ' الطريقة 2: التكرار عبر جميع المعلمات (بدائل)
            'For i As Integer = 0 To _reportDocument.DataDefinition.ParameterFields.Count - 1
            '    If String.Equals(_reportDocument.DataDefinition.ParameterFields(i).Name, parameterName, StringComparison.OrdinalIgnoreCase) Then
            '        Return True
            '    End If
            'Next
            'Return False
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من وجود المعلمة {parameterName}: {ex.Message}")
            Return False
        End Try
    End Function
    '''' <summary>
    '''' تعيين معايير التقرير كمعلمات
    '''' </summary>
    'Private Sub SetReportParameters()
    '    Try
    '        ' تعيين معايير التواريخ
    '        _reportDocument.SetParameterValue("StartDate", _criteria.StartDate)
    '        _reportDocument.SetParameterValue("EndDate", _criteria.EndDate)
    '        _reportDocument.SetParameterValue("ReportTitle", _criteria.ReportTitle)
    '        _reportDocument.SetParameterValue("ReportDate", DateTime.Now.ToString("dd/MM/yyyy"))

    '        ' تعيين معايير إضافية حسب نوع التقرير
    '        Select Case _reportType
    '            Case ReportType.OutgoingRecords, ReportType.UnarchivedRecords
    '                If Not String.IsNullOrEmpty(_criteria.BookCategory) Then
    '                    _reportDocument.SetParameterValue("BookCategory", _criteria.BookCategory)
    '                End If
    '                If Not String.IsNullOrEmpty(_criteria.WillMaster) Then
    '                    _reportDocument.SetParameterValue("WillMaster", _criteria.WillMaster)
    '                End If
    '                If Not String.IsNullOrEmpty(_criteria.WillSub) Then
    '                    _reportDocument.SetParameterValue("WillSub", _criteria.WillSub)
    '                End If
    '            Case ReportType.ArchivedRecords, ReportType.DeletedRecords
    '                If Not String.IsNullOrEmpty(_criteria.ArchiveFolder) Then
    '                    _reportDocument.SetParameterValue("ArchiveFolder", _criteria.ArchiveFolder)
    '                End If
    '        End Select
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تعيين معايير التقرير: {ex.Message}")
    '        Throw
    '    End Try
    'End Sub

    ''' <summary>
    ''' تعيين معايير التقرير كمعلمات مع التحقق من وجودها
    ''' </summary>
    Private Sub SetReportParameters()
        Try
            ' في حالة التقارير الصادرة
            Dim criteria As New ReportGenerator.ReportCriteria()
            criteria.StartDate = New DateTime(2023, 1, 1)
            criteria.EndDate = New DateTime(2023, 12, 31)
            criteria.BookCategory = "صادر"
            criteria.WillMaster = "وزارة الدفاع"
            criteria.WillSub = "إدارة الشؤون الإدارية"

            ' تعيين المعايير
            SetParameterIfExists("StartDate", criteria.StartDate)  ' DateTime
            SetParameterIfExists("EndDate", criteria.EndDate)    ' DateTime
            SetParameterIfExists("BookCategory", criteria.BookCategory)  ' String
            SetParameterIfExists("WillMaster", criteria.WillMaster)      ' String
            SetParameterIfExists("WillSub", criteria.WillSub)           ' String


            ' تعيين معايير التواريخ (هذه موجودة في جميع التقارير)
            SetParameterIfExists("StartDate", _criteria.StartDate)
            SetParameterIfExists("EndDate", _criteria.EndDate)
            SetParameterIfExists("ReportTitle", _criteria.ReportTitle)
            SetParameterIfExists("ReportDate", DateTime.Now.ToString("dd/MM/yyyy"))

            ' تعيين معايير إضافية حسب نوع التقرير
            Select Case _reportType
                Case ReportType.OutgoingRecords, ReportType.UnarchivedRecords
                    ' تعيين معايير خاصة بالتقارير الصادرة
                    SetParameterIfExists("BookCategory", _criteria.BookCategory)
                    SetParameterIfExists("WillMaster", _criteria.WillMaster)
                    SetParameterIfExists("WillSub", _criteria.WillSub)

                Case ReportType.ArchivedRecords, ReportType.DeletedRecords
                    ' تعيين معايير خاصة بالتقارير المؤرشفة والمحذوفة
                    SetParameterIfExists("ArchiveFolder", _criteria.ArchiveFolder)
            End Select
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين معايير التقرير: {ex.Message}")
            Throw
        End Try
    End Sub



    ''' <summary>
    ''' تطبيق صيغة اختيار السجلات مع التأكد من توافق الجداول
    ''' </summary>
    Private Sub ApplyRecordSelectionFormula()
        Try
            Dim formula As String = String.Empty

            ' تحديد الجدول الصحيح بناءً على نوع التقرير
            Dim tableName As String
            Select Case _reportType
                Case ReportType.OutgoingRecords, ReportType.UnarchivedRecords
                    tableName = "TbSader"
                Case ReportType.ArchivedRecords
                    tableName = "TbArchive"
                Case ReportType.DeletedRecords
                    tableName = "TbDeletedArchive"
                Case Else
                    tableName = "TbSader"
            End Select

            ' إضافة شرط التواريخ باستخدام المعلمات
            formula &= $"{{{tableName}.DateBook}} >= {{?StartDate}} AND {{{tableName}.DateBook}} <= {{?EndDate}}"

            ' إضافة شروط إضافية حسب نوع التقرير
            Select Case _reportType
                Case ReportType.OutgoingRecords, ReportType.UnarchivedRecords
                    If Not String.IsNullOrEmpty(_criteria.BookCategory) Then
                        formula &= $" AND {{{tableName}.BookCategory}} = '{_criteria.BookCategory}'"
                    End If
                    If Not String.IsNullOrEmpty(_criteria.WillMaster) Then
                        formula &= $" AND {{{tableName}.WillMaster}} = '{_criteria.WillMaster}'"
                    End If
                    If Not String.IsNullOrEmpty(_criteria.WillSub) Then
                        formula &= $" AND {{{tableName}.WillSub}} = '{_criteria.WillSub}'"
                    End If

                    ' للسجلات غير المؤرشفة
                    If _reportType = ReportType.UnarchivedRecords Then
                        formula &= " AND ({" & tableName & ".ArchiveID} = 0 OR IsNull({" & tableName & ".ArchiveID}))"
                    End If

                Case ReportType.ArchivedRecords, ReportType.DeletedRecords
                    If Not String.IsNullOrEmpty(_criteria.ArchiveFolder) Then
                        formula &= $" AND {{{tableName}.ArchiveFolder}} = '{_criteria.ArchiveFolder}'"
                    End If
            End Select

            ' إذا تم تحديد سجلات محددة
            If _criteria.SelectedIDs IsNot Nothing AndAlso _criteria.SelectedIDs.Count > 0 Then
                Dim ids As String = String.Join(",", _criteria.SelectedIDs)
                formula &= $" AND {{{tableName}.ID}} IN [{ids}]"
            End If

            ' تعيين صيغة اختيار السجلات
            _reportDocument.RecordSelectionFormula = formula
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق صيغة اختيار السجلات: {ex.Message}")
            Throw
        End Try
    End Sub


    '''' <summary>
    '''' تطبيق صيغة اختيار السجلات
    '''' </summary>
    'Private Sub ApplyRecordSelectionFormula()
    '    Try
    '        Dim formula As String = String.Empty

    '        ' إضافة شرط التواريخ
    '        formula &= $"{{TbSader.DateBook}} >= DateTime({Me._criteria.StartDate.Year}, {Me._criteria.StartDate.Month}, {Me._criteria.StartDate.Day}) AND " &
    '                   $"{{TbSader.DateBook}} <= DateTime({Me._criteria.EndDate.Year}, {Me._criteria.EndDate.Month}, {Me._criteria.EndDate.Day})"

    '        ' إضافة شروط إضافية حسب نوع التقرير
    '        Select Case _reportType
    '            Case ReportType.OutgoingRecords, ReportType.UnarchivedRecords
    '                If Not String.IsNullOrEmpty(_criteria.BookCategory) Then
    '                    formula &= $" AND {{TbSader.BookCategory}} = '{_criteria.BookCategory}'"
    '                End If
    '                If Not String.IsNullOrEmpty(_criteria.WillMaster) Then
    '                    formula &= $" AND {{TbSader.WillMaster}} = '{_criteria.WillMaster}'"
    '                End If
    '                If Not String.IsNullOrEmpty(_criteria.WillSub) Then
    '                    formula &= $" AND {{TbSader.WillSub}} = '{_criteria.WillSub}'"
    '                End If

    '                ' للسجلات غير المؤرشفة
    '                If _reportType = ReportType.UnarchivedRecords Then
    '                    formula &= " AND ({TbSader.ArchiveID} = 0 OR IsNull({TbSader.ArchiveID}))"
    '                End If

    '            Case ReportType.ArchivedRecords
    '                If Not String.IsNullOrEmpty(_criteria.ArchiveFolder) Then
    '                    formula &= $" AND {{TbArchive.ArchiveFolder}} = '{_criteria.ArchiveFolder}'"
    '                End If

    '            Case ReportType.DeletedRecords
    '                If Not String.IsNullOrEmpty(_criteria.ArchiveFolder) Then
    '                    formula &= $" AND {{TbDeletedArchive.ArchiveFolder}} = '{_criteria.ArchiveFolder}'"
    '                End If
    '        End Select

    '        ' إذا تم تحديد سجلات محددة
    '        If _criteria.SelectedIDs IsNot Nothing AndAlso _criteria.SelectedIDs.Count > 0 Then
    '            Dim ids As String = String.Join(",", _criteria.SelectedIDs)
    '            Select Case _reportType
    '                Case ReportType.OutgoingRecords, ReportType.UnarchivedRecords
    '                    formula &= $" AND {{TbSader.ID}} IN [{ids}]"
    '                Case ReportType.ArchivedRecords
    '                    formula &= $" AND {{TbArchive.ID}} IN [{ids}]"
    '                Case ReportType.DeletedRecords
    '                    formula &= $" AND {{TbDeletedArchive.ID}} IN [{ids}]"
    '            End Select
    '        End If

    '        ' تعيين صيغة اختيار السجلات
    '        _reportDocument.RecordSelectionFormula = formula
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق صيغة اختيار السجلات: {ex.Message}")
    '        Throw
    '    End Try
    'End Sub

    ''' <summary>
    ''' تصدير التقرير إلى ملف PDF
    ''' </summary>
    Public Function ExportToPDF(report As ReportDocument, outputPath As String) As Boolean
        Try
            ' استخدام الطريقة المدعومة لتصدير PDF
            Dim exportOpts As New ExportOptions()
            Dim diskOpts As New DiskFileDestinationOptions()
            Dim pdfOpts As ExportOptions = exportOpts.Clone()

            diskOpts.DiskFileName = outputPath
            exportOpts = report.ExportOptions

            ' تحديد خصائص التصدير
            With exportOpts
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
                .DestinationOptions = diskOpts
            End With

            ' تصدير التقرير
            report.Export()
            Return True
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تصدير التقرير إلى PDF: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' طباعة التقرير
    ''' </summary>
    Public Sub PrintReport(report As ReportDocument)
        Try
            ' استخدام الطريقة الأساسية للطباعة
            report.PrintToPrinter(1, False, 0, 0)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في طباعة التقرير: {ex.Message}")
            Throw New Exception($"فشل في طباعة التقرير: {ex.Message}")
        End Try
    End Sub
End Class