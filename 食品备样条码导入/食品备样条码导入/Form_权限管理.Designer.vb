<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_权限管理
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
        Me.权限列表 = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.新权限 = New System.Windows.Forms.TextBox()
        Me.添加新权限 = New System.Windows.Forms.Button()
        Me.现有人员 = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.科室 = New System.Windows.Forms.ComboBox()
        Me.有权人员 = New System.Windows.Forms.ListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.SuspendLayout()
        '
        '权限列表
        '
        Me.权限列表.FormattingEnabled = True
        Me.权限列表.ItemHeight = 16
        Me.权限列表.Location = New System.Drawing.Point(12, 51)
        Me.权限列表.Name = "权限列表"
        Me.权限列表.Size = New System.Drawing.Size(130, 468)
        Me.权限列表.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "权限列表"
        '
        '新权限
        '
        Me.新权限.Location = New System.Drawing.Point(152, 13)
        Me.新权限.Name = "新权限"
        Me.新权限.Size = New System.Drawing.Size(140, 26)
        Me.新权限.TabIndex = 2
        '
        '添加新权限
        '
        Me.添加新权限.Location = New System.Drawing.Point(298, 12)
        Me.添加新权限.Name = "添加新权限"
        Me.添加新权限.Size = New System.Drawing.Size(108, 28)
        Me.添加新权限.TabIndex = 3
        Me.添加新权限.Text = "添加新权限"
        Me.添加新权限.UseVisualStyleBackColor = True
        '
        '现有人员
        '
        Me.现有人员.FormattingEnabled = True
        Me.现有人员.ItemHeight = 16
        Me.现有人员.Location = New System.Drawing.Point(467, 83)
        Me.现有人员.Name = "现有人员"
        Me.现有人员.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.现有人员.Size = New System.Drawing.Size(133, 436)
        Me.现有人员.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(482, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "现有人员"
        '
        '科室
        '
        Me.科室.FormattingEnabled = True
        Me.科室.Location = New System.Drawing.Point(467, 51)
        Me.科室.Name = "科室"
        Me.科室.Size = New System.Drawing.Size(133, 24)
        Me.科室.TabIndex = 6
        '
        '有权人员
        '
        Me.有权人员.FormattingEnabled = True
        Me.有权人员.ItemHeight = 16
        Me.有权人员.Location = New System.Drawing.Point(234, 83)
        Me.有权人员.Name = "有权人员"
        Me.有权人员.Size = New System.Drawing.Size(133, 436)
        Me.有权人员.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(231, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 16)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "有权限人员"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(335, 54)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(91, 20)
        Me.CheckBox1.TabIndex = 9
        Me.CheckBox1.Text = "科室信息"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(373, 313)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(87, 33)
        Me.Button3.TabIndex = 12
        Me.Button3.Text = "去除人员"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(373, 171)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(87, 31)
        Me.Button4.TabIndex = 13
        Me.Button4.Text = "加入人员"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(633, 83)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(115, 33)
        Me.Button1.TabIndex = 14
        Me.Button1.Text = "设置修改字段"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(633, 150)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(115, 32)
        Me.Button2.TabIndex = 15
        Me.Button2.Text = "模板设置"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(633, 273)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(115, 30)
        Me.Button5.TabIndex = 16
        Me.Button5.Text = "下载签名"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Form_权限管理
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(760, 555)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.有权人员)
        Me.Controls.Add(Me.科室)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.现有人员)
        Me.Controls.Add(Me.添加新权限)
        Me.Controls.Add(Me.新权限)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.权限列表)
        Me.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form_权限管理"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form_权限管理"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents 权限列表 As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents 新权限 As System.Windows.Forms.TextBox
    Friend WithEvents 添加新权限 As System.Windows.Forms.Button
    Friend WithEvents 现有人员 As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents 科室 As System.Windows.Forms.ComboBox
    Friend WithEvents 有权人员 As System.Windows.Forms.ListBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
End Class
