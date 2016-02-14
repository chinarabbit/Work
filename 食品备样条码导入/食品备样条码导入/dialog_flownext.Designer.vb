<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dialog_flownext
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lab_curstep = New System.Windows.Forms.Label()
        Me.lab_nextstep = New System.Windows.Forms.Label()
        Me.lab_curperson = New System.Windows.Forms.Label()
        Me.Lab_qfrq = New System.Windows.Forms.Label()
        Me.qfrq = New System.Windows.Forms.DateTimePicker()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(228, 136)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(195, 36)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(4, 4)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(89, 28)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "确定"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(101, 4)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(89, 28)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "取消"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "当前进度:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(206, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "下一进度:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(22, 112)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "执行人:"
        '
        'lab_curstep
        '
        Me.lab_curstep.AutoSize = True
        Me.lab_curstep.BackColor = System.Drawing.Color.Yellow
        Me.lab_curstep.Location = New System.Drawing.Point(109, 27)
        Me.lab_curstep.Name = "lab_curstep"
        Me.lab_curstep.Size = New System.Drawing.Size(56, 16)
        Me.lab_curstep.TabIndex = 4
        Me.lab_curstep.Text = "Label4"
        '
        'lab_nextstep
        '
        Me.lab_nextstep.AutoSize = True
        Me.lab_nextstep.BackColor = System.Drawing.Color.Yellow
        Me.lab_nextstep.Location = New System.Drawing.Point(292, 27)
        Me.lab_nextstep.Name = "lab_nextstep"
        Me.lab_nextstep.Size = New System.Drawing.Size(56, 16)
        Me.lab_nextstep.TabIndex = 5
        Me.lab_nextstep.Text = "Label5"
        '
        'lab_curperson
        '
        Me.lab_curperson.AutoSize = True
        Me.lab_curperson.Location = New System.Drawing.Point(92, 112)
        Me.lab_curperson.Name = "lab_curperson"
        Me.lab_curperson.Size = New System.Drawing.Size(56, 16)
        Me.lab_curperson.TabIndex = 6
        Me.lab_curperson.Text = "Label4"
        '
        'Lab_qfrq
        '
        Me.Lab_qfrq.AutoSize = True
        Me.Lab_qfrq.Location = New System.Drawing.Point(22, 69)
        Me.Lab_qfrq.Name = "Lab_qfrq"
        Me.Lab_qfrq.Size = New System.Drawing.Size(80, 16)
        Me.Lab_qfrq.TabIndex = 7
        Me.Lab_qfrq.Text = "签发日期:"
        Me.Lab_qfrq.Visible = False
        '
        'qfrq
        '
        Me.qfrq.Location = New System.Drawing.Point(108, 62)
        Me.qfrq.Name = "qfrq"
        Me.qfrq.Size = New System.Drawing.Size(129, 26)
        Me.qfrq.TabIndex = 8
        Me.qfrq.Visible = False
        '
        'dialog_flownext
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(439, 187)
        Me.Controls.Add(Me.qfrq)
        Me.Controls.Add(Me.Lab_qfrq)
        Me.Controls.Add(Me.lab_curperson)
        Me.Controls.Add(Me.lab_nextstep)
        Me.Controls.Add(Me.lab_curstep)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dialog_flownext"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "处理完毕"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lab_curstep As Label
    Friend WithEvents lab_nextstep As Label
    Friend WithEvents lab_curperson As Label
    Friend WithEvents Lab_qfrq As Label
    Friend WithEvents qfrq As DateTimePicker
End Class
