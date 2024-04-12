using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace CustomMacroBase.CustomEffect.PolarCoordinatesEffect
{
    public class PolarCoordinatesEffect : ShaderEffect
    {
        string? AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(PolarCoordinatesEffect), 0);
        public static readonly DependencyProperty TileProperty = DependencyProperty.Register("Tile", typeof(double), typeof(PolarCoordinatesEffect), new UIPropertyMetadata(1D, PixelShaderConstantCallback(0)));

        /// <summary>
        /// 极坐标
        /// </summary>
        public PolarCoordinatesEffect()
        {
            PixelShader = new PixelShader
            {
                UriSource = new Uri($"pack://application:,,,/{AssemblyName};component/CustomEffect/PolarCoordinatesEffect/PolarCoordinatesEffect.ps"),
            };

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(TileProperty);
        }

        public Brush Input
        {
            get => (Brush)(this.GetValue(InputProperty));
            set => SetValue(InputProperty, value);
        }
        public double Tile
        {
            get => (double)(this.GetValue(TileProperty));
            set => SetValue(TileProperty, value);
        }
    }
}
