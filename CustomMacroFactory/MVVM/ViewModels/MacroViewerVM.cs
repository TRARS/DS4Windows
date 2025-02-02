﻿using CommunityToolkit.Mvvm.ComponentModel;
using CustomMacroBase.CustomControlEx.ConsoleListBoxEx;
using CustomMacroBase.CustomControlEx.ToggleButtonEx;
using CustomMacroBase.CustomControlEx.ToggleButtonGroupEx;
using CustomMacroBase.CustomControlEx.VerticalButtonEx;
using CustomMacroBase.Helper.Extensions;
using CustomMacroBase.Helper.HotKey;
using CustomMacroFactory.MVVM.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;
using TrarsUI.Shared.Controls.VerticalRadioButtonEx;
using TrarsUI.Shared.Interfaces.UIComponents;
using Helper = CustomMacroBase.Helper;
using RelayCommand = CommunityToolkit.Mvvm.Input.RelayCommand;
namespace CustomMacroFactory.MVVM.ViewModels
{
    public partial class MacroViewerVM
    {
        [ObservableProperty]
        private UIElement mainMenu;
        [ObservableProperty]
        private UIElement mainOption;
        [ObservableProperty]
        private UIElement gameList;
        [ObservableProperty]
        private UIElement macroList;
        [ObservableProperty]
        private UIElement logArea;

        [ObservableProperty]
        private bool topContent_Left_Hide;
        [ObservableProperty]
        private bool topContent_Middle_Hide;
        [ObservableProperty]
        private bool topContent_Right_Hide;

        [ObservableProperty]
        private bool bottomContent_Bottom_Hide;

        public ObservableCollection<MenuItem_model> MenuItemModelList { get; set; } = new();
    }

    // Init
    public partial class MacroViewerVM : ObservableObject, IContentVM
    {
        public string Title { get; set; } = $"Macro Extension ({System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location):yyyy-MM-dd HH:mm:ss})";

