using CustomMacroBase;
using System.Collections.Generic;
using DS4StateLite = CustomMacroBase.GamePadState.DS4StateLite;

namespace CustomMacroFactory.MacroFactory
{
    public static partial class MacroManager
    {
        static readonly MacroCreator Creator = new();
        static MacroManager() { }
    }

    public static partial class MacroManager
    {
        public static List<MacroBase> GameList => Creator.CurrentGameList;
        public static MacroBase? PreSettings => Creator.PreSettingsMacro;

        public static void Entry(in int ind, in DS4StateLite _realState, in DS4StateLite _virtualState)
        {
            Creator.PreSettingsMacro?.UpdateEntry(in ind, in _realState, in _virtualState); //Inner
            Creator.CurrentRunnableMacro?.UpdateEntry(in ind, in _realState, in _virtualState);
        }
    }
}
