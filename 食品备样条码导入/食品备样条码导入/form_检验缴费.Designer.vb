<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class form_检验缴费
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
        Me.DGV1 = New System.Windows.Forms.DataGridView()
        Me.guid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.缴费单编号 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.dgv2 = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rguid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.报告编号 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.样品名称 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.检验费 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.主检科室 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.缴费日期 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.缴费金额 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.凭证号码 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.备注 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.经手人 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.DGV1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgv2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'DGV1
        '
        Me.DGV1.AllowUserToAddRows = False
        Me.DGV1.AllowUserToDeleteRows = False
        Me.DGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.guid, Me.缴费单编号})
        Me.DGV1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DGV1.Location = New System.Drawing.Point(12, 54)
        Me.DGV1.MultiSelect = False
        Me.DGV1.Name = "DGV1"
        Me.DGV1.ReadOnly = True
        Me.DGV1.RowHeadersVisible = False
        Me.DGV1.RowHeadersWidth = 20
        Me.DGV1.RowTemplate.Height = 23
        Me.DGV1.Size = New System.Drawing.Size(160, 454)
        Me.DGV1.TabIndex = 0
        '
        'guid
        '
        Me.guid.HeaderText = "guid"
        Me.guid.Name = "guid"
        Me.guid.ReadOnly = True
        Me.guid.Visible = False
        '
        '缴费单编号
        '
        Me.缴费单编号.HeaderText = "缴费单编号"
        Me.缴费单编号.Name = "缴费单编号"
        Me.缴费单编号.ReadOnly = True
        Me.缴费单编号.Width = 160
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(228, 5)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(65, 29)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "刷新"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'dgv2
        '
        Me.dgv2.AllowUserToAddRows = False
        Me.dgv2.AllowUserToDeleteRows = False
        Me.dgv2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.rguid, Me.报告编号, Me.样品名称, Me.检验费, Me.主检科室, Me.缴费日期, Me.缴费金额, Me.凭证号码, Me.备注, Me.经手人})
        Me.dgv2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv2.Location = New System.Drawing.Point(178, 310)
        Me.dgv2.Name = "dgv2"
        Me.dgv2.ReadOnly = True
        Me.dgv2.RowHeadersVisible = False
        Me.dgv2.RowTemplate.Height = 23
        Me.dgv2.Size = New System.Drawing.Size(749, 198)
        Me.dgv2.TabIndex = 4
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.TextBox2)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.TextBox5)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.TextBox4)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.ComboBox2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(178, 46)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(749, 258)
        Me.Panel1.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(271, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(0, 16)
        Me.Label6.TabIndex = 25
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.TextBox2.ForeColor = System.Drawing.Color.Red
        Me.TextBox2.Location = New System.Drawing.Point(101, 91)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(128, 26)
        Me.TextBox2.TabIndex = 24
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Red
        Me.Label13.Location = New System.Drawing.Point(337, 94)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(71, 16)
        Me.Label13.TabIndex = 23
        Me.Label13.Text = "Label13"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(503, 8)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(87, 35)
        Me.Button2.TabIndex = 21
        Me.Button2.Text = "确定收费"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(101, 186)
        Me.TextBox5.Multiline = True
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox5.Size = New System.Drawing.Size(341, 58)
        Me.TextBox5.TabIndex = 20
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(18, 189)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(80, 16)
        Me.Label11.TabIndex = 19
        Me.Label11.Text = "备    注:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(614, 228)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 16)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "Label10"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(121, 14)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 16)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Label8"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(18, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(96, 16)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "缴费单编号:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(544, 228)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 16)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "经办人:"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(336, 140)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(215, 26)
        Me.TextBox4.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(250, 143)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 16)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "发票编号:"
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Items.AddRange(New Object() {"POS机支付", "银行转账"})
        Me.ComboBox2.Location = New System.Drawing.Point(101, 140)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(112, 24)
        Me.ComboBox2.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(18, 143)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(80, 16)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "付款方式:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(250, 94)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "金额大写:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 94)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "应缴金额:"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(101, 49)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(597, 26)
        Me.TextBox1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "付款单位:"
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(11, 5)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(90, 20)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "检品缴费"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(107, 5)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(90, 20)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.Text = "协议缴费"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.RadioButton2)
        Me.Panel2.Controls.Add(Me.RadioButton1)
        Me.Panel2.Location = New System.Drawing.Point(12, 2)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(200, 32)
        Me.Panel2.TabIndex = 7
        '
        'rguid
        '
        Me.rguid.HeaderText = "rguid"
        Me.rguid.Name = "rguid"
        Me.rguid.ReadOnly = True
        Me.rguid.Visible = False
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
        Me.检验费.HeaderText = "检验费"
        Me.检验费.Name = "检验费"
        Me.检验费.ReadOnly = True
        '
        '主检科室
        '
        Me.主检科室.HeaderText = "主检科室"
        Me.主检科室.Name = "主检科室"
        Me.主检科室.ReadOnly = True
        '
        '缴费日期
        '
        Me.缴费日期.HeaderText = "缴费日期"
        Me.缴费日期.Name = "缴费日期"
        Me.缴费日期.ReadOnly = True
        Me.缴费日期.Visible = False
        Me.缴费日期.Width = 180
        '
        '缴费金额
        '
        Me.缴费金额.HeaderText = "缴费金额"
        Me.缴费金额.Name = "缴费金额"
        Me.缴费金额.ReadOnly = True
        Me.缴费金额.Visible = False
        '
        '凭证号码
        '
        Me.凭证号码.HeaderText = "凭证号码"
        Me.凭证号码.Name = "凭证号码"
        Me.凭证号码.ReadOnly = True
        Me.凭证号码.Visible = False
        '
        '备注
        '
        Me.备注.HeaderText = "备注"
        Me.备注.Name = "备注"
        Me.备注.ReadOnly = True
        Me.备注.Visible = False
        '
        '经手人
        '
        Me.经手人.HeaderText = "经手人"
        Me.经手人.Name = "经手人"
        Me.经手人.ReadOnly = True
        Me.经手人.Visible = False
        '
        'form_检验缴费
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(938, 520)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.dgv2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DGV1)
        Me.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "form_检验缴费"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "检验缴费"
        CType(Me.DGV1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgv2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DGV1 As DataGridView
    Friend WithEvents Button1 As Button
    Friend WithEvents dgv2 As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents Label13 As Label
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents Panel2 As Panel
    Friend WithEvents guid As DataGridViewTextBoxColumn
    Friend WithEvents 缴费单编号 As DataGridViewTextBoxColumn
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents rguid As DataGridViewTextBoxColumn
    Friend WithEvents 报告编号 As DataGridViewTextBoxColumn
    Friend WithEvents 样品名称 As DataGridViewTextBoxColumn
    Friend WithEvents 检验费 As DataGridViewTextBoxColumn
    Friend WithEvents 主检科室 As DataGridViewTextBoxColumn
    Friend WithEvents 缴费日期 As DataGridViewTextBoxColumn
    Friend WithEvents 缴费金额 As DataGridViewTextBoxColumn
    Friend WithEvents 凭证号码 As DataGridViewTextBoxColumn
    Friend WithEvents 备注 As DataGridViewTextBoxColumn
    Friend WithEvents 经手人 As DataGridViewTextBoxColumn
End Class
