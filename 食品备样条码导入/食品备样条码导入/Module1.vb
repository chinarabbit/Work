Imports System.IO, System.Data.SqlClient, Microsoft.Office.Interop
Module Module1
    Public glb_loginname, glb_sqlconstr, glb_auth, glb_管理科室, glb_姓名, glb_科室 As String
    Public glb_loginpass As Boolean = False
    Public jylxarray As New ArrayList
    Public filearray As New ArrayList
    Public pdfdir As String = ""
    Public rwjd() As Integer = {0, 4, 5, 8, 12, 16, 20}
    Public xmjd() As Integer = {0, 4, 8, 20}
    Public rwjdmc() As String = {"接样受理", "任务分派", "项目检验", "报告编制", "报告审核", "报告签发", "报告完成"}
    Public xmjdmc() As String = {"项目受理", "项目分派", "项目检验", "项目完成"}
    Public Sub main()
        Application.EnableVisualStyles()
        Application.Run(mainform)
    End Sub
    Sub init_inifile()
        If Not System.IO.File.Exists(Application.StartupPath + "\食品报告.ini") Then
            Dim mystream As System.IO.FileStream
            mystream = New System.IO.FileStream(Application.StartupPath + "\食品报告.ini", FileMode.Create)
            mystream.Close()
        End If

    End Sub

    Public Function GetIni(ByVal section As String, ByVal key As String, Optional ByVal lpDefault As String = "", Optional ByVal iniFilePath As String = "") As String
        Dim Str As String = ""
        Str = LSet(Str, 256)
        If iniFilePath = "" Then iniFilePath = Application.StartupPath + "\食品报告.ini"
        GetPrivateProfileString(section, key, lpDefault, Str, Len(Str), iniFilePath)
        Return Microsoft.VisualBasic.Left(Str, InStr(Str, Chr(0)) - 1)
    End Function
    Public Function WriteIni(ByVal section As String, ByVal key As String, ByVal lpDefault As String, Optional ByVal iniFilePath As String = "") As Long
        If iniFilePath = "" Then iniFilePath = Application.StartupPath + "\食品报告.ini"
        Return WritePrivateProfileString(section, key, lpDefault, iniFilePath)
    End Function

    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32

    Public Function printreport(bgbh As String, print_flag As Boolean, Optional copys As Integer = 1, Optional preview As Boolean = False, Optional bug As Boolean = False, Optional pdf As Boolean = False) As Boolean
        Dim filename As String = ""
        Dim jylx As String = ""
        Dim typetext As String = ""
        Dim rguid As String = ""
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim reader As SqlClient.SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()

        Dim sign As String = ""
        Dim itemname As String = ""
        Dim xh1, xh2, xh3 As Integer
        Dim mc1, mc2, mc3, oldmc1, oldmc2, oldmc3 As String
        Dim xhtext, mctext, dw As String
        xh1 = 0
        xh2 = 0
        xh3 = 0
        mc1 = ""
        mc2 = ""
        mc3 = ""
        oldmc1 = ""
        oldmc2 = ""
        oldmc3 = ""
        xhtext = ""
        mctext = ""
        Dim itemarray As New ArrayList
        Dim docend As Boolean = False
        Dim app As New Word.Application
        Dim doc As Word.Document
        cmd.CommandText = "select * from [KingsLims].[dbo].[sys_view_检验任务] where 报告编号='" + bgbh + "'"
        reader = cmd.ExecuteReader
        If reader.Read Then
            If IsDBNull(reader.Item("检验类型")) Then jylx = "" Else jylx = reader.Item("检验类型")

            For f = 0 To jylxarray.Count - 1
                If jylxarray(f) = jylx Then
                    filename = filearray(f)
                    Exit For
                End If
            Next
            If filename = "" Then Return False
            If Not System.IO.File.Exists(filename) Then Return False
            rguid = reader.Item("guid")
            app.ScreenUpdating = False
            doc = app.Documents.Add(filename)
            doc.Activate()
            'doc.SaveAs2(Application.StartupPath + "\temp\" + bgbh)
            If bug Then
                app.Visible = True
                app.ScreenUpdating = True
            End If
            app.Selection.Find.Forward = True
            app.Selection.Find.ClearFormatting()
            app.Selection.Find.MatchWholeWord = False
            app.Selection.Find.MatchWildcards = True
            app.Selection.Find.MatchCase = False
            app.Selection.Find.Text = "#/*/#"

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
                        ' If itemname = "抽样日期" Or itemname = "到样日期" Or itemname = "签发日期" Then
                        If reader.GetFieldType(reader.GetOrdinal(itemname)) Is GetType(DateTime) Then
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
            '插入签名
            app.Selection.GoTo(-1,,, "主检签名")
            If IsDBNull(reader.Item("主检")) Then sign = "" Else sign = reader.Item("主检")
            If download_sign_jpg(sign) Then app.Selection.InlineShapes.AddPicture(Application.StartupPath + "\temp\" + sign + ".jpg")
            app.Selection.GoTo(-1,,, "审核签名")
            If IsDBNull(reader.Item("审核")) Then sign = "" Else sign = reader.Item("审核")
            If download_sign_jpg(sign) Then app.Selection.InlineShapes.AddPicture(Application.StartupPath + "\temp\" + sign + ".jpg")
            app.Selection.GoTo(-1,,, "批准签名")
            If IsDBNull(reader.Item("签发")) Then sign = "" Else sign = reader.Item("签发")
            If download_sign_jpg(sign) Then app.Selection.InlineShapes.AddPicture(Application.StartupPath + "\temp\" + sign + ".jpg")
        End If
        reader.Close()
        If rguid = "" Then
            Return False
        Else
            '写检验项目
            app.Selection.GoTo(-1,,, "附页项目")
            xh1 = 0
            xh2 = 0
            xh3 = 0
            mc1 = ""
            mc2 = ""
            mc3 = ""
            oldmc1 = ""
            oldmc2 = ""
            oldmc3 = ""
            Dim curnum As Integer = 0
            cmd.CommandText = "select * from [KingsLims].[dbo].[sys_view_检验项目] where rguid='" + rguid + "' order by 显示序号"
            reader = cmd.ExecuteReader
            Do While reader.Read
                If curnum > 0 Then app.Selection.MoveRight(12, 1)
                '分析序号和名称
                If IsDBNull(reader.Item("名称1")) Then mc1 = "" Else mc1 = reader.Item("名称1")
                If IsDBNull(reader.Item("名称2")) Then mc2 = "" Else mc2 = reader.Item("名称2")
                If IsDBNull(reader.Item("名称3")) Then mc3 = "" Else mc3 = reader.Item("名称3")
                If IsDBNull(reader.Item("单位")) Then dw = "" Else dw = reader.Item("单位")
                If mc1 <> oldmc1 Then
                    xh1 += 1
                    xh2 = 0
                    xh3 = 0
                    mctext = mc1 + dw
                    xhtext = xh1.ToString
                Else
                    If mc2 <> oldmc2 Then
                        xh2 += 1
                        xh3 = 0
                        mctext = mc2 + dw
                        xhtext = xh1.ToString + "." + xh2.ToString
                    Else
                        If mc3 <> oldmc3 Then
                            xh3 += 1
                            mctext = mc3 + dw
                            xhtext = xh1.ToString + "." + xh2.ToString + "." + xh3.ToString
                        End If
                    End If
                End If
                oldmc1 = mc1
                oldmc2 = mc2
                oldmc3 = mc3
                If Not (mc1 = "标签" And xh2 > 0) Then app.Selection.Text = xhtext
                app.Selection.MoveRight(12, 1)
                If Not (mc1 = "标签" And xh2 > 0) Then app.Selection.Text = mctext
                app.Selection.MoveRight(12, 1)
                If jylx <> "委托测试" Then
                    If Not IsDBNull(reader.Item("标准值")) Then app.Selection.Text = reader.Item("标准值")
                    app.Selection.MoveRight(12, 1)
                End If
                If Not IsDBNull(reader.Item("实测值")) Then app.Selection.Text = reader.Item("实测值")
                app.Selection.MoveRight(12, 1)
                If jylx = "委托测试" Then
                    If Not IsDBNull(reader.Item("备注")) Then app.Selection.Text = reader.Item("备注")
                Else
                    If Not IsDBNull(reader.Item("单项结论")) Then app.Selection.Text = reader.Item("单项结论")
                End If

                curnum += 1
            Loop
            reader.Close()

            app.ActiveDocument.Bookmarks.Add("temp", app.Selection.Range)
            '替换上下标
            Dim tmpstr As String = ""
            app.Selection.Find.Forward = True
            app.Selection.Find.ClearFormatting()
            app.Selection.Find.MatchWholeWord = False
            app.Selection.Find.MatchWildcards = True
            app.Selection.Find.MatchCase = False
            app.Selection.Find.Text = "^^\(*\)"
            app.Selection.HomeKey(Word.WdUnits.wdStory)
            Do
                app.Selection.Find.ClearFormatting()
                app.Selection.Find.Execute()
                If InStr(app.Selection.Text, "^(") > 0 Then
                    tmpstr = app.Selection.Text
                    tmpstr = Replace(tmpstr, "^(", "")
                    tmpstr = Replace(tmpstr, ")", "")
                    app.Selection.Font.Superscript = Word.WdConstants.wdToggle
                    app.Selection.Text = tmpstr
                    app.Selection.MoveRight()
                Else
                    Exit Do
                End If
            Loop
            tmpstr = ""
            app.Selection.Find.Text = "_\(*\)"
            app.Selection.HomeKey(Word.WdUnits.wdStory)
            Do
                app.Selection.Find.ClearFormatting()
                app.Selection.Find.Execute()
                If InStr(app.Selection.Text, "_(") > 0 Then
                    tmpstr = app.Selection.Text
                    tmpstr = Replace(tmpstr, "_(", "")
                    tmpstr = Replace(tmpstr, ")", "")
                    app.Selection.Font.Subscript = Word.WdConstants.wdToggle
                    app.Selection.Text = tmpstr
                    app.Selection.MoveRight()
                Else
                    Exit Do
                End If
            Loop
            Dim totalpage, curpage, rowheight As Integer
            totalpage = app.Selection.Information(Word.WdInformation.wdNumberOfPagesInDocument)
            app.Selection.GoTo(-1,,, "附页表头")
            app.Selection.Borders(-3).LineStyle = Word.WdLineStyle.wdLineStyleSingle
            app.Selection.Borders(-3).LineWidth = Word.WdLineWidth.wdLineWidth100pt
            app.Selection.MoveDown()
            curpage = app.Selection.Information(Word.WdInformation.wdActiveEndPageNumber)
            'app.Selection.GoTo(-1,,, "temp")

            Do Until curpage = totalpage
                app.Selection.MoveDown()
                If app.Selection.Information(Word.WdInformation.wdActiveEndPageNumber) > curpage Then
                    If app.Selection.Information(Word.WdInformation.wdWithInTable) Then
                        app.Selection.SelectRow()
                        app.Selection.Borders(-1).LineStyle = Word.WdLineStyle.wdLineStyleSingle
                        app.Selection.Borders(-1).LineWidth = Word.WdLineWidth.wdLineWidth100pt
                    End If
                    curpage = app.Selection.Information(Word.WdInformation.wdActiveEndPageNumber)
                End If
            Loop

            app.Selection.GoTo(-1,,, "temp")
            app.Selection.MoveRight()
            If app.Selection.Information(Word.WdInformation.wdVerticalPositionRelativeToPage) < 700 Then
                curpage = app.Selection.Information(Word.WdInformation.wdActiveEndPageNumber)
                app.Selection.MoveRight(12, 3)
                If curpage = app.Selection.Information(Word.WdInformation.wdActiveEndPageNumber) Then
                    app.Selection.Text = "以下空白"
                    'app.ActiveDocument.Bookmarks.Add("temp", app.Selection.Range)
                    'app.Selection.SelectRow()
                    'app.Selection.Borders(-3).LineStyle = Word.WdLineStyle.wdLineStyleSingle
                    'app.Selection.Borders(-3).LineWidth = Word.WdLineWidth.wdLineWidth100pt
                    If curnum = 1 Then
                        app.Selection.SelectRow()
                        app.Selection.Borders(-1).LineStyle = Word.WdLineStyle.wdLineStyleNone
                        app.Selection.MoveLeft()
                    End If
                    '增加最后一行行高
                    'app.Selection.GoTo(-1,,, "temp")
                    app.Selection.Rows.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly
                    rowheight = app.Selection.Rows.Height
                    Do Until rowheight > 900
                        rowheight += 20
                        app.Selection.Rows.Height = rowheight
                        If app.Selection.Information(Word.WdInformation.wdNumberOfPagesInDocument) > totalpage Then
                            app.Selection.Rows.Height = rowheight - 20
                            Exit Do
                        End If
                    Loop
                Else
                    app.Selection.Rows.Delete()
                End If
            End If



            If Not (bug Or preview Or pdf) Then
                'app.PrintPreview = False
                app.ScreenUpdating = True
                app.ScreenRefresh()
                app.PrintOut(False,,,,,,, copys)
                app.Quit(False)
                cmd.CommandText = "insert into 打印日志 (报告编号,操作人,打印内容,打印份数) values ('" + bgbh + "','" + glb_姓名 + "','" + "检验报告" + "','" + copys.ToString + "')"
                cmd.ExecuteNonQuery()
                If print_flag Then
                    cmd.CommandText = "update 已完成检验任务 set 打印标记=1 where 报告编号='" + bgbh + "'"
                    cmd.ExecuteNonQuery()
                End If
            Else
                If pdf Then
                    cmd.CommandText = "insert into 打印日志 (报告编号,操作人,打印内容,打印份数) values ('" + bgbh + "','" + glb_姓名 + "','" + "导出pdf" + "','1')"
                    cmd.ExecuteNonQuery()
                    If Right(pdfdir, 1) = "\" Then pdfdir = Left(pdfdir, Len(pdfdir) - 1)
                    app.ScreenUpdating = True
                    app.ScreenRefresh()
                    app.ActiveDocument.ExportAsFixedFormat(pdfdir + "\" + bgbh + ".pdf", Word.WdExportFormat.wdExportFormatPDF, False, Word.WdExportOptimizeFor.wdExportOptimizeForPrint, Word.WdExportRange.wdExportAllDocument, 1, 1, Word.WdExportItem.wdExportDocumentContent, True, True, Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks, True, True, False)
                    app.Quit(False)
                Else
                    '  cmd.CommandText = "insert into 打印日志 (报告编号,操作人,打印内容,打印份数) values ('" + bgbh + "','" + glb_姓名 + "','" + "预览报告" + "','1')"
                    ' cmd.ExecuteNonQuery()
                    app.ScreenUpdating = True
                    app.ScreenRefresh()
                    app.Visible = True
                    app.ShowMe()
                End If
            End If
        End If

        con.Close()
        Return True
    End Function
    Public Function printreport2(bgbh As String, print_flag As Boolean, Optional copys As Integer = 1, Optional preview As Boolean = False, Optional bug As Boolean = False, Optional pdf As Boolean = False) As Boolean
        Dim filename As String = ""
        Dim jylx As String = ""
        Dim typetext As String = ""
        Dim rguid As String = ""
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim reader As SqlClient.SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()

        Dim sign As String = ""
        Dim itemname As String = ""
        Dim xh1, xh2, xh3 As Integer
        Dim mc1, mc2, mc3, oldmc1, oldmc2, oldmc3 As String
        Dim xhtext, mctext, dw As String
        xh1 = 0
        xh2 = 0
        xh3 = 0
        mc1 = ""
        mc2 = ""
        mc3 = ""
        oldmc1 = ""
        oldmc2 = ""
        oldmc3 = ""
        xhtext = ""
        mctext = ""
        Dim infoname(), infodata(), data(,) As String
        Dim xharray, mcarray, bzzarray, sczarray, dxjlarray, bzarray As New ArrayList
        Dim app As New Word.Application
        Dim doc As Word.Document
        cmd.CommandText = "select * from [KingsLims].[dbo].[sys_view_检验任务] where 报告编号='" + bgbh + "'"
        reader = cmd.ExecuteReader
        If reader.Read Then
            If IsDBNull(reader.Item("检验类型")) Then jylx = "" Else jylx = reader.Item("检验类型")

            For f = 0 To jylxarray.Count - 1
                If jylxarray(f) = jylx Then
                    filename = filearray(f)
                    Exit For
                End If
            Next
            If filename = "" Then Return False
            If Not System.IO.File.Exists(filename) Then Return False
            rguid = reader.Item("guid")
            doc = app.Documents.Add(filename)
            doc.Activate()

            If bug Then
                app.Visible = True
                app.ScreenUpdating = True
            Else
                app.ScreenUpdating = False
            End If
            ReDim infoname(reader.FieldCount - 1)
            ReDim infodata(reader.FieldCount - 1)
            For i = 0 To reader.FieldCount - 1
                infoname(i) = reader.GetName(i).ToString
                infodata(i) = reader.Item(i).ToString
            Next
            ' app.Run("infowrite", infoname, infodata)
            '插入签名
            app.Selection.GoTo(-1,,, "主检签名")
            If IsDBNull(reader.Item("主检")) Then sign = "" Else sign = reader.Item("主检")
            If download_sign_jpg(sign) Then app.Selection.InlineShapes.AddPicture(Application.StartupPath + "\temp\" + sign + ".jpg")
            app.Selection.GoTo(-1,,, "审核签名")
            If IsDBNull(reader.Item("审核")) Then sign = "" Else sign = reader.Item("审核")
            If download_sign_jpg(sign) Then app.Selection.InlineShapes.AddPicture(Application.StartupPath + "\temp\" + sign + ".jpg")
            app.Selection.GoTo(-1,,, "批准签名")
            If IsDBNull(reader.Item("签发")) Then sign = "" Else sign = reader.Item("签发")
            If download_sign_jpg(sign) Then app.Selection.InlineShapes.AddPicture(Application.StartupPath + "\temp\" + sign + ".jpg")
        End If
        reader.Close()
        If rguid = "" Then
            Return False
        Else
            '写检验项目
            xh1 = 0
            xh2 = 0
            xh3 = 0
            mc1 = ""
            mc2 = ""
            mc3 = ""
            oldmc1 = ""
            oldmc2 = ""
            oldmc3 = ""
            Dim curnum As Integer = 0
            cmd.CommandText = "select * from [KingsLims].[dbo].[sys_view_检验项目] where rguid='" + rguid + "' order by 显示序号"
            reader = cmd.ExecuteReader

            Do While reader.Read
                '分析序号和名称
                If IsDBNull(reader.Item("名称1")) Then mc1 = "" Else mc1 = reader.Item("名称1")
                If IsDBNull(reader.Item("名称2")) Then mc2 = "" Else mc2 = reader.Item("名称2")
                If IsDBNull(reader.Item("名称3")) Then mc3 = "" Else mc3 = reader.Item("名称3")
                If IsDBNull(reader.Item("单位")) Then dw = "" Else dw = reader.Item("单位")
                If mc1 <> oldmc1 Then
                    xh1 += 1
                    xh2 = 0
                    xh3 = 0
                    mctext = mc1 + dw
                    xhtext = xh1.ToString
                Else
                    If mc2 <> oldmc2 Then
                        xh2 += 1
                        xh3 = 0
                        mctext = mc2 + dw
                        xhtext = xh1.ToString + "." + xh2.ToString
                    Else
                        If mc3 <> oldmc3 Then
                            xh3 += 1
                            mctext = mc3 + dw
                            xhtext = xh1.ToString + "." + xh2.ToString + "." + xh3.ToString
                        End If
                    End If
                End If
                oldmc1 = mc1
                oldmc2 = mc2
                oldmc3 = mc3
                If Not (mc1 = "标签" And xh2 > 0) Then xharray.Add(xhtext) Else xharray.Add("")
                If Not (mc1 = "标签" And xh2 > 0) Then mcarray.Add(mctext) Else mcarray.Add("")
                bzzarray.Add(reader.Item("标准值").ToString)
                sczarray.Add(reader.Item("实测值").ToString)
                bzarray.Add(reader.Item("备注").ToString)
                dxjlarray.Add(reader.Item("单项结论").ToString)
                curnum += 1
            Loop
            reader.Close()
            ReDim data(5, curnum - 1)
            For i = 0 To curnum - 1
                data(0, i) = xharray(i)
                data(1, i) = mcarray(i)
                data(2, i) = bzzarray(i)
                data(3, i) = sczarray(i)
                data(4, i) = dxjlarray(i)
                data(5, i) = bzarray(i)
            Next
            'app.Run("datawrite", data)


            If Not (bug Or preview Or pdf) Then
                'app.PrintPreview = False
                app.ScreenUpdating = True
                app.ScreenRefresh()
                app.PrintOut(False,,,,,,, copys)
                app.Quit(False)
                cmd.CommandText = "insert into 打印日志 (报告编号,操作人,打印内容,打印份数) values ('" + bgbh + "','" + glb_姓名 + "','" + "检验报告" + "','" + copys.ToString + "')"
                cmd.ExecuteNonQuery()
                If print_flag Then
                    cmd.CommandText = "update 已完成检验任务 set 打印标记=1 where 报告编号='" + bgbh + "'"
                    cmd.ExecuteNonQuery()
                End If
            Else
                If pdf Then
                    cmd.CommandText = "insert into 打印日志 (报告编号,操作人,打印内容,打印份数) values ('" + bgbh + "','" + glb_姓名 + "','" + "导出pdf" + "','1')"
                    cmd.ExecuteNonQuery()
                    If Right(pdfdir, 1) = "\" Then pdfdir = Left(pdfdir, Len(pdfdir) - 1)
                    app.ScreenUpdating = True
                    app.ScreenRefresh()
                    app.ActiveDocument.ExportAsFixedFormat(pdfdir + "\" + bgbh + ".pdf", Word.WdExportFormat.wdExportFormatPDF, False, Word.WdExportOptimizeFor.wdExportOptimizeForPrint, Word.WdExportRange.wdExportAllDocument, 1, 1, Word.WdExportItem.wdExportDocumentContent, True, True, Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks, True, True, False)
                    app.Quit(False)
                Else
                    '  cmd.CommandText = "insert into 打印日志 (报告编号,操作人,打印内容,打印份数) values ('" + bgbh + "','" + glb_姓名 + "','" + "预览报告" + "','1')"
                    ' cmd.ExecuteNonQuery()
                    app.ScreenUpdating = True
                    app.ScreenRefresh()
                    app.Visible = True
                    app.ShowMe()
                End If
            End If
        End If

        con.Close()
        Return True
    End Function
    Public Function jyrw_print(rguid As String, dotname As String) As Word.Application
        If rguid = "" Then Return Nothing
        Dim filename As String
        filename = Application.StartupPath + "\temp\" + dotname
        If Not System.IO.File.Exists(filename) Then Return Nothing
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
        cmd.CommandText = "select * from sys_view_检验任务 where guid='" + rguid + "'"
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
        Loop
        reader.Close()
        con.Close()
        app.ScreenUpdating = True
        app.ScreenRefresh()
        Return app
    End Function
    Public Function download_sign_jpg(name As String) As Boolean
        If name = "" Then
            Return False
        Else
            If System.IO.File.Exists(Application.StartupPath + "\temp\" + name + ".jpg") Then Return True
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "SELECT a.电子签名  FROM [KingsLims].[dbo].[登录用户] a inner join [KingsLims].[dbo].[人事信息] b  on a.人员ID =b.guid where b.姓名='" + name + "'"
            reader = cmd.ExecuteReader
            If reader.Read Then
                Try
                    System.IO.File.WriteAllBytes(Application.StartupPath + "\temp\" + name + ".jpg", reader.Item(0))
                Catch ex As Exception
                    Return False
                End Try
                Return True
            End If
        End If
        Return True
    End Function
    Public Sub save_color(form_name As String, color_name As String, color As Int32)
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        cmd.CommandText = "delete from zyn_颜色 where userid='" + glb_loginname + "' and form_name='" + form_name + "' and color_name='" + color_name + "'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into zyn_颜色 (userid,form_name,color_name,color) values ('" + glb_loginname + "','" + form_name + "','" + color_name + "'," + color.ToString + ")"
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub
    Function upper_money(ByVal XXJE As String) As String
        '小写转换成大写
        Dim SL As String
        Dim JE As String
        Dim m As String
        Dim dxje As String
        Dim n As Integer
        Dim j As Integer
        Dim i As Integer
        Dim w As Integer
        SL = "零壹贰叁肆伍陆柒捌玖"
        JE = "分角元拾佰仟万拾佰仟亿拾佰仟"
        m = Trim(XXJE)
        If InStr(m, ".") <= 0 Then
            m = Trim(m) + "00"
        End If
        If Len(m) - InStr(m, ".") = 1 Then
            m = m + "0"
        End If
        m = Replace(Trim(m), ".", "")
        n = Len(m)
        j = n
        dxje = ""
        For i = 1 To n
            w = Val(Mid(m, i, 1))
            If w > 0 Then
                dxje = dxje + Mid(SL, w + 1, 1)
                dxje = dxje + Mid(JE, j, 1)
            ElseIf w = 0 Then
                If Mid(JE, j, 1) = "万" Then
                    dxje = dxje + Mid(JE, j, 1)
                ElseIf Mid(JE, j, 1) = "元" Then
                    If Len(m) = 3 Then
                        dxje = dxje + "零"
                    End If
                    dxje = dxje + "元"
                    If Val(Mid(m, i + 1, 1)) > 0 And Len(m) > 3 Then
                        dxje = dxje + Mid(SL, w + 1, 1)
                    End If
                ElseIf Val(Mid(m, i + 1, 1)) > 0 Then
                    dxje = dxje + Mid(SL, w + 1, 1)
                ElseIf Val(Mid(m, i)) = 0 Then
                    If j > 10 Then dxje = dxje + "亿"
                    If (j >= 7) And (j <= 9) Then dxje = dxje + "万"
                    If j >= 3 Then dxje = dxje + "元"
                    dxje = dxje + "整"
                    Exit For
                End If
            End If
            j = j - 1
        Next
        upper_money = dxje
    End Function

    Public Class DataGridViewComboBoxColumnEx
        Inherits DataGridViewComboBoxColumn
        Private _value As ComboBoxStyle = ComboBoxStyle.DropDownList
        Public Property dropdownstyle As ComboBoxStyle
            Get
                Return _value
            End Get
            Set(ByVal value As ComboBoxStyle)
                _value = value
            End Set
        End Property
    End Class

    Public Class DataGridViewTextboxColumn_Combox
        Inherits DataGridViewTextBoxColumn
        Private combo_items As New ArrayList
        Public Property comboitem As ArrayList
            Get
                Return combo_items
            End Get
            Set(value As ArrayList)
                combo_items = value
            End Set
        End Property

    End Class

    Public Function flow_next(rguid As String, curtaskstep As Integer, curitemstep As Integer) As Boolean
        Dim nexttaskstep, nextitemstep, returnstep As Integer
        nextitemstep = -1
        nexttaskstep = -1
        Dim curtaskmc, nexttaskmc As String
        curtaskmc = ""
        nexttaskmc = ""
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        cmd.CommandText = "select isnull(返工,-1) from sys_view_检验任务 where guid='" + rguid + "'"
        con.Open()
        reader = cmd.ExecuteReader
        If reader.Read Then
            returnstep = reader.Item(0)
        End If
        reader.Close()
        con.Close()
        If curtaskstep >= 0 And rguid <> "" Then
            For i = 0 To rwjd.Count - 1
                If rwjd(i) = curtaskstep Then
                    If i < rwjd.Count - 1 Then
                        nexttaskstep = rwjd(i + 1)
                        nexttaskmc = rwjdmc(i + 1)
                        curtaskmc = rwjdmc(i)
                    Else
                        nexttaskstep = curtaskstep
                        nexttaskmc = rwjdmc(i)
                        curtaskmc = rwjdmc(i)
                    End If
                End If
            Next
            For i = 0 To xmjd.Count - 1
                If xmjd(i) = curitemstep Then
                    If i < xmjd.Count - 1 Then nextitemstep = xmjd(i + 1) Else nextitemstep = curitemstep
                End If
            Next
        End If
        If returnstep > nexttaskstep Then
            For i = 0 To rwjd.Count - 1
                If rwjd(i) = returnstep Then
                    nexttaskstep = returnstep
                    nexttaskmc = rwjdmc(i)
                End If
            Next
        End If
        If returnstep > nexttaskstep Then Return False
        If nexttaskstep = -1 Or nextitemstep = -1 Then Return False
        dialog_flownext.lab_curstep.Text = curtaskmc
        dialog_flownext.lab_nextstep.Text = nexttaskmc
        If curtaskstep = 16 Then
            dialog_flownext.Lab_qfrq.Visible = True
            dialog_flownext.qfrq.Visible = True
        Else
            dialog_flownext.Lab_qfrq.Visible = False
            dialog_flownext.qfrq.Visible = False
        End If
        If dialog_flownext.ShowDialog = DialogResult.OK Then
            con.Open()
            cmd.CommandType = CommandType.Text
            cmd.Parameters.Clear()
            cmd.CommandText = "update 检验任务 set 签发日期=@qfrq where guid=@guid"
            cmd.Parameters.AddWithValue("qfrq", dialog_flownext.qfrq.Value.Date)
            cmd.Parameters.AddWithValue("guid", rguid)
            cmd.ExecuteNonQuery()

            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sys_Proc_下达任务"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("begintaskvalue", curtaskstep)
            cmd.Parameters.AddWithValue("endtaskvalue", nexttaskstep)
            cmd.Parameters.AddWithValue("beginitemvalue", curitemstep)
            cmd.Parameters.AddWithValue("enditemvalue", nextitemstep)
            cmd.Parameters.AddWithValue("op", glb_loginname)
            cmd.Parameters.AddWithValue("postil", "正常流转")
            cmd.Parameters.AddWithValue("currdate", Now())
            cmd.Parameters.AddWithValue("rguid", rguid)
            cmd.ExecuteNonQuery()
            con.Close()
            Return True
        Else
            Return False
        End If

    End Function

    Public Function flow_prev(rguid As String, curtaskstep As Integer, curitemstep As Integer) As Boolean
        Dim prevtaskstep, previtemstep, returnstep As Integer
        Dim curtaskmc, prevtaskmc, dhyy As String
        previtemstep = curitemstep
        curtaskmc = ""
        prevtaskstep = -1
        returnstep = -1
        Dialog_flowprev.comb_prevstep.Items.Clear()
        For i = 0 To rwjd.Count - 1
            If rwjd(i) = curtaskstep Then Exit For
            Dialog_flowprev.comb_prevstep.Items.Add(rwjdmc(i))
            Dialog_flowprev.comb_prevstep.SelectedItem = rwjdmc(i)
        Next
        For i = 0 To rwjd.Count - 1
            If rwjd(i) = curtaskstep Then
                curtaskmc = rwjdmc(i)
                Exit For
            End If
        Next
        Dialog_flowprev.lab_curstep.Text = curtaskmc
        If Dialog_flowprev.ShowDialog = DialogResult.OK Then
            prevtaskmc = Dialog_flowprev.comb_prevstep.SelectedItem
            If prevtaskmc <> "" Then
                For i = 0 To rwjdmc.Count - 1
                    If rwjdmc(i) = prevtaskmc Then
                        prevtaskstep = rwjd(i)
                        Exit For
                    End If
                Next
                If prevtaskstep = -1 Then Return False
                If Dialog_flowprev.rb2.Checked Then returnstep = curtaskstep
                dhyy = Dialog_flowprev.TextBox1.Text
                Dim con As New SqlConnection
                Dim cmd As New SqlCommand
                con.ConnectionString = glb_sqlconstr
                cmd.Connection = con
                con.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sys_Proc_打回任务"
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("begintaskvalue", curtaskstep)
                cmd.Parameters.AddWithValue("endtaskvalue", prevtaskstep)
                cmd.Parameters.AddWithValue("beginitemvalue", curitemstep)
                cmd.Parameters.AddWithValue("enditemvalue", previtemstep)
                cmd.Parameters.AddWithValue("op", glb_loginname)
                cmd.Parameters.AddWithValue("postil", dhyy)
                cmd.Parameters.AddWithValue("currdate", Now())
                cmd.Parameters.AddWithValue("rguid", rguid)
                cmd.Parameters.AddWithValue("rework", returnstep)
                cmd.Parameters.AddWithValue("BackItemValue", "")
                cmd.ExecuteNonQuery()
                con.Close()
                Return True
            Else
                Return False
            End If
            Else
            Return False
        End If


    End Function

End Module
