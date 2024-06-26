using CustomMacroBase.CustomControlEx.ConsoleListBoxEx;
using CustomMacroBase.CustomControlEx.ToggleButtonEx;
using CustomMacroBase.CustomControlEx.ToggleButtonGroupEx;
using CustomMacroBase.CustomControlEx.VerticalButtonEx;
using CustomMacroBase.CustomControlEx.VerticalRadioButtonEx;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.Extensions;
using CustomMacroBase.Helper.HotKey;
using CustomMacroFactory.MainView.UserControlEx.PixelPicker;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace CustomMacroFactory.MainView.UserControlEx.ClientEx
{
    public partial class uClient_viewmodel
    {
        private string _loadedMessage => $"{this.GetType().Name} is loaded";

        private readonly uClient_model model = new();

        public UIElement MainMenu
        {
            get { return model.PartCollection[0]; }
            set
            {
                if (model.PartCollection[0] == value)
                    return;
                model.PartCollection[0] = value;
                NotifyPropertyChanged();
            }
        }
        public UIElement MainOption
        {
            get { return model.PartCollection[1]; }
            set
            {
                if (model.PartCollection[1] == value)
                    return;
                model.PartCollection[1] = value;
                NotifyPropertyChanged();
            }
        }
        public UIElement GameList
        {
            get { return model.PartCollection[2]; }
            set
            {
                if (model.PartCollection[2] == value)
                    return;
                model.PartCollection[2] = value;
                NotifyPropertyChanged();
            }
        }
        public UIElement MacroList
        {
            get { return model.PartCollection[3]; }
            set
            {
                if (model.PartCollection[3] == value)
                    return;
                model.PartCollection[3] = value;
                NotifyPropertyChanged();
            }
        }
        public UIElement SnapshotArea
        {
            get { return model.PartCollection[4]; }
            set
            {
                if (model.PartCollection[4] == value)
                    return;
                model.PartCollection[4] = value;
                NotifyPropertyChanged();
            }
        }
        public UIElement LogArea
        {
            get { return model.PartCollection[5]; }
            set
            {
                if (model.PartCollection[5] == value)
                    return;
                model.PartCollection[5] = value;
                NotifyPropertyChanged();
            }
        }

        public bool TopContent_Left_Hide
        {
            get => model.DoubleCollection[0];
            set
            {
                if (model.DoubleCollection[0] == value)
                    return;
                model.DoubleCollection[0] = value;
                NotifyPropertyChanged();
            }
        }
        public bool TopContent_Middle_Hide
        {
            get => model.DoubleCollection[1];
            set
            {
                if (model.DoubleCollection[1] == value)
                    return;
                model.DoubleCollection[1] = value;
                NotifyPropertyChanged();
            }
        }
        public bool TopContent_Right_Hide
        {
            get => model.DoubleCollection[2];
            set
            {
                if (model.DoubleCollection[2] == value)
                    return;
                model.DoubleCollection[2] = value;
                NotifyPropertyChanged();
            }
        }
        public bool BottomContent_Top_Hide
        {
            get => model.DoubleCollection[3];
            set
            {
                if (model.DoubleCollection[3] == value)
                    return;
                model.DoubleCollection[3] = value;
                NotifyPropertyChanged();
            }
        }
        public bool BottomContent_Bottom_Hide
        {
            get => model.DoubleCollection[4];
            set
            {
                if (model.DoubleCollection[4] == value)
                    return;
                model.DoubleCollection[4] = value;
                NotifyPropertyChanged();
            }
        }

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

    // Init
    public partial class uClient_viewmodel : NotificationObject
    {
        public uClient_viewmodel()
        {
            //控制部分区域可见性
            this.TopContent_Left_Hide = false;      // menu
            this.TopContent_Middle_Hide = false;    // game list
            this.TopContent_Right_Hide = false;     // macro list
            this.BottomContent_Top_Hide = false;    // rgb picker
            this.BottomContent_Bottom_Hide = false; // log

            //TopContent_Left
            this.MainMenu = new PartCreator<cVerticalButton>(container =>
            {
                container.Add(new() { Text = "Clear Log", Command = () => { Mediator.Instance.NotifyColleagues(MessageType.PrintCleanup, null); } });
                container.Add(new() { Text = "Disconnect", Command = () => { Mediator.Instance.NotifyColleagues(MessageType.Ds4Disconnect, null); } });
                container.Add(new()
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
                container.Add(new()
                {
                    Text = "Settings",
                    Command = () => { Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, "Right-click on this button to adjust settings related to the analog stick"); },
                    RightClickContent = ((Func<ObservableCollection<UIElement>>)(() =>
                    {
                        var list = new ObservableCollection<UIElement>();
                        {
                            if (MacroFactory.MacroManager.AnalogStickMacro is CustomMacroBase.MacroBase pre)
                            {
                                list.Add(new cToggleButtonGroup() { DataContext = pre.MainGate });
                            }
                        }
                        return list;
                    })).Invoke(),
                });
                container.Add(new cAimCursorButton().Init(btn =>
                {
                    string pathData = "M512 857.6c-190.592 0-345.6-155.008-345.6-345.6S321.408 166.4 512 166.4s345.6 155.008 345.6 345.6-155.008 345.6-345.6 345.6z m0-640C349.696 217.6 217.6 349.696 217.6 512s132.096 294.4 294.4 294.4c162.304 0 294.4-132.096 294.4-294.4S674.304 217.6 512 217.6z" +
                                      "M972.8 537.6H51.2a25.6 25.6 0 1 1 0-51.2h921.6a25.6 25.6 0 1 1 0 51.2z" +
                                      "M512 998.4a25.6 25.6 0 0 1-25.6-25.6V51.2a25.6 25.6 0 1 1 51.2 0v921.6a25.6 25.6 0 0 1-25.6 25.6z";
                    btn.IconOnly = true;
                    btn.Icon = new()
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
                    };
                }));
            }).GetVerticalContent();

            //TopContent_LeftEx
            this.MainOption = new PartCreator<cToggleButton>(container =>
            {
                container.Add(new cToggleButton()
                {
                    DotCornerRadius = new(5),
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
                container.Add(new cToggleButton()
                {
                    DotCornerRadius = new(5),
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
                container.Add(new cToggleButton()
                {
                    DotCornerRadius = new(5),
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
            }).GetVerticalContent();

            //TopContent_Middle
            this.GameList = new PartCreator<cVerticalRadioButton>(container =>
            {
                foreach (var item in MacroFactory.MacroManager.CurrentGameList)
                {
                    //每个游戏类安排一个按钮
                    cVerticalRadioButton temp = new();
                    {
                        temp.SetBinding(cVerticalRadioButton.EnableColorfulTextProperty, new Binding(nameof(item.UseColorfulText)) { Source = item, Mode = BindingMode.TwoWay });
                        temp.SetBinding(cVerticalRadioButton.TextProperty, new Binding(nameof(item.Title)) { Source = item, Mode = BindingMode.OneWay });
                        temp.SetBinding(cVerticalRadioButton.IsCheckedProperty, new Binding(nameof(item.Selected)) { Source = item, Mode = BindingMode.TwoWay });
                    }
                    container.Add(temp);
                }

                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (container.Count > 0) { container[0].IsChecked = true; }
                });

                //
                this.MenuItemModelList.Add(new()
                {
                    Text = "GameList Show/Hide",
                    Command = new(_ => { container.ForEach(x => x.Hide = !x.Hide); })
                });
            }).GetVerticalContent();

            //TopContent_Right
            this.MacroList = new PartCreator<ContentControl>(container =>
            {
                foreach (var item in MacroFactory.MacroManager.CurrentGameList)
                {
                    //每个按钮对应具体内容
                    ContentControl temp = new();
                    {
                        temp.Content = new cToggleButtonGroup() { DataContext = item.MainGate, Margin = new Thickness(0, 4, 4, 4) };
                        temp.SetBinding(ContentControl.VisibilityProperty, new Binding(nameof(item.Selected)) { Source = item, Mode = BindingMode.OneWay, Converter = new BooleanToVisibilityConverter() });
                    }
                    container.Add(temp);
                }
            }).GetVerticalContent();

            //BottomContent_Top
            this.SnapshotArea = new PartCreator<ContentControl>(container =>
            {
                container.Add(new() { Content = MainEntry.GetService<uPixelPicker>() });
            }).GetVerticalContent();

            //BottomContent_Bottom
            this.LogArea = new PartCreator<ContentControl>(container =>
            {
                container.Add(new() { Content = new cConsoleListBox() });
            }).GetVerticalContent();
        }
    }
}
