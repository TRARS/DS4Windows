using CustomMacroBase.Helper;
using CustomMacroFactory.MainView.Interfaces;
using System.Windows;

namespace CustomMacroFactory.MainView.UserControlEx.TitleBarEx
{
    public partial class uTitleBar_viewmodel : NotificationObject, IViewModel
    {
        private readonly uTitleBar_model model = new();

        public string Title
        {
            get { return model.Title; }
            set
            {
                if (model.Title == value)
                    return;
                model.Title = value;
                NotifyPropertyChanged();
            }
        }

        public uTitleBar_viewmodel()
        {
            this.Title = $"Macro Extension ({System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location):yyyy-MM-dd HH:mm:ss})";

            ButtonEventInit();
        }
    }

    public partial class uTitleBar_viewmodel
    {
        public RelayCommand ResetPosButtonCommand { get; set; }
        public RelayCommand MinimizeButtonCommand { get; set; }
        public RelayCommand MaximizeButtonCommand { get; set; }
        public RelayCommand CloseButtonCommand { get; set; }

        private void ButtonEventInit()
        {
            ResetPosButtonCommand = new(para =>
            {
                Mediator.Instance.NotifyColleagues(MessageType.WindowPosReset, new Vector(0, 0));
            });
            MinimizeButtonCommand = new(para => { throw new System.NotImplementedException(); });
            MaximizeButtonCommand = new(para => { throw new System.NotImplementedException(); });
            CloseButtonCommand = new(para =>
            {
                Mediator.Instance.NotifyColleagues(MessageType.WindowClose, null);
            });
        }
    }
}
