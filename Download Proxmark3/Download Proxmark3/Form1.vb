Imports System.Text.RegularExpressions
Imports System.Net
Imports System.IO
Imports System.Management

Public Class Form1
    Dim ProxSpace As String = "https://github.com/Gator96100/ProxSpace/releases"
    Dim Proxmark As String = "https://github.com/RfidResearchGroup/proxmark3/releases"
    Dim wc As New WebClient
    Dim UrlProx As String = Nothing
    Dim urlmark As String = Nothing
    Dim Version As New List(Of String)
    Dim Version2 As New List(Of String)

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        Dim sourceString As String = New System.Net.WebClient().DownloadString(ProxSpace)
        If sourceString.Length > 0 Then
            Version.Clear()
            Dim matches As MatchCollection = Regex.Matches(sourceString, "<span class=""css-truncate-target"" style=""max-width: 125px"">(.*)</span>", RegexOptions.IgnoreCase)
            For Each Matche As Match In matches
                Version.Add(Matche.Groups(1).Value)
            Next
            ComboBox1.Items.AddRange(Version.Distinct.ToArray)
            If ComboBox1.Items.Count > 0 Then
                ComboBox1.SelectedIndex = 0
            End If
        End If
        Dim sourceString2 As String = New System.Net.WebClient().DownloadString(Proxmark)
        If sourceString2.Length > 0 Then
            Version2.Clear()
            Dim matches1 As MatchCollection = Regex.Matches(sourceString2, "<span class=""css-truncate-target"" style=""max-width: 125px"">(v[0-9]\.[0-9]{1,5})</span>", RegexOptions.IgnoreCase)
            For Each Matche1 As Match In matches1
                Version2.Add(Matche1.Groups(1).Value)
            Next
            ComboBox2.Items.AddRange(Version2.Distinct.ToArray)
            If ComboBox2.Items.Count > 0 Then
                ComboBox2.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            CheckBox2.Checked = False
            If ComboBox2.Items.Count > 0 Then
                ComboBox2.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            CheckBox1.Checked = False
            CheckBox3.Visible = False
            If ComboBox1.Items.Count > 0 Then
                ComboBox1.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim folder As New FolderBrowserDialog With {
            .Description = "Veuillez choisir un répertoire pour télécharger le fichier de votre de choix !",
            .ShowNewFolderButton = False,
            .RootFolder = Environment.SpecialFolder.Desktop}
        If folder.ShowDialog = Windows.Forms.DialogResult.OK Then
            AddHandler wc.DownloadProgressChanged, AddressOf DownloadProgress
            AddHandler wc.DownloadFileCompleted, AddressOf DownloadFinish
                If CheckBox1.Checked = True Then
                wc.DownloadFileAsync(New Uri("https://github.com" & UrlProx), Path.Combine(folder.SelectedPath, GetFileName("https://github.com" & UrlProx)))
                End If
            If CheckBox2.Checked = True Then
                Threading.Thread.Sleep(600)
                wc.DownloadFileAsync(New Uri("https://github.com" & urlmark), Path.Combine(folder.SelectedPath, GetFileName("https://github.com" & urlmark)))
            End If
            Button1.Enabled = False
            GroupBox1.Enabled = False
            CheckBox1.Visible = False
            CheckBox2.Visible = False
            CheckBox2.Visible = False
        Else : Exit Sub
        End If
    End Sub

    Private Sub DownloadProgress(sender As Object, e As DownloadProgressChangedEventArgs)
        On Error Resume Next
        ProgressBar1.Value = e.ProgressPercentage
        Label3.Text = FormatSize(e.BytesReceived) & " Sur " & FormatSize(e.TotalBytesToReceive) & " - " & e.ProgressPercentage & " %"
        Label3.Left = (Me.Width - Label3.Width) / 2
        ToolStripStatusLabel1.Text = "Téléchargement en cours..."
        ToolStripStatusLabel1.ForeColor = Color.Red
    End Sub

    Private Sub DownloadFinish(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
        If e.Error IsNot Nothing Then
            ToolStripStatusLabel1.Text = "Erreur de téléchargement"
            ToolStripStatusLabel1.ForeColor = Color.Red
        ElseIf e.UserState Is Nothing Then
            Label3.Text = String.Empty
            ProgressBar1.Value = 0
            Button1.Enabled = True
            GroupBox1.Enabled = True
            CheckBox1.Visible = True
            CheckBox2.Visible = True
            CheckBox2.Visible = True
            ToolStripStatusLabel1.Text = "Idle..."
            ToolStripStatusLabel1.ForeColor = Color.Black
        End If
    End Sub

    Private Function GetFileName(ByVal URL As String) As String
        Try
            Return URL.Substring(URL.LastIndexOf("/") + 1)
        Catch ex As Exception
            Return URL
        End Try
    End Function

    Private Function FormatSize(byteCount As Long) As String
        Dim suf As String() = {"Bytes", "KB", "MB", "GB"}
        If byteCount = 0 Then Return "0" & suf(0)
        Dim bytes As Long = Math.Abs(byteCount)
        Dim place As Integer = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)))
        Dim num As Double = Math.Round(bytes / Math.Pow(1024, place), 1)
        Return (Math.Sign(byteCount) * num).ToString() & " " & suf(place)
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim sourceString As String = New System.Net.WebClient().DownloadString(ProxSPace)
        Dim matches2 As MatchCollection = Regex.Matches(sourceString, "<a href=""(.*)"" rel=""nofollow"" class=""d-flex flex-items-center min-width-0"">", RegexOptions.IgnoreCase)
        Dim result As String = matches2(ComboBox1.SelectedIndex).Groups(1).Value
        If result.Length > 0 Then
            Button1.Enabled = True
            If result.IndexOf("32") > 0 Or result.IndexOf("64") > 0 Then
                CheckBox3.Visible = True
                If System.Environment.Is64BitOperatingSystem Then
                    CheckBox3.Text = "64 bits"
                    CheckBox3.Checked = True
                    UrlProx = matches2(10).Groups(1).Value
                Else
                    CheckBox3.Text = "32 bits"
                    CheckBox3.Checked = True
                    UrlProx = matches2(9).Groups(1).Value
                End If
                Else
                    CheckBox3.Visible = False
                    UrlProx = matches2(ComboBox1.SelectedIndex).Groups(1).Value
                End If
            Else
                Button1.Enabled = False
            End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim sourceString2 As String = New System.Net.WebClient().DownloadString(Proxmark)
        Dim matches4 As MatchCollection = Regex.Matches(sourceString2, "\/RfidResearchGroup\/.*zip", RegexOptions.IgnoreCase)
        Dim result As String = matches4(ComboBox2.SelectedIndex).Value
        If result.Length > 0 Then
            Button1.Enabled = True
            urlmark = matches4(ComboBox2.SelectedIndex).Value
        Else
            Button1.Enabled = False
        End If
    End Sub

    Private Sub ComboBox2_DrawItem(sender As System.Object, e As System.Windows.Forms.DrawItemEventArgs) Handles ComboBox2.DrawItem
        e.DrawBackground()
        If e.Index > -1 Then
            Using fmt As New StringFormat With {
                .Alignment = StringAlignment.Center,
                .LineAlignment = StringAlignment.Center}
                If (e.State & DrawItemState.Selected) = DrawItemState.Selected Then
                    e.Graphics.DrawString(ComboBox2.Items(e.Index).ToString(), e.Font, New SolidBrush(e.ForeColor), e.Bounds, fmt)
                Else
                    e.Graphics.DrawString(ComboBox2.Items(e.Index).ToString(), e.Font, New SolidBrush(e.ForeColor), e.Bounds, fmt)
                End If
            End Using
        End If
    End Sub

    Private Sub ComboBox1_DrawItem(sender As System.Object, e As System.Windows.Forms.DrawItemEventArgs) Handles ComboBox1.DrawItem
        e.DrawBackground()
        If e.Index > -1 Then
            Using fmt As New StringFormat With {
                .Alignment = StringAlignment.Center,
                .LineAlignment = StringAlignment.Center}
                If (e.State & DrawItemState.Selected) = DrawItemState.Selected Then
                    e.Graphics.DrawString(ComboBox1.Items(e.Index).ToString(), e.Font, New SolidBrush(e.ForeColor), e.Bounds, fmt)
                Else
                    e.Graphics.DrawString(ComboBox1.Items(e.Index).ToString(), e.Font, New SolidBrush(e.ForeColor), e.Bounds, fmt)
                End If
            End Using
        End If
    End Sub

    Private Sub CheckBox1_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles CheckBox1.Paint
        If CheckBox1.Checked = True Then
            e.Graphics.DrawImageUnscaled(My.Resources.Yes, 0, 4)
        ElseIf CheckBox1.Checked = False Then
            e.Graphics.DrawImageUnscaled(My.Resources.cancel, 0, 4)
        End If
    End Sub

    Private Sub CheckBox2_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles CheckBox2.Paint
        If CheckBox2.Checked = True Then
            e.Graphics.DrawImageUnscaled(My.Resources.Yes, 0, 4)
        ElseIf CheckBox2.Checked = False Then
            e.Graphics.DrawImageUnscaled(My.Resources.cancel, 0, 4)
        End If
    End Sub

    Private Sub CheckBox3_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles CheckBox3.Paint
        If CheckBox3.Checked = True Then
            e.Graphics.DrawImageUnscaled(My.Resources.Yes, 0, 4)
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Enabled = True Then
            CheckBox3.Checked = True
        End If
    End Sub
End Class
