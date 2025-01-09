using CommunityToolkit.Mvvm.ComponentModel;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Interfaces.UIComponents;

namespace CustomMacroFactory.MVVM.ViewModels
{
    partial class uClientVM : ObservableObject, IuClientVM
    {
        private IMessageBoxService _messageBox;

        [ObservableProperty]
        private object content;

        [ObservableProperty]
        private string token;

        public uClientVM(IMessageBoxService messageBox)
        {
            this._messageBox = messageBox;
        }
    }

    partial class uClientVM
    {
        public void SetContent(object content)
        {
            this.Content = content;
        }
    }
}