        public MacroViewerVM()
        {
            //控制部分区域可见性
            this.TopContent_Left_Hide = false;      // menu
            this.TopContent_Middle_Hide = false;    // game list
            this.TopContent_Right_Hide = false;     // macro list
            this.BottomContent_Bottom_Hide = false; // log

            //TopContent_Left
            this.MainMenu = new PartCreator<cVerticalButton>(container =>
            {
                container.Add(new() { Text = "Clear Log", Command = new RelayCommand(() => { Helper.Mediator.Instance.NotifyColleagues(Helper.MessageType.PrintCleanup, null); }) });
                container.Add(new() { Text = "Disconnect", Command = new RelayCommand(() => { Helper.Mediator.Instance.NotifyColleagues(Helper.MessageType.Ds4Disconnect, null); }) });
                container.Add(new()
                {
                    Text = "Picker",
                    Command = new RelayCommand(() => { MVVM.Helpers.Manager.OpenImageColorPicker(); })
                });
                container.Add(new()
                {
                    Text = "Settings",
                    Command = new RelayCommand(() => { Helper.Mediator.Instance.NotifyColleagues(Helper.MessageType.PrintNewMessage, "Right-click on this button to adjust settings related to the analog stick"); }),
                    RightClickContent = new ObservableCollection<UIElement>().Init(list =>
                    {
                        if (MacroFactory.MacroManager.AnalogStickMacro is CustomMacroBase.MacroBase pre)
                        {
                            list.Add(new cToggleButtonGroup() { DataContext = pre.MainGate });
                        }
                    }),
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
                container.Add(new cToggleButton().Init(btn =>
                {
                    btn.DotCornerRadius = new(5);
                    btn.Margin = new(4, 0, 4, 0);
                    btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                    btn.VerticalAlignment = VerticalAlignment.Center;
                    btn.GuideLineColor = new SolidColorBrush(Colors.OrangeRed);
                    btn.Text = "Ex1";
                    btn.ToolTip = "Temporarily allow DS4W to recognize a virtual DS4 controller as a real DS4 controller once.";
                    btn.Checked += (s, e) => { Helper.SingleDs4Accessor.Instance.Reset(0); };
                    btn.Unchecked += (s, e) => { Helper.SingleDs4Accessor.Instance.Reset(1); };
                    btn.Loaded += (s, e) =>
                    {
                        ToolTipService.SetInitialShowDelay(btn, 256);
                        Helper.SingleDs4Accessor.Instance.OnSlotConsumed += () => { Application.Current.Dispatcher.Invoke(() => { btn.IsChecked = false; }); };
                    };
                }));
                container.Add(new cToggleButton().Init(btn =>
                {
                    btn.DotCornerRadius = new(5);
                    btn.Margin = new(4, 0, 4, 0);
                    btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                    btn.VerticalAlignment = VerticalAlignment.Center;
                    btn.GuideLineColor = new SolidColorBrush(Colors.OrangeRed);
                    btn.Text = "Ex2";
                    btn.ToolTip = "Allow using hotkeys to control toggles.";
                    btn.Checked += (s, e) => { RealKeyboard.Instance.StartMonitoring(); };
                    btn.Unchecked += (s, e) => { RealKeyboard.Instance.StopMonitoring(); };
                    btn.Loaded += (s, e) =>
                    {
                        ToolTipService.SetInitialShowDelay(btn, 256);
                    };
                }));

                container.Add(new cToggleButton().Init(btn =>
                {
                    btn.DotCornerRadius = new(5);
                    btn.Margin = new(4, 0, 4, 0);
                    btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                    btn.VerticalAlignment = VerticalAlignment.Center;
                    btn.GuideLineColor = new SolidColorBrush(Colors.OrangeRed);
                    btn.Text = "Ex3";
                    btn.ToolTip = "Allow the 2nd controller to access the Macro.";
                    btn.Checked += (s, e) => { Helper.GamepadInputMixer.Instance.Reset(0); };
                    btn.Unchecked += (s, e) => { Helper.GamepadInputMixer.Instance.Reset(1); };
                    btn.Loaded += (s, e) =>
                    {
                        ToolTipService.SetInitialShowDelay(btn, 256);
                    };
                }));
            }).GetVerticalContent();

            //TopContent_Middle
            this.GameList = new PartCreator<cVerticalRadioButton>(container =>
            {
                foreach (var item in MacroFactory.MacroManager.CurrentGameList)
                {
                    //每个游戏类安排一个按钮
                    container.Add(new cVerticalRadioButton() { AquaOnSelection = true }.Init(btn =>
                    {
                        btn.GroupName = "aS[8d7^_0>F+$B2|#q3&y>@uPr{4r";
                        btn.SetBinding(cVerticalRadioButton.EnableColorfulTextProperty, new Binding(nameof(item.UseColorfulText)) { Source = item, Mode = BindingMode.TwoWay });
                        btn.SetBinding(cVerticalRadioButton.TextProperty, new Binding(nameof(item.Title)) { Source = item, Mode = BindingMode.OneWay });
                        btn.SetBinding(cVerticalRadioButton.IsCheckedProperty, new Binding(nameof(item.Selected)) { Source = item, Mode = BindingMode.TwoWay });
                    }));
                }

                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (container.Count > 0) { container[0].IsChecked = true; }
                });

                //
                this.MenuItemModelList.Add(new()
                {
                    Text = "GameList Show/Hide",
                    Command = new RelayCommand(() => { container.ForEach(x => x.Hide = !x.Hide); })
                });
            }).GetVerticalContent();

            //TopContent_Right
            this.MacroList = new PartCreator<ContentControl>(container =>
            {
                foreach (var item in MacroFactory.MacroManager.CurrentGameList)
                {
                    //每个按钮对应具体内容
                    container.Add(new ContentControl().Init(temp =>
                    {
                        temp.Content = new cToggleButtonGroup() { DataContext = item.MainGate, Margin = new Thickness(0, 4, 4, 4) };
                        temp.SetBinding(ContentControl.VisibilityProperty, new Binding(nameof(item.Selected)) { Source = item, Mode = BindingMode.OneWay, Converter = new BooleanToVisibilityConverter() });
                    }));
                }
            }).GetVerticalContent();

            //BottomContent_Bottom
            this.LogArea = new PartCreator<ContentControl>(container =>
            {
                container.Add(new() { Content = new cConsoleListBox() });
            }).GetVerticalContent();
        }
    }
}
