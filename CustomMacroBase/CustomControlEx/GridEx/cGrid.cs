using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.GridEx
{
    public partial class cGrid : Grid
    {
        static cGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cGrid), new FrameworkPropertyMetadata(typeof(cGrid)));
        }
    }

    public partial class cGrid
    {
        /// <summary>
        /// DataContext中转
        /// </summary>
        public object ParentModel
        {
            get { return (object)GetValue(ParentModelProperty); }
            set { SetValue(ParentModelProperty, value); }
        }
        public static readonly DependencyProperty ParentModelProperty = DependencyProperty.Register(
            name: "ParentModel",
            propertyType: typeof(object),
            ownerType: typeof(cGrid),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
