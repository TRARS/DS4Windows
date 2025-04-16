using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CustomMacroBase.CustomControlEx.ConsoleListBoxEx;
using CustomMacroBase.CustomControlEx.VerticalButtonEx;
using CustomMacroBase.DTOs;
using CustomMacroBase.Helper.Extensions;
using CustomMacroBase.Helper.HotKey;
using CustomMacroBase.Interfaces;
using CustomMacroBase.Messages;
using CustomMacroFactory.MVVM.Helpers;
using CustomMacroFactory.MVVM.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using TrarsUI.Shared.Controls.Specialized.ToggleTreeViewEx;
using TrarsUI.Shared.Controls.ToggleButtonEx;
using TrarsUI.Shared.Controls.VerticalButtonEx;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Interfaces.UIComponents;
using Helper = CustomMacroBase.Helper;

namespace CustomMacroFactory.MVVM.ViewModels
{
    partial class MacroViewerVM
    {
        [ObservableProperty]
        private ObservableCollection<IMacroPacket> macroPacketList = new();
        [ObservableProperty]
        private MacroPacket? selectedMacroPacket;


        [ObservableProperty]
        private UIElement mainMenu;
        [ObservableProperty]
        private UIElement mainOption;
        [ObservableProperty]
        private UIElement logArea;

        [ObservableProperty]
        private bool topContent_Left_Hide;
        [ObservableProperty]
        private bool topContent_Right_Hide;

        [ObservableProperty]
        private bool bottomContent_Bottom_Hide;

        public ObservableCollection<MenuItem_model> MenuItemModelList { get; set; } = new();
    }

    // Init
    partial class MacroViewerVM : ObservableObject, IContentVM
    {
        public string Title { get; set; } = $"Macro Extension ({System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location):yyyy-MM-dd HH:mm:ss})";

        private IDispatcherService _dispatcher;

        public MacroViewerVM(IDispatcherService dispatcher, Manager manager)
        {
            _dispatcher = dispatcher;

            //控制部分区域可见性
            this.TopContent_Left_Hide = false;      // menu
            this.TopContent_Right_Hide = false;     // macro list
            this.BottomContent_Bottom_Hide = false; // log

            //TopContent_Left
            this.MainMenu = new PartCreator<cVerticalButton>(container =>
            {
                container.Add(new() { Text = "Clear Log", Command = new RelayCommand(() => { WeakReferenceMessenger.Default.Send(new PrintCleanup("")); }) });
                container.Add(new() { Text = "Disconnect", Command = new RelayCommand(() => { WeakReferenceMessenger.Default.Send(new Ds4Disconnect("")); }) });
                container.Add(new()
                {
                    Text = "Picker",
                    Command = new RelayCommand(() => { manager.OpenImageColorPicker(); })
                });
                container.Add(new()
                {
                    Text = "Settings",
                    Command = new RelayCommand(() => { WeakReferenceMessenger.Default.Send(new PrintNewMessage("Right-click on this button to adjust settings related to the analog stick")); }),
                    RightClickContent = new ObservableCollection<UIElement>().Init(list =>
                    {
                        if (MacroFactory.MacroManager.AnalogStickMacro is CustomMacroBase.MacroBase pre)
                        {
                            list.Add(new cToggleTreeView() { DataContext = pre.MainGate });
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

            //TopContent_Right
            var selectedFirst = false;
            foreach (var item in MacroFactory.MacroManager.CurrentGameList)
            {
                MacroPacketList.Add(new MacroPacket(item.Title, item.MainGate)
                {
                    IsChecked = !selectedFirst,
                    IconData = "",
                    ColorfulText = item.UseColorfulText
                });
                if (selectedFirst is false) { selectedFirst = true; }
            }
            //MenuCommand
            this.MenuItemModelList.Add(new()
            {
                Text = "GameList Show/Hide",
                Command = new RelayCommand(() =>
                {
                    // 统计数量
                    int checkedCount = MacroPacketList.Count(x => x.IsChecked);
                    int notCheckedCount = MacroPacketList.Count(x => x.IsChecked is false);
                    int usedCount = MacroPacketList.Count(x => x.Unused is false);
                    int unusedCount = MacroPacketList.Count(x => x.Unused);

                    //
                    if (checkedCount == 0)
                    {
                        if (unusedCount == 0)
                        {
                            MacroPacketList.ForEach(x => { x.Unused = true; });
                        }
                        else
                        {
                            MacroPacketList.ForEach(x => { x.Unused = false; });
                        }
                    }
                    else if (checkedCount >= 1)
                    {
                        if (checkedCount != usedCount)
                        {
                            MacroPacketList.ForEach(x => { x.Unused = !x.IsChecked; });
                        }
                        else
                        {
                            MacroPacketList.ForEach(x => { x.Unused = false; });
                        }
                    }
                })
            });


            //BottomContent_Bottom
            this.LogArea = new PartCreator<ContentControl>(container =>
            {
                container.Add(new() { Content = new cConsoleListBox() });
            }).GetVerticalContent();
        }
    }

    // Command
    partial class MacroViewerVM
    {
        /// <summary>
        /// 上移
        /// </summary>
        [RelayCommand]
        private async Task OnMoveUpAsync(object para)
        {
            await _dispatcher.BeginInvoke(() =>
            {
                if (!(para is IMacroPacket item)) { return; }

                var list = this.MacroPacketList;
                var index = list.IndexOf(item);
                var count = list.Count;

                if (count > 1 && index > 0)
                {
                    var previous = list[index - 1]; // 获取 list[index - 1]
                    list.RemoveAt(index - 1); // 移除 previous
                    list.Insert(index, previous); // 将 previous 插入到 item 的旧位置
                }
            });
        }
        /// <summary>
        /// 下移
        /// </summary>
        [RelayCommand]
        private async Task OnMoveDownAsync(object para)
        {
            await _dispatcher.BeginInvoke(() =>
            {
                if (!(para is IMacroPacket item)) { return; }

                var list = this.MacroPacketList;
                var index = list.IndexOf(item);
                var count = list.Count;

                if (count > 1 && index > -1 && index < count - 1)
                {
                    var next = list[index + 1]; // 获取 list[index + 1]
                    list.RemoveAt(index + 1); // 移除 next
                    list.Insert(index, next); // 将 next 插入到 item 的旧位置
                }
            });
        }
        /// <summary>
        /// 启用/禁用
        /// </summary>
        [RelayCommand]
        private async Task OnUnusedAsync(object para)
        {
            await _dispatcher.BeginInvoke(() =>
            {
                if (!(para is IMacroPacket item)) { return; }

                item.Unused = !item.Unused;
            });
        }
        /// <summary>
        /// 删除
        /// </summary>
        [RelayCommand]
        private async Task OnRemoveMacroAsync(object para)
        {
            await _dispatcher.BeginInvoke(() =>
            {
                if (!(para is IMacroPacket item)) { return; }

                var list = this.MacroPacketList;
                var index = list.IndexOf(item);

                if (index > -1)
                {
                    item.IsChecked = false;
                    list.RemoveAt(index);
                    WeakReferenceMessenger.Default.Send(new PrintNewMessage($" Remove: {item.MacroContent.GetType().Name}"));
                }
            });
        }
    }
}
