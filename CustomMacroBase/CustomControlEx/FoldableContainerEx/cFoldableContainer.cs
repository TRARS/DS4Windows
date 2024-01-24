using CustomMacroBase.Helper;
using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.FoldableContainerEx
{
    public partial class cFoldableContainer : ContentControl
    {
        static cFoldableContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cFoldableContainer), new FrameworkPropertyMetadata(typeof(cFoldableContainer)));
        }

        public cFoldableContainer()
        {
            this.ArrowCommand = new(_ =>
            {
                this.BodyVisibility = this.BodyVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            });
        }
    }

    public partial class cFoldableContainer
    {
        public object ExpanderHeader
        {
            get { return (object)GetValue(ExpanderHeaderProperty); }
            set { SetValue(ExpanderHeaderProperty, value); }
        }
        public static readonly DependencyProperty ExpanderHeaderProperty = DependencyProperty.Register(
            name: "ExpanderHeader",
            propertyType: typeof(object),
            ownerType: typeof(cFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Visibility ExpanderVisibility
        {
            get { return (Visibility)GetValue(ExpanderVisibilityProperty); }
            set { SetValue(ExpanderVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ExpanderVisibilityProperty = DependencyProperty.Register(
            name: "ExpanderVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(cFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Visibility BodyVisibility
        {
            get { return (Visibility)GetValue(BodyVisibilityProperty); }
            set { SetValue(BodyVisibilityProperty, value); }
        }
        public static readonly DependencyProperty BodyVisibilityProperty = DependencyProperty.Register(
            name: "BodyVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(cFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public RelayCommand ArrowCommand
        {
            get { return (RelayCommand)GetValue(ArrowCommandProperty); }
            set { SetValue(ArrowCommandProperty, value); }
        }
        public static readonly DependencyProperty ArrowCommandProperty = DependencyProperty.Register(
            name: "ArrowCommand",
            propertyType: typeof(RelayCommand),
            ownerType: typeof(cFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
