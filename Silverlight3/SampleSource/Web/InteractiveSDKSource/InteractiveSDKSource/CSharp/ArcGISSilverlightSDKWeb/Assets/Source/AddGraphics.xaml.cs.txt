using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Geometry;
using System.Runtime.Serialization;

namespace ArcGISSilverlightSDK
{
    public partial class AddGraphics : UserControl
    {

        public AddGraphics()
        {
            InitializeComponent();

            AddMarkerGraphics();
            AddPictureMarkerAndTextGraphics();
            AddLineGraphics();
            AddPolygonGraphics();
        }

        void AddMarkerGraphics()
        {
            string jsonCoordinateString = "{'Coordinates':[{'X':13,'Y':55.59},{'X':72.83,'Y':18.97},{'X':55.43,'Y':34.3}]}";
            CustomCoordinateList coordinateList = DeserializeJson<CustomCoordinateList>(jsonCoordinateString);
            
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

            for (int i = 0; i < coordinateList.Coordinates.Count; i++)
            {
                Graphic graphic = new Graphic()
                    {
                        Geometry = new MapPoint(coordinateList.Coordinates[i].X, coordinateList.Coordinates[i].Y),
                        Symbol = i > 0 ? RedMarkerSymbol : BlackMarkerSymbol
                    };
                graphicsLayer.Graphics.Add(graphic);
            }

        }

        private void AddPictureMarkerAndTextGraphics()
        {
            string gpsNMEASentences = "$GPGGA, 92204.9, -35.6334, N, -60.2343, W, 1, 04, 2.4, 25.7, M,,,,*75\r\n" +
                                     "$GPGGA, 92510.5, -49.9334, N, -65.2131, W, 1, 04, 2.6, 1.7, M,,,,*75\r\n";
            string[] gpsNMEASentenceArray = gpsNMEASentences.Split('\n');

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

            for (int i = 0; i < gpsNMEASentenceArray.Length - 1; i++)
            {
                string[] gpsNMEASentence = gpsNMEASentenceArray[i].Split(',');

                Graphic graphic = new Graphic()
                {
                    Geometry = new MapPoint(Convert.ToDouble(gpsNMEASentence[4]), Convert.ToDouble(gpsNMEASentence[2])),
                    Symbol = GlobePictureSymbol
                };

                graphicsLayer.Graphics.Add(graphic);


                TextSymbol textSymbol = new TextSymbol()
                {
                    FontFamily = new System.Windows.Media.FontFamily("Arial"),
                    Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black),
                    FontSize = 10,
                    Text = gpsNMEASentence[9]
                };

                Graphic graphicText = new Graphic()
                {
                    Geometry = new MapPoint(Convert.ToDouble(gpsNMEASentence[4]), Convert.ToDouble(gpsNMEASentence[2])),
                    Symbol = textSymbol
                };

                graphicsLayer.Graphics.Add(graphicText);
            }
        }

        private void AddLineGraphics()
        {
            string geoRSSLine = @"<?xml version='1.0' encoding='utf-8'?>
                                    <feed xmlns='http://www.w3.org/2005/Atom' xmlns:georss='http://www.georss.org/georss'>
                                    <georss:line>-118.169, 34.016, -104.941, 39.7072, -96.724, 32.732</georss:line>
                                    <georss:line>-28.69, 14.16, -14.91, 23.702, -1.74, 13.72</georss:line>
                                </feed>";

            List<ESRI.ArcGIS.Client.Geometry.Polyline> polylineList = new List<ESRI.ArcGIS.Client.Geometry.Polyline>();

            using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(new System.IO.StringReader(geoRSSLine)))
            {
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case System.Xml.XmlNodeType.Element:
                            string nodeName = xmlReader.Name;
                            if (nodeName == "georss:line")
                            {
                                string lineString = xmlReader.ReadElementContentAsString();

                                string[] lineCoords = lineString.Split(',');

                                ESRI.ArcGIS.Client.Geometry.PointCollection pointCollection = new ESRI.ArcGIS.Client.Geometry.PointCollection();
                                for (int i = 0; i < lineCoords.Length; i += 2)
                                {
                                    pointCollection.Add(new MapPoint(Convert.ToDouble(lineCoords[i]), Convert.ToDouble(lineCoords[i + 1])));
                                }

                                ESRI.ArcGIS.Client.Geometry.Polyline polyline = new ESRI.ArcGIS.Client.Geometry.Polyline();
                                polyline.Paths.Add(pointCollection);

                                polylineList.Add(polyline);

                            }
                            break;
                    }
                }
            }

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

            foreach (ESRI.ArcGIS.Client.Geometry.Polyline polyline in polylineList)
            {
                Graphic graphic = new Graphic()
                {
                    Symbol = DefaultLineSymbol,
                    Geometry = polyline
                };

                graphicsLayer.Graphics.Add(graphic);
            }
        }


        private void AddPolygonGraphics()
        {
            string coordinateString1 = "130,5.59 118.42,3.92 117.3,23.3 143.2,22.9 130,5.59";

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

            PointCollectionConverter pointConverter = new PointCollectionConverter();
            ESRI.ArcGIS.Client.Geometry.PointCollection pointCollection1 =
                pointConverter.ConvertFromString(coordinateString1) as ESRI.ArcGIS.Client.Geometry.PointCollection;

            ESRI.ArcGIS.Client.Geometry.Polygon polygon1 = new ESRI.ArcGIS.Client.Geometry.Polygon();
            polygon1.Rings.Add(pointCollection1);

            Graphic graphic = new Graphic()
            {
                Geometry = polygon1,
                Symbol = DefaultFillSymbol
            };

            graphicsLayer.Graphics.Add(graphic);


            string coordinateString2 = "13,-5.59 18.42,-33.92 -43.23,-32.9 -45.3,6.6 13,-5.59";

            ESRI.ArcGIS.Client.Geometry.PointCollection pointCollection2 =
                pointConverter.ConvertFromString(coordinateString2) as ESRI.ArcGIS.Client.Geometry.PointCollection;

            ESRI.ArcGIS.Client.Geometry.Polygon polygon2 = new ESRI.ArcGIS.Client.Geometry.Polygon();
            polygon2.Rings.Add(pointCollection2);

            Graphic graphicVideo = new Graphic()
            {
                Geometry = polygon2,
                Symbol = VideoFillSymbol
            };

            graphicsLayer.Graphics.Add(graphicVideo);
        }


        internal static T DeserializeJson<T>(string json)
        {
            T objectInstance = Activator.CreateInstance<T>();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(System.Text.Encoding.Unicode.GetBytes(json));
            System.Runtime.Serialization.Json.DataContractJsonSerializer jsonSerializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(objectInstance.GetType());
            objectInstance = (T)jsonSerializer.ReadObject(memoryStream);
            memoryStream.Close();
            return objectInstance;
        }
    }

    [DataContract]
    public class CustomCoordinateList
    {
        [DataMember]
        public List<CustomCoordinate> Coordinates = new List<CustomCoordinate>();
    }

    [DataContract]
    public class CustomCoordinate
    {
        public CustomCoordinate() { }
        public CustomCoordinate(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
    }

}
