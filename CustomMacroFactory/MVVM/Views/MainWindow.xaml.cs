using CommunityToolkit.Mvvm.Messaging;
using CustomMacroBase.GamePadState;
using CustomMacroBase.Helper;
using CustomMacroFactory.MacroFactory;
using System;
using System.Threading.Tasks;
using System.Windows;
using TrarsUI.Shared.DTOs;
using TrarsUI.Shared.Helper;
using TrarsUI.Shared.Helper.Extensions;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Interfaces.UIComponents;
using TrarsUI.Shared.Messages;
using TrarsUI.Shared.ShadowLayer;
using TrarsUI.SourceGenerator.Attributes;

namespace CustomMacroFactory.MVVM.Views
{
    [UseChrome]
    public partial class MainWindow : Window, IMainWindow
    {
        bool canExit = false;
        bool canDebounce = false;
        ShadowHelper shadowHelper = new();

        public Action? OnTaskbarMinimize { get; set; }

        public MainWindow(ITokenProviderService tokenProvider, IDebouncerService debouncer)
        {
            InitializeComponent(); Left = Top = 0; // this.Opacity = 0;
            InitWindowBorderlessBehavior(); // 无边框

            // 通过任务栏图标最小化
            OnTaskbarMinimize = () =>
            {
                WeakReferenceMessenger.Default.Send(new WindowMinimizeMessage("OnTaskbarMinimize"), this.Token);
            };

            // 设置Token
            this.Token = tokenProvider.MainWindowToken;

            // 标题
            WeakReferenceMessenger.Default.Register<WindowTitleChangedMessage, string>(this, this.Token, (r, m) =>
            {
                this.Title = m.Value;
            });

            // 顶置
            WeakReferenceMessenger.Default.Register<WindowTopmostMessage, string>(this, this.Token, (r, m) =>
            {
                var flag = !((MainWindow)r).Topmost;
                shadowHelper.ShadowTopmost(this.Token, flag);

                ((MainWindow)r).Topmost = flag;
                m.Reply(flag);
            });

            // 重置位置
            WeakReferenceMessenger.Default.Register<WindowPosResetMessage, string>(this, this.Token, async (r, m) =>
            {
                canDebounce = true;
                shadowHelper.ShadowFadeInOut(this.Token, false); await Task.Delay(64);

                ((MainWindow)r).SetDoubleAnimation(OpacityProperty, Opacity, 0d, 96).ContinueWith(async () =>
                {
                    await Task.Delay(64);
                    ((MainWindow)r).TryMoveToPrimaryMonitor(m.Value);
                    await Task.Delay(96);
                    ((MainWindow)r).SetDoubleAnimation(OpacityProperty, Opacity, 1d, 192).ContinueWith(async () =>
                    {
                        shadowHelper.ShadowFadeInOut(this.Token, true);
                        await Task.Delay(128); canDebounce = false;
                        await shadowHelper.ShadowZindex(this.Token);
                    }).Begin();
                }).Begin();
            });

            // 最小化
            WeakReferenceMessenger.Default.Register<WindowMinimizeMessage, string>(this, this.Token, async (r, m) =>
            {
                shadowHelper.ShadowFadeInOut(this.Token, null);
                await Task.Delay(64);
                ((MainWindow)r).WindowState = WindowState.Minimized;
            });

            // 最大化
            WeakReferenceMessenger.Default.Register<WindowMaximizeMessage, string>(this, this.Token, (r, m) =>
            {
                ((MainWindow)r).WindowState = (((MainWindow)r).WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
            });

            // 关闭
            WeakReferenceMessenger.Default.Register<WindowCloseMessage, string>(this, this.Token, (r, m) =>
            {
                canExit = false; shadowHelper.Close(this.Token);

                ((MainWindow)r).SetDoubleAnimation(OpacityProperty, Opacity, 0d, 256).ContinueWith(() =>
                {
                    canExit = true;
                    this.UnRegister();
                    this.Close();
                    Environment.Exit(0);
                }).Begin();
            });

            // 窗体截图
            WeakReferenceMessenger.Default.Register<WindowSaveToTransparentPngMessage, string>(this, this.Token, async (r, m) =>
            {
                await Task.Delay(1000);
                ((MainWindow)r).SaveToPng(allowTransparency: true);
            });

            // Loaded
            this.Loaded += async (s, e) =>
            {
                var stateInfo = new WindowStateInfo(this.WindowState == WindowState.Maximized, this.Topmost);
                WeakReferenceMessenger.Default.Send(new WindowStateUpdateMessage(stateInfo), this.Token);

                // 阴影
                var mainLayer = this;
                var shadowLayer = shadowHelper.CreateSubWindow<Shadow>(shadow =>
                {
                    shadowHelper.ShadowInit(this.Token, shadow, mainLayer, mainLayer.PART_Border.CornerRadius);
                });

                await Task.Delay(512);

                mainLayer.Activate();
            };

            // StateChanged
            this.StateChanged += async (s, e) =>
            {
                var stateInfo = new WindowStateInfo(this.WindowState == WindowState.Maximized, this.Topmost);
                WeakReferenceMessenger.Default.Send(new WindowStateUpdateMessage(stateInfo), this.Token);

                var flag = this.WindowState == WindowState.Normal;
                if (this.WindowState != WindowState.Minimized)
                {
                    shadowHelper.ShadowFadeInOut(this.Token, flag);
                }

                if (this.WindowState == WindowState.Normal)
                {
                    await Task.Delay(128);
                    await shadowHelper.ShadowZindex(this.Token);
                }
            };

            // LocationChanged
            this.LocationChanged += (s, e) =>
            {
                shadowHelper.ShadowMove(this.Token);
            };

            // SizeChanged
            this.SizeChanged += (s, e) =>
            {
                shadowHelper.ShadowMove(this.Token);
            };

            // Activated
            this.Activated += async (s, e) =>
            {
                await shadowHelper.ShadowZindex(this.Token);
                shadowHelper.ShadowBlurRadius(this.Token, true);
                this.IsActive = true;

                debouncer.Cancel();
            };

            // Deactivated
            this.Deactivated += async (s, e) =>
            {
                await shadowHelper.ShadowZindex(this.Token);
                shadowHelper.ShadowBlurRadius(this.Token, false);

                // 防闪烁
                if (canDebounce)
                {
                    await debouncer.DebounceAsync(async () =>
                    {
                        await Task.Delay(1); this.IsActive = false;
                    }, TimeSpan.FromMilliseconds(192));
                }
                else
                {
                    this.IsActive = false;
                }
            };

            // Closing
            this.Closing += (s, e) =>
            {
                e.Cancel = !canExit;
                WeakReferenceMessenger.Default.Send(new WindowCloseMessage("Closing"), this.Token);
            };
        }
    }

