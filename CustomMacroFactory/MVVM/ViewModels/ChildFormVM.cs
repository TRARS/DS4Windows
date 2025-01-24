using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TrarsUI.Shared.DTOs;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Interfaces.UIComponents;
using TrarsUI.Shared.Messages;

namespace CustomMacroFactory.MVVM.ViewModels
{
    partial class ChildFormVM : ObservableObject, IChildFormVM
    {
        public ObservableCollection<IToken> SubViewModelList { get; init; }
        public ObservableCollection<DialogPacket> DialogMessageList { get; init; }

        [ObservableProperty]
        private TaskCompletionSource<bool> taskCompletionSource;

        public ChildFormVM(IMessageBoxService messageBox,
                           ITokenProviderService tokenProvider,
                           IDialogYesNoService dialogYesNo,
                           IAbstractFactory<IuTitleBarVM> titleBarFactory,
                           IAbstractFactory<IuRainbowLineVM> rainbowLineFactory,
                           IAbstractFactory<IuClientVM> clientFactory)
        {
            var token = tokenProvider.GetRecurrentTokenForChildForm();

            var titleBar = titleBarFactory.Create();
            var rainbowLine = rainbowLineFactory.Create();
            var client = clientFactory.Create();

            titleBar.Token = rainbowLine.Token = client.Token = token;

            this.DialogMessageList = new();
            this.SubViewModelList = new()
            {
                titleBar,
                rainbowLine,
                client,
            };

            // 设置内容
            WeakReferenceMessenger.Default.Register<SetContentMessage, string>(this, token, (r, m) =>
            {
                var contentVM = m.Value;
                {
                    titleBar.Title = $"{contentVM?.Title ?? "null"}";// + " -> " + token;
                }
                client.SetContent(contentVM);
            });

            // 设置Icon
            WeakReferenceMessenger.Default.Register<SetTitleBarIconMessage, string>(this, token, (r, m) =>
            {
                titleBar.SetIcon(m.Value);
            });

            // 本地弹框
            WeakReferenceMessenger.Default.Register<DialogYesNoMessage, string>(this, token, (r, m) =>
            {
                m.Reply(dialogYesNo.ShowDialog(
                    m,
                    () => { TaskCompletionSource = new(); return TaskCompletionSource; },
                    dp => { this.DialogMessageList.Remove(dp); },
                    dp => { this.DialogMessageList.Add(dp); }));
            });
        }
    }
}
