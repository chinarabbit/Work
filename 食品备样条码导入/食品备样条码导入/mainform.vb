Imports System.Configuration, System, System.Collections.Specialized

Public Class mainform

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles 备样条码.Click
        Form_tiaoma.Show()
        If Form_tiaoma.WindowState = FormWindowState.Minimized Then Form_tiaoma.WindowState = FormWindowState.Normal
        Form_tiaoma.Select()
    End Sub

    Private Sub mainform_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        Try
            If System.IO.Directory.Exists(Application.StartupPath + "\temp") Then System.IO.Directory.Delete(Application.StartupPath + "\temp", True)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub mainform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoginForm1.ShowDialog()
        If Not glb_loginpass Then Me.Close()

        If InStr(glb_auth, "|样品管理|") > 0 Then 备样条码.Enabled = True
        If InStr(glb_auth, "|批量修改|") > 0 And InStr(glb_管理科室, "业务管理科") > 0 Then 批量修改.Enabled = True
        If InStr(glb_auth, "|报告存档|") > 0 Then 报告存档.Enabled = True Else 报告存档.Enabled = False
        If InStr(glb_auth, "|留样管理|") > 0 Then 留样管理.Enabled = True Else 留样管理.Enabled = False
        If InStr(glb_auth, "|附页管理|") > 0 Then 附页管理.Enabled = True Else 附页管理.Enabled = False
        If InStr(glb_auth, "|报告发放|") > 0 Then 报告发放.Enabled = True Else 报告发放.Enabled = False
        If InStr(glb_auth, "|导出项目|") > 0 Then 导出项目.Enabled = True Else 导出项目.Enabled = False
        If InStr(glb_auth, "|报告打印|") > 0 Then 报告打印.Enabled = True Else 报告打印.Enabled = False
        If InStr(glb_auth, "|任务收费|") > 0 Then 检验缴费.Enabled = True Else 检验缴费.Enabled = False
        If InStr(glb_auth, "|报告编制|") > 0 Or InStr(glb_auth, "|样品主检|") > 0 Then 报告编制.Enabled = True Else 报告编制.Enabled = False
        If InStr(glb_auth, "|报告审核|") > 0 Then 报告审核.Enabled = True Else 报告审核.Enabled = False
        If InStr(glb_auth, "|报告签发|") > 0 Then 报告签发.Enabled = True Else 报告签发.Enabled = False
        If InStr(glb_auth, "|缴费单管理|") > 0 Then 缴费单管理.Enabled = True Else 缴费单管理.Enabled = False
        If InStr(glb_auth, "|协议管理|") > 0 Or InStr(glb_auth, "|协议查看|") > 0 Then 协议管理.Enabled = True Else 协议管理.Enabled = False
        If glb_loginname = "sa" Then
            权限设置.Visible = True
            备样条码.Enabled = True
            批量修改.Enabled = True
            报告存档.Enabled = True
            留样管理.Enabled = True
            附页管理.Enabled = True
            报告发放.Enabled = True
            导出项目.Enabled = True
            协议管理.Enabled = True
            报告打印.Enabled = True
            检验缴费.Enabled = True
            缴费单管理.Enabled = True
            报告编制.Enabled = True
            报告审核.Enabled = True
            报告签发.Enabled = True
        End If
        Try
            If System.IO.Directory.Exists(Application.StartupPath + "\temp") Then System.IO.Directory.Delete(Application.StartupPath + "\temp", True)
            System.IO.Directory.CreateDirectory(Application.StartupPath + "\temp")
            Dim con As New SqlClient.SqlConnection
            Dim cmd As New SqlClient.SqlCommand
            Dim reader As SqlClient.SqlDataReader
            con.ConnectionString = glb_sqlconstr
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "select 文件名称,文件内容,检验类型 from zyn_报表模板"
            reader = cmd.ExecuteReader
            Do While reader.Read
                If Not IsDBNull(reader.Item(1)) Then System.IO.File.WriteAllBytes(Application.StartupPath + "\temp\" + reader.Item(0).ToString, reader.Item(1))
                If Not IsDBNull(reader.Item(2)) Then
                    jylxarray.Add(reader.Item(2))
                    filearray.Add(Application.StartupPath + "\temp\" + reader.Item(0))
                End If
            Loop
            reader.Close()
            con.Close()

        Catch ex As Exception

        End Try
        Me.Text = "主界面-" + glb_姓名
        If glb_loginname = "sa" Then Button2.Visible = True Else Button2.Visible = False

        '显示更新说明
        Call version_update()
    End Sub

    Private Sub 权限设置_Click(sender As Object, e As EventArgs) Handles 权限设置.Click
        '  Dim newform As New Form_权限管理
        ' newform.ShowDialog()
        Form_权限管理.Show()
    End Sub

    Private Sub 任务收费_Click(sender As Object, e As EventArgs) Handles 批量修改.Click
        'Dim newform As New form_批量收费
        'newform.ShowDialog()
        form_批量修改.Show()

        If form_批量修改.WindowState = FormWindowState.Minimized Then form_批量修改.WindowState = FormWindowState.Normal
        form_批量修改.Select()
    End Sub


    Private Sub 报告存档_Click(sender As Object, e As EventArgs) Handles 报告存档.Click
        form_报告存档.Show()
        If form_报告存档.WindowState = FormWindowState.Minimized Then form_报告存档.WindowState = FormWindowState.Normal
        form_报告存档.Select()
    End Sub

    Private Sub 留样信息_Click(sender As Object, e As EventArgs) Handles 留样管理.Click
        Form_留样管理.Show()
        If Form_留样管理.WindowState = FormWindowState.Minimized Then Form_留样管理.WindowState = FormWindowState.Normal
        Form_留样管理.Select()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub

    Private Sub 附页管理_Click(sender As Object, e As EventArgs) Handles 附页管理.Click
        form_附页管理.Show()
        If form_附页管理.WindowState = FormWindowState.Minimized Then form_附页管理.WindowState = FormWindowState.Normal
        form_附页管理.Select()
    End Sub

    Private Sub 报告发放_Click(sender As Object, e As EventArgs) Handles 报告发放.Click
        form_报告发放.Show()
        If form_报告发放.WindowState = FormWindowState.Minimized Then form_报告发放.WindowState = FormWindowState.Normal
        form_报告发放.Select()
    End Sub

    Private Sub 导出项目_Click(sender As Object, e As EventArgs) Handles 导出项目.Click
        form_导出检验项目.Show()
        If form_导出检验项目.WindowState = FormWindowState.Minimized Then form_导出检验项目.WindowState = FormWindowState.Normal
        form_导出检验项目.Select()
    End Sub

    Private Sub 协议管理_Click(sender As Object, e As EventArgs) Handles 协议管理.Click
        Form_协议管理.Show()
        If Form_协议管理.WindowState = FormWindowState.Minimized Then Form_协议管理.WindowState = FormWindowState.Normal
        Form_协议管理.Select()
    End Sub

    Private Sub 报告打印_Click(sender As Object, e As EventArgs) Handles 报告打印.Click
        form_报告打印.Show()
        If form_报告打印.WindowState = FormWindowState.Minimized Then form_报告打印.WindowState = FormWindowState.Normal
        form_报告打印.Select()
    End Sub


    Private Sub 检验缴费_Click(sender As Object, e As EventArgs) Handles 检验缴费.Click
        form_检验缴费.Show()
        If form_检验缴费.WindowState = FormWindowState.Minimized Then form_检验缴费.WindowState = FormWindowState.Normal
        form_检验缴费.Select()
    End Sub

    Private Sub 缴费单管理_Click(sender As Object, e As EventArgs) Handles 缴费单管理.Click
        form_缴费单管理.Show()
        If form_缴费单管理.WindowState = FormWindowState.Minimized Then form_缴费单管理.WindowState = FormWindowState.Normal
        form_缴费单管理.Select()
    End Sub

    Private Sub 报告编制_Click(sender As Object, e As EventArgs) Handles 报告编制.Click
        form_报告编制.Show()
        If form_报告编制.WindowState = FormWindowState.Minimized Then form_报告编制.WindowState = FormWindowState.Normal
        form_报告编制.Select()
    End Sub

    Private Sub 报告审核_Click(sender As Object, e As EventArgs) Handles 报告审核.Click
        Form_报告审核.Show()
        If Form_报告审核.WindowState = FormWindowState.Minimized Then Form_报告审核.WindowState = FormWindowState.Normal
        Form_报告审核.Select()
    End Sub

    Private Sub 报告签发_Click(sender As Object, e As EventArgs) Handles 报告签发.Click
        Form_报告签发.Show()
        If Form_报告签发.WindowState = FormWindowState.Minimized Then Form_报告签发.WindowState = FormWindowState.Normal
        Form_报告签发.Select()
    End Sub

    Sub version_update()
        Dim curver, localver, localdispversion, displayversion As Version
        Dim keystr As String = "Software\食品报告"
        Dim rootkey, key As Microsoft.Win32.RegistryKey
        rootkey = My.Computer.Registry.CurrentUser
        Try
            curver = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion
            key = rootkey.OpenSubKey(keystr)
            If key.GetValue("version") Is Nothing Then
                key = rootkey.CreateSubKey(keystr)
                key.SetValue("version", curver.ToString)
            Else
                localver = New Version(key.GetValue("version").ToString)
                localdispversion = New Version(key.GetValue("displayversion", "0.0.0.0").ToString)
                If curver > localver Then
                    Dim con As New SqlClient.SqlConnection
                    Dim cmd As New SqlClient.SqlCommand
                    Dim reader As SqlClient.SqlDataReader
                    con.ConnectionString = glb_sqlconstr
                    cmd.Connection = con
                    con.Open()
                    cmd.CommandText = "select top 1 版本号,更新说明 from zyn_版本更新 order by id desc"
                    reader = cmd.ExecuteReader
                    If reader.Read Then
                        displayversion = New Version(reader.Item(0).ToString)
                        If localdispversion < displayversion Then
                            MessageBox.Show(reader.Item(1).ToString)
                            key = rootkey.CreateSubKey(keystr)
                            key.SetValue("displayversion", displayversion.ToString)
                        End If
                    End If
                    reader.Close()
                    con.Close()
                    key = rootkey.CreateSubKey(keystr)
                    key.SetValue("version", curver.ToString)
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim ss As String = ConfigurationManager.AppSettings("test")
        MessageBox.Show(ss)
    End Sub
End Class