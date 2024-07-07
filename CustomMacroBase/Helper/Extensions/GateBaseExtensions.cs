using CustomMacroBase.PreBase;

namespace CustomMacroBase.Helper.Extensions
{
    public static class GateBaseExtensions
    {
        public static GateBase Comment(this GateBase target, string comment)
        {
            target.TooltipSuffix = comment; return target;
        }
    }
}
