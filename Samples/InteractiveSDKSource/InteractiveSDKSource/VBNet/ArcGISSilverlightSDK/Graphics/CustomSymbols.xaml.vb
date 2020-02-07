﻿Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes

Namespace ArcGISSilverlightSDK
    Partial Public Class CustomSymbols
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub GraphicsLayer_MouseLeftButtonDown(ByVal sender As System.Object, ByVal graphic As ESRI.ArcGIS.Client.Graphic, ByVal args As System.Windows.Input.MouseButtonEventArgs)
            If (graphic.Selected) Then
                graphic.UnSelect()
            Else
                graphic.Select()
            End If
        End Sub

    End Class
End Namespace
