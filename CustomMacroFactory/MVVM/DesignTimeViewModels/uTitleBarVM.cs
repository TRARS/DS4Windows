using TrarsUI.Shared.Interfaces.UIComponents;

namespace CustomMacroFactory.MVVM.DesignTimeViewModels
{
    internal class uTitleBarVM : IuTitleBarVM
    {
        public string Title { get; set; } = "Designtime uTitleBar";
        public string Token { get; set; } = string.Empty;

        public void SetIcon(string icon) => throw new System.NotImplementedException();
    }
}
