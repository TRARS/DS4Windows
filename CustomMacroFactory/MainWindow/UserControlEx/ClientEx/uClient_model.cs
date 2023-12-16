using CustomMacroBase.CustomControlEx.ToggleButtonEx;
using CustomMacroBase.CustomControlEx.VerticalButtonEx;
using CustomMacroBase.CustomControlEx.VerticalRadioButtonEx;
using CustomMacroBase.Helper;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace CustomMacroFactory.MainWindow.UserControlEx.ClientEx
{
    partial class uClient_model
    {
        public ObservableCollection<cVerticalButton> TopContent_Left { get; set; } = new();
        public ObservableCollection<cToggleButton> TopContent_LeftEx { get; set; } = new();
        public ObservableCollection<cVerticalRadioButton> TopContent_Middle { get; set; } = new();
        public ObservableCollection<ContentControl> TopContent_Right { get; set; } = new();
        public ObservableCollection<ContentControl> BottomContent_Top { get; set; } = new();
        public ObservableCollection<ContentControl> BottomContent_Bottom { get; set; } = new();

        public bool TopContent_Left_Hide { get; set; } = false;
        public bool TopContent_Middle_Hide { get; set; } = false;
        public bool TopContent_Right_Hide { get; set; } = false;
        public bool BottomContent_Top_Hide { get; set; } = false;
        public bool BottomContent_Bottom_Hide { get; set; } = false;

        public ObservableCollection<MenuItem_model> MenuItemModelList { get; set; } = new();
    }

    class MenuItem_model : NotificationObject
    {
        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                if (_Text == value)
                    return;
                _Text = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _HideGameListCommand;
        public RelayCommand HideGameListCommand
        {
            get { return _HideGameListCommand; }
            set
            {
                if (_HideGameListCommand == value)
                    return;
                _HideGameListCommand = value;
                NotifyPropertyChanged();
            }
        }
    }
}
