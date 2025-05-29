using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CustomMacroBase.CustomControlEx.AggregateBindingControl;
using CustomMacroBase.GamePadState;
using CustomMacroBase.Helper.Converter;
using CustomMacroBase.Helper.ProConSimulate;
using CustomMacroBase.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using TrarsUI.Shared.Controls.ComboBoxEx;
using TrarsUI.Shared.Controls.SliderEx;
using TrarsUI.Shared.Controls.ToggleButtonEx;
using TrarsUI.Shared.Controls.ValueIndicatorEx;
using TrarsUI.Shared.DTOs;
using static CustomMacroBase.PixelMatcher.OpenCV;


//MacroBase(viewmodel)
namespace CustomMacroBase
{
    /// <summary>
    /// DS4按键名
    /// </summary>
    public struct DS4Btn
    {
        public readonly string Square => "Square";
        public readonly string Triangle => "Triangle";
        public readonly string Circle => "Circle";
        public readonly string Cross => "Cross";
        public readonly string DpadUp => "DpadUp";
        public readonly string DpadDown => "DpadDown";
        public readonly string DpadLeft => "DpadLeft";
        public readonly string DpadRight => "DpadRight";
        public readonly string Share => "Share";
        public readonly string Options => "Options";
        public readonly string TouchButton => "TouchButton";
        public readonly string OutputTouchButton => "OutputTouchButton";
        public readonly string PS => "PS";
        public readonly string Mute => "Mute";
        public readonly string L1 => "L1";
        public readonly string L2 => "L2";//byte
        public readonly string L3 => "L3";
        public readonly string R1 => "R1";
        public readonly string R2 => "R2";//byte
        public readonly string R3 => "R3";
        public readonly string LX => "LX";//byte
        public readonly string RX => "RX";//byte
        public readonly string LY => "LY";//byte
        public readonly string RY => "RY";//byte

        public readonly string Touch0RawTrackingNum => "Touch0RawTrackingNum";
        public readonly string Touch0Id => "Touch0Id";
        public readonly string Touch0IsActive => "Touch0IsActive";
        //public readonly string Touch0 => "Touch0";
    }

    /// <summary>
    /// 脚本基类
    /// </summary>
    public abstract partial class MacroBase : ObservableObject
    {
        #region 仅限基类内部访问的 字段
        private static DS4StateLite? rStateLite;//真实手柄
        private static DS4StateLite? vStateLite;//虚拟手柄
        private static int vIndex = -1;
        #endregion

        #region 仅限基类和子类内部访问的 方法/属性/字段
        /// <summary>
        /// <para>DS4按键名集合</para>
        /// </summary>
        protected readonly static DS4Btn btnKey = new();

        /// <summary>
        /// 获取真实手柄按键状态
        /// </summary>
        protected static DS4StateLite RealDS4 => rStateLite;
        /// <summary>
        /// 获取虚拟手柄按键状态
        /// </summary>
        protected static DS4StateLite VirtualDS4 => vStateLite;

        /// <summary>
        /// 获取手柄Index
        /// </summary>
        protected static int Ind => vIndex;

        /// <summary>
        /// <para>通过反射更新虚拟手柄按键状态</para>
        /// </summary>
        protected static void UpdateNow(in string propName, in byte propValue)
        {
            vStateLite?.GetType()?.GetField(propName)?.SetValue(vStateLite, propValue);
        }
        /// <summary>
        /// <para>通过反射更新虚拟手柄按键状态</para>
        /// </summary>
        protected static void UpdateNow(in string propName, in bool propValue)
        {
            vStateLite?.GetType()?.GetField(propName)?.SetValue(vStateLite, propValue);
        }

        /// <summary>
        /// <para>通过反射获取虚拟手柄按键状态</para>
        /// </summary>
        protected static T GetNow<T>(in string propName)
        {
            return (T)vStateLite?.GetType()?.GetField(propName)?.GetValue(vStateLite)!;
        }

        /// <summary>
        /// <para>范围找色_new</para>
        /// <para>-</para>
        /// <para>参数 color：<see cref="System.Int32"/> 类型，匹配的颜色（比如填'0xFF0000'意为待查找颜色为红色）</para>
        /// <para>参数 rect：<see cref="System.Drawing.Rectangle"/>? 类型，描述匹配范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内，填'null'默认全图）</para>
        /// <para>参数 tolerance：<see cref="System.Int32"/>? 类型，匹配的容差（比如填'20'意为被对比的两个颜色之间RGB分量差值允许浮动±20，填'null'默认20）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="int"/> 类型，匹配到的颜色数量</para>
        /// </summary>
        protected static int FindColor(int color, Rectangle? rect, int? tolerance)
        {
            return PixelMatcher.PixelMatcherHost.FindColor(color, rect, tolerance);
        }


