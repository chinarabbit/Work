<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class mydgv
    Inherits System.Windows.Forms.UserControl

    'UserControl 重写 Dispose，以清理组件列表。
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
        Me.components = New System.ComponentModel.Container()
        Me.dgv = New System.Windows.Forms.DataGridView()
        Me.dgvmenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.筛选 = New System.Windows.Forms.ToolStripMenuItem()
        Me.模糊筛选 = New System.Windows.Forms.ToolStripMenuItem()
        Me.排除筛选 = New System.Windows.Forms.ToolStripMenuItem()
        Me.清除筛选条件 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.导出所有记录 = New System.Windows.Forms.ToolStripMenuItem()
        Me.导出选中记录 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.自定义视图 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.flp1 = New System.Windows.Forms.FlowLayoutPanel()
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.dgvmenu.SuspendLayout()
        Me.flp1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgv
        '
        Me.dgv.AllowUserToAddRows = False
        Me.dgv.AllowUserToDeleteRows = False
        Me.dgv.AllowUserToOrderColumns = True
        Me.dgv.ColumnHeadersHeight = 25
        Me.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv.ContextMenuStrip = Me.dgvmenu
        Me.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv.EnableHeadersVisualStyles = False
        Me.dgv.Location = New System.Drawing.Point(0, 0)
        Me.dgv.Margin = New System.Windows.Forms.Padding(4)
        Me.dgv.Name = "dgv"
        Me.dgv.RowTemplate.Height = 23
        Me.dgv.Size = New System.Drawing.Size(828, 548)
        Me.dgv.TabIndex = 3
        '
        'dgvmenu
        '
        Me.dgvmenu.Font = New System.Drawing.Font("宋体", 10.0!)
        Me.dgvmenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.筛选, Me.模糊筛选, Me.排除筛选, Me.清除筛选条件, Me.ToolStripMenuItem1, Me.导出所有记录, Me.导出选中记录, Me.ToolStripMenuItem2, Me.自定义视图})
        Me.dgvmenu.Name = "dgvmenu"
        Me.dgvmenu.Size = New System.Drawing.Size(180, 202)
        '
        '筛选
        '
        Me.筛选.Name = "筛选"
        Me.筛选.Size = New System.Drawing.Size(179, 22)
        Me.筛选.Text = "筛选"
        '
        '模糊筛选
        '
        Me.模糊筛选.Name = "模糊筛选"
        Me.模糊筛选.Size = New System.Drawing.Size(179, 22)
        Me.模糊筛选.Text = "模糊筛选"
        '
        '排除筛选
        '
        Me.排除筛选.Name = "排除筛选"
        Me.排除筛选.Size = New System.Drawing.Size(179, 22)
        Me.排除筛选.Text = "排除筛选"
        '
        '清除筛选条件
        '
        Me.清除筛选条件.Name = "清除筛选条件"
        Me.清除筛选条件.Size = New System.Drawing.Size(179, 22)
        Me.清除筛选条件.Text = "清除筛选条件"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(179, 22)
        Me.ToolStripMenuItem1.Text = "---------------"
        '
        '导出所有记录
        '
        Me.导出所有记录.Name = "导出所有记录"
        Me.导出所有记录.Size = New System.Drawing.Size(179, 22)
        Me.导出所有记录.Text = "导出所有记录"
        '
        '导出选中记录
        '
        Me.导出选中记录.Name = "导出选中记录"
        Me.导出选中记录.Size = New System.Drawing.Size(179, 22)
        Me.导出选中记录.Text = "导出选中记录"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(179, 22)
        Me.ToolStripMenuItem2.Text = "--------------"
        '
        '自定义视图
        '
        Me.自定义视图.Name = "自定义视图"
        Me.自定义视图.Size = New System.Drawing.Size(179, 22)
        Me.自定义视图.Text = "自定义视图"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "111"
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(731, 549)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(97, 26)
        Me.TextBox1.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(656, 552)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "编号快查"
        '
        'flp1
        '
        Me.flp1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.flp1.Controls.Add(Me.Label1)
        Me.flp1.Location = New System.Drawing.Point(0, 556)
        Me.flp1.Name = "flp1"
        Me.flp1.Size = New System.Drawing.Size(368, 16)
        Me.flp1.TabIndex = 13
        '
        'mydgv
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.flp1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.dgv)
        Me.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "mydgv"
        Me.Size = New System.Drawing.Size(828, 572)
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.dgvmenu.ResumeLayout(False)
        Me.flp1.ResumeLayout(False)
        Me.flp1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgv As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgvmenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 导出所有记录 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 清除筛选条件 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 导出选中记录 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 模糊筛选 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents 筛选 As ToolStripMenuItem
    Friend WithEvents 排除筛选 As ToolStripMenuItem
    Friend WithEvents flp1 As FlowLayoutPanel
    Friend WithEvents 自定义视图 As ToolStripMenuItem
End Class
