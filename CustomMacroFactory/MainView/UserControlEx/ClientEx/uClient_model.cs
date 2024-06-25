using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CustomMacroFactory.MainView.UserControlEx.ClientEx
{
    partial class uClient_model
    {
        public ObservableCollection<UIElement> PartCollection { get; set; } = new(Enumerable.Repeat(new UIElement(), 10));
        public ObservableCollection<bool> DoubleCollection { get; set; } = new(Enumerable.Repeat(false, 10));

        public ObservableCollection<MenuItem_model> MenuItemModelList { get; set; } = new();
    }
}
