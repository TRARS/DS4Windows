using System;

namespace CustomMacroBase.Helper.Extensions
{
    public static class InitializerExtensions
    {
        /// <summary>
        /// 为对象提供一个初始化和配置的方法
        /// </summary>
        public static T Init<T>(this T target, Action<T> configure) where T : class, new()
        {
            configure.Invoke(target); return target;
        }
    }
}
