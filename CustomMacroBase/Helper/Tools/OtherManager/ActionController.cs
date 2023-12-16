using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomMacroBase.Helper.Tools.OtherManager
{
    public sealed partial class ActionController : IEnumerable<Action>
    {
        private readonly List<Action> action_delegate_list = new();

        /// <summary>
        /// _cooldown：动作的冷却时间
        /// </summary>
        public ActionController(int _cooldown = 1000)
        {
            action_cooldown_period = _cooldown;
        }

        public void Add(Action _a)
        {
            action_delegate_list.Add(_a);
        }

        public IEnumerator<Action> GetEnumerator()
        {
            return ((IEnumerable<Action>)action_delegate_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)action_delegate_list).GetEnumerator();
        }
    }

    public sealed partial class ActionController
    {
        /// <summary>
        /// 该值为true时触发动作
        /// </summary>
        public bool Start_Condition { get => action_start_condition; set => action_start_condition = value; }
    }

    public sealed partial class ActionController
    {
        readonly int action_cooldown_period = 0;

        bool action_cooldown = true;
        bool action_start_condition = false;

        public void ExecuteAction()
        {
            if (action_start_condition)
            {
                if (action_cooldown)
                {
                    action_cooldown = false;

                    ((Func<Task>)(async () =>
                    {
                        action_delegate_list.ForEach(_ => _.Invoke());
                        await Task.Delay(action_cooldown_period).ConfigureAwait(false);
                        action_cooldown = true;
                    }))();
                }
            }
        }
    }
}
