using System;
using System.Threading;

namespace CustomMacroBase.Helper
{
    public sealed partial class GamepadInputMixer
    {
        private static readonly Lazy<GamepadInputMixer> lazyObject = new(() => new GamepadInputMixer());
        public static GamepadInputMixer Instance => lazyObject.Value;

        private GamepadInputMixer() { }
    }

    public sealed partial class GamepadInputMixer
    {
        private int slot = 1;
        public bool AllowMix() => slot == 0;
        public void Reset(int value = 0)
        {
            Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, value switch
            {
                0 => "2nd controller can now access to the macros.",
                1 => "Ex3 toggle switch has been reset.",
                _ => throw new NotImplementedException()
            });

            Interlocked.Exchange(ref slot, value);
        }
    }
}
