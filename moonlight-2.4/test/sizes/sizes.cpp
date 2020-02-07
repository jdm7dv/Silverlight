#define PRINT_SIZEOF(x) printf ("%d: %s\n", sizeof (x), #x)

#include "glib.h"
#include "gtk/gtk.h"
#include "libmoon.h"

int
main()
{
  PRINT_SIZEOF (KeySpline);
  PRINT_SIZEOF (AnimationClock);
  PRINT_SIZEOF (Animation/*Timeline*/);
  PRINT_SIZEOF (DoubleAnimation);
  PRINT_SIZEOF (ColorAnimation);
  PRINT_SIZEOF (PointAnimation);
  PRINT_SIZEOF (KeyFrame);
  PRINT_SIZEOF (KeyFrameCollection);
  PRINT_SIZEOF (ColorKeyFrameCollection);
  PRINT_SIZEOF (DoubleKeyFrameCollection);
  PRINT_SIZEOF (PointKeyFrameCollection);
  PRINT_SIZEOF (DoubleKeyFrame);
  PRINT_SIZEOF (ColorKeyFrame);
  PRINT_SIZEOF (PointKeyFrame);
  PRINT_SIZEOF (DiscreteDoubleKeyFrame);
  PRINT_SIZEOF (DiscreteColorKeyFrame);
  PRINT_SIZEOF (DiscretePointKeyFrame);
  PRINT_SIZEOF (LinearDoubleKeyFrame);
  PRINT_SIZEOF (LinearColorKeyFrame);
  PRINT_SIZEOF (LinearPointKeyFrame);
  PRINT_SIZEOF (SplineDoubleKeyFrame);
  PRINT_SIZEOF (SplineColorKeyFrame);
  PRINT_SIZEOF (SplinePointKeyFrame);
  PRINT_SIZEOF (DoubleAnimationUsingKeyFrames);
  PRINT_SIZEOF (ColorAnimationUsingKeyFrames);
  PRINT_SIZEOF (PointAnimationUsingKeyFrames);
  PRINT_SIZEOF (Storyboard);
  PRINT_SIZEOF (BeginStoryboard);
  PRINT_SIZEOF (Brush);
  PRINT_SIZEOF (SolidColorBrush);
  PRINT_SIZEOF (GradientBrush);
  PRINT_SIZEOF (TileBrush);
  PRINT_SIZEOF (ImageBrush);
  PRINT_SIZEOF (VideoBrush);
  PRINT_SIZEOF (LinearGradientBrush);
  PRINT_SIZEOF (RadialGradientBrush);
  PRINT_SIZEOF (GradientStopCollection);
  PRINT_SIZEOF (GradientStop);
  PRINT_SIZEOF (VisualBrush);
  PRINT_SIZEOF (Canvas);
  PRINT_SIZEOF (TimeManager);
  PRINT_SIZEOF (Clock);
  PRINT_SIZEOF (ClockGroup);
  PRINT_SIZEOF (Timeline);
  PRINT_SIZEOF (TimelineCollection);
  PRINT_SIZEOF (TimelineGroup);
  PRINT_SIZEOF (ParallelTimeline);
  PRINT_SIZEOF (TimelineMarker);
  PRINT_SIZEOF (Collection);
  PRINT_SIZEOF (UIElementCollection);
  PRINT_SIZEOF (TriggerCollection);
  PRINT_SIZEOF (TriggerActionCollection);
  PRINT_SIZEOF (ResourceDictionary);
  PRINT_SIZEOF (StrokeCollection);
  PRINT_SIZEOF (StylusPointCollection);
  PRINT_SIZEOF (TimelineMarkerCollection);
  PRINT_SIZEOF (MediaAttributeCollection);
  PRINT_SIZEOF (InlineCollection);
  PRINT_SIZEOF (Control);
  PRINT_SIZEOF (EventObject);
  PRINT_SIZEOF (DependencyObject);
  PRINT_SIZEOF (Downloader);
  PRINT_SIZEOF (ErrorEventArgs);
  PRINT_SIZEOF (ImageErrorEventArgs);
  PRINT_SIZEOF (ParserErrorEventArgs);
  PRINT_SIZEOF (FrameworkElement);
  PRINT_SIZEOF (Geometry);
  PRINT_SIZEOF (GeometryCollection);
  PRINT_SIZEOF (GeometryGroup);
  PRINT_SIZEOF (EllipseGeometry);
  PRINT_SIZEOF (LineGeometry);
  PRINT_SIZEOF (PathFigureCollection);
  PRINT_SIZEOF (PathGeometry);
  PRINT_SIZEOF (Rect);
  PRINT_SIZEOF (RectangleGeometry);
  PRINT_SIZEOF (PathSegmentCollection);
  PRINT_SIZEOF (PathFigure);
  PRINT_SIZEOF (PathSegment);
  PRINT_SIZEOF (ArcSegment);
  PRINT_SIZEOF (BezierSegment);
  PRINT_SIZEOF (LineSegment);
  PRINT_SIZEOF (PolyBezierSegment);
  PRINT_SIZEOF (PolyLineSegment);
  PRINT_SIZEOF (PolyQuadraticBezierSegment);
  PRINT_SIZEOF (QuadraticBezierSegment);
  PRINT_SIZEOF (MediaAttribute);
  PRINT_SIZEOF (MediaBase);
  PRINT_SIZEOF (Image);
  PRINT_SIZEOF (MediaElement);
  PRINT_SIZEOF (NameScope);
  PRINT_SIZEOF (Panel);
  PRINT_SIZEOF (Surface);
  PRINT_SIZEOF (Shape);
  PRINT_SIZEOF (Ellipse);
  PRINT_SIZEOF (Rectangle);
  PRINT_SIZEOF (Line);
  PRINT_SIZEOF (Polygon);
  PRINT_SIZEOF (Polyline);
  PRINT_SIZEOF (Path);
  PRINT_SIZEOF (StylusInfo);
  PRINT_SIZEOF (StylusPoint);
  PRINT_SIZEOF (Stroke);
  PRINT_SIZEOF (DrawingAttributes);
  PRINT_SIZEOF (InkPresenter);
  PRINT_SIZEOF (Inline);
  PRINT_SIZEOF (LineBreak);
  PRINT_SIZEOF (Run);
  PRINT_SIZEOF (TextBlock);
  PRINT_SIZEOF (Glyphs);
  PRINT_SIZEOF (Transform);
  PRINT_SIZEOF (RotateTransform);
  PRINT_SIZEOF (TranslateTransform);
  PRINT_SIZEOF (ScaleTransform);
  PRINT_SIZEOF (SkewTransform);
  PRINT_SIZEOF (MatrixTransform);
  PRINT_SIZEOF (TransformCollection);
  PRINT_SIZEOF (TransformGroup);
  PRINT_SIZEOF (TriggerAction);
  PRINT_SIZEOF (EventTrigger);
  PRINT_SIZEOF (UIElement);
}