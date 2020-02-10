imports System
imports System.Collections.Generic
imports System.Linq
imports System.Windows
imports System.Windows.Controls
imports System.Windows.Documents
imports System.Windows.Input
imports System.Windows.Media
imports System.Windows.Media.Animation
imports System.Windows.Shapes


Partial Public Class cwLawBot
  Inherits ChildWindow




  Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    Me.DialogResult = True
  End Sub

  Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    Me.DialogResult = False
  End Sub

  Public Sub New()
    InitializeComponent()
  End Sub
End Class
