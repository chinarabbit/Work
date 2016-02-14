Imports System.ComponentModel
Imports System.Data.SqlClient, Microsoft.Office.Interop

Public Class Form_报告审核
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
    Public pubcon3 As New SqlConnection
    Public click_index As New ArrayList


    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)
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
        cmd.CommandText = "select guid,报告编号,样品名称,主检科室,检验类型,计划编号,协议编号,抽样单编号,受检单位,生产单位,抽样单位,下达日期,商定完成日期,要求完成日期,检验结束日期,主检,受理人,当前进度,委托单位地址 from 检验任务 where 进度=@进度 and (主检科室=@主检科室 or @主检科室='') and (主检<>@当前人 or @当前人='') and (报告编号 like @报告编号 or @报告编号='') order by 报告编号"
        cmd.Parameters.AddWithValue("进度", 12)
        If glb_loginname = "sa" Then cmd.Parameters.AddWithValue("主检科室", "") Else cmd.Parameters.AddWithValue("主检科室", glb_科室)
        If glb_loginname = "sa" Then cmd.Parameters.AddWithValue("当前人", "") Else cmd.Parameters.AddWithValue("当前人", glb_姓名)
        If Trim(TextBox8.Text) <> "" Then cmd.Parameters.AddWithValue("报告编号", "%" + Trim(TextBox8.Text) + "%") Else cmd.Parameters.AddWithValue("报告编号", "")
        TextBox8.Text = ""
        Mydgv1.load_data(cmd)

        Mydgv1.select_cell(0, "报告编号")
        load_flag = True
        Call data_refresh(sender, e)

    End Sub



    Private Sub form_报告审核_Resize(sender As Object, e As EventArgs) Handles Me.Resize
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

        FlowLayoutPanel1.Width = DGV1.Width
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
                pubtable2.Clear()
                click_index.Clear()
                pubcmd2.Parameters("guid").Value = guid
                pubada2.Fill(pubtable2)
                DGV1.Columns(0).Visible = False
                If lab_jindu.Text <> "12" Then
                    Button15.Enabled = False
                    Button4.Enabled = False
                Else
                    Button15.Enabled = True
                    Button4.Enabled = True
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

    Private Sub form_报告审核_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
                Case "标示项"
                    cd3.Color = Color.FromArgb(reader.Item("color"))
            End Select
        Loop
        reader.Close()
        con.Close()

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

        For Each datacol As DataColumn In pubtable2.Columns
            Dim colname As String = datacol.ColumnName
            Dim dgvcol As New DataGridViewTextBoxColumn
            dgvcol.DataPropertyName = colname
            dgvcol.Name = colname
            dgvcol.SortMode = DataGridViewColumnSortMode.Programmatic
            dgvcol.ReadOnly = True
            DGV1.Columns.Add(dgvcol)
        Next
        DGV1.Columns(0).Visible = False
        DGV1.Columns(1).Visible = False

        Label32.BackColor = cd1.Color
        Label33.BackColor = cd2.Color
        Label36.BackColor = cd3.Color
        load_flag = True
        Me.Text = "报告审核-" + glb_姓名
        Button1_Click(sender, e)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' pubbinds.EndEdit()
        BS1.EndEdit()
        pubada.Update(pubtable)
        Button2.Enabled = False
        Button10.Enabled = False
    End Sub


    Private Sub form_报告审核_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If Button2.Enabled And (e.KeyCode = Keys.S And e.Modifiers = Keys.Control) Then Button2_Click(sender, e)
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        BS1.CancelEdit()
        Button2.Enabled = False
        Button10.Enabled = False
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If TextBox6.Text <> "" Then TextBox6.Text += (Chr(13) + Chr(10))
        TextBox6.Text += ("【报告审核】" + Now.ToString + " " + glb_姓名 + ":" + TextBox7.Text)
        TextBox7.Text = ""
    End Sub

    Private Sub control_edited()
        If load_flag Then
            Button2.Enabled = True
            Button10.Enabled = True
        End If

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
        If flow_next(guid, 12, 20) Then
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
        help += "单击颜色示例可修改颜色"
        MessageBox.Show(help)
    End Sub



    Private Sub TextBox8_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox8.KeyDown
        If e.KeyCode = Keys.Enter And Trim(TextBox8.Text) <> "" Then Button1_Click(sender, e)
    End Sub

    Private Sub form_报告审核_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Button2.Enabled Then
            If MessageBox.Show("当前信息尚未保存,确认退出?", "退出提示", MessageBoxButtons.YesNo) = DialogResult.No Then e.Cancel = True
        End If
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
            If click_index.Contains(row.Index) Then
                row.DefaultCellStyle.BackColor = cd3.Color
            Else
                Dim jl As String = row.Cells("单项结论").Value.ToString
                If jl = "不合格" Or jl = "不符合" Then
                    row.DefaultCellStyle.BackColor = cd1.Color
                ElseIf jl = "问题项" Then
                    row.DefaultCellStyle.BackColor = cd2.Color
                Else
                    row.DefaultCellStyle.BackColor = Color.White
                End If
            End If

        Next
    End Sub

    Private Sub Label32_Click(sender As Object, e As EventArgs) Handles Label32.Click
        cd1.ShowDialog()
        If Label32.BackColor <> cd1.Color Then
            Label32.BackColor = cd1.Color
            Call save_color(Me.Name, "不合格", cd1.Color.ToArgb)
            pubtable2.Clear()
            pubada2.Fill(pubtable2)
        End If
    End Sub

    Private Sub Label33_Click(sender As Object, e As EventArgs) Handles Label33.Click
        cd2.ShowDialog()
        If Label33.BackColor <> cd2.Color Then
            Label33.BackColor = cd2.Color
            Call save_color(Me.Name, "问题项", cd2.Color.ToArgb)
            pubtable2.Clear()
            pubada2.Fill(pubtable2)
        End If
    End Sub

    Private Sub Label36_Click(sender As Object, e As EventArgs) Handles Label36.Click
        cd3.ShowDialog()
        If Label36.BackColor <> cd3.Color Then
            Label36.BackColor = cd3.Color
            Call save_color(Me.Name, "标示项", cd3.Color.ToArgb)
            For Each row_index In click_index
                DGV1.Rows(row_index).DefaultCellStyle.BackColor = cd3.Color
            Next
        End If
    End Sub
    Private Sub TextBox7_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox7.KeyDown
        If e.KeyCode = Keys.Enter Then Button11_Click(sender, e)
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Dim help As String = ""
        help += "单击颜色示例可修改颜色"
        MessageBox.Show(help)
    End Sub

    Private Sub DGV1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        If click_index.Contains(e.RowIndex) Then
            click_index.Remove(e.RowIndex)
            Dim jl As String = DGV1.Rows(e.RowIndex).Cells("单项结论").Value.ToString
            If jl = "不合格" Or jl = "不符合" Then
                DGV1.Rows(e.RowIndex).DefaultCellStyle.BackColor = cd1.Color
            ElseIf jl = "问题项" Then
                DGV1.Rows(e.RowIndex).DefaultCellStyle.BackColor = cd2.Color
            Else
                DGV1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
            End If
        Else
            click_index.Add(e.RowIndex)
            DGV1.Rows(e.RowIndex).DefaultCellStyle.BackColor = cd3.Color
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
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
        If flow_prev(guid, 12, 20) Then
            Button1_Click(sender, e)
            Mydgv1_currentcell_changed(sender, e)
        End If

        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Close()
    End Sub
End Class