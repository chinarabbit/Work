﻿Imports System.Data.SqlClient

Public Class dialog_标准筛选
    Public pubcon As New SqlConnection
    Public pubada As New SqlDataAdapter
    Public pubcmdbuilder As New SqlCommandBuilder
    Public pubtable As New DataTable
    Public pubbinds As New BindingSource
    Private Sub dialog_标准筛选_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pubbinds.RemoveFilter()
        TextBox1.Text = ""
        Label4.Text = ""
        pubcon.ConnectionString = glb_sqlconstr
        mycmd.Connection = pubcon
        pubada.SelectCommand = mycmd
        pubcmdbuilder.DataAdapter = pubada
        pubtable.Clear()
        pubada.Fill(pubtable)
        pubbinds.DataSource = pubtable
        dgv1.DataSource = pubbinds
        dgv1.Columns(0).Width = 900
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Text = dgv1.CurrentCell.Value
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DialogResult = DialogResult.Cancel
    End Sub



    Private Sub dialog_标准筛选_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        dgv1.Height = Me.Height - 112
        dgv1.Width = Me.Width - 16
        '  dgv1.Columns(0).Width = dgv1.Width - 10
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If TextBox1.Text <> "" Then
            Dim str As String = TextBox1.Text
            If e.KeyCode = Keys.Enter Then
                If Label4.Text = "" Then
                    pubbinds.Filter = "[检验依据] like '%" + str + "%'"
                    Label4.Text = "关键词:" + str
                Else
                    pubbinds.Filter += " and [检验依据] like '%" + str + "%'"
                    Label4.Text += "," + str
                End If
                TextBox1.Text = ""
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pubbinds.RemoveFilter()
        Label4.Text = ""
        TextBox1.Text = ""
    End Sub

    Private Sub dgv1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv1.CellContentClick

    End Sub

    Private Sub dgv1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv1.CellDoubleClick
        Button1_Click(sender, e)
    End Sub
End Class