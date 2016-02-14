Imports System.Data.SqlClient
Public Class Form_tiaoma

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()
        Me.TextBox1.Text = OpenFileDialog1.FileName
        If OpenFileDialog1.FileName = "" Then Exit Sub
        DataGridView1.Rows.Clear()
        DataGridView1.AllowUserToAddRows = True
        DataGridView1.AllowUserToDeleteRows = True
        DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically
        DataGridView1.Visible = False
        Dim fs As New System.IO.FileStream(OpenFileDialog1.FileName, IO.FileMode.Open)
        Dim sr As New System.IO.StreamReader(fs)
        Dim str, bh As String
        str = sr.ReadToEnd
        sr.Close()
        fs.Close()

        DataGridView1.Rows.Clear()

        Dim ypstr, hj As String
        Dim f As Integer = 0
        Dim bhf As Integer = 0
        Dim jdmax, jdmin As Integer
        jdmin = 0
        jdmax = str.Length
        ProgressBar1.Minimum = jdmin
        ProgressBar1.Maximum = jdmax
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True
        Do
            ypstr = str.Substring(0, str.IndexOf("HJ") - 1)
            str = str.Substring(str.IndexOf("HJ"))
            f = str.IndexOf(",")
            If f = -1 Then
                hj = str
            Else
                hj = str.Substring(0, f)
                str = str.Substring(f + 1)
            End If

            Do
                bhf = ypstr.IndexOf(",")
                If bhf = -1 Then
                    bh = ypstr
                Else
                    bh = ypstr.Substring(0, bhf)
                    ypstr = ypstr.Substring(bhf + 1)
                End If

                Me.DataGridView1.Rows.Add(bh, hj, "准备好导入")

            Loop Until bhf = -1

            ProgressBar1.Value = jdmax - str.Length
        Loop Until f = -1
        DataGridView1.Visible = True
        ProgressBar1.Visible = False
    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not glb_loginpass Then Me.Close()
        Label2.Text = "当前用户:" + glb_姓名
        sqlcon1.ConnectionString = glb_sqlconstr
        ComboBox2.SelectedIndex = 0
        ComboBox1.Items.Clear()
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        cmd.CommandText = "select 名称 from 货架信息 where (科室=@ks or @ks='') order by 名称"
        If glb_loginname = "sa" Then cmd.Parameters.AddWithValue("ks", "") Else cmd.Parameters.AddWithValue("ks", glb_科室)
        reader = cmd.ExecuteReader
        Do While reader.Read
            ComboBox1.Items.Add(reader.Item(0))
        Loop
        reader.Close()
        con.Close()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim Message As String = "确定导入所有数据?"
        Dim Caption As String = "确定导入"
        Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim num As Integer = 0
        Dim Result As DialogResult
        Result = MessageBox.Show(Message, Caption, Buttons)
        ' Try

        If Result = System.Windows.Forms.DialogResult.Yes Then
            sqlcon1.Open()
            SqlCmd1.CommandType = CommandType.StoredProcedure
            SqlCmd1.CommandText = "ZYN_备样条码导入"
            SqlCmd1.Parameters.AddWithValue("样品编号", "")
            SqlCmd1.Parameters.AddWithValue("货架位置", "")
            SqlCmd1.Parameters.AddWithValue("操作人", glb_姓名)
            num = DataGridView1.Rows.Count
            ProgressBar1.Minimum = 0
            ProgressBar1.Maximum = num
            ProgressBar1.Value = 0
            ProgressBar1.Visible = True
            For i = 0 To num - 1
                Try
                    If DataGridView1.Rows(i).Cells(0).Value <> "" And DataGridView1.Rows(i).Visible = True Then
                        SqlCmd1.Parameters(0).Value = DataGridView1.Rows(i).Cells(0).Value
                        SqlCmd1.Parameters(1).Value = DataGridView1.Rows(i).Cells(1).Value
                        SqlCmd1.ExecuteNonQuery()
                        DataGridView1.Rows(i).Visible = False
                    End If
                Catch ex As Exception
                    DataGridView1.Rows(i).Cells(2).Value = "导入未成功"
                End Try
                ProgressBar1.Value = i
            Next
            ProgressBar1.Visible = False
            sqlcon1.Close()
            Buttons = MessageBoxButtons.OK
            Message = "处理完成,请查看是否有未导入记录"
            Caption = "处理完成"
            Result = MessageBox.Show(Message, Caption, Buttons)
            DataGridView1.Rows.Clear()
            DataGridView1.Visible = True
        End If

    End Sub

    Sub load_data()
        DGV2.Rows.Clear()
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        con.Open()
        cmd.Connection = con
        If RadioButton1.Checked Then
            cmd.CommandText = "select top 1000 报告编号,样品名称 from 样品记录 where 到样日期>=@D1 and 到样日期<@d2  and (isnull(存放地点,'')='') and 报告编号 like @bh  group by 报告编号,样品名称 order by 报告编号"
        Else
            cmd.CommandText = "select top 1000 报告编号,样品名称 from 样品记录 where 到样日期>=@D1 and 到样日期<@d2  and 报告编号 like @bh  group by 报告编号,样品名称 order by 报告编号"
        End If
        Dim dat1, dat2 As Date
        Dim w, m As Integer
        Select Case ComboBox2.SelectedItem.ToString
            Case "本日样品"
                dat1 = Today.Date
                dat2 = Today.AddDays(1).Date
            Case "本周样品"
                w = Today.DayOfWeek
                If w = 0 Then w = 7
                dat1 = Today.AddDays(1 - w).Date
                dat2 = Today.AddDays(7 - w + 1).Date
            Case "本月样品"
                m = System.DateTime.DaysInMonth(Today.Year, Today.Month)
                dat1 = Convert.ToDateTime(Today.Year.ToString + "/" + Today.Month.ToString + "/" + "1").Date
                dat2 = Convert.ToDateTime(Today.Year.ToString + "/" + Today.Month.ToString + "/" + m.ToString).AddDays(1).Date
            Case "所有样品"
                dat1 = Convert.ToDateTime("2014-01-01").Date
                dat2 = Today.AddDays(10).Date
        End Select
        cmd.Parameters.AddWithValue("d1", dat1)
        cmd.Parameters.AddWithValue("d2", dat2)
        cmd.Parameters.AddWithValue("bh", "%" + TextBox2.Text + "%")
        reader = cmd.ExecuteReader
        Do While reader.Read
            DGV2.Rows.Add(reader.Item(0), reader.Item(1))
        Loop
        reader.Close()
        con.Close()
    End Sub

   

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call load_data()
    End Sub

    Private Sub TextBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call load_data()
        End If
    End Sub

   
    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        ' Call load_data()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        Call load_data()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Call load_data()
    End Sub

   
    Private Sub DGV2_SelectionChanged(sender As Object, e As EventArgs) Handles DGV2.SelectionChanged
        Dim bgbh As String = ""
        If Not IsNothing(DGV2.CurrentRow) Then
            If DGV2.SelectedRows.Count > 0 Then
                For Each sr In DGV2.SelectedRows
                    If bgbh = "" Then bgbh = sr.cells(0).value Else bgbh = sr.cells(0).value + "," + bgbh
                Next
            Else
                bgbh = DGV2.CurrentRow.Cells(0).Value
            End If
            Call load_selectbh(bgbh)
        Else
            DGV3.Rows.Clear()
        End If


    End Sub

    Sub load_selectbh(ByRef bgbh As String)
        DGV3.Rows.Clear()
        Dim bgcol() As String = Split(bgbh, ",")
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select 样品编号,存放地点 from 样品记录 where 报告编号=@bgbh"
        For Each b In bgcol
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("bgbh", b)
            reader = cmd.ExecuteReader
            Do While reader.Read
                DGV3.Rows.Add(reader.Item(0), reader.Item(1).ToString)
            Loop
            reader.Close()
        Next

        con.Close()
    End Sub

End Class
