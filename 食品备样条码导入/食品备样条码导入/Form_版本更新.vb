Imports System.Data.SqlClient

Public Class Form_版本更新
    Dim refresh_flag As Boolean = False
    Dim id As Integer = 0
    Private Sub Form_版本更新_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refresh_flag = False
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select id,版本号 from zyn_版本更新 order by id desc"
        reader = cmd.ExecuteReader
        Do While reader.Read
            dgv1.Rows.Add(reader.Item(1).ToString)
            id = reader.Item(0)
        Loop
        reader.Close()
        con.Close()
        refresh_flag = True
    End Sub

    Private Sub dgv1_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgv1.CurrentCellChanged
        If refresh_flag Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "select * from zyn_版本更新 where id=" + id.ToString
            reader = cmd.ExecuteReader
            If reader.Read Then
                Dim ver As Version
                ver = New Version(reader.Item("版本号").ToString)
                TextBox1.Text = ver.Major.ToString
                TextBox2.Text = ver.Minor.ToString
                TextBox3.Text = ver.Build.ToString
                TextBox4.Text = ver.Revision.ToString
                TextBox5.Text = reader.Item("更新说明")
                DateTimePicker1.Value = reader.Item("更新时间")
            End If
            reader.Close()
            con.Close()
        End If
    End Sub
End Class