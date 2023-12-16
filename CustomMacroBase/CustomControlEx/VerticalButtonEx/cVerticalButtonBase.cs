using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.VerticalButtonEx
{
    public partial class cVerticalButtonBase : Button
    {
        static cVerticalButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cVerticalButtonBase), new FrameworkPropertyMetadata(typeof(cVerticalButtonBase)));
        }
        public cVerticalButtonBase()
        {
            base.Click += (s, e) =>
            {
                //已知数据源是宿主控件本身，Style里的绑定路径可以显示地写上 DataContext.XXX
                var host = ((cVerticalButton)this.DataContext);
                host.Command?.Invoke();
                if (host.SubContent.Count > 0)
                {
                    host.SubContentVisibility = !host.SubContentVisibility;
                    host.BadgeText = host.SubContentVisibility ? "-" : "+";
                }
                else if (host.SubFlag && host.BadgeText.Length == 0)
                {
                    host.BadgeText = "-";
                }
            };
        }
    }
}
