<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class form_缴费单管理
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

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DGV2 = New System.Windows.Forms.DataGridView()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgv1 = New System.Windows.Forms.DataGridView()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.pguid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.缴费单编号 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.guid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.报告编号 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.样品名称 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.检验费 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.收费情况 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.DGV2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgv1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DGV2
        '
        Me.DGV2.AllowUserToAddRows = False
        Me.DGV2.AllowUserToDeleteRows = False
        Me.DGV2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.guid, Me.报告编号, Me.样品名称, Me.检验费, Me.收费情况})
        Me.DGV2.Location = New System.Drawing.Point(208, 56)
        Me.DGV2.Name = "DGV2"
        Me.DGV2.ReadOnly = True
        Me.DGV2.RowHeadersWidth = 30
        Me.DGV2.RowTemplate.Height = 23
        Me.DGV2.Size = New System.Drawing.Size(676, 509)
        Me.DGV2.TabIndex = 1
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"未缴费", "已缴费"})
        Me.ComboBox1.Location = New System.Drawing.Point(13, 13)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(123, 24)
        Me.ComboBox1.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(169, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 24)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "刷新"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(280, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Label1"
        '
        'dgv1
        '
        Me.dgv1.AllowUserToAddRows = False
        Me.dgv1.AllowUserToDeleteRows = False
        Me.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.pguid, Me.缴费单编号})
        Me.dgv1.Location = New System.Drawing.Point(13, 56)
        Me.dgv1.MultiSelect = False
        Me.dgv1.Name = "dgv1"
        Me.dgv1.ReadOnly = True
        Me.dgv1.RowHeadersWidth = 15
        Me.dgv1.RowTemplate.Height = 23
        Me.dgv1.Size = New System.Drawing.Size(140, 511)
        Me.dgv1.TabIndex = 5
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(757, 16)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(127, 34)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "删除选中报告"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(626, 16)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(108, 34)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "打印缴费单"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'pguid
        '
        Me.pguid.HeaderText = "pguid"
        Me.pguid.Name = "pguid"
        Me.pguid.ReadOnly = True
        Me.pguid.Visible = False
        '
        '缴费单编号
        '
        Me.缴费单编号.HeaderText = "缴费单编号"
        Me.缴费单编号.Name = "缴费单编号"
        Me.缴费单编号.ReadOnly = True
        Me.缴费单编号.Width = 120
        '
        'guid
        '
        Me.guid.HeaderText = "guid"
        Me.guid.Name = "guid"
        Me.guid.ReadOnly = True
        Me.guid.Visible = False
        '
        '报告编号
        '
        Me.报告编号.HeaderText = "报告编号"
        Me.报告编号.Name = "报告编号"
        Me.报告编号.ReadOnly = True
        Me.报告编号.Width = 150
        '
        '样品名称
        '
        Me.样品名称.HeaderText = "样品名称"
        Me.样品名称.Name = "样品名称"
        Me.样品名称.ReadOnly = True
        Me.样品名称.Width = 300
        '
        '检验费
        '
        Me.检验费.HeaderText = "应收费"
        Me.检验费.Name = "检验费"
        Me.检验费.ReadOnly = True
        '
        '收费情况
        '
        Me.收费情况.HeaderText = "收费情况"
        Me.收费情况.Name = "收费情况"
        Me.收费情况.ReadOnly = True
        '
        'form_缴费单管理
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(929, 577)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.dgv1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.DGV2)
        Me.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "form_缴费单管理"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "缴费单管理"
        CType(Me.DGV2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgv1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DGV2 As DataGridView
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents dgv1 As DataGridView
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents pguid As DataGridViewTextBoxColumn
    Friend WithEvents 缴费单编号 As DataGridViewTextBoxColumn
    Friend WithEvents guid As DataGridViewTextBoxColumn
    Friend WithEvents 报告编号 As DataGridViewTextBoxColumn
    Friend WithEvents 样品名称 As DataGridViewTextBoxColumn
    Friend WithEvents 检验费 As DataGridViewTextBoxColumn
    Friend WithEvents 收费情况 As DataGridViewTextBoxColumn
End Class
