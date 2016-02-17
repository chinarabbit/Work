Imports System.Data.SqlClient

Public Class Form_版本更新
    Dim refresh_flag As Boolean = False
    Dim id As Integer = 0
    Private Sub Form_版本更新_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refresh_flag = False
        dgv1.Rows.Clear()
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select id,版本号 from zyn_版本更新 order by id desc"
        reader = cmd.ExecuteReader
        Do While reader.Read
            dgv1.Rows.Add(reader.Item(0).ToString, reader.Item(1).ToString)
        Loop
        reader.Close()
        con.Close()
        refresh_flag = True
        dgv1_CurrentCellChanged(sender, e)
    End Sub

    Private Sub dgv1_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgv1.CurrentCellChanged
        If dgv1.CurrentCell Is Nothing Then Exit Sub
        If refresh_flag Then
            id = CInt(dgv1.CurrentRow.Cells("版本id").Value)
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
                id = reader.Item("id")
            End If
            reader.Close()
            con.Close()
        End If
        Button2.Enabled = False
        Button3.Enabled = False
        If id > 0 Then Button4.Enabled = True Else Button4.Enabled = False
        TextBox1.ReadOnly = True
        TextBox2.ReadOnly = True
        TextBox3.ReadOnly = True
        TextBox4.ReadOnly = True
        TextBox5.ReadOnly = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button2.Enabled = True
        Button3.Enabled = True
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox1.ReadOnly = False
        TextBox2.ReadOnly = False
        TextBox3.ReadOnly = False
        TextBox4.ReadOnly = False
        TextBox5.ReadOnly = False
        id = -1
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        If id = -1 Then
            cmd.CommandText = "insert into zyn_版本更新 (版本号,更新说明) values (@版本号,@更新说明)"
            Dim bbh As String = TextBox1.Text.ToString + "." + TextBox2.Text.ToString + "." + TextBox3.Text.ToString + "." + TextBox4.Text.ToString
            cmd.Parameters.AddWithValue("版本号", bbh)
            cmd.Parameters.AddWithValue("更新说明", TextBox5.Text)
            cmd.ExecuteNonQuery()
        Else
            cmd.CommandText = "update zyn_版本更新 set 版本号=@版本号,更新说明=@更新说明 where id=@id"
            Dim bbh As String = TextBox1.Text.ToString + "." + TextBox2.Text.ToString + "." + TextBox3.Text.ToString + "." + TextBox4.Text.ToString
            cmd.Parameters.AddWithValue("版本号", bbh)
            cmd.Parameters.AddWithValue("更新说明", TextBox5.Text)
            cmd.Parameters.AddWithValue("id", id.ToString)
            cmd.ExecuteNonQuery()
        End If
        con.Close()
        MyBase.OnLoad(e)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        dgv1_CurrentCellChanged(sender, e)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If id > 0 Then
            Button2.Enabled = True
            Button3.Enabled = True
            TextBox1.ReadOnly = False
            TextBox2.ReadOnly = False
            TextBox3.ReadOnly = False
            TextBox4.ReadOnly = False
            TextBox5.ReadOnly = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If id > 0 Then
            If MessageBox.Show("删除指定项目?", "确认删除", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Dim con As New SqlConnection
                Dim cmd As New SqlCommand
                con.ConnectionString = glb_sqlconstr
                cmd.Connection = con
                con.Open()
                cmd.CommandText = "delete zyn_版本更新 where id=" + id.ToString
                cmd.ExecuteNonQuery()
                con.Close()
                MyBase.OnLoad(e)
            End If
        End If
    End Sub
End Class