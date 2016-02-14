<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class mainform
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(mainform))
        Me.备样条码 = New System.Windows.Forms.Button()
        Me.批量修改 = New System.Windows.Forms.Button()
        Me.权限设置 = New System.Windows.Forms.Button()
        Me.报告存档 = New System.Windows.Forms.Button()
        Me.留样管理 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.附页管理 = New System.Windows.Forms.Button()
        Me.报告发放 = New System.Windows.Forms.Button()
        Me.导出项目 = New System.Windows.Forms.Button()
        Me.协议管理 = New System.Windows.Forms.Button()
        Me.报告打印 = New System.Windows.Forms.Button()
        Me.检验缴费 = New System.Windows.Forms.Button()
        Me.缴费单管理 = New System.Windows.Forms.Button()
        Me.报告编制 = New System.Windows.Forms.Button()
        Me.报告审核 = New System.Windows.Forms.Button()
        Me.报告签发 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        '备样条码
        '
        Me.备样条码.Enabled = False
        Me.备样条码.Location = New System.Drawing.Point(47, 26)
        Me.备样条码.Margin = New System.Windows.Forms.Padding(5)
        Me.备样条码.Name = "备样条码"
        Me.备样条码.Size = New System.Drawing.Size(108, 66)
        Me.备样条码.TabIndex = 0
        Me.备样条码.Text = "备样条码离线导入"
        Me.备样条码.UseVisualStyleBackColor = True
        '
        '批量修改
        '
        Me.批量修改.Enabled = False
        Me.批量修改.Location = New System.Drawing.Point(200, 26)
        Me.批量修改.Margin = New System.Windows.Forms.Padding(5)
        Me.批量修改.Name = "批量修改"
        Me.批量修改.Size = New System.Drawing.Size(108, 66)
        Me.批量修改.TabIndex = 1
        Me.批量修改.Text = "任务信息批量修改"
        Me.批量修改.UseVisualStyleBackColor = True
        '
        '权限设置
        '
        Me.权限设置.Location = New System.Drawing.Point(417, 351)
        Me.权限设置.Name = "权限设置"
        Me.权限设置.Size = New System.Drawing.Size(98, 51)
        Me.权限设置.TabIndex = 2
        Me.权限设置.Text = "权限设置"
        Me.权限设置.UseVisualStyleBackColor = True
        Me.权限设置.Visible = False
        '
        '报告存档
        '
        Me.报告存档.Enabled = False
        Me.报告存档.Location = New System.Drawing.Point(47, 279)
        Me.报告存档.Name = "报告存档"
        Me.报告存档.Size = New System.Drawing.Size(108, 60)
        Me.报告存档.TabIndex = 3
        Me.报告存档.Text = "报告资料存档管理"
        Me.报告存档.UseVisualStyleBackColor = True
        '
        '留样管理
        '
        Me.留样管理.Enabled = False
        Me.留样管理.Location = New System.Drawing.Point(200, 279)
        Me.留样管理.Margin = New System.Windows.Forms.Padding(5)
        Me.留样管理.Name = "留样管理"
        Me.留样管理.Size = New System.Drawing.Size(108, 60)
        Me.留样管理.TabIndex = 4
        Me.留样管理.Text = "留样管理"
        Me.留样管理.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(563, 348)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(98, 54)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "退出"
        Me.Button1.UseVisualStyleBackColor = True
        '
        '附页管理
        '
        Me.附页管理.Enabled = False
        Me.附页管理.Location = New System.Drawing.Point(356, 279)
        Me.附页管理.Name = "附页管理"
        Me.附页管理.Size = New System.Drawing.Size(106, 60)
        Me.附页管理.TabIndex = 6
        Me.附页管理.Text = "附页管理"
        Me.附页管理.UseVisualStyleBackColor = True
        '
        '报告发放
        '
        Me.报告发放.Enabled = False
        Me.报告发放.Location = New System.Drawing.Point(356, 26)
        Me.报告发放.Name = "报告发放"
        Me.报告发放.Size = New System.Drawing.Size(106, 66)
        Me.报告发放.TabIndex = 7
        Me.报告发放.Text = "报告发放"
        Me.报告发放.UseVisualStyleBackColor = True
        '
        '导出项目
        '
        Me.导出项目.Enabled = False
        Me.导出项目.Location = New System.Drawing.Point(511, 279)
        Me.导出项目.Name = "导出项目"
        Me.导出项目.Size = New System.Drawing.Size(106, 60)
        Me.导出项目.TabIndex = 8
        Me.导出项目.Text = "导出项目"
        Me.导出项目.UseVisualStyleBackColor = True
        '
        '协议管理
        '
        Me.协议管理.Enabled = False
        Me.协议管理.Location = New System.Drawing.Point(47, 114)
        Me.协议管理.Name = "协议管理"
        Me.协议管理.Size = New System.Drawing.Size(108, 59)
        Me.协议管理.TabIndex = 9
        Me.协议管理.Text = "协议管理"
        Me.协议管理.UseVisualStyleBackColor = True
        '
        '报告打印
        '
        Me.报告打印.Enabled = False
        Me.报告打印.Location = New System.Drawing.Point(200, 114)
        Me.报告打印.Name = "报告打印"
        Me.报告打印.Size = New System.Drawing.Size(108, 59)
        Me.报告打印.TabIndex = 10
        Me.报告打印.Text = "报告打印"
        Me.报告打印.UseVisualStyleBackColor = True
        '
        '检验缴费
        '
        Me.检验缴费.Enabled = False
        Me.检验缴费.Location = New System.Drawing.Point(356, 114)
        Me.检验缴费.Name = "检验缴费"
        Me.检验缴费.Size = New System.Drawing.Size(106, 59)
        Me.检验缴费.TabIndex = 11
        Me.检验缴费.Text = "检验缴费"
        Me.检验缴费.UseVisualStyleBackColor = True
        '
        '缴费单管理
        '
        Me.缴费单管理.Location = New System.Drawing.Point(510, 26)
        Me.缴费单管理.Name = "缴费单管理"
        Me.缴费单管理.Size = New System.Drawing.Size(114, 66)
        Me.缴费单管理.TabIndex = 12
        Me.缴费单管理.Text = "缴费单管理"
        Me.缴费单管理.UseVisualStyleBackColor = True
        '
        '报告编制
        '
        Me.报告编制.Location = New System.Drawing.Point(509, 114)
        Me.报告编制.Name = "报告编制"
        Me.报告编制.Size = New System.Drawing.Size(115, 59)
        Me.报告编制.TabIndex = 13
        Me.报告编制.Text = "报告编制"
        Me.报告编制.UseVisualStyleBackColor = True
        '
        '报告审核
        '
        Me.报告审核.Location = New System.Drawing.Point(47, 201)
        Me.报告审核.Name = "报告审核"
        Me.报告审核.Size = New System.Drawing.Size(108, 59)
        Me.报告审核.TabIndex = 14
        Me.报告审核.Text = "报告审核"
        Me.报告审核.UseVisualStyleBackColor = True
        '
        '报告签发
        '
        Me.报告签发.Location = New System.Drawing.Point(200, 201)
        Me.报告签发.Name = "报告签发"
        Me.报告签发.Size = New System.Drawing.Size(108, 59)
        Me.报告签发.TabIndex = 15
        Me.报告签发.Text = "报告签发"
        Me.报告签发.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(47, 359)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(97, 35)
        Me.Button2.TabIndex = 16
        Me.Button2.Text = "test"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'mainform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(673, 414)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.报告签发)
        Me.Controls.Add(Me.报告审核)
        Me.Controls.Add(Me.报告编制)
        Me.Controls.Add(Me.缴费单管理)
        Me.Controls.Add(Me.检验缴费)
        Me.Controls.Add(Me.报告打印)
        Me.Controls.Add(Me.协议管理)
        Me.Controls.Add(Me.导出项目)
        Me.Controls.Add(Me.报告发放)
        Me.Controls.Add(Me.附页管理)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.留样管理)
        Me.Controls.Add(Me.报告存档)
        Me.Controls.Add(Me.权限设置)
        Me.Controls.Add(Me.批量修改)
        Me.Controls.Add(Me.备样条码)
        Me.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(5)
        Me.Name = "mainform"
        Me.Text = "主界面"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents 备样条码 As System.Windows.Forms.Button
    Friend WithEvents 批量修改 As System.Windows.Forms.Button
    Friend WithEvents 权限设置 As System.Windows.Forms.Button
    Friend WithEvents 报告存档 As System.Windows.Forms.Button
    Friend WithEvents 留样管理 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents 附页管理 As System.Windows.Forms.Button
    Friend WithEvents 报告发放 As System.Windows.Forms.Button
    Friend WithEvents 导出项目 As System.Windows.Forms.Button
    Friend WithEvents 协议管理 As System.Windows.Forms.Button
    Friend WithEvents 报告打印 As Button
    Friend WithEvents 检验缴费 As Button
    Friend WithEvents 缴费单管理 As Button
    Friend WithEvents 报告编制 As Button
    Friend WithEvents 报告审核 As Button
    Friend WithEvents 报告签发 As Button
    Friend WithEvents Button2 As Button
End Class
