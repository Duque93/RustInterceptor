

using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static Rust_Interceptor.Forms.Structs.WindowStruct;

namespace Rust_Interceptor.Forms.Hooks
{
    class UKeyHook
    {

        private static UKeyHook instance;
        public static UKeyHook getInstance(IKeyEventsListener escuchador)
        {
            if (instance == null) instance = new UKeyHook(escuchador);
            return instance;
        }

        private IKeyEventsListener escuchador;
        public UKeyActions action
        {
            get;
            private set;
        }

        private Thread chivato;
        private ConcurrentDictionary<Keys, byte> keysDown;
        public bool working {
            get;
            private set;
            } = false;

        private uint TIPO_MOUSEVENT = 0;
        public POINT currentPos
        {
            get;
            private set;
        }


        private UKeyHook(IKeyEventsListener escuchador)
        {
            this.escuchador = escuchador;
            this.action = new UKeyActions();
            init();
        }
        public void start()
        {
            if (working) throw new Exception("El hook ya ha sido inicializado");
            working = true;
            chivato.Start();
            //chivato.Join();
        }
        public void stop()
        {
            working = false;

            if (chivato.IsAlive)
            {
                Thread.Sleep(1000);
                if (chivato.IsAlive) chivato.Abort();
            }
        }
        private void init()
        {
            keysDown = new ConcurrentDictionary<Keys, byte>();
            chivato = new Thread(
                () =>
                {
                    do
                    {
                        Thread.Sleep(1);
                        POINT oldCurrentPos = currentPos;
                        //GetCursorPos(out currentPos);
                        currentPos = System.Windows.Forms.Cursor.Position;
                        if (!oldCurrentPos.Equals(currentPos))
                        {
                            INPUT input = action.createInput(currentPos, MouseFlags.MOUSEEVENTF_MOVE);
                            this.createThreadEvent(this.escuchador.onMouseMove, this, input.data.mouse).Start();
                            Console.WriteLine(currentPos);
                        }

                        byte bit = new byte();

                        if (GetAsyncKeyState(Keys.LButton) != 0) //PRESSED
                        {
                            if (keysDown.TryAdd(Keys.LButton, bit))
                            {
                                INPUT input = action.createInput(currentPos, MouseFlags.MOUSEEVENTF_LEFTDOWN);
                                this.createThreadEvent(this.escuchador.onLeftMouseButtonDown, this, input.data.mouse).Start();
                            }
                        }
                        else if (keysDown.TryRemove(Keys.LButton, out bit))//NOT PRESED
                        {
                            INPUT input = action.createInput(currentPos, MouseFlags.MOUSEEVENTF_LEFTUP);
                            this.createThreadEvent(this.escuchador.onLeftMouseButtonUp, this, input.data.mouse).Start();
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////
                        if (GetAsyncKeyState(Keys.RButton) != 0) //PRESSED
                        {
                            if(keysDown.TryAdd(Keys.RButton, bit))
                            {
                                INPUT input = action.createInput(currentPos, MouseFlags.MOUSEEVENTF_RIGHTDOWN);
                                this.createThreadEvent(this.escuchador.onRightMouseButtonDown, this, input.data.mouse).Start();
                            }

                        }
                        else if (keysDown.TryRemove(Keys.RButton, out bit))//NOT PRESSED
                        {
                            INPUT input = action.createInput(currentPos, MouseFlags.MOUSEEVENTF_RIGHTUP);
                            this.createThreadEvent(this.escuchador.onRightMouseButtonUp, this, input.data.mouse).Start();
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////
                        if (GetAsyncKeyState(Keys.MButton) != 0) //PRESSED
                        {
                            if (keysDown.TryAdd(Keys.MButton, bit))
                            {
                                INPUT input = action.createInput(currentPos, MouseFlags.MOUSEEVENTF_MIDDLEDOWN);
                                this.createThreadEvent(this.escuchador.onMiddleMouseButtonDown, this, input.data.mouse).Start();
                            }
                        }
                        else if (keysDown.TryRemove(Keys.MButton, out bit)) //NOT PRESSED
                        {
                            INPUT input = action.createInput(currentPos, MouseFlags.MOUSEEVENTF_MIDDLEUP);
                            this.createThreadEvent(this.escuchador.onMiddleMouseButtonUp, this, input.data.mouse).Start();
                        }

                        //////////////////////////////////////////////////////////////////////////////////////////////

                    } while (working);

                }
                );
            chivato.SetApartmentState(ApartmentState.MTA);
            chivato.IsBackground = true;
            chivato.CurrentCulture = System.Globalization.CultureInfo.CurrentCulture;
            chivato.Priority = ThreadPriority.Highest;
            chivato.Name = "UMouseWorkerThread";


        }
        
        private delegate bool eventCallback(object sender, MOUSEINPUT data);
        private Thread createThreadEvent(eventCallback callback, object sender, MOUSEINPUT data)
        {
            Thread hilo = new Thread(
                () =>
                {
                    callback.Invoke(sender, data);
                }
                );

            hilo.SetApartmentState(ApartmentState.MTA);
            hilo.IsBackground = true;
            hilo.CurrentCulture = System.Globalization.CultureInfo.CurrentCulture;
            hilo.Priority = ThreadPriority.Normal;
            hilo.Name = callback.ToString()+"Thread";

            return hilo;
        }
        
       
        /// <summary>
        /// Deuvelvo 0 si no esta presionada, 1 si lo esta
        /// </summary>
        /// <param name="vKey"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        /// <summary>
        /// DON'T use System.Drawing.Point, the order of the fields in System.Drawing.Point isn't guaranteed to stay the same.
        /// </summary>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);
    }
}
