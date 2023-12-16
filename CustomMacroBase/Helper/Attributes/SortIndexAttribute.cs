using System;

namespace CustomMacroBase.Helper.Attributes
{
    /// <summary>
    /// 排序索引，数字越小，位置越靠上
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public partial class SortIndexAttribute : Attribute
    {
        public int SortIndex { get; init; } = 0;

        public SortIndexAttribute(int _idx)
        {
            this.SortIndex = _idx;
        }
    }
}
