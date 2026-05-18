Imports System.Data.SqlClient

Public Class AlertsClass

#Region "📥 Get All Alerts"
    Public Function GetAllAlerts() As DataTable

        Dim dt As New DataTable()

        Using con = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("
            SELECT 
                A.ID,
                A.Title,
                A.Description,
                A.AlertDate,
                A.AlertTime,
                A.AlertType,
                A.DayOfWeek,
                A.DayOfMonth,
                A.IsActive,
                A.RepeatInterval,
                U.UserName
            FROM TbAlerts A
            LEFT JOIN TbUsers U ON A.UserID = U.ID
            ORDER BY A.ID DESC", con)

                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)

            End Using
        End Using

        Return dt

    End Function

    Public Function GetAlertsStats() As DataTable

        Using con = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("
        SELECT 
            COUNT(*) AS Total,
            SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) AS Active,
            SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END) AS Inactive
        FROM TbAlerts", con)

                Dim dt As New DataTable()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)

                Return dt

            End Using
        End Using

    End Function
#End Region


#Region "➕ Insert"

    Public Sub InsertAlert(title As String,
                       description As String,
                       alertType As String,
                       alertDate As Date?,
                       dayOfWeek As Integer?,
                       dayOfMonth As Integer?,
                       userId As Integer,
                       isActive As Boolean,
                       alertTime As TimeSpan?,
                       repeatInterval As Integer?)

        Using con As SqlConnection = DatabaseHelper.GetConnection()

            Using cmd As New SqlCommand("
        INSERT INTO TbAlerts
        (Title, Description, AlertType, AlertDate, AlertTime,
         DayOfWeek, DayOfMonth, UserID, IsActive, RepeatInterval)
        VALUES
        (@Title, @Description, @Type, @Date, @AlertTime,
         @DayOfWeek, @DayOfMonth, @UserID, @IsActive, @RepeatInterval)", con)

                cmd.Parameters.AddWithValue("@Title", title)
                cmd.Parameters.AddWithValue("@Description", description)
                cmd.Parameters.AddWithValue("@Type", alertType)

                cmd.Parameters.AddWithValue("@Date",
                If(alertDate.HasValue, alertDate.Value, DBNull.Value))

                cmd.Parameters.AddWithValue("@AlertTime",
                If(alertTime.HasValue, alertTime.Value, DBNull.Value))

                cmd.Parameters.AddWithValue("@DayOfWeek",
                If(dayOfWeek.HasValue, dayOfWeek.Value, DBNull.Value))

                cmd.Parameters.AddWithValue("@DayOfMonth",
                If(dayOfMonth.HasValue, dayOfMonth.Value, DBNull.Value))

                cmd.Parameters.AddWithValue("@UserID", userId)
                cmd.Parameters.AddWithValue("@IsActive", isActive)

                cmd.Parameters.AddWithValue("@RepeatInterval",
                If(repeatInterval.HasValue, repeatInterval.Value, DBNull.Value))

                con.Open()
                cmd.ExecuteNonQuery()

            End Using
        End Using

    End Sub


#End Region

#Region "✏️ Update"

    Public Sub UpdateAlert(id As Integer,
                       title As String,
                       description As String,
                       alertType As String,
                       alertDate As Date?,
                       dayOfWeek As Integer?,
                       dayOfMonth As Integer?,
                       isActive As Boolean,
                       alertTime As TimeSpan?,
                       repeatInterval As Integer?)

        Using con As SqlConnection = DatabaseHelper.GetConnection()

            Using cmd As New SqlCommand("
        UPDATE TbAlerts SET
            Title = @Title,
            Description = @Description,
            AlertType = @Type,
            AlertDate = @Date,
            AlertTime = @AlertTime,
            DayOfWeek = @DayOfWeek,
            DayOfMonth = @DayOfMonth,
            IsActive = @IsActive,
            RepeatInterval = @RepeatInterval
        WHERE ID = @ID", con)

                cmd.Parameters.AddWithValue("@ID", id)
                cmd.Parameters.AddWithValue("@Title", title)
                cmd.Parameters.AddWithValue("@Description", description)
                cmd.Parameters.AddWithValue("@Type", alertType)

                cmd.Parameters.AddWithValue("@Date",
                If(alertDate.HasValue, alertDate.Value, DBNull.Value))

                cmd.Parameters.AddWithValue("@AlertTime",
                If(alertTime.HasValue, alertTime.Value, DBNull.Value))

                cmd.Parameters.AddWithValue("@DayOfWeek",
                If(dayOfWeek.HasValue, dayOfWeek.Value, DBNull.Value))

                cmd.Parameters.AddWithValue("@DayOfMonth",
                If(dayOfMonth.HasValue, dayOfMonth.Value, DBNull.Value))

                cmd.Parameters.AddWithValue("@IsActive", isActive)

                cmd.Parameters.AddWithValue("@RepeatInterval",
                If(repeatInterval.HasValue, repeatInterval.Value, DBNull.Value))

                con.Open()
                cmd.ExecuteNonQuery()

            End Using
        End Using

    End Sub

    'Public Sub UpdateAlert(id As Integer,
    '                   title As String,
    '                   description As String,
    '                   alertType As String,
    '                   alertDate As Date?,
    '                   dayOfWeek As Integer?,
    '                   dayOfMonth As Integer?,
    '                   isActive As Boolean,
    '                   alertTime As TimeSpan?,
    '                   isRepeating As Boolean)

    '    Using con As SqlConnection = DatabaseHelper.GetConnection()
    '        Using cmd As New SqlCommand("
    '                                        UPDATE TbAlerts SET
    '                                            Title = @Title,
    '                                            Description = @Description,
    '                                            AlertType = @Type,
    '                                            AlertDate = @Date,
    '                                            AlertTime = @AlertTime,
    '                                            DayOfWeek = @DayOfWeek,
    '                                            DayOfMonth = @DayOfMonth,
    '                                            IsActive = @IsActive,
    '                                            isRepeating = @isRepeating
    '                                        WHERE ID = @ID", con)

    '            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id

    '            cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = title
    '            cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = description
    '            cmd.Parameters.Add("@Type", SqlDbType.NVarChar).Value = alertType

    '            cmd.Parameters.Add("@Date", SqlDbType.Date).Value =
    '            If(alertDate.HasValue, alertDate.Value.Date, DBNull.Value)

    '            cmd.Parameters.Add("@AlertTime", SqlDbType.Time).Value =
    '            If(alertTime.HasValue, alertTime.Value, DBNull.Value)

    '            cmd.Parameters.Add("@DayOfWeek", SqlDbType.Int).Value =
    '            If(dayOfWeek.HasValue, dayOfWeek.Value, DBNull.Value)

    '            cmd.Parameters.Add("@DayOfMonth", SqlDbType.Int).Value =
    '            If(dayOfMonth.HasValue, dayOfMonth.Value, DBNull.Value)

    '            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive

    '            'التكرار
    '            cmd.Parameters.Add("@IsRepeating", SqlDbType.Bit).Value = isRepeating

    '            con.Open()
    '            cmd.ExecuteNonQuery()

    '        End Using
    '    End Using

    'End Sub
#End Region

#Region "🗑 Delete"
    Public Sub DeleteAlert(id As Integer)

        Using con = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("DELETE FROM TbAlerts WHERE ID=@ID", con)

                cmd.Parameters.AddWithValue("@ID", id)

                con.Open()
                cmd.ExecuteNonQuery()

            End Using
        End Using

    End Sub
#End Region

#Region "🔔 Get Due Alerts"
    Public Function GetDueAlerts() As DataTable

        Dim dt As New DataTable()

        Using con = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("GetDueAlerts", con)

                cmd.CommandType = CommandType.StoredProcedure

                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)

            End Using
        End Using

        Return dt

    End Function
#End Region

#Region "✔ Mark As Shown"
    Public Sub MarkAlertAsShown(id As Integer)

        Using con As SqlConnection = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("MarkAlertAsShown", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@ID", id)

                con.Open()
                cmd.ExecuteNonQuery()

            End Using
        End Using

    End Sub
#End Region

#Region "👤 Users"
    Public Function GetUsers() As DataTable

        Dim dt As New DataTable()

        Using con = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("SELECT ID, UserName FROM TbUsers", con)

                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)

            End Using
        End Using

        Return dt

    End Function
#End Region

End Class


'Imports System.Data.SqlClient

'Public Class AlertsClass

'    ' جلب التنبيهات المستحقة اليوم
'    Public Function GetDueAlerts() As DataTable

'        Dim dt As New DataTable()

'        Using con = DatabaseHelper.GetConnection()
'            Using cmd As New SqlCommand("GetDueAlerts", con)
'                cmd.CommandType = CommandType.StoredProcedure

'                Dim da As New SqlDataAdapter(cmd)
'                da.Fill(dt)
'            End Using
'        End Using

'        Return dt

'    End Function

'    ' تحديث آخر عرض للتنبيه
'    Public Sub MarkAlertAsShown(id As Integer)

'        Using con As SqlConnection = DatabaseHelper.GetConnection()
'            Using cmd As New SqlCommand("MarkAlertAsShown", con)

'                cmd.CommandType = CommandType.StoredProcedure
'                cmd.Parameters.AddWithValue("@ID", id)

'                con.Open()
'                cmd.ExecuteNonQuery()

'            End Using
'        End Using

'    End Sub

'    'Public Function GetAlerts() As DataTable

'    '    Dim dt As New DataTable()

'    '    Using con = DatabaseHelper.GetConnection()
'    '        Using cmd As New SqlCommand("SELECT * FROM TbAlerts", con)
'    '            Dim da As New SqlDataAdapter(cmd)
'    '            da.Fill(dt)
'    '        End Using
'    '    End Using

'    '    Return dt

'    'End Function

'    'Public Sub AddAlert(title As String, desc As String, alertDate As Date,
'    '                    isMonthly As Boolean, dayOfMonth As Integer, userId As Object)

'    '    Using con = DatabaseHelper.GetConnection()

'    '        Using cmd As New SqlCommand("
'    '        INSERT INTO TbAlerts (Title, Description, AlertDate, IsMonthly, DayOfMonth, UserID)
'    '        VALUES (@Title, @Desc, @Date, @Monthly, @Day, @UserID)", con)

'    '            cmd.Parameters.AddWithValue("@Title", title)
'    '            cmd.Parameters.AddWithValue("@Desc", desc)
'    '            cmd.Parameters.AddWithValue("@Date", alertDate)
'    '            cmd.Parameters.AddWithValue("@Monthly", isMonthly)
'    '            cmd.Parameters.AddWithValue("@Day", dayOfMonth)
'    '            cmd.Parameters.AddWithValue("@UserID", userId)

'    '            con.Open()
'    '            cmd.ExecuteNonQuery()

'    '        End Using

'    '    End Using

'    'End Sub

'    Public Sub InsertAlert(title As String,
'                           description As String,
'                           alertType As String,
'                           alertDate As Date?,
'                           dayOfWeek As Integer?,
'                           dayOfMonth As Integer?,
'                           userId As Integer)

'        Using con As SqlConnection = DatabaseHelper.GetConnection()
'            Using cmd As New SqlCommand("
'            INSERT INTO Alerts
'            (Title, Description, AlertType, AlertDate, DayOfWeek, DayOfMonth, UserID)
'            VALUES
'            (@Title, @Description, @Type, @Date, @DayOfWeek, @DayOfMonth, @UserID)", con)

'                cmd.Parameters.AddWithValue("@Title", title)
'                cmd.Parameters.AddWithValue("@Description", description)
'                cmd.Parameters.AddWithValue("@Type", alertType)

'                cmd.Parameters.AddWithValue("@Date",
'                    If(alertDate.HasValue, alertDate.Value, DBNull.Value))

'                cmd.Parameters.AddWithValue("@DayOfWeek",
'                    If(dayOfWeek.HasValue, dayOfWeek.Value, DBNull.Value))

'                cmd.Parameters.AddWithValue("@DayOfMonth",
'                    If(dayOfMonth.HasValue, dayOfMonth.Value, DBNull.Value))

'                cmd.Parameters.AddWithValue("@UserID", userId)

'                con.Open()
'                cmd.ExecuteNonQuery()

'            End Using
'        End Using

'    End Sub
'    Public Sub DeleteAlert(id As Integer)

'        Using con = DatabaseHelper.GetConnection()

'            Using cmd As New SqlCommand("DELETE FROM TbAlerts WHERE ID=@ID", con)
'                cmd.Parameters.AddWithValue("@ID", id)
'                con.Open()
'                cmd.ExecuteNonQuery()
'            End Using

'        End Using

'    End Sub

'    Public Function GetUsers() As DataTable

'        Dim dt As New DataTable()

'        Using con = DatabaseHelper.GetConnection()
'            Using cmd As New SqlCommand("SELECT ID, UserName FROM TbUsers", con)
'                Dim da As New SqlDataAdapter(cmd)
'                da.Fill(dt)
'            End Using
'        End Using

'        Return dt

'    End Function

'End Class