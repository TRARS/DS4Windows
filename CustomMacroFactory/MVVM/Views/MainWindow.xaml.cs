using CommunityToolkit.Mvvm.Messaging;
using CustomMacroBase.GamePadState;
using CustomMacroBase.Helper;
using CustomMacroFactory.MacroFactory;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using TrarsUI.Shared.Helper.Extensions;
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
                    if (await WeakReferenceMessenger.Default.Send(new DialogYesNoMessage("Exit ?", (x) => { yesnoCallback = x; }), this.Token))
                    {
                        canExit = false; shadowHelper.Close();

                        ((Window)r).SetDoubleAnimation(OpacityProperty, Opacity, 0d, 256).ContinueWith(() =>
                        {
                            canExit = true;
                            this.UnRegister();
                            this.Close();
                            Environment.Exit(0);
                        }).Begin();
                    }
                    yesnoCallback?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"WindowCloseMessage error: {ex.Message}");
                }
            });

            this.Closing += (s, e) => 
            {
                Mediator.Instance.NotifyColleagues(MessageType.Ds4Disconnect, null);
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
            shadowHelper.Close();
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
                this.WindowState = WindowState.Minimized; shadowHelper.ShadowFadeInOut(null);
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