    // Method
    public partial class MainWindow
    {
        /// <summary>
        /// MacroEntry
        /// </summary>
        public void MacroEntry(in int ind, in DS4StateLite _realState, in DS4StateLite _virtualState)
        {
            MacroManager.Entry(in ind, in _realState, in _virtualState);
        }

        /// <summary>
        /// Exit ?
        /// </summary>
        public void Exit()
        {
            shadowHelper.Close(this.Token);
        }

        /// <summary>
        /// HideToTray
        /// </summary>
        public void HideToTray()
        {
            if (this.WindowState is WindowState.Minimized)
            {
                this.WindowState = (this.ShowInTaskbar ? this.WindowState : WindowState.Normal);
                this.ShowInTaskbar = !this.ShowInTaskbar;
            }
            else
            {
                OnTaskbarMinimize?.Invoke(); //this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        /// <summary>
        /// Init
        /// </summary>
        public void Init(Func<dynamic> getDs4Host, Func<dynamic> getRootHub)
        {
            Mediator.Instance.Register(MessageType.Ds4Disconnect, _ =>
            {
                dynamic? ds4 = getDs4Host?.Invoke();
                ds4?.CloseBT();
            });
            Mediator.Instance.Register(MessageType.Ds4Rumble, para =>
            {//byte rightLightFastMotor, byte leftHeavySlowMotor
                dynamic? rootHub = getRootHub?.Invoke();
                dynamic? d = rootHub?.DS4Controllers[0];
                byte LightRumble = (bool)para ? byte.MinValue : byte.MaxValue;
                byte HeavyRumble = (bool)para ? byte.MaxValue : byte.MinValue;
                if (d is not null)
                {
                    Task.Run(() =>
                    {
                        d.setRumble(LightRumble, HeavyRumble);//LightRumble //HeavyRumble
                        Task.Delay(200).Wait();
                        d.setRumble(0, 0);
                        Task.Delay(200).Wait();
                    });
                }
            });
            Mediator.Instance.Register(MessageType.Ds4Latency, para =>
            {
                dynamic? rootHub = getRootHub?.Invoke();
                dynamic? d = rootHub?.DS4Controllers[0];
                ((double[])para)[0] = (d is not null ? d.Latency : 0);
            });

            this.Show();
        }
    }
}