        /// <summary>
        /// <para>范围找图</para>
        /// <para>-</para>
        /// <para>参数 bitmap：<see cref="Bitmap"/>类型，匹配的小图</para>
        /// <para>参数 rect：<see cref="Rectangle"/>? 类型，描述匹配范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内，填'null'默认全图）</para>
        /// <para>参数 tolerance：<see cref="double"/>? 类型，匹配的相似度（范围0~1，比如填'null'意为使用默认值0.8）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="System.Drawing.Point"/>? 类型，匹配成功返矩形左上角坐标，匹配失败返回null</para>
        /// </summary>
        protected static System.Drawing.Point? FindImage(ref Bitmap bitmap, Rectangle? rect, double? tolerance)
        {
            return PixelMatcher.PixelMatcherHost.FindImage(ref bitmap, rect, tolerance);
        }

        /// <summary>
        /// <para>范围找图</para>
        /// <para>-</para>
        /// <para>参数 saveFolder：<see cref="string"/> 类型，匹配的小图的绝对路径</para>
        /// <para>参数 rect：<see cref="Rectangle"/>? 类型，描述匹配范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内，填'null'默认全图）</para>
        /// <para>参数 tolerance：<see cref="double"/>? 类型，相似度（范围0~1，比如填'null'意为使用默认值0.8）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="System.Drawing.Point"/>? 类型，匹配成功返矩形左上角坐标，匹配失败返回null</para>
        /// </summary>
        protected static System.Drawing.Point? FindImage(string path, Rectangle? rect, double? tolerance)
        {
            return PixelMatcher.PixelMatcherHost.FindImage(path, rect, tolerance);
        }

        /// <summary>
        /// <para>范围找字（仅限数字0~9）</para>
        /// <para>-</para>
        /// <para>参数 rect：<see cref="Rectangle"/> 类型，描述查找范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内）</para>
        /// <para>参数 deviceType：<see cref="DeviceType"/> 类型，指定设备类型（Mkldnn/Onnx/Openblas/Gpu）</para>
        /// <para>参数 zoomratio：<see cref="double"/> 类型，缩放比例（比如填'2'意为宽高均拉伸至原来的2倍）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="string"/> 类型，识别成功返回字符串，识别失败返回空字符串</para>
        /// </summary>
        protected static string FindNumber(Rectangle rect, DeviceType deviceType, double zoomratio = 2.0)
        {
            return PixelMatcher.PixelMatcherHost.FindNumber(rect, true, deviceType, zoomratio);
        }

        /// <summary>
        /// <para>范围找字</para>
        /// <para>-</para>
        /// <para>参数 rect：<see cref="Rectangle"/> 类型，描述查找范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内）</para>
        /// <para>参数 deviceType：<see cref="DeviceType"/> 类型，指定设备类型（Mkldnn/Onnx/Openblas/Gpu）</para>
        /// <para>参数 language：<see cref="ModelType"/> 类型，指定语言（EnglishV3/JapanV3/ChineseV3/TraditionalChineseV3）</para>
        /// <para>参数 zoomratio：<see cref="double"/> 类型，缩放比例（比如填'2'意为宽高均拉伸至原来的2倍）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="string"/> 类型，识别成功返回字符串，识别失败返回空字符串</para>
        /// </summary>
        protected static string FindText(Rectangle rect, DeviceType deviceType, ModelType language = ModelType.EnglishV3, double zoomratio = 2.0)
        {
            return PixelMatcher.PixelMatcherHost.FindText(rect, true, deviceType, language, zoomratio);
        }

        /// <summary>
        /// <para>打印文本</para>
        /// </summary>
        protected static void Print([CallerMemberName] string str = "")
        {
            WeakReferenceMessenger.Default.Send(new PrintNewMessage(str));
        }
        #endregion

        #region MacroModel属性筛选
        /// <summary>
        /// <para>顶层开关</para>
        /// </summary>
        [ObservableProperty]
        private ToggleTreeViewNode mainGate;

        /// <summary>
        /// <para>按钮文本，默认值为类名</para>
        /// </summary>
        [ObservableProperty]
        private string title;

