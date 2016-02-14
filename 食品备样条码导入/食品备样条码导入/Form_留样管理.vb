Public Class Form_留样管理

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim cmd As New SqlClient.SqlCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "zyn_留样管理读取任务信息"
        cmd.Parameters.AddWithValue("date1", DateTimePicker1.Value.Date)
        cmd.Parameters.AddWithValue("date2", DateTimePicker2.Value.Date)
        If ComboBox1.SelectedItem <> "全部科室" Then cmd.Parameters.AddWithValue("主检科室", ComboBox1.SelectedItem)
        If ComboBox2.SelectedItem <> "全部" Then cmd.Parameters.AddWithValue("判定", ComboBox2.SelectedItem.ToString)
        If ComboBox3.SelectedItem <> "全部" Then cmd.Parameters.AddWithValue("样品类型", ComboBox3.SelectedItem.ToString)
        Dim zt As Integer = -10
        Select Case ComboBox4.SelectedItem
            Case "待入库样品"
                zt = -2
            Case "库存样品"
                zt = 0
            Case "已处置样品"
                zt = 24
        End Select
        If zt <> -10 Then cmd.Parameters.AddWithValue("处理状态", zt)
        Mydgv1.load_data(cmd)
    End Sub

    Private Sub Form_留样管理_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim reader As SqlClient.SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select 名称 from 科室信息 where 管理标识='检验科室' order by 名称"
        reader = cmd.ExecuteReader
        ComboBox1.Items.Add("全部科室")
        Do While reader.Read
            ComboBox1.Items.Add(reader.Item(0).ToString)
        Loop
        reader.Close()
        con.Close()
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        ComboBox4.SelectedIndex = 0
        For i = 2014 To Year(Now())
            ComboBox5.Items.Add(i.ToString)
        Next
        ComboBox5.SelectedItem = Year(Now()).ToString
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim strary As New ArrayList
        strary.Add(Mydgv1.get_col("报告编号"))
        strary.Add(Mydgv1.get_col("备样量"))
        strary.Add(Mydgv1.get_col("样品名称"))
        strary.Add(Mydgv1.get_col("规格型号"))
        strary.Add(Mydgv1.get_col("检验类型"))
        strary.Add(Mydgv1.get_col("委托单位"))
        strary.Add(Mydgv1.get_col("生产单位"))
        strary.Add(Mydgv1.get_col("到样日期"))
        strary.Add(Mydgv1.get_col("判定"))
        If strary(0).count = 0 Then Exit Sub
        pb1.Maximum = strary(0).count - 1
        pb1.Minimum = 0
        pb1.Value = 0
        pb1.Visible = True
        Dim word As New Microsoft.Office.Interop.Word.Application
        Dim doc As Microsoft.Office.Interop.Word.Document
        doc = word.Documents.Add(Application.StartupPath + "\temp\留样处理记录.dot")
        word.Selection.GoTo(-1, , , "start")
        For i = 0 To strary(0).Count - 1
            If i > 0 Then word.Selection.MoveRight(12)
            word.Selection.TypeText((i + 1).ToString)
            word.Selection.MoveRight(12)
            word.Selection.TypeText(strary(0)(i))
            word.Selection.MoveRight(12)
            word.Selection.TypeText(strary(1)(i))
            word.Selection.MoveRight(12)
            word.Selection.TypeText(strary(2)(i))
            word.Selection.MoveRight(12)
            word.Selection.TypeText(strary(3)(i))
            word.Selection.MoveRight(12)
            word.Selection.TypeText(strary(4)(i))
            word.Selection.MoveRight(12)
            word.Selection.TypeText(strary(5)(i))
            word.Selection.MoveRight(12)
            word.Selection.TypeText(strary(6)(i))
            word.Selection.MoveRight(12)
            word.Selection.TypeText(strary(7)(i))
            word.Selection.MoveRight(12)
            word.Selection.TypeText(strary(8)(i))
            pb1.Value = i
        Next
        pb1.Visible = False
        word.Visible = True
        word.ShowMe()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim ypbhary As ArrayList
        ypbhary = Mydgv1.get_col("报告编号")
        If ypbhary.Count > 0 Then
            If MessageBox.Show("共计样品" + ypbhary.Count.ToString + "个,全部处置?", "处置样品", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim con As New SqlClient.SqlConnection
                Dim cmd As New SqlClient.SqlCommand
                con.ConnectionString = glb_sqlconstr
                cmd.Connection = con
                con.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "zyn_处置样品"
                cmd.Parameters.AddWithValue("报告编号", "")
                cmd.Parameters.AddWithValue("操作人", glb_姓名)
                pb1.Maximum = ypbhary.Count - 1
                pb1.Minimum = 0
                pb1.Value = 0
                pb1.Visible = True
                For i = 0 To ypbhary.Count - 1
                    cmd.Parameters("报告编号").Value = ypbhary(i)
                    cmd.ExecuteNonQuery()
                    pb1.Value = i
                Next
                con.Close()
                pb1.Visible = False
                Call Button1_Click(sender, e)
                MessageBox.Show("样品处置完成")
            End If
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged

    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Not Trim(TextBox1.Text) = "" Then
                Dim cmd As New SqlClient.SqlCommand
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "zyn_留样管理读取任务信息"
                cmd.Parameters.AddWithValue("报告编号", ComboBox5.SelectedItem + "%" + TextBox1.Text + "%")
                Mydgv1.load_data(cmd)
            End If
            TextBox1.Text = ""
        End If
    End Sub


End Class