using System.Collections.ObjectModel;
using TrarsUI.Shared.Interfaces.UIComponents;

namespace CustomMacroFactory.MVVM.DesignTimeViewModels
{
    internal class MainWindowVM : IMainWindowVM
    {
        public ObservableCollection<IToken> SubViewModelList { get; init; } = new()
        {
            MainEntry.GetRequiredService<IuTitleBarVM>(),
            MainEntry.GetRequiredService<IuRainbowLineVM>(),
            MainEntry.GetRequiredService<IuClientVM>(),
        };
    }
}
