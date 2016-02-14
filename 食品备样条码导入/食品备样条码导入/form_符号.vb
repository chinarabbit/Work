Imports System.Data.SqlClient
Public Class form_符号
    Public fh As String = ""
    Public fhguid As String = ""
    Public fhtag As String = ""
    Private Sub form_符号_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        My.Computer.Clipboard.Clear()
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select fh,guid,username from zyn_符号 where username='sa' order by id"
        reader = cmd.ExecuteReader
        Do While reader.Read
            Dim lb As New Label
            lb.Text = reader.Item(0)
            lb.Name = reader.Item(1).ToString
            lb.Tag = reader.Item(2)
            lb.Width = 25
            lb.TextAlign = ContentAlignment.MiddleCenter
            AddHandler lb.Click, AddressOf btn_click
            AddHandler lb.DoubleClick, AddressOf double_click
            FlowLayoutPanel1.Controls.Add(lb)
        Loop
        reader.Close()
        If glb_loginname <> "sa" Then
            cmd.CommandText = "select fh,guid,username from zyn_符号 where username='" + glb_loginname + "' order by id"
            reader = cmd.ExecuteReader
            Do While reader.Read
                Dim lb As New Label
                lb.Text = reader.Item(0)
                lb.Name = reader.Item(1).ToString
                lb.Tag = reader.Item(2)
                lb.Width = 25
                AddHandler lb.Click, AddressOf btn_click
                AddHandler lb.DoubleClick, AddressOf double_click
                FlowLayoutPanel1.Controls.Add(lb)
            Loop
            reader.Close()
        End If
        con.Close()

    End Sub
    Sub btn_click(ByVal btn As Object, ByVal e As EventArgs)
        Dim lb As Label
        If TypeOf (btn) Is Label Then
            lb = btn
            If lb.ForeColor = SystemColors.ControlText Then
                If fhguid <> "" Then
                    Dim oldlb As Label
                    oldlb = FlowLayoutPanel1.Controls.Item(fhguid)
                    oldlb.ForeColor = SystemColors.ControlText
                    oldlb.BackColor = SystemColors.Control
                End If
                My.Computer.Clipboard.SetText(lb.Text)
                lb.ForeColor = Color.Red
                lb.BackColor = Color.YellowGreen
                fhguid = lb.Name
                fhtag = lb.Tag
            Else
                lb.ForeColor = SystemColors.ControlText
                lb.BackColor = SystemColors.Control
                My.Computer.Clipboard.Clear()
                fhguid = ""
                fhtag = ""
            End If

        End If

    End Sub
    Sub double_click(sender As Object, e As EventArgs)
        Dim lb As Label
        If TypeOf (sender) Is Label Then
            lb = sender
            My.Computer.Clipboard.SetText(lb.Text)
            Me.Dispose()
        End If

    End Sub
    Private Sub form_符号_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        FlowLayoutPanel1.Width = Me.Width - 17
        FlowLayoutPanel1.Height = Me.Height - 80
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        fh = InputBox("添加符号:", "添加", My.Computer.Clipboard.GetText)
        If Trim(fh) = "" Then Exit Sub
        fh = fh.Substring(0, 1)
        Dim newid As String = Guid.NewGuid.ToString
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "insert into zyn_符号 (fh,username,guid) values (@fh,@username,@guid)"
        cmd.Parameters.AddWithValue("fh", fh)
        cmd.Parameters.AddWithValue("username", glb_loginname)
        cmd.Parameters.AddWithValue("guid", newid)
        cmd.ExecuteNonQuery()
        con.Close()

        Dim lb As New Label
        lb.Text = fh
        lb.Name = newid
        lb.Tag = glb_loginname
        lb.Width = 25
        AddHandler lb.Click, AddressOf btn_click
        AddHandler lb.DoubleClick, AddressOf double_click
        FlowLayoutPanel1.Controls.Add(lb)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        My.Computer.Clipboard.Clear()
        Me.Dispose()
    End Sub

    Private Sub form_符号_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If fhtag = "sa" And glb_loginname <> "sa" Then
            MessageBox.Show("系统内置符号不能删除!")
            Exit Sub
        End If
        If fhguid <> "" And fhtag = glb_loginname Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "delete zyn_符号 where guid='" + fhguid + "' and username='" + glb_loginname + "'"
            If cmd.ExecuteNonQuery > 0 Then
                FlowLayoutPanel1.Controls.RemoveByKey(fhguid)
                fhguid = ""
                fhtag = ""
            End If

        End If
    End Sub

    Private Sub form_符号_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then Button1_Click(sender, e)
        If e.KeyCode = Keys.Escape Then Button2_Click(sender, e)
    End Sub
End Class