Imports System.Data.SqlClient
Public Class form_报告发放
    Public ep_flag As Boolean = True
    Public old_list1 As Int32 = 0

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        Timer1.Stop()
    End Sub
    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = Keys.Down And ListBox1.Visible = True Then
            old_list1 = 0
            ListBox1.SelectedIndex = 0
            ListBox1.Select()

        End If
    End Sub


    Private Sub TextBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim bgbh As String = ""
            Dim cmd As New SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "zyn_报告发放查询任务"
            bgbh = TextBox2.Text
            If bgbh <> "" Then
                If Len(bgbh) = 6 Then cmd.Parameters.AddWithValue("流水号", bgbh) Else cmd.Parameters.AddWithValue("报告编号", bgbh)
                Call Mydgv1.load_data(cmd)
                Mydgv1.dgv_selectcell(0, 1)
                Call 取邮件地址()

            End If
            TextBox2.Text = ""
        End If

    End Sub

    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click
        ep_flag = False
        TextBox1.Text = ListBox1.SelectedItem.ToString
        ListBox1.Visible = False
        Call 查询报告()
        Call 加单位列表()
        ComboBox3.SelectedItem = TextBox8.Text
        ep_flag = True
    End Sub

    Private Sub ListBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox1.KeyDown
        Timer1.Stop()

    End Sub
    Private Sub ListBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles ListBox1.KeyUp
        If e.KeyCode = Keys.Up And old_list1 = 0 Then
            TextBox1.Focus()
        End If
        If e.KeyCode = Keys.Enter Then
            ep_flag = False
            TextBox1.Text = ListBox1.SelectedItem.ToString
            ListBox1.Visible = False
            Call 查询报告()
            ep_flag = True
        Else
            old_list1 = ListBox1.SelectedIndex
        End If

    End Sub
    Sub 查询报告()
        Dim cmd As New SqlCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "zyn_报告发放查询任务"
        cmd.Parameters.AddWithValue("名称", TextBox1.Text)
        If RadioButton1.Checked Then
            cmd.Parameters.AddWithValue("发出", 0)
        ElseIf RadioButton2.Checked Then
            cmd.Parameters.AddWithValue("发出", 1)
        End If
        Mydgv1.load_data(cmd)
        Mydgv1.dgv_selectcell(0, 1)
        Call 取邮件地址()
    End Sub
    Sub 加单位列表()
        ComboBox3.Items.Clear()
        If Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "抽样单位") <> "" Then ComboBox3.Items.Add(Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "抽样单位"))
        If Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "委托单位") <> "" Then ComboBox3.Items.Add(Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "委托单位"))
        If Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "受检单位") <> "" Then ComboBox3.Items.Add(Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "受检单位"))
        If Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "生产单位") <> "" Then ComboBox3.Items.Add(Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "生产单位"))
    End Sub
    Sub 取邮件地址(Optional ByVal name As String = "", Optional ByVal flag As Boolean = True)
        Dim person, phone, address, postzip As String
        person = ""
        phone = ""
        address = ""
        postzip = ""
        Try
            If flag Then
                If RadioButton4.Checked Then
                    name = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "抽样单位")
                    person = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "抽样单位联系人")
                    phone = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "抽样单位电话")
                    address = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "抽样单位地址")
                    postzip = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "抽样单位邮编")
                ElseIf RadioButton5.Checked Then
                    name = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "委托单位")
                    person = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "委托单位联系人")
                    phone = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "委托单位电话")
                    address = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "委托单位地址")
                    postzip = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "委托单位邮编")
                ElseIf RadioButton6.Checked Then
                    name = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "受检单位")
                    person = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "受检单位联系人")
                    phone = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "受检单位电话")
                    address = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "受检单位地址")
                    postzip = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "受检单位邮编")
                ElseIf RadioButton7.Checked Then
                    name = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "生产单位")
                    person = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "生产单位联系人")
                    phone = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "生产单位电话")
                    address = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "生产单位地址")
                    postzip = Mydgv1.get_cellvalue(Mydgv1.dgv_currentcell.RowIndex, "生产单位邮编")
                End If
                TextBox8.Text = name
            End If

            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "select top 1 * from 邮寄地址 where 单位名称='" + name + "'"
            reader = cmd.ExecuteReader
            If reader.Read Then
                TextBox3.Text = reader.Item("收件人").ToString
                TextBox4.Text = reader.Item("电话").ToString
                TextBox5.Text = reader.Item("地址").ToString
                TextBox6.Text = reader.Item("邮编").ToString
            Else
                TextBox3.Text = person
                TextBox4.Text = phone
                TextBox5.Text = address
                TextBox6.Text = postzip
            End If
            reader.Close()
            con.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadioButton1_Click(sender As Object, e As EventArgs) Handles RadioButton1.Click
        If TextBox1.Text <> "" And ListBox1.Visible = False Then
            Call 查询报告()
        End If
    End Sub

    Private Sub RadioButton2_Click(sender As Object, e As EventArgs) Handles RadioButton2.Click
        If TextBox1.Text <> "" And ListBox1.Visible = False Then
            Call 查询报告()
        End If
    End Sub

    Private Sub RadioButton3_Click(sender As Object, e As EventArgs) Handles RadioButton3.Click
        If TextBox1.Text <> "" And ListBox1.Visible = False Then
            Call 查询报告()
        End If
    End Sub

    Private Sub form_报告发放_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 1
        ComboBox2.SelectedIndex = 1
        TextBox9.Text = glb_姓名
    End Sub



    Private Sub Mydgv1_cells_selected(sender As Object, e As EventArgs) Handles Mydgv1.cells_selected
        TextBox8.ReadOnly = True
        Call 取邮件地址()
        Call 加单位列表()
    End Sub

    Private Sub RadioButton4_Click(sender As Object, e As EventArgs) Handles RadioButton4.Click
        TextBox8.ReadOnly = True
        Call 取邮件地址()
    End Sub

    Private Sub RadioButton5_Click(sender As Object, e As EventArgs) Handles RadioButton5.Click
        TextBox8.ReadOnly = True
        Call 取邮件地址()
    End Sub

    Private Sub RadioButton6_Click(sender As Object, e As EventArgs) Handles RadioButton6.Click
        TextBox8.ReadOnly = True
        Call 取邮件地址()
    End Sub

    Private Sub RadioButton7_Click(sender As Object, e As EventArgs) Handles RadioButton7.Click
        TextBox8.ReadOnly = True
        Call 取邮件地址()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim printer, oldprinter As String
        Dim keystr As String = "Software\食品报告"
        Dim rootkey, key As Microsoft.Win32.RegistryKey
        rootkey = My.Computer.Registry.CurrentUser
        key = rootkey.OpenSubKey(keystr, True)
        printer = key.GetValue("emsprinter")
        Dim word As New Microsoft.Office.Interop.Word.Application
        Dim doc As Microsoft.Office.Interop.Word.Document
        doc = word.Documents.Add(Application.StartupPath + "\temp\ems.dotx")
        word.Selection.GoTo(-1, , , "sjr")
        word.Selection.TypeText(TextBox3.Text)
        word.Selection.MoveRight(12)
        word.Selection.TypeText(TextBox4.Text)
        word.Selection.MoveRight(12)
        word.Selection.TypeText(TextBox8.Text)
        word.Selection.MoveRight(12, 2)
        word.Selection.TypeText(TextBox5.Text)
        word.Selection.MoveRight(12, 2)
        word.Selection.TypeText(TextBox6.Text)
        oldprinter = word.ActivePrinter
        word.ActivePrinter = printer
        word.ActiveDocument.PageSetup.PageHeight = 360.05
        word.ActiveDocument.PageSetup.PageWidth = 311.85
        word.ActiveDocument.PrintOut(False)
        'word.Visible = True
        'word.ShowMe()
        word.ActivePrinter = oldprinter
        word.Quit(False)

        If CheckBox1.Checked Then Call 更新保存地址()

    End Sub
    Sub 更新保存地址()
        Dim mc As String = Trim(TextBox8.Text)
        If mc = "" Then Exit Sub
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.Parameters.AddWithValue("mc", mc)
        cmd.Parameters.AddWithValue("lxr", TextBox3.Text)
        cmd.Parameters.AddWithValue("dh", TextBox4.Text)
        cmd.Parameters.AddWithValue("dz", TextBox5.Text)
        cmd.Parameters.AddWithValue("yb", TextBox6.Text)

        cmd.CommandText = "delete 邮寄地址 where 单位名称=@mc"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into 邮寄地址 (guid,单位名称,收件人,地址,邮编,电话) values (newid(),@mc,@lxr,@dz,@yb,@dh)"
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub

   
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call 更新保存地址()
    End Sub

    Private Sub RadioButton8_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton8.CheckedChanged
        TextBox8.ReadOnly = False

    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        ComboBox3.SelectedItem = TextBox8.Text
        If TextBox8.Text <> "" And ep_flag And TextBox8.ReadOnly = False Then
            Dim itemheight As Integer = 23
            Dim cboxheight As Integer = 0
            Dim itemstr As String = ""
            Dim range As New System.Drawing.Rectangle
            Dim dLeft, dTop As Double
            dLeft = TextBox8.Left + GroupBox3.Left
            dTop = TextBox8.Bottom + GroupBox3.Top
            ListBox2.Top = dTop
            ListBox2.Left = dLeft
            ListBox2.Width = TextBox8.Width
            ListBox2.Items.Clear()

            Dim n As Integer = 0
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "select 单位名称 from 邮寄地址 where 单位名称 like @名称 order by 单位名称"

            cmd.Parameters.AddWithValue("名称", "%" + TextBox8.Text + "%")
            reader = cmd.ExecuteReader
            Do While reader.Read
                ListBox2.Items.Add(reader.Item(0).ToString)
                n += 1
            Loop
            reader.Close()
            con.Close()
            If n > 20 Then n = 20
            If n = 0 Then ListBox2.Height = 50 Else ListBox2.Height = n * 16 + 10
            ListBox2.Top = ListBox2.Top - ListBox2.Height - TextBox8.Height
            ListBox2.Visible = True
        Else
            ListBox2.Visible = False
        End If
    End Sub

    Private Sub ListBox2_Click(sender As Object, e As EventArgs) Handles ListBox2.Click
        ep_flag = False
        TextBox8.Text = ListBox2.SelectedItem.ToString
        ep_flag = True
        ListBox2.Visible = False
        Call 取邮件地址(ListBox2.SelectedItem.ToString, False)
        TextBox8.Focus()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim rguid, bgbh As String
        Dim selectrows As ArrayList
        selectrows = Mydgv1.get_selectrows

        Dim sj As DateTime
        If DateTimePicker1.Value.Date = Today.Date Then sj = Now() Else sj = DateTimePicker1.Value.Date

        Dim con As New SqlConnection
        Dim cmd As New SqlCommand

        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()

        For i = 0 To selectrows.Count - 1
            cmd.Parameters.Clear()
            rguid = Mydgv1.get_cellvalue(selectrows(i), "guid")
            bgbh = Mydgv1.get_cellvalue(selectrows(i), "报告编号")
            cmd.Parameters.AddWithValue("rguid", rguid)
            cmd.Parameters.AddWithValue("bgbh", bgbh)
            cmd.Parameters.AddWithValue("czr", TextBox9.Text)
            cmd.Parameters.AddWithValue("sj", sj)
            cmd.Parameters.AddWithValue("lqr", ComboBox3.Text)
            cmd.Parameters.AddWithValue("bz", TextBox10.Text)
            cmd.Parameters.AddWithValue("yjbh", TextBox7.Text)
            cmd.Parameters.AddWithValue("fcfs", ComboBox1.SelectedItem)
            cmd.Parameters.AddWithValue("fs", ComboBox2.SelectedItem)
            cmd.CommandText = "insert into  发出记录 (guid,rguid,报告编号,操作人,时间,领取人或单位,备注,邮寄编号,发出方式,份数) values (newid(),@rguid,@bgbh,@czr,@sj,@lqr,@bz,@yjbh,@fcfs,@fs)"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update 已完成检验任务 set 发出=1 where guid=@rguid"
            cmd.ExecuteNonQuery()
            Mydgv1.set_cellvalue(selectrows(i), "发出", "1")
            Mydgv1.dgv_selectrow(selectrows(i), False)
        Next
        con.Close()

    End Sub



    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If TextBox1.Text <> "" And ep_flag Then
            Dim itemheight As Integer = 23
            Dim cboxheight As Integer = 0
            Dim itemstr As String = ""
            Dim range As New System.Drawing.Rectangle
            Dim dLeft, dTop As Double
            dLeft = TextBox1.Left + GroupBox1.Left
            dTop = TextBox1.Bottom + GroupBox1.Top
            ListBox1.Top = dTop
            ListBox1.Left = dLeft
            ListBox1.Width = TextBox1.Width
            ListBox1.Items.Clear()

            Dim n As Integer = 0
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "zyn_查询多单位名称"
            cmd.CommandType = CommandType.StoredProcedure
            If RadioButton1.Checked Then
                cmd.Parameters.AddWithValue("发出", 0)
            ElseIf RadioButton2.Checked Then
                cmd.Parameters.AddWithValue("发出", 1)
            End If
            cmd.Parameters.AddWithValue("名称", TextBox1.Text)
            reader = cmd.ExecuteReader
            Do While reader.Read
                ListBox1.Items.Add(reader.Item(0).ToString)
                n += 1
            Loop
            reader.Close()
            con.Close()
            If n > 30 Then n = 30
            If n = 0 Then ListBox1.Height = 50 Else ListBox1.Height = n * 16 + 10
            ListBox1.Visible = True
        Else
            ListBox1.Visible = False
        End If
        Timer1.Stop()
    End Sub

    Private Sub TextBox1_DoubleClick(sender As Object, e As EventArgs) Handles TextBox1.DoubleClick
        TextBox1.SelectAll()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If ep_flag Then Timer1.Start()
    End Sub

    Private Sub ListBox1_LostFocus(sender As Object, e As EventArgs) Handles ListBox1.LostFocus
        ListBox1.ClearSelected()
    End Sub


End Class