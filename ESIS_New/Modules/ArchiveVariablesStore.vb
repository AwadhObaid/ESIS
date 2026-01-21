Imports System.Data
Imports System.Data.SqlClient
Imports ALquhaliLibrary.DBCLS

Public Module ArchiveVariablesStore


    Private Function ToTinyInt(userId As Integer) As Byte
        If userId < 0 OrElse userId > 255 Then
            Throw New Exception("LoggedInUserID أكبر من 255 بينما عمود ID في ArchiveVariables نوعه tinyint. غيّر ID إلى INT أو اجعل رقم المستخدم <=255.")
        End If
        Return CByte(userId)
    End Function

    Public Sub LoadForUser(userId As Integer,
                           ByRef ExternalSoredPath As String,
                           ByRef ScandImagePath As String,
                           ByRef ScandImageName As String,
                           ByRef ImageExtensionIndex As Integer,
                           ByRef PDFStoredPath As String,
                           ByRef DefualtSenderEmail As String,
                           ByRef EmailAppPassword As String,
                           ByRef SMTPServer As String,
                           ByRef PortNumber As String,
                           ByRef FrmConStrPASS_Decrypted As String,
                           ByRef PathSearch As String,
                           ByRef LastSaderNumber As String,
                           ByRef daysToKeep As Integer,
                           ByRef NotificationsFormInterval As Integer)

        Dim uid As Byte = ToTinyInt(userId)

        Using con = GetConnection()
            Using cmd As New SqlCommand("SELECT TOP 1 * FROM dbo.ArchiveVariables WHERE ID=@UserID", con)
                cmd.Parameters.AddWithValue("@UserID", uid) ' ✅ هذا هو سبب خطأ @UserID إذا كان مفقود
                Dim dt As New DataTable()
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using

                If dt.Rows.Count = 0 Then
                    ' لا يوجد سجل لهذا المستخدم
                    ExternalSoredPath = ""
                    ScandImagePath = ""
                    ScandImageName = "ScanImage"
                    ImageExtensionIndex = 0
                    PDFStoredPath = ""

                    DefualtSenderEmail = ""
                    EmailAppPassword = ""
                    SMTPServer = ""
                    PortNumber = ""
                    FrmConStrPASS_Decrypted = ""

                    PathSearch = ""
                    LastSaderNumber = ""
                    daysToKeep = 0
                    NotificationsFormInterval = 5
                    Return
                End If

                Dim r = dt.Rows(0)
                ExternalSoredPath = If(IsDBNull(r("ExternalSoredPath")), "", r("ExternalSoredPath").ToString())
                ScandImagePath = If(IsDBNull(r("ScandImagePath")), "", r("ScandImagePath").ToString())
                ScandImageName = If(IsDBNull(r("ScandImageName")), "ScanImage", r("ScandImageName").ToString())
                ImageExtensionIndex = If(IsDBNull(r("ImageExtension")), 0, CInt(r("ImageExtension")))
                PDFStoredPath = If(IsDBNull(r("PDFStoredPath")), "", r("PDFStoredPath").ToString())

                DefualtSenderEmail = If(IsDBNull(r("DefualtSenderEmail")), "", r("DefualtSenderEmail").ToString())
                EmailAppPassword = If(IsDBNull(r("EmailAppPassword")), "", r("EmailAppPassword").ToString())
                SMTPServer = If(IsDBNull(r("SMTPServer")), "", r("SMTPServer").ToString())
                PortNumber = If(IsDBNull(r("PortNumber")), "", r("PortNumber").ToString())

                Dim enc = If(IsDBNull(r("FrmConStrPASS")), "", r("FrmConStrPASS").ToString())
                FrmConStrPASS_Decrypted = If(String.IsNullOrWhiteSpace(enc), "", Decrypt(enc))

                PathSearch = If(IsDBNull(r("PathSearch")), "", r("PathSearch").ToString())
                LastSaderNumber = If(IsDBNull(r("LastSaderNumber")), "", r("LastSaderNumber").ToString())
                daysToKeep = If(IsDBNull(r("daysToKeep")), 0, CInt(r("daysToKeep")))
                NotificationsFormInterval = If(IsDBNull(r("NotificationsFormInterval")), 5, CInt(r("NotificationsFormInterval")))
            End Using
        End Using
    End Sub

    Public Sub SaveForUser(userId As Integer,
                           ExternalSoredPath As String,
                           ScandImagePath As String,
                           ScandImageName As String,
                           ImageExtensionIndex As Integer,
                           PDFStoredPath As String,
                           DefualtSenderEmail As String,
                           EmailAppPassword As String,
                           SMTPServer As String,
                           PortNumber As String,
                           FrmConStrPASS_Encrypted As String,
                           PathSearch As String,
                           LastSaderNumber As String,
                           daysToKeep As Integer,
                           NotificationsFormInterval As Integer)

        Dim uid As Byte = ToTinyInt(userId)

        Dim sql As String =
