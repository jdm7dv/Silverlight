Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.ServiceModel.Syndication
Imports System.IO
Imports System.Collections.Generic
Imports System.Linq
Imports ESRI.ArcGIS.Client
Imports ESRI.ArcGIS.Client.Symbols
Imports System.Windows.Media.Imaging
Imports ESRI.ArcGIS.Client.Geometry
Imports System.Windows.Media
Imports System.Windows.Threading
Imports System.Xml.Linq
Imports System.Collections

Namespace ArcGISSilverlightSDK
    Partial Public Class GeoRSS
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Fetch_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If FeedLocationTextBox.Text <> String.Empty Then
                LoadRSS(FeedLocationTextBox.Text.Trim())
                Dim UpdateTimer As DispatcherTimer = New System.Windows.Threading.DispatcherTimer()
                UpdateTimer.Interval = New TimeSpan(0, 0, 0, 0, 60000)
                AddHandler UpdateTimer.Tick, AddressOf AnonymousMethod1
                UpdateTimer.Start()
            End If
        End Sub
        Private Sub AnonymousMethod1(ByVal evtsender As Object, ByVal args As Object)
            LoadRSS(FeedLocationTextBox.Text.Trim())
        End Sub

        Protected Sub LoadRSS(ByVal uri As String)
            Dim wc As New WebClient()
            AddHandler wc.OpenReadCompleted, AddressOf wc_OpenReadCompleted
            Dim feedUri As New Uri(uri, UriKind.Absolute)
            wc.OpenReadAsync(feedUri)
        End Sub

        Private Sub wc_OpenReadCompleted(ByVal sender As Object, ByVal e As OpenReadCompletedEventArgs)

            If e.Error IsNot Nothing Then
                FeedLocationTextBox.Text = "Error in Reading Feed. Try Again later!!"
                Return
            End If

            ' Optional, use LINQ to query GeoRSS feed.
            'UseLinq(e.Result); return;

            Using s As Stream = e.Result
                Dim feed As SyndicationFeed
                Dim feedItems As New List(Of SyndicationItem)()

                Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
                graphicsLayer.ClearGraphics()

                Using reader As XmlReader = XmlReader.Create(s)
                    feed = SyndicationFeed.Load(reader)
                    For Each feedItem As SyndicationItem In feed.Items
                        Dim ec As SyndicationElementExtensionCollection = feedItem.ElementExtensions

                        Dim x As String = ""
                        Dim y As String = ""
                        Dim magnitude As String = feedItem.Title.Text

                        For Each ee As SyndicationElementExtension In ec
                            Dim xr As XmlReader = ee.GetReader()
                            Select Case ee.OuterName
                                Case ("lat")
                                    y = xr.ReadElementContentAsString()
                                    Exit Select
                                Case ("long")
                                    x = xr.ReadElementContentAsString()
                                    Exit Select
                            End Select
                        Next ee

                        If (Not String.IsNullOrEmpty(x)) Then
                            Dim graphic As New Graphic() With {.Geometry = New MapPoint(Convert.ToDouble(x), Convert.ToDouble(y)), .Symbol = QuakePictureSymbol}

                            graphic.Attributes.Add("MAGNITUDE", magnitude)

                            graphicsLayer.Graphics.Add(graphic)
                        End If
                    Next feedItem
                End Using
            End Using
        End Sub

        Private Sub UseLinq(ByVal s As Stream)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()

            Dim doc As XDocument = XDocument.Load(s)
            Dim geo As XNamespace = "http://www.w3.org/2003/01/geo/wgs84_pos#"


            Dim rssGraphics = _
             From rssGraphicElement In doc.Descendants("item") _
             Select New RssGraphic With { _
                .Geometry = New MapPoint(Convert.ToDouble(rssGraphicElement.Element(geo + "long").Value), Convert.ToDouble(rssGraphicElement.Element(geo + "lat").Value)), _
                .Symbol = QuakePictureSymbol, _
                .AddRssAttribute = String.Format("{0},{1}", "MAGNITUDE", rssGraphicElement.Element("title").Value) _
            }

            For Each rssGraphic As RssGraphic In rssGraphics
                For Each rssAttribute As KeyValuePair(Of String, Object) In rssGraphic.RssAttributes
                    rssGraphic.Attributes.Add(rssAttribute.Key, rssAttribute.Value)
                Next rssAttribute
                graphicsLayer.Graphics.Add(CType(rssGraphic, Graphic))
            Next rssGraphic
        End Sub

        Friend Class RssGraphic
            Inherits Graphic
            Private privateRssAttributes As Dictionary(Of String, Object)
            Public Property RssAttributes() As Dictionary(Of String, Object)
                Get
                    Return privateRssAttributes
                End Get
                Set(ByVal value As Dictionary(Of String, Object))
                    privateRssAttributes = value
                End Set
            End Property
            ' Collection initializer not available in VB.NET
            Public WriteOnly Property AddRssAttribute() As String
                Set(ByVal value As String)
                    Dim tempString As String() = value.Split(New [Char]() {","c})
                    privateRssAttributes.Add(tempString(0), tempString(1))
                End Set
            End Property
        End Class
    End Class
End Namespace

