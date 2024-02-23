using CustomMacroBase.CustomEffect;
using CustomMacroBase.Helper;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SD = System.Drawing;

namespace CustomMacroFactory.MainWindow.UserControlEx.PixelPicker
{
    //绑定用的属性？
    partial class uPixelPicker_viewmodel : NotificationObject
    {
        private uPixelPicker_model model = new();

        public ImageSource? ImageSource
        {
            get { return model.ImageSource; }
            set
            {
                model.ImageSource = value;
                NotifyPropertyChanged();
            }
        }
        public Thickness ImageMargin
        {
            get { return model.ImageMargin; }
            set
            {
                model.ImageMargin = value;
                NotifyPropertyChanged();
            }
        }
        public Thickness ImageMarginWhenLeftDown
        {
            get { return model.ImageMarginWhenLeftDown; }
            set
            {
                model.ImageMarginWhenLeftDown = value;
                NotifyPropertyChanged();
            }
        }
        public uPixelPicker_model.PixelDetail BGRA
        {
            get { return model.BGRA; }
            set
            {
                model.BGRA = value;
                NotifyPropertyChanged();
            }
        }
        public Visibility Visibility
        {
            get { return model.Visibility; }
            set
            {
                model.Visibility = value;
                NotifyPropertyChanged();
            }
        }
        public Rect ViewboxRect
        {
            get { return model.ViewboxRect; }
            set
            {
                model.ViewboxRect = value;
                NotifyPropertyChanged();
            }
        }

        public string SizeInfo
        {
            get { return model.SizeInfo; }
            set
            {
                model.SizeInfo = value;
                NotifyPropertyChanged(); NotifyPropertyChanged("MessageStr");
            }
        }
        public string ClickPointInfo
        {
            get { return model.ClickPointInfo; }
            set
            {
                model.ClickPointInfo = value;
                NotifyPropertyChanged(); NotifyPropertyChanged("MessageStr");
            }
        }
        public string MessageStr => (SizeInfo.Equals(string.Empty) ? "" : $" size{SizeInfo} {(ClickPointInfo.Equals(string.Empty) ? "" : $"-> {ClickPointInfo}")} {""}");
    }
    //直接操作的内部属性
    partial class uPixelPicker_viewmodel
    {
        private bool IsMouseO2ver { get; set; } = false;
        private bool IsMouseOver { get; set; } = false;
        private bool IsMouseLeftDown { get; set; } = false;
        private Point GetMousePoint { get; set; } = new();
        private double LocalRatio = 1;
    }

