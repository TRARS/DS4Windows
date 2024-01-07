using System.Windows.Controls;

namespace CustomMacroFactory.MainWindow.UserControlEx.PixelPicker
{
    public partial class uPixelPicker : UserControl
    {
        uPixelPicker_viewmodel viewmodel = new();

        public uPixelPicker()
        {
            InitializeComponent();
            this.DataContext = viewmodel;

            this.Loaded += (s, e) => { viewmodel.Loaded?.Invoke(s, e); };
            this.PreviewMouseRightButtonUp += (s, e) => { viewmodel.PreviewMouseRightButtonUp?.Invoke(s, e); };
            this.PreviewMouseLeftButtonDown += (s, e) => { viewmodel.PreviewMouseLeftButtonDown?.Invoke(s, e); };
            this.PreviewMouseLeftButtonUp += (s, e) => { viewmodel.PreviewMouseLeftButtonUp?.Invoke(s, e); };
            this.MouseEnter += (s, e) => { viewmodel.MouseEnter?.Invoke(s, e); };
            this.MouseLeave += (s, e) => { viewmodel.MouseLeave?.Invoke(s, e); };
            this.MouseMove += (s, e) => { viewmodel.MouseMove?.Invoke(s, e); };
        }
    }
}
