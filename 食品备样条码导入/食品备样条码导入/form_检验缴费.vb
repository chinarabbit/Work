Imports System.Data.SqlClient

Public Class form_检验缴费
    Public jpjf As Boolean = True
    Public pguid As String = ""
    Private Sub form_检验缴费_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label8.Text = ""
        Label13.Text = ""
        Label10.Text = glb_姓名
        TextBox2.BackColor = Color.White
        TextBox2.ForeColor = Color.Red
        ComboBox2.SelectedIndex = 0
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con

        If jpjf Then
            cmd.CommandText = "select guid,缴费单编号 from 缴费单 where 状态=0 order by 缴费单编号"
            DGV1.Columns(1).Name = "缴费单编号"
            DGV1.Columns(1).HeaderText = "缴费单编号"
        Else
            cmd.CommandText = "select guid,协议编号 from 协议 where 状态=4 order by 协议编号"
            DGV1.Columns(1).Name = "协议编号"
            DGV1.Columns(1).HeaderText = "协议编号"
        End If
        reader = cmd.ExecuteReader
        DGV1.Rows.Clear()
        Do While reader.Read
            DGV1.Rows.Add(reader.Item(0), reader.Item(1))
        Loop
        reader.Close()
        con.Close()

    End Sub


    Private Sub DGV1_SelectionChanged(sender As Object, e As EventArgs) Handles DGV1.SelectionChanged
        dgv2.Rows.Clear()
        TextBox4.Text = ""
        TextBox5.Text = ""
        Label6.Text = ""
        Dim rguidarray, jyfarray As New ArrayList
        Dim je As Long = 0
        Dim hjjyf As Long = 0
        If DGV1.CurrentRow.Cells("guid").Value <> "" Then pguid = DGV1.CurrentRow.Cells("guid").Value
        If pguid <> "" Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            If jpjf Then
                Label9.Text = "缴费单编号"
                dgv2.Columns("报告编号").Visible = True
                dgv2.Columns("样品名称").Visible = True
                dgv2.Columns("检验费").Visible = True
                dgv2.Columns("主检科室").Visible = True
                dgv2.Columns("缴费日期").Visible = False
                dgv2.Columns("缴费金额").Visible = False
                dgv2.Columns("凭证号码").Visible = False
                dgv2.Columns("备注").Visible = False
                dgv2.Columns("经手人").Visible = False
                TextBox2.ReadOnly = True
                cmd.CommandText = "select a.guid,a.报告编号,a.样品名称,isnull(a.检验费,0) as 检验费,a.主检科室 from sys_view_检验任务 a inner join 缴费记录 b on a.guid=b.rguid where b.pguid='" + pguid + "' order by a.报告编号"
                reader = cmd.ExecuteReader
                Do While reader.Read
                    dgv2.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4))
                    hjjyf += reader.Item(3)
                    rguidarray.Add(reader.Item(0))
                    jyfarray.Add(reader.Item(3))
                Loop
                reader.Close()
                cmd.CommandText = "select * from 缴费单 where guid='" + pguid + "'"
                reader = cmd.ExecuteReader
                If reader.Read Then
                    Label8.Text = reader.Item("缴费单编号")
                    TextBox1.Text = reader.Item("付款单位")
                    If IsDBNull(reader.Item("应收金额")) Then je = 0 Else je = reader.Item("应收金额")
                End If
                reader.Close()
                If je <> hjjyf Then
                    cmd.CommandText = "update 缴费单 set 应收金额=" + hjjyf.ToString + " where guid='" + pguid + "'"
                    cmd.ExecuteNonQuery()
                    For i = 0 To rguidarray.Count - 1
                        cmd.CommandText = "update 缴费记录 set 应收金额=" + jyfarray(i).ToString + " where rguid='" + rguidarray(i) + "'"
                        cmd.ExecuteNonQuery()
                    Next
                End If
                TextBox2.Text = hjjyf.ToString("0.00")
            Else
                Label9.Text = "协议编号"
                dgv2.Columns("报告编号").Visible = False
                dgv2.Columns("样品名称").Visible = False
                dgv2.Columns("检验费").Visible = False
                dgv2.Columns("主检科室").Visible = False
                dgv2.Columns("缴费日期").Visible = True
                dgv2.Columns("缴费金额").Visible = True
                dgv2.Columns("凭证号码").Visible = True
                dgv2.Columns("备注").Visible = True
                dgv2.Columns("经手人").Visible = True

                TextBox2.ReadOnly = False
                cmd.CommandText = "select 协议编号,企业名称,协议金额 from 协议 where guid='" + pguid + "'"
                reader = cmd.ExecuteReader
                If reader.Read Then
                    Label8.Text = reader.Item("协议编号")
                    TextBox1.Text = reader.Item("企业名称")
                    TextBox2.Text = ""
                    Label6.Text = "当前协议金额:" + reader.Item("协议金额").ToString
                End If
                reader.Close()
                cmd.CommandText = "select 创建日期,金额,凭证号码,备注,经办人 from  协议扣费记录 where 缴费状态=1 and xguid='" + pguid + "'"
                reader = cmd.ExecuteReader
                Do While reader.Read
                    dgv2.Rows.Add("", "", "", "", "", reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4))
                Loop
                reader.Close()
            End If
            con.Close()
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then jpjf = False
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then jpjf = True
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text <> "" Then Label13.Text = upper_money(TextBox2.Text) Else Label13.Text = "零元"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Trim(TextBox4.Text) = "" Then
            If MessageBox.Show("没有登记发票编号,确认保存?", "请选择", MessageBoxButtons.YesNo) = DialogResult.No Then
                TextBox4.Focus()
                Exit Sub
            End If
        End If

        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()

        If jpjf Then
            cmd.CommandText = "update 缴费单 set 凭证号码=@凭证号码,付款方式=@付款方式,经办人=@经办人,付款日期=getdate(),状态=1,备注=@备注 where guid='" + pguid + "'"
            cmd.Parameters.AddWithValue("凭证号码", TextBox4.Text)
            cmd.Parameters.AddWithValue("付款方式", ComboBox2.SelectedItem)
            cmd.Parameters.AddWithValue("备注", TextBox5.Text)
            cmd.Parameters.AddWithValue("经办人", glb_姓名)
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update 缴费记录 set 收费状态=1 where pguid='" + pguid + "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update 检验任务 set 收费情况='已收费' where guid in (select rguid from 缴费记录 where pguid='" + pguid + "')"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update 已完成检验任务 set 收费情况='已收费' where guid in (select rguid from 缴费记录 where pguid='" + pguid + "')"
            cmd.ExecuteNonQuery()
            Button1_Click(sender, e)
        Else
            If IsNumeric(TextBox2.Text) Then
                If CDbl(TextBox2.Text) = 0 Then
                    MessageBox.Show("充值金额不能为0")
                    Exit Sub
                End If
            End If
            If Not (IsNumeric(TextBox2.Text)) Then
                MessageBox.Show("充值金额应为数字,请重新输入!", "输入错误", MessageBoxButtons.OK)
                TextBox2.Focus()
            Else
                If MessageBox.Show("确定" + Label8.Text + "充值" + TextBox2.Text + "元?", "确定充值", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    cmd.CommandText = "update 协议 set 应收金额=@金额+isnull(应收金额,0) where guid=@xguid"
                    cmd.Parameters.AddWithValue("金额", CDbl(TextBox2.Text))
                    cmd.Parameters.AddWithValue("xguid", pguid)
                    cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                    cmd.CommandText = "insert into 协议扣费记录 (guid,xguid,协议编号,经办人,创建日期,金额,备注,缴费状态,凭证号码) values (newid(),@xguid,@协议编号,@经办人,getdate(),@协议金额,@备注,1,@凭证号码)"
                    cmd.Parameters.AddWithValue("xguid", pguid)
                    cmd.Parameters.AddWithValue("协议编号", Label8.Text)
                    cmd.Parameters.AddWithValue("经办人", glb_姓名)
                    cmd.Parameters.AddWithValue("备注", TextBox5.Text)
                    cmd.Parameters.AddWithValue("协议金额", CDbl(TextBox2.Text))
                    cmd.Parameters.AddWithValue("凭证号码", TextBox4.Text)
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "select 协议金额 from 协议 where guid='" + pguid + "'"
                    reader = cmd.ExecuteReader
                    If reader.Read Then
                        Label6.Text = "当前协议金额:" + reader.Item(0).ToString
                    End If
                    reader.Close()
                    dgv2.Rows.Clear()
                    cmd.CommandText = "select 创建日期,金额,凭证号码,备注,经办人 from  协议扣费记录 where 缴费状态=1 and xguid='" + pguid + "'"
                    reader = cmd.ExecuteReader
                    Do While reader.Read
                        dgv2.Rows.Add("", "", "", "", reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4))
                    Loop
                    reader.Close()
                End If
            End If
        End If



        con.Close()
    End Sub
End Class