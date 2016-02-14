Imports System.IO, System.Data.SqlClient, Microsoft.Office.Interop

Public Class form_缴费单管理
    Public jguid As String = ""
    Private Sub form_缴费单管理_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        Label1.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        dgv1.Rows.Clear()
        DGV2.Rows.Clear()
        If ComboBox1.SelectedIndex = 1 Then
            Label1.Text = "已缴费单据只能查看,不能修改"
            Button2.Enabled = False
            Button3.Enabled = False
        Else
            Label1.Text = ""
            Button2.Enabled = True
            Button3.Enabled = True
        End If
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select guid,缴费单编号 from 缴费单 where 状态=" + ComboBox1.SelectedIndex.ToString + " order by 缴费单编号"
        reader = cmd.ExecuteReader
        Do While reader.Read
            dgv1.Rows.Add(reader.Item(0), reader.Item(1))
        Loop
        reader.Close()
        con.Close()
        'If dgv1.Rows.Count > 0 Then dgv1.Rows(0).Selected = False
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 0 Then DGV2.Columns("检验费").HeaderText = "应收费" Else DGV2.Columns("检验费").HeaderText = "实际收费"
        Button1_Click(sender, e)
    End Sub

    Private Sub dgv1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv1.CellClick

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MessageBox.Show("确定从缴费单中删除指定报告?", "确认", MessageBoxButtons.YesNo) = DialogResult.No Then Exit Sub

        If DGV2.SelectedRows.Count > 0 Then
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            For Each row In DGV2.SelectedRows
                If row.cells("收费情况").value = "未收费" Then
                    cmd.CommandText = "delete from 缴费记录 where rguid in (select guid from sys_view_检验任务 where 收费情况='未收费' and guid='" + row.cells("guid").value + "')"
                    cmd.ExecuteNonQuery()
                End If
            Next
            cmd.CommandText = "update 缴费单 set 应收金额=isnull((select sum(应收金额) from 缴费记录 where pguid='" + jguid + "'),0) where guid='" + jguid + "'"
            cmd.ExecuteNonQuery()
            con.Close()
            Dim ee As New DataGridViewCellEventArgs(dgv1.CurrentCell.ColumnIndex, dgv1.CurrentCell.RowIndex)
            dgv1_CellClick(sender, ee)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If jguid = "" Then Exit Sub
        Dim filename As String
        filename = Application.StartupPath + "\temp\" + "缴费通知单.dotx"
        If Not System.IO.File.Exists(filename) Then Exit Sub
        Dim app As New Word.Application
        Dim doc As Word.Document
        app.ScreenUpdating = False
        app.Visible = False
        doc = app.Documents.Add(filename)
        doc.Activate()
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim reader As SqlClient.SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select aa.报告编号,aa.样品名称,bb.* from sys_view_检验任务 aa inner join (select a.缴费单编号,a.应收金额 as 总金额,a.付款单位 as 缴费单位,b.科室 as 主检科室 ,b.应收金额 as 检验费,b.rguid  from 缴费单 a inner join 缴费记录 b on a.guid=b.pguid where a.guid='" + jguid + "') bb on aa.GUID =bb.rguid"
        reader = cmd.ExecuteReader
        Dim num As Integer = 0
        Dim totalmoney As Long = 0
        Do While reader.Read
            If num = 0 Then
                app.Selection.Find.Forward = True
                app.Selection.Find.ClearFormatting()
                app.Selection.Find.MatchWholeWord = False
                app.Selection.Find.MatchWildcards = True
                app.Selection.Find.MatchCase = False
                app.Selection.Find.Text = "#/*/#"
                Dim itemname As String = ""
                Dim typetext As String = ""
                Do
                    app.Selection.Find.ClearFormatting()
                    app.Selection.Find.Execute()
                    If InStr(app.Selection.Text, "#/") > 0 Then
                        itemname = app.Selection.Text
                        itemname = Replace(itemname, "#/", "")
                        itemname = Replace(itemname, "/#", "")
                        Try
                            If IsDBNull(reader.Item(itemname)) Then
                                typetext = "/"
                            Else
                                If Trim(reader.Item(itemname)) = "" Then typetext = "/" Else typetext = reader.Item(itemname)
                            End If
                            If itemname = "抽样日期" Or itemname = "到样日期" Or itemname = "签发日期" Then
                                typetext = String.Format("{0:yyyy年M月d日}", reader.Item(itemname))
                            End If
                            app.Selection.Text = typetext
                        Catch ex As Exception

                        End Try
                        app.Selection.MoveRight()
                    Else
                        Exit Do
                    End If
                Loop
            End If
            If num = 0 Then
                app.Selection.GoTo(-1,,, "明细")
                totalmoney = reader.Item("总金额")
            Else
                app.Selection.InsertRowsBelow(1)
            End If
            app.Selection.TypeText((num + 1).ToString)
            app.Selection.MoveRight(12, 1)
            app.Selection.TypeText(reader.Item("报告编号"))
            app.Selection.MoveRight(12, 1)
            app.Selection.TypeText(reader.Item("样品名称"))
            app.Selection.MoveRight(12, 1)
            app.Selection.TypeText(reader.Item("主检科室"))
            app.Selection.MoveRight(12, 1)
            app.Selection.TypeText(reader.Item("检验费"))
            app.Selection.MoveRight(12, 1)
            app.Selection.TypeText(reader.Item("缴费单位"))
            num += 1
        Loop
        app.Selection.MoveRight(12, 2)
        app.Selection.TypeText(totalmoney.ToString + "元")
        reader.Close()
        con.Close()
        app.PrintPreview = False
        app.PrintOut(False,,,,,,, 1)
        app.Quit(False)
    End Sub

    Private Sub dgv1_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgv1.CurrentCellChanged
        If dgv1.CurrentRow Is Nothing Then Exit Sub
        DGV2.Rows.Clear()

        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        If dgv1.CurrentRow.Cells("pguid").Value <> "" Then jguid = dgv1.CurrentRow.Cells("pguid").Value
        If jguid <> "" Then
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            If ComboBox1.SelectedIndex = 0 Then
                cmd.CommandText = "delete 缴费记录 where rguid not in (SELECT rguid  FROM [KingsLims].[dbo].[缴费记录] a inner join sys_view_检验任务 b on a.RGuid =b.guid where a.PGuid ='" + jguid + "') and pguid='" + jguid + "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "update  缴费记录  set 应收金额=bb.检验费 from  缴费记录 as aa join (SELECT b.guid,b.检验费 from  缴费记录 a inner join sys_view_检验任务 b on a.rguid=b.guid where isnull(b.收费情况,'未收费')='未收费' and a.PGuid ='" + jguid + "' ) as bb on aa.rguid=bb.guid  "
                cmd.ExecuteNonQuery()
                cmd.CommandText = "update 缴费单 set 应收金额=(select sum(应收金额) from 缴费记录 where pguid='" + jguid + "') where guid='" + jguid + "'"
                cmd.ExecuteNonQuery()
            End If
            cmd.CommandText = "select a.guid,a.报告编号,a.样品名称,b.应收金额,a.收费情况 from sys_view_检验任务 a inner join 缴费记录 b on a.guid=b.rguid where b.pguid='" + jguid + "'"
            reader = cmd.ExecuteReader
            Do While reader.Read
                DGV2.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4))
            Loop
            reader.Close()
            cmd.CommandText = "select 应收金额 from 缴费单 where guid='" + jguid + "'"
            If DGV2.Rows.Count > 0 Then
                reader = cmd.ExecuteReader
                If reader.Read Then DGV2.Rows.Add("", "", "合计", reader.Item(0), "") Else DGV2.Rows.Add("", "", "合计", "0", "")
                DGV2.Rows(0).Selected = False
            End If

            con.Close()
        End If

    End Sub
End Class