        /// <summary>
        /// <para>按钮选中状态</para>
        /// </summary>
        [ObservableProperty]
        private bool selected;

        /// <summary>
        /// <para>按钮文本染色</para>
        /// </summary>
        [ObservableProperty]
        private bool useColorfulText;
        #endregion

        #region 通知MacroCreator更新选中项

        public event Action? OnSelectedChangedAction;

        partial void OnSelectedChanged(bool value)
        {
            if (value)
            {
                OnSelectedChangedAction?.Invoke();
            }
        }

        #endregion

        #region 脚本入口
        /// <summary>
        /// 入口
        /// </summary>
        public void UpdateEntry(in int _ind, in DS4StateLite _realState, in DS4StateLite _virtualState)
        {
            rStateLite = _realState;
            vStateLite = _virtualState;
            vIndex = _ind;
            this.UpdateState();
        }
        #endregion



        #region 抽象方法，需自实现
        /// <summary>
        /// <para>该方法将于轮询中反复被执行</para>
        /// <para>于该方法中监听 RealDS4 并操作 VirtualDS4 以实现自定义脚本</para>
        /// <para>-</para>
        /// <para>轮询频率设置为250Hz/500Hz/1000Hz时，理论上该方法每秒被执行250次/500次/1000次</para>
        /// <para></para>
        /// </summary>
        public abstract void UpdateState();//in DS4StateLite _pState, in DS4StateLite _cState
        /// <summary>
        /// <para>该方法将于本类被实例化时执行一次</para>
        /// <para>于该方法中操作 MainGate.Children 以实现往UI上放置默认滑块开关</para>
        /// </summary>
        public abstract void Init();
        #endregion



        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MacroBase()
        {
            MainGate = new() { UseDelayRender = true, Text = "MainGate", Enable = true };
            Title = Regex.Replace(this.GetType().Name, "Game_", string.Empty);

            try
            {
                this.Init();
                this.SetTooltipPrefix(MainGate, true, "Root");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"{this.GetType().Name}.cs -> Init()", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 提示下标
        /// </summary>
        private void SetTooltipPrefix(ToggleTreeViewNode node, bool isRoot, string tip)
        {
            if (node is null) return;

            node.TooltipPrefix = tip;

            List<ToggleTreeViewNode> list = node.GateBaseList;

            for (int i = 0; i < list.Count; i++)
            {
                SetTooltipPrefix(list[i], false, $"{(isRoot ? tip + " - " : tip)}[{i}]");
            }
        }
        #endregion
    }
    /// <summary>
    /// 脚本基类_ProConSendReport
    /// </summary>
    public abstract partial class MacroBase
    {
        private struct ProConBtn
        {
            public uint A => 0x8;
            public uint B => 0x4;
            public uint X => 0x2;
            public uint Y => 0x1;

            public uint L => 0x40_0000;
            public uint R => 0x40;
            public uint ZL => 0x80_0000;
            public uint ZR => 0x80;

            public uint DpadLeft => 0x8_0000;
            public uint DpadUp => 0x2_0000;
            public uint DpadRight => 0x4_0000;
            public uint DpadDown => 0x1_0000;
            public uint DpadLeftUp => 0xA_0000;
            public uint DpadRightUp => 0x6_0000;
            public uint DpadLeftDown => 0x9_0000;
            public uint DpadRightDown => 0x5_0000;

            public uint Plus => 0x200;
            public uint Minus => 0x100;
            public uint Home => 0x1000;
            public uint Capture => 0x2000;
            public uint LeftStickBtn => 0x800;
            public uint RightStickBtn => 0x400;
        }
        private static ProConBtn procon = new();
        private static bool proconTask_is_running = false;

        private static void ProConStartGamepad()
        {
            if (proconTask_is_running)
            {
                //Print($"ProConTask is running");
                return;
            }
            else
            {
                Task.Run(() =>
                {
                    proconTask_is_running = true;
                    BTController.send_padcolor();
                    BTController.start_gamepad();//太坑了，此处有去无回
                }).ContinueWith(_ =>
                {
                    proconTask_is_running = false;
                    MessageBox.Show("proconTask_is_running = false");//不可能抵达此处
                });
            }
        }

        /// <summary>
        /// <para>将虚拟DS4手柄的按键状态转发给蓝牙模拟的Pro手柄</para>
        /// <para>-</para>
        /// <para>参数 waittime：新的btkeyLib.dll（2022.09.26）轮询频率提高至1ms，该参数不再需要</para>
        /// </summary>
        protected static void ProConSendReport(uint waittime = 32)
        {
            ProConStartGamepad();

            uint key = 0;
            // Square Cross Circle Triangle -> Y B A X
            if (VirtualDS4.Square) key |= procon.Y;
            if (VirtualDS4.Cross) key |= procon.B;
            if (VirtualDS4.Circle) key |= procon.A;
            if (VirtualDS4.Triangle) key |= procon.X;

            //L1 R1 L2 R2 -> L R ZL ZR
            if (VirtualDS4.L1) key |= procon.L;
            if (VirtualDS4.R1) key |= procon.R;
            if (VirtualDS4.L2 > 0) key |= procon.ZL;
            if (VirtualDS4.R2 > 0) key |= procon.ZR;

            //Share Options -> Minus Plus 
            if (VirtualDS4.Share) key |= procon.Minus;
            if (VirtualDS4.Options) key |= procon.Plus;

            //L3 R3 -> 
            if (VirtualDS4.L3) key |= procon.LeftStickBtn;
            if (VirtualDS4.R3) key |= procon.RightStickBtn;

            //PS TouchBtn -> home capture 
            if (VirtualDS4.PS) key |= procon.Home;
            if (VirtualDS4.TouchButton) key |= procon.Capture;

            //Dpad
            bool slant = false;
            if (!slant && VirtualDS4.DpadLeft && VirtualDS4.DpadUp) { key |= procon.DpadLeftUp; slant = true; }
            if (!slant && VirtualDS4.DpadRight && VirtualDS4.DpadUp) { key |= procon.DpadRightUp; slant = true; }
            if (!slant && VirtualDS4.DpadLeft && VirtualDS4.DpadDown) { key |= procon.DpadLeftDown; slant = true; }
            if (!slant && VirtualDS4.DpadRight && VirtualDS4.DpadDown) { key |= procon.DpadRightDown; slant = true; }
            if (!slant)
            {
                if (VirtualDS4.DpadUp) key |= procon.DpadUp;
                if (VirtualDS4.DpadDown) key |= procon.DpadDown;
                if (VirtualDS4.DpadLeft) key |= procon.DpadLeft;
                if (VirtualDS4.DpadRight) key |= procon.DpadRight;
            }

            BTController.send_button(key, waittime);
            //Left Stick
            BTController.send_stick_l((uint)(VirtualDS4.LX * 4095 / 255), (uint)(4095 - VirtualDS4.LY * 4095 / 255), waittime);
            //Right Stick
            BTController.send_stick_r((uint)(VirtualDS4.RX * 4095 / 255), (uint)(4095 - VirtualDS4.RY * 4095 / 255), waittime);
        }

        /// <summary>
        /// 读取Amiibo
        /// </summary>
        /// <param name="path"></param>
        protected static void ProConSendAmiibo(string path)
        {
            BTController.send_amiibo(path);
        }
    }
    /// <summary>
    /// 脚本基类_GetStringDiffRate
    /// </summary>
    public abstract partial class MacroBase
    {
        /// <summary>
        /// 获取两字符串相似度
        /// </summary>
        protected static double GetStringDiffRate(string str1, string str2)
        {
            var levenshteinDistance = Fastenshtein.Levenshtein.Distance(str1, str2);
            var length = Math.Max(str1.Length, str2.Length);
            var diffRatio = 1.0 - ((double)levenshteinDistance / length);
            return diffRatio;
        }
    }
    /// <summary>
    /// 脚本基类_CreateControl
    /// </summary>
    public abstract partial class MacroBase
    {
        /// <summary>
        /// CreateTVN
        /// </summary>
        protected static ToggleTreeViewNode CreateTVN(string text,
                                                 bool enable = true,
                                                 string? groupName = null,
                                                 bool hideself = false)
        {
            return new ToggleTreeViewNode()
            {
                Text = text,
                Enable = enable,
                GroupName = groupName,
                HideSelf = hideself
            };
        }

        /// <summary>
        /// CreateTextBlock
        /// </summary>
        protected static TextBlock CreateTextBlock(string str, System.Windows.Media.Color color)
        {
            return new TextBlock()
            {
                Text = $"{str}",
                Foreground = new SolidColorBrush(color),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
            };
        }

        /// <summary>
        /// CreateSlider
        /// </summary>
        protected static UIElement CreateSlider(double minimum,
                                                double maximum,
                                                object model,
                                                string propName,
                                                double tickFrequency = 1,
                                                string sliderTextPrefix = "",
                                                double defalutValue = 0,
                                                string sliderTextSuffix = "",
                                                bool hideself = false,
                                                HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch,
                                                VerticalAlignment verticalAlignment = VerticalAlignment.Stretch)
        {
            var stackpanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = horizontalAlignment, VerticalAlignment = verticalAlignment };
            {
                var slider = new cSlider()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Width = 100,
                    Maximum = maximum,
                    Minimum = minimum,
                    TickFrequency = tickFrequency,
                    LargeChange = tickFrequency,
                    SmallChange = tickFrequency,
                    IsSnapToTickEnabled = true,
                    DefalutValue = defalutValue,
                };
                {
                    slider.Loaded += (s, e) =>
                    {
                        if (slider.IsInitialized is false)
                        {
                            slider.IsInitialized = true;
                            slider.Value = defalutValue;
                        }
                    };
                    slider.PreviewMouseWheel += (s, e) =>
                    {
                        if (e.Delta > 0) { slider.Value = Math.Min(slider.Value + tickFrequency, slider.Maximum); }
                        if (e.Delta < 0) { slider.Value = Math.Max(slider.Value - tickFrequency, slider.Minimum); }
                        e.Handled = true;
                    };
                    slider.PreviewMouseRightButtonUp += (s, e) =>
                    {
                        slider.Value = Math.Clamp(slider.DefalutValue, slider.Minimum, slider.Maximum);
                    };

                    BindingOperations.SetBinding(slider, Slider.ValueProperty, new Binding(propName) { Source = model });
                }

                var textblock = new TextBlock() { Foreground = new SolidColorBrush(Colors.White), VerticalAlignment = VerticalAlignment.Center, Margin = new(5, 0, 0, 0) };
                {
                    bool flag = (tickFrequency % 1 == 0);

                    var run_pre = new Run($"{sliderTextPrefix}{(sliderTextPrefix.Length == 0 ? "" : " ")}");
                    var run_value = new Run();
                    var run_suf = new Run($"{(sliderTextSuffix.Length == 0 ? "" : " ")}{sliderTextSuffix}");
                    textblock.Inlines.Add(run_pre);
                    textblock.Inlines.Add(run_value);
                    textblock.Inlines.Add(run_suf);

                    BindingOperations.SetBinding(run_value, Run.TextProperty, new Binding(propName) { Source = model, Mode = BindingMode.OneWay, StringFormat = $"{(flag ? "0" : "0.00")}" });
                }

                stackpanel.Children.Add(slider);
                stackpanel.Children.Add(textblock);
                stackpanel.Visibility = hideself ? Visibility.Hidden : Visibility.Visible;
                stackpanel.Height = hideself ? 0 : stackpanel.Height;
                stackpanel.IsHitTestVisible = hideself is false;
                stackpanel.Margin = hideself ? new(0) : new(0, 1, 0, 1);
            }

            return stackpanel;
        }
        /// <summary>
        /// CreateSlider2
        /// </summary>
        protected static UIElement CreateSlider2(double minimum,
                                                 double maximum,
                                                 object model,
                                                 string propName,
                                                 double tickFrequency = 1,
                                                 string sliderTextPrefix = "",
                                                 double defalutValue = 0,
                                                 string sliderTextSuffix = "",
                                                 bool hideself = false,
                                                 bool useonoffswitch = true,
                                                 HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch,
                                                 VerticalAlignment verticalAlignment = VerticalAlignment.Stretch)
        {
            var stackpanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = horizontalAlignment, VerticalAlignment = verticalAlignment };
            {
                var slider = new cSlider()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Width = 100,
                    Maximum = maximum,
                    Minimum = minimum,
                    TickFrequency = tickFrequency,
                    LargeChange = tickFrequency,
                    SmallChange = tickFrequency,
                    IsSnapToTickEnabled = true,
                    DefalutValue = defalutValue,
                };
                {
                    slider.Loaded += (s, e) =>
                    {
                        if (slider.IsInitialized is false)
                        {
                            slider.IsInitialized = true;
                            slider.Value = defalutValue;
                        }
                    };
                    slider.PreviewMouseWheel += (s, e) =>
                    {
                        if (e.Delta > 0) { slider.Value = Math.Min(slider.Value + tickFrequency, slider.Maximum); }
                        if (e.Delta < 0) { slider.Value = Math.Max(slider.Value - tickFrequency, slider.Minimum); }
                        e.Handled = true;
                    };
                    slider.PreviewMouseRightButtonUp += (s, e) =>
                    {
                        slider.Value = Math.Clamp(slider.DefalutValue, slider.Minimum, slider.Maximum);
                    };
                }

