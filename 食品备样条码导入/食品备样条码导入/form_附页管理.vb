Imports System.Data.SqlClient
Public Class form_附页管理

    Private Sub form_附页管理_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        ComboBox1.SelectedIndex = 0
        con.Close()
        pgb1.Minimum = 0
        pgb1.Maximum = 0
        pgb1.Visible = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim myexcel As New Microsoft.Office.Interop.Excel.Application
        Dim workbook As Microsoft.Office.Interop.Excel.Workbook
        Dim sheet As Microsoft.Office.Interop.Excel.Worksheet
        Dim keystr As String = "Software\食品报告"
        Dim rootkey, key As Microsoft.Win32.RegistryKey
        rootkey = My.Computer.Registry.CurrentUser
        key = rootkey.OpenSubKey(keystr, True)
        Dim filename, month As String
        Dim page As Integer = 0
        If Mydgv1.get_rownum = 0 Then Exit Sub
        pgb1.Maximum = Mydgv1.get_rownum - 1
        pgb1.Visible = True
        For i = 0 To Mydgv1.get_rownum - 1
            pgb1.Value = i
            month = Mydgv1.get_cellvalue(i, "创建日期").ToString.Substring(0, 7)
            filename = "\\" + key.GetValue("server").ToString + "\kingslims$\Ysjl\" + month + "\" + Mydgv1.get_cellvalue(i, "服务器文件")
            If System.IO.File.Exists(filename) Then
                workbook = myexcel.Workbooks.Open(filename)
                sheet = workbook.Sheets(1)
                page = sheet.PageSetup.Pages.Count + 1
                Mydgv1.set_cellvalue(i, "实际页数", page.ToString)
                If Mydgv1.get_cellvalue(i, "总页数") = Mydgv1.get_cellvalue(i, "实际页数") Then
                    Mydgv1.set_rowvisible(i, False)
                End If
                workbook.Close(False)
            Else
                Mydgv1.set_cellvalue(i, "实际页数", "-1")
            End If
        Next
        pgb1.Visible = False
    End Sub

  
    Private Sub Mydgv1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Mydgv1.MouseDoubleClick
        Dim row As Integer = -1
        row = Mydgv1.get_currentrow
        If row = -1 Then Exit Sub
        Dim myexcel As New Microsoft.Office.Interop.Excel.Application
        Dim workbook As Microsoft.Office.Interop.Excel.Workbook

        Dim keystr As String = "Software\食品报告"
        Dim rootkey, key As Microsoft.Win32.RegistryKey
        rootkey = My.Computer.Registry.CurrentUser
        key = rootkey.OpenSubKey(keystr, True)
        Dim filename, month As String
        month = Mydgv1.get_cellvalue(row, "创建日期").ToString.Substring(0, 7)
        filename = "\\" + key.GetValue("server").ToString + "\kingslims$\Ysjl\" + month + "\" + Mydgv1.get_cellvalue(row, "服务器文件")
        If System.IO.File.Exists(filename) Then
            workbook = myexcel.Workbooks.Open(filename)
            myexcel.Visible = True
        End If
    End Sub

 
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim cmd As New SqlCommand
        cmd.CommandText = "select a.guid as rguid,b.guid as yguid,a.报告编号,a.样品名称,a.创建日期,a.总页数,0 as 实际页数,b.服务器文件 from 已完成检验任务 a inner join 原始记录索引 b on a.guid=b.rguid where a.创建日期>=@date1 and a.创建日期<@date2 and (a.检验类型=@检验类型 or @检验类型='')"
        cmd.Parameters.AddWithValue("date1", DateTimePicker1.Value.Date)
        cmd.Parameters.AddWithValue("date2", DateTimePicker2.Value.AddDays(1).Date)
        If ComboBox1.SelectedIndex <> 0 Then cmd.Parameters.AddWithValue("检验类型", ComboBox1.SelectedItem) Else cmd.Parameters.AddWithValue("检验类型", "")
        Mydgv1.load_data(cmd)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        Dim page, guid As String
        For i = 0 To Mydgv1.get_rownum - 1
            If Mydgv1.get_rowvisible(i) = True And Mydgv1.get_cellvalue(i, "实际页数") <> "-1" Then
                page = Mydgv1.get_cellvalue(i, "实际页数")
                guid = Mydgv1.get_cellvalue(i, "rguid")
                cmd.CommandText = "update 已完成检验任务 set 总页数=" + page + " where guid='" + guid + "'"
                Call cmd.ExecuteNonQuery()
                Mydgv1.set_rowvisible(i, False)
            End If
        Next
        con.Close()

    End Sub
End Class