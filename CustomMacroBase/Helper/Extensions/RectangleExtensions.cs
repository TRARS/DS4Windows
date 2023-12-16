using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomMacroBase.Helper.Extensions
{
    public static class RectangleExtensions
    {
        public static void AddAdorner(this Rectangle target)
        {
            var layer = AdornerLayer.GetAdornerLayer(target);
            layer.Add(new TestAdorner(target));
        }

        public static void RemoveAdorner(this Rectangle target)
        {
            var layer = AdornerLayer.GetAdornerLayer(target);
            if (layer != null && layer.GetAdorners(target) != null)
            {
                foreach (var item in layer.GetAdorners(target))
                {
                    if (item is not null) layer.Remove(item);
                }
            }
        }
    }

    public class TestAdorner : Adorner
    {
        public TestAdorner(UIElement adornedElement) : base(adornedElement) { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);

            Pen renderPen = new Pen(new SolidColorBrush(Colors.Red), 1.0) { StartLineCap = PenLineCap.Square, EndLineCap = PenLineCap.Square };

            // Draw a circle at each corner.
            drawingContext.DrawLine(renderPen, new Point(adornedElementRect.TopLeft.X - 3, adornedElementRect.TopLeft.Y - 3), new Point(adornedElementRect.TopLeft.X + 5, adornedElementRect.TopLeft.Y - 3));
            drawingContext.DrawLine(renderPen, new Point(adornedElementRect.TopLeft.X - 3, adornedElementRect.TopLeft.Y - 3), new Point(adornedElementRect.TopLeft.X - 3, adornedElementRect.TopLeft.Y + 5));

            drawingContext.DrawLine(renderPen, new Point(adornedElementRect.TopRight.X + 3, adornedElementRect.TopRight.Y - 3), new Point(adornedElementRect.TopRight.X - 5, adornedElementRect.TopRight.Y - 3));
            drawingContext.DrawLine(renderPen, new Point(adornedElementRect.TopRight.X + 3, adornedElementRect.TopRight.Y - 3), new Point(adornedElementRect.TopRight.X + 3, adornedElementRect.TopRight.Y + 5));

            drawingContext.DrawLine(renderPen, new Point(adornedElementRect.BottomLeft.X - 3, adornedElementRect.BottomLeft.Y + 3), new Point(adornedElementRect.BottomLeft.X + 5, adornedElementRect.BottomLeft.Y + 3));
            drawingContext.DrawLine(renderPen, new Point(adornedElementRect.BottomLeft.X - 3, adornedElementRect.BottomLeft.Y + 3), new Point(adornedElementRect.BottomLeft.X - 3, adornedElementRect.BottomLeft.Y - 5));

            drawingContext.DrawLine(renderPen, new Point(adornedElementRect.BottomRight.X + 3, adornedElementRect.BottomRight.Y + 3), new Point(adornedElementRect.BottomRight.X - 5, adornedElementRect.BottomRight.Y + 3));
            drawingContext.DrawLine(renderPen, new Point(adornedElementRect.BottomRight.X + 3, adornedElementRect.BottomRight.Y + 3), new Point(adornedElementRect.BottomRight.X + 3, adornedElementRect.BottomRight.Y - 5));
        }

    }
}