                var textblock = new TextBlock() { Foreground = new SolidColorBrush(Colors.White), VerticalAlignment = VerticalAlignment.Center, Margin = new(5, 0, 0, 0) };
                {
                    bool flag = (tickFrequency % 1 == 0);

                    var run_pre = new Run($"{sliderTextPrefix}{(sliderTextPrefix.Length == 0 ? "" : " ")}");
                    var run_value = new Run();
                    var run_suf = new Run($"{(sliderTextSuffix.Length == 0 ? "" : " ")}{sliderTextSuffix}");
                    textblock.Inlines.Add(run_pre);
                    textblock.Inlines.Add(run_value);
                    textblock.Inlines.Add(run_suf);

                    BindingOperations.SetBinding(run_value, Run.TextProperty, new Binding(propName) { Source = model, Mode = BindingMode.OneWay, StringFormat = $"{(flag ? "0" : "0.00")}" });
                }

                if (useonoffswitch)
                {
                    //doublebool2double
                    var temp = new cAggregateBindingControl() { Visibility = Visibility.Collapsed };
                    var btn = new cToggleButton() { GuideLineVisibility = Visibility.Collapsed, IsChecked = true, UseAlternateDotColor = true };
                    {
                        //将cToggleButton.IsChecked和Slider.Value绑定至cAggregateBindingControl的TextBlock.Text上
                        MultiBinding multiBinding = new MultiBinding() { Converter = new cSlider_converter_doublebool2double(), Mode = BindingMode.OneWay };
                        {
                            multiBinding.Bindings.Add(new Binding(nameof(Slider.Value)) { Source = slider });
                            multiBinding.Bindings.Add(new Binding(nameof(cToggleButton.IsChecked)) { Source = btn });
                        }
                        BindingOperations.SetBinding(temp, cAggregateBindingControl.DoubleTemp1Property, multiBinding);
                        BindingOperations.SetBinding(temp, cAggregateBindingControl.DoubleTemp2Property, new Binding(propName) { Source = model, Mode = BindingMode.OneWayToSource });
                    }
                    stackpanel.Children.Add(temp);
                    stackpanel.Children.Add(btn);
                }
                else
                {
                    BindingOperations.SetBinding(slider, Slider.ValueProperty, new Binding(propName) { Source = model });
                }

