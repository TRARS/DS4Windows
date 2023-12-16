using System;

namespace CustomMacroBase.Helper.Attributes
{
    /// <summary>
    /// 记录编译时间
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public partial class VersionAttribute : Attribute
    {
        public DateTime ReleaseDate;

        public VersionAttribute(bool _flag)
        {
            ReleaseDate = System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location);
        }
    }
}
