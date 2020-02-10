Imports Microsoft.VisualBasic
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
Imports ESRI.ArcGIS.Client
Imports ESRI.ArcGIS.Client.Tasks
Imports ESRI.ArcGIS.Client.Toolkit
Imports ESRI.ArcGIS.Client.Symbols

Namespace ArcGISSilverlightSDK
    Partial Public Class Bookmarks
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MyBookmarks_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            MyBookmarks.Map = MyMap

            'MyBookmarks.ClearBookmarks(); // Remove all bookmarks
            'MyBookmarks.AddBookmark("Mix - Las Vegas", new ESRI.ArcGIS.Client.Geometry.Envelope(-115.212, 36.083, -115.111, 36.157));
            'MyBookmarks.AddBookmark("DevSummit - Palm Springs", new ESRI.ArcGIS.Client.Geometry.Envelope(-116.55, 33.816, -113.525, 33.834));
            'MyBookmarks.AddBookmark("User Conference - San Diego", new ESRI.ArcGIS.Client.Geometry.Envelope(-117.211, 32.665, -117.11, 32.739));
        End Sub
    End Class
End Namespace
