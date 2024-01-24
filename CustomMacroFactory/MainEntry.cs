using CustomMacroBase.Helper;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CustomMacroFactory
{
    public sealed partial class MainEntry
    {
        public MainWindow.MainWindow MainView = new();
        public Func<object>? Ds4Host;
        public Func<object>? RootHub;

        public MainEntry() { }

        public void Init()
        {
            MainView.Show();

            Mediator.Instance.Register(MessageType.WindowPosReset, para =>
            {
                MainView.TryMoveToPrimaryMonitor((Vector)para);
            });

            Mediator.Instance.Register(MessageType.WindowClose, _ =>
            {
                MainView.WindowState = WindowState.Minimized;
            });

            Mediator.Instance.Register(MessageType.Ds4Disconnect, _ =>
            {
                dynamic? ds4 = Ds4Host?.Invoke();
                ds4?.CloseBT();
            });

            Mediator.Instance.Register(MessageType.Ds4Rumble, para =>
            {//byte rightLightFastMotor, byte leftHeavySlowMotor
                dynamic? rootHub = RootHub?.Invoke();
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
                dynamic? rootHub = RootHub?.Invoke();
                dynamic? d = rootHub?.DS4Controllers[0];
                ((double[])para)[0] = (d is not null ? d.Latency : 0);
            });
        }
    }
}