                stackpanel.Children.Add(slider);
                stackpanel.Children.Add(textblock);
                stackpanel.Visibility = hideself ? Visibility.Hidden : Visibility.Visible;
                stackpanel.Height = hideself ? 0 : stackpanel.Height;
                stackpanel.IsHitTestVisible = hideself is false;
                stackpanel.Margin = hideself ? new(0) : new(0, 1, 0, 1);
            }

            return stackpanel;
        }
        /// <summary>
        /// CreateComboBox
        /// </summary>
        protected static UIElement CreateComboBox(object model,
                                                  string itemSourcePropName,
                                                  string selectedItemPropName,
                                                  string commentText = "",
                                                  int defalutIndex = 0,
                                                  bool hideself = false,
                                                  HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch,
                                                  VerticalAlignment verticalAlignment = VerticalAlignment.Stretch)
        {
            var stackpanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = horizontalAlignment, VerticalAlignment = verticalAlignment };
            {
                var combobox = new cComboBox()
                {
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Width = 100
                };
                {
                    combobox.Loaded += (s, e) =>
                    {
                        combobox.SelectedIndex = defalutIndex;
                    };

                    BindingOperations.SetBinding(combobox, ComboBox.ItemsSourceProperty, new Binding(itemSourcePropName) { Source = model });
                    BindingOperations.SetBinding(combobox, ComboBox.SelectedItemProperty, new Binding(selectedItemPropName) { Source = model });
                }

                var textblock = new TextBlock() { Text = commentText, Foreground = new SolidColorBrush(Colors.White), VerticalAlignment = VerticalAlignment.Center, Margin = new(5, 0, 0, 0) };

                stackpanel.Children.Add(combobox);
                stackpanel.Children.Add(textblock);
                stackpanel.Visibility = hideself ? Visibility.Hidden : Visibility.Visible;
                stackpanel.Height = hideself ? 0 : stackpanel.Height;
                stackpanel.IsHitTestVisible = hideself is false;
                stackpanel.Margin = hideself ? new(0) : new(0, 1, 0, 1);
            }

            return stackpanel;
        }

        /// <summary>
        /// CreateComboBox
        /// </summary>
        protected static UIElement CreateComboBox(object model,
                                                  string itemSourcePropName,
                                                  string selectedItemPropName,
                                                  string selectedIndexPropName,
                                                  string commentText = "",
                                                  int defalutIndex = 0,
                                                  bool hideself = false,
                                                  HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch,
                                                  VerticalAlignment verticalAlignment = VerticalAlignment.Stretch)
        {
            var stackpanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = horizontalAlignment, VerticalAlignment = verticalAlignment };
            {
                var combobox = new cComboBox()
                {
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Width = 100
                };
                {
                    combobox.Loaded += (s, e) =>
                    {
                        combobox.SelectedIndex = defalutIndex;
                    };

                    BindingOperations.SetBinding(combobox, ComboBox.ItemsSourceProperty, new Binding(itemSourcePropName) { Source = model });
                    BindingOperations.SetBinding(combobox, ComboBox.SelectedItemProperty, new Binding(selectedItemPropName) { Source = model });
                    BindingOperations.SetBinding(combobox, ComboBox.SelectedIndexProperty, new Binding(selectedIndexPropName) { Source = model });
                }

                var textblock = new TextBlock() { Text = commentText, Foreground = new SolidColorBrush(Colors.White), VerticalAlignment = VerticalAlignment.Center, Margin = new(5, 0, 0, 0) };

                stackpanel.Children.Add(combobox);
                stackpanel.Children.Add(textblock);
                stackpanel.Visibility = hideself ? Visibility.Hidden : Visibility.Visible;
                stackpanel.Height = hideself ? 0 : stackpanel.Height;
                stackpanel.IsHitTestVisible = hideself is false;
                stackpanel.Margin = hideself ? new(0) : new(0, 1, 0, 1);
            }

            return stackpanel;
        }

        /// <summary>
        /// CreateValueIndicator
        /// </summary>
        protected static UIElement CreateValueIndicator(object model,
                                                        params ValueIndicatorPacket[] propPackets)
        {
            var stackpanel = new StackPanel() { Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Center };
            {
                foreach (var item in propPackets)
                {
                    var valueindicator = new cValueIndicator()
                    {
                        PropName = item.PropName,
                        PropNameColor = item.PropNameColor,
                        PropValueColor = item.PropValueColor,
                        PropValueColorSwitcher = item.PropValueColorSwitcher,
                        ColonColor = item.ColonColor,
                        BackgroundColor = item.BackgroundColor,
                        BackgroundCornerRadius = item.BackgroundCornerRadius,
                    };

                    BindingOperations.SetBinding(valueindicator, cValueIndicator.PropValueProperty, new Binding(item.PropName) { Source = model });

                    stackpanel.Children.Add(valueindicator);
                }
            }

            return stackpanel;
        }
        /// <summary>
        /// ValueIndicatorPacket
        /// </summary>
        protected class ValueIndicatorPacket
        {
            public string PropName;
            public SolidColorBrush PropNameColor;
            public SolidColorBrush PropValueColor;
            public Func<double, SolidColorBrush> PropValueColorSwitcher;
            public SolidColorBrush ColonColor;
            public SolidColorBrush BackgroundColor;
            public CornerRadius BackgroundCornerRadius;

            /// <summary>
            /// <para>propname:  属性名</para>
            /// <para>propnamecolor: 属性名的颜色</para>
            /// <para>propvaluecolor: 属性值的颜色</para>
            /// <para>propvaluecolorswitcher: 属性值的颜色的选择</para>
            /// <para>coloncolor: 冒号颜色</para>
            /// <para>backgroundcolor: 背景颜色</para>
            /// <para>backgroundcornerradius: 背景圆角半径</para>
            /// </summary>
            public ValueIndicatorPacket(string propname,
                                        SolidColorBrush? propnamecolor = null,
                                        SolidColorBrush? propvaluecolor = null,
                                        Func<double, SolidColorBrush>? propvaluecolorswitcher = null,
                                        SolidColorBrush? coloncolor = null,
                                        SolidColorBrush? backgroundcolor = null,
                                        CornerRadius? backgroundcornerradius = null)
            {
                PropName = propname;
                PropNameColor = propnamecolor ?? new(Colors.White);
                PropValueColor = propvaluecolor ?? new(Colors.White);
                PropValueColorSwitcher = propvaluecolorswitcher ?? (_ => { return this.PropValueColor; });
                ColonColor = coloncolor ?? new(Colors.White);
                BackgroundColor = backgroundcolor ?? new(Colors.Black);
                BackgroundCornerRadius = backgroundcornerradius ?? new(2.5);
            }
        }
    }
}

