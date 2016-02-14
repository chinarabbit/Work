Imports System.ComponentModel
Imports System.Data.SqlClient, Microsoft.Office.Interop

Public Class form_报告编制
    Public guid As String = ""
    Public load_flag As Boolean = False
    Public pubcon As New SqlConnection
    Public pubcmd As New SqlCommand
    Public pubada As New SqlDataAdapter
    Public pubcmdbuilder As New SqlCommandBuilder
    Public pubtable As New DataTable
    Public pubbinds As New BindingSource
    Public pubcon2 As New SqlConnection
    Public pubcmd2 As New SqlCommand
    Public pubada2 As New SqlDataAdapter
    Public pubcmdbuilder2 As New SqlCommandBuilder
    Public pubtable2 As New DataTable
    Public pubbinds2 As New BindingSource
    Dim cur_taskstep As Integer

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If load_flag Then Button1_Click(sender, e)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button2.Enabled Then
            Dim btn As DialogResult
            btn = MessageBox.Show("是否保存当前信息?", "保存提示", MessageBoxButtons.YesNo)
            If btn = DialogResult.Yes Then
                Button2_Click(sender, e)
            ElseIf btn = DialogResult.No Then
                Button10_Click(sender, e)
            End If
        End If
        load_flag = False
        Dim cmd As New SqlCommand
        Dim jd As Integer = -1
        cmd.CommandText = "select guid,报告编号,样品名称,主检科室,检验类型,计划编号,协议编号,抽样单编号,受检单位,生产单位,抽样单位,下达日期,商定完成日期,要求完成日期,检验结束日期,主检,受理人,当前进度,委托单位地址,加急,锁定,检验依据 from 检验任务 where 进度=@进度 and (主检=@主检 or @主检='') and (主检科室=@主检科室 or @主检科室='') and (报告编号 like @报告编号 or @报告编号='') order by 报告编号"
        Select Case ComboBox1.SelectedItem
            Case "项目检验"
                jd = 5
            Case "报告编制"
                jd = 8
        End Select
        If jd > -1 Then cmd.Parameters.AddWithValue("进度", jd) Else Exit Sub
        If glb_loginname = "sa" Then
            cmd.Parameters.AddWithValue("主检", "")
            cmd.Parameters.AddWithValue("主检科室", "")
        Else
            If InStr(glb_auth, "|报告编制|") > 0 Then cmd.Parameters.AddWithValue("主检", "") Else cmd.Parameters.AddWithValue("主检", glb_姓名)
            cmd.Parameters.AddWithValue("主检科室", glb_科室)
        End If
        If Trim(TextBox8.Text) <> "" Then cmd.Parameters.AddWithValue("报告编号", "%" + Trim(TextBox8.Text) + "%") Else cmd.Parameters.AddWithValue("报告编号", "")
        TextBox8.Text = ""
        Mydgv1.load_data(cmd)

        Mydgv1.select_cell(0, "报告编号")
        load_flag = True
        Call data_refresh(sender, e)

    End Sub



    Private Sub form_报告编制_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        SplitContainer1.Height = Me.Height - 35
        SplitContainer1.Width = Me.Width - 2
        FlowLayoutPanel3.Width = Me.Width - 2
    End Sub

    Private Sub SplitContainer1_Resize(sender As Object, e As EventArgs) Handles SplitContainer1.Resize
        Mydgv1.Width = SplitContainer1.Panel1.Width
        Mydgv1.Height = SplitContainer1.Height - 76
        TabControl1.Width = SplitContainer1.Panel2.Width - 15
        TabControl1.Height = SplitContainer1.Height - 40
    End Sub
    Private Sub SplitContainer1_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Paint
        Mydgv1.Width = SplitContainer1.Panel1.Width
        Mydgv1.Height = SplitContainer1.Height - 76
        TabControl1.Width = SplitContainer1.Panel2.Width - 15
        TabControl1.Height = SplitContainer1.Height - 40

    End Sub
    Private Sub SplitContainer1_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        Mydgv1.Width = SplitContainer1.Panel1.Width
        TabControl1.Width = SplitContainer1.Panel2.Width
    End Sub
    Private Sub TabControl1_Resize(sender As Object, e As EventArgs) Handles TabControl1.Resize
        DGV1.Width = TabControl1.Width - 8
        DGV1.Height = TabControl1.Height - 57
        DGV2.Width = DGV1.Width
        DGV2.Height = DGV1.Height

        FlowLayoutPanel1.Width = DGV1.Width
        FlowLayoutPanel4.Width = FlowLayoutPanel1.Width
        FlowLayoutPanel2.Width = TabControl1.Width - 8
        Panel1.Width = TabControl1.Width - 8
        Panel1.Height = TabControl1.Height - 60


    End Sub
    Sub data_refresh(sender As Object, e As EventArgs)
        If load_flag Then
            If Button2.Enabled Then
                Dim btn As DialogResult
                btn = MessageBox.Show("是否保存当前信息?", "保存提示", MessageBoxButtons.YesNo)
                If btn = DialogResult.Yes Then
                    Button2_Click(sender, e)
                ElseIf btn = DialogResult.No Then
                    Button10_Click(sender, e)
                End If
            End If
            load_flag = False
            If guid <> Mydgv1.get_cellvalue(Mydgv1.get_currentrow, "guid") Then
                guid = Mydgv1.get_cellvalue(Mydgv1.get_currentrow, "guid")
                pubcmd.CommandText = "select * from 检验任务 where guid='" + guid + "'"
                pubtable.Clear()
                pubada.Fill(pubtable)
                Button2.Enabled = False
                Button10.Enabled = False
                '去掉单项结论头尾空格
                Dim con As New SqlConnection
                Dim cmd As New SqlCommand
                con.ConnectionString = glb_sqlconstr
                cmd.Connection = con
                con.Open()
                cmd.Parameters.AddWithValue("guid", guid)
                cmd.CommandText = "update 检验项目 set 单项结论=ltrim(rtrim(单项结论)) where rguid=@guid"
                cmd.ExecuteNonQuery()
                pubtable2.Clear()
                pubcmd2.Parameters("guid").Value = guid
                pubada2.Fill(pubtable2)
                DGV1.Columns(0).Visible = False
                If Lab_jindu.Text <> "8" Then
                    '填充未完成项目
                    TabPage3.Parent = TabControl1
                    Dim reader As SqlDataReader
                    cmd.CommandText = "select guid,rguid,显示序号,项目名称,检验人,检验室,isnull(项目分包,0) as 项目分包 from 检验项目 WHERE rguid=@guid and 进度<20 order by 显示序号"
                    reader = cmd.ExecuteReader
                    DGV2.Rows.Clear()
                    Do While reader.Read
                        DGV2.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4), reader.Item(5), reader.Item(6))
                    Loop
                    reader.Close()
                Else
                    TabPage3.Parent = Nothing

                End If
                con.Close()
                If Lab_jindu.Text <> "8" Then
                    Button15.Enabled = False
                    Button24.Enabled = False
                Else
                    Button15.Enabled = True
                    Button24.Enabled = True
                End If
            End If
                load_flag = True
        End If
    End Sub
    Private Sub Mydgv1_currentcell_changed(sender As Object, e As EventArgs) Handles Mydgv1.currentcell_changed
        Call data_refresh(sender, e)
    End Sub

    Sub fh_insert(sender As Object, e As KeyEventArgs)
        Dim tb As Object
        Dim start As Integer = 0
        If TypeOf (sender) Is TextBox Or TypeOf (sender) Is ComboBox Then
            tb = sender
        Else
            Exit Sub
        End If
        If (e.KeyCode = Keys.B And e.Modifiers = Keys.Control) Then
            form_符号.ShowDialog()
            start = tb.SelectionStart
            tb.Text = tb.Text.Insert(start, My.Computer.Clipboard.GetText())
            tb.SelectionStart = start + 1
        End If
        If (e.KeyCode = Keys.Oemplus And e.Modifiers = Keys.Control) Then
            tb.SelectedText = "^(" + tb.SelectedText + ")"
        End If
        If (e.KeyCode = Keys.OemMinus And e.Modifiers = Keys.Control) Then
            tb.SelectedText = "_(" + tb.SelectedText + ")"
        End If

    End Sub

    Private Sub form_报告编制_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select * from zyn_颜色 where userid='" + glb_loginname + "' and form_name='" + Me.Name + "'"
        reader = cmd.ExecuteReader
        Do While reader.Read
            Select Case reader.Item("color_name")
                Case "不合格"
                    cd1.Color = Color.FromArgb(reader.Item("color"))
                Case "问题项"
                    cd2.Color = Color.FromArgb(reader.Item("color"))
            End Select
        Loop
        reader.Close()
        con.Close()

        ComboBox1.SelectedIndex = 0
        pubcon.ConnectionString = glb_sqlconstr
        pubcmd.Connection = pubcon
        pubcmd.CommandText = "select * from 检验任务 where guid='" + guid + "'"
        pubada.SelectCommand = pubcmd
        pubcmdbuilder.DataAdapter = pubada
        pubada.Fill(pubtable)
        ' pubbinds.DataSource = pubtable
        BS1.DataSource = pubtable
        Dim tb As TextBox
        Dim cb As ComboBox
        For Each control In Panel1.Controls
            If control.tag IsNot Nothing Then
                control.databindings.add("text", BS1, control.tag.ToString, True)
                If TypeOf (control) Is TextBox Then
                    tb = control
                    If tb.ReadOnly = True Then
                        tb.BackColor = Color.White
                    Else
                        AddHandler tb.TextChanged, AddressOf control_edited
                        AddHandler tb.KeyDown, AddressOf fh_insert
                    End If
                    If tb.Tag.ToString = "留言" And tb.ReadOnly Then AddHandler tb.TextChanged, AddressOf control_edited
                End If
                If TypeOf (control) Is ComboBox Then
                    cb = control
                    If cb.Enabled Then AddHandler cb.SelectedIndexChanged, AddressOf control_edited
                End If
            End If
        Next
        For Each control In FlowLayoutPanel2.Controls
            If control.tag IsNot Nothing Then
                If TypeOf (control) Is TextBox Then
                    tb = control
                    tb.DataBindings.Add("text", BS1, tb.Tag.ToString, True)
                    If control.ReadOnly = True Then control.BackColor = Color.White
                ElseIf TypeOf (control) Is CheckBox Then
                    control.databindings.add("checked", BS1, control.tag.ToString)
                ElseIf TypeOf (control) Is Label Then
                    control.databindings.add("text", BS1, control.tag.ToString)
                    control.visible = False
                End If
            End If
        Next

        Button2.Enabled = False
        Button10.Enabled = False
        pubcon2.ConnectionString = glb_sqlconstr
        pubcmd2.Connection = pubcon2
        pubcmd2.CommandText = "select guid,rguid,显示序号,项目名称,名称1,名称2,名称3,单位,标准值,实测值,单项结论,检验人,检验方法,备注 from 检验项目 WHERE rguid=@guid and 进度=20 order by 显示序号"
        pubcmd2.Parameters.AddWithValue("guid", guid)
        pubada2.SelectCommand = pubcmd2
        pubcmdbuilder2.DataAdapter = pubada2
        pubada2.Fill(pubtable2)
        pubbinds2.DataSource = pubtable2
        DGV1.AutoGenerateColumns = False
        DGV1.DataSource = pubbinds2

        'pubcon3.ConnectionString = glb_sqlconstr
        'pubcmd3.Connection = pubcon3
        'pubcmd3.CommandText = "select 标准值 from 检验项目 WHERE rguid=@guid and 进度=20 group by 标准值 union select null as 标准值 "
        'pubcmd3.Parameters.AddWithValue("guid", guid)
        'pubada3.SelectCommand = pubcmd3
        'pubcmdbuilder3.DataAdapter = pubada3
        'pubada3.Fill(pubtable3)
        'pubbinds3.DataSource = pubtable3
        For Each datacol As DataColumn In pubtable2.Columns
            Dim colname As String = datacol.ColumnName
            If colname = "单项结论111" Then
                Dim dgvcol As New DataGridViewTextboxColumn_Combox
                dgvcol.comboitem.Add("合格")
                dgvcol.comboitem.Add("不合格")
                dgvcol.comboitem.Add("/")
                dgvcol.comboitem.Add("符合")
                dgvcol.comboitem.Add("不符合")
                dgvcol.comboitem.Add("问题项")
                dgvcol.DefaultCellStyle.WrapMode = DataGridViewTriState.False
                dgvcol.DataPropertyName = colname
                dgvcol.Name = colname
                dgvcol.SortMode = DataGridViewColumnSortMode.Programmatic
                DGV1.Columns.Add(dgvcol)
                'ElseIf colname = "标准值" Then
                '    Dim dgvcol As New DataGridViewTextboxColumn_Combox
                '    dgvcol.DefaultCellStyle.WrapMode = DataGridViewTriState.False
                '    dgvcol.DataPropertyName = colname
                '    dgvcol.Name = colname
                '    dgvcol.SortMode = DataGridViewColumnSortMode.Programmatic
                '    DGV1.Columns.Add(dgvcol)
            ElseIf colname = "单项结论" Then
                Dim dgvcol As New DataGridViewComboBoxColumn
                dgvcol.Items.Add("合格")
                dgvcol.Items.Add("不合格")
                dgvcol.Items.Add("/")
                dgvcol.Items.Add("问题项")
                dgvcol.Items.Add("符合")
                dgvcol.Items.Add("不符合")
                dgvcol.Items.Add(DBNull.Value)
                dgvcol.FlatStyle = FlatStyle.Flat
                dgvcol.DataPropertyName = colname
                dgvcol.Name = colname
                dgvcol.SortMode = DataGridViewColumnSortMode.Programmatic
                DGV1.Columns.Add(dgvcol)
            Else
                Dim dgvcol As New DataGridViewTextBoxColumn
                dgvcol.DataPropertyName = colname
                dgvcol.Name = colname
                dgvcol.SortMode = DataGridViewColumnSortMode.Programmatic
                DGV1.Columns.Add(dgvcol)
            End If
        Next
        DGV1.Columns(0).Visible = False
        DGV1.Columns(1).Visible = False
        DGV1.Columns("显示序号").ReadOnly = True
        DGV1.Columns("项目名称").ReadOnly = True
        DGV1.Columns("检验人").ReadOnly = True

        Label32.BackColor = cd1.Color
        Label33.BackColor = cd2.Color

        load_flag = True
        Me.Text = "报告编制-" + glb_姓名
        Button1_Click(sender, e)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' pubbinds.EndEdit()
        BS1.EndEdit()
        pubada.Update(pubtable)
        Button2.Enabled = False
        Button10.Enabled = False
    End Sub



    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If DGV1.CurrentCell Is Nothing Then Exit Sub
        If DGV1.SelectedRows.Count = 0 Then DGV1.CurrentRow.Selected = True
        If MessageBox.Show("确认删除选中项目?", "删除项目", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            For Each row In DGV1.SelectedRows
                DGV1.Rows.Remove(row)
            Next
            pubbinds2.EndEdit()
            pubada2.Update(pubtable2)
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If guid = "" Then Exit Sub
        DGV1.Refresh()
        pubbinds2.EndEdit()
        pubada2.Update(pubtable2)
        Dim num As Integer
        num = DGV1.Rows.Count + 1
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        cmd.CommandText = "insert into 检验项目 (guid,rguid,进度,返工,删除,显示序号,检验人,检验室,检验完成日期,单项结论) values (newid(),@rguid,20,0,0,@显示序号,@检验人,@检验室,getdate(),'合格')"
        cmd.Parameters.AddWithValue("rguid", guid)
        cmd.Parameters.AddWithValue("显示序号", num)
        cmd.Parameters.AddWithValue("检验人", glb_姓名)
        cmd.Parameters.AddWithValue("检验室", glb_科室)
        cmd.ExecuteNonQuery()
        con.Close()
        pubtable2.Clear()
        pubcmd2.Parameters("guid").Value = guid
        pubada2.Fill(pubtable2)
        DGV1.CurrentCell = DGV1.Rows(num - 1).Cells("名称1")
        DGV1.BeginEdit(True)
    End Sub

    Private Sub dgv1_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles DGV1.CellValidated
        Dim colname As String
        Dim rowindex As Integer
        colname = DGV1.Columns(e.ColumnIndex).Name
        rowindex = e.RowIndex
        If colname = "名称1" Or colname = "名称2" Or colname = "名称3" Then
            Dim mc2, mc3 As String
            If IsDBNull(DGV1.CurrentCell.OwningRow.Cells("名称2").Value) Then mc2 = "" Else mc2 = DGV1.CurrentCell.OwningRow.Cells("名称2").Value
            If IsDBNull(DGV1.CurrentCell.OwningRow.Cells("名称3").Value) Then mc3 = "" Else mc3 = DGV1.CurrentCell.OwningRow.Cells("名称3").Value
            If DGV1.CurrentCell.OwningColumn.HeaderText = "名称1" Or DGV1.CurrentCell.OwningColumn.HeaderText = "名称2" Or DGV1.CurrentCell.OwningColumn.HeaderText = "名称3" Then
                DGV1.CurrentCell.OwningRow.Cells("项目名称").Value = DGV1.CurrentCell.OwningRow.Cells("名称1").Value
                If mc2 <> "" Then
                    DGV1.CurrentCell.OwningRow.Cells("项目名称").Value += ("-" + mc2)
                    If mc3 <> "" Then
                        DGV1.CurrentCell.OwningRow.Cells("项目名称").Value += ("-" + mc3)
                    End If
                End If
            End If

        End If
        pubbinds2.EndEdit()
        pubada2.Update(pubtable2)

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim num As Integer = 1
        For Each row In DGV1.Rows
            row.cells("显示序号").value = num
            num += 1
        Next
        pubbinds2.EndEdit()
        pubada2.Update(pubtable2)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        Dim num As Integer
        If DGV1.CurrentRow Is Nothing Then num = 1 Else num = DGV1.CurrentRow.Index + 1
        For i = num - 1 To DGV1.Rows.Count - 1
            DGV1.Rows(i).Cells("显示序号").Value += 1
        Next
        pubbinds2.EndEdit()
        pubada2.Update(pubtable2)

        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        cmd.CommandText = "insert into 检验项目 (guid,rguid,进度,返工,删除,显示序号,检验人,检验室,检验完成日期) values (newid(),@rguid,20,0,0,@显示序号,@检验人,@检验室,getdate())"
        cmd.Parameters.AddWithValue("rguid", guid)
        cmd.Parameters.AddWithValue("显示序号", num)
        cmd.Parameters.AddWithValue("检验人", glb_姓名)
        cmd.Parameters.AddWithValue("检验室", glb_科室)
        cmd.ExecuteNonQuery()
        con.Close()
        pubtable2.Clear()
        pubcmd2.Parameters("guid").Value = guid
        pubada2.Fill(pubtable2)
        DGV1.CurrentCell = DGV1.Rows(num - 1).Cells("名称1")
        DGV1.BeginEdit(True)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If DGV1.CurrentRow Is Nothing Then Exit Sub
        If DGV1.CurrentRow.Index = 0 Then
            MessageBox.Show("已经是第一条项目")
        Else
            Dim row As Integer = DGV1.CurrentRow.Index
            Button5_Click(sender, e)
            DGV1.CurrentRow.Cells("显示序号").Value -= 1
            DGV1.Rows(row - 1).Cells("显示序号").Value += 1
            pubbinds2.EndEdit()
            pubada2.Update(pubtable2)
            pubtable2.Clear()
            pubcmd2.Parameters("guid").Value = guid
            pubada2.Fill(pubtable2)
            DGV1.CurrentCell = DGV1.Rows(row - 1).Cells("显示序号")
            DGV1.Rows(row - 1).Selected = True
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If DGV1.CurrentRow Is Nothing Then Exit Sub
        If DGV1.CurrentRow.Index = DGV1.Rows.Count - 1 Then
            MessageBox.Show("已经是最后一条项目")
        Else
            Dim row As Integer = DGV1.CurrentRow.Index
            Button5_Click(sender, e)
            DGV1.CurrentRow.Cells("显示序号").Value += 1
            DGV1.Rows(row + 1).Cells("显示序号").Value -= 1
            pubbinds2.EndEdit()
            pubada2.Update(pubtable2)
            pubtable2.Clear()
            pubcmd2.Parameters("guid").Value = guid
            pubada2.Fill(pubtable2)
            DGV1.CurrentCell = DGV1.Rows(row + 1).Cells("显示序号")
            DGV1.Rows(row + 1).Selected = True
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If guid = "" Then Exit Sub
        DGV1.Refresh()
        pubbinds2.EndEdit()
        pubada2.Update(pubtable2)
        Dim num As Integer
        num = DGV1.Rows.Count + 1
        Dim bqxm, bqnr As New ArrayList
        bqxm.Add("应标注内容")
        bqxm.Add("食品名称")
        bqxm.Add("配料表")
        bqxm.Add("净含量")
        bqxm.Add("生产者的名称")
        bqxm.Add("地址")
        bqxm.Add("联系方式")
        bqxm.Add("生产日期")
        bqxm.Add("保质期")
        bqxm.Add("贮存条件")
        bqxm.Add("食品生产许可证编号")
        bqxm.Add("产品标准代号")
        bqxm.Add("产地")
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        Dim reader As SqlDataReader
        cmd.CommandText = "select '',样品名称,'','',生产单位,生产单位地址,生产单位电话,生产日期,保质期,'',许可证编号,'','' from 检验任务 where guid='" + guid + "'"
        reader = cmd.ExecuteReader
        If reader.Read Then
            For i = 0 To reader.FieldCount - 1
                bqnr.Add(reader.Item(i))
            Next
        End If
        reader.Close()
        cmd.CommandText = "insert into 检验项目 (guid,rguid,进度,返工,删除,显示序号,检验人,检验室,检验完成日期,单项结论,项目名称,名称1,名称2,标准值,实测值) values (newid(),@rguid,20,0,0,@显示序号,@检验人,@检验室,getdate(),@单项结论,@项目名称,@名称1,@名称2,@标准值,@实测值)"
        cmd.Parameters.AddWithValue("rguid", guid)
        cmd.Parameters.AddWithValue("显示序号", 0)
        cmd.Parameters.AddWithValue("检验人", glb_姓名)
        cmd.Parameters.AddWithValue("检验室", glb_科室)
        cmd.Parameters.AddWithValue("项目名称", "")
        cmd.Parameters.AddWithValue("名称1", "标签")
        cmd.Parameters.AddWithValue("名称2", "")
        cmd.Parameters.AddWithValue("标准值", "")
        cmd.Parameters.AddWithValue("实测值", "")
        cmd.Parameters.AddWithValue("单项结论", "")

        '添加单独一项
        cmd.Parameters("显示序号").SqlValue = num
        cmd.Parameters("名称1").SqlValue = "营养成分表标示形式"
        cmd.Parameters("项目名称").SqlValue = "营养成分表标示形式"
        cmd.Parameters("标准值").SqlValue = "按GB 28050对营养成分表标示形式的要求"
        cmd.Parameters("实测值").SqlValue = "符合要求"
        cmd.Parameters("单项结论").SqlValue = "合格"
        cmd.ExecuteNonQuery()
        num += 1
        cmd.Parameters("名称1").SqlValue = "标签"

        For i = 0 To bqxm.Count - 1
            cmd.Parameters("显示序号").SqlValue = num
            cmd.Parameters("名称2").SqlValue = bqxm(i)
            cmd.Parameters("项目名称").SqlValue = "标签-" + bqxm(i)
            cmd.Parameters("标准值").SqlValue = bqxm(i)
            cmd.Parameters("实测值").SqlValue = bqnr(i)
            If i = 0 Then cmd.Parameters("单项结论").SqlValue = "合格" Else cmd.Parameters("单项结论").SqlValue = ""
            cmd.ExecuteNonQuery()
            num += 1
        Next
        con.Close()
        pubtable2.Clear()
        pubcmd2.Parameters("guid").Value = guid
        pubada2.Fill(pubtable2)
        DGV1.CurrentCell = DGV1.Rows(num - 2).Cells("名称1")
        DGV1.BeginEdit(True)
    End Sub

    Private Sub dgv1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DGV1.EditingControlShowing
        Dim dgv As DataGridView = sender
        If TypeOf (e.Control) Is DataGridViewComboBoxEditingControl Then
            Dim combo As ComboBox = e.Control
            AddHandler combo.KeyDown, AddressOf combo_keydown
        End If

        If TypeOf (e.Control) Is DataGridViewTextBoxEditingControl Then
            Dim editcontrol As DataGridViewTextBoxEditingControl = e.Control
            If TypeOf (dgv.CurrentCell.OwningColumn) Is DataGridViewTextboxColumn_Combox Then
                Dim col As DataGridViewTextboxColumn_Combox = dgv.CurrentCell.OwningColumn
                editcontrol.Controls.Clear()
                AddHandler editcontrol.GotFocus, AddressOf edit_gotfocus
                Dim mycombox As New ComboBox
                mycombox.Top = -1
                mycombox.Left = -1
                mycombox.ItemHeight = editcontrol.Height
                mycombox.Width = editcontrol.Width + 2
                mycombox.Items.Clear()
                For Each item In col.comboitem
                    mycombox.Items.Add(item.ToString)
                Next
                mycombox.Text = editcontrol.Text
                AddHandler mycombox.TextChanged, AddressOf combo_textchanged
                RemoveHandler mycombox.KeyDown, AddressOf fh_insert
                AddHandler mycombox.KeyDown, AddressOf fh_insert
                editcontrol.Controls.Add(mycombox)
                mycombox.Select()
            Else
                editcontrol.Controls.Clear()
                RemoveHandler editcontrol.KeyDown, AddressOf fh_insert
                AddHandler editcontrol.KeyDown, AddressOf fh_insert
            End If
        End If

    End Sub
    Private Sub combo_keydown(sender As Object, e As KeyEventArgs)
        Dim combo As ComboBox = sender
        If e.KeyCode = Keys.Divide Or e.KeyCode = Keys.OemQuestion Then combo.SelectedItem = "/"
        If e.KeyCode = Keys.Delete Then combo.SelectedItem = DBNull.Value
    End Sub
    Private Sub combo_textchanged(sender As Object, e As EventArgs)
        Dim combo As ComboBox = sender
        combo.Parent.Text = combo.Text
    End Sub
    Private Sub edit_gotfocus(sender As Object, e As EventArgs)
        Dim edit As DataGridViewTextBoxEditingControl = sender
        RemoveHandler edit.GotFocus, AddressOf edit_gotfocus
        edit.Controls(0).Focus()
    End Sub
    Private Sub form_报告编制_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If Button2.Enabled And (e.KeyCode = Keys.S And e.Modifiers = Keys.Control) Then Button2_Click(sender, e)
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        BS1.CancelEdit()
        Button2.Enabled = False
        Button10.Enabled = False
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If TextBox6.Text <> "" Then TextBox6.Text += (Chr(13) + Chr(10))
        TextBox6.Text += ("【报告编制】" + Now.ToString + " " + glb_姓名 + ":" + TextBox7.Text)
        TextBox7.Text = ""
    End Sub

    Private Sub control_edited()
        If load_flag Then
            Button2.Enabled = True
            Button10.Enabled = True
        End If

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If DGV2.RowCount = 0 Then Exit Sub
        If DGV2.SelectedRows.Count = 0 Then DGV2.CurrentCell.OwningRow.Selected = True
        If MessageBox.Show("确定删除选中项目?", "确认删除", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            ' Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "delete 检验项目 where guid=@guid"
            cmd.Parameters.AddWithValue("guid", "")
            For Each row In DGV2.SelectedRows
                If Not (row.cells("dgv2_检验科室").value <> glb_科室 And row.cells("dgv2_项目分包").value = True) Or glb_loginname = "sa" Then
                    cmd.Parameters("guid").SqlValue = row.cells("dgv2_guid").value
                    cmd.ExecuteNonQuery()
                    DGV2.Rows.Remove(row)
                End If
            Next
            If DGV2.Rows.Count = 0 Then
                cmd.CommandText = "update 检验任务 set 进度=8 where guid='" + guid + "'"
                cmd.ExecuteNonQuery()
                MessageBox.Show("当前任务进度已改为报告编制,请刷新后再查看")
            End If
            con.Close()
        End If
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        MessageBox.Show("只能删除未分包的项目。如所有未完成项目都被删除，检验任务进度会自动被修改为报告编制。")
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Dim dbtn As DialogResult = DialogResult.Yes
        If Button2.Enabled Then
            dbtn = MessageBox.Show("信息未保存,选择是,保存后再预览,选择否,用未保存信息预览,选择取消退出预览", "提示", MessageBoxButtons.YesNoCancel)
            If dbtn = DialogResult.Yes Then
                Button2_Click(sender, e)
            End If
        End If
        If dbtn <> DialogResult.Cancel Then
            Me.UseWaitCursor = True
            Me.Cursor = Cursors.WaitCursor
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "select top 1 报告编号 from 检验任务 where guid='" + guid + "'"
            reader = cmd.ExecuteReader
            If reader.Read Then
                printreport(reader.Item(0).ToString, False, 1, True, False, False)
            End If
            reader.Close()
            con.Close()
            Me.UseWaitCursor = False
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        If guid = "" Then Exit Sub
        If Button2.Enabled = True Then
            If MessageBox.Show("信息未保存,保存后继续?", "提示", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Button2_Click(sender, e)
            Else
                Exit Sub
            End If
        End If

        Me.UseWaitCursor = True
        Me.Cursor = Cursors.WaitCursor
        If flow_next(guid, 8, 20) Then
            Button1_Click(sender, e)
            Mydgv1_currentcell_changed(sender, e)
        End If

        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Dim help As String = ""
        help += "快捷键说明:" + Chr(13) + Chr(10)
        help += "Ctrl+S:保存,Ctrl+B:插入符号" + Chr(13) + Chr(10)
        help += "Ctrl+ -:插入下标格式_(),Ctrl+ +:插入上标格式^()" + Chr(13) + Chr(10)
        help += "可先选中要加标的内容再按快捷键,也可插入上下标格式后再在括号中补齐内容" + Chr(13) + Chr(10)
        help += "单击颜色示例可修改颜色"
        MessageBox.Show(help)
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Me.UseWaitCursor = True
        Me.Cursor = Cursors.WaitCursor
        Dim app As Word.Application
        app = jyrw_print(guid, "报告原始记录.dotx")
        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
        If app IsNot Nothing Then
            app.Visible = True
            '    app.PrintOut(False,,,,,,, 1)
        Else
            MessageBox.Show("预览失败,请检查模板文件")
        End If
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        Me.UseWaitCursor = True
        Me.Cursor = Cursors.WaitCursor
        Dim app As Word.Application
        app = jyrw_print(guid, "报告原始记录.dotx")
        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
        If app IsNot Nothing Then
            app.PrintOut(False,,,,,,, 1)
            app.Quit(False)
        Else
            MessageBox.Show("打印失败,请检查模板文件")
        End If
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        Me.Close()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If TextBox3.Text <> "" Or ComboBox2.SelectedIndex < 0 Then Exit Sub
        Dim jylx As String = TextBox32.Text
        Dim jyyj As String = ""
        Dim jl As String = ""
        Dim pd As String = ""
        Dim bzarray As String() = Split(TextBox1.Text, Chr(13) + Chr(10))
        If ComboBox2.SelectedItem = "合格" Or ComboBox2.SelectedItem = "符合" Then
            pd = "符合"
        ElseIf ComboBox2.SelectedItem = "不合格" Or ComboBox2.SelectedItem = "不符合" Then
            pd = "不符合"
        Else
            pd = "/"
        End If
        If pd = "/" Then
            jl = "/"
        Else
            For Each bz As String In bzarray
                Dim num As Integer = InStr(bz, "《")
                If num > 0 Then
                    If jyyj = "" Then jyyj += bz.Substring(0, num - 1) + "标准" Else jyyj += "、" + bz.Substring(0, num - 1) + "标准"
                End If
            Next
            Dim sl As Integer = InStrRev(jyyj, "、")
            If sl > 0 Then jyyj = jyyj.Substring(0, sl - 1) + "和" + jyyj.Substring(sl)
            Select Case jylx
                Case "不定期", "区县监督"
                    jl = "该抽检产品经检验，所检项目" + pd + jyyj + "要求。"
                Case "委托检验"
                    jl = "该样品经检验，所检项目" + pd + jyyj + "要求。"
                Case "定期", "国家监督抽查（本级）", "评价性抽检", "国家监督抽查（市级）"
                    jl = "经抽样检验，所检项目" + pd + jyyj + "要求。"
                Case "委托检验（换证用）"
                    jl = "该样品按" + jyyj + "检验，所检项目" + pd + "要求。"
                Case "委托测试", "国家风险监测（本级）", "国家风险监测（市级）"
                    jl = "/"
                Case Else
                    jl = "/"
            End Select
        End If

        TextBox3.Text = jl
    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged

    End Sub

    Private Sub TextBox8_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox8.KeyDown
        If e.KeyCode = Keys.Enter And Trim(TextBox8.Text) <> "" Then Button1_Click(sender, e)
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        If DGV1.CurrentCell Is Nothing Then Exit Sub
        If DGV1.SelectedRows.Count = 0 Then DGV1.CurrentRow.Selected = True

        If Form_批量判定.ShowDialog() = DialogResult.OK Then
            For Each row In DGV1.SelectedRows
                row.cells("单项结论").value = Form_批量判定.dgvcol.SelectedItem.ToString
            Next
            pubbinds2.EndEdit()
            pubada2.Update(pubtable2)
        End If

    End Sub



    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        Dim cmd As New SqlCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "p_标准浏览"
        cmd.Parameters.AddWithValue("主检科室", glb_科室)
        cmd.Parameters.AddWithValue("样品名称", TextBox10.Text)
        dialog_标准筛选.mycmd = cmd
        If dialog_标准筛选.ShowDialog() = DialogResult.OK Then
            If TextBox1.Text = "" Then TextBox1.Text += dialog_标准筛选.Label1.Text Else TextBox1.Text += (Chr(13) + Chr(10) + dialog_标准筛选.Label1.Text)
        End If


    End Sub


    Private Sub form_报告编制_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Button2.Enabled Then
            If MessageBox.Show("当前信息尚未保存,确认退出?", "退出提示", MessageBoxButtons.YesNo) = DialogResult.No Then e.Cancel = True
        End If
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        If DGV2.RowCount = 0 Then Exit Sub
        If DGV2.SelectedRows.Count = 0 Then DGV2.CurrentCell.OwningRow.Selected = True
        If MessageBox.Show("确定设置选中项目状态为已完成?", "确认", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            ' Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "update 检验项目 set 进度=20 where guid=@guid"
            cmd.Parameters.AddWithValue("guid", "")
            For Each row In DGV2.SelectedRows
                If Not (row.cells("dgv2_检验科室").value <> glb_科室 And row.cells("dgv2_项目分包").value = True) Or glb_loginname = "sa" Then
                    cmd.Parameters("guid").SqlValue = row.cells("dgv2_guid").value
                    cmd.ExecuteNonQuery()
                    DGV2.Rows.Remove(row)
                End If
            Next
            pubtable2.Clear()
            pubcmd2.Parameters("guid").Value = guid
            pubada2.Fill(pubtable2)
            DGV1.Columns(0).Visible = False
            If DGV2.Rows.Count = 0 Then
                cmd.CommandText = "update 检验任务 set 进度=8 where guid='" + guid + "'"
                cmd.ExecuteNonQuery()
                MessageBox.Show("当前任务进度已改为报告编制,请刷新后再查看")
            End If
            con.Close()
        End If
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        Dim str As String = ""
        For Each row As DataGridViewRow In DGV1.Rows
            If row.Cells("单项结论").Value.ToString = "问题项" Then
                If str = "" Then str += row.Cells("项目名称").Value Else str += ("," + row.Cells("项目名称").Value)
            End If
        Next
        TextBox4.Text = str

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            CheckBox1.ForeColor = Color.Red

        Else
            CheckBox1.ForeColor = SystemColors.ControlText

        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            CheckBox2.ForeColor = Color.Red
        Else
            CheckBox2.ForeColor = SystemColors.ControlText

        End If
    End Sub

    Private Sub DGV1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGV1.DataBindingComplete
        For Each row As DataGridViewRow In DGV1.Rows
            Dim jl As String = row.Cells("单项结论").Value.ToString
            If jl = "不合格" Or jl = "不符合" Then
                row.DefaultCellStyle.BackColor = cd1.Color
            ElseIf jl = "问题项" Then
                row.DefaultCellStyle.BackColor = cd2.Color
            Else
                row.DefaultCellStyle.BackColor = Color.White
            End If
            '错误提示
            Dim rowcolor As Color = Color.Red
            If row.Cells("名称1").Value.ToString = "" Then
                row.DefaultCellStyle.ForeColor = rowcolor
            ElseIf row.Cells("名称2").Value.ToString = "" And row.Cells("名称3").Value.ToString <> "" Then
                row.DefaultCellStyle.ForeColor = rowcolor
            Else
                row.DefaultCellStyle.ForeColor = SystemColors.ControlText
            End If
        Next
    End Sub

    Private Sub Label32_Click(sender As Object, e As EventArgs) Handles Label32.Click
        cd1.ShowDialog()
        If Label32.BackColor <> cd1.Color Then
            Label32.BackColor = cd1.Color
            Call save_color(Me.Name, "不合格", cd1.Color.ToArgb)
            pubbinds2.ResetBindings(False)
        End If
    End Sub

    Private Sub Label33_Click(sender As Object, e As EventArgs) Handles Label33.Click
        cd2.ShowDialog()
        If Label33.BackColor <> cd2.Color Then
            Label33.BackColor = cd2.Color
            Call save_color(Me.Name, "问题项", cd2.Color.ToArgb)
            pubbinds2.ResetBindings(False)
        End If
    End Sub
    Private Sub TextBox7_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox7.KeyDown
        If e.KeyCode = Keys.Enter Then Button11_Click(sender, e)
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        If guid = "" Then Exit Sub
        If Button2.Enabled = True Then
            If MessageBox.Show("信息未保存,保存后继续?", "提示", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Button2_Click(sender, e)
            Else
                Exit Sub
            End If
        End If
        Me.UseWaitCursor = True
        Me.Cursor = Cursors.WaitCursor
        If flow_prev(guid, 8, 20) Then
            Button1_Click(sender, e)
            Mydgv1_currentcell_changed(sender, e)
        End If

        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        Dim i As Integer = 0
        Dim rguids As String = ""
        For Each row As DataGridViewRow In Mydgv1.selected_rows
            rguids += row.Cells("guid").Value + ","
            i += 1
        Next

        If rguids <> "" And i > 1 Then
            guid = ""
            Me.UseWaitCursor = True
            Me.Cursor = Cursors.WaitCursor
            Form_批量编辑.Label3.Text = rguids
            Form_批量编辑.ShowDialog()
            Call data_refresh(sender, e)
            Me.UseWaitCursor = False
            Me.Cursor = Cursors.Default
        Else
            MessageBox.Show("至少选中两个报告才能进行批量编辑!")
        End If
    End Sub

    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        If DGV1.SelectedRows.Count < 1 And DGV1.CurrentCell IsNot Nothing Then DGV1.CurrentRow.Selected = True
        If DGV1.SelectedRows.Count < 1 Then Exit Sub
        Dim mc1, mc2, mc3, mc As String
        Dim rowindex As Integer
        For Each row As DataGridViewRow In DGV1.SelectedRows
            If row.Index > 0 Then
                rowindex = row.Index - 1
                If DGV1.Rows(rowindex).Cells("名称1").Value.ToString = "" Then mc1 = "未定义" Else mc1 = DGV1.Rows(rowindex).Cells("名称1").Value.ToString
                If DGV1.Rows(rowindex).Cells("名称2").Value.ToString = "" Then mc2 = "未定义" Else mc2 = DGV1.Rows(rowindex).Cells("名称2").Value.ToString
                If DGV1.Rows(rowindex).Cells("名称3").Value.ToString = "" Then mc3 = "未定义" Else mc3 = DGV1.Rows(rowindex).Cells("名称3").Value.ToString
                If row.Cells("名称3").Value.ToString = "" Then
                    If row.Cells("名称2").Value.ToString <> "" Then
                        row.Cells("名称3").Value = row.Cells("名称2").Value
                        row.Cells("名称2").Value = mc2
                        row.Cells("名称1").Value = mc1
                    End If

                End If

            End If

        Next

        'pubbinds2.EndEdit()
        'pubada2.Update(pubtable2)
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        If guid = "" Then Exit Sub
        If Button2.Enabled = True Then
            If MessageBox.Show("信息未保存,保存后继续?", "提示", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Button2_Click(sender, e)
            Else
                Exit Sub
            End If
        End If
        Dim currow As Integer = Mydgv1.get_currentrow
        Dim bgbh As String = Mydgv1.get_cellvalue(currow, "报告编号").ToString
        Dim newbgbh, msg As String
        If bgbh.Substring(Len(bgbh) - 1, 1) = "G" Then
            newbgbh = bgbh.Substring(0, Len(bgbh) - 1)
            msg = "当前报告编号为" + bgbh + ",确认去掉G?"
        Else
            newbgbh = bgbh + "G"
            msg = "当前报告编号为" + bgbh + ",确认加G?"
        End If
        If MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "update 检验任务 set 报告编号=@bgbh where guid='" + guid + "'"
            cmd.Parameters.AddWithValue("bgbh", newbgbh)
            cmd.ExecuteNonQuery()
            con.Close()
            Call Button1_Click(sender, e)
            Mydgv1.select_cell(currow, "报告编号")
        End If

    End Sub
End Class