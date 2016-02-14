Imports System.Windows.Forms

Public Class dialog_flownext

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dialog_flownext_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lab_curperson.Text = glb_姓名
        qfrq.Value = Now()
    End Sub
End Class
