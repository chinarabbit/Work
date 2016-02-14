Imports System.Data.SqlClient, Microsoft.Office.Interop
Public Class Form_协议管理
    Public xguid As String = "", add_tmp As String = ""
    Public edit_flag As Boolean = False, use_stat As Boolean = False
    Public add_flag As Boolean = False, readonly_flag As Boolean = False
    Public refresh_flag As Boolean = False
    Private Sub Form_协议管理_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "SELECT 名称  FROM 科室信息 WHERE (管理标识 = '检验科室')"
        reader = cmd.ExecuteReader
        协议科室.Items.Clear()
        Do While reader.Read
            协议科室.Items.Add(reader.Item(0))
        Loop
        reader.Close()
        con.Close()
        ComboBox1.SelectedIndex = 1
        ComboBox2.SelectedIndex = 0
        If InStr(glb_auth, "|协议查看|") > 0 Then
            Button2.Dispose()
            Button3.Dispose()
            Button5.Dispose()
            TabPage4.Dispose()
        End If
    End Sub


    Private Sub Form_协议管理_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If Me.Width < 1180 Then Me.Width = 1180
    End Sub

    Private Sub Form_协议管理_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        DGV1.Height = Me.Height - 94
        DGV2.Height = Me.Height - 95
        DGV3.Height = Me.Height - 95
        TG1.Height = Me.Height - 53
        TG1.Width = Me.Width - 790
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        refresh_flag = True
        DGV1.Rows.Clear()
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select guid,协议编号,企业名称,isnull(协议金额,0),isnull(应收金额,0),isnull(协议次数,0),isnull(签订次数,0),地址,联系人,联系电话,登记时间,人员分组  from 协议 where (协议编号 like @p1 or @p1='%全部年份%') and 状态=@p2  and (企业名称=@p3 or @p3='') order by 协议编号"
        cmd.Parameters.AddWithValue("p1", "%" + ComboBox1.SelectedItem.ToString + "%")
        If ComboBox2.SelectedIndex = 0 Then cmd.Parameters.AddWithValue("p2", 4) Else cmd.Parameters.AddWithValue("p2", 8)
        cmd.Parameters.AddWithValue("p3", TextBox5.Text)
        reader = cmd.ExecuteReader
        Dim i As Integer = 0
        Do While reader.Read
            DGV1.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4), reader.Item(5), reader.Item(6), reader.Item(7), reader.Item(8), reader.Item(9), reader.Item(10), reader.Item(11))
            If reader.Item(3) < 0 Then
                DGV1.Rows(i).DefaultCellStyle.BackColor = Color.Red
            ElseIf reader.Item(3) < reader.Item(4) * 0.1 And reader.Item(3) > 0 Then
                DGV1.Rows(i).DefaultCellStyle.BackColor = Color.Yellow
            ElseIf reader.Item(3) >= reader.Item(4) * 0.1 And reader.Item(3) > 0 Then
                DGV1.Rows(i).DefaultCellStyle.BackColor = Color.PaleGreen
            End If
            i += 1
        Loop
        reader.Close()
        con.Close()
        If DGV1.Rows.Count = 0 Then xguid = ""
        Dim stat As Boolean = False
        If xguid <> "" And DGV1.Rows.Count > 0 Then
            DGV1.Rows(0).Selected = False
            For i = 0 To DGV1.RowCount - 1
                If DGV1.Rows(i).Cells(0).Value = xguid Then
                    DGV1.Rows(i).Cells(1).Selected = True
                    If DGV1.Rows(i).Displayed = False Then DGV1.FirstDisplayedScrollingRowIndex = i
                    stat = True
                    Exit For
                End If
            Next
        End If
        If Not (stat) Then
            If DGV1.Rows.Count = 0 Then
                xguid = ""
            Else
                DGV1.Rows(0).Cells(1).Selected = True
                xguid = DGV1.CurrentRow.Cells(0).Value
            End If
        End If
        DGV1.Focus()
        loadxy()
        refresh_flag = False
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Call Button1_Click(sender, e)
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Call Button1_Click(sender, e)
    End Sub

    Private Sub DGV1_SelectionChanged(sender As Object, e As EventArgs) Handles DGV1.SelectionChanged
        If Not refresh_flag Then
            If DGV1.Rows.Count > 0 Then xguid = DGV1.CurrentRow.Cells(0).Value
            loadxy()
        End If

    End Sub

    Sub loadxy()
        If edit_flag Then
            edit_flag = False
            Button2.Text = "修改"
            Button3.Text = "新建"
            Call field_edit(False)
        End If
        If add_flag Then
            add_flag = False
            xguid = add_tmp
        End If
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim totalmoney As Double = 0
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        cmd.CommandText = "select top 1 * from 协议 where guid=@p1"
        cmd.Parameters.AddWithValue("p1", xguid)
        reader = cmd.ExecuteReader
        If reader.Read Then
            协议编号.Text = reader.Item("协议编号").ToString
            协议科室.SelectedItem = reader.Item("协议科室").ToString
            折扣.Text = reader.Item("折扣").ToString
            扣费类型.SelectedItem = reader.Item("扣费类型").ToString
            登记时间.Value = reader.Item("登记时间").ToString
            企业名称.Text = reader.Item("企业名称").ToString
            地址.Text = reader.Item("地址").ToString
            联系人.Text = reader.Item("联系人").ToString
            手机.Text = reader.Item("手机").ToString
            邮编.Text = reader.Item("邮编").ToString
            联系电话.Text = reader.Item("联系电话").ToString
            协议金额.Text = reader.Item("协议金额").ToString
            应收金额.Text = reader.Item("应收金额").ToString
            协议次数.Text = reader.Item("协议次数").ToString
            签订次数.Text = reader.Item("签订次数").ToString
            备注.Text = reader.Item("备注").ToString
            Label24.Text = reader.Item("经办人").ToString
            TextBox6.Text = reader.Item("协议编号").ToString
            TextBox7.Text = reader.Item("企业名称").ToString
            人员分组.Text = reader.Item("人员分组").ToString
            TextBox10.Text = 协议金额.Text
            TextBox11.Text = 应收金额.Text
            TextBox12.Text = 协议次数.Text
            TextBox13.Text = 签订次数.Text
            Label35.Text = reader.Item("扣费类型").ToString
            If reader.Item("扣费类型").ToString = "按次" Then
                TextBox90.Enabled = True
                TextBox9.Enabled = False
            Else
                TextBox9.Enabled = True
                TextBox90.Enabled = False
            End If
            If reader.Item("状态") = 4 Then use_stat = True Else use_stat = False
        Else
            协议编号.Text = ""
            协议科室.SelectedItem = ""
            折扣.Text = ""
            扣费类型.SelectedItem = ""
            登记时间.Value = Today
            企业名称.Text = ""
            地址.Text = ""
            联系人.Text = ""
            手机.Text = ""
            邮编.Text = ""
            联系电话.Text = ""
            协议金额.Text = ""
            应收金额.Text = ""
            协议次数.Text = ""
            签订次数.Text = ""
            备注.Text = ""
            人员分组.Text = ""
            Label24.Text = ""
            TextBox6.Text = ""
            TextBox7.Text = ""
            TextBox10.Text = ""
            TextBox11.Text = ""
            TextBox12.Text = ""
            TextBox13.Text = ""
            Label35.Text = ""
            xguid = ""
        End If
        reader.Close()
        cmd.Parameters.Clear()
        DGV2.Rows.Clear()
        cmd.CommandText = "select 创建日期,金额,次数,经办人,备注 from 协议扣费记录 where 金额>0 and xguid=@xguid order by 创建日期"
        cmd.Parameters.AddWithValue("xguid", xguid)
        reader = cmd.ExecuteReader
        Do While reader.Read
            DGV2.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4))
        Loop
        reader.Close()
        cmd.Parameters.Clear()
        DGV3.Rows.Clear()
        totalmoney = 0
        cmd.CommandText = "select b.报告编号,a.金额,a.创建日期,a.经办人,a.备注 from 协议扣费记录 a left outer join sys_view_检验任务 b on a.rguid=b.guid  where a.金额<0 and a.xguid=@xguid order by a.创建日期 desc"
        cmd.Parameters.AddWithValue("xguid", xguid)
        reader = cmd.ExecuteReader
        Do While reader.Read
            DGV3.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4))
            totalmoney += reader.Item(1)
        Loop
        reader.Close()
        con.Close()
        DGV3.Rows.Add("合计金额", totalmoney)
        If xguid = "" Or Not (use_stat) Then GroupBox2.Enabled = False Else GroupBox2.Enabled = True
        If xguid = "" Then GroupBox3.Enabled = False Else GroupBox3.Enabled = True
        If use_stat Then Button5.Text = "作废协议" Else Button5.Text = "生效协议"
        If 扣费类型.SelectedItem = "按费" Then
            TextBox1.Enabled = True
            TextBox2.Enabled = False
            TextBox3.Enabled = False
            TextBox3.Text = "0"
            TextBox2.Text = "0"
            TextBox1.Text = "0"
         
        Else
            TextBox1.Enabled = False
            TextBox2.Enabled = True
            TextBox3.Enabled = True
            If IsNumeric(应收金额.Text) And IsNumeric(签订次数.Text) Then
                If CInt(签订次数.Text) > 0 Then
                    TextBox2.Text = CStr(CDbl(应收金额.Text) / CInt(签订次数.Text))
                    TextBox3.Text = "0"
                End If
            Else
                TextBox3.Text = "0"
                TextBox2.Text = "0"
                TextBox1.Text = "0"
              
            End If
        End If
        TextBox90.Text = "0"
        TextBox9.Text = "0"
        TextBox4.Text = ""
        TextBox8.Text = ""
    End Sub
    Sub field_edit(ByRef edit_flag As Boolean)
        If edit_flag Then
            协议编号.ReadOnly = False
            协议科室.Enabled = True
            折扣.ReadOnly = False
            扣费类型.Enabled = True
            登记时间.Enabled = True
            企业名称.ReadOnly = False
            地址.ReadOnly = False
            联系人.ReadOnly = False
            手机.ReadOnly = False
            邮编.ReadOnly = False
            联系电话.ReadOnly = False
            备注.ReadOnly = False
            人员分组.ReadOnly = False
            GroupBox2.Enabled = False
            GroupBox3.Enabled = False
        Else
            协议编号.ReadOnly = True
            协议科室.Enabled = False
            折扣.ReadOnly = True
            扣费类型.Enabled = False
            登记时间.Enabled = False
            企业名称.ReadOnly = True
            地址.ReadOnly = True
            联系人.ReadOnly = True
            手机.ReadOnly = True
            邮编.ReadOnly = True
            联系电话.ReadOnly = True
            备注.ReadOnly = True
            人员分组.ReadOnly = True
            GroupBox2.Enabled = True
            GroupBox3.Enabled = True
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "修改" And xguid <> "" Then
            edit_flag = True
            Button2.Text = "取消"
            Button3.Text = "保存"
            Call field_edit(True)
        ElseIf Button2.Text = "取消" Then
            edit_flag = False
            add_flag = False
            Button2.Text = "修改"
            Button3.Text = "新建"
            Call field_edit(False)
            loadxy()
        ElseIf Button2.Text = "保存" Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            con.Open()
            cmd.Connection = con
            cmd.CommandText = "select top 1 * from 协议 where 协议编号=@协议编号 "
            cmd.Parameters.AddWithValue("协议编号", 协议编号.Text)
            reader = cmd.ExecuteReader
            If reader.Read Then
                reader.Close()
                con.Close()
                MessageBox.Show("协议编号和已有编号重复")
            Else
                reader.Close()
                cmd.Parameters.Clear()
                cmd.CommandText = "insert into 协议 (guid,经办人,协议编号,协议科室,企业名称,登记时间,地址,邮编,联系人,联系电话,手机,扣费类型,折扣,状态,备注,人员分组) values (@guid,@经办人,@协议编号,@协议科室,@企业名称,@登记时间,@地址,@邮编,@联系人,@联系电话,@手机,@扣费类型,@折扣,4,@备注,@人员分组)"
                cmd.Parameters.AddWithValue("协议编号", 协议编号.Text)
                cmd.Parameters.AddWithValue("协议科室", 协议科室.SelectedItem)
                cmd.Parameters.AddWithValue("企业名称", 企业名称.Text)
                cmd.Parameters.AddWithValue("登记时间", 登记时间.Value)
                cmd.Parameters.AddWithValue("地址", 地址.Text)
                cmd.Parameters.AddWithValue("邮编", 邮编.Text)
                cmd.Parameters.AddWithValue("联系电话", 联系电话.Text)
                cmd.Parameters.AddWithValue("手机", 手机.Text)
                cmd.Parameters.AddWithValue("折扣", CDec(折扣.Text))
                cmd.Parameters.AddWithValue("扣费类型", 扣费类型.SelectedItem)
                cmd.Parameters.AddWithValue("联系人", 联系人.Text)
                cmd.Parameters.AddWithValue("guid", xguid)
                cmd.Parameters.AddWithValue("经办人", glb_姓名)
                cmd.Parameters.AddWithValue("备注", 备注.Text)
                cmd.Parameters.AddWithValue("人员分组", 人员分组.Text)
                Call cmd.ExecuteNonQuery()
                con.Close()
                edit_flag = False
                add_flag = False
                Button2.Text = "修改"
                Button3.Text = "新建"
                Call field_edit(False)
                Call Button1_Click(sender, e)
            End If
           
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "保存" Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            con.ConnectionString = glb_sqlconstr
            con.Open()
            cmd.Connection = con
            cmd.CommandText = "update 协议 set 协议编号=@协议编号,协议科室=@协议科室,企业名称=@企业名称,登记时间=@登记时间,地址=@地址,邮编=@邮编,联系人=@联系人,联系电话=@联系电话,手机=@手机,扣费类型=@扣费类型,折扣=@折扣,备注=@备注,人员分组=@人员分组 where guid=@guid"
            cmd.Parameters.AddWithValue("协议编号", 协议编号.Text)
            cmd.Parameters.AddWithValue("协议科室", 协议科室.SelectedItem)
            cmd.Parameters.AddWithValue("企业名称", 企业名称.Text)
            cmd.Parameters.AddWithValue("登记时间", 登记时间.Value)
            cmd.Parameters.AddWithValue("地址", 地址.Text)
            cmd.Parameters.AddWithValue("邮编", 邮编.Text)
            cmd.Parameters.AddWithValue("联系电话", 联系电话.Text)
            cmd.Parameters.AddWithValue("手机", 手机.Text)
            cmd.Parameters.AddWithValue("折扣", CDec(折扣.Text))
            cmd.Parameters.AddWithValue("扣费类型", 扣费类型.SelectedItem)
            cmd.Parameters.AddWithValue("联系人", 联系人.Text)
            cmd.Parameters.AddWithValue("guid", xguid)
            cmd.Parameters.AddWithValue("备注", 备注.Text)
            cmd.Parameters.AddWithValue("人员分组", 人员分组.Text)
            Call cmd.ExecuteNonQuery()
            con.Close()
            edit_flag = False
            Button2.Text = "修改"
            Button3.Text = "新建"
            Call field_edit(False)
            Call Button1_Click(sender, e)
        ElseIf Button3.Text = "新建" Then
            add_tmp = xguid
            edit_flag = True
            add_flag = True
            Call field_edit(True)
            Button2.Text = "保存"
            Button3.Text = "取消"
            xguid = System.Guid.NewGuid.ToString
            协议编号.Text = ""
            协议科室.SelectedItem = ""
            折扣.Text = ""
            扣费类型.SelectedItem = ""
            登记时间.Value = Today
            企业名称.Text = ""
            地址.Text = ""
            联系人.Text = ""
            手机.Text = ""
            邮编.Text = ""
            联系电话.Text = ""
            协议金额.Text = ""
            应收金额.Text = ""
            协议次数.Text = ""
            签订次数.Text = ""
            备注.Text = ""
            人员分组.Text = ""
            Label24.Text = glb_姓名
        ElseIf Button3.Text = "取消" Then
            edit_flag = False
            Button2.Text = "修改"
            Button3.Text = "新建"
            Call field_edit(False)
            xguid = add_tmp
            loadxy()
        End If
    End Sub

  
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If IsNumeric(TextBox1.Text) Then
            If CDbl(TextBox1.Text) = 0 Then
                MessageBox.Show("充值金额不能为0")
                Exit Sub
            End If
        End If
        If 扣费类型.SelectedItem = "按费" And Not (IsNumeric(TextBox1.Text)) Then
            MessageBox.Show("充值金额应为数字,请重新输入!", "输入错误", MessageBoxButtons.OK)
            TextBox1.Focus()
        ElseIf 扣费类型.SelectedItem = "按次" And Not (IsNumeric(TextBox2.Text) And IsNumeric(TextBox3.Text)) Then
            MessageBox.Show("充值次数及单次金额应为数字,请重新输入!", "输入错误", MessageBoxButtons.OK)
            TextBox2.Focus()
        Else
            If MessageBox.Show("确定" + 协议编号.Text + "充值" + TextBox1.Text + "元?", "确定充值", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim con As New SqlConnection
                Dim cmd As New SqlCommand
                con.ConnectionString = glb_sqlconstr
                con.Open()
                cmd.Connection = con
                cmd.CommandText = "update 协议 set 应收金额=@金额+isnull(应收金额,0),签订次数=@次数+isnull(签订次数,0) where guid=@xguid"
                cmd.Parameters.AddWithValue("金额", CDbl(TextBox1.Text))
                cmd.Parameters.AddWithValue("次数", CInt(TextBox3.Text))
                cmd.Parameters.AddWithValue("xguid", xguid)
                cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
                cmd.CommandText = "insert into 协议扣费记录 (guid,xguid,协议编号,经办人,创建日期,金额,次数,备注) values (newid(),@xguid,@协议编号,@经办人,getdate(),@协议金额,@协议次数,@备注)"
                cmd.Parameters.AddWithValue("xguid", xguid)
                cmd.Parameters.AddWithValue("协议编号", 协议编号.Text)
                cmd.Parameters.AddWithValue("经办人", glb_姓名)
                cmd.Parameters.AddWithValue("备注", TextBox4.Text)
                cmd.Parameters.AddWithValue("协议金额", CDbl(TextBox1.Text))
                cmd.Parameters.AddWithValue("协议次数", CInt(TextBox3.Text))
                cmd.ExecuteNonQuery()
                con.Close()
                Call Button1_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub 导出快捷报表_Click(sender As Object, e As EventArgs) Handles 导出快捷报表.Click
        Dim dgv As Object
        dgv = dgvmenu.SourceControl
        If dgv.Rows.Count = 0 Then
            MessageBox.Show("没有记录")
            Exit Sub
        End If
        Dim App As New Excel.Application
        Dim Book As Excel.Workbook
        Dim sheet As Excel.Worksheet
        Book = App.Workbooks.Add
        sheet = Book.Sheets(1)

        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = dgv.Rows.Count - 1
        ProgressBar1.Value = 0
        ProgressBar1.Width = dgv.Width / 2
        ProgressBar1.Left = dgv.Left + dgv.Width / 4
        ProgressBar1.Top = dgv.Top + dgv.Height / 2 - 15
        ProgressBar1.Visible = True

        Dim colnum, rownum, num As Integer

        sheet.Cells(1, 1) = "序号"
        colnum = 1
        rownum = 1
        num = 0
        For i = 0 To dgv.Columns.Count - 1
            If dgv.Columns(i).Visible = True Then
                colnum += 1
                sheet.Cells(rownum, colnum) = dgv.Columns(i).headertext
            End If
        Next

        For i = 0 To dgv.Rows.Count - 1
            colnum = 1
            If dgv.Rows(i).Visible = True Then
                num += 1
                rownum += 1
                sheet.Cells(rownum, colnum) = num
                For j = 0 To dgv.Columns.Count - 1
                    If dgv.Columns(j).Visible = True Then
                        colnum += 1
                        sheet.Cells(rownum, colnum) = dgv.Rows(i).Cells(j).Value
                    End If
                Next
            End If
            ProgressBar1.Value = i
        Next
        ProgressBar1.Visible = False
        App.Visible = True
    End Sub

   
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If xguid = "" Then Exit Sub
        If use_stat Then
            If MessageBox.Show("确定作废编号为" + 协议编号.Text + "的协议?", "确定作废", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then Exit Sub
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            con.ConnectionString = glb_sqlconstr
            con.Open()
            cmd.Connection = con
            cmd.CommandText = "update 协议 set 状态=8 where guid=@xguid"
            cmd.Parameters.AddWithValue("xguid", xguid)
            cmd.ExecuteNonQuery()
            con.Close()
            Button1_Click(sender, e)
        Else
            If MessageBox.Show("确定生效编号为" + 协议编号.Text + "的已作废协议?", "确定生效", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then Exit Sub
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            con.ConnectionString = glb_sqlconstr
            con.Open()
            cmd.Connection = con
            cmd.CommandText = "update 协议 set 状态=4 where guid=@xguid"
            cmd.Parameters.AddWithValue("xguid", xguid)
            cmd.ExecuteNonQuery()
            con.Close()
            Button1_Click(sender, e)
        End If
    End Sub

    Private Sub 企业名称_KeyDown(sender As Object, e As KeyEventArgs) Handles 企业名称.KeyDown
        If e.KeyCode = Keys.Down And ListBox1.Visible = True Then
            ListBox1.SelectedIndex = 0
            ListBox1.Select()
        End If
    End Sub

    Private Sub 企业名称_TextChanged(sender As Object, e As EventArgs) Handles 企业名称.TextChanged
        If 企业名称.Text <> "" And 企业名称.ReadOnly = False Then
            Dim itemheight As Integer = 23
            Dim cboxheight As Integer = 0
            Dim itemstr As String = ""
            Dim range As New System.Drawing.Rectangle
            Dim dLeft, dTop As Double
            dLeft = 企业名称.Left
            dTop = 企业名称.Bottom
            ListBox1.Top = dTop + GroupBox1.Top
            ListBox1.Left = dLeft + GroupBox1.Left
            ListBox1.Width = 企业名称.Width
            ListBox1.Items.Clear()

            Dim n As Integer = 0
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "select 企业名称 from 企业名录 where 企业名称 like @名称 order by 企业名称"

            cmd.Parameters.AddWithValue("名称", "%" + 企业名称.Text + "%")
            reader = cmd.ExecuteReader
            Do While reader.Read
                ListBox1.Items.Add(reader.Item(0).ToString)
                n += 1
            Loop
            reader.Close()
            con.Close()
            If n > 20 Then n = 20
            If n = 0 Then
                ListBox1.Visible = False
            Else
                ListBox1.Height = n * 16 + 10
                ListBox1.Visible = True
            End If

        Else
            ListBox1.Visible = False
        End If
    End Sub

    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click
        企业名称.Text = ListBox1.SelectedItem.ToString
        Call fill_entinfo(企业名称.Text)
        ListBox1.Visible = False
    End Sub

    Private Sub ListBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            企业名称.Text = ListBox1.SelectedItem.ToString
            Call fill_entinfo(企业名称.Text)
            ListBox1.Visible = False
        End If
        If e.KeyCode = Keys.Up And ListBox1.SelectedIndex = 0 Then
            企业名称.Focus()
        End If
    End Sub
    Sub fill_entinfo(ByVal ent_name As String)
        If ent_name = "" Then Exit Sub
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        Dim reader As SqlDataReader
        cmd.CommandText = "select top 1 地址,邮编,联系人,联系电话,手机 from 企业名录 where 企业名称 = @p1"
        cmd.Parameters.AddWithValue("p1", ent_name)
        reader = cmd.ExecuteReader
        If reader.Read Then
            地址.Text = reader.Item(0).ToString
            邮编.Text = reader.Item(1).ToString
            联系人.Text = reader.Item(2).ToString
            联系电话.Text = reader.Item(3).ToString
            手机.Text = reader.Item(4).ToString
        End If
        reader.Close()
        con.Close()
    End Sub


    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If IsNumeric(TextBox9.Text) Then
            If CDbl(TextBox9.Text) = 0 Then
                MessageBox.Show("金额不能为0")
                Exit Sub
            End If
        End If
        Dim act_text As String = ""
        If RadioButton1.Checked Then act_text = "增加" Else act_text = "扣除"
        If Label35.Text = "按费" And Not (IsNumeric(TextBox9.Text)) Then
            MessageBox.Show("金额应为数字,请重新输入!", "输入错误", MessageBoxButtons.OK)
            TextBox9.Focus()
        ElseIf Label35.Text = "按次" And Not (IsNumeric(TextBox90.Text)) Then
            MessageBox.Show("次数应为数字,请重新输入!", "输入错误", MessageBoxButtons.OK)
            TextBox90.Focus()
        Else
            If MessageBox.Show("确定" + 协议编号.Text + act_text + TextBox9.Text + "元?", "确定调整", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim con As New SqlConnection
                Dim cmd As New SqlCommand
                con.ConnectionString = glb_sqlconstr
                con.Open()
                cmd.Connection = con
                cmd.Parameters.Clear()
                cmd.CommandText = "insert into 协议扣费记录 (guid,xguid,协议编号,经办人,创建日期,金额,次数,备注) values (newid(),@xguid,@协议编号,@经办人,getdate(),@协议金额,@协议次数,@备注)"
                cmd.Parameters.AddWithValue("xguid", xguid)
                cmd.Parameters.AddWithValue("协议编号", 协议编号.Text)
                cmd.Parameters.AddWithValue("经办人", glb_姓名)
                cmd.Parameters.AddWithValue("备注", TextBox8.Text)
                If RadioButton1.Checked Then
                    cmd.Parameters.AddWithValue("协议金额", CDbl(TextBox9.Text))
                    cmd.Parameters.AddWithValue("协议次数", CInt(TextBox90.Text))
                Else
                    cmd.Parameters.AddWithValue("协议金额", 0 - CDbl(TextBox9.Text))
                    cmd.Parameters.AddWithValue("协议次数", 0 - CInt(TextBox90.Text))
                End If
                cmd.ExecuteNonQuery()
                'If RadioButton1.Checked Then
                '    cmd.Parameters.Clear()
                '    cmd.CommandText = "update 协议 set 应收金额=应收金额+@协议金额,签订次数=签订次数+@协议次数 where guid=@xguid"
                '    cmd.Parameters.AddWithValue("协议金额", CDbl(TextBox9.Text))
                '    cmd.Parameters.AddWithValue("协议次数", CInt(TextBox90.Text))
                '    cmd.Parameters.AddWithValue("xguid", xguid)
                '    cmd.ExecuteNonQuery()
                'End If
                con.Close()
                Call Button1_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub TabPage4_Enter(sender As Object, e As EventArgs) Handles TabPage4.Enter
        loadxy()
    End Sub
    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

        If IsNumeric(TextBox2.Text) And IsNumeric(TextBox3.Text) Then
            TextBox3.Text = CInt(TextBox3.Text).ToString
            TextBox1.Text = CDbl(TextBox2.Text) * CInt(TextBox3.Text)
        End If

    End Sub
    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If IsNumeric(TextBox2.Text) And IsNumeric(TextBox3.Text) Then TextBox1.Text = CDbl(TextBox2.Text) * CInt(TextBox3.Text)
    End Sub
   
    Private Sub TextBox90_TextChanged(sender As Object, e As EventArgs) Handles TextBox90.TextChanged
        If IsNumeric(TextBox2.Text) And IsNumeric(TextBox90.Text) Then
            TextBox90.Text = CInt(TextBox90.Text).ToString
            TextBox9.Text = CDbl(TextBox2.Text) * CInt(TextBox90.Text)
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub

    Private Sub TextBox5_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox5.KeyDown
        If e.KeyCode = Keys.Down And ListBox2.Visible = True Then
            ListBox2.SelectedIndex = 0
            ListBox2.Select()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        If TextBox5.Text <> "" Then
            Dim itemheight As Integer = 23
            Dim cboxheight As Integer = 0
            Dim itemstr As String = ""
            Dim range As New System.Drawing.Rectangle
            Dim dLeft, dTop As Double
            dLeft = TextBox5.Left
            dTop = TextBox5.Bottom
            ListBox2.Top = dTop
            ListBox2.Left = dLeft
            ListBox2.Width = TextBox5.Width
            ListBox2.Items.Clear()

            Dim n As Integer = 0
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "select 企业名称 from 协议 where 企业名称 like @名称 and 状态=@状态 and (协议编号 like @年份 or @年份='%全部年份%') order by 企业名称"
            cmd.Parameters.AddWithValue("名称", "%" + TextBox5.Text + "%")
            If ComboBox2.SelectedIndex = 0 Then cmd.Parameters.AddWithValue("状态", 4) Else cmd.Parameters.AddWithValue("状态", 8)
            cmd.Parameters.AddWithValue("年份", "%" + ComboBox1.SelectedItem.ToString + "%")
            reader = cmd.ExecuteReader
            Do While reader.Read
                ListBox2.Items.Add(reader.Item(0).ToString)
                n += 1
            Loop
            reader.Close()
            con.Close()
            If n > 20 Then n = 20
            If n = 0 Then ListBox2.Height = 50 Else ListBox2.Height = n * 16 + 10
            ListBox2.Visible = True
        Else
            ListBox2.Visible = False
        End If
    End Sub

    Private Sub ListBox2_Click(sender As Object, e As EventArgs) Handles ListBox2.Click
        TextBox5.Text = ListBox2.SelectedItem.ToString
        ListBox2.Visible = False
        Call Button1_Click(sender, e)
    End Sub

    Private Sub ListBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextBox5.Text = ListBox2.SelectedItem.ToString
            ListBox2.Visible = False
            Call Button1_Click(sender, e)
        End If
        If e.KeyCode = Keys.Up And ListBox1.SelectedIndex = 0 Then
            TextBox5.Focus()
        End If
    End Sub

End Class