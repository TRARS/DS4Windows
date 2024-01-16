using System.Windows;

namespace DS4WinWPF.UI.Helper.Base
{
    // Model
    public class UniversalObjectModel
    {
        public Point ClickPos { get; set; } = new Point(0, 0);
    }

    // ViewModel
    public partial class UniversalObjectViewModel : NotificationObject
    {
        private UniversalObjectModel model = new();

        public Point ClickPos
        {
            get { return model.ClickPos; }
            set
            {
                if (model.ClickPos == value) { return; }
                model.ClickPos = value;
                NotifyPropertyChanged();
            }
        }


    }
}
