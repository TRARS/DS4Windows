using CustomMacroBase.Helper;
using CustomMacroFactory.MainView.UserControlEx.ClientEx;
using CustomMacroFactory.MainView.UserControlEx.RainbowLineEx;
using CustomMacroFactory.MainView.UserControlEx.TitleBarEx;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CustomMacroFactory.MainView
{
    partial class MainWindow_viewmodel : NotificationObject
    {
        public ObservableCollection<UIElement> ViewList { get; init; }
        public RelayCommand LoadedCommand { get; set; }
        public RelayCommand ClosingCommand { get; set; }
        public RelayCommand PreviewKeyDownCommand { get; set; }

        public MainWindow_viewmodel()
        {
            CreateCommand();

            ViewList = new()
            {
                MainEntry.GetService<uTitleBar>(),
                MainEntry.GetService<uRainbowLine>(),
                MainEntry.GetService<uClient>()
            };
        }

        private void CreateCommand()
        {
            LoadedCommand = new(para =>
            {
                if (para is RoutedEventArgs e)
                {
                    Mediator.Instance.NotifyColleagues(MainWindowMessageType.Loaded, null);
                }
            });
            ClosingCommand = new(para =>
            {
                if (para is CancelEventArgs e)
                {
                    e.Cancel = true;
                    Mediator.Instance.NotifyColleagues(MainWindowMessageType.Closing, null);
                }
            });
            PreviewKeyDownCommand = new(para =>
            {
                if (para is KeyEventArgs e)
                {
                    Mediator.Instance.NotifyColleagues(MessageType.WindowKeyDown, e.Key);
                }
            });
        }
    }
}
