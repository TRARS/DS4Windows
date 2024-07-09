using CustomMacroBase.Helper;
using CustomMacroFactory.MainView.Interfaces;
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
        public ObservableCollection<IViewModel> SubViewModelList { get; init; }
        public RelayCommand LoadedCommand { get; set; }
        public RelayCommand ClosingCommand { get; set; }
        public RelayCommand PreviewKeyDownCommand { get; set; }

        public MainWindow_viewmodel()
        {
            CreateCommand();

            SubViewModelList = new()
            {
                MainEntry.GetService<uTitleBar_viewmodel>(),
                MainEntry.GetService<uRainbowLine_viewmodel>(),
                MainEntry.GetService<uClient_viewmodel>()
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
