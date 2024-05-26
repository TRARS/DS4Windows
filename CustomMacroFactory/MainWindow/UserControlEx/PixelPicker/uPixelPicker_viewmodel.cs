using CustomMacroBase.CustomEffect;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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
    partial class uPixelPicker_viewmodel : NotificationObject
    {
        private uPixelPicker_model model = new();

        public Visibility Visibility
        {
            get { return model.Visibility; }
            set
            {
                model.Visibility = value;
                NotifyPropertyChanged();
            }
        }
        public UIElement CurrentScreenshot
        {
            get { return model.CurrentScreenshot; }
            set
            {
                model.CurrentScreenshot = value;
                NotifyPropertyChanged();
            }
        }
        public UIElement AdditionalInfo
        {
            get { return model.AdditionalInfo; }
            set
            {
                model.AdditionalInfo = value;
                NotifyPropertyChanged();
            }
        }
        public UIElement Magnifier
        {
            get { return model.Magnifier; }
            set
            {
                model.Magnifier = value;
                NotifyPropertyChanged();
            }
        }

        public uPixelPicker_viewmodel()
        {
            CreateImageControl();
            RegisterDelegate();
            HostEventInit();
            ViewModelInit();
        }
    }
    partial class uPixelPicker_viewmodel
    {
        private bool IsMouseOver { get; set; } = false;
        private bool IsMouseLeftDown { get; set; } = false;
        private Point GetMousePoint { get; set; } = new();
        private double LocalRatio = 1;
    }
    partial class uPixelPicker_viewmodel
    {
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
        public PixelDetail BGRA
        {
            get { return model.BGRA; }
            set
            {
                model.BGRA = value;
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
        public string MessageStr => (SizeInfo.Equals(string.Empty) ? "" : $" size{SizeInfo} {ClickPointInfo} {""}");
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

        //左_附加信息栏
        Border MessageBox = new()
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
        Border MagnifierBox = new()
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

        private void CreateImageControl()
        {
            //Image
            Image.SetBinding(Image.SourceProperty, new Binding(nameof(ImageSource)));
            Image.SetBinding(Image.MarginProperty, new Binding(nameof(ImageMargin)));

            //MessageBox
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
                MessageBox.Child = Grid;
            }

            //MagnifierBox
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
                MagnifierBox.Child = Grid;
            }
        }
    }
    //RegisterDelegate
    partial class uPixelPicker_viewmodel
    {
        private Stopwatch timer = Stopwatch.StartNew();

        private void RegisterDelegate()
        {
            MediatorAsync.Instance.Register(AsyncMessageType.AsyncSnapshot, async (para, token) => 
            {
                var source = await Task.Run(async () => 
                {
                    await Task.Yield();

                    if (para is SD.Bitmap bmp)
                    {
                        // 限制刷新频率
                        if (timer.Elapsed.TotalMilliseconds > 100)
                        {
                            timer.Restart();

                            using (bmp)
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
                                return bitmapImage;
                            }
                        }
                        else
                        {
                            bmp.Dispose();
                        }
                    }

                    return null;
                });

                if (source is not null)
                {
                    await Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var old = ImageSource as BitmapImage;
                        {
                            ImageSource = source;
                        }
                        old?.StreamSource?.Dispose();
                    });
                }

                return null;
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

        private Point old_click_pos = new(0, 0);

        private void HostEventInit()
        {
            Loaded = (s, e) =>
            {
                var host = (UserControl)s;
                host.Margin = new Thickness(1);
                host.MinHeight = 400;
                host.MaxHeight = host.ActualHeight;
                RenderOptions.SetBitmapScalingMode(host, BitmapScalingMode.Fant);
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
                    var click = GetColorInfo(e.GetPosition(Image));
                    {
                        if (click.IsSuccess is false)
                        {
                            old_click_pos = new(0, 0);
                            ClickPointInfo = $"{click.Info}";
                        }
                        else
                        {
                            ClickPointInfo = $"\n -> {click.Info} " + $"\n -> Rect<{click.GetBoundingRect(old_click_pos!)}> ";
                            old_click_pos = click.Point;
                        }
                    }
                }
            };
            MouseEnter = (s, e) => { IsMouseOver = true; LocalRatio = GetDeviceScaleFactor((dynamic)s); };
            MouseLeave = (s, e) => { IsMouseOver = IsMouseLeftDown = false; };
            MouseMove = (s, e) =>
            {
                if (IsMouseLeftDown && ImageSource != null)
                {
                    var p = ((Point)e.GetPosition((dynamic)s)).GetDiff(GetMousePoint);
                    ImageMargin = new Thickness(ImageMarginWhenLeftDown.Left + p.X, ImageMarginWhenLeftDown.Top + p.Y, 0, 0);
                }
                if (BGRA.Values is not null && !IsMouseLeftDown)
                {
                    var click = GetColorInfo(e.GetPosition(Image));
                    if (click.IsSuccess) { Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, click.Info); }
                }

                { //放大镜
                    Point p = e.GetPosition((dynamic)s);
                    double rect_width = 128;
                    ViewboxRect = new Rect(p.X - rect_width / 2 + 1, p.Y - rect_width / 2 + 1, rect_width, rect_width);
                }
            };
        }
    }
    //ViewModelInit
    partial class uPixelPicker_viewmodel
    {
        private void ViewModelInit()
        {
            this.CurrentScreenshot = Image;
            this.AdditionalInfo = MessageBox;
            this.Magnifier = MagnifierBox;
        }
    }

    //Method
    partial class uPixelPicker_viewmodel
    {
        private double GetDeviceScaleFactor(UIElement para)
        {
            return 1 / PresentationSource.FromVisual(para).CompositionTarget.TransformToDevice.M11;
        }

        private ClickInfo GetColorInfo(Point p)
        {
            if (BGRA.Values is null) { return new(); }

            var str = string.Empty;
            var pt = new Point((int)(p.X / LocalRatio), (int)(p.Y / LocalRatio));
            var ptr = ((int)pt.Y * BGRA.Stride + (int)pt.X) << 2;
            if (pt.X >= 0 && pt.Y >= 0 && pt.X < BGRA.Stride && ptr < BGRA.Values.Length)
            {
                str = $"{BGRA.GetHexRGB(ptr)}";
                str = $"Click<{pt}> (RGB)_({str})";

                return new(true, str, pt);
            }

            return new();
        }
    }
}
