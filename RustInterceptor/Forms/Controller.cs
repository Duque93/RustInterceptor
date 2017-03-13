using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rust_Interceptor.Forms
{
    public partial class Controller : Form
    {
        
        public static Controller getInstance()
        {
            if (instance == null) instance = new Controller();
            return instance;
        }
        private static Controller instance;
        private Controller()
        {
            InitializeComponent();
            
        }

        public delegate void voidCallback();
        public void mostrarse()
        {
            if (this.InvokeRequired) this.Invoke(new voidCallback(mostrarse));
            else
            {
                this.TopMost = true;
                this.Show();
            }
        }

        //Sobreescribe WndProc para permitir que el evento de despalzar el Form siga siendo posible
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private delegate int getIntValueCallback();
        public int getZoomValue()
        {
            if (this.InvokeRequired) return (int)this.Invoke(new getIntValueCallback(getZoomValue));
            else
            {
                return this.trackBarZoom.Value;
            }
            
        }
    }
}
