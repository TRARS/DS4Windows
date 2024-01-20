using CustomMacroBase.Helper.Tools.TimeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Key = SharpDX.DirectInput.Key;

namespace CustomMacroBase.Helper.HotKey
{
    internal class HotKeyPacket()
    {
        public Key[] Keys;
        public Action Callback;
    }

    public partial class HotKeyManager
    {
        private static readonly Lazy<HotKeyManager> lazyObject = new(() => new HotKeyManager());
        public static HotKeyManager Instance => lazyObject.Value;
    }

    public partial class HotKeyManager
    {
        private CooldownTimer CT = new();
        private readonly Dictionary<string, HotKeyPacket> hotKeyCollection = new();

        public bool RegisterHotKey(string accesskey, Key[] keys, Action callback)
        {
            return hotKeyCollection.TryAdd(accesskey, new() { Keys = keys, Callback = callback });
        }
        public bool UnregisterHotKey(string accesskey)
        {
            return hotKeyCollection.Remove(accesskey);
        }
    }

    public partial class HotKeyManager
    {
        public async Task CheckHotKeys(Dictionary<Key, bool> keyState)
        {
            var hot = false;

            foreach (var item in hotKeyCollection.Values)
            {
                var keys = item.Keys;
                var callback = item.Callback;
                var allKeysPressed = keys.All(key => keyState.TryGetValue(key, out bool isPressed) && isPressed);

                if (allKeysPressed)
                {
                    callback.Invoke(); hot = true;
                }
            }

            if (hot) { await Task.Delay(256); }
        }
    }
}
