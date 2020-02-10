Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Controls

Namespace ArcGISSilverlightSDK
    Partial Public Class ElementLayer
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub RedlandsButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
            MessageBox.Show("You found Redlands")
        End Sub
    End Class
End Namespace
