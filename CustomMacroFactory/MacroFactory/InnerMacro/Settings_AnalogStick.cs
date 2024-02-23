using CustomMacroBase;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.Tools.OtherManager;
using CustomMacroBase.PreBase;
using System;

namespace CustomMacroFactory.MacroFactory.InnerMacro
{
    partial class Settings_AnalogStick
    {
        class InnerModel : NotificationObject
        {
            //
            private double _LeftStickDeadZone = 0;
            public double LeftStickDeadZone
            {
                get => _LeftStickDeadZone;
                set
                {
                    if (_LeftStickDeadZone != value)
                    {
                        _LeftStickDeadZone = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
            private double _LeftStickEnlargementFactor = 0;
            public double LeftStickEnlargementFactor
            {
                get => _LeftStickEnlargementFactor;
                set
                {
                    if (_LeftStickEnlargementFactor != value)
                    {
                        _LeftStickEnlargementFactor = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
            private double _LeftStickClipRadius = 0;
            public double LeftStickClipRadius
            {
                get => _LeftStickClipRadius;
                set
                {
                    if (_LeftStickClipRadius != value)
                    {
                        _LeftStickClipRadius = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }

            //
            private double _RightStickDeadZone = 0;
            public double RightStickDeadZone
            {
                get => _RightStickDeadZone;
                set
                {
                    if (_RightStickDeadZone != value)
                    {
                        _RightStickDeadZone = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
            private double _RightStickEnlargementFactor = 0;
            public double RightStickEnlargementFactor
            {
                get => _RightStickEnlargementFactor;
                set
                {
                    if (_RightStickEnlargementFactor != value)
                    {
                        _RightStickEnlargementFactor = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
            private double _RightStickClipRadius = 0;
            public double RightStickClipRadius
            {
                get => _RightStickClipRadius;
                set
                {
                    if (_RightStickClipRadius != value)
                    {
                        _RightStickClipRadius = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
        }

        InnerModel model = new();
    }

    partial class Settings_AnalogStick : MacroBase
    {
        public override void Init()
        {
            MainGate.Text = "Analog Stick Setting";
            MainGate.Enable = false;

            MainGate.Add(CreateGateBase("LeftStick"));//[0]
            {
                MainGate[0].AddEx(() => CreateSlider2(0, 127, model, nameof(model.LeftStickDeadZone), 1, "DeadZone", defalutValue: 0));
                MainGate[0].AddEx(() => CreateSlider2(128, 1280, model, nameof(model.LeftStickEnlargementFactor), 1, "EnlargementFactor", defalutValue: 1280));
                MainGate[0].AddEx(() => CreateSlider2(64, 180, model, nameof(model.LeftStickClipRadius), 1, "ClipRadius", defalutValue: 128));
            }

            MainGate.Add(CreateGateBase("RightStick"));//[1]
            {
                MainGate[1].AddEx(() => CreateSlider2(0, 127, model, nameof(model.RightStickDeadZone), 1, "DeadZone", defalutValue: 0));
                MainGate[1].AddEx(() => CreateSlider2(128, 1280, model, nameof(model.RightStickEnlargementFactor), 1, "EnlargementFactor", defalutValue: 1280));
                MainGate[1].AddEx(() => CreateSlider2(64, 180, model, nameof(model.RightStickClipRadius), 1, "ClipRadius", defalutValue: 128));
            }
        }

        public override void UpdateState()
        {
            if (MainGate.Enable)
            {
                LeftStickFix(MainGate[0].Enable);
                RightStickFix(MainGate[1].Enable);

                var realState = RealDS4;
                var virtualState = VirtualDS4;
                virtualState.CopyTo(ref realState);//
            }
        }
    }

    partial class Settings_AnalogStick
    {
        StickController SM = new(new(0, 1280), new(0, 1280));

        private void LeftStickFix(bool canExecute)
        {
            if (canExecute is false)
            {
                SM.SetLeft(double.NaN, double.NaN, double.NaN); return;
            }

            var deadzone = model.LeftStickDeadZone;
            var factor = model.LeftStickEnlargementFactor;
            var clip = model.LeftStickClipRadius;

            SM.SetLeft(deadzone, factor, clip);
            SM.ExecuteAction(VirtualDS4);
        }
        private void RightStickFix(bool canExecute)
        {
            if (canExecute is false) 
            {
                SM.SetRight(double.NaN, double.NaN, double.NaN); return;
            }

            var deadzone = model.RightStickDeadZone;
            var factor = model.RightStickEnlargementFactor;
            var clip = model.RightStickClipRadius;

            SM.SetRight(deadzone, factor, clip);
            SM.ExecuteAction(VirtualDS4);
        }
    }
}
