﻿using CustomMacroBase.CustomControlEx.ComboBoxEx;
using CustomMacroBase.CustomControlEx.SliderEx;
using CustomMacroBase.CustomControlEx.StackPanelEx;
using CustomMacroBase.CustomControlEx.ValueIndicatorEx;
using CustomMacroBase.GamePadState;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.ProConSimulate;
using CustomMacroBase.PreBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

//GateBase
namespace CustomMacroBase.PreBase
{
    /// <summary>
    /// 滑块开关控件模型
    /// </summary>
    public partial class GateBase
    {
        /// <summary>
        /// <para>_text：开关注释</para>
        /// <para>_enable：开关状态</para>
        /// <para>_groupname：开关分组</para>
        /// </summary>
        public GateBase(string? _text = null, bool? _enable = null, string? _groupname = null)
        {
            this.Text = _text ?? this.Text;
            this.Enable = _enable ?? this.Enable;
            this.GroupName = _groupname ?? this.GroupName;
        }

        /// <summary>
        /// <para>通过下标访问自身Children列表中对应的子项</para>
        /// </summary>
        public GateBase this[int idx]
        {
            get
            {
                if ((idx >= 0) && (idx < Children.Count))
                {
                    return Children[idx];
                }
                else
                {
                    MessageBox.Show($"Index was outside the bounds of the array", $"Children[{idx}]");
                    return this;
                }
            }
        }

        /// <summary>
        /// <para>往自身Children列表添加子项</para>
        /// </summary>
        public void Add(GateBase child) => Children.Add(child);

        /// <summary>
        /// <para>往自身ChildrenEx列表添加子项</para>
        /// </summary>
        public void AddEx(Func<UIElement> child) => ChildrenEx.Add(child);

        /// <summary>
        /// 滑块开关状态反转
        /// </summary>
        public void EnableReverse() => this.Enable = !this.Enable;

        /// <summary>
        /// 震动一下以表示状态发生变化
        /// </summary>
        public void SetDs4Rumble() => Mediator.Instance.NotifyColleagues(MessageType.Ds4Rumble, this.Enable);

        /// <summary>
        /// 连起来
        /// </summary>
        public void EnableReverseAndSetDs4Rumble() => ((Action)(() => { EnableReverse(); SetDs4Rumble(); }))();
    }
    public partial class GateBase
    {
        /// <summary>
        /// 组名字典
        /// </summary>
        private static Dictionary<string, List<GateBase>> GroupNameList = new();
        /// <summary>
        /// 注册进组
        /// </summary>
        private static void RegisteGroupName(string groupname, GateBase item)
        {
            if (GroupNameList.ContainsKey(groupname) is false)
            {
                GroupNameList.Add(groupname, new List<GateBase>() { item });
            }
            else
            {
                GroupNameList[groupname].Add(item);
            }
        }
        /// <summary>
        /// 通知同组控件做出反应
        /// </summary>
        private static void NotifyGroupMemberUpdate(GateBase item)
        {
            if ((item.GroupName is null) || (item.Enable is false)) { return; }
            if (GroupNameList.ContainsKey(item.GroupName))
            {
                GroupNameList[item.GroupName].FindAll(_ => _.Equals(item) is false).ForEach(_ => { _.Enable = false; });
            }
        }
    }
    public partial class GateBase : NotificationObject
    {
        /// <summary>
        /// 获取或设置组名，使得处于相同组里的滑块开关在任意时刻最多只有一个处于启用状态
        /// </summary>
        public string? GroupName
        {
            get { return _groupname; }
            init
            {
                _groupname = value;
                if (_groupname is not null)
                {
                    RegisteGroupName(_groupname, this);
                    NotifyGroupMemberUpdate(this);
                }
            }
        }
        private string? _groupname = null;

