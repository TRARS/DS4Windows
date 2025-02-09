using CommunityToolkit.Mvvm.Messaging;
using CustomMacroFactory.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrarsUI.Shared.DTOs;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Messages;

namespace CustomMacroFactory.MVVM.Helpers
{
    internal sealed partial class Manager
    {
        readonly Dictionary<string, string> _windows = new();
        readonly List<string> _tokens = new();

        readonly IAbstractFactory<ImageColorPickerVM> _imageColorPickerVMFactory;

        public Manager(IAbstractFactory<ImageColorPickerVM> imageColorPickerVMFactory)
        {
            _imageColorPickerVMFactory = imageColorPickerVMFactory;
        }

        public async Task OpenDialog(string msg, string token)
        {
            Action? yesnoCallback = null;
            await WeakReferenceMessenger.Default.Send(new DialogYesNoMessage(msg, (x) => { yesnoCallback = x; }), token);
            yesnoCallback?.Invoke();
        }

        public async void OpenImageColorPicker()
        {
            await this.OpenWith<ImageColorPickerVM>(async () =>
            {
                var token = await WeakReferenceMessenger.Default.Send(new OpenChildFormMessage(new()
                {
                    Icon = @"M477.300081 737.89955l78.385789 65.432422c19.558048 236.283747-269.409551 288.967599-385.068269 127.997696 195.682878 16.383705 125.591339-191.535752 306.68248-193.430118z m501.238978-560.629909a27.596303 27.596303 0 0 1 43.519216 30.719447l-2.918347 5.683098c-14.847733 25.189947-190.153377 322.554194-248.469128 414.251743a214.728935 214.728935 0 0 1-126.512923 102.039764l-43.928809 17.868478-71.67871-59.749324 9.625427-46.488764a212.578574 212.578574 0 0 1 69.067557-135.421562l19.762844-18.534066c94.4623-87.038433 351.481673-310.317614 351.481673-310.317615z
                         M835.642431 684.038119c117.19469 150.218096-37.528924 305.760896-255.636999 335.097969l-11.724589 1.433574 1.945565-1.843167a276.987014 276.987014 0 0 0 71.67871-195.734077l34.559378-14.079746a298.132234 298.132234 0 0 0 159.075537-124.925752zM507.763532 0.018432c172.2337-1.382375 306.272887 76.030631 348.92172 170.236935l-168.444968 147.965337q-66.200408 58.162153-131.069641 117.75788c-82.78891 77.310608-90.161577 129.021678-108.695643 218.108074a239.611687 239.611687 0 0 0-189.743785 139.159096c-37.119332 63.230862-69.42595 66.558802-138.237512 48.383129A511.990784 511.990784 0 0 1 507.814732 0.018432zM214.03442 458.966971a85.246466 85.246466 0 1 0 60.619708 24.524358 85.297665 85.297665 0 0 0-60.619708-24.524358z m85.502461-255.995392a85.092868 85.092868 0 1 0 60.414912 24.473159 85.092868 85.092868 0 0 0-60.414912-24.473159z m255.995392-85.297665a85.195266 85.195266 0 1 0 60.466111 24.370761 85.297665 85.297665 0 0 0-60.466111-24.268363z",
                    ViewModel = _imageColorPickerVMFactory.Create(),
                    WindowInfo = new WindowInfo()
                    {
                        Width = 640,
                        Height = 540,
                        MinWidth = 640,
                        MinHeight = 540,
                        MaxWidth = 640,
                        MaxHeight = 540
                    }
                }));
                return token;
            });
        }
    }

    internal sealed partial class Manager
    {
        private async Task OpenWith<T>(Func<Task<string>> action)
        {
            var type = typeof(T).Name;
            if (this.Begin(type)) { return; }
            var token = await action.Invoke();
            this.After(type, token);
        }

        private bool Begin(string type)
        {
            // 限制打开数量
            if (_windows.ContainsKey(type))
            {
                this.ShowChildForm(_windows[type]); return true;
            }

            return false;
        }

        private void After(string type, string token)
        {
            // VM宿主关闭时反注册
            WeakReferenceMessenger.Default.Register<WindowClosingMessage, string>(this, token, (r, m) =>
            {
                _windows.Remove(type);
                WeakReferenceMessenger.Default.Unregister<WindowClosingMessage, string>(this, token);
            });

            // 内部维护
            _tokens.Add(token);
            _windows.TryAdd(type, token);
        }
    }

    internal sealed partial class Manager
    {
        public void ShowAllChildForm()
        {
            _tokens.ForEach(token =>
            {
                WeakReferenceMessenger.Default.Send(new WindowNormalizeMessage("ShowAllChildForm"), token);
            });
        }

        public void CloseAllChildForm()
        {
            _tokens.ForEach(token =>
            {
                WeakReferenceMessenger.Default.Send(new WindowCloseMessage("CloseAllChildForm"), token);
            });
            _tokens.Clear();
        }

        public void ShowChildForm(string token)
        {
            WeakReferenceMessenger.Default.Send(new WindowNormalizeMessage("ShowChildForm"), token);
        }

        public void CloseChildForm(string token)
        {
            WeakReferenceMessenger.Default.Send(new WindowCloseMessage("CloseAllChildForm"), token);
            _tokens.Remove(token);
        }
    }
}
