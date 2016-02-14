Imports System.Data.SqlClient

Public Class Form_批量编辑
    Dim rguids As String = ""
    Dim ckz As String = ""
    Dim scz As String = ""

    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged

        If ComboBox1.SelectedItem.ToString = "不修改" Then
            Button1.Enabled = False
            Button2.Enabled = False
        Else
            Button1.Enabled = True
            Button2.Enabled = True
        End If
    End Sub

    Private Sub Form_批量编辑_Load(sender As Object, e As EventArgs) Handles Me.Load
        rguids = Label3.Text
        ListBox1.Items.Clear()
        DGV1.Rows.Clear()
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select 项目名称 from 检验项目 where CHARINDEX(rGUID,@rguids,0)>=1 group by 项目名称"
        cmd.Parameters.AddWithValue("rguids", rguids)
        reader = cmd.ExecuteReader
        Do While reader.Read
            ListBox1.Items.Add(reader.Item(0))
        Loop
        reader.Close()
        con.Close()
        Dim cmd2 As New SqlCommand
        cmd2.CommandText = "select guid,报告编号,样品名称,检验依据,结论,判定,检验类型 from 检验任务 where CHARINDEX(GUID,@rguids,0)>=1"
        cmd2.Parameters.AddWithValue("rguids", rguids)
        Mydgv1.load_data(cmd2)

        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("合格")
        ComboBox1.Items.Add("不合格")
        ComboBox1.Items.Add("/")
        ComboBox1.Items.Add("问题项")
        ComboBox1.Items.Add("符合")
        ComboBox1.Items.Add("不符合")
        ComboBox1.Items.Add(DBNull.Value)
        ComboBox1.Items.Add("不修改")
        ComboBox1.SelectedItem = "不修改"

        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        Button5.Enabled = False
        Button6.Enabled = False
        Button7.Enabled = False
        Button8.Enabled = False
        Button9.Enabled = False
        Button10.Enabled = False
        Button12.Enabled = False
        Button13.Enabled = False
        Label1.Text = ""
        TabControl1.SelectedIndex = 1

        TextBox7.Text = ""
        TextBox8.Text = ""
        ComboBox2.SelectedIndex = -1

    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox1.Text = ""
        CheckBox1.Checked = False
        Button3.Enabled = False
        Button4.Enabled = False
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        TextBox2.Text = ""
        CheckBox2.Checked = False
        Button5.Enabled = False
        Button6.Enabled = False
    End Sub
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        TextBox6.Text = ""
        CheckBox3.Checked = False
        Button9.Enabled = False
        Button10.Enabled = False
    End Sub
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TextBox1.Enabled = False
            Button3.Enabled = True
            Button4.Enabled = True
        Else
            TextBox1.Enabled = True
            If TextBox1.Text = "" Then
                Button3.Enabled = False
                Button4.Enabled = False
            End If
        End If
    End Sub
    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            TextBox2.Enabled = False
            Button5.Enabled = True
            Button6.Enabled = True
        Else
            TextBox2.Enabled = True
            If TextBox2.Text = "" Then
                Button5.Enabled = False
                Button6.Enabled = False
            End If
        End If
    End Sub
    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked Then
            TextBox6.Enabled = False
            Button9.Enabled = True
            Button10.Enabled = True
        Else
            TextBox6.Enabled = True
            If TextBox6.Text = "" Then
                Button9.Enabled = False
                Button10.Enabled = False
            End If
        End If
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Button3.Enabled = True
        Button4.Enabled = True
        If TextBox1.Text <> "" Then CheckBox1.Checked = False
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Button5.Enabled = True
        Button6.Enabled = True
        If TextBox2.Text <> "" Then CheckBox2.Checked = False
    End Sub
    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        Button9.Enabled = True
        Button10.Enabled = True
        If TextBox6.Text <> "" Then CheckBox3.Checked = False
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ComboBox1.SelectedItem = "不修改"
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim xmmc As String = RTrim(ListBox1.SelectedItem.ToString)
        Dim tmp As String = ""
        If xmmc <> "" Then
            DGV1.Rows.Clear()
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "select a.guid,b.报告编号,b.样品名称,a.单位,a.标准值,a.实测值,a.单项结论 from 检验项目 a inner join 检验任务 b on a.rguid=b.guid where CHARINDEX(b.GUID,@rguids,0)>=1 and a.项目名称=@xmmc"
            cmd.Parameters.AddWithValue("rguids", rguids)
            cmd.Parameters.AddWithValue("xmmc", xmmc)
            reader = cmd.ExecuteReader
            Do While reader.Read
                DGV1.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4), reader.Item(5), reader.Item(6))
            Loop
            reader.Close()
            con.Close()
            DGV1.CurrentCell = Nothing

            If InStr(xmmc, "-") > 0 Then
                TextBox3.Text = xmmc.Substring(0, InStr(xmmc, "-") - 1)
                tmp = xmmc.Substring(InStr(xmmc, "-"))
            Else
                TextBox3.Text = xmmc
                TextBox4.Text = ""
                TextBox5.Text = ""
            End If
            If InStr(tmp, "-") > 0 Then
                TextBox4.Text = tmp.Substring(0, InStr(tmp, "-") - 1)
                TextBox5.Text = tmp.Substring(InStr(tmp, "-"))
            Else
                TextBox4.Text = tmp
                TextBox5.Text = ""
            End If
        Else
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox5.Text = ""
        End If
        Label1.Text = xmmc
        Button7.Enabled = False
        Button8.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If DGV1.SelectedRows.Count < 1 Then
            If DGV1.CurrentRow Is Nothing Then Exit Sub Else DGV1.CurrentRow.Selected = True
        End If
        Dim rowindex As New ArrayList
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "update 检验项目 set 标准值=@sqlvalue where guid=@xguid"
        If CheckBox1.Checked Then cmd.Parameters.AddWithValue("sqlvalue", "") Else cmd.Parameters.AddWithValue("sqlvalue", TextBox1.Text)
        cmd.Parameters.AddWithValue("xguid", "")

        For Each row As DataGridViewRow In DGV1.SelectedRows
            cmd.Parameters("xguid").SqlValue = row.Cells("xguid").Value
            cmd.ExecuteNonQuery()
            rowindex.Add(row.Index)
        Next
        con.Close()
        Call Button4_Click(sender, e)
        Call ListBox1_SelectedIndexChanged(sender, e)

        For Each i As Integer In rowindex
            DGV1.Rows(i).Selected = True
        Next
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If DGV1.SelectedRows.Count < 1 Then
            If DGV1.CurrentRow Is Nothing Then Exit Sub Else DGV1.CurrentRow.Selected = True
        End If
        Dim rowindex As New ArrayList
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "update 检验项目 set 实测值=@sqlvalue where guid=@xguid"
        If CheckBox1.Checked Then cmd.Parameters.AddWithValue("sqlvalue", "") Else cmd.Parameters.AddWithValue("sqlvalue", TextBox2.Text)
        cmd.Parameters.AddWithValue("xguid", "")

        For Each row As DataGridViewRow In DGV1.SelectedRows
            cmd.Parameters("xguid").SqlValue = row.Cells("xguid").Value
            cmd.ExecuteNonQuery()
            rowindex.Add(row.Index)
        Next
        con.Close()
        Call Button6_Click(sender, e)
        Call ListBox1_SelectedIndexChanged(sender, e)

        For Each i As Integer In rowindex
            DGV1.Rows(i).Selected = True
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If DGV1.SelectedRows.Count < 1 Then
            If DGV1.CurrentRow Is Nothing Then Exit Sub Else DGV1.CurrentRow.Selected = True
        End If
        Dim rowindex As New ArrayList
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "update 检验项目 set 单项结论=@sqlvalue where guid=@xguid"
        cmd.Parameters.AddWithValue("sqlvalue", ComboBox1.SelectedItem.ToString)
        cmd.Parameters.AddWithValue("xguid", "")

        For Each row As DataGridViewRow In DGV1.SelectedRows
            cmd.Parameters("xguid").SqlValue = row.Cells("xguid").Value
            cmd.ExecuteNonQuery()
            rowindex.Add(row.Index)
        Next
        con.Close()
        Call Button2_Click(sender, e)
        Call ListBox1_SelectedIndexChanged(sender, e)

        For Each i As Integer In rowindex
            DGV1.Rows(i).Selected = True
        Next
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If DGV1.SelectedRows.Count < 1 Then
            If DGV1.CurrentRow Is Nothing Then Exit Sub Else DGV1.CurrentRow.Selected = True
        End If
        Dim rowindex As New ArrayList
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "update 检验项目 set 单位=@sqlvalue where guid=@xguid"
        If CheckBox3.Checked Then cmd.Parameters.AddWithValue("sqlvalue", "") Else cmd.Parameters.AddWithValue("sqlvalue", TextBox6.Text.ToString)
        cmd.Parameters.AddWithValue("xguid", "")

        For Each row As DataGridViewRow In DGV1.SelectedRows
            cmd.Parameters("xguid").SqlValue = row.Cells("xguid").Value
            cmd.ExecuteNonQuery()
            rowindex.Add(row.Index)
        Next
        con.Close()
        Call Button10_Click(sender, e)
        Call ListBox1_SelectedIndexChanged(sender, e)

        For Each i As Integer In rowindex
            DGV1.Rows(i).Selected = True
        Next
    End Sub
    Sub xmmc_edit()
        Button7.Enabled = True
        Button8.Enabled = True
        Dim tmp As String = ""
        If TextBox3.Text.ToString <> "" Then
            tmp += TextBox3.Text.ToString
            TextBox4.Enabled = True
            If TextBox4.Text.ToString <> "" Then
                tmp += "-" + TextBox4.Text.ToString
                TextBox5.Enabled = True
                If TextBox5.Text.ToString <> "" Then tmp += "-" + TextBox5.Text.ToString
            Else
                TextBox5.Enabled = False
            End If
        Else
            TextBox4.Enabled = False
            TextBox5.Enabled = False
        End If
        Label1.Text = tmp
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        Call xmmc_edit()
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        Call xmmc_edit()
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        Call xmmc_edit()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim xmmc As String = RTrim(ListBox1.SelectedItem.ToString)
        Dim tmp As String = ""
        If xmmc <> "" Then
            If InStr(xmmc, "-") > 0 Then
                TextBox3.Text = xmmc.Substring(0, InStr(xmmc, "-") - 1)
                tmp = xmmc.Substring(InStr(xmmc, "-"))
            Else
                TextBox3.Text = xmmc
                TextBox4.Text = ""
                TextBox5.Text = ""
            End If
            If InStr(tmp, "-") > 0 Then
                TextBox4.Text = tmp.Substring(0, InStr(tmp, "-") - 1)
                TextBox5.Text = tmp.Substring(InStr(tmp, "-"))
            Else
                TextBox4.Text = tmp
                TextBox5.Text = ""
            End If
        Else
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox5.Text = ""
        End If
        Label1.Text = xmmc
        Button7.Enabled = False
        Button8.Enabled = False
        Label1.Focus()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim xmmc, mc1, mc2, mc3 As String
        xmmc = Label1.Text
        If TextBox3.Enabled Then mc1 = TextBox3.Text Else mc1 = ""
        If TextBox4.Enabled Then mc2 = TextBox4.Text Else mc2 = ""
        If TextBox5.Enabled Then mc3 = TextBox5.Text Else mc3 = ""

        If DGV1.SelectedRows.Count < 1 Then
            If DGV1.CurrentRow Is Nothing Then Exit Sub Else DGV1.CurrentRow.Selected = True
        End If
        Dim rowindex As New ArrayList
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "update 检验项目 set 项目名称=@xmmc,名称1=@mc1,名称2=@mc2,名称3=@mc3 where guid=@xguid"
        cmd.Parameters.AddWithValue("xmmc", xmmc)
        cmd.Parameters.AddWithValue("mc1", mc1)
        cmd.Parameters.AddWithValue("mc2", mc2)
        cmd.Parameters.AddWithValue("mc3", mc3)
        cmd.Parameters.AddWithValue("xguid", "")

        For Each row As DataGridViewRow In DGV1.SelectedRows
            cmd.Parameters("xguid").SqlValue = row.Cells("xguid").Value
            cmd.ExecuteNonQuery()
        Next
        con.Close()
        Call Button8_Click(sender, e)
        Call Me.Form_批量编辑_Load(sender, e)
        ListBox1.SelectedItem = xmmc

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim cmd As New SqlCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "p_标准浏览"
        cmd.Parameters.AddWithValue("主检科室", glb_科室)
        cmd.Parameters.AddWithValue("样品名称", "")
        dialog_标准筛选.mycmd = cmd
        If dialog_标准筛选.ShowDialog() = DialogResult.OK Then
            If TextBox7.Text = "" Then TextBox7.Text += dialog_标准筛选.Label1.Text Else TextBox7.Text += (Chr(13) + Chr(10) + dialog_标准筛选.Label1.Text)
        End If

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex >= 0 Then
            Button12.Enabled = True
            Button13.Enabled = True
        Else
            Button12.Enabled = False
            Button13.Enabled = False
        End If
        If TextBox8.Text <> "" Or ComboBox2.SelectedIndex < 0 Then Exit Sub
        Dim jylx As String = ""
        If Mydgv1.dgv_currentcell IsNot Nothing Then jylx = Mydgv1.dgv_currentcell.OwningRow.Cells("检验类型").Value.ToString()
        Dim jyyj As String = ""
        Dim jl As String = ""
        Dim pd As String = ""
        Dim bzarray As String() = Split(TextBox7.Text, Chr(13) + Chr(10))
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
        TextBox8.Text = jl
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        TextBox7.Text = ""
        TextBox8.Text = ""
        ComboBox2.SelectedIndex = -1
        Button12.Enabled = False
        Button13.Enabled = False
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged
        Button12.Enabled = True
        Button13.Enabled = True
    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        Button12.Enabled = True
        Button13.Enabled = True
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If Mydgv1.get_selectrows.Count < 1 Then
            If Mydgv1.dgv_currentcell IsNot Nothing Then Mydgv1.dgv_selectrow(Mydgv1.get_currentrow, True)
        End If
        Dim rowindex As New ArrayList
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "update 检验任务 set 检验依据=@检验依据,判定=@判定,结论=@结论  where guid=@guid"
        If ComboBox2.SelectedIndex = -1 Then cmd.Parameters.AddWithValue("判定", "/") Else cmd.Parameters.AddWithValue("判定", ComboBox2.SelectedItem.ToString)
        cmd.Parameters.AddWithValue("guid", "")
        cmd.Parameters.AddWithValue("检验依据", TextBox7.Text.ToString)
        cmd.Parameters.AddWithValue("结论", TextBox8.Text.ToString)
        For Each row As DataGridViewRow In Mydgv1.dgv_selectrows
            cmd.Parameters("guid").SqlValue = row.Cells("guid").Value
            cmd.ExecuteNonQuery()
            rowindex.Add(row.Index)
        Next
        con.Close()
        Dim cmd2 As New SqlCommand
        cmd2.CommandText = "select guid,报告编号,样品名称,检验依据,结论,判定,检验类型 from 检验任务 where CHARINDEX(GUID,@rguids,0)>=1"
        cmd2.Parameters.AddWithValue("rguids", rguids)
        Mydgv1.load_data(cmd2)
        For Each i In rowindex
            Mydgv1.dgv_selectrow(i, True)
        Next
        Button12.Enabled = False
    End Sub


End Class