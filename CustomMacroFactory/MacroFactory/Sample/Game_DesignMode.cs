using CustomMacroBase;
using System.Windows.Media;

namespace CustomMacroFactory.MacroFactory.Sample
{
    partial class Game_DesignMode : MacroBase
    {
        public override void Init()
        {
            this.UseColorfulText = true;

            MainGate.Text = "CustomMacroFactory/MacroFactory/Sample/Game_DesignMode.cs";

            MainGate.Add(CreateTVN("[0]"));
            MainGate[0].AddEx(() => CreateTextBlock("- This sample is applicable only to design mode.", Colors.Crimson));
            MainGate[0].Add(CreateTVN("[0][0]"));
            MainGate[0][0].Add(CreateTVN("[0][0][0]"));
            MainGate[0][0][0].Add(CreateTVN("[0][0][0][0]"));

            MainGate.Add(CreateTVN("[1]"));
            MainGate[1].AddEx(() => CreateTextBlock("- This sample is applicable only to design mode.", Colors.LawnGreen));
            MainGate[1].Add(CreateTVN("[1][0]"));
            MainGate[1].Add(CreateTVN("[1][1]"));

            MainGate.AddEx(() => CreateTextBlock("- This sample is applicable only to design mode.", Colors.DodgerBlue));
        }

        public override void UpdateState()
        {
            //throw new NotImplementedException();
        }
    }
}
