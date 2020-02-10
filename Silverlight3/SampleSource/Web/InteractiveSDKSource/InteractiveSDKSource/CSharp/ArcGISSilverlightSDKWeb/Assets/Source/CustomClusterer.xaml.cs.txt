using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;

namespace ArcGISSilverlightSDK
{
    public partial class CustomClusterer : UserControl
    {
        public CustomClusterer()
        {
            InitializeComponent();
        }

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if (MyMap == null) return;
			GraphicsLayer layer = MyMap.Layers["MyFeatureLayer"] as GraphicsLayer;
			layer.Clusterer = new SumClusterer() { AggregateColumn = "POP1990", SymbolScale = 0.001 };
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			if (MyMap == null) return;
			GraphicsLayer layer = MyMap.Layers["MyFeatureLayer"] as GraphicsLayer;
			layer.Clusterer = null;
		}
    }

    public class SumClusterer : GraphicsClusterer
    {
        public SumClusterer()
        {
            MinimumColor = Colors.Red;
            MaximumColor = Colors.Yellow;
            SymbolScale = 1;
            base.Radius = 50;
        }

        public string AggregateColumn { get; set; }
        public double SymbolScale { get; set; }
        public Color MinimumColor { get; set; }
        public Color MaximumColor { get; set; }

        protected override Graphic OnCreateGraphic(GraphicCollection cluster, MapPoint point, int maxClusterCount)
        {
            if (cluster.Count == 1) return cluster[0];
            Graphic graphic = null;

            double sum = 0;

            foreach (Graphic g in cluster)
            {
                if (g.Attributes.ContainsKey(AggregateColumn))
                {
                    try
                    {
                        sum += Convert.ToDouble(g.Attributes[AggregateColumn]);
                    }
                    catch { }
                }
            }
            double size = (sum + 450) / 30; 
            size = (Math.Log(sum * SymbolScale / 10) * 10 + 20);
            if (size < 12) size = 12;
            graphic = new Graphic() { Symbol = new ClusterSymbol() { Size = size }, Geometry = point };
            graphic.Attributes.Add("Count", sum);
            graphic.Attributes.Add("Size", size);
            graphic.Attributes.Add("Color", InterpolateColor(size - 12, 100));
            return graphic;
        }

        private static Brush InterpolateColor(double value, double max)
        {
            value = (int)Math.Round(value * 255.0 / max);
            if (value > 255) value = 255; 
            else if (value < 0) value = 0;
            return new SolidColorBrush(Color.FromArgb(127, 255, (byte)value, 0));
        }
    }

    internal class ClusterSymbol : ESRI.ArcGIS.Client.Symbols.MarkerSymbol
    {
        public ClusterSymbol()
        {
            string template = "<ControlTemplate " +
            #if SILVERLIGHT
                "xmlns=\"http://schemas.microsoft.com/client/2007\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"" +
            #else
				"xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"" +
            #endif
            
            @" xmlns:esri=""clr-namespace:ESRI.ArcGIS.Client.ValueConverters;assembly=ESRI.ArcGIS.Client"">
				<Grid IsHitTestVisible=""False"">
					<Grid.Resources>" +
            #if SILVERLIGHT
                        "<esri:DictionaryConverter x:Name=\"MyDictionaryConverter\" />" +
            #else
						"<esri:DictionaryConverter x:Key=\"MyDictionaryConverter\" />" +
            #endif
 @"</Grid.Resources>
					<Ellipse
						Fill=""{Binding Path=Attributes, Converter={StaticResource MyDictionaryConverter}, ConverterParameter=Color, Mode=OneWay}"" 
						Width=""{Binding Path=Attributes, Converter={StaticResource MyDictionaryConverter}, ConverterParameter=Size, Mode=OneWay}""
						Height=""{Binding Path=Attributes, Converter={StaticResource MyDictionaryConverter}, ConverterParameter=Size, Mode=OneWay}"" />
					<Grid HorizontalAlignment=""Center"" VerticalAlignment=""Center"">
					<TextBlock 
						Text=""{Binding Path=Attributes, Converter={StaticResource MyDictionaryConverter}, ConverterParameter=Count, Mode=OneWay}"" 
						FontSize=""9"" Margin=""1,1,0,0"" FontWeight=""Bold""
						Foreground=""#99000000"" />
					<TextBlock
						Text=""{Binding Path=Attributes, Converter={StaticResource MyDictionaryConverter}, ConverterParameter=Count, Mode=OneWay}"" 
						FontSize=""9"" Margin=""0,0,1,1"" FontWeight=""Bold""
						Foreground=""White"" />
					</Grid>
				</Grid>
			</ControlTemplate>";
            #if SILVERLIGHT
                this.ControlTemplate = System.Windows.Markup.XamlReader.Load(template) as ControlTemplate;
            #else
			    MemoryStream templateStream = new MemoryStream(UTF8Encoding.Default.GetBytes(template));
			    ControlTemplate = System.Windows.Markup.XamlReader.Load(templateStream) as ControlTemplate;
            #endif
        }

        public double Size { get; set; }

        public override double OffsetX
        {
            get
            {
                return Size / 2;
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        public override double OffsetY
        {
            get
            {
                return Size / 2;
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}
