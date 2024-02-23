using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.AggregateBindingControl
{
    public partial class cAggregateBindingControl : Control
    {
        static cAggregateBindingControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cAggregateBindingControl), new FrameworkPropertyMetadata(typeof(cAggregateBindingControl)));
        }
    }

    public partial class cAggregateBindingControl
    {
        public double DoubleTemp1
        {
            get { return (double)GetValue(DoubleTemp1Property); }
            set { SetValue(DoubleTemp1Property, value); }
        }
        public static readonly DependencyProperty DoubleTemp1Property = DependencyProperty.Register(
            name: "DoubleTemp1",
            propertyType: typeof(double),
            ownerType: typeof(cAggregateBindingControl),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDoubleTemp1PropertyChanged)
        );
        private static void OnDoubleTemp1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (cAggregateBindingControl)d;
            var newDoubleTemp1Value = (double)e.NewValue;
            control.UpdateDoubleTemp2(newDoubleTemp1Value);
        }
        private void UpdateDoubleTemp2(double newDoubleTemp1Value)
        {
            DoubleTemp2 = newDoubleTemp1Value;
        }

        public double DoubleTemp2
        {
            get { return (double)GetValue(DoubleTemp2Property); }
            set { SetValue(DoubleTemp2Property, value); }
        }
        public static readonly DependencyProperty DoubleTemp2Property = DependencyProperty.Register(
            name: "DoubleTemp2",
            propertyType: typeof(double),
            ownerType: typeof(cAggregateBindingControl),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
