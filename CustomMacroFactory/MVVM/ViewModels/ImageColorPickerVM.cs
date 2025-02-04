using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CustomMacroBase.CustomEffect;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.Extensions;
using CustomMacroBase.Messages;
using CustomMacroBase.PixelMatcher;
using CustomMacroFactory.MVVM.Models;
using CustomMacroFactory.MVVM.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrarsUI.Shared.Interfaces.UIComponents;
using TrarsUI.Shared.Messages;
using SD = System.Drawing;

namespace CustomMacroFactory.MVVM.ViewModels
{
    public partial class ImageColorPickerVM : ObservableObject, IContentVM
    {
        private const string aimIconData = "M512 857.6c-190.592 0-345.6-155.008-345.6-345.6S321.408 166.4 512 166.4s345.6 155.008 345.6 345.6-155.008 345.6-345.6 345.6z m0-640C349.696 217.6 217.6 349.696 217.6 512s132.096 294.4 294.4 294.4c162.304 0 294.4-132.096 294.4-294.4S674.304 217.6 512 217.6z" +
                                           "M972.8 537.6H51.2a25.6 25.6 0 1 1 0-51.2h921.6a25.6 25.6 0 1 1 0 51.2z" +
                                           "M512 998.4a25.6 25.6 0 0 1-25.6-25.6V51.2a25.6 25.6 0 1 1 51.2 0v921.6a25.6 25.6 0 0 1-25.6 25.6z";

        [ObservableProperty]
        private string title = "Image Color Picker";

        [ObservableProperty]
        private UIElement currentScreenshot;

        [ObservableProperty]
        private UIElement additionalInfo;

        [ObservableProperty]
        private UIElement magnifier;

        [ObservableProperty]
        private bool allowRefresh;

        public TrarsUI.Shared.Collections.AlertMessageObservableCollection SystemMessages { get; set; } = new(5, 1);

        /// <summary>
        /// Aim图标
        /// </summary>
        public ObservableCollection<UIElement> AimIcon { get; set; } = new()
        {
            new System.Windows.Shapes.Path()
            {
                Width = 25 - 2,
                Height = 25 - 2,
                Fill = new SolidColorBrush(Colors.Transparent),
                Stroke = new SolidColorBrush(Colors.Red),
                IsHitTestVisible = false,
                Stretch = Stretch.Fill,
                Data = Geometry.Parse(aimIconData.ToString(CultureInfo.InvariantCulture)),
                Effect = new DropShadowEffect()
                {
                    Color = Colors.Blue,
                    ShadowDepth = 0,
                    BlurRadius = 5,
                    Opacity = 0.5
                }
            }
        };

        public ImageColorPickerVM()
        {
            PixelMatcherHost.TryInit();

            CreateImageControl();
            RegisterDelegate();

            this.CurrentScreenshot = Image;
            this.AdditionalInfo = MessageBox;
            this.Magnifier = MagnifierBox;
            this.AllowRefresh = true;
            WeakReferenceMessenger.Default.Send(new CanUpdateFrames(this.AllowRefresh));
        }
    }
    public partial class ImageColorPickerVM
    {
        private UIElement ScreenshotArea;

        private bool IsBusy { get; set; } = false;
        private bool IsMouseOver { get; set; } = false;
        private bool IsMouseLeftDown { get; set; } = false;
        private Point GetMousePoint { get; set; } = new();
        private double LocalRatio = 1;
    }
    public partial class ImageColorPickerVM
    {
        [ObservableProperty]
        private ImageSource? imageSource = null;

        [ObservableProperty]
        private Thickness imageMargin = new();

        [ObservableProperty]
        private Thickness imageMarginWhenLeftDown = new();

        [ObservableProperty]
        private PixelDetail bGRA = new();

