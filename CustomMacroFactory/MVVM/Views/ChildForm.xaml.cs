using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using TrarsUI.Shared.DTOs;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Interfaces.UIComponents;
using TrarsUI.Shared.Messages;
using TrarsUI.SourceGenerator.Attributes;

namespace CustomMacroFactory.MVVM.Views
{
    [UseChrome]
    public partial class ChildForm : Window, IChildForm
    {
        ITokenProviderService _tokenProvider;
        IDebouncerService _debouncer;
        IStringEncryptorService _stringEncryptor;

        public ChildForm(ITokenProviderService tokenProvider, IDebouncerService debouncer, IStringEncryptorService stringEncryptor)
        {
            _tokenProvider = tokenProvider;
            _debouncer = debouncer;
            _stringEncryptor = stringEncryptor;

            InitializeComponent();
            InitWindowBorderlessBehavior(); // 无边框
            InitWindowMessageWithToken(); // 注册消息
        }
    }

    public partial class ChildForm
    {
        public void SetClientContent(IContentVM obj)
        {
            WeakReferenceMessenger.Default.Send(new SetContentMessage(obj), this.Token);
        }

        public void SetTitleBarIcon(string icon)
        {
            WeakReferenceMessenger.Default.Send(new SetTitleBarIconMessage(icon), this.Token);
        }

        public void SetWindowInfo(WindowInfo info)
        {
            WeakReferenceMessenger.Default.Send(new SetWindowInfoMessage(info), this.Token);
        }
    }
}