"IF EXISTS (SELECT 1 FROM dbo.ArchiveVariables WHERE ID=@UserID)
BEGIN
    UPDATE dbo.ArchiveVariables SET
        ExternalSoredPath=@ExternalSoredPath,
        ScandImagePath=@ScandImagePath,
        ScandImageName=@ScandImageName,
        ImageExtension=@ImageExtension,
        PDFStoredPath=@PDFStoredPath,

        DefualtSenderEmail=@DefualtSenderEmail,
        EmailAppPassword=@EmailAppPassword,
        SMTPServer=@SMTPServer,
        PortNumber=@PortNumber,
        FrmConStrPASS=@FrmConStrPASS,

        PathSearch=@PathSearch,
        LastSaderNumber=@LastSaderNumber,
        daysToKeep=@daysToKeep,
        NotificationsFormInterval=@NotificationsFormInterval,

        EditDate=GETDATE(),
        EditUser=@EditUser
    WHERE ID=@UserID
END
ELSE
BEGIN
    INSERT INTO dbo.ArchiveVariables
    (ID, ExternalSoredPath, ScandImagePath, ScandImageName, ImageExtension, PDFStoredPath,
     DefualtSenderEmail, EmailAppPassword, SMTPServer, PortNumber, FrmConStrPASS,
     PathSearch, LastSaderNumber, daysToKeep, NotificationsFormInterval, AddDate, AddUser)
    VALUES
    (@UserID, @ExternalSoredPath, @ScandImagePath, @ScandImageName, @ImageExtension, @PDFStoredPath,
     @DefualtSenderEmail, @EmailAppPassword, @SMTPServer, @PortNumber, @FrmConStrPASS,
     @PathSearch, @LastSaderNumber, @daysToKeep, @NotificationsFormInterval, GETDATE(), @AddUser)
END"

        Using con = GetConnection()
            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@UserID", uid)

                cmd.Parameters.AddWithValue("@ExternalSoredPath", If(String.IsNullOrWhiteSpace(ExternalSoredPath), DBNull.Value, ExternalSoredPath.Trim()))
                cmd.Parameters.AddWithValue("@ScandImagePath", If(String.IsNullOrWhiteSpace(ScandImagePath), DBNull.Value, ScandImagePath.Trim()))
                cmd.Parameters.AddWithValue("@ScandImageName", If(String.IsNullOrWhiteSpace(ScandImageName), DBNull.Value, ScandImageName.Trim()))
                cmd.Parameters.AddWithValue("@ImageExtension", If(ImageExtensionIndex < 0, DBNull.Value, ImageExtensionIndex))
                cmd.Parameters.AddWithValue("@PDFStoredPath", If(String.IsNullOrWhiteSpace(PDFStoredPath), DBNull.Value, PDFStoredPath.Trim()))

                cmd.Parameters.AddWithValue("@DefualtSenderEmail", If(String.IsNullOrWhiteSpace(DefualtSenderEmail), DBNull.Value, DefualtSenderEmail.Trim()))
                cmd.Parameters.AddWithValue("@EmailAppPassword", If(String.IsNullOrWhiteSpace(EmailAppPassword), DBNull.Value, EmailAppPassword))
                cmd.Parameters.AddWithValue("@SMTPServer", If(String.IsNullOrWhiteSpace(SMTPServer), DBNull.Value, SMTPServer.Trim()))
                cmd.Parameters.AddWithValue("@PortNumber", If(String.IsNullOrWhiteSpace(PortNumber), DBNull.Value, PortNumber.Trim()))
                cmd.Parameters.AddWithValue("@FrmConStrPASS", If(String.IsNullOrWhiteSpace(FrmConStrPASS_Encrypted), DBNull.Value, FrmConStrPASS_Encrypted))

                cmd.Parameters.AddWithValue("@PathSearch", If(String.IsNullOrWhiteSpace(PathSearch), DBNull.Value, PathSearch.Trim()))
                cmd.Parameters.AddWithValue("@LastSaderNumber", If(String.IsNullOrWhiteSpace(LastSaderNumber), DBNull.Value, LastSaderNumber.Trim()))
                cmd.Parameters.AddWithValue("@daysToKeep", daysToKeep)
                cmd.Parameters.AddWithValue("@NotificationsFormInterval", NotificationsFormInterval)

                cmd.Parameters.AddWithValue("@AddUser", uid)
                cmd.Parameters.AddWithValue("@EditUser", uid)

                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

End Module
