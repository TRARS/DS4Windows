using CommunityToolkit.Mvvm.Input;
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
    }

    public partial class cFoldableContainer
    {
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            name: "Header",
            propertyType: typeof(object),
            ownerType: typeof(cFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Visibility ArrowVisibility
        {
            get { return (Visibility)GetValue(ArrowVisibilityProperty); }
            set { SetValue(ArrowVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ArrowVisibilityProperty = DependencyProperty.Register(
            name: "ArrowVisibility",
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

        public IRelayCommand ArrowRightClickCommand
        {
            get { return (IRelayCommand)GetValue(ArrowRightClickCommandProperty); }
            set { SetValue(ArrowRightClickCommandProperty, value); }
        }
        public static readonly DependencyProperty ArrowRightClickCommandProperty = DependencyProperty.Register(
            name: "ArrowRightClickCommand",
            propertyType: typeof(IRelayCommand),
            ownerType: typeof(cFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    public partial class cFoldableContainer
    {
        [RelayCommand]
        private void OnArrowClick()
        {
            this.BodyVisibility = this.BodyVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
