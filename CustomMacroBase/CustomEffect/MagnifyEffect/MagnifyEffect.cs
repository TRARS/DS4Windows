using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace CustomMacroBase.CustomEffect
{
    /// <summary>An effect that magnifies a circular region.</summary>
    public class MagnifyEffect : ShaderEffect
    {
        string? AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(MagnifyEffect), 0);
        public static readonly DependencyProperty CenterPointProperty = DependencyProperty.Register("CenterPoint", typeof(Point), typeof(MagnifyEffect), new UIPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(MagnifyEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty MagnificationAmountProperty = DependencyProperty.Register("MagnificationAmount", typeof(double), typeof(MagnifyEffect), new UIPropertyMetadata(((double)(2D)), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(MagnifyEffect), new UIPropertyMetadata(((double)(1.5D)), PixelShaderConstantCallback(4)));
        public MagnifyEffect()
        {
            PixelShader = new PixelShader
            {
                UriSource = new Uri($"pack://application:,,,/{AssemblyName};component/CustomEffect/MagnifyEffect/MagnifyEffect.ps"),
            };

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(CenterPointProperty);
            this.UpdateShaderValue(RadiusProperty);
            this.UpdateShaderValue(MagnificationAmountProperty);
            this.UpdateShaderValue(AspectRatioProperty);
        }
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        /// <summary>The center point of the magnified region.</summary>
        public Point CenterPoint
        {
            get
            {
                return ((Point)(this.GetValue(CenterPointProperty)));
            }
            set
            {
                this.SetValue(CenterPointProperty, value);
            }
        }
        /// <summary>The radius of the magnified region.</summary>
        public double Radius
        {
            get
            {
                return ((double)(this.GetValue(RadiusProperty)));
            }
            set
            {
                this.SetValue(RadiusProperty, value);
            }
        }
        /// <summary>The magnification factor.</summary>
        public double MagnificationAmount
        {
            get
            {
                return ((double)(this.GetValue(MagnificationAmountProperty)));
            }
            set
            {
                this.SetValue(MagnificationAmountProperty, value);
            }
        }
        /// <summary>The aspect ratio (width / height) of the input.</summary>
        public double AspectRatio
        {
            get
            {
                return ((double)(this.GetValue(AspectRatioProperty)));
            }
            set
            {
                this.SetValue(AspectRatioProperty, value);
            }
        }
    }
}
