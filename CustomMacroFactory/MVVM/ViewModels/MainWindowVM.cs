using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TrarsUI.Shared.DTOs;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Interfaces.UIComponents;
using TrarsUI.Shared.Messages;

namespace CustomMacroFactory.MVVM.ViewModels
{
    partial class MainWindowVM : ObservableObject, IMainWindowVM
    {
        public ObservableCollection<IToken> SubViewModelList { get; init; }
        public ObservableCollection<DialogPacket> DialogMessageList { get; init; }

        [ObservableProperty]
        private TaskCompletionSource<bool> taskCompletionSource;

        public MainWindowVM(IMessageBoxService messageBox,
                            ITokenProviderService tokenProvider,
                            IContentProviderService contentProvider,
                            IAbstractFactory<IChildForm> childFormFactory,
                            IAbstractFactory<IuTitleBarVM> titleBarFactory,
                            IAbstractFactory<IuRainbowLineVM> rainbowLineFactory,
                            IAbstractFactory<IuClientVM> clientFactory)
        {
            var token = tokenProvider.MainWindowToken;

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
            {
                var contentVM = contentProvider.Create();
                {
                    titleBar.Title = $"{contentVM?.Title ?? "null"}";// + " -> " + token;
                }
                client.SetContent(contentVM);
            }

            // 打开子窗体
            WeakReferenceMessenger.Default.Register<OpenChildFormMessage>(this, (r, m) =>
            {
                var childForm = childFormFactory.Create();
                {
                    var context = m.Value;
                    childForm.SetWindowInfo(context.WindowInfo);
                    childForm.SetClientContent(context.ViewModel);
                    childForm.SetTitleBarIcon(context.Icon);
                    childForm.Show();
                }
            });

            // 本地弹框
            WeakReferenceMessenger.Default.Register<DialogYesNoMessage, string>(this, token, (r, m) =>
            {
                m.Reply(((Func<Task<bool>>)(() =>
                {
                    var tk = tokenProvider.GetRandomToken();
                    var msg = m.Message;
                    var dp = new DialogPacket(tk, msg);
                    m.Callback?.Invoke(() => { this.DialogMessageList.Remove(dp); tokenProvider.RemoveToken(tk); });
                    this.DialogMessageList.Add(dp);

                    TaskCompletionSource = new TaskCompletionSource<bool>();
                    return TaskCompletionSource.Task;
                }))());
            });
        }
    }
}
