Imports Microsoft.VisualBasic
Imports System

Namespace DiggSample
	Public Class DiggStory
		Private privateId As Integer
		Public Property Id() As Integer
			Get
				Return privateId
			End Get
			Set(ByVal value As Integer)
				privateId = value
			End Set
		End Property
		Private privateTitle As String
		Public Property Title() As String
			Get
				Return privateTitle
			End Get
			Set(ByVal value As String)
				privateTitle = value
			End Set
		End Property
		Private privateDescription As String
		Public Property Description() As String
			Get
				Return privateDescription
			End Get
			Set(ByVal value As String)
				privateDescription = value
			End Set
		End Property
		Private privateNumDiggs As Integer
		Public Property NumDiggs() As Integer
			Get
				Return privateNumDiggs
			End Get
			Set(ByVal value As Integer)
				privateNumDiggs = value
			End Set
		End Property
		Private privateHrefLink As Uri
		Public Property HrefLink() As Uri
			Get
				Return privateHrefLink
			End Get
			Set(ByVal value As Uri)
				privateHrefLink = value
			End Set
		End Property
		Private privateThumbNail As String
		Public Property ThumbNail() As String
			Get
				Return privateThumbNail
			End Get
			Set(ByVal value As String)
				privateThumbNail = value
			End Set
		End Property
		Private privateUserName As String
		Public Property UserName() As String
			Get
				Return privateUserName
			End Get
			Set(ByVal value As String)
				privateUserName = value
			End Set
		End Property
	End Class
End Namespace


