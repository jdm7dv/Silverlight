Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Controls

Namespace DiggSample
	Partial Public Class StoryDetailsView
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub CloseBtn_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Visibility = Visibility.Collapsed
		End Sub
	End Class
End Namespace


