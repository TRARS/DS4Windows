using System;

namespace CustomMacroBase.CustomControlEx.ToggleButtonEx
{
    partial class cToggleButton_math
    {
        private static readonly Lazy<cToggleButton_math> lazyObject = new(() => new cToggleButton_math());
        public static cToggleButton_math Instance => lazyObject.Value;

        private cToggleButton_math() { }
    }

    partial class cToggleButton_math
    {
        public double WidthCalculator(double value)
        {
            return (double)value * 2 + 1;
        }
    }
}
