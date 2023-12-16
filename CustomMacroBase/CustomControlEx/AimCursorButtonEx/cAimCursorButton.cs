using CustomMacroBase.Helper;
using System;
using System.Text;
using System.Windows;

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
            base.PreviewMouseLeftButtonDown += (s, e) => { this.PreviewMouseLeftButtonDown?.Invoke(); };
            base.PreviewMouseLeftButtonUp += (s, e) => { this.PreviewMouseLeftButtonUp?.Invoke(); };
            base.PreviewMouseMove += (s, e) => { this.PreviewMouseMove?.Invoke(); };
            base.MouseLeave += (s, e) => { this.MouseLeave?.Invoke(); };

            PreviewMouseLeftButtonDown = () =>
            {
                isMouseLeftDown = true;
                PixelMatcher.AimCursor.Instance.SetWindowOpacity(true);
            };
            PreviewMouseLeftButtonUp = () =>
            {
                isMouseLeftDown = false;
                PixelMatcher.AimCursor.Instance.SetWindowOpacity(false);
                PixelMatcher.PixelMatcherHost.SetTargetWindowHandleEx(wHwnd, wTitle.ToString(), wClassName.ToString());
            };
            PreviewMouseMove = () =>
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
            MouseLeave = () =>
            {
                isMouseLeftDown = false;
                PixelMatcher.AimCursor.Instance.SetWindowOpacity(false);
            };
        }
    }

    public partial class cAimCursorButton
    {
        //AimCursor专用
        public new Action PreviewMouseLeftButtonDown
        {
            get { return (Action)GetValue(PreviewMouseLeftButtonDownProperty); }
            set { SetValue(PreviewMouseLeftButtonDownProperty, value); }
        }
        public static readonly DependencyProperty PreviewMouseLeftButtonDownProperty = DependencyProperty.Register(
            name: "PreviewMouseLeftButtonDown",
            propertyType: typeof(Action),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public new Action PreviewMouseLeftButtonUp
        {
            get { return (Action)GetValue(PreviewMouseLeftButtonUpProperty); }
            set { SetValue(PreviewMouseLeftButtonUpProperty, value); }
        }
        public static readonly DependencyProperty PreviewMouseLeftButtonUpProperty = DependencyProperty.Register(
            name: "PreviewMouseLeftButtonUp",
            propertyType: typeof(Action),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public new Action PreviewMouseMove
        {
            get { return (Action)GetValue(PreviewMouseMoveProperty); }
            set { SetValue(PreviewMouseMoveProperty, value); }
        }
        public static readonly DependencyProperty PreviewMouseMoveProperty = DependencyProperty.Register(
            name: "PreviewMouseMove",
            propertyType: typeof(Action),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public new Action MouseLeave
        {
            get { return (Action)GetValue(MouseLeaveProperty); }
            set { SetValue(MouseLeaveProperty, value); }
        }
        public static readonly DependencyProperty MouseLeaveProperty = DependencyProperty.Register(
            name: "MouseLeave",
            propertyType: typeof(Action),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
