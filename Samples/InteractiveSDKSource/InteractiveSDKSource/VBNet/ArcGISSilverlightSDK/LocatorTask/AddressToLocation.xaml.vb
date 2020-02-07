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
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry

Namespace ArcGISSilverlightSDK
    Partial Public Class AddressToLocation
        Inherits UserControl
        Private _candidateList As List(Of AddressCandidate)
        Private _firstZoom As Boolean = True
        Private _lastIndex As Integer = 0

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub FindAddressButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim locatorTask As New Locator("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Locators/ESRI_Geocode_USA/GeocodeServer")
            AddHandler locatorTask.AddressToLocationsCompleted, AddressOf LocatorTask_AddressToLocationsCompleted
            AddHandler locatorTask.Failed, AddressOf LocatorTask_Failed
            Dim addressParams As New AddressToLocationsParameters()
            Dim addressDictionary As Dictionary(Of String, String) = addressParams.Address

            If (Not String.IsNullOrEmpty(State.Text)) Then
                addressDictionary.Add("Address", Address.Text)
            End If
            If (Not String.IsNullOrEmpty(City.Text)) Then
                addressDictionary.Add("City", City.Text)
            End If
            If (Not String.IsNullOrEmpty(State.Text)) Then
                addressDictionary.Add("State", State.Text)
            End If
            If (Not String.IsNullOrEmpty(Zip.Text)) Then
                addressDictionary.Add("Zip", Zip.Text)
            End If

            locatorTask.AddressToLocationsAsync(addressParams)
        End Sub

        Private Sub LocatorTask_AddressToLocationsCompleted(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.Tasks.AddressToLocationsEventArgs)
            Dim returnedCandidates As List(Of AddressCandidate) = args.Results

            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()

            _candidateList = New List(Of AddressCandidate)()
            Dim candidateListBox As New ListBox()

            For Each candidate As AddressCandidate In returnedCandidates
                If candidate.Score >= 65 Then
                    _candidateList.Add(candidate)
                    candidateListBox.Items.Add(candidate.Address)

                    Dim graphic As New Graphic() With {.Symbol = DefaultMarkerSymbol, .Geometry = candidate.Location}

                    graphic.Attributes.Add("Address", candidate.Address)

                    Dim latlon As String = String.Format("{0}, {1}", candidate.Location.X, candidate.Location.Y)
                    graphic.Attributes.Add("LatLon", latlon)

                    graphicsLayer.Graphics.Add(graphic)
                End If
            Next candidate

            AddHandler candidateListBox.SelectionChanged, AddressOf _candidateListBox_SelectionChanged

            CandidateScrollViewer.Content = candidateListBox
            CandidatePanelGrid.Visibility = Visibility.Visible

            Dim pt As MapPoint = _candidateList(0).Location
            If _firstZoom Then
                MyMap.ZoomToResolution(MyMap.Resolution / 4, pt)
                _firstZoom = False
            Else
                MyMap.PanTo(pt)
            End If

            _lastIndex = 0
            candidateListBox.SelectedIndex = 0
        End Sub

        Private Sub _candidateListBox_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            Dim listBox As ListBox = TryCast(sender, ListBox)
            Dim index As Integer = listBox.SelectedIndex
            If index >= 0 AndAlso _lastIndex <> index Then
                _lastIndex = index
                Dim candidate As AddressCandidate = _candidateList(index)
                MyMap.PanTo(candidate.Location)
            End If
        End Sub

        Private Sub LocatorTask_Failed(ByVal sender As Object, ByVal e As TaskFailedEventArgs)
            MessageBox.Show("Locator service failed: " & e.Error.Message)
        End Sub
    End Class
End Namespace
