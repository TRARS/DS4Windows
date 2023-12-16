using CustomMacroBase.GamePadState;
using CustomMacroBase.Helper;

namespace DS4Windows
{
    public static partial class CustomMacroLink//
    {
        public static DS4WinWPF.DS4Forms.MainWindow Ds4_MainWindow_Instance;
        public static DS4Windows.ControlService Ds4_ControlService_Instance;
        public static CustomMacroFactory.MainEntry CustomMacroFactoryInstance;

        static CustomMacroLink()
        {
            CustomMacroFactoryInstance = new()
            {
                Ds4Host = () => Ds4_MainWindow_Instance,
                RootHub = () => Ds4_ControlService_Instance
            };
        }
    }

    public static partial class CustomMacroLink
    {
        //Method for DS4StateLite
        private static void StateConverter(in DS4State cState, out DS4StateLite r, out DS4StateLite v)
        {
            r = new();
            v = new();

            r.Square = v.Square = cState.Square;
            r.Triangle = v.Triangle = cState.Triangle;
            r.Circle = v.Circle = cState.Circle;
            r.Cross = v.Cross = cState.Cross;
            r.DpadUp = v.DpadUp = cState.DpadUp;
            r.DpadDown = v.DpadDown = cState.DpadDown;
            r.DpadLeft = v.DpadLeft = cState.DpadLeft;
            r.DpadRight = v.DpadRight = cState.DpadRight;
            r.Share = v.Share = cState.Share;
            r.Options = v.Options = cState.Options;
            r.TouchButton = v.TouchButton = cState.TouchButton;
            r.OutputTouchButton = v.OutputTouchButton = cState.OutputTouchButton;
            r.PS = v.PS = cState.PS;
            r.Mute = v.Mute = cState.Mute;
            r.L1 = v.L1 = cState.L1;
            r.L2 = v.L2 = cState.L2;
            r.L3 = v.L3 = cState.L3;
            r.R1 = v.R1 = cState.R1;
            r.R2 = v.R2 = cState.R2;
            r.R3 = v.R3 = cState.R3;
            r.LX = v.LX = cState.LX;
            r.RX = v.RX = cState.RX;
            r.LY = v.LY = cState.LY;
            r.RY = v.RY = cState.RY;

            r.Touch0RawTrackingNum = v.Touch0RawTrackingNum = cState.TrackPadTouch0.RawTrackingNum;
            r.Touch0Id = v.Touch0Id = cState.TrackPadTouch0.Id;
            r.Touch0IsActive = v.Touch0IsActive = cState.TrackPadTouch0.IsActive;
            r.Touch0 = v.Touch0 = new short[2] { cState.TrackPadTouch0.X, cState.TrackPadTouch0.Y };

            r.Touch1RawTrackingNum = v.Touch1RawTrackingNum = cState.TrackPadTouch1.RawTrackingNum;
            r.Touch1Id = v.Touch1Id = cState.TrackPadTouch1.Id;
            r.Touch1IsActive = v.Touch1IsActive = cState.TrackPadTouch1.IsActive;
            r.Touch1 = v.Touch1 = new short[2] { cState.TrackPadTouch1.X, cState.TrackPadTouch1.Y };
        }
        private static void StateConverter(in DS4StateLite v, in DS4State cState)
        {
            cState.Square = v.Square;
            cState.Triangle = v.Triangle;
            cState.Circle = v.Circle;
            cState.Cross = v.Cross;
            cState.DpadUp = v.DpadUp;
            cState.DpadDown = v.DpadDown;
            cState.DpadLeft = v.DpadLeft;
            cState.DpadRight = v.DpadRight;
            cState.Share = v.Share;
            cState.Options = v.Options;
            cState.TouchButton = v.TouchButton;
            cState.OutputTouchButton = v.OutputTouchButton;
            cState.PS = v.PS;
            cState.Mute = v.Mute;
            cState.L1 = v.L1;
            cState.L2 = v.L2;
            cState.L3 = v.L3;
            cState.R1 = v.R1;
            cState.R2 = v.R2;
            cState.R3 = v.R3;
            cState.LX = v.LX;
            cState.RX = v.RX;
            cState.LY = v.LY;
            cState.RY = v.RY;

            cState.TrackPadTouch0.RawTrackingNum = v.Touch0RawTrackingNum;
            cState.TrackPadTouch0.Id = v.Touch0Id;
            cState.TrackPadTouch0.IsActive = v.Touch0IsActive;
            cState.TrackPadTouch0.X = v.Touch0[0];
            cState.TrackPadTouch0.Y = v.Touch0[1];

            cState.TrackPadTouch1.RawTrackingNum = v.Touch1RawTrackingNum;
            cState.TrackPadTouch1.Id = v.Touch1Id;
            cState.TrackPadTouch1.IsActive = v.Touch1IsActive;
            cState.TrackPadTouch1.X = v.Touch1[0];
            cState.TrackPadTouch1.Y = v.Touch1[1];

            //
            cState.L2Btn = v.L2 > 0;
            cState.R2Btn = v.R2 > 0;
        }

        public static void Entry(in DS4State currentState)
        {
            StateConverter(in currentState, out DS4StateLite realState, out DS4StateLite virtualState);
            CustomMacroFactoryInstance.MainView.DS4StateCustomUpdateMain(in realState, in virtualState);
            StateConverter(in virtualState, in currentState);
        }
    }

    public static partial class CustomMacroLink
    {
        public static bool AllowOnce(string[] hardwareIds) => SingleDs4Accessor.Instance.AllowOnce(hardwareIds);
    }
}
