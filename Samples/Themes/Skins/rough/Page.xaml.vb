Partial Public Class Page
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub

    'Because we're autogenerating columns we can't attach a style to control the content and need to use code to work around it
    Private Sub dataGridInstance_AutoGeneratingColumn(ByVal sender As Object, ByVal e As DataGridAutoGeneratingColumnEventArgs)
        If (e.Column.ToString().Equals("System.Windows.Controls.DataGridTextBoxColumn")) Then

            'Change the font color to White - needed because we're autogenerating the columns instead of doing it manually
            'CType(e.Column, DataGridTextBoxColumn).Foreground = New SolidColorBrush(Colors.White)

            'Change the font family to Verdana - needed because we're autogenerating the columns instead of doing it manually
            CType(e.Column, DataGridTextBoxColumn).FontFamily = New FontFamily("Verdana")

            'Change the font size to 10 - needed because we're autogenerating the columns instead of doing it manually
            CType(e.Column, DataGridTextBoxColumn).FontSize = 10

        End If
    End Sub

End Class