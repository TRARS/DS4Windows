using CustomMacroBase.CustomEffect;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.Extensions;
using CustomMacroBase.PixelMatcher;
using CustomMacroFactory.MainView.Interfaces;
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

namespace CustomMacroFactory.MainView.UserControlEx.PixelPicker
{
    public partial class uPixelPicker_viewmodel : NotificationObject, IViewModel
    {
        private readonly uPixelPicker_model model = new();

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
            PixelMatcherHost.TryInit();

            CreateImageControl();
            RegisterDelegate();
            HostEventInit();
            ViewModelInit();
        }
    }
    public partial class uPixelPicker_viewmodel
    {
        private bool IsBusy { get; set; } = false;
        private bool IsMouseOver { get; set; } = false;
        private bool IsMouseLeftDown { get; set; } = false;
        private Point GetMousePoint { get; set; } = new();
        private double LocalRatio = 1;
    }
    public partial class uPixelPicker_viewmodel
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
                NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MessageStr));
            }
        }
        public string ClickPointInfo
        {
            get { return model.ClickPointInfo; }
            set
            {
                model.ClickPointInfo = value;
                NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MessageStr));
            }
        }
        public string MessageStr => (SizeInfo.Equals(string.Empty) ? "" : $" size{SizeInfo} {ClickPointInfo} {""}");
    }

    //CreateImageControl
    public partial class uPixelPicker_viewmodel
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
            MessageBox.Child = new Grid().Init(grid =>
            {
                grid.Children.Add(new TextBlock().Init(tb =>
                {
                    tb.Margin = new(1, 0, 0, 0);
                    tb.Foreground = new SolidColorBrush(Colors.Snow);
                    tb.Background = new SolidColorBrush(Colors.SlateGray);
                    tb.SetBinding(TextBlock.TextProperty, new Binding(nameof(MessageStr)) { Source = this });
                }));
            });

            //MagnifierBox
            MagnifierBox.Child = new Grid().Init(grid =>
            {
                grid.Children.Add(new Rectangle().Init(rect =>
                {
                    rect.Effect = new MagnifyEffect() { MagnificationAmount = 16 };
                    rect.Fill = new VisualBrush().Init(vb =>
                    {
                        vb.Visual = Image;
                        vb.ViewboxUnits = BrushMappingMode.Absolute;
                        vb.AlignmentX = AlignmentX.Left;
                        vb.AlignmentY = AlignmentY.Top;
                        vb.Viewport = new Rect(0, 0, 1, 1);
                        vb.Stretch = Stretch.None;
                        BindingOperations.SetBinding(vb, VisualBrush.ViewboxProperty, new Binding(nameof(ViewboxRect)) { Source = this });
                    });
                }));
                grid.Children.Add(new Grid().Init(reticle =>
                {
                    reticle.Children.Add(new Rectangle().Init(vertical_line =>
                    {
                        vertical_line.Width = 1;
                        vertical_line.Fill = new SolidColorBrush(Colors.Red);
                        vertical_line.HorizontalAlignment = HorizontalAlignment.Center;
                        vertical_line.OpacityMask = new LinearGradientBrush().Init(lgb =>
                        {
                            lgb.StartPoint = new(0, 0);
                            lgb.EndPoint = new(1, 0);
                            lgb.GradientStops.Add(new() { Offset = 0, Color = Colors.Black });
                            lgb.GradientStops.Add(new() { Offset = 0.46875, Color = Colors.Black });
                            lgb.GradientStops.Add(new() { Offset = 0.46875, Color = Colors.Transparent });
                            lgb.GradientStops.Add(new() { Offset = 0.53125, Color = Colors.Transparent });
                            lgb.GradientStops.Add(new() { Offset = 0.53125, Color = Colors.Black });
                            lgb.GradientStops.Add(new() { Offset = 1, Color = Colors.Black });
                        });
                    }));
                    reticle.Children.Add(new Rectangle().Init(horizontal_line =>
                    {
                        horizontal_line.Height = 1;
                        horizontal_line.Fill = new SolidColorBrush(Colors.Red);
                        horizontal_line.VerticalAlignment = VerticalAlignment.Center;
                        horizontal_line.OpacityMask = new LinearGradientBrush().Init(lgb =>
                        {
                            lgb.StartPoint = new(0, 0);
                            lgb.EndPoint = new(1, 0);
                            lgb.GradientStops.Add(new() { Offset = 0, Color = Colors.Black });
                            lgb.GradientStops.Add(new() { Offset = 0.46875, Color = Colors.Black });
                            lgb.GradientStops.Add(new() { Offset = 0.46875, Color = Colors.Transparent });
                            lgb.GradientStops.Add(new() { Offset = 0.53125, Color = Colors.Transparent });
                            lgb.GradientStops.Add(new() { Offset = 0.53125, Color = Colors.Black });
                            lgb.GradientStops.Add(new() { Offset = 1, Color = Colors.Black });
                        });
                    }));
                    reticle.Children.Add(new Border().Init(box_white =>
                    {
                        box_white.Width = 9;
                        box_white.Height = 9;
                        box_white.BorderBrush = new SolidColorBrush(Colors.White);
                        box_white.BorderThickness = new Thickness(1);
                        box_white.HorizontalAlignment = HorizontalAlignment.Center;
                        box_white.VerticalAlignment = VerticalAlignment.Center;
                    }));
                    reticle.Children.Add(new Border().Init(box_black =>
                    {
                        box_black.Width = 11;
                        box_black.Height = 11;
                        box_black.BorderBrush = new SolidColorBrush(Colors.Black);
                        box_black.BorderThickness = new Thickness(1);
                        box_black.HorizontalAlignment = HorizontalAlignment.Center;
                        box_black.VerticalAlignment = VerticalAlignment.Center;
                    }));
                }));
            });
        }
    }
    //RegisterDelegate
    public partial class uPixelPicker_viewmodel
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
                        using (bmp)
                        {
                            // 限制刷新频率
                            if (timer.Elapsed.TotalMilliseconds > 100 && IsBusy is false)
                            {
                                timer.Restart();

                                SD.Imaging.BitmapData data = bmp.LockBits(new SD.Rectangle(0, 0, bmp.Width, bmp.Height), SD.Imaging.ImageLockMode.ReadWrite, SD.Imaging.PixelFormat.Format32bppArgb);
                                IntPtr ptr = data.Scan0;
                                int bytes = Math.Abs(data.Stride) * bmp.Height;//w*h*4

                                SizeInfo = $"({bmp.Width},{bmp.Height})";

                                BGRA.Stride = data.Width;
                                BGRA.Values = new byte[bytes];
                                System.Runtime.InteropServices.Marshal.Copy(ptr, BGRA.Values, 0, bytes);
                                bmp.UnlockBits(data);

                                var bitmapImage = new BitmapImage().Init(bi =>
                                {
                                    using (var ms = new MemoryStream())
                                    {
                                        bmp.Save(ms, SD.Imaging.ImageFormat.Bmp);
                                        bi.BeginInit();
                                        bi.StreamSource = ms;
                                        bi.CacheOption = BitmapCacheOption.OnLoad;
                                        bi.EndInit();
                                        bi.Freeze();
                                    }
                                });
                                return bitmapImage;
                            }
                        }
                    }

                    return null;
                });

                if (source is not null)
                {
                    if (IsBusy is false)
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
                    else
                    {
                        source.StreamSource.Dispose();
                    }
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
    public partial class uPixelPicker_viewmodel
    {
        public RelayCommand LoadedCommand { get; set; }
        public RelayCommand PreviewMouseRightButtonUpCommand { get; set; }
        public RelayCommand PreviewMouseLeftButtonDownCommand { get; set; }
        public RelayCommand PreviewMouseLeftButtonUpCommand { get; set; }
        public RelayCommand MouseEnterCommand { get; set; }
        public RelayCommand MouseLeaveCommand { get; set; }
        public RelayCommand MouseMoveCommand { get; set; }

        private Point old_click_pos = new(0, 0);

        private void HostEventInit()
        {
            LoadedCommand = new(para =>
            {
                if (para is RoutedEventArgs e)
                {
                    var host = (UserControl)e.Source;
                    host.Margin = new Thickness(1);
                    host.MinHeight = 400;
                    host.MaxHeight = host.ActualHeight;
                    RenderOptions.SetBitmapScalingMode(host, BitmapScalingMode.Fant);
                }
            });
            PreviewMouseRightButtonUpCommand = new(para =>
            {
                if (ImageMargin.Equals(new(0)))
                {
                    //save to desktop
                    if (ImageSource is BitmapImage bi && bi is not null && bi.Width > 0 && bi.Height > 0)
                    {
                        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        var filePath = System.IO.Path.Combine(desktopPath, "_uPixelPicker.png");
                        BitmapSourceToPngFile(bi, filePath);
                        Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, $"save to {filePath}");
                    }
                }
                else
                {
                    ImageMargin = new(0);
                }
            });
            PreviewMouseLeftButtonDownCommand = new(para =>
            {
                if (para is MouseButtonEventArgs e)
                {
                    var sender = (UserControl)e.Source;

                    IsMouseLeftDown = true;
                    GetMousePoint = (e.GetPosition(sender));
                    ImageMarginWhenLeftDown = ImageMargin;
                }
            });
            PreviewMouseLeftButtonUpCommand = new(para =>
            {
                if (para is MouseButtonEventArgs e)
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
                                ClickPointInfo = $"\n -> {click.Info} " + $"\n -> Rect<{click.GetBoundingRect(old_click_pos)}> ";
                                old_click_pos = click.Point;
                            }
                        }
                    }
                }
            });
            MouseEnterCommand = new(para =>
            {
                if (para is MouseEventArgs e)
                {
                    IsMouseOver = true;
                    LocalRatio = GetDeviceScaleFactor((UserControl)e.Source);
                }
            });
            MouseLeaveCommand = new(para =>
            {
                IsMouseOver = IsMouseLeftDown = false;
            });
            MouseMoveCommand = new(para =>
            {
                if (para is MouseEventArgs e)
                {
                    var sender = (UserControl)e.Source;

                    if (IsMouseLeftDown && ImageSource != null)
                    {
                        var p = e.GetPosition(sender).GetDiff(GetMousePoint);
                        ImageMargin = new Thickness(ImageMarginWhenLeftDown.Left + p.X, ImageMarginWhenLeftDown.Top + p.Y, 0, 0);
                    }
                    if (BGRA.Values is not null && !IsMouseLeftDown)
                    {
                        var click = GetColorInfo(e.GetPosition(Image));
                        if (click.IsSuccess) { Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, click.Info); }
                    }

                    { //放大镜
                        var p = e.GetPosition(sender);
                        var rect_width = 128d;
                        ViewboxRect = new Rect(p.X - rect_width / 2 + 1, p.Y - rect_width / 2 + 1, rect_width, rect_width);
                    }
                }
            });
        }
    }
    //ViewModelInit
    public partial class uPixelPicker_viewmodel
    {
        private void ViewModelInit()
        {
            this.CurrentScreenshot = Image;
            this.AdditionalInfo = MessageBox;
            this.Magnifier = MagnifierBox;
        }
    }

    //Method
    public partial class uPixelPicker_viewmodel
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

        private void BitmapSourceToPngFile(BitmapSource bitmapSource, string pngFilePath)
        {
            try
            {
                IsBusy = true;

                using (var stream = new FileStream(pngFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(stream);
                }
            }
            catch (Exception ex)
            {
                Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, $"BitmapSourceToPngFile Error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
