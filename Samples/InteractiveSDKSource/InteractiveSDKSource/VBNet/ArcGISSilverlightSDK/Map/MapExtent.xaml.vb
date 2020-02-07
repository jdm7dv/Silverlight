﻿Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Controls

Namespace ArcGISSilverlightSDK
    Partial Public Class MapExtent
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MyMap_ExtentChange(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.ExtentEventArgs)
            setExtentText(args.NewExtent)
        End Sub

        Private Sub setExtentText(ByVal newExtent As ESRI.ArcGIS.Client.Geometry.Envelope)
            ExtentTextBlock.Text = String.Format("MinX: {0}" & Constants.vbLf & "MinY: {1}" & Constants.vbLf & "MaxX: {2}" & Constants.vbLf & "MaxY: {3}", newExtent.XMin, newExtent.YMin, newExtent.XMax, newExtent.YMax)
        End Sub
    End Class
End Namespace