        /// <summary>
        /// 获取或设置文本内容，用作滑块开关控件的注释
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; NotifyPropertyChanged(); }
        }
        private string _text = "Sub_NoName";

        /// <summary>
        /// 获取或设置滑块开关状态
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                NotifyPropertyChanged();
                NotifyGroupMemberUpdate(this);
            }
        }
        private bool _enable = false;

        /// <summary>
        /// 获取或设置滑块开关整体显示状态
        /// </summary>
        public bool HideSelf
        {
            get { return _hideSelf; }
            set
            {
                _hideSelf = value;
                NotifyPropertyChanged();
            }
        }
        private bool _hideSelf = false;

        /// <summary>
        /// 获取或设置SliderButton状态
        /// </summary>
        public bool DisableSliderButton
        {
            get { return _DisableSliderButton; }
            set
            {
                _DisableSliderButton = value;
                NotifyPropertyChanged();
            }
        }
        private bool _DisableSliderButton = false;



        /// <summary>
        /// 获取当前滑块开关的子项列表，用以存放与宿主类型相同的对象
        /// </summary>
        public List<GateBase> Children { get; init; } = new();
        /// <summary>
        /// 获取当前滑块开关的额外子项列表，用以存放委托
        /// </summary>
        public List<Func<dynamic>> ChildrenEx { get; init; } = new();
    }
}

//MacroInfo(model)
namespace CustomMacroBase.PreBase
{
    /// <summary>
    /// 脚本基类模型
    /// </summary>
    public class MacroModel
    {
        /// <summary>
        /// 获取或设置文本内容，将绑定到用户界面的按钮上
        /// </summary>
        public string Title { get; set; } = "NoTitle";

        /// <summary>
        /// 获取或设置被选中状态，将绑定到用户界面的按钮上
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// 获取或设置彩色字状态
        /// </summary>
        public bool ColorfulText { get; set; } = false;