//MacroBase_SL
namespace CustomMacroBase
{
    public abstract partial class MacroBase
    {
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            AllowTrailingCommas = true,
            WriteIndented = true
        };

        record MacroConfigs<T>(MacroConfig Gate, T GateVM);
        record MacroConfig(bool Enable, List<MacroConfig>? Children);

        /// <summary>
        /// Save Config
        /// </summary>
        public void SaveConfig<T>(string key, ToggleTreeViewNode gate, T vm, string? saveFolder = null, string? saveFileName = null)
        {
            var savePath = string.Empty;
            var realFileName = saveFileName ?? $"{this.Title}({key}).json";
            if (!string.IsNullOrWhiteSpace(saveFolder))
            {
                try
                {
                    Directory.CreateDirectory(saveFolder);
                    savePath = Path.Combine(saveFolder, realFileName);
                }
                catch (Exception ex)
                {
                    Print($"Failed to create directory: {ex.Message}");
                    throw;
                }
            }
            else
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var realPath = Directory.Exists(directoryPath) ? directoryPath : desktopPath;
                savePath = Path.Combine(realPath, realFileName);
            }

            try
            {
                var root = new MacroConfig(MainGate.Enable, GenerateMacroConfig(MainGate));
                using (StreamWriter sw = new StreamWriter($"{savePath}", false, Encoding.Unicode))
                {
                    sw.WriteLine(JsonSerializer.Serialize(new MacroConfigs<T>(root, vm), jsonOptions));
                    Print($"SaveConfig: {this.Title}");
                }
            }
            catch (Exception ex)
            {
                Print(ex.Message);
            }
        }

        /// <summary>
        /// Load Config
        /// </summary>
        public void LoadConfig<T>(string key, ToggleTreeViewNode gate, Action<T> vmLoader, string? saveFolder = null, string? saveFileName = null)
        {
            var savePath = string.Empty;
            var realFileName = saveFileName ?? $"{this.Title}({key}).json";
            if (!string.IsNullOrWhiteSpace(saveFolder))
            {
                if (Directory.Exists(saveFolder))
                {
                    savePath = Path.Combine(saveFolder, realFileName);
                }
                else
                {
                    Print($"not found: {saveFolder}");
                    return;
                }
            }
            else
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var realPath = Directory.Exists(directoryPath) ? directoryPath : desktopPath;
                savePath = Path.Combine(realPath, realFileName);
            }

            try
            {
                if (File.Exists(savePath))
                {
                    string content = File.ReadAllText(savePath);
                    if (JsonSerializer.Deserialize(content, typeof(MacroConfigs<T>)) is MacroConfigs<T> jsonObj)
                    {
                        RestoreFromMacroConfig(MainGate, jsonObj.Gate);
                        vmLoader?.Invoke(jsonObj.GateVM);
                        Print($"LoadConfig: {this.Title}");
                    }
                }
            }
            catch (Exception ex)
            {
                Print(ex.Message);
            }
        }

        private List<MacroConfig>? GenerateMacroConfig(ToggleTreeViewNode gate)
        {
            var list = gate.Children.OfType<GateNode>().Where(node => node.Type == GateNodeType.GateBase);
            var result = new List<MacroConfig>();
            foreach (var node in list)
            {
                var item = (ToggleTreeViewNode)node.Content;
                result.Add(new MacroConfig(item.Enable, GenerateMacroConfig(item)));
            }
            return result.Count > 0 ? result : null;
        }
        private void RestoreFromMacroConfig(ToggleTreeViewNode gateRoot, MacroConfig jsonRoot, int depth = 0)
        {
            var isRoot = depth == 0; depth++;
            if (isRoot) { gateRoot.Enable = jsonRoot.Enable; }

            var gateChildren = gateRoot.Children.OfType<GateNode>().Where(node => node.Type == GateNodeType.GateBase);
            if (gateChildren is null) { return; }
            if (jsonRoot.Children is null) { return; }

            var gateList = gateChildren.ToList();
            var count = 0;
            foreach (var item in jsonRoot.Children)
            {
                var currentGate = (ToggleTreeViewNode)gateList[count++].Content;
                currentGate.Enable = item.Enable;

                RestoreFromMacroConfig(currentGate, item, depth);
            }
        }
    }
}