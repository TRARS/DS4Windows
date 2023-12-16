using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CustomMacroBase.Helper.Tools.TimeManager
{
    public partial class StopwatchTask
    {
        /// <summary>
        /// 内部Task
        /// </summary>
        public sealed class InternalTask
        {
            bool task_is_running = false;
            Stopwatch stopwatch = new Stopwatch();

            public void Run(Action<Stopwatch> action)
            {
                if (task_is_running is false)
                {
                    task_is_running = true;

                    ((Func<Task>)(async () =>
                    {
                        await Task.Run(() => { action.Invoke(stopwatch); }).ConfigureAwait(false);
                        task_is_running = false;
                    }))();
                }
            }
        }
    }

    public partial class StopwatchTask
    {

        /// <summary>
        /// 内部字典
        /// </summary>
        private readonly Dictionary<string, InternalTask> internalList = new();

        /// <summary>
        /// 通过key访问内部Task
        /// </summary>
        public InternalTask this[string key]
        {
            get
            {
                if (internalList.ContainsKey(key) is false)
                {
                    internalList.Add(key, new());
                }
                return internalList[key];
            }
        }
    }
}
