Public Class Form_模板设置
    Dim guidstr As String = ""
    Private Sub Form_模板设置_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call load_dgv()
        ComboBox2.Items.Clear()
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim reader As SqlClient.SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "SELECT [名称]  FROM [KingsLims].[dbo].[lookup]  where type='监督检验' or type='委托检验'"
        reader = cmd.ExecuteReader
        Do While reader.Read
            ComboBox2.Items.Add(reader.Item(0).ToString)
        Loop
        reader.Close()
        con.Close()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv1.CellClick
        If e.RowIndex <> -1 Then
            guidstr = dgv1.Rows(e.RowIndex).Cells("guid").Value
            ComboBox1.SelectedItem = dgv1.Rows(e.RowIndex).Cells("表单名称").Value.ToString
            TextBox1.Text = dgv1.Rows(e.RowIndex).Cells("文件名称").Value
            ComboBox2.SelectedItem = dgv1.Rows(e.RowIndex).Cells("检验类型").Value.ToString
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        guidstr = ""
        TextBox1.Text = ""
        TextBox2.Text = ""
        dgv1.ClearSelection()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()
        Me.TextBox2.Text = OpenFileDialog1.FileName
        TextBox1.Text = OpenFileDialog1.SafeFileName
    End Sub

    Sub load_dgv()
        Dim i As Integer = 0
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim reader As SqlClient.SqlDataReader
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "select guid,表单名称,文件名称,检验类型,文件内容 from zyn_报表模板 "
        reader = cmd.ExecuteReader
        ComboBox1.Items.Clear()
        dgv1.Rows.Clear()
        Do While reader.Read
            i = dgv1.Rows.Add()
            dgv1.Rows(i).Cells("guid").Value = reader.Item(0)
            dgv1.Rows(i).Cells("表单名称").Value = reader.Item(1)
            dgv1.Rows(i).Cells("文件名称").Value = reader.Item(2)
            dgv1.Rows(i).Cells("检验类型").Value = reader.Item(3)
            If IsDBNull(reader.Item("文件内容")) Then dgv1.Rows(i).Cells("文件内容").Value = "无" Else dgv1.Rows(i).Cells("文件内容").Value = reader.Item(2)
            ComboBox1.Items.Add(reader.Item(1).ToString)
        Loop
        reader.Close()
        con.Close()
        dgv1.ClearSelection()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim form, filename, jylx As String
        If ComboBox1.SelectedItem <> "" Then form = ComboBox1.SelectedItem Else form = ComboBox1.Text
        filename = TextBox1.Text
        If ComboBox2.SelectedIndex < 0 Then jylx = "" Else jylx = ComboBox2.SelectedItem
        Dim buffer As Byte()
        Dim fs As New System.IO.FileStream(TextBox2.Text, IO.FileMode.Open)
        Dim br As New System.IO.BinaryReader(fs)
        buffer = br.ReadBytes(fs.Length)
        br.Close()
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.Parameters.AddWithValue("form", form)
        cmd.Parameters.AddWithValue("filename", filename)
        cmd.Parameters.AddWithValue("file", buffer)
        cmd.Parameters.AddWithValue("jylx", jylx)
        If guidstr = "" Then
            cmd.CommandText = "insert into zyn_报表模板 (表单名称,文件名称,文件内容,检验类型) values (@form,@filename,@file,@jylx)"
        Else
            cmd.CommandText = "update zyn_报表模板 set 表单名称=@form,文件名称=@filename,文件内容=@file,检验类型=@jylx where guid='" + guidstr + "'"
        End If
        cmd.ExecuteNonQuery()

        If System.IO.Directory.Exists(Application.StartupPath + "\temp") Then System.IO.Directory.Delete(Application.StartupPath + "\temp", True)
        System.IO.Directory.CreateDirectory(Application.StartupPath + "\temp")
        Dim reader As SqlClient.SqlDataReader
        cmd.CommandText = "select 文件名称,文件内容 from zyn_报表模板 where 文件内容 is not null "
        reader = cmd.ExecuteReader
        Do While reader.Read
            System.IO.File.WriteAllBytes(Application.StartupPath + "\temp\" + reader.Item(0).ToString, reader.Item(1))
        Loop

        con.Close()
        Call load_dgv()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If guidstr <> "" Then
            If MessageBox.Show("删除该记录?", "删除提示", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim con As New SqlClient.SqlConnection
                Dim cmd As New SqlClient.SqlCommand
                con.ConnectionString = glb_sqlconstr
                cmd.Connection = con
                con.Open()
                cmd.CommandText = "delete from zyn_报表模板 where guid='" + guidstr + "'"
                cmd.ExecuteNonQuery()
                con.Close()
                Call load_dgv()
            End If
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim path As String = ""
        If guidstr <> "" Then
            Try
                Dim con As New SqlClient.SqlConnection
                Dim cmd As New SqlClient.SqlCommand
                Dim reader As SqlClient.SqlDataReader
                con.ConnectionString = glb_sqlconstr
                cmd.Connection = con
                con.Open()
                cmd.CommandText = "select 文件名称,文件内容 from zyn_报表模板 where 文件内容 is not null and  guid='" + guidstr + "'"
                reader = cmd.ExecuteReader
                If reader.Read Then
                    FolderBrowserDialog1.ShowDialog()
                    path = FolderBrowserDialog1.SelectedPath
                    System.IO.File.WriteAllBytes(FolderBrowserDialog1.SelectedPath + "\" + reader.Item(0).ToString, reader.Item(1))
                End If
                reader.Close()
                con.Close()
            Catch ex As Exception

            End Try

        End If

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim form, filename, jylx As String
        If ComboBox1.SelectedItem <> "" Then form = ComboBox1.SelectedItem Else form = ComboBox1.Text
        filename = TextBox1.Text
        jylx = ComboBox2.SelectedItem

        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        con.Open()
        cmd.Parameters.AddWithValue("form", form)
        cmd.Parameters.AddWithValue("filename", filename)
        cmd.Parameters.AddWithValue("jylx", jylx)
        If guidstr = "" Then
            cmd.CommandText = "insert into zyn_报表模板 (表单名称,文件名称,检验类型) values (@form,@filename,@jylx)"
        Else
            cmd.CommandText = "update zyn_报表模板 set 表单名称=@form,文件名称=@filename,检验类型=@jylx where guid='" + guidstr + "'"
        End If
        cmd.ExecuteNonQuery()

        con.Close()
        Call load_dgv()
    End Sub
End Class