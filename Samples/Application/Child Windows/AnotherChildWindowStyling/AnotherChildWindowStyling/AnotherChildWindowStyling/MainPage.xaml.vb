Partial Public Class MainPage
	Inherits UserControl

	Public Sub New()
		InitializeComponent()
	End Sub

  Private Sub btClick_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btClick.Click

    Dim cw As New cwLawBot
    cw.Show()

  End Sub
End Class