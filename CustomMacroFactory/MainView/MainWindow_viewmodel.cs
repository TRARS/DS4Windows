using CustomMacroBase.Helper;
using CustomMacroFactory.MainView.UserControlEx.ClientEx;
using CustomMacroFactory.MainView.UserControlEx.RainbowLineEx;
using CustomMacroFactory.MainView.UserControlEx.TitleBarEx;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CustomMacroFactory.MainView
{
    class MainWindow_viewmodel : NotificationObject
    {
        private readonly ObservableCollection<UIElement> ViewList = new(Enumerable.Repeat<UIElement>(new(), 10));

        public UIElement TitleBarView
        {
            get { return ViewList[0]; }
            set
            {
                if (ViewList[0] == value)
                    return;
                ViewList[0] = value;
                NotifyPropertyChanged();
            }
        }

        public UIElement RainbowLineView
        {
            get { return ViewList[1]; }
            set
            {
                if (ViewList[1] == value)
                    return;
                ViewList[1] = value;
                NotifyPropertyChanged();
            }
        }

        public UIElement ClientView
        {
            get { return ViewList[2]; }
            set
            {
                if (ViewList[2] == value)
                    return;
                ViewList[2] = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindow_viewmodel()
        {
            TitleBarView = MainEntry.GetService<uTitleBar>();
            RainbowLineView = MainEntry.GetService<uRainbowLine>();
            ClientView = MainEntry.GetService<uClient>();
        }
    }
}
