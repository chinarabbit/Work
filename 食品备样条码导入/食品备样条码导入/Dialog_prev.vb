Imports System.Windows.Forms

Public Class Dialog_flowprev

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog_prev_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lab_curperson.Text = glb_姓名
        TextBox1.Text = "请修改:"
    End Sub
End Class
