Imports System.Data.SqlClient
Public Class form_报告打印
    Public ep_flag As Boolean = True
    Public dw As String = ""
    Public color_bhg, color_jj, color_dy As Int32
    Public old_list1 As Int32 = 0

    Private Sub form_报告打印_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        DateTimePicker1.Value = DateAdd(DateInterval.Month, -3, Now())
        DateTimePicker2.Value = Now()
        If glb_loginname = "sa" Then
            bug.Visible = True
            bug.Checked = True
            CheckBox1.Checked = False
        End If
        For i = 2014 To Year(Now())
            ComboBox4.Items.Add(i.ToString)
        Next
        ComboBox4.SelectedItem = Year(Now()).ToString
        color_bhg = 0
        color_jj = 0
        color_dy = 0
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        cmd.CommandText = "select * from zyn_颜色 where userid='" + glb_loginname + "' and form_name='" + Me.Name + "'"
        reader = cmd.ExecuteReader
        Do While reader.Read
            Select Case reader.Item("color_name")
                Case "不合格"
                    color_bhg = reader.Item("color")
                Case "加急"
                    color_jj = reader("color")
                Case "打印标记"
                    color_dy = reader("color")
            End Select
        Loop
        reader.Close()
        con.Close()
        cd1.Color = Color.FromArgb(color_bhg)
        cd2.Color = Color.FromArgb(color_jj)
        cd3.Color = Color.FromArgb(color_dy)
        Label3.BackColor = cd1.Color
        Label6.BackColor = cd2.Color
        Label7.BackColor = cd3.Color
        Button1_Click(sender, e)
    End Sub

    Private Sub form_报告打印_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        Mydgv1.Height = Me.Height - 124
        Mydgv1.Width = Me.Width - 319
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim 单位 As String = ""
        cmd.Parameters.AddWithValue("下达日期1", DateTimePicker1.Value.Date)
        cmd.Parameters.AddWithValue("下达日期2", DateAdd(DateInterval.Day, 1, DateTimePicker2.Value.Date))
        cmd.CommandText = "select * from 已完成检验任务 where " + ComboBox1.SelectedItem + ">=@下达日期1 and " + ComboBox1.SelectedItem + "<@下达日期2  "
        Select Case ComboBox3.SelectedItem
            Case "未打印"
                cmd.CommandText += " and 打印标记=0"
            Case "已打印"
                cmd.CommandText += " and 打印标记=1"
        End Select
        单位 = TextBox2.Text

        If 单位 <> "" Then
            cmd.Parameters.AddWithValue("单位", 单位)
            cmd.CommandText += " and " + ComboBox2.SelectedItem + "=@单位"
        End If
        ListBox2.Visible = False
        dw = TextBox2.Text
        cmd.CommandText += " order by 加急 desc,charindex(判定,'不合格,/,合格'),报告编号"
        Mydgv1.load_data(cmd)
        For i = 0 To Mydgv1.get_rownum - 1
            If Mydgv1.get_cellvalue(i, "打印标记") = True Then Mydgv1.set_rowcolor(i, cd3.Color)
            If Mydgv1.get_cellvalue(i, "判定") = "不合格" Then Mydgv1.set_rowcolor(i, cd1.Color)
            If Mydgv1.get_cellvalue(i, "加急").ToString = "True" Then Mydgv1.set_rowcolor(i, cd2.Color)
        Next
    End Sub


    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = Keys.Enter And Trim(TextBox1.Text) <> "" Then
            Dim cmd As New SqlClient.SqlCommand
            cmd.CommandText = "select * from 已完成检验任务 where 报告编号 like @报告编号"
            cmd.Parameters.AddWithValue("报告编号", ComboBox4.SelectedItem + "%" + Trim(TextBox1.Text) + "%")
            Mydgv1.load_data(cmd)
            For i = 0 To Mydgv1.get_rownum - 1
                If Mydgv1.get_cellvalue(i, "打印标记") = True Then Mydgv1.set_rowcolor(i, cd3.Color)
                If Mydgv1.get_cellvalue(i, "判定") = "不合格" Then Mydgv1.set_rowcolor(i, cd1.Color)
                If Mydgv1.get_cellvalue(i, "加急").ToString = "True" Then Mydgv1.set_rowcolor(i, cd2.Color)
            Next
            TextBox1.Text = ""
        End If
    End Sub


    Private Sub Mydgv1_cells_doubleclick(sender As Object, e As EventArgs) Handles Mydgv1.cells_doubleclick
        add_print(Mydgv1.get_currentrow)
        Call Mydgv1.clear_allselect()
    End Sub

    Private Sub add_print(row As Integer)
        Dim bgbh As String = Trim(Mydgv1.get_cellvalue(row, "报告编号"))
        Dim addflag As Boolean = True
        For Each item In ListBox1.Items
            If bgbh = item Then addflag = False
        Next
        If addflag Then
            ListBox1.Items.Add(bgbh)
            ' Mydgv1.set_rowcolor(row, System.Drawing.Color.YellowGreen)
        End If
    End Sub
    Private Sub del_print(index As Integer)
        If index < 0 Then Exit Sub
        Dim bgbh As String = ListBox1.Items(index)
        '  Dim fdcell As DataGridViewCell
        Dim incol As Integer = Mydgv1.get_colindex("报告编号")

        '  fdcell = Mydgv1.find_str(bgbh, incol, 0, incol)
        '  If fdcell IsNot Nothing Then Mydgv1.set_rowcolor(fdcell.RowIndex, ListBox1.BackColor)
        ListBox1.Items.RemoveAt(index)
    End Sub


    Private Sub ListBox1_DoubleClick(sender As Object, e As EventArgs) Handles ListBox1.DoubleClick
        Call del_print(ListBox1.SelectedIndex)
        Call Mydgv1.clear_allselect()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For Each i In Mydgv1.get_selectrows
            Call add_print(i)
        Next
        Call Mydgv1.clear_allselect()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For Each i In Mydgv1.get_allrows
            Call add_print(i)
        Next
        Call Mydgv1.clear_allselect()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Do While ListBox1.SelectedIndex >= 0
            Call del_print(ListBox1.SelectedIndex)
        Loop
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Do While ListBox1.Items.Count > 0
            Call del_print(0)
        Loop
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If CheckBox3.Checked Then
            FolderBrowserDialog1.ShowDialog()
            pdfdir = FolderBrowserDialog1.SelectedPath
            '  TextBox2.Text = pdfdir
        End If
        Dim copys As Integer = 1
        copys = NumericUpDown1.Value
        Dim listxh As Integer = 0
        ProgressBar1.Maximum = ListBox1.Items.Count + 1
        ProgressBar1.Minimum = 0
        ProgressBar1.Value = 1
        ProgressBar1.Visible = True
        Do Until ListBox1.Items.Count = listxh
            If printreport(ListBox1.Items(listxh), CheckBox1.Checked, copys, CheckBox2.Checked, bug.Checked, CheckBox3.Checked) Then
                ListBox1.Items.RemoveAt(listxh)
            Else
                listxh += 1
            End If
            ProgressBar1.Value += 1
        Loop
        ProgressBar1.Visible = False
        If CheckBox1.Checked And ComboBox3.SelectedIndex = 0 Then Button1_Click(sender, e)
    End Sub



    Private Sub ListBox2_Click(sender As Object, e As EventArgs) Handles ListBox2.Click
        ep_flag = False
        TextBox2.Text = ListBox2.SelectedItem
        ListBox2.Visible = False
        Call Button1_Click(sender, e)
        ep_flag = True
    End Sub

    Private Sub ListBox2_KeyUp(sender As Object, e As KeyEventArgs) Handles ListBox2.KeyUp
        If e.KeyCode = Keys.Up And old_list1 = 0 Then
            TextBox2.Focus()
        End If
        If e.KeyCode = Keys.Enter Then
            ep_flag = False
            TextBox2.Text = ListBox2.SelectedItem.ToString
            ListBox2.Visible = False
            Call Button1_Click(sender, e)
            ep_flag = True
        Else
            old_list1 = ListBox2.SelectedIndex
        End If


    End Sub

    Private Sub TextBox2_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox2.KeyUp
        If e.KeyCode = Keys.Down And ListBox2.Visible = True Then
            old_list1 = 0
            ListBox2.SelectedIndex = 0
            ListBox2.Select()
        End If

    End Sub

    Private Sub TextBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox2.KeyDown
        Timer1.Stop()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If ep_flag = False Then
            Timer1.Stop()
            Exit Sub
        End If
        If TextBox2.Text <> "" And dw <> TextBox2.Text Then
            dw = TextBox2.Text
            Dim itemheight As Integer = 23
            Dim cboxheight As Integer = 0
            Dim itemstr As String = ""
            Dim range As New System.Drawing.Rectangle
            Dim dLeft, dTop As Double
            dLeft = TextBox2.Left
            dTop = TextBox2.Bottom
            ListBox2.Top = dTop
            ListBox2.Left = dLeft
            ListBox2.Width = TextBox2.Width
            ListBox2.Items.Clear()

            Dim n As Integer = 0
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            'cmd.CommandText = "zyn_查询多单位名称_报告打印"
            cmd.CommandText = "select distinct 单位名称 from (SELECT top 50 " + ComboBox2.SelectedItem + " as 单位名称  FROM 已完成检验任务 where " + ComboBox2.SelectedItem + " Like @名称 And 下达日期>=@下达日期1 And 下达日期 <@下达日期2  And (打印标记=@打印状态 Or @打印状态=null)  group by " + ComboBox2.SelectedItem + ") a  order by 单位名称"
            cmd.Parameters.AddWithValue("名称", "%" + dw + "%")
            cmd.Parameters.AddWithValue("下达日期1", DateTimePicker1.Value.Date)
            cmd.Parameters.AddWithValue("下达日期2", DateAdd(DateInterval.Day, 1, DateTimePicker2.Value.Date))
            Select Case ComboBox3.SelectedItem
                Case "未打印"
                    cmd.Parameters.AddWithValue("打印状态", 0)
                Case "已打印"
                    cmd.Parameters.AddWithValue("打印状态", 1)
            End Select

            reader = cmd.ExecuteReader
            Do While reader.Read
                ListBox2.Items.Add(reader.Item(0).ToString)
                n += 1
            Loop
            reader.Close()
            con.Close()
            If n > 30 Then n = 30
            If n = 0 Then ListBox2.Height = 50 Else ListBox2.Height = n * 16 + 10
            ListBox2.Visible = True

        ElseIf TextBox2.Text = "" Then
            ListBox2.Visible = False
        End If
        Timer1.Stop()
    End Sub


    Private Sub ListBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox2.KeyDown
        Timer1.Stop()
    End Sub

    Private Sub TextBox2_DoubleClick(sender As Object, e As EventArgs) Handles TextBox2.DoubleClick
        TextBox2.SelectAll()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        TextBox2.Text = ""
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()

        Dim listxh As Integer = 0
        ProgressBar1.Maximum = ListBox1.Items.Count + 1
        ProgressBar1.Minimum = 0
        ProgressBar1.Value = 1
        ProgressBar1.Visible = True
        Do Until ListBox1.Items.Count = listxh
            Try
                cmd.CommandText = "update 已完成检验任务 set 打印标记=1 where 报告编号='" + ListBox1.Items(listxh) + "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "insert into 打印日志 (报告编号,操作人,打印内容,打印份数) values ('" + ListBox1.Items(listxh) + "','" + glb_姓名 + "','" + "设置标记" + "','1')"
                cmd.ExecuteNonQuery()
                ListBox1.Items.RemoveAt(listxh)
            Catch ex As Exception
                listxh += 1
            End Try
            ProgressBar1.Value += 1
        Loop
        ProgressBar1.Visible = False
        con.Close()
        Button1_Click(sender, e)
    End Sub




    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        cd2.ShowDialog()
        If Label6.BackColor <> cd2.Color Then
            Label6.BackColor = cd2.Color
            Call save_color(Me.Name, "加急", cd2.Color.ToArgb)
            For i = 0 To Mydgv1.get_rownum - 1
                If Mydgv1.get_cellvalue(i, "加急").ToString = "True" Then Mydgv1.set_rowcolor(i, cd2.Color)
            Next
        End If

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()

        Dim listxh As Integer = 0
        ProgressBar1.Maximum = ListBox1.Items.Count + 1
        ProgressBar1.Minimum = 0
        ProgressBar1.Value = 1
        ProgressBar1.Visible = True
        Do Until ListBox1.Items.Count = listxh
            Try
                cmd.CommandText = "update 已完成检验任务 set 打印标记=0 where 报告编号='" + ListBox1.Items(listxh) + "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "insert into 打印日志 (报告编号,操作人,打印内容,打印份数) values ('" + ListBox1.Items(listxh) + "','" + glb_姓名 + "','" + "去掉标记" + "','1')"
                cmd.ExecuteNonQuery()
                ListBox1.Items.RemoveAt(listxh)
            Catch ex As Exception
                listxh += 1
            End Try
            ProgressBar1.Value += 1
        Loop
        ProgressBar1.Visible = False
        con.Close()
        Button1_Click(sender, e)
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Dim bgbh As String = Mydgv1.get_cellvalue(Mydgv1.get_currentrow, "报告编号")
        If bgbh <> "" Then
            form_打印日志.pro_bgbh = bgbh
            form_打印日志.ShowDialog()
        End If

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        cd1.ShowDialog()
        If Label3.BackColor <> cd1.Color Then
            Label3.BackColor = cd1.Color
            Call save_color(Me.Name, "不合格", cd1.Color.ToArgb)
            For i = 0 To Mydgv1.get_rownum - 1
                If Mydgv1.get_cellvalue(i, "判定") = "不合格" Then Mydgv1.set_rowcolor(i, cd1.Color)
            Next
        End If

    End Sub
    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click
        cd3.ShowDialog()
        If Label7.BackColor <> cd3.Color Then
            Label7.BackColor = cd3.Color
            Call save_color(Me.Name, "打印标记", cd3.Color.ToArgb)
            For i = 0 To Mydgv1.get_rownum - 1
                If Mydgv1.get_cellvalue(i, "打印标记") = True Then Mydgv1.set_rowcolor(i, cd3.Color)
            Next
        End If
    End Sub

    Private Sub TextBox2_DragLeave(sender As Object, e As EventArgs) Handles TextBox2.DragLeave

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If ep_flag Then Timer1.Start()
    End Sub

    Private Sub ListBox2_LostFocus(sender As Object, e As EventArgs) Handles ListBox2.LostFocus
        ListBox2.ClearSelected()
    End Sub
End Class