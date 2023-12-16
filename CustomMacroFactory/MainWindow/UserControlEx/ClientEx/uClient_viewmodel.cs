using CustomMacroBase.CustomControlEx.ConsoleListBoxEx;
using CustomMacroBase.CustomControlEx.ToggleButtonEx;
using CustomMacroBase.CustomControlEx.VerticalButtonEx;
using CustomMacroBase.CustomControlEx.VerticalRadioButtonEx;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.Extensions;
using CustomMacroFactory.MainWindow.UserControlEx.PixelPicker;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        //
        private bool is_in_designmode => (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

        //递归遍历所有子项
        private void GetChildren(StackPanel sp,
                                 CustomMacroBase.PreBase.GateBase parent_model,
                                 Thickness margin = new(),
                                 cToggleButton? parent_view = null,
                                 double container_bk_opacity = 0.0)
        {
            //container_bk_opacity = Math.Clamp(container_bk_opacity + 0.3, 0, 1);
            //var container_bk_color = Colors.DarkSlateGray;

            var mg = margin with { Left = margin.Left + 15, Top = 0 };
            var isroot = parent_view is null;

            if (isroot)
            {
                cToggleButton root_view = new() { VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0) };
                root_view.SetBinding(cToggleButton.TextProperty, new Binding("Text") { Source = parent_model, Mode = BindingMode.OneWay });
                root_view.SetBinding(cToggleButton.IsCheckedProperty, new Binding("Enable") { Source = parent_model, Mode = BindingMode.TwoWay });
                root_view.SetBinding(cToggleButton.VisibilityProperty, new Binding("HideSelf") { Source = parent_model, Mode = BindingMode.OneWay, Converter = new uClient_converter_bool2visibility() });
                root_view.SetBinding(cToggleButton.DisableSliderButtonProperty, new Binding("DisableSliderButton") { Source = parent_model, Mode = BindingMode.OneWay });
                //root_view.BackgroundColor = new SolidColorBrush(Color.FromArgb((byte)Math.Clamp(container_bk_opacity * 255, 0, 255), container_bk_color.R, container_bk_color.G, container_bk_color.B));
                sp.Children.Add(root_view);
            }

            //Children
            foreach (var child_model in parent_model.Children)
            {
                cToggleButton child_view = new() { VerticalAlignment = VerticalAlignment.Center, Margin = mg };
                if (isroot)
                {
                    child_view.SetBinding(cToggleButton.EnableProperty, new Binding("Enable") { Source = parent_model, Mode = BindingMode.OneWay });
                }
                if (!isroot)
                {
                    Binding b1 = new("Enable") { Source = parent_view, Mode = BindingMode.OneWay };
                    Binding b2 = new("Enable") { Source = parent_model, Mode = BindingMode.OneWay };
                    MultiBinding mb = new();
                    mb.Bindings.Add(b1);
                    mb.Bindings.Add(b2);
                    mb.Converter = new uClient_converter_boolbool2bool();
                    child_view.SetBinding(cToggleButton.EnableProperty, mb);
                }
                child_view.SetBinding(cToggleButton.TextProperty, new Binding("Text") { Source = child_model, Mode = BindingMode.OneWay });
                child_view.SetBinding(cToggleButton.IsCheckedProperty, new Binding("Enable") { Source = child_model, Mode = BindingMode.TwoWay });
                child_view.SetBinding(cToggleButton.VisibilityProperty, new Binding("HideSelf") { Source = child_model, Mode = BindingMode.OneWay, Converter = new uClient_converter_bool2visibility() });
                child_view.SetBinding(cToggleButton.DisableSliderButtonProperty, new Binding("DisableSliderButton") { Source = child_model, Mode = BindingMode.OneWay });
                //child_view.BackgroundColor = new SolidColorBrush(Color.FromArgb((byte)Math.Clamp(container_bk_opacity * 255, 0, 255), container_bk_color.R, container_bk_color.G, container_bk_color.B));
                sp.Children.Add(child_view);

                GetChildren(sp, child_model, mg, child_view, container_bk_opacity);
            }

            //ChildrenEx
            foreach (var child_delegate in parent_model.ChildrenEx)
            {
                Grid child_view = new() { VerticalAlignment = VerticalAlignment.Center, Margin = mg };
                if (isroot)
                {
                    child_view.SetBinding(Grid.OpacityProperty, new Binding("Enable") { Source = parent_model, Mode = BindingMode.OneWay, Converter = new uClient_converter_bool2opacity(), ConverterParameter = 0.25 });//
                    child_view.SetBinding(Grid.IsHitTestVisibleProperty, new Binding("Enable") { Source = parent_model, Mode = BindingMode.OneWay });//
                }
                if (!isroot)
                {
                    Binding b1 = new("Enable") { Source = parent_view, Mode = BindingMode.OneWay };
                    Binding b2 = new("Enable") { Source = parent_model, Mode = BindingMode.OneWay };
                    {
                        MultiBinding mb = new();
                        mb.Bindings.Add(b1);
                        mb.Bindings.Add(b2);
                        mb.Converter = new uClient_converter_boolbool2bool();//禁用时，禁止鼠标命中
                        child_view.SetBinding(Grid.IsHitTestVisibleProperty, mb);
                    }
                    {
                        MultiBinding mb = new() { ConverterParameter = 0.25 };
                        mb.Bindings.Add(b1);
                        mb.Bindings.Add(b2);
                        mb.Converter = new uClient_converter_boolbool2double();//禁用时，降低不透明度
                        child_view.SetBinding(Grid.OpacityProperty, mb);
                    }
                }

                dynamic? obj = child_delegate.Invoke();//生成额外子项
                if (obj is not null)
                {
                    try { obj.SetBinding(UIElement.AllowDropProperty, new Binding("Enable") { Source = parent_model, Mode = BindingMode.OneWay }); }
                    catch { }
                    child_view.Children.Add(obj);
                    sp.Children.Add(child_view);
                }
            }
        }

        //载入数据
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
                        new() { SubFlag = true, Text = "Snapshot", Command = () => { CustomMacroBase.PixelMatcher.PixelMatcherHost.CopyToClipboardEx(); }, ToolTip = "Get a screenshot of the target window" },
                        new() { SubFlag = true, Text = "Resize", Command = () => { CustomMacroBase.PixelMatcher.PixelMatcherHost.SetTargetWindowSizeEx(1920, 1080); }, ToolTip = "Resize the target window (client area) to 1920x1080" },
                    }
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
                this.TopContent_LeftEx.Add(((Func<cToggleButton>)(() =>
                {
                    var ex1 = new cToggleButton()
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
                    };

                    return ex1;
                })).Invoke());

            }

            //TopContent_Middle
            {
                foreach (var item in MacroFactory.MacroManager.GameList)
                {
                    //TopContent_Middle 每个游戏类安排一个按钮
                    cVerticalRadioButton MC = new();
                    {
                        MC.ColorfulText = is_in_designmode;
                        MC.SetBinding(cVerticalRadioButton.TextProperty, new Binding("Title") { Source = item, Mode = BindingMode.OneWay });
                        MC.SetBinding(cVerticalRadioButton.IsCheckedProperty, new Binding("Selected") { Source = item, Mode = BindingMode.TwoWay });
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
                foreach (var item in MacroFactory.MacroManager.GameList)
                {
                    //TopContent_Right 每个按钮对应具体内容
                    ContentControl RC = new();
                    {
                        Grid grid = new() { Margin = new Thickness(4) };
                        {
                            StackPanel sp = new() { Orientation = Orientation.Vertical };
                            {
                                GetChildren(sp, item.MainGate);
                            }
                            grid.Children.Add(sp);
                        }
                        RC.Content = grid;
                        RC.SetBinding(ContentControl.VisibilityProperty, new Binding("Selected") { Source = item, Mode = BindingMode.OneWay, Converter = new BooleanToVisibilityConverter() });
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

        //
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
