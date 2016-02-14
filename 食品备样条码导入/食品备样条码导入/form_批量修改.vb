Imports System.Data.SqlClient
Public Class form_批量修改
    Dim datatype As String = ""
    Dim datatable As String = ""

    Private Sub form_批量修改_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select 名称  from lookup where  type='委托检验' or type='监督检验' order by 序号 "
        reader = cmd.ExecuteReader
        Me.ComboBox1.Items.Add("全部类别")
        Do While reader.Read
            ComboBox1.Items.Add(reader.Item(0).ToString)
        Loop
        reader.Close()

        cmd.CommandText = "select 名称 from lookup where type= 'zyn_批量修改字段' order by 序号 "
        reader = cmd.ExecuteReader
        Do While reader.Read
            ComboBox2.Items.Add(reader.Item(0).ToString)
        Loop
        reader.Close()
        con.Close()
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim cmd As New SqlCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "zyn_读取批量修改任务"
        cmd.Parameters.AddWithValue("date1", DateTimePicker1.Value.Date)
        cmd.Parameters.AddWithValue("date2", DateTimePicker2.Value.Date)
        If RadioButton2.Checked Then cmd.Parameters.AddWithValue("受理人", glb_姓名)
        If ComboBox1.SelectedIndex <> 0 Then cmd.Parameters.AddWithValue("检验类型", ComboBox1.SelectedItem)
        Mydgv1.load_data(cmd)
        Label4.Visible = False
        Button2.Visible = False
        TextBox1.Visible = False
        DateTimePicker3.Visible = False
    End Sub

    Private Sub Mydgv1_Load(sender As Object, e As EventArgs) Handles Mydgv1.Load

    End Sub

    Private Sub form_批量修改_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Mydgv1.Width = Me.Width - 449
        Mydgv1.Height = Me.Height - 84
        ListBox1.Visible = False
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim tbname, colname As String
        tbname = "检验任务"
        colname = ComboBox2.SelectedItem
        Dim con As New SqlConnection
        con.ConnectionString = glb_sqlconstr
        con.Open()
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.CommandText = "select b.name 'DataType' from syscolumns a inner join systypes b on a.xtype=b.xtype where a.id=object_id('" + tbname + "') and a.name='" + colname + "'"
        Dim reader As SqlDataReader
        reader = cmd.ExecuteReader
        If reader.Read Then datatype = reader.Item(0).ToString

        If datatype = "varchar" Or datatype = "money" Then
            Label4.Visible = True
            Button2.Visible = True
            TextBox1.Visible = True
            DateTimePicker3.Visible = False
        ElseIf datatype = "datetime" Then
            Label4.Visible = True
            Button2.Visible = True
            TextBox1.Visible = False
            DateTimePicker3.Visible = True
        Else
            Label4.Visible = False
            Button2.Visible = False
            TextBox1.Visible = False
            DateTimePicker3.Visible = False
        End If

        reader.Close()
        con.Close()
        Mydgv1.display_col(colname)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim colname As String
        colname = ComboBox2.SelectedItem
        If (TextBox1.Text = "" And TextBox1.Visible = True) Then
            MessageBox.Show("请输入内容")
            TextBox1.Focus()
            Exit Sub
        End If

        If datatype = "money" And TextBox1.Visible = True Then
            If Not IsNumeric(TextBox1.Text) Then
                MessageBox.Show(colname + "是数字类型字段,请输入数字")
                TextBox1.Focus()
                Exit Sub
            End If
        End If
        Dim myarray, bianhao, old, jindu As ArrayList
        myarray = Mydgv1.getselect_col("Guid")
        bianhao = Mydgv1.getselect_col("报告编号")
        old = Mydgv1.getselect_col(colname)
        jindu = Mydgv1.getselect_col("进度")
        If myarray.Count = 0 Then
            MessageBox.Show("请选择要修改的记录")
            Exit Sub
        End If

        Dim Message As String = "选中" + myarray.Count.ToString + "条记录,确定批量修改" + colname + "字段值?"
        Dim Caption As String = "确定修改"
        Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim Result As DialogResult
        Result = MessageBox.Show(Message, Caption, Buttons)
        If Result = Windows.Forms.DialogResult.No Then Exit Sub
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con

        Dim sqlstr, sql2 As String
        sqlstr = " set " + colname + "="
        sql2 = "insert into zyn_批量修改记录 (guid,修改字段,修改人,新内容,报告编号,原内容) values (newid(),'" + colname + "','" + glb_姓名 + "','"
        If datatype = "varchar" Then
            sqlstr = sqlstr + "'" + TextBox1.Text + "' where guid='"
            sql2 = sql2 + TextBox1.Text + "'"
        ElseIf datatype = "money" Then
            sqlstr = sqlstr + TextBox1.Text + " where guid='"
            sql2 = sql2 + TextBox1.Text.ToString + "'"
        ElseIf datatype = "datetime" Then
            sqlstr = sqlstr + "'" + DateTimePicker3.Value + "' where guid='"
            sql2 = sql2 + DateTimePicker3.Value.ToString + "'"
        End If
        con.Open()
        For i = 0 To myarray.Count - 1
            If jindu(i) = "20" Then cmd.CommandText = "update 已完成检验任务 " + sqlstr + myarray(i).ToString + "'" Else cmd.CommandText = "update 检验任务 " + sqlstr + myarray(i).ToString + "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = sql2 + ",'" + bianhao(i) + "','" + old(i) + "')"
            cmd.ExecuteNonQuery()
        Next
        con.Close()
        MessageBox.Show("批量修改已完成,要继续操作请重新导入数据!")
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        If ComboBox3.SelectedIndex >= 0 And TextBox3.Text <> "" Then
            Dim itemheight As Integer = 23
            Dim cboxheight As Integer = 0
            Dim itemstr As String = ""
            Dim range As New System.Drawing.Rectangle
            Dim dLeft, dTop As Double
            dLeft = TextBox3.Left + Panel1.Left
            dTop = TextBox3.Bottom + Panel1.Top
            ListBox1.Top = dTop
            ListBox1.Left = dLeft
            ListBox1.Width = TextBox3.Width
            ListBox1.Items.Clear()
            Dim n As Integer = 0
            Dim sql As String
            sql = "select distinct top 100 " + ComboBox3.SelectedItem.ToString + " from sys_view_检验任务 where " + ComboBox3.SelectedItem.ToString + " like @p1"
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = sql
            cmd.Parameters.AddWithValue("p1", "%" + TextBox3.Text + "%")
            reader = cmd.ExecuteReader
            Do While reader.Read
                ListBox1.Items.Add(reader.Item(0).ToString)
                n += 1
            Loop
            reader.Close()
            con.Close()
            If n > 20 Then n = 20
            If n = 0 Then ListBox1.Height = 50 Else ListBox1.Height = n * 16 + 10
            ListBox1.Visible = True
        Else
            ListBox1.Visible = False
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        TextBox3.Text = ""
        TextBox3.Focus()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Trim(TextBox3.Text) <> "" And ComboBox3.SelectedIndex >= 0 Then
            Dim sql As String
            Dim cmd As New SqlCommand
            ' cmd.CommandType = CommandType.StoredProcedure
            ' cmd.CommandText = "zyn_读取批量修改任务"
            sql = "SELECT [GUID],[创建日期],[任务类型],[检验类型],[主检科室],[报告编号],[协议编号],[计划编号],[样品名称],[样品数],[样品等级],[样品来源],[规格型号],[商标],[生产日期],[保质期],[送样人],[样品堆放方式],[样品处理意见],[样品特性],[样品状态],[到样日期],[抽样基数],[抽样数量],[抽样日期],[抽样人],[抽样地点],[抽样方式],[抽样单编号],[委托单编号],[委托单位],[委托单位地址],[委托单位联系人],[委托单位邮编],[委托单位电话],[委托单位传真],[受检单位],[受检单位地址],[受检单位联系人],[受检单位电话],[受检单位所在省份],[受检单位所在城市],[受检单位所在地区],[受检单位营业执照],[生产单位],[生产单位地址],[生产单位联系人],[生产单位邮编],[生产单位电话],[生产单位手机],[生产单位法人代表],[生产单位所在省份],[生产单位所在城市],[生产单位所在地区],[生产单位营业执照],[生产单位机构代码],[抽样单位],[抽样单位地址],[抽样单位联系人],[抽样单位邮编],[抽样单位电话],[抽样单位传真],[抽样单位电邮],[接收短信手机号],[委托日期],[分派日期],[下达日期],[商定完成日期],[要求完成日期],[任务来源],[许可证类型],[许可证编号],[检查封样人员],[检验依据],[标注执行标准],[备注],[应收费],[检验费],[收费情况],[受理人],[留言],[当前进度],[产品类别代码],[产品代码四级],[业态类型],[抽样编号],[进度] FROM [KingsLims].[dbo].[sys_view_检验任务] where " + ComboBox3.SelectedItem.ToString + "='" + TextBox3.Text + "'"
            cmd.CommandText = sql
            Mydgv1.load_data(cmd)
            Label4.Visible = False
            Button2.Visible = False
            TextBox1.Visible = False
            DateTimePicker3.Visible = False
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        TextBox3.Text = ListBox1.SelectedItem.ToString
        ListBox1.Visible = False
        Button4_Click(sender, e)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub DateTimePicker3_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker3.ValueChanged

    End Sub
End Class