        [ObservableProperty]
        private Rect viewboxRect = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MessageStr))]
        private string sizeInfo = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MessageStr))]
        private string clickPointInfo = string.Empty;

        public string MessageStr => (SizeInfo.Equals(string.Empty) ? "" : $" size{SizeInfo} {ClickPointInfo} {""}");

        [ObservableProperty]
        private ColorHex clickHex = new();

        [ObservableProperty]
        private ColorHex moveHex = new();
    }

    //CreateImageControl
    public partial class ImageColorPickerVM
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
                            lgb.EndPoint = new(0, 1);
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
    public partial class ImageColorPickerVM
    {
        private Stopwatch timer = Stopwatch.StartNew();

        private void RegisterDelegate()
        {
            WeakReferenceMessenger.Default.Register<AsyncSnapshotMessage>(this, (r, m) =>
            {
                m.Reply(((Func<Task<bool>>)(async () =>
                {
                    var bmp = m.Bitmap;

                    var source = await Task.Run(async () =>
                    {
                        await Task.Yield();

                        if (bmp is SD.Bitmap)
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

                    return true;
                }))());

            });

            WeakReferenceMessenger.Default.Register<AlertMessage>(this, (r, m) =>
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.SystemMessages.Add(new(m.Value));
                });
            });
        }
    }

    //RelayCommand
    public partial class ImageColorPickerVM
    {
        private Point old_click_pos = new(0, 0);

        [RelayCommand]
        private void OnLoaded(object para)
        {
            if (para is RoutedEventArgs e)
            {
                var host = (UserControl)e.Source;
                host.Margin = new Thickness(1);
                host.MinHeight = 400;
                host.MaxHeight = host.ActualHeight;
                RenderOptions.SetBitmapScalingMode(host, BitmapScalingMode.Fant);

                host.MouseLeftButtonDown += (s, e) =>
                {
                    host.Focus(); //聚焦
                };

                ScreenshotArea = ((ImageColorPicker)host).ScreenshotArea;
            }
        }

        [RelayCommand]
        private void OnPreviewMouseRightButtonUp(object para)
        {
            ImageMargin = new(0);
        }

        [RelayCommand]
        private void OnPreviewMouseLeftButtonDown(object para)
        {
            if (para is MouseButtonEventArgs e)
            {
                if (!IsMouseInsideScreenshotArea(e)) { return; }

                IsMouseLeftDown = true;
                GetMousePoint = (e.GetPosition(this.ScreenshotArea));
                ImageMarginWhenLeftDown = ImageMargin;
            }
        }

        [RelayCommand]
        private void OnPreviewMouseLeftButtonUp(object para)
        {
            if (para is MouseButtonEventArgs e)
            {
                if (!IsMouseInsideScreenshotArea(e)) { return; }

                IsMouseLeftDown = false;
                if (BGRA.Values is not null)
                {
                    var pt = e.GetPosition(Image);
                    var click = GetColorInfo(pt);
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
                    this.ClickHex = GetColorHex(pt);
                }
            }
        }

        [RelayCommand]
        private void OnMouseEnter(object para)
        {
            if (para is MouseEventArgs e)
            {
                IsMouseOver = true;
                LocalRatio = GetDeviceScaleFactor((UserControl)e.Source);
            }
        }

        [RelayCommand]
        private void OnMouseLeave(object para)
        {
            IsMouseOver = IsMouseLeftDown = false;
        }

        [RelayCommand]
        private void OnMouseMove(object para)
        {
            if (para is MouseEventArgs e)
            {
                if (!IsMouseInsideScreenshotArea(e)) { return; }

                if (IsMouseLeftDown && ImageSource != null)
                {
                    var p = e.GetPosition(this.ScreenshotArea).GetDiff(GetMousePoint);
                    ImageMargin = new Thickness(ImageMarginWhenLeftDown.Left + p.X, ImageMarginWhenLeftDown.Top + p.Y, 0, 0);
                }
                if (BGRA.Values is not null && !IsMouseLeftDown)
                {
                    var pt = e.GetPosition(Image);
                    var click = GetColorInfo(pt);
                    if (click.IsSuccess) { WeakReferenceMessenger.Default.Send(new PrintNewMessage(click.Info)); }
                    this.MoveHex = GetColorHex(pt);
                }

                { //放大镜
                    var p = e.GetPosition(this.ScreenshotArea);
                    var rect_width = 128d;
                    ViewboxRect = new Rect(p.X - rect_width / 2 + 1, p.Y - rect_width / 2 + 1, rect_width, rect_width);
                }
            }
        }

        [RelayCommand]
        private void OnPreviewKeyDown(object para)
        {
            if (para is KeyEventArgs e)
            {
                if (!IsMouseOver || BGRA.Values is null || IsMouseLeftDown || !AllowRefresh) { return; }

                double step = LocalRatio;

                var inputKey = e.Key;
                if (e.ImeProcessedKey != Key.None) { inputKey = e.ImeProcessedKey; }

                if ((inputKey == Key.W) || (inputKey == Key.Up))
                {
                    ImageMargin = new Thickness(ImageMargin.Left, ImageMargin.Top + step, 0, 0);
                }
                if ((inputKey == Key.A) || (inputKey == Key.Left))
                {
                    ImageMargin = new Thickness(ImageMargin.Left + step, ImageMargin.Top, 0, 0);
                }
                if ((inputKey == Key.S) || (inputKey == Key.Down))
                {
                    ImageMargin = new Thickness(ImageMargin.Left, ImageMargin.Top - step, 0, 0);
                }
                if ((inputKey == Key.D) || (inputKey == Key.Right))
                {
                    ImageMargin = new Thickness(ImageMargin.Left - step, ImageMargin.Top, 0, 0);
                }

                e.Handled = true;
            }
        }

        [RelayCommand]
        private async Task OnPauseRefreshAsync(object para)
        {
            try
            {
                Action? yesnoCallback = null;
                var msg = this.AllowRefresh ? "Pause Refresh ?" : "Allow Refresh ?";
                var token = ((IToken)para).Token;

                if (await WeakReferenceMessenger.Default.Send(new DialogYesNoMessage(msg, (x) => { yesnoCallback = x; }), token))
                {
                    this.AllowRefresh = !this.AllowRefresh;
                    WeakReferenceMessenger.Default.Send(new CanUpdateFrames(this.AllowRefresh));
                }
                yesnoCallback?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"OnPauseRefreshAsync error: {ex.Message}");
            }
        }

        [RelayCommand]
        private void OnSaveToDesktop()
        {
            if (ImageSource is BitmapImage bi && bi is not null && bi.Width > 0 && bi.Height > 0)
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = System.IO.Path.Combine(desktopPath, "_uPixelPicker.png");
                BitmapSourceToPngFile(bi, filePath);
                WeakReferenceMessenger.Default.Send(new PrintNewMessage($"save to {filePath}"));
            }
        }

        /// <summary>
        /// "Get a screenshot of the target window"
        /// </summary>
        [RelayCommand]
        private void OnGetSnapshot()
        {
            var result = PixelMatcherHost.GetTargetWindowSnapshotEx();
            var msg = string.Empty;
            if (result)
            {
                msg = this.AllowRefresh ? "Screenshot captured successfully" : "Screenshot captured, but refreshing is disabled";
            }
            else
            {
                msg = "Failed to capture screenshot";
            };
            WeakReferenceMessenger.Default.Send(new AlertMessage(msg));
        }

        /// <summary>
        /// "Resize the target window (client area) to 1920x1080" 
        /// </summary>
        [RelayCommand]
        private void OnResizeTargetWindow1080P()
        {
            PixelMatcherHost.SetTargetWindowSizeEx(1920, 1080);
        }

        /// <summary>
        /// "Resize the target window (client area) to 1280x720"
        /// </summary>
        [RelayCommand]
        private void OnResizeTargetWindow720P()
        {
            PixelMatcherHost.SetTargetWindowSizeEx(1280, 720);
        }
    }

    //Method
    public partial class ImageColorPickerVM
    {
        private bool IsMouseInsideScreenshotArea(MouseEventArgs e)
        {
            // 获取鼠标在 ScreenshotArea 中的位置
            var mousePosition = e.GetPosition(this.ScreenshotArea);

            // 使用 RenderSize 检查鼠标是否超出 ScreenshotArea 的范围
            var renderSize = this.ScreenshotArea.RenderSize;
            if (mousePosition.X < 0 || mousePosition.Y < 0 ||
                mousePosition.X > renderSize.Width ||
                mousePosition.Y > renderSize.Height)
            {
                return false; // 如果超出范围，直接返回
            }

            return true;
        }

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
        private ColorHex GetColorHex(Point p)
        {
            if (BGRA.Values is null) { return new(); }
            var pt = new Point((int)(p.X / LocalRatio), (int)(p.Y / LocalRatio));
            var ptr = ((int)pt.Y * BGRA.Stride + (int)pt.X) << 2;
            if (pt.X >= 0 && pt.Y >= 0 && pt.X < BGRA.Stride && ptr < BGRA.Values.Length)
            {
                return new() { Hex = BGRA.GetColorHexString(ptr), Pos = $"({pt.X}, {pt.Y})" };
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
                WeakReferenceMessenger.Default.Send(new PrintNewMessage($"BitmapSourceToPngFile Error: {ex.Message}"));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
