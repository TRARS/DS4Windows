using CustomMacroFactory.MVVM.ViewModels;
using TrarsUI.Shared.Interfaces.UIComponents;

namespace CustomMacroFactory.Factories
{
    internal class AContentProviderService : IContentProviderService
    {
        public IContentVM Create()
        {
            return new MacroViewerVM();
        }
    }
}
