using CustomMacroBase;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.Attributes;
using CustomMacroBase.Helper.Tools.OtherManager;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CustomMacroFactory.MacroFactory.InnerMacro
{
    partial class Settings_0_AnalogStick
    {
        class InnerModel
        {
            public ObservableCollection<double> DoubleList = new(Enumerable.Repeat(0d, 20));
        }

        class InnerViewModel : NotificationObject
        {
            InnerModel model = new();

            public double LeftStickDeadZone
            {
                get => model.DoubleList[0];
                set
                {
                    if (model.DoubleList[0] != value)
                    {
                        model.DoubleList[0] = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
            public double LeftStickEnlargementFactor
            {
                get => model.DoubleList[1];
                set
                {
                    if (model.DoubleList[1] != value)
                    {
                        model.DoubleList[1] = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
            public double LeftStickClipRadius
            {
                get => model.DoubleList[2];
                set
                {
                    if (model.DoubleList[2] != value)
                    {
                        model.DoubleList[2] = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }

            public double RightStickDeadZone
            {
                get => model.DoubleList[3];
                set
                {
                    if (model.DoubleList[3] != value)
                    {
                        model.DoubleList[3] = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
            public double RightStickEnlargementFactor
            {
                get => model.DoubleList[4];
                set
                {
                    if (model.DoubleList[4] != value)
                    {
                        model.DoubleList[4] = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
            public double RightStickClipRadius
            {
                get => model.DoubleList[5];
                set
                {
                    if (model.DoubleList[5] != value)
                    {
                        model.DoubleList[5] = Math.Floor(value);
                        NotifyPropertyChanged();
                    }
                }
            }
        }

        static InnerViewModel viewmodel = new();
    }

    [SortIndex(0)]
    partial class Settings_0_AnalogStick : MacroBase
    {
        public override void Init()
        {
            MainGate.Text = "Analog Stick Setting";
            MainGate.Enable = false;

            MainGate.Add(CreateGateBase("LeftStick"));//[0]
            {
                MainGate[0].AddEx(() => CreateSlider2(0, 127, viewmodel, nameof(viewmodel.LeftStickDeadZone), 1, "DeadZone", defalutValue: 0));
                MainGate[0].AddEx(() => CreateSlider2(128, 1280, viewmodel, nameof(viewmodel.LeftStickEnlargementFactor), 1, "EnlargementFactor", defalutValue: 1280));
                MainGate[0].AddEx(() => CreateSlider2(64, 180, viewmodel, nameof(viewmodel.LeftStickClipRadius), 1, "ClipRadius", defalutValue: 128));
            }

            MainGate.Add(CreateGateBase("RightStick"));//[1]
            {
                MainGate[1].AddEx(() => CreateSlider2(0, 127, viewmodel, nameof(viewmodel.RightStickDeadZone), 1, "DeadZone", defalutValue: 0));
                MainGate[1].AddEx(() => CreateSlider2(128, 1280, viewmodel, nameof(viewmodel.RightStickEnlargementFactor), 1, "EnlargementFactor", defalutValue: 1280));
                MainGate[1].AddEx(() => CreateSlider2(64, 180, viewmodel, nameof(viewmodel.RightStickClipRadius), 1, "ClipRadius", defalutValue: 128));
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

    partial class Settings_0_AnalogStick
    {
        StickController SM = new(new(0, 1280), new(0, 1280));

        private void LeftStickFix(bool canExecute)
        {
            if (canExecute is false)
            {
                SM.SetLeft(double.NaN, double.NaN, double.NaN); return;
            }

            var deadzone = viewmodel.LeftStickDeadZone;
            var factor = viewmodel.LeftStickEnlargementFactor;
            var clip = viewmodel.LeftStickClipRadius;

            SM.SetLeft(deadzone, factor, clip);
            SM.ExecuteAction(VirtualDS4);
        }
        private void RightStickFix(bool canExecute)
        {
            if (canExecute is false)
            {
                SM.SetRight(double.NaN, double.NaN, double.NaN); return;
            }

            var deadzone = viewmodel.RightStickDeadZone;
            var factor = viewmodel.RightStickEnlargementFactor;
            var clip = viewmodel.RightStickClipRadius;

            SM.SetRight(deadzone, factor, clip);
            SM.ExecuteAction(VirtualDS4);
        }
    }
}
