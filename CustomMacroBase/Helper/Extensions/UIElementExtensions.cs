using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CustomMacroBase.Helper.Extensions
{
    public static partial class UIElementExtensions
    {
        public static void Nothing(this UIElement target, string str)
        {
            MessageBox.Show($"{target}");
        }
    }

    public static partial class UIElementExtensions
    {
        private static Drawing DrawMyText(UIElement target, SolidColorBrush brush, string textString, double emSize, double margin)
        {
            dynamic obj = target;

            // Create a new DrawingGroup of the control.
            DrawingGroup drawingGroup = new DrawingGroup();

            // Open the DrawingGroup in order to access the DrawingContext.

            for (int i = (int)emSize; i > 1; i--)
            {
                // Create the formatted text based on the properties set.
                FormattedText formattedText;
                Size TextRealSize;

                using (DrawingContext drawingContext = drawingGroup.Open())
                {
                    formattedText = new FormattedText(textString,
                                                      CultureInfo.GetCultureInfo("en-us"),
                                                      FlowDirection.LeftToRight,
                                                      new Typeface(new FontFamily("Comic Sans MS Bold, Verdana Bold, Arial Bold"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                                      i,
                                                      Brushes.Black,
                                                      VisualTreeHelper.GetDpi(target).PixelsPerDip);

                    TextRealSize = new Size(formattedText.Width, formattedText.Height);
                    Point fix = new Point((obj.Width - TextRealSize.Width) / 2, (obj.Height - TextRealSize.Height) / 2);

                    // Build the geometry object that represents the text.
                    Geometry textGeometry = formattedText.BuildGeometry(fix);

                    // Draw a rounded rectangle under the text that is slightly larger than the text.
                    drawingContext.DrawRoundedRectangle(brush, null, new Rect(new Size(obj.Width, obj.Height)), 0, 0);

                    // Draw the outline based on the properties that are set.
                    drawingContext.DrawGeometry(Brushes.Gold, new Pen(Brushes.Maroon, 1.5), textGeometry);

                    // Return the updated DrawingGroup content to be used by the control.
                    //return drawingGroup;

                    if ((TextRealSize.Width + margin < obj.Width) && (TextRealSize.Height + margin < obj.Height)) { return drawingGroup; }
                }
            }

            return null;
        }

        public static void DrawTextToFill(this UIElement target, string str, Color color = default, double fontSize = 40, double margin = 5)
        {
            dynamic obj = target;
            dynamic brush = new SolidColorBrush(color == (Color)ColorConverter.ConvertFromString("#00000000") ? Colors.DarkGray : color);
            obj.Fill = new DrawingBrush(DrawMyText(target, brush, str, fontSize, margin));
        }
        public static void DrawTextToBackground(this UIElement target, string str, Color color = default, double fontSize = 40, double margin = 5)
        {
            dynamic obj = target;
            dynamic brush = new SolidColorBrush(color == (Color)ColorConverter.ConvertFromString("#00000000") ? Colors.DarkGray : color);
            obj.Background = new DrawingBrush(DrawMyText(target, brush, str, fontSize, margin));
        }
    }

    public static partial class UIElementExtensions
    {
        public static Storyboard SetDoubleAnimation(this UIElement target, DependencyProperty dp, double from, double to, double duration, IEasingFunction? easing = null, FillBehavior behavior = FillBehavior.HoldEnd)
        {
            // 创建DoubleAnimation对象
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from; // 起始值
            animation.To = to; // 终止值
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(duration)); // 动画持续时间
            animation.FillBehavior = behavior;
            animation.EasingFunction = easing;

            // 创建Storyboard对象
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation); // 将动画添加到Storyboard中

            // 将动画关联到指定依赖属性
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(dp));

            return storyboard;
        }
    }
}
