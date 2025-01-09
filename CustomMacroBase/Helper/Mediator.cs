﻿using System;
using System.Collections.Generic;

namespace CustomMacroBase.Helper
{
    //增加类型只能往后加不能往中间插入，不然可能会和CustomMacroPlugin.dll那边的对不上号（除非那边也引用最新的CustomMacroBase.dll）
    public enum MessageType
    {
        WindowClose = 0,          // （弃用）
        WindowKeyDown,            // （弃用）检测按键按下，以便用wasd或者↑←↓→控制拾色器区域里面的截图
        Ds4Disconnect,            // 调用DS4Windows方法（断开连接）
        Ds4Rumble,                // 调用DS4Windows方法（使手柄震动）
        Ds4Latency,               // 调用DS4Windows方法（获取输入延迟）
        PrintNewMessage,          // 打印新消息至日志区域
        PrintCleanup,             // 清空日志
        GetFrames,                // （弃用）更新截图
        PixelPickerOnOff,         // （弃用）拾色器区域 展开/关闭
        WindowPosReset,           // （弃用）窗体位置恢复至左上角
        CanUpdateFrames,          //
        WindowTopmost,            // （弃用）总在最前
    }

    //私有字段/属性/方法
    public sealed partial class Mediator
    {
        private Dictionary<string, List<Action<object?>>> internalListEx = new();
        private Dictionary<MessageType, List<Action<object?>>> internalList = new();
    }

    //限制为单例
    public sealed partial class Mediator
    {
        private static readonly Lazy<Mediator> lazyObject = new(() => new Mediator());
        public static Mediator Instance => lazyObject.Value;

        private Mediator() { }
    }

    //公开方法
    public sealed partial class Mediator
    {
        public void Register(MessageType type, Action<object> callback)
        {
            if (!internalList.ContainsKey(type))
            {
                internalList.Add(type, new List<Action<object?>>() { callback });
            }
            else
            {
                internalList[type].Add(callback);
            }
        }
        public void UnRegister(MessageType type)
        {
            if (internalList.ContainsKey(type))
            {
                internalList.Remove(type);
            }
        }
        public void NotifyColleagues(MessageType type, object? args)
        {
            if (internalList.ContainsKey(type))
            {
                foreach (Action<object?> item in internalList[type])
                {
                    item?.Invoke(args);
                }
            }
        }
    }
    public sealed partial class Mediator
    {
        public void Register(string type, Action<object> callback)
        {
            if (!internalListEx.ContainsKey(type.Trim()))
            {
                internalListEx.Add(type, new List<Action<object?>>() { callback });
            }
            else
            {
                internalListEx[type].Add(callback);
            }
        }
        public void UnRegister(string type)
        {
            if (internalListEx.ContainsKey(type))
            {
                internalListEx.Remove(type);
            }
        }
        public void NotifyColleagues(string type, object? args)
        {
            if (internalListEx.ContainsKey(type.Trim()))
            {
                foreach (Action<object?> item in internalListEx[type])
                {
                    item?.Invoke(args);
                }
            }
        }
    }
}
