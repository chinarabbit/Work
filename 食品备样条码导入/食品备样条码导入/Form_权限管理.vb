Imports System.Data.SqlClient
Public Class Form_权限管理

    Private Sub 添加新权限_Click(sender As Object, e As EventArgs) Handles 添加新权限.Click
        Dim newauth As String = ""
        newauth = Trim(新权限.Text)
        If newauth = "" Then Exit Sub
        Dim con As New SqlConnection
        con.ConnectionString = glb_sqlconstr
        Dim cmd As New SqlCommand
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select top 1 * from zyn_权限名称 where 权限名称='" + newauth + "'"
        Dim reader As SqlDataReader
        reader = cmd.ExecuteReader
        If reader.Read Then Exit Sub
        reader.Close()
        cmd.CommandText = "insert into zyn_权限名称 (guid,权限名称) values (newid(),'" + newauth + "')"
        cmd.ExecuteNonQuery()

        权限列表.Items.Clear()
        cmd.CommandText = "select 权限名称 from zyn_权限名称 order by 权限名称"
        reader = cmd.ExecuteReader
        Do While reader.Read
            权限列表.Items.Add(reader.Item(0))
        Loop
        reader.Close()
        con.Close()
    End Sub

  
    Private Sub Form_权限管理_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As New SqlConnection
        con.ConnectionString = glb_sqlconstr
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        cmd.Connection = con
        con.Open()
        权限列表.Items.Clear()
        cmd.CommandText = "select 权限名称 from zyn_权限名称 order by 权限名称"
        reader = cmd.ExecuteReader
        Do While reader.Read
            权限列表.Items.Add(reader.Item(0))
        Loop
        reader.Close()

        cmd.CommandText = "select 名称 from 科室信息 order by 名称"
        reader = cmd.ExecuteReader
        科室.Items.Add("")
        Do While reader.Read
            科室.Items.Add(reader.Item(0))
        Loop
        reader.Close()


        con.Close()
    End Sub

    Private Sub 权限列表_SelectedIndexChanged(sender As Object, e As EventArgs) Handles 权限列表.SelectedIndexChanged
         Call 刷新权限人员()
    End Sub

    Private Sub 科室_SelectedIndexChanged(sender As Object, e As EventArgs) Handles 科室.SelectedIndexChanged
        Dim con As New SqlConnection
        con.ConnectionString = glb_sqlconstr
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        cmd.Connection = con
        con.Open()
        现有人员.Items.Clear()
        Dim ks As String
        If Not (科室.SelectedItem Is Nothing) Then
            ks = 科室.SelectedItem
            cmd.CommandText = "select 姓名 from 人事信息 where 科室='" + ks + "'"
            reader = cmd.ExecuteReader
            Do While reader.Read
                现有人员.Items.Add(reader.Item(0))
            Loop
        End If

        Call 刷新权限人员()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
      
        Call 增删权限(现有人员, "增加")
        Call 刷新权限人员()

    End Sub
   

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call 增删权限(有权人员, "删除")
        Call 刷新权限人员()

    End Sub

    Private Sub 有权人员_DoubleClick(sender As Object, e As EventArgs) Handles 有权人员.DoubleClick
        Call 增删权限(有权人员, "删除")
        Call 刷新权限人员()
    End Sub

    Private Sub 现有人员_DoubleClick(sender As Object, e As EventArgs) Handles 现有人员.DoubleClick

        Call 增删权限(现有人员, "增加")
        Call 刷新权限人员()
    End Sub

    Sub 刷新权限人员()
        有权人员.Items.Clear()
        Dim ks, auth As String
        If CheckBox1.Checked Then
            If 科室.SelectedItem Is Nothing Then ks = "" Else ks = 科室.SelectedItem
        Else
            ks = ""
        End If
        If 权限列表.SelectedItem Is Nothing Then auth = "" Else auth = 权限列表.SelectedItem
        有权人员.Items.AddRange(人员列表_科室权限(ks, auth).ToArray)
      
    End Sub
    Sub 增删权限(ByRef list As ListBox, ByVal todo As String)
        Dim flag As Integer
        If todo = "增加" Then flag = 1 Else flag = 0
        Dim auth As String

        If 权限列表.SelectedItem Is Nothing Then Exit Sub

        auth = 权限列表.SelectedItem

        Dim con As New SqlConnection
        con.ConnectionString = glb_sqlconstr
        con.Open()
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "zyn_添加删除权限"
        cmd.Parameters.AddWithValue("增加", flag)
        cmd.Parameters.AddWithValue("姓名", "")
        cmd.Parameters.AddWithValue("权限名称", auth)
        For Each p In list.SelectedItems
            cmd.Parameters("姓名").Value = p.ToString
            cmd.ExecuteNonQuery()
        Next
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Call 刷新权限人员()
    End Sub
    Private Function 人员列表_科室权限(Optional ByVal 科室 As String = "", Optional ByVal 权限 As String = "") As ArrayList
        Dim list As New ArrayList

        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = glb_sqlconstr
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        cmd.Connection = con
        con.Open()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Clear()
        cmd.CommandText = "zyn_按科室和权限读取人员姓名"
        cmd.Parameters.AddWithValue("科室", 科室)
        cmd.Parameters.AddWithValue("权限", 权限)
        reader = cmd.ExecuteReader
        Do While reader.Read
            list.Add(reader.Item(0).ToString)
        Loop
        reader.Close()
        con.Close()

        Return list
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        form_批量修改字段.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form_模板设置.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim path As String = ""
        Dim psnname As String = ""
        psnname = Me.现有人员.SelectedItem

        If psnname <> "" Then
            Try
                Dim con As New SqlClient.SqlConnection
                Dim cmd As New SqlClient.SqlCommand
                Dim reader As SqlClient.SqlDataReader
                con.ConnectionString = glb_sqlconstr
                cmd.Connection = con
                con.Open()
                cmd.CommandText = "select a.电子签名 from 登录用户 a inner join 人事信息 b on a.人员id=b.guid where b.姓名='" + psnname + "'"
                reader = cmd.ExecuteReader
                If reader.Read Then
                    FolderBrowserDialog1.ShowDialog()
                    path = FolderBrowserDialog1.SelectedPath
                    System.IO.File.WriteAllBytes(FolderBrowserDialog1.SelectedPath + "\" + psnname + ".jpg", reader.Item(0))
                End If
                reader.Close()
                con.Close()
            Catch ex As Exception

            End Try

        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form_版本更新.Show()
    End Sub
End Class