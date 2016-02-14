Imports System.Text
Imports System.Security.Cryptography
Public Class LoginForm1

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim keystr As String = "Software\食品报告"
        Dim rootkey, key As Microsoft.Win32.RegistryKey
        rootkey = My.Computer.Registry.CurrentUser
        key = rootkey.OpenSubKey(keystr, True)

        Dim constr, username, pwd As String
        Dim con As New SqlClient.SqlConnection
        username = Trim(UsernameTextBox.Text).ToLower
        glb_loginname = username
        pwd = PasswordTextBox.Text
        If username <> "sa" Then
            pwd = getMD5Hash(pwd)
        End If
        constr = "Data Source=" + key.GetValue("server").ToString + ";Initial Catalog=KingsLims;User ID=" + username + ";Password=" + pwd
        con.ConnectionString = constr
        Dim cmd As New SqlClient.SqlCommand
        Dim reader As SqlClient.SqlDataReader
        Try

            con.Open()
            cmd.Connection = con
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "zyn_读取用户信息"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("username", glb_loginname)
            reader = cmd.ExecuteReader
            If reader.Read Then
                glb_姓名 = reader.Item(0).ToString
                glb_管理科室 = reader.Item(1).ToString
                glb_科室 = reader.Item(2).ToString
                key.SetValue("loginname", username)
            End If
            reader.Close()

            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "zyn_读取权限"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("username", username)
            reader = cmd.ExecuteReader
            If reader.Read Then
                glb_sqlconstr = constr
                glb_loginpass = True
                Do
                    glb_auth += "|" + reader.Item("权限名称") + "|"
                Loop While reader.Read
                reader.Close()

                Me.Close()
            ElseIf glb_loginname = "sa" Then
                glb_sqlconstr = constr
                glb_loginpass = True
                Me.Close()
            Else
                Me.Label1.Text = "没有系统权限"
            End If


            con.Close()

        Catch ex As Exception
            Me.Label1.Text = "登录数据库失败"
        End Try

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub
   
    Function getMD5Hash(ByVal strToHash As String) As String
        Dim md5Obj As New Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)
        bytesToHash = md5Obj.ComputeHash(bytesToHash)
        Dim strResult As String = ""
        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next
        Return strResult
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        form_loginini.ShowDialog()
    End Sub

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim keystr As String = "Software\食品报告"
        Dim rootkey, key As Microsoft.Win32.RegistryKey
        rootkey = My.Computer.Registry.CurrentUser
        key = rootkey.OpenSubKey(keystr)
        If key Is Nothing Then
            key = rootkey.CreateSubKey(keystr)
            key.SetValue("server", "192.168.0.77")
        End If

        If Not (key.GetValue("loginname") Is Nothing) Then Me.UsernameTextBox.Text = key.GetValue("loginname").ToString

    End Sub


End Class
