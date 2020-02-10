Imports Microsoft.VisualBasic
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports ESRI.ArcGIS.Client.ValueConverters
Imports ESRI.ArcGIS.Client.Tasks
Imports ESRI.ArcGIS.Client
Imports System.Collections.Generic

Namespace ArcGISSilverlightSDK
    Partial Public Class QueryWithoutMap
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub QueryButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim queryTask As New QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Demographics/ESRI_Census_USA/MapServer/5")
            AddHandler queryTask.ExecuteCompleted, AddressOf QueryTask_ExecuteCompleted
            AddHandler queryTask.Failed, AddressOf QueryTask_Failed

            Dim query As New ESRI.ArcGIS.Client.Tasks.Query()
            query.Text = StateNameTextBox.Text

            query.OutFields.Add("*")
            queryTask.ExecuteAsync(query)
        End Sub

        Private Sub QueryTask_ExecuteCompleted(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.Tasks.QueryEventArgs)
            Dim featureSet As FeatureSet = args.FeatureSet

            If featureSet IsNot Nothing AndAlso featureSet.Features.Count > 0 Then
                Dim resultList As New List(Of QueryResultData)()
                For Each feature As Graphic In featureSet.Features
                    resultList.Add(New QueryResultData() With {.STATE_NAME = feature.Attributes("STATE_NAME").ToString(), .SUB_REGION = feature.Attributes("SUB_REGION").ToString(), .STATE_FIPS = feature.Attributes("STATE_FIPS").ToString(), .STATE_ABBR = feature.Attributes("STATE_ABBR").ToString(), .POP2000 = feature.Attributes("POP2000").ToString(), .POP2007 = feature.Attributes("POP2007").ToString()})
                Next feature
                QueryDetailsDataGrid.ItemsSource = resultList
            Else
                MessageBox.Show("No features returned from query")
            End If
        End Sub


        Private Sub QueryTask_Failed(ByVal sender As Object, ByVal args As TaskFailedEventArgs)
            MessageBox.Show("Query execute error: " & args.Error.Message)
        End Sub

        Public Class QueryResultData
            Private privateSTATE_NAME As String
            Public Property STATE_NAME() As String
                Get
                    Return privateSTATE_NAME
                End Get
                Set(ByVal value As String)
                    privateSTATE_NAME = value
                End Set
            End Property
            Private privateSUB_REGION As String
            Public Property SUB_REGION() As String
                Get
                    Return privateSUB_REGION
                End Get
                Set(ByVal value As String)
                    privateSUB_REGION = value
                End Set
            End Property
            Private privateSTATE_FIPS As String
            Public Property STATE_FIPS() As String
                Get
                    Return privateSTATE_FIPS
                End Get
                Set(ByVal value As String)
                    privateSTATE_FIPS = value
                End Set
            End Property
            Private privateSTATE_ABBR As String
            Public Property STATE_ABBR() As String
                Get
                    Return privateSTATE_ABBR
                End Get
                Set(ByVal value As String)
                    privateSTATE_ABBR = value
                End Set
            End Property
            Private privatePOP2000 As String
            Public Property POP2000() As String
                Get
                    Return privatePOP2000
                End Get
                Set(ByVal value As String)
                    privatePOP2000 = value
                End Set
            End Property
            Private privatePOP2007 As String
            Public Property POP2007() As String
                Get
                    Return privatePOP2007
                End Get
                Set(ByVal value As String)
                    privatePOP2007 = value
                End Set
            End Property
        End Class
    End Class
End Namespace

