using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static Rust_Interceptor.Forms.Structs.WindowStruct;

namespace Rust_Interceptor.Forms.Hooks
{
    class WindowHook
    {
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECTANGULO lpRect);

        /// <summary>
        /// Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window.
        /// </param>
        /// <param name="lpwndpl">
        /// A pointer to the WINDOWPLACEMENT structure that receives the show state and position information.
        /// <para>
        /// Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fails if lpwndpl-> length is not set correctly.
        /// </para>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// <para>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </para>
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        private IntPtr externalWindowHwnd;
        private Form windowForm;

        private Size lastSize;
        private Thread chivato;


        public WindowHook(IntPtr externalWindowHwnd, Overlay windowForm)
        {

            this.externalWindowHwnd = externalWindowHwnd;
            this.windowForm = windowForm;

            chivato = new Thread(
                () =>
                {
                    while (windowForm.working)
                    {
                        RECTANGULO rectangulo = new RECTANGULO();
                        GetWindowRect(externalWindowHwnd, out rectangulo);
                        windowForm.resizeForm(rectangulo);
                        Thread.Sleep(250);
                    }
                });

            chivato.SetApartmentState(ApartmentState.MTA);
            chivato.IsBackground = true;
            chivato.CurrentCulture = System.Globalization.CultureInfo.CurrentCulture;
            chivato.Priority = ThreadPriority.Highest;
            chivato.Name = "WindowHookWorkerThread";

            chivato.Start();
            //chivato.Join();
        }
        
    }
}
