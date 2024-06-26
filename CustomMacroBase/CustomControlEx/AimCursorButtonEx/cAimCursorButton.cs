using CustomMacroBase.Helper;
using System;
using System.Text;

namespace CustomMacroBase.CustomControlEx.VerticalButtonEx
{
    public partial class cAimCursorButton : cVerticalButton
    {
        IntPtr wHwnd = IntPtr.Zero;
        StringBuilder wTitle = new StringBuilder(256);
        StringBuilder wClassName = new StringBuilder(256);
        bool isMouseLeftDown = false;
        System.Drawing.Point mousePoint => System.Windows.Forms.Control.MousePosition;

        public cAimCursorButton()
        {
            base.PreviewMouseLeftButtonDown += (s, e) =>
            {
                isMouseLeftDown = true;
                PixelMatcher.AimCursor.Instance.SetWindowOpacity(true);
            };
            base.PreviewMouseLeftButtonUp += (s, e) =>
            {
                isMouseLeftDown = false;
                PixelMatcher.AimCursor.Instance.SetWindowOpacity(false);
                PixelMatcher.PixelMatcherHost.SetTargetWindowHandleEx(wHwnd, wTitle.ToString(), wClassName.ToString());
            };
            base.PreviewMouseMove += (s, e) =>
            {
                if (isMouseLeftDown)
                {
                    //移动准星
                    PixelMatcher.AimCursor.Instance.MoveTo(mousePoint);

                    //获取准星指向窗体句柄进而获取更多信息
                    IntPtr formHandle = User32.WindowFromPoint(mousePoint);
                    if (wHwnd != formHandle)
                    {
                        wHwnd = formHandle;
                        User32.GetWindowText(wHwnd, wTitle, wTitle.Capacity);
                        User32.GetClassName(wHwnd, wClassName, wClassName.Capacity);

                        string msg = $"{wHwnd:x8}\"{wTitle}\"{wClassName}";
                        Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, msg);
                    }
                }
                else { PixelMatcher.AimCursor.Instance.MoveTo(mousePoint); }
            };
            base.MouseLeave += (s, e) =>
            {
                isMouseLeftDown = false;
                PixelMatcher.AimCursor.Instance.SetWindowOpacity(false);
            };
        }
    }
}
