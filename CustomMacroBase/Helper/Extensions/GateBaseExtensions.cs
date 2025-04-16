
using TrarsUI.Shared.DTOs;

namespace CustomMacroBase.Helper.Extensions
{
    public static class ToggleTreeViewNodeExtensions
    {
        public static ToggleTreeViewNode Comment(this ToggleTreeViewNode target, string comment)
        {
            target.TooltipSuffix = comment; return target;
        }
    }
}
