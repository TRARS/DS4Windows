using System;
using System.Collections.Generic;

namespace CustomMacroBase.Helper.Tools.TimeManager
{
    public partial class CooldownTimer
    {
        //状态
        private abstract class TimerState
        {
            public abstract Action? NextState { get; init; }
            public abstract void Start(int threshold, out bool flag);
            public abstract void Stop(int threshold, out bool flag);
            public abstract TimerState Reset();
            public static void Print(string str) => Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
        }
        private class Started : TimerState
        {
            public override Action? NextState { get; init; }
            public override void Start(int threshold, out bool flag)
            {
                flag = true;
            }
            public override void Stop(int threshold, out bool flag)
            {
                flag = false; NextState?.Invoke();
            }
            public override TimerState Reset()
            {
                return this;
            }
        }
        private class Stopped : TimerState
        {
            DateTime lastDateTime = DateTime.Now;
            bool threshold_reached = false;

            public override Action? NextState { get; init; }
            public override void Start(int threshold, out bool flag)
            {
                if (threshold_reached || threshold == 0)
                {
                    flag = true;
                    NextState?.Invoke();
                    return;
                }

                flag = threshold_reached = (DateTime.Now).Subtract(lastDateTime).TotalMilliseconds > threshold;
            }
            public override void Stop(int threshold, out bool flag)
            {
                if (threshold == 0)
                {
                    flag = true; return;
                }
                flag = false;

                lastDateTime = DateTime.Now;
            }
            public override TimerState Reset()
            {
                threshold_reached = false;
                lastDateTime = DateTime.Now;
                return this;
            }
        }

        //状态机
        private sealed class TimerStateMachine
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime previousDateTime = DateTime.Now;

            private TimerState? currentState = null;
            private readonly TimerState? startedState = null;
            private readonly TimerState? stoppedState = null;

            public TimerStateMachine()
            {
                startedState = new Started() { NextState = () => { this.currentState = stoppedState?.Reset(); } };
                stoppedState = new Stopped() { NextState = () => { this.currentState = startedState?.Reset(); } };

                currentState = stoppedState.Reset();
            }

            public void Update(int threshold, out bool result)
            {
                result = false;

                currentDateTime = DateTime.Now;
                {
                    switch (currentDateTime.Subtract(previousDateTime).TotalMilliseconds > 50)
                    {
                        case true:
                            currentState?.Stop(threshold, out result); break;
                        case false:
                            currentState?.Start(threshold, out result); break;
                    }
                }
                previousDateTime = currentDateTime;
            }
        }
    }



    public partial class CooldownTimer
    {
        /// <summary>
        /// 内部计时器类
        /// </summary>
        public sealed class InnerTimer
        {
            TimerStateMachine machine = new();

            /// <summary>
            /// <para>_threshold：<see cref="int"/>类型，超时阈值（比如填'100'，则当持续访问该方法时，于100毫秒内返回false，于100毫秒后返回true）</para>
            /// </summary>
            public bool Elapsed(int _threshold)
            {
                machine.Update(_threshold, out bool flag);
                return flag;
            }
        }
    }

    public partial class CooldownTimer
    {
        /// <summary>
        /// 内部字典
        /// </summary>
        private readonly Dictionary<string, InnerTimer> internalList = new();

        /// <summary>
        /// 通过key访问内部计时器
        /// </summary>
        public InnerTimer this[string key]
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
