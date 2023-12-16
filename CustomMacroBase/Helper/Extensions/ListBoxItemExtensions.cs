using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CustomMacroBase.Helper.Extensions
{
    public static class ListBoxItemExtensions
    {
        public static void AddAdorner(this ListBoxItem target, bool? ismoveup)
        {
            var layer = AdornerLayer.GetAdornerLayer(target);
            layer.Add(new ListBoxItemAdorner(target, ismoveup));
        }

        public static void RemoveAdorner(this ListBoxItem target)
        {
            var layer = AdornerLayer.GetAdornerLayer(target);
            if (layer != null && layer.GetAdorners(target) != null)
            {
                foreach (var item in layer.GetAdorners(target))
                {
                    layer.Remove(item);
                }
            }

        }
    }

    public class ListBoxItemAdorner : Adorner
    {
        bool? Flag = null;

        public ListBoxItemAdorner(UIElement adornedElement, bool? flag) : base(adornedElement)
        {
            Flag = flag;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Flag is null) return;

            Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);

            Point topLeft = new(adornedElementRect.TopLeft.X, adornedElementRect.TopLeft.Y);
            Point topRight = new(adornedElementRect.TopRight.X, adornedElementRect.TopRight.Y);
            Point bottomLeft = new(adornedElementRect.BottomLeft.X, adornedElementRect.BottomLeft.Y);
            Point bottomRight = new(adornedElementRect.BottomRight.X, adornedElementRect.BottomRight.Y);

            Pen renderPen = new Pen(new SolidColorBrush(Colors.Red), 1.0) { StartLineCap = PenLineCap.Square, EndLineCap = PenLineCap.Square };
            Pen renderPen2 = new Pen(new SolidColorBrush(Colors.Red), 3.0) { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round };

            // Draw a circle at each corner.
            if (Flag is true)
            {
                //drawingContext.DrawLine(renderPen, topLeft, new Point(topRight.X - topLeft.X, topLeft.Y));
                drawingContext.DrawLine(renderPen, topLeft.fix(0, 1), topRight.fix(0, 1));
                drawingContext.DrawLine(renderPen2, topLeft.fix(1, 1), topLeft.fix(1, 1));
            }
            else
            {
                drawingContext.DrawLine(renderPen, bottomLeft, bottomRight);
                drawingContext.DrawLine(renderPen2, bottomLeft.fix(1, 0), bottomLeft.fix(1, 0));
            }
        }
    }

    public static class PointExtensionsPrivate
    {
        public static Point fix(this Point target, double x, double y)
        {
            return new Point(target.X + x, target.Y + y);
        }
    }
}
