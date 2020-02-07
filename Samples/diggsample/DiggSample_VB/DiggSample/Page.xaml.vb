Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports System.Xml.Linq

Namespace DiggSample
	Partial Public Class Page
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub SearchBtn_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Retrieve Topic to Search for from WaterMarkTextBox
			Dim topic As String = txtSearchTopic.Text

			' Construct Digg REST URL
			Dim diggUrl As String = String.Format("http://services.digg.com/stories/topic/{0}?count=20&appkey=http%3A%2F%2Fscottgu.com", topic)

			' Initiate Async Network call to Digg
			Dim diggService As New WebClient()
			AddHandler diggService.DownloadStringCompleted, AddressOf DiggService_DownloadStoriesCompleted
			diggService.DownloadStringAsync(New Uri(diggUrl))
		End Sub

		Private Sub DiggService_DownloadStoriesCompleted(ByVal sender As Object, ByVal e As DownloadStringCompletedEventArgs)
			If e.Error Is Nothing Then
				DisplayStories(e.Result)
			End If
		End Sub

		Private Sub DisplayStories(ByVal xmlContent As String)
			Dim xmlStories As XDocument = XDocument.Parse(xmlContent)

            Dim stories = From story In xmlStories.Descendants("story") _
                          Where story.Element("thumbnail") IsNot Nothing AndAlso (Not story.Element("thumbnail").Attribute("src").Value.EndsWith(".gif")) _
                          Select New DiggStory With {.Id = CInt(Fix(story.Attribute("id").Value)), .Title = (CStr(story.Element("title"))).Trim(), .Description = (CStr(story.Element("description"))).Trim(), .ThumbNail = CStr(story.Element("thumbnail").Attribute("src").Value), .HrefLink = New Uri(CStr(story.Attribute("link").Value)), .NumDiggs = CInt(Fix(story.Attribute("diggs").Value)), .UserName = CStr(story.Element("user").Attribute("name").Value)}

			StoriesList.SelectedIndex = -1
			StoriesList.ItemsSource = stories
		End Sub

		Private Sub StoriesList_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			Dim story As DiggStory = CType(StoriesList.SelectedItem, DiggStory)

			If story IsNot Nothing Then
				DetailsView.DataContext = story
				DetailsView.Visibility = Visibility.Visible
			End If
		End Sub
	End Class
End Namespace

