using System.Runtime.InteropServices;
using System.Windows.Media;


namespace CustomMacroBase.Helper.ProConSimulate
{
    internal static partial class BTController
    {
        [DllImport(@".\btkeyLib.dll")]
        public static extern void send_button(uint key, uint waittime);

        [DllImport(@".\btkeyLib.dll")]
        public static extern void send_stick_r(uint h, uint v, uint waittime);

        [DllImport(@".\btkeyLib.dll")]
        public static extern void send_stick_l(uint h, uint v, uint waittime);




        [DllImport(@".\btkeyLib.dll")]
        public static extern void start_gamepad();

        [DllImport(@".\btkeyLib.dll", EntryPoint = "shutdown_gamepad")]
        public static extern void shutdown_gamepad();


        [DllImport(@".\btkeyLib.dll", EntryPoint = "send_padcolor")]
        private static extern void __send_padcolor(uint pad_color, uint button_color, uint leftgrip_color, uint rightgrip_color);
        public static void send_padcolor(Color pad_color, Color button_color, Color leftgrip_color, Color rightgrip_color)
        {
            uint pad_color2 = (uint)((int)pad_color.R | (int)pad_color.G << 8 | (int)pad_color.B << 16);
            uint button_color2 = (uint)((int)button_color.R | (int)button_color.G << 8 | (int)button_color.B << 16);
            uint leftgrip_color2 = (uint)((int)leftgrip_color.R | (int)leftgrip_color.G << 8 | (int)leftgrip_color.B << 16);
            uint rightgrip_color2 = (uint)((int)rightgrip_color.R | (int)rightgrip_color.G << 8 | (int)rightgrip_color.B << 16);
            BTController.__send_padcolor(pad_color2, button_color2, leftgrip_color2, rightgrip_color2);
        }
        public static void send_padcolor()
        {
            BTController.send_padcolor(
                (Color)ColorConverter.ConvertFromString("#FF3D3D3D"),
                (Color)ColorConverter.ConvertFromString("GhostWhite"),
                (Color)ColorConverter.ConvertFromString("#FF2D2D2D"),//#FF1FDE01
                (Color)ColorConverter.ConvertFromString("#FF2D2D2D")//#FFFF3378
            );
        }

        //[DllImport(@".\libwdi.dll")]
        //public static extern void DriverReplace(int vid, int pid);
    }

    internal static partial class BTController
    {
        [DllImport(@".\btkeylib.dll")]
        public static extern void send_amiibo(string filename);

        //[DllImport(@".\btkeylib.dll")]
        //public static extern bool get_rumble();
    }
}
