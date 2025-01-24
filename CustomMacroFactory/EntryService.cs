using CustomMacroFactory.MVVM.ViewModels;
using TrarsUI.Shared.Interfaces.UIComponents;

namespace CustomMacroFactory
{
    internal class EntryService : IContentProviderService
    {
        public IContentVM Create()
        {
            return new MacroViewerVM();
        }
    }
}
