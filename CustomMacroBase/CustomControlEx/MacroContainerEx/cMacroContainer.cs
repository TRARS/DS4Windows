using CommunityToolkit.Mvvm.Input;
using CustomMacroBase.DTOs;
using CustomMacroBase.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomMacroBase.CustomControlEx.MacroContainerEx
{
    public partial class cMacroContainer : Control
    {
        static cMacroContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cMacroContainer), new FrameworkPropertyMetadata(typeof(cMacroContainer)));
        }
    }

    public partial class cMacroContainer
    {
        public ObservableCollection<IMacroPacket> ItemsSource
        {
            get { return (ObservableCollection<IMacroPacket>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            name: "ItemsSource",
            propertyType: typeof(ObservableCollection<IMacroPacket>),
            ownerType: typeof(cMacroContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, async (s, e) =>
            {
                if (((ObservableCollection<IMacroPacket>)e.NewValue).FirstOrDefault(item => item.IsChecked) is MacroPacket item)
                {
                    await Task.Yield();
                    ((cMacroContainer)s).SelectedItem = item;
                }
            })
        );

        public MacroPacket SelectedItem
        {
            get { return (MacroPacket)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            name: "SelectedItem",
            propertyType: typeof(MacroPacket),
            ownerType: typeof(cMacroContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double PageMinWidth
        {
            get { return (double)GetValue(PageMinWidthProperty); }
            set { SetValue(PageMinWidthProperty, value); }
        }
        public static readonly DependencyProperty PageMinWidthProperty = DependencyProperty.Register(
            name: "PageMinWidth",
            propertyType: typeof(double),
            ownerType: typeof(cMacroContainer),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );


    }

    public partial class cMacroContainer
    {
        public ICommand MoveUpCommand
        {
            get { return (ICommand)GetValue(MoveUpCommandProperty); }
            set { SetValue(MoveUpCommandProperty, value); }
        }
        public static readonly DependencyProperty MoveUpCommandProperty = DependencyProperty.Register(
            name: "MoveUpCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cMacroContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ICommand MoveDownCommand
        {
            get { return (ICommand)GetValue(MoveDownCommandProperty); }
            set { SetValue(MoveDownCommandProperty, value); }
        }
        public static readonly DependencyProperty MoveDownCommandProperty = DependencyProperty.Register(
            name: "MoveDownCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cMacroContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ICommand RemoveCommand
        {
            get { return (ICommand)GetValue(RemoveCommandProperty); }
            set { SetValue(RemoveCommandProperty, value); }
        }
        public static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(
            name: "RemoveCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cMacroContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ICommand UnusedCommand
        {
            get { return (ICommand)GetValue(UnusedCommandProperty); }
            set { SetValue(UnusedCommandProperty, value); }
        }
        public static readonly DependencyProperty UnusedCommandProperty = DependencyProperty.Register(
            name: "UnusedCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cMacroContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    public partial class cMacroContainer
    {
        [RelayCommand]
        private void OnVerticalRadioButtonSelectedChanged(object para)
        {
            this.SelectedItem = (MacroPacket)para;
        }
    }
}
