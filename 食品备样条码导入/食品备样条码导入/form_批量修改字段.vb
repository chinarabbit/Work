Imports System.Data.SqlClient
Public Class form_批量修改字段
    Private Sub form_批量修改字段_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        cmd.Connection = con
        con.ConnectionString = glb_sqlconstr
        con.Open()
        DGV1.Rows.Clear()
        'DGV1.Columns.Clear()
        'DGV1.Columns.Add("字段名称", "字段名称")
        'DGV1.Columns.Add("长度", "长度")
        'DGV1.Columns.Add("类型", "类型")
        'DGV1.Columns.Add("修改", "修改")

        cmd.CommandText = "select a.name 名称,a.length 长度 , b.name 类型 from syscolumns a inner join systypes b on a.xtype=b.xtype where a.id=object_id('检验任务') order by a.colid "
        reader = cmd.ExecuteReader
        Dim i As Integer = 0
        Do While reader.Read
            i = DGV1.Rows.Add()
            DGV1.Rows(i).Cells(0).Value = reader.Item(0).ToString
            DGV1.Rows(i).Cells(1).Value = reader.Item(1).ToString
            DGV1.Rows(i).Cells(2).Value = reader.Item(2).ToString
            DGV1.Rows(i).Cells(3).Value = False
        Loop
        reader.Close()

        cmd.CommandText = "select 名称 from lookup where type ='zyn_批量修改字段' "
        reader = cmd.ExecuteReader
        Do While reader.Read
            For j = 0 To DGV1.Rows.Count - 1
                If DGV1.Rows(j).Cells(0).Value = reader.Item(0).ToString Then DGV1.Rows(j).Cells(3).Value = True
            Next
        Loop
        reader.Close()
        con.Close()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand

        cmd.Connection = con
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.CommandText = "delete from lookup where type='zyn_批量修改字段'"
        cmd.ExecuteNonQuery()

        For i = 0 To DGV1.Rows.Count - 1
            If DGV1.Rows(i).Cells(3).Value = True Then
                cmd.CommandText = "insert into lookup (type,序号,名称) values ('zyn_批量修改字段'," + i.ToString + ",'" + DGV1.Rows(i).Cells(0).Value.ToString + "')"
                cmd.ExecuteNonQuery()
            End If
        Next
        Me.Dispose()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Dispose()
    End Sub
End Class