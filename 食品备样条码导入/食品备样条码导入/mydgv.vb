Imports System.Data.SqlClient, Microsoft.Office.Interop, System.Reflection, System.ComponentModel
Public Class mydgv
    Dim SelectedCol As Integer = 0, IsFindit As Boolean = True
    Dim filterarray As New ArrayList
    Dim colarray As New ArrayList
    Dim findstrarray As New ArrayList
    Dim colorcell As New ArrayList
    Dim col As Integer = 0
    Dim selectcol As Integer = -1
    Dim selectrow As Integer = -1
    Dim totalnum, visiblenum, selectnum As Integer
    Dim findcol As Integer = -1
    Dim firstfilter As Integer = -1
    Dim findallstr As String = ""
    Dim myformname As String = ""
    Dim con As New SqlConnection
    Dim pubada As New SqlDataAdapter
    Dim pubcmdbuilder As New SqlCommandBuilder
    Dim pubtable As New DataTable
    Dim pubbinds As New BindingSource
    Dim status_hide As Boolean = False
    Dim dgvcol_change As Boolean = False
    Dim mouse_col As Integer = -1
    Dim mouse_row As Integer = -1

    Public Property form_name As String
        Get
            Return myformname
        End Get
        Set(value As String)
            myformname = value
        End Set
    End Property

    Public Property hide_status As Boolean
        Get
            Return status_hide
        End Get
        Set(value As Boolean)
            status_hide = value
        End Set
    End Property
    Public Property dgv_width As Double
        Get
            Return dgv.Width
        End Get
        Set(Value As Double)
            dgv.Width = Value
        End Set
    End Property
    Public Property dgv_height As Double
        Get
            Return dgv.Height
        End Get
        Set(Value As Double)
            dgv.Height = Value
        End Set
    End Property
    Public Property dgv_currentcell As System.Windows.Forms.DataGridViewCell
        Get
            Return dgv.CurrentCell
        End Get
        Set(value As System.Windows.Forms.DataGridViewCell)
            dgv.CurrentCell = value
        End Set
    End Property
    Public Property dgv_selectrows As System.Windows.Forms.DataGridViewSelectedRowCollection
        Get
            Return dgv.SelectedRows
        End Get
        Set(value As System.Windows.Forms.DataGridViewSelectedRowCollection)

        End Set
    End Property
    Public Property dgv_selectcols As System.Windows.Forms.DataGridViewSelectedColumnCollection
        Get
            Return dgv.SelectedColumns
        End Get
        Set(value As System.Windows.Forms.DataGridViewSelectedColumnCollection)

        End Set
    End Property
    Sub view_paint()
        Dim temp_flag As Boolean = dgvcol_change
        dgvcol_change = False
        If myformname = "" Then Exit Sub
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim ada As New SqlDataAdapter
        Dim bulider As New SqlCommandBuilder
        Dim table As New DataTable
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        cmd.CommandText = "select guid,隐藏,username,宽度,显示顺序,列名称,管理隐藏,表单名称,控件名称 from zyn_表格属性 where 表单名称='" + myformname + "' and ( username='" + glb_loginname + "' or username='sa') and 控件名称='" + Me.Name + "'  order by 显示顺序"
        ada.SelectCommand = cmd
        bulider.DataAdapter = ada
        ada.Fill(table)
        Dim row, row_sa As DataRow
        Dim rows(), rows_sa() As DataRow

        For Each col As DataGridViewColumn In dgv.Columns

            rows = table.Select("列名称='" + col.Name + "' and username='" + glb_loginname + "'")

            If rows.Count = 0 Then
                rows_sa = table.Select("列名称='" + col.Name + "' and username='sa'")
                If rows_sa.Count = 0 Then
                    row = table.NewRow()
                    row("guid") = Guid.NewGuid.ToString
                    row("username") = glb_loginname
                    row("宽度") = col.Width
                    row("显示顺序") = col.DisplayIndex
                    row("列名称") = col.Name
                    row("表单名称") = myformname
                    row("控件名称") = Me.Name
                    row("隐藏") = False
                    row("管理隐藏") = False
                    table.Rows.Add(row)
                Else
                    row_sa = rows_sa(0)
                    row = table.NewRow()
                    row("guid") = Guid.NewGuid.ToString
                    row("username") = glb_loginname
                    row("宽度") = row_sa("宽度")
                    row("显示顺序") = col.DisplayIndex
                    row("列名称") = col.Name
                    row("表单名称") = myformname
                    row("控件名称") = Me.Name
                    row("隐藏") = row_sa("隐藏")
                    row("管理隐藏") = row_sa("管理隐藏")
                    table.Rows.Add(row)
                End If
            Else
                row = rows(0)
            End If
            If row("隐藏") = True Or row("管理隐藏") = True Then
                col.Visible = False
            Else
                col.Visible = True
                If IsDBNull(row("宽度")) Then row("宽度") = col.Width Else col.Width = row("宽度")
                If IsDBNull(row("显示顺序")) Then
                    row("显示顺序") = col.DisplayIndex
                Else
                    If row("显示顺序") < dgv.ColumnCount Then col.DisplayIndex = row("显示顺序") Else col.DisplayIndex = dgv.ColumnCount - 1
                End If
            End If
        Next
        ada.Update(table)
        dgvcol_change = temp_flag
    End Sub
    Public Sub load_data(ByVal cmd As SqlCommand)
        dgvcol_change = False
        con.ConnectionString = glb_sqlconstr
        cmd.Connection = con
        pubada.SelectCommand = cmd
        pubcmdbuilder.DataAdapter = pubada
        pubtable.Clear()
        pubbinds.RemoveFilter()
        pubada.Fill(pubtable)
        pubbinds.DataSource = pubtable

        dgv.DataSource = pubbinds

        Call view_paint()

        dgvcol_change = True
    End Sub
    Public Sub dgv_selectrow(ByVal index As Integer, ByVal b As Boolean)
        Try
            dgv.Rows(index).Selected = b
        Catch ex As Exception

        End Try

    End Sub
    Public Sub dgv_selectcell(ByVal row As Integer, ByVal col As Integer)
        Try
            dgv.CurrentCell = dgv.Rows(row).Cells(col)
        Catch ex As Exception

        End Try
    End Sub

    Sub show_col(ByVal sender As Object, ByVal e As EventArgs)
        dgv.Columns(sender.text).visible = True
        Dim mycon As New SqlConnection
        mycon.ConnectionString = glb_sqlconstr
        mycon.Open()
        Dim mycmd As New SqlCommand
        mycmd.Connection = mycon
        mycmd.CommandType = CommandType.StoredProcedure
        mycmd.CommandText = "zyn_写表格列属性"
        mycmd.Parameters.AddWithValue("username", glb_loginname)
        mycmd.Parameters.AddWithValue("表单名称", myformname)
        mycmd.Parameters.AddWithValue("列名称", sender.text)
        mycmd.Parameters.AddWithValue("隐藏", 0)
        mycmd.Parameters.AddWithValue("控件名称", Me.Name)
        'mycmd.Parameters.AddWithValue("宽度", sender.width)
        'mycmd.Parameters.AddWithValue("显示顺序", sender.displayindex)
        mycmd.ExecuteNonQuery()
        mycon.Close()
        sender.visible = False
    End Sub




    Private Sub dgv_ColumnDisplayIndexChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgv.ColumnDisplayIndexChanged
        If dgvcol_change Then
            Dim mycon As New SqlConnection
            mycon.ConnectionString = glb_sqlconstr
            mycon.Open()
            Dim mycmd As New SqlCommand
            mycmd.Connection = mycon
            mycmd.CommandType = CommandType.StoredProcedure
            mycmd.CommandText = "zyn_写表格列属性"
            mycmd.Parameters.AddWithValue("username", glb_loginname)
            mycmd.Parameters.AddWithValue("表单名称", myformname)
            mycmd.Parameters.AddWithValue("列名称", e.Column.Name)
            mycmd.Parameters.AddWithValue("显示顺序", e.Column.DisplayIndex)
            mycmd.Parameters.AddWithValue("控件名称", Me.Name)
            ' mycmd.Parameters.AddWithValue("宽度", e.Column.Width)
            mycmd.ExecuteNonQuery()
            mycon.Close()
        End If

    End Sub

    Private Sub dgv_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgv.ColumnWidthChanged
        If dgvcol_change Then
            ' Call update_colprop(e.Column.Name, e.Column.Width, e.Column.DisplayIndex, Not (e.Column.Visible))
            Dim mycon As New SqlConnection
            mycon.ConnectionString = glb_sqlconstr
            mycon.Open()
            Dim mycmd As New SqlCommand
            mycmd.Connection = mycon
            mycmd.CommandType = CommandType.StoredProcedure
            mycmd.CommandText = "zyn_写表格列属性"
            mycmd.Parameters.AddWithValue("username", glb_loginname)
            mycmd.Parameters.AddWithValue("表单名称", myformname)
            mycmd.Parameters.AddWithValue("列名称", e.Column.Name)
            mycmd.Parameters.AddWithValue("宽度", e.Column.Width)
            mycmd.Parameters.AddWithValue("控件名称", Me.Name)
            'mycmd.Parameters.AddWithValue("显示顺序", e.Column.DisplayIndex)
            mycmd.ExecuteNonQuery()
            mycon.Close()
        End If

    End Sub



    Private Sub dgv_MouseDown(sender As Object, e As MouseEventArgs) Handles dgv.MouseDown
        findcol = dgv.HitTest(e.X, e.Y).ColumnIndex
        mouse_col = dgv.HitTest(e.X, e.Y).ColumnIndex
        mouse_row = dgv.HitTest(e.X, e.Y).RowIndex

        If e.Button = Windows.Forms.MouseButtons.Right Then
            If mouse_col > -1 Then
                dgvmenu.Items("模糊筛选").Visible = True
                dgvmenu.Items("筛选").Visible = True
                dgvmenu.Items("排除筛选").Visible = True
            Else
                dgvmenu.Items("筛选").Visible = False
                dgvmenu.Items("排除筛选").Visible = False
                dgvmenu.Items("模糊筛选").Visible = False

            End If

        End If
    End Sub
    Sub temp()
        Me.UseWaitCursor = True
        Me.Cursor = Cursors.WaitCursor
        Dim pgb As New ProgressBar
        Dim App As New Excel.Application
        Dim Book As Excel.Workbook
        Dim sheet As Excel.Worksheet
        Book = App.Workbooks.Add
        sheet = Book.Sheets(1)
        App.ScreenUpdating = False
        pgb.Minimum = 0
        pgb.Maximum = dgv.Rows.Count - 1
        pgb.Value = 0
        pgb.Width = dgv.Width / 2
        pgb.Left = dgv.Width / 4
        pgb.Top = dgv.Height / 2 - 15
        pgb.Visible = True
        dgv.Controls.Add(pgb)
        Dim colnum, rownum, num As Integer

        sheet.Cells(1, 1) = "序号"
        colnum = 1
        rownum = 1
        num = 0
        For i = 0 To dgv.Columns.Count - 1
            If dgv.Columns(i).Visible = True Then
                colnum += 1
                sheet.Cells(rownum, colnum) = dgv.Columns(i).Name
            End If
        Next

        For i = 0 To dgv.Rows.Count - 1
            colnum = 1
            If dgv.Rows(i).Visible = True Then
                num += 1
                rownum += 1
                sheet.Cells(rownum, colnum) = num
                For j = 0 To dgv.Columns.Count - 1
                    If dgv.Columns(j).Visible = True Then
                        colnum += 1
                        sheet.Cells(rownum, colnum) = dgv.Rows(i).Cells(j).Value
                    End If
                Next
            End If
            pgb.Value = i
        Next
        pgb.Dispose()
        App.ScreenUpdating = True
        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
        App.Visible = True
    End Sub
    Private Sub 导出所有记录_Click(sender As Object, e As EventArgs) Handles 导出所有记录.Click
        Me.UseWaitCursor = True
        Me.Cursor = Cursors.WaitCursor
        'Dim pgb As New ProgressBar
        'pgb.Style = ProgressBarStyle.Marquee
        'pgb.MarqueeAnimationSpeed = 100
        'pgb.Width = dgv.Width / 2
        'pgb.Left = dgv.Width / 4
        'pgb.Top = dgv.Height / 2 - 15
        'pgb.Visible = True
        'dgv.Controls.Add(pgb)

        Dim App As New Excel.Application
        Dim Book As Excel.Workbook
        Dim sheet As Excel.Worksheet
        Book = App.Workbooks.Add(Application.StartupPath + "\temp\" + "报表导出.xltm")
        sheet = Book.Sheets(1)

        Dim colnum, rownum As Integer
        Dim dataarray(,) As String
        colnum = 0
        rownum = 0
        ReDim dataarray(dgv.ColumnCount, dgv.RowCount)
        dataarray(0, 0) = "序号"
        colnum += 1
        For Each col As DataGridViewColumn In dgv.Columns
            rownum = 0
            If col.Visible Then
                dataarray(colnum, rownum) = col.HeaderText
                rownum += 1
                For Each row As DataGridViewRow In dgv.Rows
                    If row.Visible Then
                        dataarray(colnum, rownum) = row.Cells(col.Index).Value.ToString
                        If colnum = 1 Then dataarray(0, rownum) = rownum.ToString
                        rownum += 1
                    End If
                Next
                colnum += 1
            End If
            ' pgb.Value += 1
        Next
        '  colnum = dgv.ColumnCount + 1
        ' rownum = dgv.RowCount + 1
        App.Run("infowrite", dataarray, rownum, colnum)
        '  pgb.Dispose()
        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
        App.Visible = True
    End Sub


    Private Sub 导出选中记录_Click(sender As Object, e As EventArgs) Handles 导出选中记录.Click
        If dgv.SelectedRows.Count = 0 Then
            MessageBox.Show("没有被选中的记录")
            Exit Sub
        End If
        Me.UseWaitCursor = True
        Me.Cursor = Cursors.WaitCursor
        'Dim pgb As New ProgressBar
        'pgb.Style = ProgressBarStyle.Marquee
        'pgb.MarqueeAnimationSpeed = 100
        'pgb.Width = dgv.Width / 2
        'pgb.Left = dgv.Width / 4
        'pgb.Top = dgv.Height / 2 - 15
        'pgb.Visible = True
        'dgv.Controls.Add(pgb)

        Dim App As New Excel.Application
        Dim Book As Excel.Workbook
        Dim sheet As Excel.Worksheet
        Book = App.Workbooks.Add(Application.StartupPath + "\temp\" + "报表导出.xltm")
        sheet = Book.Sheets(1)

        Dim colnum, rownum As Integer
        Dim dataarray(,) As String
        colnum = 0
        rownum = 0
        ReDim dataarray(dgv.ColumnCount, dgv.RowCount)
        dataarray(0, 0) = "序号"
        colnum += 1
        For Each col As DataGridViewColumn In dgv.Columns
            rownum = 0
            If col.Visible Then
                dataarray(colnum, rownum) = col.HeaderText
                rownum += 1
                For Each row As DataGridViewRow In dgv.SelectedRows
                    If row.Visible Then
                        dataarray(colnum, rownum) = row.Cells(col.Index).Value.ToString
                        If colnum = 1 Then dataarray(0, rownum) = rownum.ToString
                        rownum += 1
                    End If
                Next
                colnum += 1
            End If
            ' pgb.Value += 1
        Next
        '  colnum = dgv.ColumnCount + 1
        ' rownum = dgv.RowCount + 1
        App.Run("infowrite", dataarray, rownum, colnum)
        '  pgb.Dispose()
        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
        App.Visible = True
    End Sub

    Public Function selected_rows() As DataGridViewSelectedRowCollection
        Return dgv.SelectedRows
    End Function

    Public Function get_selectrows() As ArrayList
        Dim rowarray As New ArrayList
        If dgv.Rows.Count > 0 Then
            For i = 0 To dgv.RowCount - 1
                If dgv.Rows(i).Visible And dgv.Rows(i).Selected Then rowarray.Add(i)
            Next
            If rowarray.Count = 0 Then rowarray.Add(dgv.CurrentCell.RowIndex)
        End If
        Return rowarray
    End Function
    Public Function get_allrows() As ArrayList
        '取得所有可见行
        Dim rowarray As New ArrayList
        For i = 0 To dgv.RowCount - 1
            If dgv.Rows(i).Visible Then rowarray.Add(i)
        Next
        Return rowarray
    End Function
    Public Function getselect_col(colname As String) As ArrayList
        Dim newarray As New ArrayList
        If Not dgv.Columns(colname) Is Nothing Then
            For i = 0 To dgv.SelectedRows.Count - 1
                If dgv.SelectedRows(i).Visible = True Then newarray.Add(dgv.SelectedRows(i).Cells(colname).Value)
            Next
        End If
        Return newarray
    End Function
    Public Function get_col(colname As String) As ArrayList
        Dim newarray As New ArrayList
        If Not dgv.Columns(colname) Is Nothing Then
            For i = 0 To dgv.Rows.Count - 1
                If dgv.Rows(i).Visible = True Then newarray.Add(dgv.Rows(i).Cells(colname).Value)
            Next
        End If
        Return newarray
    End Function
    Public Function get_colindex(colname As String) As Integer
        Return dgv.Columns(colname).Index
    End Function
    Public Function get_rownum() As Integer
        Return dgv.RowCount
    End Function
    Public Function get_currentrow() As Integer
        If dgv.CurrentRow Is Nothing Then Return -1 Else Return dgv.CurrentRow.Index
    End Function
    Public Function get_currentcol() As Integer
        Return dgv.CurrentCell.ColumnIndex
    End Function
    Public Function get_currentcell() As DataGridViewCell
        Return dgv.CurrentCell
    End Function
    Public Function get_cellvalue(ByVal row As Integer, ByVal colname As String) As String
        If row > -1 And Not (dgv.Columns(colname) Is Nothing) Then
            Return dgv.Rows(row).Cells(colname).Value.ToString
        Else
            Return ""
        End If
    End Function
    Public Sub set_cellvalue(ByVal row As Integer, ByVal colname As String, ByVal newvalue As String)
        dgv.Rows(row).Cells(colname).Value = newvalue
    End Sub
    Public Sub set_rowcolor(ByVal row As Integer, ByVal color As System.Drawing.Color)
        dgv.Rows(row).DefaultCellStyle.BackColor = color
    End Sub
    Public Function getcol_name() As ArrayList
        Dim newarray As New ArrayList
        '返回所有未隐藏的列名
        For i = 0 To dgv.Columns.Count - 1
            If dgv.Columns(i).Visible = True Then newarray.Add(dgv.Columns(i).Name)
        Next
        Return newarray
    End Function
    Public Function get_rowvisible(ByVal row As Integer) As Boolean
        Return dgv.Rows(row).Visible
    End Function
    Public Sub set_rowvisible(ByVal row As Integer, ByVal boo As Boolean)
        dgv.Rows(row).Visible = boo
    End Sub

    Public Sub display_col(ByRef colname As String)
        '显示指定列名的列,如果有选中行,只是显示列,没有选中行,定位到第一个可见cell
        If colname <> "" Then
            If Not (dgv.Columns(colname) Is Nothing) Then
                If dgv.Columns(colname).Visible = True Then
                    If dgv.SelectedRows.Count > 0 Then
                        For i = 0 To dgv.SelectedRows.Count - 1
                            dgv.FirstDisplayedScrollingColumnIndex = dgv.Columns(colname).Index
                        Next
                    Else
                        For i = 0 To dgv.Rows.Count - 1
                            If dgv.Rows(i).Visible = True Then
                                dgv.CurrentCell = dgv.Rows(i).Cells(colname)
                                dgv.Columns(colname).Selected = True
                                Exit Sub
                            End If
                        Next
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub clear_allselect()
        dgv.ClearSelection()
    End Sub
    Public Sub select_row(ByVal str As String, Optional ByVal colname As String = "", Optional ByVal selectall As Boolean = False)
        '查找到对应值的行并选中,只处理未隐藏行,colname是可选列名称,selectall指示是否选中所有命中行
        Dim firstdisplay As Boolean = True
        For i = 0 To dgv.Rows.Count - 1
            If dgv.Rows(i).Visible = False Then Continue For
            If colname = "" Then
                For j = 0 To dgv.Columns.Count - 1
                    If dgv.Rows(i).Cells(j).Value = str Then
                        If firstdisplay Then
                            dgv.FirstDisplayedScrollingRowIndex = i
                            firstdisplay = False
                        End If
                        dgv.Rows(i).Selected = True
                        If Not selectall Then Exit Sub
                        Exit For
                    End If
                Next
            Else
                If dgv.Rows(i).Cells(colname).Value = str Then
                    If firstdisplay Then
                        dgv.FirstDisplayedScrollingRowIndex = i
                        firstdisplay = False
                    End If
                    dgv.Rows(i).Selected = True
                    If Not selectall Then Exit Sub
                End If
            End If
        Next
    End Sub
    Public Sub select_cell(ByVal num As Integer, colname As String)
        '选中单元格
        If num >= dgv.RowCount Then Exit Sub
        If dgv.Rows(num).Cells(colname) IsNot Nothing Then
            If dgv.Rows(num).Cells(colname).Visible Then dgv.CurrentCell = dgv.Rows(num).Cells(colname)
        End If

    End Sub
    Public Sub add_row(ByRef colname As ArrayList, colvalue As ArrayList)
        For i = 0 To colname.Count - 1
            If dgv.Columns(colname(i)) Is Nothing Then dgv.Columns.Add(colname(i), colname(i))
        Next

        Dim index As Integer
        index = dgv.Rows.Add()
        For i = 0 To colname.Count - 1
            dgv.Rows(index).Cells(colname(i)).Value = colvalue(i)
        Next
        totalnum += 1
        visiblenum += 1
        Label1.Text = "共计:" + totalnum.ToString + ",显示:" + visiblenum.ToString
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Not (dgv.Columns("报告编号") Is Nothing) Then
                For i = 0 To dgv.Rows.Count - 1
                    If InStr(dgv.Rows(i).Cells(dgv.Columns("报告编号").Index).Value, TextBox1.Text) > 0 And dgv.Rows(i).Visible = True Then
                        dgv.CurrentCell = dgv.Rows(i).Cells(dgv.Columns("报告编号").Index)
                        Exit For
                    End If
                Next
            End If
            TextBox1.Text = ""
        End If

    End Sub


    Public Event cells_selected(ByVal sender As System.Object, ByVal e As System.EventArgs)

    Private Sub dgv_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellContentClick

    End Sub

    Private Sub dgv_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellClick
        RaiseEvent cells_selected(Me, e)
    End Sub

    Private Sub mydgv_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If status_hide Then
            Label1.Visible = False
            Label2.Visible = False
            TextBox1.Visible = False
            flp1.Visible = False
        End If
    End Sub

    Public Event cells_doubleclick(ByVal sender As System.Object, ByVal e As System.EventArgs)

    Private Sub dgv_cellsdoubleclick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellDoubleClick
        RaiseEvent cells_doubleclick(Me, e)
    End Sub

    Public Event currentcell_changed(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Private Sub dgv_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgv.CurrentCellChanged
        RaiseEvent currentcell_changed(Me, e)
    End Sub

    Private Sub mydgv_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        dgv.Width = Me.Width
        If status_hide Then dgv.Height = Me.Height Else dgv.Height = Me.Height - 24
        flp1.Width = Me.Width - 178

    End Sub

    Private Sub 模糊筛选_Click(sender As Object, e As EventArgs) Handles 模糊筛选.Click
        If dgv.Columns(mouse_col).ValueType = GetType(String) Then
            If mouse_col < 0 Then Exit Sub
            Dim mousestr As String = ""
            If mouse_row > -1 Then mousestr = dgv.Rows(mouse_row).Cells(mouse_col).Value.ToString
            Dim findstr As String = InputBox("请输入模糊筛选内容:", "请输入", mousestr)
            If findstr = "" Then Exit Sub
            If pubbinds.Filter Is Nothing Then
                pubbinds.Filter = dgv.Columns(mouse_col).Name + " like '%" + findstr + "%'"
            Else
                pubbinds.Filter += " and " + dgv.Columns(mouse_col).Name + " like '%" + findstr + "%'"
            End If
        End If
    End Sub

    Private Sub 筛选_Click(sender As Object, e As EventArgs) Handles 筛选.Click
        If mouse_col < 0 Then Exit Sub
        If dgv.Columns(mouse_col).ValueType = GetType(System.String) Then
            Dim mousestr As String = ""
            If mouse_row > -1 Then mousestr = dgv.Rows(mouse_row).Cells(mouse_col).Value.ToString
            Dim findstr As String = InputBox("请输入筛选内容:", "请输入", mousestr)
            If findstr = "" Then Exit Sub
            If pubbinds.Filter Is Nothing Then
                pubbinds.Filter = dgv.Columns(mouse_col).Name + "='" + findstr + "'"
            Else
                pubbinds.Filter += " and " + dgv.Columns(mouse_col).Name + "='" + findstr + "'"
            End If


        ElseIf dgv.Columns(mouse_col).ValueType = GetType(System.DateTime) Then
            Dialog_日期筛选.lab_zhiduan.Text = dgv.Columns(mouse_col).Name.ToString
            Dim dat_0 As DateTime
            If dgv.Rows(mouse_row).Cells(mouse_col).Value IsNot DBNull.Value Then dat_0 = dgv.Rows(mouse_row).Cells(mouse_col).Value Else dat_0 = Now()
            If mouse_row > -1 Then
                Dialog_日期筛选.dat_1.Value = dat_0
                Dialog_日期筛选.dat_2.Value = dat_0
            Else
                Dialog_日期筛选.dat_1.Value = Now().Date
                Dialog_日期筛选.dat_2.Value = Now().Date
            End If

            If Dialog_日期筛选.ShowDialog = DialogResult.OK Then
                Dim dat_1 As DateTime = Dialog_日期筛选.dat_1.Value
                Dim dat_2 As DateTime = Dialog_日期筛选.dat_2.Value.AddDays(1)
                If pubbinds.Filter Is Nothing Then
                    pubbinds.Filter = dgv.Columns(mouse_col).Name + ">='" + dat_1.Date + "'"
                    pubbinds.Filter += " and " + dgv.Columns(mouse_col).Name + "<='" + dat_2.Date + "'"
                Else
                    pubbinds.Filter += " and " + dgv.Columns(mouse_col).Name + ">='" + dat_1.Date + "'"
                    pubbinds.Filter += " and " + dgv.Columns(mouse_col).Name + "<='" + dat_2.Date + "'"
                End If
            End If

        ElseIf dgv.Columns(mouse_col).ValueType = GetType(System.Boolean) Then
            Dialog_是否筛选.lab_zhiduan.Text = dgv.Columns(mouse_col).Name.ToString
            Dialog_是否筛选.Radio1.Checked = True
            If Dialog_是否筛选.ShowDialog = DialogResult.OK Then
                Dim fil_booleen As Boolean
                If Dialog_是否筛选.Radio1.Checked Then fil_booleen = True Else fil_booleen = False
                If pubbinds.Filter Is Nothing Then
                    pubbinds.Filter = dgv.Columns(mouse_col).Name + "=" + fil_booleen.ToString
                Else
                    pubbinds.Filter += " and " + dgv.Columns(mouse_col).Name + "=" + fil_booleen.ToString
                End If
            End If
        End If

    End Sub

    Private Sub 排除筛选_Click(sender As Object, e As EventArgs) Handles 排除筛选.Click
        If dgv.Columns(mouse_col).ValueType = GetType(String) Then
            If mouse_col < 0 Then Exit Sub
            Dim mousestr As String = ""
            If mouse_row > -1 Then mousestr = dgv.Rows(mouse_row).Cells(mouse_col).Value.ToString
            Dim findstr As String = InputBox("请输入排除筛选内容:", "请输入", mousestr)
            If findstr = "" Then Exit Sub
            If pubbinds.Filter Is Nothing Then
                pubbinds.Filter = dgv.Columns(mouse_col).Name + "<>'" + findstr + "'"

            Else
                pubbinds.Filter += " and " + dgv.Columns(mouse_col).Name + "<>'" + findstr + "'"
            End If
        End If
    End Sub

    Private Sub 清除筛选条件_Click(sender As Object, e As EventArgs) Handles 清除筛选条件.Click
        pubbinds.RemoveFilter()

    End Sub

    Private Sub dgv_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgv.DataBindingComplete
        Label1.Text = dgv.Rows.Count.ToString + "条记录"
    End Sub

    Private Sub 自定义视图_Click(sender As Object, e As EventArgs) Handles 自定义视图.Click
        form_自定义列.formname = myformname
        form_自定义列.controlname = Me.Name
        If form_自定义列.ShowDialog() = DialogResult.Yes Then
            Call view_paint()
        End If
    End Sub

End Class
