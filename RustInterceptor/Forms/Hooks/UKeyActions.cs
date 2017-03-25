using Rust_Interceptor.Forms.Structs;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Rust_Interceptor.Forms.Structs.WindowStruct;

namespace Rust_Interceptor.Forms.Hooks
{
    internal class UKeyActions
    {
        private const int TIPO_MOUSEVENT = 0;
        
        public bool moveMouseToPoint(POINT target, bool absolute = true)
        {
            MouseFlags flag = absolute ? MouseFlags.MOUSE_MOVE_ABSOLUTE : MouseFlags.MOUSEEVENTF_MOVE;
            INPUT inputEvent = createInput(target, flag, true);
            if (SendInput(1, ref inputEvent, Marshal.SizeOf(typeof(INPUT))) == 0)
            {
                MessageBox.Show("Ha fallado el remplazo del evento en MouseHook. Codigo error -->" + Marshal.GetLastWin32Error());
            }
            return false;
        }

        public INPUT createInput(POINT pos , MouseFlags flag, bool automated = false)
        {
            INPUT mouseEvent = new INPUT() { };
            mouseEvent.tipo = TIPO_MOUSEVENT;
            //Si pongo de la manera directa sin MouseFlags.MOUSEEVENTF_ABSOLUTE en el parametro de flag, resulta que simplemente suma a la posición actual del raton.
            //La manera correcta es la siguiente
            mouseEvent.data.mouse.dx = flag == MouseFlags.MOUSE_MOVE_ABSOLUTE ? Convert.ToInt32(pos.X ) : Convert.ToInt32(pos.X * (65536.0f / SystemInfo.getSystemInfo(SystemInfo.SystemMetric.SM_CXSCREEN)));
            mouseEvent.data.mouse.dy = flag == MouseFlags.MOUSE_MOVE_ABSOLUTE ? Convert.ToInt32(pos.Y ) : Convert.ToInt32(pos.Y * (65536.0f / SystemInfo.getSystemInfo(SystemInfo.SystemMetric.SM_CYSCREEN)));
            mouseEvent.data.mouse.mouseData = 0;
            mouseEvent.data.mouse.time = 0;
            mouseEvent.data.mouse.dwExtraInfo = automated  ? new IntPtr(-1) : SystemInfo.GetMessageExtraInfo();  //Coloco esto como flag para saber cuando es automatizado //mouseEvent.data.mouse.dwExtraInfo = ControllerSystemInfo.GetMessageExtraInfo();
            mouseEvent.data.mouse.dwFlags = (uint)flag;

            return mouseEvent;
        }
        //MAS INFO --> http://stackoverflow.com/questions/12761169/send-keys-through-sendinput-in-user32-dll
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

    }
}
