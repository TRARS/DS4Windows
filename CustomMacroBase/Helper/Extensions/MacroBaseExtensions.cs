using CustomMacroBase.Helper.Attributes;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomMacroBase.Helper.Extensions
{
    public static class MacroBaseExtensions
    {
        private static int delay = 50;//轻微控制打印延时以便能按顺序打印

        public static bool IsUnused(this MacroBase target)
        {
            bool flag = false;
            string titleStr = string.Empty;
            string classStr = string.Empty;
            string releaseTime = string.Empty;

            //反射
            Type t = target.GetType();

            //反射读取特性
            foreach (Attribute item in Attribute.GetCustomAttributes(t))
            {
                if (item is DoNotLoadAttribute)
                {
                    flag = ((DoNotLoadAttribute)item).DoNotLoad; break;//该值指示是否被载入
                }
            }
            //反射读取属性
            foreach (PropertyInfo item in t.GetProperties())
            {
                if (item.Name is nameof(target.Title))
                {
                    titleStr = target!.GetType()!.GetProperty(nameof(target.Title))!.GetValue(target)!.ToString()!; break;//按钮上的文本
                }
            }
            classStr = t.Name;//类名

            Print($"{(flag ? "UnLoaded" : "Loaded")}： {titleStr} (.{classStr})", delay += 50);//打印输出到窗口（需叠加延迟以控制输出顺序

            return flag;
        }

        public static int GetIndex(this MacroBase target)
        {
            int index = 10000;

            //反射
            Type t = target.GetType();

            //反射读取特性
            foreach (Attribute item in Attribute.GetCustomAttributes(t))
            {
                if (item is SortIndexAttribute)
                {
                    index = ((SortIndexAttribute)item).SortIndex; break;//排序用
                }
            }

            return index;
        }

        private static void Print(string _str, int _delay)
        {
            Task.Run(() =>
            {
                Task.Delay(800 + _delay).Wait();
                Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, _str);
            });
        }
    }
}
