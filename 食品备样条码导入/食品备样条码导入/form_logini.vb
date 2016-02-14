Public Class form_loginini

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub form_loginini_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim keystr As String = "Software\食品报告"
        Dim rootkey, key As Microsoft.Win32.RegistryKey
        rootkey = My.Computer.Registry.CurrentUser
        key = rootkey.OpenSubKey(keystr, True)
        ComboBox2.Items.Clear()
        ComboBox2.Items.Add("192.168.0.77")
        ComboBox2.Items.Add("test-2008.cqidc.org.cn")
        If key.GetValue("server") Is Nothing Then ComboBox2.SelectedIndex = 0 Else ComboBox2.SelectedItem = key.GetValue("server")

        ComboBox1.Items.Clear()
        For Each iprt As String In System.Drawing.Printing.PrinterSettings.InstalledPrinters
            Me.ComboBox1.Items.Add(iprt)
        Next
        Dim emsprint As String = key.GetValue("emsprinter")
        If emsprint <> "" Then Me.ComboBox1.SelectedItem = emsprint
     

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim keystr As String = "Software\食品报告"
        Dim rootkey, key As Microsoft.Win32.RegistryKey
        rootkey = My.Computer.Registry.CurrentUser
        key = rootkey.OpenSubKey(keystr, True)
        key.SetValue("server", ComboBox2.SelectedItem.ToString)
        If ComboBox1.SelectedIndex >= 0 Then key.SetValue("emsprinter", Me.ComboBox1.SelectedItem)
        Me.Close()
    End Sub

    
End Class