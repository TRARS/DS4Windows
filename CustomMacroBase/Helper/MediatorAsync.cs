using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CustomMacroBase.Helper
{
    /// <summary>
    /// 适用于 MediatorAsync 的消息枚举
    /// </summary>
    public enum AsyncMessageType
    {
        AsyncSnapshot,
    }

    public sealed partial class MediatorAsync
    {
        private ConcurrentDictionary<AsyncMessageType, Func<object?, CancellationToken, Task<object?>>> internalDictionary = new();
    }

    public sealed partial class MediatorAsync
    {
        private static readonly Lazy<MediatorAsync> lazyObject = new(() => new MediatorAsync());
        public static MediatorAsync Instance => lazyObject.Value;

        private MediatorAsync() { }
    }

    public sealed partial class MediatorAsync
    {
        /// <summary>
        /// 注册异步方法供日后使用（每种消息只能注册一次）
        /// </summary>
        public void Register(AsyncMessageType type, Func<object?, CancellationToken, Task<object?>> callback)
        {
            if (internalDictionary.TryAdd(type, callback) is false)
            {
                Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, $"duplicate registration messages: {type}");
            }
        }

        /// <summary>
        /// 获取异步方法提供的返回值
        /// </summary>
        public async Task<object?> NotifyColleagues(AsyncMessageType type, object? args, CancellationToken? token = null)
        {
            if (internalDictionary.TryGetValue(type, out var func))
            {
                return await func.Invoke(args, token ?? CancellationToken.None);
            }

            return null;
        }
    }
}
