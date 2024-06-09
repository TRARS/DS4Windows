﻿using CustomMacroBase.CustomControlEx.ConsoleListBoxEx;
using CustomMacroBase.CustomControlEx.ToggleButtonEx;
using CustomMacroBase.CustomControlEx.ToggleButtonGroupEx;
using CustomMacroBase.CustomControlEx.VerticalButtonEx;
using CustomMacroBase.CustomControlEx.VerticalRadioButtonEx;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.Extensions;
using CustomMacroBase.Helper.HotKey;
using CustomMacroFactory.MainWindow.UserControlEx.PixelPicker;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace CustomMacroFactory.MainWindow.UserControlEx.ClientEx
{
    //model
    partial class uClient_viewmodel
    {
        //
        private uClient_model model = new();

        public ObservableCollection<cVerticalButton> TopContent_Left
        {
            get { return model.TopContent_Left; }
            set
            {
                if (model.TopContent_Left == value)
                    return;
                model.TopContent_Left = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<cToggleButton> TopContent_LeftEx
        {
            get { return model.TopContent_LeftEx; }
            set
            {
                if (model.TopContent_LeftEx == value)
                    return;
                model.TopContent_LeftEx = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<cVerticalRadioButton> TopContent_Middle
        {
            get { return model.TopContent_Middle; }
            set
            {
                if (model.TopContent_Middle == value)
                    return;
                model.TopContent_Middle = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<ContentControl> TopContent_Right
        {
            get { return model.TopContent_Right; }
            set
            {
                if (model.TopContent_Right == value)
                    return;
                model.TopContent_Right = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<ContentControl> BottomContent_Top
        {
            get { return model.BottomContent_Top; }
            set
            {
                if (model.BottomContent_Top == value)
                    return;
                model.BottomContent_Top = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<ContentControl> BottomContent_Bottom
        {
            get { return model.BottomContent_Bottom; }
            set
            {
                if (model.BottomContent_Bottom == value)
                    return;
                model.BottomContent_Bottom = value;
                NotifyPropertyChanged();
            }
        }

        public bool TopContent_Left_Hide
        {
            get => model.TopContent_Left_Hide;
            set
            {
                if (model.TopContent_Left_Hide == value)
                    return;
                model.TopContent_Left_Hide = value;
                NotifyPropertyChanged();
            }
        }
        public bool TopContent_Middle_Hide
        {
            get => model.TopContent_Middle_Hide;
            set
            {
                if (model.TopContent_Middle_Hide == value)
                    return;
                model.TopContent_Middle_Hide = value;
                NotifyPropertyChanged();
            }
        }
        public bool TopContent_Right_Hide
        {
            get => model.TopContent_Right_Hide;
            set
            {
                if (model.TopContent_Right_Hide == value)
                    return;
                model.TopContent_Right_Hide = value;
                NotifyPropertyChanged();
            }
        }
        public bool BottomContent_Top_Hide
        {
            get => model.BottomContent_Top_Hide;
            set
            {
                if (model.BottomContent_Top_Hide == value)
                    return;
                model.BottomContent_Top_Hide = value;
                NotifyPropertyChanged();
            }
        }
        public bool BottomContent_Bottom_Hide
        {
            get => model.BottomContent_Bottom_Hide;
            set
            {
                if (model.BottomContent_Bottom_Hide == value)
                    return;
                model.BottomContent_Bottom_Hide = value;
                NotifyPropertyChanged();
            }
        }

        //
        public ObservableCollection<MenuItem_model> MenuItemModelList
        {
            get => model.MenuItemModelList;
            set
            {
                if (model.MenuItemModelList == value)
                    return;
                model.MenuItemModelList = value;
                NotifyPropertyChanged();
            }
        }
    }

    //Init
    partial class uClient_viewmodel : NotificationObject
    {
        public uClient_viewmodel()
        {
            Init();
            Init2();
        }
    }

    //Init具体实现
    partial class uClient_viewmodel
    {
        /// <summary>
        /// 载入数据
        /// </summary>
        private void Init()
        {
            //获取MacroManager.MacroInfo，创建些许控件并分配给 TopContent_Middle 和 TopContent_Right

            this.TopContent_Left.Clear();
            this.TopContent_Middle.Clear();
            this.TopContent_Right.Clear();
            this.BottomContent_Top.Clear();
            this.BottomContent_Bottom.Clear();

            //控制部分区域可见性
            this.TopContent_Left_Hide = false;      // some buttons
            this.TopContent_Middle_Hide = false;    // game list
            this.TopContent_Right_Hide = false;     // macro list
            this.BottomContent_Top_Hide = false;    // rgb picker
            this.BottomContent_Bottom_Hide = false; // log

            //TopContent_Left
            {
                //清空打印 断开连接 拾色器 获取截图 临时通行 调整尺寸
                this.TopContent_Left.Add(new() { Text = "Clear Log", Command = () => { Mediator.Instance.NotifyColleagues(MessageType.PrintCleanup, null); } });
                this.TopContent_Left.Add(new() { Text = "Disconnect", Command = () => { Mediator.Instance.NotifyColleagues(MessageType.Ds4Disconnect, null); } });
                this.TopContent_Left.Add(new()
                {
                    Text = "RGB Picker",
                    Command = () => { Mediator.Instance.NotifyColleagues(MessageType.PixelPickerOnOff, null); },
                    SubContent = new()
                    {
                        new() { SubFlag = true, Text = "Snapshot", Command = () => { CustomMacroBase.PixelMatcher.PixelMatcherHost.GetTargetWindowSnapshotEx(); }, ToolTip = "Get a screenshot of the target window" },
                        new() { SubFlag = true, Text = "Resize(1080P)", Command = () => { CustomMacroBase.PixelMatcher.PixelMatcherHost.SetTargetWindowSizeEx(1920, 1080); }, ToolTip = "Resize the target window (client area) to 1920x1080" },
                        new() { SubFlag = true, Text = "Resize(720P)", Command = () => { CustomMacroBase.PixelMatcher.PixelMatcherHost.SetTargetWindowSizeEx(1280, 720); }, ToolTip = "Resize the target window (client area) to 1280x720" },
                    }
                });
                this.TopContent_Left.Add(new()
                {
                    Text = "Settings",
                    Command = () => { Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, "Right-click on this button to adjust settings related to the analog stick"); },
                    RightClickContent = ((Func<ObservableCollection<UIElement>>)(() =>
                    {
                        if (MacroFactory.MacroManager.AnalogStickMacro is CustomMacroBase.MacroBase pre)
                        {
                            return new() { new cToggleButtonGroup(pre.MainGate) };
                        }
                        return null;
                    })).Invoke(),
                });
                this.TopContent_Left.Add(((Func<cVerticalButton>)(() =>
                {
                    string pathData = "M512 857.6c-190.592 0-345.6-155.008-345.6-345.6S321.408 166.4 512 166.4s345.6 155.008 345.6 345.6-155.008 345.6-345.6 345.6z m0-640C349.696 217.6 217.6 349.696 217.6 512s132.096 294.4 294.4 294.4c162.304 0 294.4-132.096 294.4-294.4S674.304 217.6 512 217.6z" +
                                      "M972.8 537.6H51.2a25.6 25.6 0 1 1 0-51.2h921.6a25.6 25.6 0 1 1 0 51.2z" +
                                      "M512 998.4a25.6 25.6 0 0 1-25.6-25.6V51.2a25.6 25.6 0 1 1 51.2 0v921.6a25.6 25.6 0 0 1-25.6 25.6z";
                    return new cAimCursorButton()
                    {
                        IconOnly = true,
                        Icon = new()
                        {
                            new System.Windows.Shapes.Path()
                            {
                                Width = 25 - 2,
                                Height = 25 - 2,
                                Fill = new SolidColorBrush(Colors.Transparent),
                                Stroke = new SolidColorBrush(Colors.White),
                                IsHitTestVisible = false,
                                Stretch = Stretch.Fill,
                                Data = Geometry.Parse(pathData.ToString(CultureInfo.InvariantCulture)),
                                Effect = new DropShadowEffect()
                                {
                                    Color = Colors.Blue,
                                    ShadowDepth = 0,
                                    BlurRadius = 5,
                                    Opacity = 0.5
                                }
                            }
                        }
                    };
                })).Invoke());
            }
            //TopContent_LeftEx
            {
                this.TopContent_LeftEx.Add(new cToggleButton()
                {
                    Margin = new(4, 0, 4, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    GuideLineColor = new SolidColorBrush(Colors.OrangeRed),
                    Text = "Ex1",
                    ToolTip = "Temporarily allow DS4W to recognize a virtual DS4 controller as a real DS4 controller once.",
                    CheckedAct = () => { SingleDs4Accessor.Instance.Reset(0); },
                    UncheckedAct = () => { SingleDs4Accessor.Instance.Reset(1); },
                    LoadedAct = (self) =>
                    {
                        ToolTipService.SetInitialShowDelay(self, 256);
                        SingleDs4Accessor.Instance.OnSlotConsumed += () => { Application.Current.Dispatcher.Invoke(() => { self.IsChecked = false; }); };
                    }
                });
                this.TopContent_LeftEx.Add(new cToggleButton()
                {
                    Margin = new(4, 0, 4, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    GuideLineColor = new SolidColorBrush(Colors.OrangeRed),
                    Text = "Ex2",
                    ToolTip = "Allow using hotkeys to control toggles.",
                    CheckedAct = () => { RealKeyboard.Instance.StartMonitoring(); },
                    UncheckedAct = () => { RealKeyboard.Instance.StopMonitoring(); },
                    LoadedAct = (self) =>
                    {
                        ToolTipService.SetInitialShowDelay(self, 256);
                    }
                });
                this.TopContent_LeftEx.Add(new cToggleButton()
                {
                    Margin = new(4, 0, 4, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    GuideLineColor = new SolidColorBrush(Colors.OrangeRed),
                    Text = "Ex3",
                    ToolTip = "Allow the 2nd controller to access the Macro.",
                    CheckedAct = () => { GamepadInputMixer.Instance.Reset(0); },
                    UncheckedAct = () => { GamepadInputMixer.Instance.Reset(1); },
                    LoadedAct = (self) =>
                    {
                        ToolTipService.SetInitialShowDelay(self, 256);
                    }
                });
            }

            //TopContent_Middle
            {
                foreach (var item in MacroFactory.MacroManager.CurrentGameList)
                {
                    //TopContent_Middle 每个游戏类安排一个按钮
                    cVerticalRadioButton MC = new();
                    {
                        MC.SetBinding(cVerticalRadioButton.EnableColorfulTextProperty, new Binding(nameof(item.UseColorfulText)) { Source = item, Mode = BindingMode.TwoWay });
                        MC.SetBinding(cVerticalRadioButton.TextProperty, new Binding(nameof(item.Title)) { Source = item, Mode = BindingMode.OneWay });
                        MC.SetBinding(cVerticalRadioButton.IsCheckedProperty, new Binding(nameof(item.Selected)) { Source = item, Mode = BindingMode.TwoWay });
                    }
                    this.TopContent_Middle.Add(MC);
                }

                //选中第一项
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (this.TopContent_Middle.Count > 0) { this.TopContent_Middle[0].IsChecked = true; }
                });
            }

            // TopContent_Right
            {
                foreach (var item in MacroFactory.MacroManager.CurrentGameList)
                {
                    //TopContent_Right 每个按钮对应具体内容
                    ContentControl RC = new();
                    {
                        Grid grid = new() { Margin = new Thickness(0, 4, 4, 4) };
                        {
                            //
                            grid.Children.Add(new cToggleButtonGroup(item.MainGate));
                        }
                        RC.Content = grid;
                        RC.SetBinding(ContentControl.VisibilityProperty, new Binding(nameof(item.Selected)) { Source = item, Mode = BindingMode.OneWay, Converter = new BooleanToVisibilityConverter() });
                    }
                    this.TopContent_Right.Add(RC);
                }
            }

            //BottomContent 打印栏
            {
                this.BottomContent_Top.Add(new() { Content = new uPixelPicker() });
                this.BottomContent_Bottom.Add(new() { Content = new cConsoleListBox() });
            }
        }

        /// <summary>
        /// 载入MenuItem
        /// </summary>
        private void Init2()
        {
            //临时隐藏
            this.MenuItemModelList.Add(new()
            {
                Text = "GameList Show/Hide",
                HideGameListCommand = new(_ =>
                {
                    this.TopContent_Middle.ForEach(x => x.Hide = !x.Hide);
                })
            });
        }
    }
}
