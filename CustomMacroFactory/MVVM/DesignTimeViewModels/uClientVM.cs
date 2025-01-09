using TrarsUI.Shared.Interfaces.UIComponents;

namespace CustomMacroFactory.MVVM.DesignTimeViewModels
{
    internal class uClientVM : IuClientVM
    {
        public object Content { get; set; }
        public string Token { get; set; }

        public void SetContent(object content) { }
    }
}
