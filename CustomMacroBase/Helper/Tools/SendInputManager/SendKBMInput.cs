using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using KeyboardKeys = System.Windows.Forms.Keys;

namespace CustomMacroBase.Helper.Tools.SendInputManager
{
    //引用dll部分
    public partial class SendKBMInput
    {
        #region 引用/声明
        ///// APIの利用に必要な構造体・共用体の定義　ここから /////
        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public int X;
            public int Y;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        };

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT_UNION
        {
            [FieldOffset(0)] public MOUSEINPUT mouse;
            [FieldOffset(0)] public KEYBDINPUT keyboard;
            [FieldOffset(0)] public HARDWAREINPUT hardware;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public int type;
            public INPUT_UNION ui;
        };
        ///// APIの利用に必要な構造体・共用体の定義　ここまで /////


        // 関数
        [DllImport("user32.dll")]
        private extern static void SendInput(int nInputs, ref INPUT pInputs, int cbsize);
        [DllImport("user32.dll")]
        private extern static void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //マウスとキーボードをロックできる関数
        [DllImport("user32.dll")]
        private static extern void BlockInput(bool Block);
        //
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);


        // 定数の定義
        private const int INPUT_MOUSE = 0;
        private const int INPUT_KEYBOARD = 1;
        private const int INPUT_HARDWARE = 2;

        private const int MOUSEEVENTF_MOVE = 0x1;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int MOUSEEVENTF_WHEEL = 0x800;
        private const int WHEEL_DELTA = 120;

        private const int KEYEVENTF_KEYDOWN = 0x0;
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        #endregion

        //要操作鼠标哪个键
        public enum MouseKeys
        {
            Left = 0,
            Right,
            Middle,
        }

        /// <summary>
        /// SendInput简化
        /// </summary>
        private static void PreSendInput(ref INPUT[] _inputs) => SendInput(_inputs.Length, ref _inputs[0], Marshal.SizeOf(_inputs[0]));

        //
        private SendKBMInput() { }
    }




    //计时器
    public partial class SendKBMInput
    {
        private sealed class InnerTimer
        {
            bool stop_condition = false;

            bool task_locker = false;
            bool flag = false;

            Func<DateTime, DateTime, int, bool> timeout = (a, b, c) => ((b).Subtract(a).TotalMilliseconds >= c);

            public void Reset()
            {
                if (stop_condition is false) { stop_condition = true; }
                if (task_locker) { task_locker = false; }
                if (flag) { flag = false; }
            }

            /// <summary>
            /// 长按时间小于_threshold返回false，否则返回true
            /// </summary>
            public bool CoolDown(int _threshold)
            {
                if (task_locker is false && flag is false)
                {
                    stop_condition = false;

                    var starttime = DateTime.Now; //计时开始

                    Task.Run(async () =>
                    {
                        task_locker = true;
                        {
                            await Task.Yield();

                            while (timeout.Invoke(starttime, DateTime.Now, _threshold) is false && stop_condition is false)
                            {
                                await Task.Delay(32);// 延迟设置较低，不需要传入token来取消
                            }

                            flag = (stop_condition is false); //true
                        }
                        task_locker = false;
                    }).ConfigureAwait(false);
                }

                return flag;
            }
        }
    }

    //键鼠通用按键状态机
    public partial class SendKBMInput
    {
        //状态
        private abstract class KeyState<T>
        {
            public abstract Action? NextState { get; init; }
            public abstract void Press(T[] keys);
            public abstract void Release(T[] keys);
            public static void Print(string str) => Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
        }
        private class KeyPressed<T> : KeyState<T>
        {
            public Action<T>? SendKeyUp { get; init; }
            public override Action? NextState { get; init; }
            public override void Press(T[] keys) { }
            public override void Release(T[] keys)
            {
                //弹起
                foreach (var key in keys) { SendKeyUp?.Invoke(key); }

                //打印
                Print($"Release : {string.Join(" + ", keys)}");

                //状态转换为已弹起
                NextState?.Invoke();
            }
        }
        private class KeyReleased<T> : KeyState<T>
        {
            public Action<T>? SendKeyDown { get; init; }
            public override Action? NextState { get; init; }
            public override void Press(T[] keys)
            {
                //按下
                foreach (var key in keys) { SendKeyDown?.Invoke(key); }

                //打印
                Print($"Press : {string.Join(" + ", keys)}");

                //状态转换为已按下
                NextState?.Invoke();
            }
            public override void Release(T[] keys) { }
        }

        //状态机
        private sealed class KeyStateMachine<T>
        {
            private readonly InnerTimer innerTimer = new();//计时器安排个

            private bool task_locker = false;
            private CancellationTokenSource? task_cts = null;
            private bool task_cts_is_disposed = false;

            private bool auto_cycle_flag = false;
            private int auto_cycle_duration = 0;
            private int auto_cycle_activation_duration = 0;

            private T[] currentKeys = Array.Empty<T>();
            private KeyState<T>? currentState = null;
            private readonly KeyState<T>? pressedState = null;
            private readonly KeyState<T>? releasedState = null;

            //构造
            public KeyStateMachine(T[] keys, Action<T>? keyup, Action<T>? keydown)
            {
                pressedState = new KeyPressed<T>() { NextState = () => { this.currentState = releasedState; }, SendKeyUp = keyup };
                releasedState = new KeyReleased<T>() { NextState = () => { this.currentState = pressedState; }, SendKeyDown = keydown };

                currentKeys = keys;
                currentState = releasedState;//默认状态已弹起
            }
            public void SetCycleOptions(int cycleActivationDuration, int cycleDuration)
            {
                if (auto_cycle_activation_duration != cycleActivationDuration) { auto_cycle_activation_duration = cycleActivationDuration; }
                if (auto_cycle_duration != cycleDuration) { auto_cycle_duration = cycleDuration; }
            }
            public void Update(bool press, bool allow_auto_cycle)
            {
                //长按后激活循环
                if (allow_auto_cycle)
                {
                    if (press && innerTimer.CoolDown(auto_cycle_activation_duration))
                    {
                        auto_cycle_flag = true; PressReleaseCycle(); return;
                    }
                    else if (press is false)
                    {
                        auto_cycle_flag = false; PressReleaseCycle(); innerTimer.Reset();
                    }
                }

                //常规按下弹起
                switch (press)
                {
                    case true:
                        currentState?.Press(currentKeys); break;
                    case false:
                        currentState?.Release(currentKeys); break;
                }
            }

            //循环
            private void PressReleaseCycle()
            {
                if (auto_cycle_flag is false)
                {
                    if (task_cts_is_disposed is false) { task_cts?.Cancel(); }
                    task_locker = false;
                    return;
                }

                if (task_locker is false)
                {
                    task_locker = true;

                    Task.Run(async () =>
                    {
                        await Task.Yield();

                        using (task_cts = new())
                        {
                            task_cts_is_disposed = false;
                            {
                                var token = task_cts.Token;
                                try
                                {
                                    do
                                    {
                                        currentState?.Release(currentKeys);
                                        await Task.Delay(auto_cycle_duration, token);
                                        currentState?.Press(currentKeys);
                                        await Task.Delay(auto_cycle_duration, token);
                                    }
                                    while (auto_cycle_flag && token.IsCancellationRequested is false);
                                }
                                catch { }
                                finally { currentState?.Release(currentKeys); }
                            }
                            task_cts_is_disposed = true;
                        }
                    }).ContinueWith(_ => { task_locker = false; }).ConfigureAwait(false);
                }
            }
        }
    }

    //键鼠代理（仅内部调用）
    public partial class SendKBMInput
    {
        //键鼠代理抽象
        private abstract class KBMProxy<T> where T : Enum
        {
            public readonly Dictionary<string, KeyStateMachine<T>> CombineStateMachineList = new();

            protected KBMProxy() { }

            //键鼠通用
            public abstract Action<T>? SendKeyDown { get; }
            public abstract Action<T>? SendKeyUp { get; }
            public abstract void Update(string specialKey, T[] keys, bool press, bool autorelease = false, int cycleActivationDuration = 0, int cycleDuration = 0);

            //鼠标专用
            public virtual Action<Point>? SendMouseMove { get; }
            public virtual void Update(Point pt) { }
        }

        //鼠标代理实现
        private class MouseProxy : KBMProxy<MouseKeys>
        {
            private static readonly Lazy<MouseProxy> lazyObject = new(() => new());
            public static MouseProxy Instance => lazyObject.Value;

            private MouseProxy() { }

            public override Action<Point> SendMouseMove => (pt) =>
            {
                Win32Point mousePosition = new()
                {
                    X = (int)(pt.X * ((double)65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width)),
                    Y = (int)(pt.Y * ((double)65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height))
                };

                //Move
                INPUT[] inputs_move = new INPUT[] {
                    new INPUT {
                        type = INPUT_MOUSE,
                        ui = new INPUT_UNION {
                            mouse = new MOUSEINPUT {
                                dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE,
                                dx = mousePosition.X,
                                dy = mousePosition.Y,
                                mouseData = 0,
                                dwExtraInfo = IntPtr.Zero,
                                time = 0
                            }
                        }
                    }
                };

                PreSendInput(ref inputs_move);
            };
            public override Action<MouseKeys> SendKeyDown => (key) =>
            {
                //Down
                INPUT[] inputs_click = new INPUT[]
                {
                    new INPUT {
                        type = INPUT_MOUSE,
                        ui = new INPUT_UNION {
                            mouse = new MOUSEINPUT {
                                dwFlags = PressOrRelease.Invoke(key, true),//MOUSEEVENTF_LEFTDOWN
                                dx = 0,
                                dy = 0,
                                mouseData = 0,
                                dwExtraInfo = IntPtr.Zero,
                                time = 0
                            }
                        }
                    }
                };

                PreSendInput(ref inputs_click);
            };
            public override Action<MouseKeys> SendKeyUp => (key) =>
            {
                //Up
                INPUT[] inputs_click = new INPUT[]
                {
                    new INPUT {
                        type = INPUT_MOUSE,
                        ui = new INPUT_UNION {
                            mouse = new MOUSEINPUT {
                                dwFlags = PressOrRelease.Invoke(key, false),//MOUSEEVENTF_LEFTUP
                                dx = 0,
                                dy = 0,
                                mouseData = 0,
                                dwExtraInfo = IntPtr.Zero,
                                time = 0
                            }
                        }
                    }
                };

                PreSendInput(ref inputs_click);
            };
            private Func<MouseKeys, bool, int> PressOrRelease = (key, flag) =>
            {
                return key switch
                {
                    MouseKeys.Left => flag ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP,
                    MouseKeys.Right => flag ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP,
                    MouseKeys.Middle => flag ? MOUSEEVENTF_MIDDLEDOWN : MOUSEEVENTF_MIDDLEUP,
                    _ => 0
                };
            };

            public override void Update(Point pt)
            {
                SendMouseMove.Invoke(pt);
            }
            public override void Update(string specialKey, MouseKeys[] keys, bool mouseFlag, bool autoCycle = false, int cycleActivationDuration = 0, int cycleDuration = 0)
            {
                var key = $"{specialKey}_{string.Join("_", keys)}";
                base.CombineStateMachineList.TryAdd(key, new(keys, SendKeyUp, SendKeyDown));
                var target = base.CombineStateMachineList[key];
                target?.SetCycleOptions(cycleActivationDuration, cycleDuration);
                target?.Update(mouseFlag, autoCycle);
            }
        }

        //键盘代理实现
        private class KeyboardProxy : KBMProxy<KeyboardKeys>
        {
            private static readonly Lazy<KeyboardProxy> lazyObject = new(() => new KeyboardProxy());
            public static KeyboardProxy Instance => lazyObject.Value;

            private KeyboardProxy() { }

            public override Action<KeyboardKeys> SendKeyDown => (key) =>
            {
                INPUT[] inputs_key = new INPUT[]
                {
                    //KeyDown
                    new INPUT {
                        type = INPUT_KEYBOARD,
                        ui = new INPUT_UNION
                        {
                            keyboard = new KEYBDINPUT
                            {
                                wVk = (short)key,
                                wScan = (short)MapVirtualKey((uint)key, 0),
                                dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN,
                                dwExtraInfo = IntPtr.Zero,
                                time = 0
                            }
                        }
                    },
                };
                PreSendInput(ref inputs_key);
            };
            public override Action<KeyboardKeys> SendKeyUp => (key) =>
            {
                INPUT[] inputs_key = new INPUT[]
                {
                    //KeyUp
                    new INPUT {
                        type = INPUT_KEYBOARD,
                        ui = new INPUT_UNION
                        {
                            keyboard = new KEYBDINPUT
                            {
                                wVk = (short)key,
                                wScan = (short)MapVirtualKey((uint)key, 0),
                                dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
                                dwExtraInfo = IntPtr.Zero,
                                time = 0
                            }
                        }
                    }
                };
                PreSendInput(ref inputs_key);
            };

            public override void Update(string specialKey, KeyboardKeys[] keys, bool keyboardFlag, bool autoCycle = false, int cycleActivationDuration = 0, int cycleDuration = 0)
            {
                var key = $"{specialKey}_{string.Join("_", keys)}";
                base.CombineStateMachineList.TryAdd(key, new(keys, SendKeyUp, SendKeyDown));
                var target = base.CombineStateMachineList[key];
                target?.SetCycleOptions(cycleActivationDuration, cycleDuration);
                target?.Update(keyboardFlag, autoCycle);
            }
        }
    }




    //单击鼠标或键盘（供外部调用）
    public partial class SendKBMInput
    {
        private static bool mouse_task_is_running = false;
        private static bool keyboard_task_is_running = false;

        /// <summary>
        /// <para>鼠标左键单击</para>
        /// <para>参数 pt：绝对坐标</para>
        /// </summary>
        public static void SendMouseLeftClick(Point pt)
        {
            if (mouse_task_is_running is false)
            {
                mouse_task_is_running = true;
                {
                    Win32Point mousePosition = new()
                    {
                        X = (int)(pt.X * ((double)65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width)),
                        Y = (int)(pt.Y * ((double)65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height))
                    };

                    //Move
                    INPUT[] inputs_move = new INPUT[] {
                        new INPUT {
                            type = INPUT_MOUSE,
                            ui = new INPUT_UNION {
                                mouse = new MOUSEINPUT {
                                    dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE,
                                    dx = mousePosition.X,
                                    dy = mousePosition.Y,
                                    mouseData = 0,
                                    dwExtraInfo = IntPtr.Zero,
                                    time = 0
                                }
                            }
                        }
                    };

                    //Click
                    INPUT[] inputs_click = new INPUT[]
                    {
                        new INPUT {
                            type = INPUT_MOUSE,
                            ui = new INPUT_UNION {
                                mouse = new MOUSEINPUT {
                                    dwFlags = MOUSEEVENTF_LEFTDOWN,
                                    dx = mousePosition.X,
                                    dy = mousePosition.Y,
                                    mouseData = 0,
                                    dwExtraInfo = IntPtr.Zero,
                                    time = 0
                                }
                            }
                        },
                        new INPUT {
                            type = INPUT_MOUSE,
                            ui = new INPUT_UNION {
                                mouse = new MOUSEINPUT {
                                    dwFlags = MOUSEEVENTF_LEFTUP,
                                    dx = mousePosition.X,
                                    dy = mousePosition.Y,
                                    mouseData = 0,
                                    dwExtraInfo = IntPtr.Zero,
                                    time = 0
                                }
                            }
                        }
                    };

                    PreSendInput(ref inputs_move);
                    PreSendInput(ref inputs_click);
                }
                mouse_task_is_running = false;
            }
        }

        /// <summary>
        /// <para>键盘某键按下&amp;弹起</para>
        /// <para>参数 key：按哪个键（比如填'<see cref="System.Windows.Forms.Keys.Enter"/>'）</para>
        /// </summary>
        public static void SendKeyBoardClick(KeyboardKeys key)
        {
            if (keyboard_task_is_running is false)
            {
                keyboard_task_is_running = true;
                {
                    INPUT[] inputs_key = new INPUT[] {
                        //KeyDown
                        new INPUT {
                            type = INPUT_KEYBOARD,
                            ui = new INPUT_UNION
                            {
                                keyboard = new KEYBDINPUT
                                {
                                    wVk = (short)key,
                                    wScan = (short)MapVirtualKey((uint)key, 0),
                                    dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN,
                                    dwExtraInfo = IntPtr.Zero,
                                    time = 0
                                }
                            }
                        },
                        //KeyUp
                        new INPUT {
                            type = INPUT_KEYBOARD,
                            ui = new INPUT_UNION
                            {
                                keyboard = new KEYBDINPUT
                                {
                                    wVk = (short)key,
                                    wScan = (short)MapVirtualKey((uint)key, 0),
                                    dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
                                    dwExtraInfo = IntPtr.Zero,
                                    time = 0
                                }
                            }
                        }
                    };

                    PreSendInput(ref inputs_key);
                }
                keyboard_task_is_running = false;
            }
        }

        /// <summary>
        /// <para>禁用键鼠输入，仅在管理员模式下有效</para>
        /// <para>※ 无法禁用数位板/数位屏输入</para>
        /// </summary>
        /// <param name="flag"></param>
        public static void SendBlockInput(bool flag)
        {
            BlockInput(flag);
        }
    }

    //长按鼠标或键盘（供外部调用）
    public partial class SendKBMInput
    {
        /// <summary>
        /// <para>鼠标移动</para>
        /// <para>参数 point：绝对坐标</para>
        /// </summary>
        public static void MouseMove(Point point)
        {
            MouseProxy.Instance.Update(point);
        }


        /// <summary>
        /// <para>鼠标某键按下</para>
        /// <para>参数 specialKey：用作访问字典的唯一key</para>
        /// <para>参数 flag：true按下 false弹起</para>
        /// <para>参数 keys：一次性按多个键，MouseKey数组</para>
        /// <para>禁用自动连发</para>
        /// </summary>
        public static void MouseDown(string specialKey, bool flag, params MouseKeys[] keys)
        {
            MouseProxy.Instance.Update(specialKey, keys, flag);
        }
        /// <summary>
        /// <para>鼠标某键按下</para>
        /// <para>参数 specialKey：用作访问字典的唯一key</para>
        /// <para>参数 flag：true按下 false弹起</para>
        /// <para>参数 keys：一次性按多个键，MouseKey数组</para>
        /// <para>参数 autoCycle：是否启用自动连发</para>
        /// <para>参数 cycleActivationDuration：启用自动连发时，长按直至激活自动连发所需耗时（毫秒）</para>
        /// <para>参数 cycleDuration：启用自动连发时，按下与弹起之间的延时（毫秒）</para>
        /// </summary>
        public static void MouseDownEx(string specialKey, bool flag, MouseKeys[] keys, bool autoCycle, int cycleActivationDuration = 0, int cycleDuration = 0)
        {
            MouseProxy.Instance.Update(specialKey, keys, flag, autoCycle, cycleActivationDuration, cycleDuration);
        }



        /// <summary>
        /// <para>键盘某键按下</para>
        /// <para>参数 specialKey：用作访问字典的唯一key</para>
        /// <para>参数 flag：true按下 false弹起</para>
        /// <para>参数 keys：一次性按多个键，Key数组（比如填'<see cref="System.Windows.Forms.Keys.Enter"/>'）</para>
        /// <para>禁用自动连发</para>
        /// </summary>
        public static void KeyDown(string specialKey, bool flag, params KeyboardKeys[] keys)
        {
            KeyboardProxy.Instance.Update(specialKey, keys, flag, false);
        }
        /// <summary>
        /// <para>键盘某键按下</para>
        /// <para>参数 specialKey：用作访问字典的唯一key</para>
        /// <para>参数 flag：true按下 false弹起</para>
        /// <para>参数 keys：一次性按多个键，Key数组（比如填'<see cref="System.Windows.Forms.Keys.Enter"/>'）</para>
        /// <para>参数 autoCycle：是否启用自动连发</para>
        /// <para>参数 cycleActivationDuration：启用自动连发时，长按直至激活自动连发所需耗时（毫秒）</para>
        /// <para>参数 cycleDuration：启用自动连发时，按下与弹起之间的延时（毫秒）</para>
        /// </summary>
        public static void KeyDownEx(string specialKey, bool flag, KeyboardKeys[] keys, bool autoCycle, int cycleActivationDuration = 0, int cycleDuration = 0)
        {
            KeyboardProxy.Instance.Update(specialKey, keys, flag, autoCycle, cycleActivationDuration, cycleDuration);
        }
    }
}
