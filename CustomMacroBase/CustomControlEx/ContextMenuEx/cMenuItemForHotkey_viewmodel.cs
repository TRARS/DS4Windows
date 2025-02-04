using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CustomMacroBase.Helper.HotKey;
using CustomMacroBase.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Key = SharpDX.DirectInput.Key;

namespace CustomMacroBase.CustomControlEx.ContextMenuEx
{
    partial class cMenuItemForHotkey_viewmodel
    {
        public class ItemViewModel : ObservableObject
        {
            private string _text;
            private string _accesskey;
            private Action _callback;

            //第一个Key
            private List<Key> _KeyEnumList0 = new();
            public List<Key> KeyEnumList0
            {
                get { return _KeyEnumList0; }
                set { _KeyEnumList0 = value; OnPropertyChanged(); }
            }
            private int _SelectedKey0 = 0;
            public int SelectedKey0
            {
                get { return _SelectedKey0; }
                set { _SelectedKey0 = value; OnPropertyChanged(); OnSelectedChanged(); }
            }

            //第二个Key
            private List<Key> _KeyEnumList1 = new();
            public List<Key> KeyEnumList1
            {
                get { return _KeyEnumList1; }
                set { _KeyEnumList1 = value; OnPropertyChanged(); }
            }
            private int _SelectedKey1 = 0;
            public int SelectedKey1
            {
                get { return _SelectedKey1; }
                set { _SelectedKey1 = value; OnPropertyChanged(); OnSelectedChanged(); }
            }

            //Key汇总
            public Key[] GetKeys
            {
                get
                {
                    if (SelectedKey0 > 0 && SelectedKey1 > 0)
                    {
                        return [KeyEnumList0[SelectedKey0], KeyEnumList1[SelectedKey1]];
                    }
                    else if (SelectedKey0 > 0)
                    {
                        return [KeyEnumList0[SelectedKey0]];
                    }
                    else if (SelectedKey1 > 0)
                    {
                        return [KeyEnumList1[SelectedKey1]];
                    }
                    else
                    {
                        return Array.Empty<Key>();
                    }
                }
            }

            public ItemViewModel(string text, string accesskey, Action callback)
            {
                _text = text;
                _accesskey = accesskey;
                _callback = callback;

                KeyEnumList0 = Enum.GetValues(typeof(Key)).Cast<Key>().ToList();
                KeyEnumList1 = Enum.GetValues(typeof(Key)).Cast<Key>().ToList();

                //if (keys?.Length > 0) { SelectedKey0 = KeyEnumList0.IndexOf((keys[0])); }
                //if (keys?.Length > 1) { SelectedKey1 = KeyEnumList1.IndexOf((keys[1])); }

                if (this.GetKeys.Length > 0) { HotKeyManager.Instance.RegisterHotKey(_accesskey, this.GetKeys, _callback); }
            }

            private void OnSelectedChanged()
            {
                HotKeyManager.Instance.UnregisterHotKey(_accesskey);

                if (this.GetKeys.Length > 0)
                {
                    HotKeyManager.Instance.RegisterHotKey(_accesskey, this.GetKeys, _callback);
                    WeakReferenceMessenger.Default.Send(new PrintNewMessage($"RegisterHotKey: {_text}, {string.Join(",", this.GetKeys)}"));
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new PrintNewMessage($"UnregisterHotKey: {_text}"));
                }
            }
        }
    }

    partial class cMenuItemForHotkey_viewmodel
    {
        public ObservableCollection<ItemViewModel> HotkeyInfoList { get; init; }

        public cMenuItemForHotkey_viewmodel(string text, string feature, Action callback)
        {
            HotkeyInfoList = new()
            {
                new ItemViewModel(text, feature, callback),
            };
        }
    }
}
