<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class form_批量修改字段
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意:  以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.DGV1 = New System.Windows.Forms.DataGridView()
        Me.字段名称 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.长度 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.类型 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.修改 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        CType(Me.DGV1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(80, 491)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 27)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "确定"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(315, 489)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 29)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "取消"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'DGV1
        '
        Me.DGV1.AllowUserToAddRows = False
        Me.DGV1.AllowUserToDeleteRows = False
        Me.DGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.字段名称, Me.长度, Me.类型, Me.修改})
        Me.DGV1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
        Me.DGV1.Location = New System.Drawing.Point(2, 2)
        Me.DGV1.Name = "DGV1"
        Me.DGV1.RowTemplate.Height = 23
        Me.DGV1.Size = New System.Drawing.Size(467, 483)
        Me.DGV1.TabIndex = 4
        '
        '字段名称
        '
        Me.字段名称.HeaderText = "字段名称"
        Me.字段名称.Name = "字段名称"
        '
        '长度
        '
        Me.长度.HeaderText = "长度"
        Me.长度.Name = "长度"
        '
        '类型
        '
        Me.类型.HeaderText = "类型"
        Me.类型.Name = "类型"
        '
        '修改
        '
        Me.修改.HeaderText = "修改"
        Me.修改.Name = "修改"
        '
        'form_批量修改字段
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(473, 519)
        Me.Controls.Add(Me.DGV1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "form_批量修改字段"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "可修改字段设置"
        CType(Me.DGV1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents DGV1 As System.Windows.Forms.DataGridView
    Friend WithEvents 字段名称 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 长度 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 类型 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 修改 As System.Windows.Forms.DataGridViewCheckBoxColumn
End Class
