using CommunityToolkit.Mvvm.Messaging;
using CustomMacroBase.GamePadState;
using CustomMacroBase.Messages;
using CustomMacroFactory.MacroFactory;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using TrarsUI.Shared.Helpers.Extensions;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Interfaces.UIComponents;
using TrarsUI.Shared.Messages;
using TrarsUI.SourceGenerator.Attributes;

namespace CustomMacroFactory.MVVM.Views
{
    [UseChrome]
    public partial class MainWindow : Window, IMainWindow
    {
        ITokenProviderService _tokenProvider;
        IDebouncerService _debouncer;
        IStringEncryptorService _stringEncryptor;

        public MainWindow(ITokenProviderService tokenProvider, IDebouncerService debouncer, IStringEncryptorService stringEncryptor)
        {
            enableShadowLayer = false;

            _tokenProvider = tokenProvider;
            _debouncer = debouncer;
            _stringEncryptor = stringEncryptor;

            InitializeComponent();
            InitWindowBorderlessBehavior(); // 无边框
            InitWindowMessageWithToken(); // 注册消息

            WeakReferenceMessenger.Default.Unregister<WindowCloseMessage, string>(this, this.Token);
            WeakReferenceMessenger.Default.Register<WindowCloseMessage, string>(this, this.Token, async (r, m) =>
            {
                try
                {
                    Action? yesnoCallback = null;
                    this.Chrome.IsHitTestVisible = false;
                    if (await WeakReferenceMessenger.Default.Send(new DialogYesNoMessage("Exit ?", (x) => { yesnoCallback = x; }), this.Token))
                    {
                        canExit = false; shadowHelper?.Close();

                        WeakReferenceMessenger.Default.Send(new Ds4Disconnect(""));

                        ((Window)r).SetDoubleAnimation(OpacityProperty, Opacity, 0d, 256).ContinueWith(() =>
                        {
                            canExit = true;
                            this.UnRegister();
                            this.Close();
                            Environment.Exit(0);
                        }).Begin();
                    }
                    this.Chrome.IsHitTestVisible = true;
                    yesnoCallback?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"WindowCloseMessage error: {ex.Message}");
                }
            });
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
        /// Exit
        /// </summary>
        public void Exit()
        {
            WeakReferenceMessenger.Default.Send(new WindowCloseMessage("Close"), this.Token);
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
                this.WindowState = WindowState.Minimized; shadowHelper?.ShadowFadeInOut(null);
                this.ShowInTaskbar = false;
            }
        }

        /// <summary>
        /// Init
        /// </summary>
        public void Init(Func<dynamic> getDs4Host, Func<dynamic> getRootHub)
        {
            WeakReferenceMessenger.Default.Register<Ds4Disconnect>(this, (r, m) =>
            {
                dynamic? ds4 = getDs4Host?.Invoke();
                ds4?.CloseBT();
            });
            WeakReferenceMessenger.Default.Register<Ds4Rumble>(this, (r, m) =>
            {
                //byte rightLightFastMotor, byte leftHeavySlowMotor
                var para = m.Value;// bool

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
            WeakReferenceMessenger.Default.Register<Ds4Latency>(this, (r, m) =>
            {
                var para = m.Value; // double[]

                dynamic? rootHub = getRootHub?.Invoke();
                dynamic? d = rootHub?.DS4Controllers[0];
                para[0] = (d is not null ? d.Latency : 0);
            });

            this.Show();
        }
    }
}