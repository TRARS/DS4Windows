using System.Windows.Controls;

namespace CustomMacroFactory.MainWindow.UserControlEx.ClientEx
{
    /// <summary>
    /// uClient.xaml 的交互逻辑
    /// </summary>
    public partial class uClient : UserControl
    {
        public uClient()
        {
            InitializeComponent();
            this.DataContext = new uClient_viewmodel();
        }


    }
}
