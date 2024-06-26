using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.VerticalButtonEx
{
    public partial class cVerticalButton : Control
    {
        static cVerticalButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cVerticalButton), new FrameworkPropertyMetadata(typeof(cVerticalButton)));
        }

        public cVerticalButton()
        {
            this.DataContext = this;//自身作为数据源，用作父控件的搜索源

            this.Loaded += (s, e) =>
            {
                if (this.SubFlag is false)
                {
                    this.BadgeOnOff = this.SubContent.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    this.BadgeOnOff = Visibility.Visible;
                    this.BadgeText = "-";
                }
            };
        }

    }

    //主要属性
    public partial class cVerticalButton
    {
        public new Action Command
        {
            get { return (Action)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static new readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            name: "Command",
            propertyType: typeof(Action),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    //传递给子控件的属性
    public partial class cVerticalButton
    {
        public bool SubFlag
        {
            get { return (bool)GetValue(SubFlagProperty); }
            set { SetValue(SubFlagProperty, value); }
        }
        public static readonly DependencyProperty SubFlagProperty = DependencyProperty.Register(
            name: "SubFlag",
            propertyType: typeof(bool),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public List<cVerticalButton> SubContent
        {
            get { return (List<cVerticalButton>)GetValue(SubContentProperty); }
            set { SetValue(SubContentProperty, value); }
        }
        public static readonly DependencyProperty SubContentProperty = DependencyProperty.Register(
            name: "SubContent",
            propertyType: typeof(List<cVerticalButton>),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(new List<cVerticalButton>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool SubContentVisibility
        {
            get { return (bool)GetValue(SubContentVisibilityProperty); }
            set { SetValue(SubContentVisibilityProperty, value); }
        }
        public static readonly DependencyProperty SubContentVisibilityProperty = DependencyProperty.Register(
            name: "SubContentVisibility",
            propertyType: typeof(bool),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    //作为图标按钮时的属性
    public partial class cVerticalButton
    {
        public ObservableCollection<UIElement> Icon
        {
            get { return (ObservableCollection<UIElement>)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            name: "Icon",
            propertyType: typeof(ObservableCollection<UIElement>),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(new ObservableCollection<UIElement>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool IconOnly
        {
            get { return (bool)GetValue(IconOnlyProperty); }
            set { SetValue(IconOnlyProperty, value); }
        }
        public static readonly DependencyProperty IconOnlyProperty = DependencyProperty.Register(
            name: "IconOnly",
            propertyType: typeof(bool),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    //存在子按钮时显示角标
    public partial class cVerticalButton
    {
        public Visibility BadgeOnOff
        {
            get { return (Visibility)GetValue(BadgeOnOffProperty); }
            set { SetValue(BadgeOnOffProperty, value); }
        }
        public static readonly DependencyProperty BadgeOnOffProperty = DependencyProperty.Register(
            name: "BadgeOnOff",
            propertyType: typeof(Visibility),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public string BadgeText
        {
            get { return (string)GetValue(BadgeTextProperty); }
            set { SetValue(BadgeTextProperty, value); }
        }
        public static readonly DependencyProperty BadgeTextProperty = DependencyProperty.Register(
            name: "BadgeText",
            propertyType: typeof(string),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata("+", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    //
    public partial class cVerticalButton
    {
        public object RightClickContent
        {
            get { return (object)GetValue(RightClickContentProperty); }
            set { SetValue(RightClickContentProperty, value); }
        }
        public static readonly DependencyProperty RightClickContentProperty = DependencyProperty.Register(
            name: "RightClickContent",
            propertyType: typeof(object),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
