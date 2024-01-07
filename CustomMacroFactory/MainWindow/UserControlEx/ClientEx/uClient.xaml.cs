using System.Windows.Controls;

namespace CustomMacroFactory.MainWindow.UserControlEx.ClientEx
{
    public partial class uClient : UserControl
    {
        public uClient()
        {
            InitializeComponent();
            this.DataContext = new uClient_viewmodel();
        }
    }
}
