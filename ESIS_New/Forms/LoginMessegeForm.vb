Public Class LoginMessegeForm
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim FadeIn As Double
        For FadeIn = 0.0 To 1.1 Step 0.2
            Me.Opacity = FadeIn
            Me.Refresh()
            Threading.Thread.Sleep(100)
        Next
        Button1.Select()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim FadeOut As Double
        For FadeOut = 1.1 To 0.0 Step -0.2
            Me.Opacity = FadeOut
            Me.Refresh()
            Threading.Thread.Sleep(50)
        Next
        Me.Close()
    End Sub

    Private Sub iconTimer_Tick(sender As Object, e As EventArgs) Handles iconTimer.Tick
        iconTimer.Stop()
        PictureBox1.Enabled = False
        Button1.Visible = True
    End Sub

End Class