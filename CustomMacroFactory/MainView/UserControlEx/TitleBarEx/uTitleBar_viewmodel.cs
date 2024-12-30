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

        public bool Topmost
        {
            get { return model.Topmost; }
            set
            {
                if (model.Topmost == value)
                    return;
                model.Topmost = value;
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
        public RelayCommand LoadedCommand { get; set; }

        public RelayCommand TopmostButtonCommand { get; set; }
        public RelayCommand ResetPosButtonCommand { get; set; }
        public RelayCommand MinimizeButtonCommand { get; set; }
        public RelayCommand MaximizeButtonCommand { get; set; }
        public RelayCommand CloseButtonCommand { get; set; }

        private void ButtonEventInit()
        {
            LoadedCommand = new(para =>
            {
                TopmostButtonCommand?.Execute(true);
            });

            TopmostButtonCommand = new(para =>
            {
                this.Topmost = para is bool defaultValue ? defaultValue : !this.Topmost;
                Mediator.Instance.NotifyColleagues(MessageType.WindowTopmost, this.Topmost);
            });
            ResetPosButtonCommand = new(_ =>
            {
                Mediator.Instance.NotifyColleagues(MessageType.WindowPosReset, new Vector(0, 0));
            });
            MinimizeButtonCommand = new(_ => { throw new System.NotImplementedException(); });
            MaximizeButtonCommand = new(_ => { throw new System.NotImplementedException(); });
            CloseButtonCommand = new(_ =>
            {
                Mediator.Instance.NotifyColleagues(MessageType.WindowClose, null);
            });
        }
    }
}
