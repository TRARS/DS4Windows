using CustomMacroBase.GamePadState;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CustomMacroBase.Helper.Tools.FlowManager
{
    public struct ActionInfoV1
    {
        /// <summary>
        /// 按键名
        /// </summary>
        public string? Key { get; init; } = null;

        /// <summary>
        /// 按键值
        /// </summary>
        public dynamic? Value { get; init; } = default;

        /// <summary>
        /// 按键持续时间
        /// </summary>
        public int Duration { get; init; } = 500;

        /// <summary>
        /// 按键持续时间（委托版）
        /// </summary>
        public Func<int>? DurationFunc { get; init; } = null;

        public int GetDuration => (DurationFunc is null ? Duration : DurationFunc.Invoke());
        public bool NoAction => (Key is null || Value is null);


        /// <summary>
        /// <para>参数_duration：状态持续时间（毫秒）</para>
        /// </summary>
        public ActionInfoV1(int _duration)
        {
            Key = Value = null; Duration = _duration;
        }

        /// <summary>
        /// <para>动作打包</para>
        /// <para>参数_key：按键名</para>
        /// <para>参数_value：按键状态</para>
        /// <para>参数_duration：按键状态持续时间（毫秒）</para>
        /// </summary>
        public ActionInfoV1(string _key, byte _value, int _duration = 0)
        {
            Key = _key; Value = _value; Duration = _duration;
        }

        /// <summary>
        /// <para>动作打包</para>
        /// <para>参数_key：按键名</para>
        /// <para>参数_value：按键状态</para>
        /// <para>参数_duration：按键状态持续时间（毫秒）</para>
        /// </summary>
        public ActionInfoV1(string _key, bool _value, int _duration = 0)
        {
            Key = _key; Value = _value; Duration = _duration;
        }
    }

    /// <summary>
    /// <para>脚本执行时，将完全无视任何来自真实手柄的操作</para>
    /// <para>适用场景：挂机</para>
    /// </summary>
    public sealed partial class FlowControllerV1 : FlowBase<ActionInfoV1>
    {
        private protected override List<ActionInfoV1> macro_actioninfo_list { get; } = new();
        private protected override string macro_name { get; }

        /// <summary>
        /// <para>_macroName：脚本名</para>
        /// </summary>
        public FlowControllerV1([CallerMemberName] string _macroName = "")
        {
            macro_name = _macroName;
        }
    }

    public sealed partial class FlowControllerV1
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
    }

    public sealed partial class FlowControllerV1
    {
        bool macro_start_condition = false;
        bool macro_stop_condition = false;
        bool macro_repeat_condition = false;

        bool macro_task_locker = false;
        bool macro_task_is_running = false;
        bool macro_task_cancelflag = false;

        CancellationTokenSource? macro_cts = null;
        bool macro_cts_is_disposed = true;

        DS4StateLite? temp = null;

        public void ExecuteMacro(DS4StateLite _virtual)
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
                        await foreach (var item in ComboFlow())
                        {
                            //Print($"action_idx: {item}");
                        };
                    }))().ConfigureAwait(false);
                }
            }
            else
            {
                macro_task_locker = false;
            }

            if (macro_task_is_running && macro_task_cancelflag is false) { temp?.CopyTo(ref _virtual); }
        }

        private async IAsyncEnumerable<int?> ComboFlow()
        {
            using (macro_cts = new())
            {
                macro_cts_is_disposed = false;
                {
                    var current_token = macro_cts.Token;
                    var canceled = false;
                    //var count = 0;

                    Print($"{macro_name} Start");
                    {
                        macro_task_is_running = true;//二次上锁
                        {
                            try
                            {
                                do
                                {
                                    //count = 0;
                                    temp = new() { LX = 128, LY = 128, RX = 128, RY = 128 };
                                    foreach (var item in macro_actioninfo_list)
                                    {
                                        if (macro_task_cancelflag) { break; }

                                        var duration = item.GetDuration;
                                        var token = duration < 120 ? CancellationToken.None : current_token;
                                        {
                                            if (item.NoAction is false) { UpdateNow(item.Key, item.Value); }
                                            await Task.Delay(duration, token).ConfigureAwait(false);
                                            //yield return count++;
                                        }
                                    }
                                }
                                while (macro_repeat_condition &&
                                       macro_task_cancelflag is false);
                            }
                            catch
                            {
                                canceled = true;
                            }
                            yield return null;
                        }
                        macro_task_is_running = false;
                    }
                    Print($"{macro_name} End {(canceled ? "(cancel a long-running delay task)" : string.Empty)}");
                }
                macro_cts_is_disposed = true;
            }
        }

        private void UpdateNow<T>(string propName, T propValue)
        {
            temp?.GetType()?.GetField(propName)?.SetValue(temp, propValue);
        }
    }
}
