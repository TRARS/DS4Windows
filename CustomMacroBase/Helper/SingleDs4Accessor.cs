﻿using System;
using System.Linq;
using System.Threading;

namespace CustomMacroBase.Helper
{
    public sealed partial class SingleDs4Accessor
    {
        private static readonly Lazy<SingleDs4Accessor> lazyObject = new(() => new SingleDs4Accessor());
        public static SingleDs4Accessor Instance => lazyObject.Value;
    }

    public sealed partial class SingleDs4Accessor
    {
        private int slot = 1;
        public bool AllowOnce(string[] hardwareIds)
        {
            var flag = (hardwareIds.Any(id => id.ToUpper().Contains(@"HID\VID_054C&PID_05C4"))) && (Interlocked.Exchange(ref slot, 1) == 0);
            if (slot != 0) { OnSlotConsumed?.Invoke(); }
            return flag;
        }
        public void Reset(int value = 0) => Interlocked.Exchange(ref slot, value);
    }

    public sealed partial class SingleDs4Accessor
    {
        public event Action OnSlotConsumed;
    }
}
