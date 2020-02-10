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
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry
Imports ESRI.ArcGIS.Client.Tasks

Namespace ArcGISSilverlightSDK
    Partial Public Class Thematic_Interactive
        Inherits UserControl
        Private ThematicItemList As New List(Of ThematicItem)()
        Private ColorList As New List(Of List(Of SolidColorBrush))()
        Private _colorShadeIndex As Integer = 0
        Private _thematicListIndex As Integer = 0
        Private _featureSet As FeatureSet = Nothing
        Private _classType As Integer = 0 ' EqualInterval = 1; Quantile = 0;
        Private _classCount As Integer = 6
        Private _lastGeneratedClassCount As Integer = 0
        Private _legendGridCollapsed As Boolean
        Private _classGridCollapsed As Boolean

        Public Sub New()
            InitializeComponent()

            _legendGridCollapsed = False
            _classGridCollapsed = False

            AddHandler LegendCollapsedTriangle.MouseLeftButtonUp, AddressOf Triangle_MouseLeftButtonUp
            AddHandler LegendExpandedTriangle.MouseLeftButtonUp, AddressOf Triangle_MouseLeftButtonUp
            AddHandler ClassCollapsedTriangle.MouseLeftButtonUp, AddressOf Triangle_MouseLeftButtonUp
            AddHandler ClassExpandedTriangle.MouseLeftButtonUp, AddressOf Triangle_MouseLeftButtonUp

            ' Get start value for number of classifications in XAML.
            _lastGeneratedClassCount = Convert.ToInt32((CType(ClassCountCombo.SelectedItem, ComboBoxItem)).Content)

            ' Set query where clause to include features with an area greater than 70 square miles.  This 
            ' will effectively exclude the District of Columbia from attributes to avoid skewing classifications.
            Dim query As New ESRI.ArcGIS.Client.Tasks.Query() With {.Where = "SQMI > 70"}
            query.OutFields.Add("*")

            Dim queryTask As New QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Demographics/ESRI_Census_USA/MapServer/5")

            AddHandler queryTask.ExecuteCompleted, AddressOf HandleExecuteCompleted

            queryTask.ExecuteAsync(query)

            CreateColorList()
            CreateThematicList()
        End Sub

        Private Sub HandleExecuteCompleted(ByVal evtsender As Object, ByVal args As QueryEventArgs)
            If args.FeatureSet Is Nothing Then
                Return
            End If
            _featureSet = args.FeatureSet
            SetRangeValues()
            RenderButton.IsEnabled = True
        End Sub

        Public Structure ThematicItem
            Private privateName As String
            Public Property Name() As String
                Get
                    Return privateName
                End Get
                Set(ByVal value As String)
                    privateName = value
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
            Private privateCalcField As String
            Public Property CalcField() As String
                Get
                    Return privateCalcField
                End Get
                Set(ByVal value As String)
                    privateCalcField = value
                End Set
            End Property
            Private privateMin As Double
            Public Property Min() As Double
                Get
                    Return privateMin
                End Get
                Set(ByVal value As Double)
                    privateMin = value
                End Set
            End Property
            Private privateMax As Double
            Public Property Max() As Double
                Get
                    Return privateMax
                End Get
                Set(ByVal value As Double)
                    privateMax = value
                End Set
            End Property
            Private privateMinName As String
            Public Property MinName() As String
                Get
                    Return privateMinName
                End Get
                Set(ByVal value As String)
                    privateMinName = value
                End Set
            End Property
            Private privateMaxName As String
            Public Property MaxName() As String
                Get
                    Return privateMaxName
                End Get
                Set(ByVal value As String)
                    privateMaxName = value
                End Set
            End Property
            Private privateRangeStarts As List(Of Double)
            Public Property RangeStarts() As List(Of Double)
                Get
                    Return privateRangeStarts
                End Get
                Set(ByVal value As List(Of Double))
                    privateRangeStarts = value
                End Set
            End Property

        End Structure

        Private Sub CreateColorList()
            ColorList = New List(Of List(Of SolidColorBrush))()

            Dim BlueShades As New List(Of SolidColorBrush)()
            Dim RedShades As New List(Of SolidColorBrush)()
            Dim GreenShades As New List(Of SolidColorBrush)()
            Dim YellowShades As New List(Of SolidColorBrush)()
            Dim MagentaShades As New List(Of SolidColorBrush)()
            Dim CyanShades As New List(Of SolidColorBrush)()

            Dim rgbFactor As Integer = 255 \ _classCount

            For j As Integer = 0 To 255 Step rgbFactor
                BlueShades.Add(New SolidColorBrush(Color.FromArgb(192, CByte(j), CByte(j), 255)))
                RedShades.Add(New SolidColorBrush(Color.FromArgb(192, 255, CByte(j), CByte(j))))
                GreenShades.Add(New SolidColorBrush(Color.FromArgb(192, CByte(j), 255, CByte(j))))
                YellowShades.Add(New SolidColorBrush(Color.FromArgb(192, 255, 255, CByte(j))))
                MagentaShades.Add(New SolidColorBrush(Color.FromArgb(192, 255, CByte(j), 255)))
                CyanShades.Add(New SolidColorBrush(Color.FromArgb(192, CByte(j), 255, 255)))
            Next j

            ColorList.Add(BlueShades)
            ColorList.Add(RedShades)
            ColorList.Add(GreenShades)
            ColorList.Add(YellowShades)
            ColorList.Add(MagentaShades)
            ColorList.Add(CyanShades)

            For Each brushList As List(Of SolidColorBrush) In ColorList
                brushList.Reverse()
            Next brushList

            Dim MixedShades As New List(Of SolidColorBrush)()
            If _classCount > 5 Then
                MixedShades.Add(New SolidColorBrush(Color.FromArgb(192, 0, 255, 255)))
            End If
            If _classCount > 4 Then
                MixedShades.Add(New SolidColorBrush(Color.FromArgb(192, 255, 0, 255)))
            End If
            If _classCount > 3 Then
                MixedShades.Add(New SolidColorBrush(Color.FromArgb(192, 255, 255, 0)))
            End If
            MixedShades.Add(New SolidColorBrush(Color.FromArgb(192, 0, 255, 0)))
            MixedShades.Add(New SolidColorBrush(Color.FromArgb(192, 0, 0, 255)))
            MixedShades.Add(New SolidColorBrush(Color.FromArgb(192, 255, 0, 0)))
            ColorList.Add(MixedShades)

            _lastGeneratedClassCount = _classCount
        End Sub

        Private Sub CreateThematicList()
            ThematicItemList.Add(New ThematicItem() With {.Name = "POP2007", .Description = "2007 Population ", .CalcField = ""})
            ThematicItemList.Add(New ThematicItem() With {.Name = "POP07_SQMI", .Description = "2007 Pop per Sq Mi", .CalcField = ""})
            ThematicItemList.Add(New ThematicItem() With {.Name = "MALES", .Description = "%Males", .CalcField = "POP2007"})
            ThematicItemList.Add(New ThematicItem() With {.Name = "FEMALES", .Description = "%Females", .CalcField = "POP2007"})
            ThematicItemList.Add(New ThematicItem() With {.Name = "MED_AGE", .Description = "Median age", .CalcField = ""})
            ThematicItemList.Add(New ThematicItem() With {.Name = "SQMI", .Description = "Total SqMi", .CalcField = ""})
            For Each items As ThematicItem In ThematicItemList
                FieldCombo.Items.Add(items.Description)
            Next items
            FieldCombo.SelectedIndex = 0
        End Sub

        Private Sub SetRangeValues()
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)

            ' if necessary, update ColorList based on current number of classes.
            If _lastGeneratedClassCount <> _classCount Then
                CreateColorList()
            End If

            ' Field on which to generate a classification scheme.  
            Dim thematicItem As ThematicItem = ThematicItemList(_thematicListIndex)

            ' Calculate value for classification scheme
            Dim useCalculatedValue As Boolean = Not String.IsNullOrEmpty(thematicItem.CalcField)

            ' Store a list of values to classify
            Dim valueList As New List(Of Double)()

            ' Get range, min, max, etc. from features
            For i As Integer = 0 To _featureSet.Features.Count - 1
                Dim graphicFeature As Graphic = _featureSet.Features(i)

                Dim graphicValue As Double = Convert.ToDouble(graphicFeature.Attributes(thematicItem.Name))
                Dim graphicName As String = graphicFeature.Attributes("STATE_NAME").ToString()

                If useCalculatedValue Then
                    Dim calcVal As Double = Convert.ToDouble(graphicFeature.Attributes(thematicItem.CalcField))
                    graphicValue = Math.Round(graphicValue / calcVal * 100, 2)
                End If

                If i = 0 Then
                    thematicItem.Min = graphicValue
                    thematicItem.Max = graphicValue
                    thematicItem.MinName = graphicName
                    thematicItem.MaxName = graphicName
                Else
                    If graphicValue < thematicItem.Min Then
                        thematicItem.Min = graphicValue
                        thematicItem.MinName = graphicName
                    End If
                    If graphicValue > thematicItem.Max Then
                        thematicItem.Max = graphicValue
                        thematicItem.MaxName = graphicName
                    End If
                End If

                valueList.Add(graphicValue)
            Next i

            ' Set up range start values
            thematicItem.RangeStarts = New List(Of Double)()

            Dim totalRange As Double = thematicItem.Max - thematicItem.Min
            Dim portion As Double = totalRange / _classCount

            thematicItem.RangeStarts.Add(thematicItem.Min)
            Dim startRangeValue As Double = thematicItem.Min

            ' Equal Interval
            If _classType = 1 Then
                For i As Integer = 1 To _classCount - 1
                    startRangeValue += portion
                    thematicItem.RangeStarts.Add(startRangeValue)
                Next i
                ' Quantile
            Else
                ' Enumerator of all values in ascending order
                Dim valueEnumerator As IEnumerable(Of Double) = _
                 From aValue In valueList _
                 Order By aValue _
                 Select aValue

                Dim increment As Integer = Convert.ToInt32(Math.Ceiling(_featureSet.Features.Count / _classCount))
                For i As Integer = increment To valueList.Count - 1 Step increment
                    Dim value As Double = valueEnumerator.ElementAt(i)
                    thematicItem.RangeStarts.Add(value)
                Next i
            End If

            ' Create graphic features and set symbol using the class range which contains the value 
            Dim brushList As List(Of SolidColorBrush) = ColorList(_colorShadeIndex)
            If _featureSet IsNot Nothing AndAlso _featureSet.Features.Count > 0 Then
                ' Clear previous graphic features
                graphicsLayer.ClearGraphics()

                For i As Integer = 0 To _featureSet.Features.Count - 1
                    Dim graphicFeature As Graphic = _featureSet.Features(i)

                    Dim graphicValue As Double = Convert.ToDouble(graphicFeature.Attributes(thematicItem.Name))
                    If useCalculatedValue Then
                        Dim calcVal As Double = Convert.ToDouble(graphicFeature.Attributes(thematicItem.CalcField))
                        graphicValue = Math.Round(graphicValue / calcVal * 100, 2)
                    End If

                    Dim brushIndex As Integer = GetRangeIndex(graphicValue, thematicItem.RangeStarts)

                    Dim symbol As New SimpleFillSymbol() With {.Fill = brushList(brushIndex), .BorderBrush = New SolidColorBrush(Colors.Transparent), .BorderThickness = 1}

                    Dim graphic As New Graphic()
                    graphic.Geometry = graphicFeature.Geometry
                    graphic.Attributes.Add("Name", graphicFeature.Attributes("STATE_NAME").ToString())
                    graphic.Attributes.Add("Description", thematicItem.Description)
                    graphic.Attributes.Add("Value", graphicValue.ToString())
                    graphic.Symbol = symbol

                    graphicsLayer.Graphics.Add(graphic)
                Next i


                ' Create new legend with ranges and swatches.
                LegendStackPanel.Children.Clear()

                Dim legendList As New ListBox()
                LegendTitle.Text = thematicItem.Description

                For c As Integer = 0 To _classCount - 1
                    Dim swatchRect As New Rectangle() With {.Width = 20, .Height = 20, .Stroke = New SolidColorBrush(Colors.Black), .Fill = brushList(c)}

                    Dim classTextBlock As New TextBlock()

                    ' First classification
                    If c = 0 Then
                        classTextBlock.Text = String.Format("  Less than {0}", Math.Round(thematicItem.RangeStarts(1), 2))
                        ' Last classification
                    ElseIf c = _classCount - 1 Then
                        classTextBlock.Text = String.Format("  {0} and above", Math.Round(thematicItem.RangeStarts(c), 2))
                        ' Middle classifications
                    Else
                        classTextBlock.Text = String.Format("  {0} to {1}", Math.Round(thematicItem.RangeStarts(c), 2), Math.Round(thematicItem.RangeStarts(c + 1), 2))
                    End If

                    Dim classStackPanel As New StackPanel()
                    classStackPanel.Orientation = Orientation.Horizontal
                    classStackPanel.Children.Add(swatchRect)
                    classStackPanel.Children.Add(classTextBlock)

                    legendList.Items.Add(classStackPanel)
                Next c

                Dim minTextBlock As New TextBlock()
                Dim minStackPanel As New StackPanel()
                minStackPanel.Orientation = Orientation.Horizontal
                minTextBlock.Text = String.Format("Min: {0} ({1})", thematicItem.Min, thematicItem.MinName)
                minStackPanel.Children.Add(minTextBlock)
                legendList.Items.Add(minStackPanel)

                Dim maxTextBlock As New TextBlock()
                Dim maxStackPanel As New StackPanel()
                maxStackPanel.Orientation = Orientation.Horizontal
                maxTextBlock.Text = String.Format("Max: {0} ({1})", thematicItem.Max, thematicItem.MaxName)
                maxStackPanel.Children.Add(maxTextBlock)
                legendList.Items.Add(maxStackPanel)

                LegendStackPanel.Children.Add(legendList)
            End If
        End Sub

        Private Function GetRangeIndex(ByVal val As Double, ByVal ranges As List(Of Double)) As Integer
            Dim index As Integer = _classCount - 1
            For r As Integer = 0 To _classCount - 2
                If val >= ranges(r) AndAlso val < ranges(r + 1) Then
                    index = r
                End If
            Next r
            Return index
        End Function

        Public Structure Values
            Private privateName As String
            Public Property Name() As String
                Get
                    Return privateName
                End Get
                Set(ByVal value As String)
                    privateName = value
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
            Private privateValue As String
            Public Property Value() As String
                Get
                    Return privateValue
                End Get
                Set(ByVal value As String)
                    privateValue = value
                End Set
            End Property
        End Structure

        Private Sub ClassTypeCombo_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            If ClassTypeCombo IsNot Nothing Then
                _classType = ClassTypeCombo.SelectedIndex
            End If
        End Sub

        Private Sub ColorBlendCombo_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            If ColorBlendCombo IsNot Nothing Then
                _colorShadeIndex = ColorBlendCombo.SelectedIndex
            End If
        End Sub

        Private Sub ClassCountCombo_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            If ClassCountCombo IsNot Nothing Then
                Dim item As ComboBoxItem = TryCast(ClassCountCombo.SelectedItem, ComboBoxItem)
                _classCount = Convert.ToInt32(item.Content)
            End If
        End Sub

        Private Sub FieldCombo_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            If FieldCombo IsNot Nothing Then
                _thematicListIndex = FieldCombo.SelectedIndex
            End If
        End Sub

        Private Sub RenderButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            SetRangeValues()
        End Sub

        Private Sub Triangle_MouseLeftButtonUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Dim source As FrameworkElement = TryCast(sender, FrameworkElement)

            Select Case source.Name
                Case "ClassExpandedTriangle", "ClassCollapsedTriangle"
                    If _classGridCollapsed Then
                        ClassExpandedTriangle.Visibility = Visibility.Visible
                        ClassCollapsedTriangle.Visibility = Visibility.Collapsed
                        ClassCollapsedTitle.Visibility = Visibility.Collapsed
                        ClassGrid.Height = Double.NaN
                        ClassGrid.UpdateLayout()
                    Else
                        ClassCollapsedTriangle.Visibility = Visibility.Visible
                        ClassExpandedTriangle.Visibility = Visibility.Collapsed
                        ClassCollapsedTitle.Visibility = Visibility.Visible
                        ClassGrid.Height = 50
                    End If
                    _classGridCollapsed = Not _classGridCollapsed
                Case "LegendExpandedTriangle", "LegendCollapsedTriangle"
                    If _legendGridCollapsed Then
                        LegendExpandedTriangle.Visibility = Visibility.Visible
                        LegendCollapsedTriangle.Visibility = Visibility.Collapsed
                        LegendCollapsedTitle.Visibility = Visibility.Collapsed
                        LegendGrid.Height = Double.NaN
                        LegendGrid.UpdateLayout()
                    Else
                        LegendCollapsedTriangle.Visibility = Visibility.Visible
                        LegendExpandedTriangle.Visibility = Visibility.Collapsed
                        LegendCollapsedTitle.Visibility = Visibility.Visible
                        LegendGrid.Height = 50
                    End If
                    _legendGridCollapsed = Not _legendGridCollapsed
            End Select
        End Sub
    End Class
End Namespace
