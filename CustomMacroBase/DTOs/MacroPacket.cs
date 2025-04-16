using CommunityToolkit.Mvvm.ComponentModel;
using CustomMacroBase.Interfaces;

namespace CustomMacroBase.DTOs
{
    public partial class MacroPacket : ObservableObject, IMacroPacket
    {
        [ObservableProperty]
        private string iconData;

        [ObservableProperty]
        private string buttonText;

        [ObservableProperty]
        private object macroContent;

        [ObservableProperty]
        private bool isChecked;

        [ObservableProperty]
        private bool unused;

        [ObservableProperty]
        private bool colorfulText;

        public MacroPacket(string text, object content)
        {
            this.ButtonText = text;
            this.MacroContent = content;
        }
    }
}
