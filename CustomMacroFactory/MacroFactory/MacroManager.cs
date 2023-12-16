﻿using CustomMacroBase;
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

        public static void Entry(in DS4StateLite _realState, in DS4StateLite _virtualState)
        {
            Creator.CurrentRunnableMacro?.UpdateEntry(in _realState, in _virtualState);
        }
    }
}
