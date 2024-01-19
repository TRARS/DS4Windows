using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Key = SharpDX.DirectInput.Key;

namespace CustomMacroBase.Helper.HotKey
{
    public partial class RealKeyboard
    {
        private static readonly Lazy<RealKeyboard> lazyObject = new(() => new RealKeyboard());
        public static RealKeyboard Instance => lazyObject.Value;
    }

    public partial class RealKeyboard
    {
        Dictionary<Key, bool> mapping = new();

        private RealKeyboard()
        {
            foreach (var item in Enum.GetValues(typeof(Key)).Cast<Key>().ToList())
            {
                mapping.TryAdd(item, false);
            }
        }
    }

    public partial class RealKeyboard
    {
        // キーボード取得用変数
        private Keyboard keyboard;
        private KeyboardState currentState;
        private bool loopFlag;

        private void GetKeyboard()
        {
            // 入力周りの初期化
            DirectInput dinput = new DirectInput();

            if (dinput is not null)
            {
                // キーボード入力周りの初期化
                keyboard = new Keyboard(dinput);

                // バッファサイズを指定
                keyboard.Properties.BufferSize = 128;

                // キャプチャするデバイスを取得
                keyboard.Acquire();
            }
        }

        // キー入力処理
        private async Task UpdateForKey()
        {
            // 初期化が出来ていない場合、処理終了
            if (keyboard == null) { return; }

            //
            keyboard.Poll();
            currentState = keyboard.GetCurrentState();

            foreach (var key in mapping.Keys)
            {
                mapping[key] = currentState.IsPressed(key);
            }

            await HotKeyManager.Instance.CheckHotKeys(mapping);
        }

        // ループ(16ms)
        private void MainLoop()
        {
            if (loopFlag is false)
            {
                loopFlag = true;
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, $"start keyboard monitoring");

                            while (loopFlag)
                            {
                                await this.UpdateForKey();
                                await Task.Delay(16);
                            }
                        }
                        catch (Exception ex) { MessageBox.Show($"error: {ex.Message}"); }
                    });
                }
            }
        }
    }

    public partial class RealKeyboard
    {
        public void StartMonitoring()
        {
            this.GetKeyboard();
            this.MainLoop();
        }

        public void StopMonitoring()
        {
            if (keyboard is not null)
            {
                loopFlag = false;
                keyboard.Unacquire();
                Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, $"stop keyboard monitoring ");
            }
        }
    }
}
