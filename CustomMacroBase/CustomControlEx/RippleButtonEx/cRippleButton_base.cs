using System.Windows;
using System.Windows.Media.Animation;

namespace CustomMacroBase.CustomControlEx.RippleButtonEx
{
    public enum ButtonType
    {
        Noamal,
        Custom
    }

    public class CustomEasingFunction : EasingFunctionBase
    {
        public CustomEasingFunction() { }

        protected override double EaseInCore(double normalizedTime)
        {
            return normalizedTime;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new CustomEasingFunction();
        }
    }
}
