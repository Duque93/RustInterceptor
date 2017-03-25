using System.Windows.Forms;
using static Rust_Interceptor.Forms.Structs.WindowStruct;

namespace Rust_Interceptor.Forms.Hooks
{
    interface IKeyEventsListener
    {
        bool onMouseMove(object sender, MOUSEINPUT data);

        bool onLeftMouseButtonDown(object sender, MOUSEINPUT data);
        bool onLeftMouseButtonUp(object sender, MOUSEINPUT data);

        bool onRightMouseButtonDown(object sender, MOUSEINPUT data);
        bool onRightMouseButtonUp(object sender, MOUSEINPUT data);

        bool onMiddleMouseButtonDown(object sender, MOUSEINPUT data);
        bool onMiddleMouseButtonUp(object sender, MOUSEINPUT data);

        bool onVerticalWheelMouseRotation(object sender, MOUSEINPUT data);
        bool onHorizontalWheelMouseRotation(object sender, MOUSEINPUT data);

        bool onKeyDown(object sender, KEYBDINPUT data, Keys key);
        bool onKeyUp(object sender, KEYBDINPUT data, Keys key);
    }
}
