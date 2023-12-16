using System;

namespace CustomMacroBase.Helper.Attributes
{
    /// <summary>
    /// 是否加载
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public partial class DoNotLoadAttribute : Attribute
    {
        public bool DoNotLoad { get; init; } = false;

        public DoNotLoadAttribute()
        {
            this.DoNotLoad = true;
        }

        public DoNotLoadAttribute(bool _flag)
        {
            this.DoNotLoad = _flag;
        }

        public DoNotLoadAttribute(YesNo _flag)
        {
            this.DoNotLoad = _flag is YesNo.Yes;
        }
    }

    public enum YesNo
    {
        Yes,
        No,
    }
}
