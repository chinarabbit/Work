Public Class form_打印日志
    Private Sub form_打印日志_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = bgbh + "的打印日志:"
        Dim cmd As New SqlClient.SqlCommand
        cmd.CommandText = "select * from [KingsLims].[dbo].[打印日志] where 报告编号='" + bgbh + "' order by 时间 desc"
        Mydgv1.load_data(cmd)
    End Sub
    Private bgbh As String = ""
    Public Property pro_bgbh As String
        Set(value As String)
            bgbh = value
        End Set
        Get
            Return bgbh
        End Get
    End Property
End Class