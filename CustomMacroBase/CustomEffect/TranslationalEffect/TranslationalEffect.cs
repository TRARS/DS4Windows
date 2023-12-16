using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace CustomMacroBase.CustomEffect
{
    public class TranslationalEffect : ShaderEffect
    {
        string? AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(TranslationalEffect), 0);
        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register(nameof(Threshold), typeof(double), typeof(TranslationalEffect), new UIPropertyMetadata(1d, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(nameof(HorizontalOffset), typeof(double), typeof(TranslationalEffect), new UIPropertyMetadata(1d, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(nameof(VerticalOffset), typeof(double), typeof(TranslationalEffect), new UIPropertyMetadata(1d, PixelShaderConstantCallback(2)));

        /// <summary>
        /// 线性减淡+平移
        /// </summary>
        public TranslationalEffect()
        {
            PixelShader = new PixelShader
            {
                UriSource = new Uri($"pack://application:,,,/{AssemblyName};component/CustomEffect/TranslationalEffect/TranslationalEffect.ps"),
            };

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ThresholdProperty);
            UpdateShaderValue(HorizontalOffsetProperty);
            UpdateShaderValue(VerticalOffsetProperty);
        }

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public double Threshold
        {
            get => (double)GetValue(ThresholdProperty);
            set => SetValue(ThresholdProperty, value);
        }
        public double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }
        public double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }
    }
}
