using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CustomMacroBase.Helper.Tools.FlowManager
{
    public abstract partial class FlowBase<T> : IEnumerable<T>
    {
        private protected abstract List<T> macro_actioninfo_list { get; }
        private protected abstract string macro_name { get; }

        public void Add(T _item)
        {
            macro_actioninfo_list.Add(_item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)macro_actioninfo_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)macro_actioninfo_list).GetEnumerator();
        }
    }

    public abstract partial class FlowBase<T>
    {
        private protected void Print([CallerMemberName] string str = "")
        {
            Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
        }
    }
}


