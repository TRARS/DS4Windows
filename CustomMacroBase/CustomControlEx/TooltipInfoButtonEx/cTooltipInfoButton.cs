using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.TooltipInfoButtonEx
{
    public partial class cTooltipInfoButton : Button
    {
        static cTooltipInfoButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cTooltipInfoButton), new FrameworkPropertyMetadata(typeof(cTooltipInfoButton)));
        }

        public cTooltipInfoButton()
        {
            this.Click += (s, e) =>
            {
                this.ContextMenuIsOpen = true;
            };
        }
    }

    public partial class cTooltipInfoButton
    {
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register(
            name: "BorderCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cTooltipInfoButton),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        //
        public bool ContextMenuIsOpen
        {
            get { return (bool)GetValue(ContextMenuIsOpenProperty); }
            set { SetValue(ContextMenuIsOpenProperty, value); }
        }
        public static readonly DependencyProperty ContextMenuIsOpenProperty = DependencyProperty.Register(
            name: "ContextMenuIsOpen",
            propertyType: typeof(bool),
            ownerType: typeof(cTooltipInfoButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        //
        public UIElement ToolTipContent
        {
            get { return (UIElement)GetValue(ToolTipContentProperty); }
            set { SetValue(ToolTipContentProperty, value); }
        }
        public static readonly DependencyProperty ToolTipContentProperty = DependencyProperty.Register(
            name: "ToolTipContent",
            propertyType: typeof(UIElement),
            ownerType: typeof(cTooltipInfoButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
