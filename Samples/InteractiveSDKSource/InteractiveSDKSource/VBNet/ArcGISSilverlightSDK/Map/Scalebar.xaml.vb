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
    Partial Public Class Scalebar
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MyScaleBar_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
            Dim scaleBar As ESRI.ArcGIS.Client.ScaleBar = TryCast(sender, ESRI.ArcGIS.Client.ScaleBar)
            scaleBar.Map = myMap
        End Sub
    End Class
End Namespace
