Public Class Form_批量判定
    Private Sub Form_批量判定_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvcol.Items.Clear()
        dgvcol.Items.Add("合格")
        dgvcol.Items.Add("不合格")
        dgvcol.Items.Add("/")
        dgvcol.Items.Add("问题项")
        dgvcol.Items.Add("符合")
        dgvcol.Items.Add("不符合")
        dgvcol.Items.Add(DBNull.Value)
        dgvcol.SelectedIndex = 0
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DialogResult = DialogResult.Cancel
    End Sub
End Class