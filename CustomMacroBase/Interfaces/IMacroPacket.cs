namespace CustomMacroBase.Interfaces
{
    public interface IMacroPacket
    {
        string IconData { get; set; }
        string ButtonText { get; set; }
        object MacroContent { get; set; }
        bool IsChecked { get; set; }
        bool Unused { get; set; }
        bool ColorfulText { get; set; }
        MacroBase MacroBase { get; set; }
    }
}