    //些许方法
    partial class uPixelPicker_viewmodel
    {
        private Func<Point, Point, Point> GetPointDiff = (p1, p2) =>
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        };
        private Func<UIElement, double> GetMultiple = (UIElement para) =>
        {
            return 1 / PresentationSource.FromVisual(para).CompositionTarget.TransformToDevice.M11;
        };
    }

    //构造函数
    partial class uPixelPicker_viewmodel
    {
        public uPixelPicker_viewmodel()
        {
            CreateImageControl();
            RegisterDelegate();
            HostEventInit();
        }
    }
    //CreateImageControl
    partial class uPixelPicker_viewmodel
    {
        //位图容器
        Image Image = new()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Stretch = Stretch.None
        };
        //红框示意
        Border RedBox = new()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush(Colors.Red),
            Background = new SolidColorBrush(Colors.Transparent),
            Width = 10,
            Height = 10,
        };

        //左_附加信息栏
        Border MsgBorder = new()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(1),
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush(Colors.Transparent),
            Background = new SolidColorBrush(Colors.Transparent),
            Width = double.NaN,
            Height = double.NaN,
        };

        //右_放大镜外边框
        Border BoxBorder = new()
        {
            UseLayoutRounding = true,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(1),
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush(Colors.Red),
            Background = new SolidColorBrush(Colors.DarkGray),
            Width = 128,
            Height = 128,
        };


        //
        Grid myGrid = new Grid()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        private void CreateImageControl()
        {
            //Image
            Image.SetBinding(Image.SourceProperty, new Binding("ImageSource") { Source = this });
            Image.SetBinding(Image.MarginProperty, new Binding("ImageMargin") { Source = this });
            //


            //MsgBorder
            {
                Grid Grid = new();
                {
                    TextBlock tb = new()
                    {
                        Margin = new(1, 0, 0, 0),
                        Foreground = new SolidColorBrush(Colors.Snow),
                        Background = new SolidColorBrush(Colors.SlateGray)
                    };
                    BindingOperations.SetBinding(tb, TextBlock.TextProperty, new Binding(nameof(MessageStr)) { Source = this });

                    Grid.Children.Add(tb);
                }
                MsgBorder.Child = Grid;
            }

            //BoxBorder
            {
                Grid Grid = new();
                {
                    Rectangle rect = new() { Effect = new MagnifyEffect() { MagnificationAmount = 16 } };
                    {
                        VisualBrush vb = new()
                        {
                            Visual = Image,//
                            ViewboxUnits = BrushMappingMode.Absolute,
                            AlignmentX = AlignmentX.Left,
                            AlignmentY = AlignmentY.Top,
                            Viewport = new Rect(0, 0, 1, 1),
                            Stretch = Stretch.None
                        };
                        BindingOperations.SetBinding(vb, VisualBrush.ViewboxProperty, new Binding(nameof(ViewboxRect)) { Source = this });
                        rect.Fill = vb;
                    }

                    Grid reticle = new();
                    {
                        Border box_white = new()
                        {
                            Width = 9,
                            Height = 9,
                            BorderBrush = new SolidColorBrush(Colors.White),
                            BorderThickness = new Thickness(1),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        Border box_black = new()
                        {
                            Width = 11,
                            Height = 11,
                            BorderBrush = new SolidColorBrush(Colors.Black),
                            BorderThickness = new Thickness(1),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        Rectangle vl = new() { Width = 1, Fill = new SolidColorBrush(Colors.Red), HorizontalAlignment = HorizontalAlignment.Center };
                        Rectangle hl = new() { Height = 1, Fill = new SolidColorBrush(Colors.Red), VerticalAlignment = VerticalAlignment.Center };
                        {//十字线遮罩
                            LinearGradientBrush lgb = new() { StartPoint = new(0, 0), EndPoint = new(1, 0) };
                            lgb.GradientStops.Add(new() { Offset = 0, Color = Colors.Black });
                            lgb.GradientStops.Add(new() { Offset = 0.46875, Color = Colors.Black });
                            lgb.GradientStops.Add(new() { Offset = 0.46875, Color = Colors.Transparent });
                            lgb.GradientStops.Add(new() { Offset = 0.53125, Color = Colors.Transparent });
                            lgb.GradientStops.Add(new() { Offset = 0.53125, Color = Colors.Black });
                            lgb.GradientStops.Add(new() { Offset = 1, Color = Colors.Black });
                            hl.OpacityMask = lgb;

                            LinearGradientBrush lgb2 = new() { StartPoint = new(0, 0), EndPoint = new(0, 1) };
                            lgb2.GradientStops.Add(new() { Offset = 0, Color = Colors.Black });
                            lgb2.GradientStops.Add(new() { Offset = 0.46875, Color = Colors.Black });
                            lgb2.GradientStops.Add(new() { Offset = 0.46875, Color = Colors.Transparent });
                            lgb2.GradientStops.Add(new() { Offset = 0.53125, Color = Colors.Transparent });
                            lgb2.GradientStops.Add(new() { Offset = 0.53125, Color = Colors.Black });
                            lgb2.GradientStops.Add(new() { Offset = 1, Color = Colors.Black });
                            vl.OpacityMask = lgb2;
                        }

                        reticle.Children.Add(vl);
                        reticle.Children.Add(hl);
                        reticle.Children.Add(box_white);
                        reticle.Children.Add(box_black);
                    }

                    //
                    Grid.Children.Add(rect);
                    Grid.Children.Add(reticle);
                }
                BoxBorder.Child = Grid;
            }

            //左右结构
            {
                myGrid.ColumnDefinitions.Add(new() { Width = new(1.0, GridUnitType.Star) });
                myGrid.ColumnDefinitions.Add(new() { Width = new(1.0, GridUnitType.Auto) });

                Grid.SetColumn(MsgBorder, 0); Grid.SetColumn(BoxBorder, 1);

                myGrid.Children.Add(MsgBorder);
                myGrid.Children.Add(BoxBorder);
            }

        }
        private void AddWhenHostLoaded(dynamic? host)
        {
            host?.Children.Add(Image);
            //host?.Children.Add(RedBox);
            host?.Children.Add(myGrid);//Magnify
        }
    }
    //RegisterDelegate
    partial class uPixelPicker_viewmodel
    {
        private void RegisterDelegate()
        {
            Mediator.Instance.Register(MessageType.GetFrame, (para) =>
            {
                if (para is not null)
                {
                    using (var bmp = para as SD.Bitmap)
                    {
                        SD.Imaging.BitmapData data = bmp.LockBits(new SD.Rectangle(0, 0, bmp.Width, bmp.Height), SD.Imaging.ImageLockMode.ReadWrite, SD.Imaging.PixelFormat.Format32bppArgb);
                        IntPtr ptr = data.Scan0;
                        int bytes = Math.Abs(data.Stride) * bmp.Height;//w*h*4

                        SizeInfo = $"({bmp.Width},{bmp.Height})";

                        BGRA.Stride = data.Width;
                        BGRA.Values = new byte[bytes];
                        System.Runtime.InteropServices.Marshal.Copy(ptr, BGRA.Values, 0, bytes);
                        bmp.UnlockBits(data);

                        var bitmapImage = new BitmapImage();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmp.Save(ms, SD.Imaging.ImageFormat.Png);
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = ms;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            bitmapImage.Freeze();
                        }
                        ImageSource = bitmapImage;
                    }
                }
            });

            Mediator.Instance.Register(MessageType.PixelPickerOnOff, (_) =>
            {
                this.Visibility = (this.Visibility is Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                Mediator.Instance.NotifyColleagues(MessageType.CanUpdateFrames, this.Visibility == Visibility.Visible);
            });

            Mediator.Instance.Register(MessageType.WindowKeyDown, (para) =>
            {
                if (!IsMouseOver || BGRA.Values is null || IsMouseLeftDown || this.Visibility != Visibility.Visible) { return; }

                double step = LocalRatio;

                if (((Key)para == Key.W) || ((Key)para == Key.Up))
                {
                    ImageMargin = new Thickness(ImageMargin.Left, ImageMargin.Top + step, 0, 0);
                }
                if (((Key)para == Key.A) || ((Key)para == Key.Left))
                {
                    ImageMargin = new Thickness(ImageMargin.Left + step, ImageMargin.Top, 0, 0);
                }
                if (((Key)para == Key.S) || ((Key)para == Key.Down))
                {
                    ImageMargin = new Thickness(ImageMargin.Left, ImageMargin.Top - step, 0, 0);
                }
                if (((Key)para == Key.D) || ((Key)para == Key.Right))
                {
                    ImageMargin = new Thickness(ImageMargin.Left - step, ImageMargin.Top, 0, 0);
                }
            });
        }
    }
    //HostEventInit
    partial class uPixelPicker_viewmodel
    {
        public Action<object, RoutedEventArgs>? Loaded;
        public Action<object, MouseButtonEventArgs>? PreviewMouseRightButtonUp;
        public Action<object, MouseButtonEventArgs>? PreviewMouseLeftButtonDown;
        public Action<object, MouseButtonEventArgs>? PreviewMouseLeftButtonUp;
        public Action<object, MouseEventArgs>? MouseEnter;
        public Action<object, MouseEventArgs>? MouseLeave;
        public Action<object, MouseEventArgs>? MouseMove;

        private void HostEventInit()
        {
            Loaded = (s, e) =>
            {
                var host = (UserControl)s;
                host.Margin = new Thickness(1);
                host.MinHeight = 400;
                host.MaxHeight = host.ActualHeight;
                RenderOptions.SetBitmapScalingMode(host, BitmapScalingMode.Fant);

                AddWhenHostLoaded(host.Content);
            };
            PreviewMouseRightButtonUp = (s, e) => { ImageMargin = new(0); };
            PreviewMouseLeftButtonDown = (s, e) =>
            {
                IsMouseLeftDown = true;
                GetMousePoint = e.GetPosition((dynamic)s);
                ImageMarginWhenLeftDown = ImageMargin;
            };
            PreviewMouseLeftButtonUp = (s, e) =>
            {
                IsMouseLeftDown = false;
                if (BGRA.Values is not null)
                {
                    ClickPointInfo = $"{GetColorInfo(e.GetPosition(Image))}";
                }
            };
            MouseEnter = (s, e) => { IsMouseOver = true; LocalRatio = GetMultiple((dynamic)s); };
            MouseLeave = (s, e) => { IsMouseOver = IsMouseLeftDown = false; };
            MouseMove = (s, e) =>
            {
                if (IsMouseLeftDown && ImageSource != null)
                {
                    var p = GetPointDiff(e.GetPosition((dynamic)s), GetMousePoint);
                    ImageMargin = new Thickness(ImageMarginWhenLeftDown.Left + p.X, ImageMarginWhenLeftDown.Top + p.Y, 0, 0);
                }
                if (BGRA.Values is not null && !IsMouseLeftDown)
                {
                    var str = GetColorInfo(e.GetPosition(Image));
                    if (string.IsNullOrWhiteSpace(str) is false) { Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str); }
                }

                { //放大镜
                    Point p = e.GetPosition((dynamic)s);
                    double rect_width = 128;
                    ViewboxRect = new Rect(p.X - rect_width / 2 + 1, p.Y - rect_width / 2 + 1, rect_width, rect_width);
                }
            };
        }
        private string GetColorInfo(Point p)
        {
            string result = string.Empty;
            var pt = new Point((int)(p.X / LocalRatio), (int)(p.Y / LocalRatio));
            var ptr = ((int)pt.Y * BGRA.Stride + (int)pt.X) << 2;
            if (pt.X >= 0 && pt.Y >= 0 && pt.X < BGRA.Stride && ptr < BGRA.Values.Length)
            {
                result = $"{BGRA.Values[ptr + 2].ToString("X2")},{BGRA.Values[ptr + 1].ToString("X2")},{BGRA.Values[ptr + 0].ToString("X2")}";
                result = $"({pt})_(RGB)_({result})";
            }
            return result;
        }
    }
}
