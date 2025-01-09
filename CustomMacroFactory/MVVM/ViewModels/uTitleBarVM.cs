using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Media;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Interfaces.UIComponents;
using TrarsUI.Shared.Messages;

namespace CustomMacroFactory.MVVM.ViewModels
{
    partial class uTitleBarVM : ObservableObject, IuTitleBarVM
    {
        private IMessageBoxService _messageBox;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private bool isTopmost;

        [ObservableProperty]
        private bool isMaximized;

        [ObservableProperty]
        private string token;

        [ObservableProperty]
        private Geometry? icon;

        public uTitleBarVM(IMessageBoxService messageBox)
        {
            this._messageBox = messageBox;

            this.Title = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}";
        }
    }

    partial class uTitleBarVM
    {
        partial void OnTokenChanged(string value)
        {
            WeakReferenceMessenger.Default.Register<WindowStateUpdateMessage, string>(this, value, (r, m) =>
            {
                this.IsMaximized = m.Value.IsMaximized;
                this.IsTopmost = m.Value.IsTopmost;
            });
        }

        partial void OnTitleChanged(string value)
        {
            if (string.IsNullOrEmpty(Token) is false)
            {
                WeakReferenceMessenger.Default.Send(new WindowTitleChangedMessage(value), Token);
            }
        }
    }

    partial class uTitleBarVM
    {
        [RelayCommand]
        private async Task OnTopmostButton()
        {
            this.IsTopmost = await WeakReferenceMessenger.Default.Send(new WindowTopmostMessage(), Token);
        }

        [RelayCommand]
        private void OnResetPosButton()
        {
            WeakReferenceMessenger.Default.Send(new WindowPosResetMessage(null), Token);
        }

        [RelayCommand]
        private void OnMinimizeButton()
        {
            WeakReferenceMessenger.Default.Send(new WindowMinimizeMessage("OnMinimize"), Token);
        }

        [RelayCommand]
        private void OnMaximizeButton()
        {
            WeakReferenceMessenger.Default.Send(new WindowMaximizeMessage("OnMaximizeButton"), Token);
        }

        [RelayCommand]
        private void OnCloseButton()
        {
            WeakReferenceMessenger.Default.Send(new WindowCloseMessage("OnClose"), Token);
        }
    }

    partial class uTitleBarVM
    {
        public void SetIcon(string icon)
        {
            if (string.IsNullOrWhiteSpace(icon)) { return; }

            Geometry? data;

            try
            {
                data = Geometry.Parse(icon.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                return;
            }

            Icon = data;
        }
    }
}
