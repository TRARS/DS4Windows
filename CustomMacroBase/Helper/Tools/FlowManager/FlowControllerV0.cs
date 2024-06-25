using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CustomMacroBase.Helper.Tools.FlowManager
{
    public struct ActionInfoV0
    {
        /// <summary>
        /// 待执行的动作
        /// </summary>
        public Action? Action { get; init; } = null;

        /// <summary>
        /// 动作持续时间（毫秒）
        /// </summary>
        public int Duration { get; init; } = 500;

        /// <summary>
        /// 动作持续时间（毫秒）（以委托的形式接收）
        /// </summary>
        public Func<int>? DurationFunc { get; init; } = null;

        public int GetDuration => (DurationFunc is null ? Duration : DurationFunc.Invoke());


        /// <summary>
        /// <para>参数_duration：空动作持续时间（毫秒）</para>
        /// </summary>
        public ActionInfoV0(int _duration)
        {
            Action = null;
            Duration = _duration;
        }

        /// <summary>
        /// <para>参数_durationfunc：空动作持续时间（毫秒）（以委托的形式接收）</para>
        /// </summary>
        public ActionInfoV0(Func<int> _durationfunc)
        {
            Action = null;
            DurationFunc = _durationfunc;
        }

        /// <summary>
        /// <para>动作打包</para>
        /// <para>参数_action：待执行的动作</para>
        /// <para>参数_duration：动作持续时间（毫秒）</para>
        /// </summary>
        public ActionInfoV0(Action? _action, int _duration)
        {
            Action = _action;
            Duration = _duration;
        }

        /// <summary>
        /// <para>动作打包</para>
        /// <para>参数_action：待执行的动作</para>
        /// <para>参数_durationfunc：动作持续时间（毫秒）（以委托的形式接收）</para>
        /// </summary>
        public ActionInfoV0(Action? _action, Func<int> _durationfunc)
        {
            Action = _action;
            DurationFunc = _durationfunc;
        }
    }

    /// <summary>
    /// <para>脚本执行时，不会影响来自真实手柄的其他操作</para>
    /// <para>适用场景：辅助搓招 or 挂机</para>
    /// </summary>
    public sealed partial class FlowControllerV0 : FlowBase<ActionInfoV0>
    {
        private protected override List<ActionInfoV0> macro_actioninfo_list { get; } = new();
        private protected override string macro_name { get; }

        /// <summary>
        /// <para>_macroName：脚本名</para>
        /// <para>_macro_act_pre：额外动作，用以在脚本中的每个动作被执行前优先弹起某些按键以避免冲突</para>
        /// </summary>
        public FlowControllerV0([CallerMemberName] string _macroName = "", Action? _macro_act_pre = null)
        {
            macro_name = _macroName;
            macro_act_pre = _macro_act_pre;
        }
    }

    public sealed partial class FlowControllerV0
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
        public bool Repeat_Condition { get => macro_repeat_condition; set { if (macro_repeat_condition != value) macro_repeat_condition = value; } }

        /// <summary>
        /// 于每次迭代前被执行的回调函数
        /// </summary>
        public Action? OnIterationStart { get; set; }
        /// <summary>
        /// 于每次迭代后被执行的回调函数
        /// </summary>
        public Action? OnIterationEnd { get; set; }
        /// <summary>
        /// 于循环结束时被执行的回调函数
        /// </summary>
        public Action? OnLoopEnd { get; set; }
    }

    public sealed partial class FlowControllerV0
    {
        bool macro_start_condition = false;
        bool macro_stop_condition = false;
        bool macro_repeat_condition = false;

        bool macro_task_locker = false;
        bool macro_task_is_running = false;
        bool macro_task_cancelflag = false;

        CancellationTokenSource? macro_cts = null;
        bool macro_cts_is_disposed = true;

        Action? macro_act = null;
        Action? macro_act_pre = null;

        public void ExecuteMacro()
        {
            if (macro_stop_condition)
            {
                if (macro_task_is_running is false) { macro_task_locker = false; return; }
                if (macro_task_cancelflag is false) { macro_task_cancelflag = true; }
                if (macro_cts_is_disposed is false) { macro_cts?.Cancel(); }
            };

            if (macro_start_condition)
            {
                if (macro_task_locker is false && macro_task_is_running is false)
                {
                    macro_task_locker = true;//上锁
                    macro_task_cancelflag = false;

                    ((Func<Task>)(async () =>
                    {
                        using (macro_cts = new())
                        {
                            macro_cts_is_disposed = false;
                            {
                                var current_token = macro_cts.Token;
                                var canceled = false;

                                Print($"{macro_name} Start");
                                {
                                    macro_task_is_running = true;//二次上锁
                                    {
                                        try
                                        {
                                            do
                                            {
                                                this.OnIterationStart?.Invoke();
                                                {
                                                    foreach (var item in macro_actioninfo_list)
                                                    {
                                                        if (macro_task_cancelflag) { break; }

                                                        macro_act = item.Action;

                                                        var duration = item.GetDuration;
                                                        var token = duration < 120 ? CancellationToken.None : current_token;
                                                        {
                                                            await Task.Delay(duration, token).ConfigureAwait(false);
                                                        }
                                                    }
                                                }
                                                this.OnIterationEnd?.Invoke();
                                            }
                                            while (macro_repeat_condition &&
                                                   macro_task_cancelflag is false);
                                        }
                                        catch
                                        {
                                            canceled = true;
                                        }
                                        this.OnLoopEnd?.Invoke();
                                    }
                                    macro_task_is_running = false;
                                }
                                Print($"{macro_name} End {(canceled ? "(cancel a long-running delay task)" : string.Empty)}");
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

            if (macro_task_is_running && macro_task_cancelflag is false)
            {
                macro_act_pre?.Invoke();
                macro_act?.Invoke();
            }
        }
    }
}
