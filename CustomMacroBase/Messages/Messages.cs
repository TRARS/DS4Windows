using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Drawing;

namespace CustomMacroBase.Messages
{
    public class AsyncSnapshotMessage : AsyncRequestMessage<bool>
    {
        public Bitmap Bitmap;

        public AsyncSnapshotMessage(Bitmap bmp)
        {
            Bitmap = bmp;
        }
    }

    public class Ds4Disconnect : ValueChangedMessage<string>
    {
        public Ds4Disconnect(string value) : base(value)
        {
        }
    }

    public class Ds4Rumble : ValueChangedMessage<bool>
    {
        public Ds4Rumble(bool value) : base(value)
        {
        }
    }

    public class Ds4Latency : ValueChangedMessage<double[]>
    {
        public Ds4Latency(double[] value) : base(value)
        {
        }
    }

    public class PrintNewMessage : ValueChangedMessage<string>
    {
        public PrintNewMessage(string value) : base(value)
        {
        }
    }

    public class PrintCleanup : ValueChangedMessage<string>
    {
        public PrintCleanup(string value) : base(value)
        {
        }
    }

    public class CanUpdateFrames : ValueChangedMessage<bool>
    {
        public CanUpdateFrames(bool value) : base(value)
        {
        }
    }
}
