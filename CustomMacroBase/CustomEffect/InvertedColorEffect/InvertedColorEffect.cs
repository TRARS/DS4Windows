using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace CustomMacroBase.CustomEffect
{
    public class InvertColorEffect : ShaderEffect
    {
        string? AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(InvertColorEffect), 0);
        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register(nameof(Threshold), typeof(double), typeof(InvertColorEffect), new UIPropertyMetadata(0d, PixelShaderConstantCallback(0)));

        /// <summary>
        /// 颜色反转
        /// </summary>
        public InvertColorEffect()
        {
            PixelShader = new PixelShader
            {
                UriSource = new Uri($"pack://application:,,,/{AssemblyName};component/CustomEffect/InvertedColorEffect/InvertedColorEffect.ps"),
            };

            this.UpdateShaderValue(InputProperty);
        }

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }
        public double Threshold
        {
            get
            {
                return ((double)(this.GetValue(ThresholdProperty)));
            }
            set
            {
                this.SetValue(ThresholdProperty, value);
            }
        }
    }
}
