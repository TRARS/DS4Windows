﻿using CommunityToolkit.Mvvm.Messaging;
using CustomMacroBase.GamePadState;
using CustomMacroBase.Helper;
using CustomMacroBase.Messages;
using CustomMacroFactory.MVVM.Views;
using System;
using System.Linq;
using TrarsUI.Shared.Interfaces.UIComponents;

namespace DS4Windows
{
    public static partial class CustomMacroLink
    {
        private static MainWindow macroWindow = (MainWindow)CustomMacroFactory.MainEntry.GetRequiredService<IMainWindow>();

        public static void Init(dynamic ds4MainWindow, dynamic ds4ControlService) => macroWindow.Init(() => ds4MainWindow, () => ds4ControlService);
        public static void Exit() => macroWindow.Exit();
        public static void HideToTray() => macroWindow.HideToTray();
    }

    public static partial class CustomMacroLink
    {
        //Method for DS4StateLite
        private static void ConvertToLiteState(in DS4State cState, out DS4StateLite r, out DS4StateLite v)
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
        private static void ConvertToFullState(in DS4StateLite v, in DS4State cState)
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

        public static void Entry(in int ind, in DS4State currentState)
        {
            var mix = GamepadInputMixer.Instance.AllowMix();
            if ((mix && ind > 1) || (mix is false && ind != 0)) { return; }

            ConvertToLiteState(in currentState, out DS4StateLite realState, out DS4StateLite virtualState);
            macroWindow.MacroEntry(in ind, in realState, in virtualState);
            ConvertToFullState(in virtualState, in currentState);
        }
    }

    public static partial class CustomMacroLink
    {
        public static bool AllowOnce(string[] hardwareIds) => SingleDs4Accessor.Instance.AllowOnce(hardwareIds);
        public static void Print(string str) => WeakReferenceMessenger.Default.Send(new PrintNewMessage(str));
        public static bool HasAnyZero<T>(string msg, T xData, T yData)
        {
            var valuesListX = typeof(T).GetFields().Select(field => Convert.ToUInt16(field.GetValue(xData))).ToList();
            var valuesListY = typeof(T).GetFields().Select(field => Convert.ToUInt16(field.GetValue(yData))).ToList();

            var hasAnyZero = valuesListX.Any(item => item == 0) || valuesListY.Any(item => item == 0);
            var dataInfo = $"xData: {string.Join(",", valuesListX)} --- yData: {string.Join(",", valuesListY)}";

            if (hasAnyZero)
            {
                Print($"{msg}_need_reload -> {dataInfo}");
            }
            else
            {
                Print($"{msg} -> {dataInfo}");
            }
            return hasAnyZero;
        }
        public static bool ReloadStickCalib(byte subcommand, ReadOnlySpan<byte> tmpBuffer, ReadOnlySpan<byte> tmpReport, ref int retryLimit)
        {
            if (subcommand != 0x10) { return false; }
            if (retryLimit-- <= 0) { return false; }

            if (tmpBuffer.SequenceEqual<byte>([0x3D, 0x60, 0x00, 0x00, 0x09]) || // LEFT  STICK FACTORY CALIB
                tmpBuffer.SequenceEqual<byte>([0x46, 0x60, 0x00, 0x00, 0x09]) || // RIGHT STICK FACTORY CALIB
                tmpBuffer.SequenceEqual<byte>([0x12, 0x80, 0x00, 0x00, 0x09]) || // LEFT  STICK USER    CALIB
                tmpBuffer.SequenceEqual<byte>([0x1D, 0x80, 0x00, 0x00, 0x09]))   // RIGHT STICK USER    CALIB
            {
                var SPI_RESP_OFFSET = 20;
                var stickCalib = new ushort[6];
                stickCalib[0] = (ushort)(((tmpReport[1 + SPI_RESP_OFFSET] << 8) & 0xF00) | tmpReport[0 + SPI_RESP_OFFSET]); // X Axis Max above center
                stickCalib[1] = (ushort)((tmpReport[2 + SPI_RESP_OFFSET] << 4) | (tmpReport[1 + SPI_RESP_OFFSET] >> 4)); // Y Axis Max above center
                stickCalib[2] = (ushort)(((tmpReport[4 + SPI_RESP_OFFSET] << 8) & 0xF00) | tmpReport[3 + SPI_RESP_OFFSET]); // X Axis Center
                stickCalib[3] = (ushort)((tmpReport[5 + SPI_RESP_OFFSET] << 4) | (tmpReport[4 + SPI_RESP_OFFSET] >> 4)); // Y Axis Center
                stickCalib[4] = (ushort)(((tmpReport[7 + SPI_RESP_OFFSET] << 8) & 0xF00) | tmpReport[6 + SPI_RESP_OFFSET]); // X Axis Min below center
                stickCalib[5] = (ushort)((tmpReport[8 + SPI_RESP_OFFSET] << 4) | (tmpReport[7 + SPI_RESP_OFFSET] >> 4)); // Y Axis Min below center

                //Print($"retryLimit: {retryLimit}");
                return stickCalib.Any(item => item == 0);
            }
            else
            {
                return false;
            }
        }
    }
}
