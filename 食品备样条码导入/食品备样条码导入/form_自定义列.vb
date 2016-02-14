Imports System.Data.SqlClient

Public Class form_自定义列
    Public formname As String = ""
    Public controlname As String = ""

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        Dim dispindex As Integer = 0
        Dim disp As Boolean = False

        For Each checkctrl As CheckBox In flp.Controls
            cmd.Parameters.Clear()
            disp = Not checkctrl.Checked
            cmd.CommandText = "update zyn_表格属性 set 隐藏=@disp,显示顺序=@dispindex where guid=@guid"
            cmd.Parameters.AddWithValue("guid", checkctrl.Name)
            cmd.Parameters.AddWithValue("disp", disp)
            cmd.Parameters.AddWithValue("dispindex", dispindex)
            cmd.ExecuteNonQuery()
            If glb_loginname = "sa" Then
                cmd.Parameters.Clear()
                cmd.CommandText = "update zyn_表格属性 set 管理隐藏=@disp where 表单名称='" + formname + "' and 控件名称='" + controlname + "' and 列名称='" + checkctrl.Text + "'"
                cmd.Parameters.AddWithValue("disp", disp)
                cmd.ExecuteNonQuery()
            End If
            dispindex += 1
        Next

        DialogResult = DialogResult.Yes
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DialogResult = DialogResult.No
    End Sub

    Private Sub form_自定义列_Load(sender As Object, e As EventArgs) Handles Me.Load
        If formname = "" Or controlname = "" Then
            Me.Dispose()
        Else
            flp.Controls.Clear()
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim ada As New SqlDataAdapter
            Dim bulider As New SqlCommandBuilder
            Dim table As New DataTable
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            cmd.CommandText = "select guid,隐藏,列名称 from zyn_表格属性 where 表单名称=@formname and username=@username and 控件名称=@controlname and (管理隐藏=@glyc or @glyc is null) order by 显示顺序"
            cmd.Parameters.AddWithValue("formname", formname)
            cmd.Parameters.AddWithValue("controlname", controlname)
            cmd.Parameters.AddWithValue("username", glb_loginname)
            If glb_loginname = "sa" Then cmd.Parameters.AddWithValue("glyc", DBNull.Value) Else cmd.Parameters.AddWithValue("glyc", False)
            ada.SelectCommand = cmd
            bulider.DataAdapter = ada
            ada.Fill(table)

            For Each row As DataRow In table.Rows
                Dim checkctrl As New CheckBox
                checkctrl.AllowDrop = True
                AddHandler checkctrl.MouseDown, AddressOf mouse_down
                AddHandler checkctrl.DragDrop, AddressOf check_dragdrop
                AddHandler checkctrl.DragEnter, AddressOf flp_DragEnter
                AddHandler checkctrl.DragOver, AddressOf flp_DragOver
                AddHandler checkctrl.CheckedChanged, AddressOf change_color
                checkctrl.Name = row("guid")
                checkctrl.Text = row("列名称")
                checkctrl.Checked = Not row("隐藏")
                If checkctrl.Checked Then checkctrl.ForeColor = Color.Blue Else checkctrl.ForeColor = Color.Red
                checkctrl.Width = 180
                'checkctrl.AutoSize = True
                flp.Controls.Add(checkctrl)
            Next

        End If
    End Sub

    Sub change_color(sender As Object, e As EventArgs)
        If TypeOf (sender) Is CheckBox Then
            Dim check As CheckBox = sender
            If check.Checked Then check.ForeColor = Color.Blue Else check.ForeColor = Color.Red
        End If
    End Sub


    Private Sub flp_DragEnter(sender As Object, e As DragEventArgs) Handles flp.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub
    Private Sub flp_DragOver(sender As Object, e As DragEventArgs) Handles flp.DragOver
        e.Effect = DragDropEffects.Move
    End Sub

    Sub mouse_down(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Right And TypeOf (sender) Is CheckBox Then
            Call DoDragDrop(sender.name, DragDropEffects.Move)
        End If
    End Sub



    Private Sub flp_DragDrop(sender As Object, e As DragEventArgs) Handles flp.DragDrop
        Dim index As Integer = flp.Controls.Count - 1
        flp.Controls.SetChildIndex(flp.Controls(e.Data.GetData(DataFormats.Text)), index)
    End Sub
    Sub check_dragdrop(sender As Object, e As DragEventArgs)
        If TypeOf (sender) Is CheckBox Then
            Dim p As Point = flp.PointToClient(New Point(e.X, e.Y))
            Dim ctrl As Control = flp.GetChildAtPoint(p)
            Dim index As Integer = flp.Controls.GetChildIndex(ctrl)
            flp.Controls.SetChildIndex(flp.Controls(e.Data.GetData(DataFormats.Text)), index)
        End If

    End Sub

    Private Sub form_自定义列_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        flp.Width = Me.Width - 21
        flp.Height = Me.Height - 79
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim check As CheckBox
        Dim sortnum As Integer = 0
        For i = 0 To flp.Controls.Count - 1
            If TypeOf (flp.Controls(i)) Is CheckBox Then
                check = flp.Controls(i)
                If check.Checked Then
                    If sortnum < i Then flp.Controls.SetChildIndex(check, sortnum)
                    sortnum += 1
                End If
            End If
        Next
    End Sub
End Class