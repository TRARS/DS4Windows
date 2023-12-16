using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CustomMacroBase.Helper.Tools.FlowManager
{
    /// <summary>
    /// <para>脚本执行时，不会影响来自真实手柄的其他操作</para>
    /// <para>适用场景：挂机 + 找图/找色/找数字（FindImage/FindColor/FindNumber）</para>
    /// </summary>
    public sealed partial class FlowControllerV2 : FlowBase<Action<Action[], bool[], Func<int, bool>>>
    {
        private protected override List<Action<Action[], bool[], Func<int, bool>>> macro_actioninfo_list { get; } = new();
        private protected override string macro_name { get; }

        /// <summary>
        /// <para>_macroName：脚本名</para>
        /// <para>_macro_act_pre：额外动作，用以在脚本中的每个动作被执行前优先弹起某些按键以避免冲突</para>
        /// </summary>
        public FlowControllerV2([CallerMemberName] string _macroName = "", Action? _macro_act_pre = null)
        {
            macro_name = _macroName;
            macro_act_pre = _macro_act_pre;
        }
    }

    public sealed partial class FlowControllerV2
    {
        /// <summary>
        /// 该值为true时触发脚本
        /// </summary>
        public bool Start_Condition { get => macro_start_condition; set { if (macro_start_condition != value) macro_start_condition = value; } }
        /// <summary>
        /// 该值为true时中断脚本
        /// </summary>
        public bool Stop_Condition { get => macro_stop_condition; set { if (macro_stop_condition != value) macro_stop_condition = value; } }
        /// <summary>
        /// 该值为true时令脚本可以循环
        /// </summary>
        //public bool Repeat_Condition { get => macro_repeat_condition; set { if (macro_repeat_condition != value) macro_repeat_condition = value; } }
    }

    public sealed partial class FlowControllerV2
    {
        bool macro_start_condition = false;
        bool macro_stop_condition = false;
        //bool macro_repeat_condition = false;

        bool macro_task_locker = false;
        bool macro_task_is_running = false;
        bool[] macro_task_cancelflag = new bool[1] { false };

        CancellationTokenSource? macro_cts = null;
        bool macro_cts_is_disposed = true;
        CancellationToken macro_token = default;
        bool macro_canceled = false;//flag: CancelFromLongDelayTime

        Action[] macro_act = new Action[1];
        Action? macro_act_pre = null;

        public void ExecuteMacro()
        {
            if (macro_stop_condition)
            {
                if (macro_task_is_running is false) { macro_task_locker = false; return; }
                if (macro_task_cancelflag[0] is false) { macro_task_cancelflag[0] = true; }
                if (macro_cts_is_disposed is false) { macro_cts?.Cancel(); }
            };

            if (macro_start_condition)
            {
                if (macro_task_locker is false && macro_task_is_running is false)
                {
                    macro_task_locker = true;//上锁
                    macro_task_cancelflag[0] = false;

                    ((Func<Task>)(async () =>
                    {
                        using (macro_cts = new())
                        {
                            macro_cts_is_disposed = false;
                            {
                                macro_token = macro_cts.Token;
                                macro_canceled = false;

                                Print($"{macro_name} Start");
                                {
                                    macro_task_is_running = true;//二次上锁
                                    {
                                        if (macro_actioninfo_list is not null)
                                        {
                                            await Task.Run(() =>
                                            {
                                                macro_actioninfo_list.FirstOrDefault(_ => _ is not null)?
                                                                     .Invoke(macro_act, macro_task_cancelflag, InnerWait);
                                            }).ConfigureAwait(false);
                                        }
                                    }
                                    macro_task_is_running = false;
                                }
                                Print($"{macro_name} End {(macro_canceled ? "(cancel a long-running delay task)" : string.Empty)}");
                            }
                            macro_cts_is_disposed = true;
                        }
                    }))().ConfigureAwait(false);
                }
            }
            else
            {
                macro_task_locker = false;//锁1复位
            }

            if (macro_task_is_running && macro_task_cancelflag[0] is false)
            {
                macro_act_pre?.Invoke();
                macro_act[0]?.Invoke();
            }
        }

        /// <summary>
        /// 从较长的延时任务中取消时，返回true，否则返回false
        /// </summary>
        private bool InnerWait(int duration)
        {
            if (duration < 120)
            {
                Task.Delay(duration).Wait();
            }
            else
            {
                try
                {
                    Task.Delay(duration, macro_token).Wait();
                }
                catch
                {
                    macro_canceled = true; return true;
                }
            }

            return false;
        }
    }
}
