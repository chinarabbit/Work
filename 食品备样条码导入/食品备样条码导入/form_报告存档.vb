Imports System.Data.SqlClient
Public Class form_报告存档
    Dim hguid As String = ""
    Dim old(5) As String
    Dim init_status As Boolean = True
    Private Sub from_报告存档_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim year As Integer = Now.Year
        ComboBox3.Items.Add("全部")
        For i = 2014 To year
            ComboBox2.Items.Add(i.ToString)
            ComboBox3.Items.Add(i.ToString)
        Next
        ComboBox3.SelectedItem = Now.Year.ToString
        Call fill_案卷()
        If dgv1.Rows.Count > 0 Then dgv1.SelectedRows(0).Selected = False
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "P_检验科室"
        reader = cmd.ExecuteReader
        Do While reader.Read
            ComboBox1.Items.Add(reader.Item(0))
        Loop
        ComboBox1.SelectedIndex = -1
        reader.Close()
        con.Close()

        TextBox1.ReadOnly = True
        TextBox2.ReadOnly = True
        TextBox3.ReadOnly = True
        TextBox4.ReadOnly = True
        ComboBox1.Enabled = False
        ComboBox2.Enabled = False
        init_status = False
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv1.CellClick
        If e.RowIndex = -1 Then Exit Sub
        hguid = dgv1.Rows(e.RowIndex).Cells("guid").Value
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        Dim reader As SqlDataReader
        cmd.CommandText = "select 案卷号,案卷标题,案卷内容,备注,主检科室,年份 from 存档盒 where guid='" + hguid + "'"
        con.Open()
        reader = cmd.ExecuteReader
        If reader.Read Then
            TextBox1.Text = reader.Item(0).ToString
            TextBox2.Text = reader.Item(1).ToString
            TextBox3.Text = reader.Item(2).ToString
            TextBox4.Text = reader.Item(3).ToString
            ComboBox1.SelectedItem = reader.Item(4)
            ComboBox2.SelectedItem = reader.Item(5).ToString
        End If
        reader.Close()
        con.Close()

        Call fill_存档明细(hguid)
    End Sub
    Sub fill_存档明细(ByVal guid As String)
        Dim cmd As New SqlCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "select guid,rguid,hguid,显示序号 as 序号,报告编号,检验类型,主检科室,受检单位 from 报告存档 where hguid='" + guid + "' order by 显示序号"
        Mydgv1.load_data(cmd)
    End Sub
    Sub fill_案卷()
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim array As New ArrayList
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        cmd.CommandText = "select 案卷号,案卷标题,主检科室,guid from 存档盒 where 年份=@year or @year=0 order by 案卷号"
        If ComboBox3.SelectedItem = "全部" Then cmd.Parameters.AddWithValue("year", 0) Else cmd.Parameters.AddWithValue("year", CInt(ComboBox3.SelectedItem))
        reader = cmd.ExecuteReader
        dgv1.Rows.Clear()
        dgv1.Columns.Clear()
        dgv1.Columns.Add("案卷号", "案卷号")
        dgv1.Columns("案卷号").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        dgv1.Columns.Add("案卷标题", "案卷标题")
        dgv1.Columns.Add("主检科室", "主检科室")
        dgv1.Columns.Add("guid", "guid")
        dgv1.Columns("guid").Visible = False
        Do While reader.Read
            array.Clear()
            For i = 0 To 3
                array.Add(reader.Item(i).ToString)
            Next
            dgv1.Rows.Add(array.ToArray)
        Loop
        reader.Close()
        con.Close()

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Button1.Text = "修改案卷" Then
            If hguid = "" Then Exit Sub
            Button1.Text = "取消修改"
            Button2.Text = "保存修改"
            old(0) = TextBox1.Text
            old(1) = TextBox2.Text
            old(2) = TextBox3.Text
            old(3) = TextBox4.Text
            old(4) = ComboBox1.SelectedItem
            old(5) = ComboBox2.SelectedItem.ToString
            TextBox1.ReadOnly = False
            TextBox2.ReadOnly = False
            TextBox3.ReadOnly = False
            TextBox4.ReadOnly = False
            ComboBox1.Enabled = True
            ComboBox2.Enabled = True
            Mydgv1.Enabled = False
            dgv1.Enabled = False
        ElseIf Button1.Text = "取消修改" Then
            TextBox1.Text = old(0)
            TextBox2.Text = old(1)
            TextBox3.Text = old(2)
            TextBox4.Text = old(3)
            ComboBox1.SelectedItem = old(4)
            ComboBox2.SelectedItem = old(5)
            TextBox1.ReadOnly = True
            TextBox2.ReadOnly = True
            TextBox3.ReadOnly = True
            TextBox4.ReadOnly = True
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            Button2.Text = "新增案卷"
            Button1.Text = "修改案卷"
            dgv1.Enabled = True
            Mydgv1.Enabled = True
        ElseIf Button1.Text = "保存新增" Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim sql As String
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            sql = "insert into 存档盒 (guid,案卷号,案卷标题,案卷内容,备注,主检科室,年份) values (newid(),@1,@2,@3,@4,@5,@6)"
            cmd.CommandText = sql
            cmd.Parameters.AddWithValue("1", TextBox1.Text)
            cmd.Parameters.AddWithValue("2", TextBox2.Text)
            cmd.Parameters.AddWithValue("3", TextBox3.Text)
            cmd.Parameters.AddWithValue("4", TextBox4.Text)
            cmd.Parameters.AddWithValue("5", ComboBox1.SelectedItem)
            cmd.Parameters.AddWithValue("6", CInt(ComboBox2.SelectedItem))
            cmd.ExecuteNonQuery()
            con.Close()
            dgv1.Enabled = True
            Mydgv1.Enabled = True
            dgv1.Rows.Clear()
            dgv1.Columns.Clear()
            Call fill_案卷()
            If hguid <> "" Then
                For i = 0 To dgv1.Rows.Count - 1
                    If dgv1.Rows(i).Cells(3).Value = hguid Then
                        dgv1.Rows(i).Selected = True
                        Exit For
                    End If
                Next
            Else
                dgv1.SelectedRows(0).Selected = False
            End If
            TextBox1.ReadOnly = True
            TextBox2.ReadOnly = True
            TextBox3.ReadOnly = True
            TextBox4.ReadOnly = True
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            Button2.Text = "新增案卷"
            Button1.Text = "修改案卷"
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "保存修改" Then
            Dim n As Integer
            n = dgv1.SelectedRows(0).Index
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim sql As String
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            sql = "update 存档盒 set 案卷号=@1,案卷标题=@2,案卷内容=@3,备注=@4,主检科室=@5,年份=@6 where guid=@7"
            cmd.CommandText = sql
            cmd.Parameters.AddWithValue("1", TextBox1.Text)
            cmd.Parameters.AddWithValue("2", TextBox2.Text)
            cmd.Parameters.AddWithValue("3", TextBox3.Text)
            cmd.Parameters.AddWithValue("4", TextBox4.Text)
            cmd.Parameters.AddWithValue("5", ComboBox1.SelectedItem)
            cmd.Parameters.AddWithValue("6", CInt(ComboBox2.SelectedItem))
            cmd.Parameters.AddWithValue("7", hguid)
            cmd.ExecuteNonQuery()
            con.Close()
            dgv1.Enabled = True
            Mydgv1.Enabled = True
            dgv1.Rows.Clear()
            dgv1.Columns.Clear()
            Call fill_案卷()
            dgv1.Rows(n).Selected = True
            TextBox1.ReadOnly = True
            TextBox2.ReadOnly = True
            TextBox3.ReadOnly = True
            TextBox4.ReadOnly = True
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            Button2.Text = "新增案卷"
            Button1.Text = "修改案卷"
        ElseIf Button2.Text = "新增案卷" Then
            Button1.Text = "保存新增"
            Button2.Text = "取消新增"
            old(0) = TextBox1.Text
            old(1) = TextBox2.Text
            old(2) = TextBox3.Text
            old(3) = TextBox4.Text
            old(4) = ComboBox1.SelectedItem
            old(5) = ComboBox2.SelectedItem.ToString
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox4.Text = ""
            ComboBox1.SelectedIndex = 0
            ComboBox2.SelectedItem = Now.Year.ToString
            TextBox1.ReadOnly = False
            TextBox2.ReadOnly = False
            TextBox3.ReadOnly = False
            TextBox4.ReadOnly = False
            ComboBox1.Enabled = True
            ComboBox2.Enabled = True
            Mydgv1.Enabled = False
            dgv1.Enabled = False
        ElseIf Button2.Text = "取消新增" Then
            TextBox1.Text = old(0)
            TextBox2.Text = old(1)
            TextBox3.Text = old(2)
            TextBox4.Text = old(3)
            ComboBox1.SelectedItem = old(4)
            ComboBox1.SelectedItem = old(5)
            TextBox1.ReadOnly = True
            TextBox2.ReadOnly = True
            TextBox3.ReadOnly = True
            TextBox4.ReadOnly = True
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            Button2.Text = "新增案卷"
            Button1.Text = "修改案卷"
            dgv1.Enabled = True
            Mydgv1.Enabled = True
        End If
    End Sub

    Private Sub TextBox5_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox5.KeyDown
        If e.KeyCode = Keys.Enter Then
            If RadioButton1.Checked = True Then
                Dim bianhao As String
                bianhao = Trim(UCase(TextBox5.Text).ToString)
                TextBox5.Text = ""
                If bianhao = "" Then Exit Sub
                Label9.Text = ""
                Label9.ForeColor = Color.Red
                Dim rguid As String = ""
                
                Dim con As New SqlConnection
                Dim cmd As New SqlCommand
                Dim reader As SqlDataReader
                con.ConnectionString = glb_sqlconstr
                cmd.Connection = con
                con.Open()
                If Len(bianhao) = 6 Then
                    cmd.CommandText = "select guid from sys_view_检验任务 where right(报告编号,6)='" + bianhao + "'"
                Else
                    cmd.CommandText = "select guid from sys_view_检验任务 where 报告编号='" + bianhao + "'"
                End If

                reader = cmd.ExecuteReader
                If reader.Read Then
                    rguid = reader.Item(0)
                End If
                reader.Close()

                If rguid = "" Then
                    Label9.Text = "没有找到报告"
                    Exit Sub
                End If
                Dim isfind As Boolean = False
                cmd.CommandText = "select a.guid,a.案卷号 from 存档盒 a inner join 报告存档 b on a.guid=b.hguid where b.rguid='" + rguid + "'"
                reader = cmd.ExecuteReader
                If reader.Read Then
                    hguid = reader.Item(0)
                    ComboBox3.SelectedItem = "全部"
                    Call fill_案卷()
                    For i = 0 To dgv1.Rows.Count - 1
                        If dgv1.Rows(i).Cells(3).Value = hguid Then
                            isfind = True
                            dgv1.Rows(i).Selected = True
                            dgv1.FirstDisplayedScrollingRowIndex = i
                            Call fill_存档明细(hguid)
                            Call Mydgv1.select_row(rguid, "rguid")
                            Label9.ForeColor = Color.Blue
                            Label9.Text = "存档案卷号:" + reader.Item(1).ToString
                        End If
                    Next
                    If Not isfind Then Label9.Text = "没能定位到档案盒"
                Else
                    Label9.Text = "该报告没有存档记录"
                End If
                reader.Close()
                con.Close()

            End If


            If RadioButton2.Checked = True Then
                If hguid = "" Then
                    Label9.ForeColor = Color.Red
                    Label9.Text = "请先选择要放入的案卷"
                    Exit Sub
                End If
                If TextBox5.Text = "" Then Exit Sub
                TextBox5.Text = UCase(TextBox5.Text)
                Label9.Text = ""
                Label9.ForeColor = Color.Red
                Dim rguid As String = ""
                Dim 报告编号, 检验类型, 主检科室, 受检单位 As String
                报告编号 = ""
                检验类型 = ""
                主检科室 = ""
                受检单位 = ""
                Dim con As New SqlConnection
                Dim cmd As New SqlCommand
                Dim reader As SqlDataReader
                con.ConnectionString = glb_sqlconstr
                cmd.Connection = con
                con.Open()
                cmd.CommandText = "select guid,报告编号,检验类型,主检科室,受检单位 from sys_view_检验任务 where 报告编号='" + Trim(TextBox5.Text.ToString) + "'"
                reader = cmd.ExecuteReader
                If reader.Read Then
                    rguid = reader.Item(0)
                    报告编号 = reader.Item(1)
                    检验类型 = reader.Item(2)
                    主检科室 = reader.Item(3)
                    If IsDBNull(reader.Item(4)) Then 受检单位 = "" Else 受检单位 = reader.Item(4)
                End If
                reader.Close()
                If rguid = "" Then
                    Label9.Text = "没有找到报告"
                    TextBox5.Text = ""
                    con.Close()
                    Exit Sub
                End If
                cmd.CommandText = "select a.案卷号,b.hguid from 存档盒 a inner join 报告存档 b on a.guid=b.hguid where b.rguid='" + rguid + "'"
                reader = cmd.ExecuteReader

                If reader.Read Then
                    If reader.Item(1) = hguid Then
                        MessageBox.Show("该报告已在本案卷中")
                        Exit Sub
                    End If
                    Dim result As DialogResult
                    result = MessageBox.Show("已有存档记录,案卷号为" + reader.Item(0) + "是否覆盖", "请选择", MessageBoxButtons.YesNo)
                    reader.Close()
                    If result = Windows.Forms.DialogResult.No Then
                        Exit Sub
                    Else
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = "delete from 报告存档 where rguid='" + rguid + "'"
                        cmd.ExecuteNonQuery()
                    End If
                End If
                If Not reader.IsClosed Then reader.Close()
                cmd.CommandText = "insert into 报告存档 (guid,rguid,hguid,报告编号,检验类型,主检科室,受检单位,显示序号) values (newid(),@rguid,@hguid,@报告编号,@检验类型,@主检科室,@受检单位,@显示序号)"
                cmd.Parameters.AddWithValue("rguid", rguid)
                cmd.Parameters.AddWithValue("hguid", hguid)
                cmd.Parameters.AddWithValue("报告编号", 报告编号)
                cmd.Parameters.AddWithValue("检验类型", 检验类型)
                cmd.Parameters.AddWithValue("主检科室", 主检科室)
                cmd.Parameters.AddWithValue("受检单位", 受检单位)
                cmd.Parameters.AddWithValue("显示序号", Mydgv1.get_rownum + 1)
                cmd.ExecuteNonQuery()

                con.Close()
                'Dim colname, colvalue As New ArrayList
                'colname.Add("序号")
                'colname.Add("报告编号")
                'colname.Add("检验类型")
                'colname.Add("受检单位")
                'colname.Add("主检科室")
                'colvalue.Add(Mydgv1.get_rownum + 1)
                'colvalue.Add(报告编号)
                'colvalue.Add(检验类型)
                'colvalue.Add(受检单位)
                'colvalue.Add(主检科室)
                'Call Mydgv1.add_row(colname, colvalue)
                Call fill_存档明细(hguid)
                Label9.ForeColor = Color.Blue
                Label9.Text = 报告编号 + "已存档"
                TextBox5.Text = ""
            End If
        End If
    End Sub


    Private Sub RadioButton2_Click(sender As Object, e As EventArgs) Handles RadioButton2.Click
        If hguid = "" Then
            Label9.ForeColor = Color.Red
            Label9.Text = "请先选择要放入的案卷"
            RadioButton1.Checked = True
        Else
            Label9.Text = ""
        End If
      
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If Not init_status Then Call fill_案卷()
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged

    End Sub
End Class