using Rust_Interceptor.Forms.Structs;
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

            int maxX = SystemInfo.getSystemInfo(SystemInfo.SystemMetric.SM_CXFULLSCREEN)/2;
            int maxY = SystemInfo.getSystemInfo(SystemInfo.SystemMetric.SM_CYFULLSCREEN)/2;

            this.trackBarXCrosshairOffset.Minimum = maxX * -1;
            this.trackBarXCrosshairOffset.Maximum = maxX;

            this.trackBarYCrosshairOffset.Minimum = maxY *-1;
            this.trackBarYCrosshairOffset.Maximum = maxY;

            this.buttonCerrar.Click += new EventHandler(
                (object sender, EventArgs e) =>
                {
                    Overlay.getInstance(null).Stop();
                    this.Close();
                });

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
            if (this.IsDisposed) return 0;
            if (this.InvokeRequired) return (int)this.Invoke(new getIntValueCallback(getZoomValue));
            else
            {
                return this.trackBarZoom.Value;
            }
            
        }
        public int getAngleValue()
        {
            if (this.IsDisposed) return 0;
            if (this.InvokeRequired) return (int)this.Invoke(new getIntValueCallback(getAngleValue));
            else
            {
                return this.trackBarAngle.Value;
            }

        }
        public int getXCrosshairOffsetValue()
        {
            if (this.IsDisposed) return 0;
            if (this.InvokeRequired) return (int)this.Invoke(new getIntValueCallback(getXCrosshairOffsetValue));
            else
            {
                return this.trackBarXCrosshairOffset.Value;
            }

        }
        public int getYCrosshairOffsetValue()
        {
            if (this.IsDisposed) return 0;
            if (this.InvokeRequired) return (int)this.Invoke(new getIntValueCallback(getYCrosshairOffsetValue));
            else
            {
                return this.trackBarYCrosshairOffset.Value;
            }

        }


        private void trackBarZoom_ValueChanged(object sender, EventArgs e)
        {
            this.labelZoomValue.Text = this.trackBarZoom.Value.ToString()+" m";
        }

        private void trackBarAngle_ValueChanged(object sender, EventArgs e)
        {
            this.labelAngle.Text = this.trackBarAngle.Value.ToString() + " º";
        }

        private void trackBarYCrosshairOffset_ValueChanged(object sender, EventArgs e)
        {
            this.labelYOffset.Text = this.trackBarYCrosshairOffset.Value.ToString() + " px";
        }

        private void trackBarXCrosshairOffset_ValueChanged(object sender, EventArgs e)
        {
            this.labelXOffset.Text = this.trackBarXCrosshairOffset.Value.ToString() + " px";
        }
    }
}
