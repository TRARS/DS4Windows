namespace CustomMacroBase.GamePadState
{
    public sealed partial class DS4StateLite
    {
        public bool Square;//=> "Square";
        public bool Triangle;// => "Triangle";
        public bool Circle;// => "Circle";
        public bool Cross;// => "Cross";
        public bool DpadUp;// => "DpadUp";
        public bool DpadDown;// => "DpadDown";
        public bool DpadLeft;// => "DpadLeft";
        public bool DpadRight;// => "DpadRight";
        public bool Share;// => "Share";
        public bool Options;// => "Options";
        public bool TouchButton;// => "TouchButton";//监听真实手柄按下触摸板用这个
        public bool OutputTouchButton;//=> "OutputTouchButton";//操作虚拟手柄按下触摸板用这个
        public bool PS;// => "PS";
        public bool Mute;// => "Mute";
        public bool L1;// => "L1";
        public byte L2;// => "L2";
        public bool L3;// => "L3";
        public bool R1;// => "R1";
        public byte R2;// => "R2";
        public bool R3;// => "R3";
        public byte LX;// => "LX";
        public byte RX;// => "RX";
        public byte LY;// => "LY";
        public byte RY;// => "RY";

        public byte Touch0RawTrackingNum = 0;
        public byte Touch0Id = 0;
        public bool Touch0IsActive;
        public short[] Touch0 = { 0, 0 };

        public byte Touch1RawTrackingNum = 0;
        public byte Touch1Id = 0;
        public bool Touch1IsActive;
        public short[] Touch1 = { 0, 0 };
    }

    public sealed partial class DS4StateLite
    {
        public DS4StateLite()
        {
            this.LX = this.LY = 128;
            this.RX = this.RY = 128;
        }

        public void CopyTo(ref DS4StateLite dst)
        {
            dst.Square = Square;
            dst.Triangle = Triangle;
            dst.Circle = Circle;
            dst.Cross = Cross;
            dst.DpadUp = DpadUp;
            dst.DpadDown = DpadDown;
            dst.DpadLeft = DpadLeft;
            dst.DpadRight = DpadRight;
            dst.Share = Share;
            dst.Options = Options;
            dst.TouchButton = TouchButton;
            dst.OutputTouchButton = OutputTouchButton;
            dst.PS = PS;
            dst.Mute = Mute;
            dst.L1 = L1;
            dst.L2 = L2;
            dst.L3 = L3;
            dst.R1 = R1;
            dst.R2 = R2;
            dst.R3 = R3;
            dst.LX = LX;
            dst.RX = RX;
            dst.LY = LY;
            dst.RY = RY;

            dst.Touch0RawTrackingNum = Touch0RawTrackingNum;
            dst.Touch0Id = Touch0Id;
            dst.Touch0IsActive = Touch0IsActive;
            dst.Touch0 = new short[2] { Touch0[0], Touch0[1] };

            dst.Touch1RawTrackingNum = Touch1RawTrackingNum;
            dst.Touch1Id = Touch1Id;
            dst.Touch1IsActive = Touch1IsActive;
            dst.Touch1 = new short[2] { Touch1[0], Touch1[1] };
        }

        //public void Touch(short x, short y) 
        //{
        //    this.Touch0RawTrackingNum = 10;
        //    this.Touch0Id = 10;
        //    this.Touch0IsActive = true;
        //    this.Touch0 = new short[2] { x, y };
        //}

        public void Reset()
        {
            Square = Triangle = Circle = Cross = false;
            DpadUp = DpadDown = DpadLeft = DpadRight = false;
            Share = Options = TouchButton = OutputTouchButton = PS = Mute = false;
            L1 = L3 = R1 = R3 = false;
            L2 = R2 = 0;
            LX = RX = LY = RY = 128;

            Touch0RawTrackingNum = 0;
            Touch0Id = 0;
            Touch0IsActive = false;
            Touch0 = new short[2] { 0, 0 };

            Touch1RawTrackingNum = 0;
            Touch1Id = 0;
            Touch1IsActive = false;
            Touch1 = new short[2] { 0, 0 };
        }
    }
}
