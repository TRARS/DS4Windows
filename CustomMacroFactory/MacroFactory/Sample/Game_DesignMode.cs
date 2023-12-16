using CustomMacroBase;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomMacroFactory.MacroFactory.Sample
{
    partial class Game_DesignMode : MacroBase
    {
        public override void Init()
        {
            MainGate.Text = "CustomMacroFactory/MacroFactory/Sample/Game_DesignMode.cs";

            MainGate.Add(CreateGateBase("[0]"));
            MainGate[0].Add(CreateGateBase("[0][0]"));
            MainGate[0].AddEx(() => CreateTextBlock("- This sample is applicable only to design mode.", Colors.Crimson));

            MainGate.Add(CreateGateBase("[1]"));
            MainGate[1].Add(CreateGateBase("[1][0]"));
            MainGate[1].Add(CreateGateBase("[1][1]"));
            MainGate[1].AddEx(() => CreateTextBlock("- This sample is applicable only to design mode.", Colors.LawnGreen));

            MainGate.AddEx(() => CreateTextBlock("- This sample is applicable only to design mode.", Colors.DodgerBlue));
        }

        public override void UpdateState()
        {
            //throw new NotImplementedException();
        }

        private TextBlock CreateTextBlock(string str, Color color)
        {
            return new TextBlock()
            {
                Text = $"{str}",
                Foreground = new SolidColorBrush(color),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
            };
        }
    }
}
