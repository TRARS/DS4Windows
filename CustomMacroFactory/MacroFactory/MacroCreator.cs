using CustomMacroBase;
using CustomMacroBase.Helper.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CustomMacroFactory.MacroFactory
{
    public partial class MacroCreator
    {
        private bool is_in_designmode => (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

        private bool is_busy = true;
        private List<MacroBase> game_list_main = new();
        private List<MacroBase> game_list_pre = new(); //inner

        public MacroCreator()
        {
            Task.Run(() => LoadFromAssembly()).Wait();
        }

        private void LoadFromAssembly()
        {
            Func<string, bool> IsFileNameMatch = (_) =>
            {
                return Regex.IsMatch(_, ".*CustomMacroSample.*") ||
                       Regex.IsMatch(_, ".*CustomMacroExample.*") ||
                       Regex.IsMatch(_, ".*CustomMacroPlugin.*") ||
                       Regex.IsMatch(_, ".*CustomMacroExtension.*");
            };
            Func<string?, bool> IsNamespaceMatch = (_) =>
            {
                if (_ is null) return false;
                return Regex.IsMatch(_, ".*CustomMacroSample.*") ||
                       Regex.IsMatch(_, ".*CustomMacroExample.*") ||
                       Regex.IsMatch(_, ".*CustomMacroPlugin.*") ||
                       Regex.IsMatch(_, ".*CustomMacroExtension.*");
            };
            Func<string?, bool> IsClassNameMatch = (_) =>
            {
                if (_ is null) return false;
                return Regex.IsMatch(_, ".*Game_.*");
            };

            try
            {
                is_busy = true;

                //载入外部Macro
                if (is_in_designmode is false)
                {
                    string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
                    foreach (string _dllpath in files)
                    {
                        if (IsFileNameMatch(Path.GetFileName(_dllpath)))
                        {
                            var rawAssembly = File.ReadAllBytes(_dllpath);//
                            var tpList = (from t in Assembly.Load(rawAssembly).GetTypes()
                                          where (t.IsClass) &&
                                                (t.BaseType == typeof(MacroBase)) &&
                                                (IsNamespaceMatch(t.Namespace)) &&
                                                (IsClassNameMatch(t.Name))
                                          select t);

                            PushToList(ref game_list_main, tpList);
                        }
                    }

                    ReSort(ref game_list_main);
                }

                //载入内部Macro
                if (is_in_designmode)
                {
                    var _GameClassNamespace = "CustomMacroFactory.MacroFactory.Sample";
                    var _GameClassNamePrefix = "Game_";
                    //Assembly.Load("CustomMacroFactory").GetTypes()
                    var tpList = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                  where (t.IsClass) &&
                                        (t.BaseType == typeof(MacroBase)) &&
                                        (Regex.IsMatch(t.Namespace, _GameClassNamespace)) &&
                                        (Regex.IsMatch(t.Name, _GameClassNamePrefix))
                                  select t);

                    PushToList(ref game_list_main, tpList);
                    ReSort(ref game_list_main);
                }

                //载入摇杆调节Macro
                if (true)
                {
                    var _GameClassNamespace = "CustomMacroFactory.MacroFactory.InnerMacro";
                    var _GameClassNamePrefix = "Settings_";
                    //Assembly.Load("CustomMacroFactory").GetTypes()
                    var tpList = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                  where (t.IsClass) &&
                                        (t.BaseType == typeof(MacroBase)) &&
                                        (Regex.IsMatch(t.Namespace, _GameClassNamespace)) &&
                                        (Regex.IsMatch(t.Name, _GameClassNamePrefix))
                                  select t);

                    PushToList(ref game_list_pre, tpList);
                    ReSort(ref game_list_pre);
                }
            }
            catch (Exception ex) { MessageBox.Show($"GetGameList Error: {ex.Message}"); }
            finally { is_busy = false; }
        }
    }

    public partial class MacroCreator
    {
        public List<MacroBase> CurrentGameList => this.game_list_main;
        public MacroBase? CurrentRunnableMacro => is_busy ? null : game_list_main.Find(item => item.Selected);
        public MacroBase? AnalogStickMacro => is_busy ? null : game_list_pre[0];
    }

    public partial class MacroCreator
    {
        private void PushToList(ref List<MacroBase> list, IEnumerable<Type> tpList)
        {
            foreach (Type item in tpList)
            {
                try
                {
                    if (item.FullName is not null && item.Assembly.CreateInstance(item.FullName) is MacroBase obj)
                    {
                        list.Add(obj);
                    }
                }
                catch (Exception ex) { MessageBox.Show($"{item.Name}: {ex.Message}"); }
            }
        }

        private void ReSort(ref List<MacroBase> list)
        {
            if (list.Any() is false || list.Count == 1) { return; }

            var temp = new List<MacroBase>();

            foreach (var item in list.OrderBy(x => x.GetSortIndex()).ToList())
            {
                if (item.IsUnused() is false)
                {
                    temp.Add(item);//被标记为[DoNotLoad]的游戏类将不会被载入
                }
            }

            list = temp;
        }
    }
}