        /// <summary>
        /// 主开关，既位于最外层的开关
        /// </summary>
        public GateBase MainGate { get; } = new() { Text = "Main_NoName", Enable = true };
    }
}

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
    public abstract partial class MacroBase : NotificationObject
    {
        #region 仅限基类内部访问的 字段
        private static DS4StateLite? rStateLite;//真实手柄
        private static DS4StateLite? vStateLite;//虚拟手柄
        private readonly MacroModel model = new();
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
        /// <para>参数 flag：<see cref="System.Boolean"/> 类型，是否获取最新画面（比如填'false'意为不获取新截图而是采用旧的截图进行找色）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="int"/> 类型，匹配到的颜色数量</para>
        /// </summary>
        protected static int FindColor(int color, Rectangle? rect, int? tolerance, bool flag = true)
        {
            return PixelMatcher.PixelMatcherHost.FindColor(color, rect, tolerance, flag);
        }


        /// <summary>
        /// <para>范围找图</para>
        /// <para>-</para>
        /// <para>参数 bitmap：<see cref="Bitmap"/>类型，匹配的小图</para>
        /// <para>参数 rect：<see cref="Rectangle"/>? 类型，描述匹配范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内，填'null'默认全图）</para>
        /// <para>参数 tolerance：<see cref="double"/>? 类型，匹配的相似度（范围0~1，比如填'null'意为使用默认值0.8）</para>
        /// <para>参数 flag：<see cref="bool"/> 类型，是否获取最新画面（比如填'true'意为先获取新截图再进行找图）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="System.Drawing.Point"/>? 类型，匹配成功返矩形左上角坐标，匹配失败返回null</para>
        /// </summary>
        protected static System.Drawing.Point? FindImage(ref Bitmap bitmap, Rectangle? rect, double? tolerance, bool flag = true)
        {
            return PixelMatcher.PixelMatcherHost.FindImage(ref bitmap, rect, tolerance, flag);
        }

        /// <summary>
        /// <para>范围找图</para>
        /// <para>-</para>
        /// <para>参数 path：<see cref="string"/> 类型，匹配的小图的绝对路径</para>
        /// <para>参数 rect：<see cref="Rectangle"/>? 类型，描述匹配范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内，填'null'默认全图）</para>
        /// <para>参数 tolerance：<see cref="double"/>? 类型，相似度（范围0~1，比如填'null'意为使用默认值0.8）</para>
        /// <para>参数 flag：<see cref="bool"/> 类型，是否获取最新画面（比如填'false'意为不获取新截图而是采用旧的截图进行找图）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="System.Drawing.Point"/>? 类型，匹配成功返矩形左上角坐标，匹配失败返回null</para>
        /// </summary>
        protected static System.Drawing.Point? FindImage(string path, Rectangle? rect, double? tolerance, bool flag = true)
        {
            return PixelMatcher.PixelMatcherHost.FindImage(path, rect, tolerance, flag);
        }

        //暂时先不判断黑底白字白底黑字，反正就自用，到时候再说
        ///// <summary>
        ///// <para>范围找字（仅限数字0~9）</para>
        ///// <para>-</para>
        ///// <para>参数 rect：<see cref="Rectangle"/> 类型，描述查找范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内）</para>
        ///// <para>参数 isWhiteText：<see cref="bool"/> 类型，输入图像是否为黑底白字</para>
        ///// <para>参数 flag：<see cref="bool"/> 类型，是否获取最新画面（比如填'false'意为不获取新截图而是采用旧的截图进行找字）</para>
        ///// <para>-</para>
        ///// <para>返回值：<see cref="string"/> 类型，识别成功返回字符串，识别失败返回空字符串</para>
        ///// </summary>
        //protected static string FindNumber(Rectangle rect, bool isWhiteText, bool flag = true)
        //{
        //    return PixelMatcher.PixelMatcherHost.FindNumber(rect, isWhiteText, flag);
        //}

        /// <summary>
        /// <para>范围找字（仅限数字0~9）</para>
        /// <para>-</para>
        /// <para>参数 rect：<see cref="Rectangle"/> 类型，描述查找范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内）</para>
        /// <para>参数 zoomratio：<see cref="double"/> 类型，缩放比例（比如填'2'意为宽高均拉伸至原来的2倍）</para>
        /// <para>参数 flag：<see cref="bool"/> 类型，是否获取最新画面（比如填'false'意为不获取新截图而是采用旧的截图进行找字）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="string"/> 类型，识别成功返回字符串，识别失败返回空字符串</para>
        /// </summary>
        protected static string FindNumber(Rectangle rect, double zoomratio = 2.0, bool flag = true)
        {
            return PixelMatcher.PixelMatcherHost.FindNumber(rect, true, zoomratio, flag);
        }

        /// <summary>
        /// <para>范围找字</para>
        /// <para>-</para>
        /// <para>参数 rect：<see cref="Rectangle"/> 类型，描述查找范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内）</para>
        /// <para>参数 iswhitetext：<see cref="bool"/> 类型，指定是否黑底白字（比如填'false'意为传入图像为白底黑字）</para>
        /// <para>参数 language：<see cref="string"/> 类型，指定语言（比如填'eng'、"jpn"，需在~/tessdata文件夹内存在对应训练数据）</para>
        /// <para>参数 whitelist：<see cref="string"/> 类型，指定白名单（比如填'abcd'意为只查找这几个字母，填null或空文本则意为不使用白名单）</para>
        /// <para>参数 zoomratio：<see cref="double"/> 类型，缩放比例（比如填'2'意为宽高均拉伸至原来的2倍）</para>
        /// <para>参数 flag：<see cref="bool"/> 类型，是否获取最新画面（比如填'false'意为不获取新截图而是采用旧的截图进行找字）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="string"/> 类型，识别成功返回字符串，识别失败返回空字符串</para>
        /// </summary>
        protected static string FindText(Rectangle rect, bool iswhitetext = true, string language = "eng", string whitelist = "", double zoomratio = 2.0, bool flag = true)
        {
            return PixelMatcher.PixelMatcherHost.FindText(rect, iswhitetext, language, whitelist, zoomratio, flag);
        }
        /// <summary>
        /// <para>范围找字</para>
        /// <para>-</para>
        /// <para>参数 rect：<see cref="Rectangle"/> 类型，描述查找范围的矩形（比如填'new(10, 20, 30, 40)'意为将范围限定在'x=10,y=20,width=30,height=40'的矩形内）</para>
        /// <para>参数 language：<see cref="string"/> 类型，指定语言（比如填'eng'、"jpn"，需在~/tessdata文件夹内存在对应训练数据）</para>
        /// <para>参数 whitelist：<see cref="string"/> 类型，指定白名单（比如填'abcd'意为只查找这几个字母，填null或空文本则意为不使用白名单）</para>
        /// <para>参数 zoomratio：<see cref="double"/> 类型，缩放比例（比如填'2'意为宽高均拉伸至原来的2倍）</para>
        /// <para>参数 flag：<see cref="bool"/> 类型，是否获取最新画面（比如填'false'意为不获取新截图而是采用旧的截图进行找字）</para>
        /// <para>-</para>
        /// <para>返回值：<see cref="string"/> 类型，识别成功返回字符串，识别失败返回空字符串</para>
        /// </summary>
        protected static string FindText(Rectangle rect, string language = "eng", string whitelist = "", double zoomratio = 2.0, bool flag = true)
        {
            return PixelMatcher.PixelMatcherHost.FindText(rect, true, language, whitelist, zoomratio, flag);
        }

        /// <summary>
        /// <para>打印文本</para>
        /// </summary>
        protected static void Print([CallerMemberName] string str = "")
        {
            Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
        }
        #endregion

        #region MacroModel属性筛选
        /// <summary>
        /// <para>顶层开关</para>
        /// </summary>
        public GateBase MainGate
        {
            get { return model.MainGate; }
        }
        /// <summary>
        /// <para>按钮文本，默认值为类名</para>
        /// </summary>
        public string Title
        {
            get { return model.Title; }
            set
            {
                model.Title = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// <para>按钮选中状态</para>
        /// </summary>
        public bool Selected
        {
            get { return model.Selected; }
            set
            {
                model.Selected = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// <para>按钮文本染色</para>
        /// </summary>
        public bool UseColorfulText
        {
            get { return model.ColorfulText; }
            set
            {
                model.ColorfulText = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region 脚本入口
        /// <summary>
        /// 入口
        /// </summary>
        public void UpdateEntry(in DS4StateLite _realState, in DS4StateLite _virtualState)
        {
            rStateLite = _realState;
            vStateLite = _virtualState;
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
            Title = Regex.Replace(this.GetType().Name, "Game_", string.Empty);
            MainGate.Text = "MainGate";

            this.Init();
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
        /// CreateGateBase
        /// </summary>
        protected static GateBase CreateGateBase(string text,
                                                 bool enable = true,
                                                 string? groupName = null,
                                                 bool hideself = false)
        {
            return new GateBase()
            {
                Text = text,
                Enable = enable,
                GroupName = groupName,
                HideSelf = hideself
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
                                                bool hideself = false)
        {
            var stackpanel = new cStackPanel() { GuideLineColor = new(Colors.WhiteSmoke) };
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
                        slider.Value = defalutValue;
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
        /// CreateComboBox
        /// </summary>
        protected static UIElement CreateComboBox(object model,
                                                  string itemSourcePropName,
                                                  string selectedItemPropName,
                                                  string commentText = "",
                                                  int defalutIndex = 0,
                                                  bool hideself = false)
        {
            var stackpanel = new cStackPanel() { GuideLineColor = new(Colors.WhiteSmoke) };
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


        ///// <summary>
        ///// CreateValueIndicator
        ///// </summary>
        //protected static UIElement CreateValueIndicator(object model, params string[] propNames)
        //{
        //    var stackpanel = new cStackPanel() { Orientation = Orientation.Vertical, GuideLineColor = new(Colors.SeaShell) };
        //    {
        //        foreach (var propName in propNames)
        //        {
        //            var valueindicator = new cValueIndicator() { PropName = propName, BackgroundColor = new(Colors.Black) };

        //            BindingOperations.SetBinding(valueindicator, cValueIndicator.PropValueProperty, new Binding(propName) { Source = model });

        //            stackpanel.Children.Add(valueindicator);
        //        }
        //    }

        //    return stackpanel;
        //}

        /// <summary>
        /// CreateValueIndicator
        /// </summary>
        protected static UIElement CreateValueIndicator(object model,
                                                        params ValueIndicatorPacket[] propPackets)
        {
            var stackpanel = new cStackPanel() { Orientation = Orientation.Vertical, GuideLineColor = new(Colors.SeaShell) };
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
            /// <para>backgroundcolor: 背景颜色</para>
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