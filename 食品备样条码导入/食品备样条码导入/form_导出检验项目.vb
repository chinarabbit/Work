Imports System.Data.SqlClient, Microsoft.Office.Interop, System.Reflection, System.ComponentModel
Public Class form_导出检验项目
    Private Sub form_导出检验项目_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select 名称  from lookup where  type='监督检验' order by 序号 "
        reader = cmd.ExecuteReader
        Me.ComboBox1.Items.Add("全部类别")
        Do While reader.Read
            ComboBox1.Items.Add(reader.Item(0).ToString)
        Loop
        reader.Close()

        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        ComboBox4.SelectedIndex = 0
        ComboBox5.SelectedIndex = 0
        con.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
       Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        Dim year, month, jylx, sql, rwpd, dxpd As String
        Dim pnum As Integer = 0
        year = ComboBox2.SelectedItem
        If ComboBox1.SelectedIndex = 0 Then jylx = "" Else jylx = ComboBox1.SelectedItem
        If ComboBox3.SelectedIndex = 0 Then month = "" Else month = ComboBox3.SelectedItem
        If ComboBox4.SelectedIndex = 0 Then rwpd = "" Else rwpd = ComboBox4.SelectedItem
        If ComboBox5.SelectedIndex = 0 Then dxpd = "" Else dxpd = ComboBox5.SelectedItem
        If Len(month) = 1 Then month = "0" + month
        cmd.Parameters.AddWithValue("p1", year)
        cmd.Parameters.AddWithValue("p2", jylx)
        cmd.Parameters.AddWithValue("p3", month)
        cmd.Parameters.AddWithValue("p4", rwpd)
        sql = " guid,创建日期,检验单位,任务类型,检验类型,主检科室,报告编号,样品名称,协议编号,计划编号,抽样单编号,抽样编号 as 样品编号,委托单编号,委托单位,委托单位地址,委托单位联系人,委托单位邮编,委托单位电话,委托单位手机,受检单位,受检单位地址,"
        sql = sql + "受检单位联系人, 受检单位邮编, 受检单位电话, 受检单位手机, 受检单位法人代表, 受检单位所在省份, 受检单位所在城市, 受检单位所在地区, 受检单位营业执照, 生产单位, 生产单位地址, 生产单位联系人,"
        sql = sql + "生产单位邮编, 生产单位电话, 生产单位手机, 生产单位法人代表,生产单位所在省份,生产单位所在城市,生产单位所在地区,生产单位营业执照,生产单位机构代码,抽样单位,抽样单位地址,"
        sql = sql + "抽样单位联系人,抽样单位邮编,抽样单位电话,抽样单位传真,接收短信手机号,委托日期,分派日期,下达日期,商定完成日期,要求完成日期,编制日期,审核日期,签发日期,完成日期,"
        sql = sql + "样品数,样品等级,样品来源,规格型号,商标,生产日期,保质期,送样人,样品堆放方式,样品处理意见,样品特性,样品状态,到样日期,抽样基数,抽样数量,抽样日期,抽样人,抽样地点,抽样方式,单价,任务来源,"
        sql = sql + "备样量,封存地点,封样状态,主检,审核,签发,判定,不合格程度,标准代号,检验依据,标注执行标准,检验项目,分包项目,不合格项,结论,备注,报告备注,"
        sql = sql + "应收费,检验费,收费情况,取报告方式,受理人,加急,入库状态,打印标记,打印份数,发出,备样地点,提供资料,留言,产品类别代码,产品代码四级,业态类型 "

        cmd.CommandText = "select count(*) from 已完成检验任务 where LEFT(报告编号,4)=@p1 and (substring(报告编号,10,2)=@p3 or @p3='') and (检验类型=@p2 or @p2='') and (判定=@p4 or @p4='') and 主检科室='食品二室' and 任务类型='监督检验'"
        reader = cmd.ExecuteReader
        If reader.Read Then pnum = reader.Item(0)
        reader.Close()
        If pnum > 1000 Then
            If MsgBox("超过1000条结果,导出时间较长,确定导出?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                con.Close()
                Exit Sub
            End If
        End If
        cmd.CommandText = "select " + sql + " from 已完成检验任务 where LEFT(报告编号,4)=@p1 and (substring(报告编号,10,2)=@p3 or @p3='') and (检验类型=@p2 or @p2='') and (判定=@p4 or @p4='') and 主检科室='食品二室' and 任务类型='监督检验'"
        reader = cmd.ExecuteReader
        Dim App As New Excel.Application
        Dim Book As Excel.Workbook
        Dim sheet As Excel.Worksheet
        Book = App.Workbooks.Add
        sheet = Book.Sheets(1)
        ' App.Visible = True
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = pnum
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True

        Dim colnum, rownum, num, colmax As Integer
        colnum = 1
        rownum = 1
        num = 1
        Dim xmmc As New ArrayList
        Dim findflag As Boolean = False
        Dim con2 As New SqlConnection
        Dim cmd2 As New SqlCommand
        Dim reader2 As SqlDataReader
        con2.ConnectionString = glb_sqlconstr
        con2.Open()
        cmd2.Connection = con2
        Do While reader.Read
            If rownum = 1 Then
                sheet.Cells(rownum, colnum) = "序号"
                colnum += 1
                For j = 0 To reader.FieldCount - 1
                    sheet.Cells(rownum, colnum) = reader.GetName(j)
                    colnum += 1
                Next
                colmax = colnum - 1
                rownum += 1
            End If
            colnum = 1
                sheet.Cells(rownum, colnum) = num.ToString
                colnum += 1
                For j = 0 To reader.FieldCount - 1
                    sheet.Cells(rownum, colnum) = reader.Item(j)
                    colnum += 1
                Next
                cmd2.Parameters.Clear()
                cmd2.Parameters.AddWithValue("p1", reader.Item("Guid"))
                cmd2.Parameters.AddWithValue("p2", dxpd)
                cmd2.CommandText = "select 项目名称,实测值,单项结论 from 已完成检验项目 where rguid=@p1 and (单项结论=@p2 or @p2='')  order by 显示序号"
                reader2 = cmd2.ExecuteReader
                Do While reader2.Read
                    For k = 0 To xmmc.Count - 1
                        If reader2.Item(0) = xmmc.Item(k) Then
                            sheet.Cells(rownum, colnum + k) = reader2.Item(1)
                            If reader2.Item(2).ToString = "不合格" Then sheet.Cells(rownum, colnum + k).interior.color = 65535
                            findflag = True
                            Exit For
                        End If
                    Next
                    If Not findflag Then
                        colmax += 1
                        xmmc.Add(reader2.Item(0))
                        sheet.Cells(1, colmax) = reader2.Item(0)
                        sheet.Cells(rownum, colmax) = reader2.Item(1)
                        If reader2.Item(2).ToString = "不合格" Then sheet.Cells(rownum, colmax).interior.color = 65535
                    End If
                Loop
                reader2.Close()
                findflag = False

            rownum += 1
            num += 1
            If ProgressBar1.Value = ProgressBar1.Maximum Then ProgressBar1.Value -= 1 Else ProgressBar1.Value += 1
        Loop
        con.Close()
        con2.Close()
        ProgressBar1.Visible = False
        App.Visible = True
    End Sub
End Class