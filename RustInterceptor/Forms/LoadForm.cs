using Rust_Interceptor.Data;
using Rust_Interceptor.Forms.Hooks;
using Rust_Interceptor.Forms.Structs;
using Rust_Interceptor.Forms.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static Rust_Interceptor.Forms.Structs.WindowStruct;

namespace Rust_Interceptor.Forms
{
    public partial class LoadForm : Form
    {
        private static LoadForm instance;
        public static LoadForm getInstance()
        {
            if (instance == null) instance = new LoadForm();
            return instance;
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

        //Parametros necesarios para simular el desplazamiento del Form
        //////////////////////////////////////////
        public volatile bool working = false;
        private Process rustProcess;
        private readonly String rustProcessName = "RustClient";
        private UKeyHook hook;
        private Overlay overlay;

       
        private LoadForm()
        {
            this.Location = new Point(0, 0);
            hook = UKeyHook.getInstance(this);
           
            InitializeComponent();

            this.listViewRecordatorio.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(
                (object sender, ListViewItemSelectionChangedEventArgs e) =>
                {
                    String[] dir = e.Item.Text.Split(':');
                    
                    this.textBoxIp.Text = dir[0];
                    this.textBoxPuerto.Text = dir[1];
                }
            );

            this.buttonCerrar.Click += new EventHandler(
                (object sender, EventArgs e) =>
                {
                   this.Cerrar();
                });

            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(
                (object sender, FormClosedEventArgs e) =>
                {
                    this.working = false;
                }
                );
            this.Load += new System.EventHandler(
                (object sender, EventArgs e) =>
                {
                    this.LoadForm_Load(sender,e);
                }
                );

        }
        
        private void LoadForm_Load(object sender, EventArgs e)
        {
            //this.MaximumSize = this.Size;
            //g = this.CreateGraphics();
            this.labelIp.Click += new EventHandler(
                (object responsable, EventArgs evento) =>
                {
                    this.textBoxIp.Focus();
                });
            this.labelPuerto.Click += new EventHandler(
                (object responsable, EventArgs evento) =>
                {
                    this.textBoxPuerto.Focus();
                });
            this.buttonEmpezar.Click += new EventHandler(
                (object responsable, EventArgs evento) =>
                {
                    if (this.textBoxIp.Text.Equals("debug"))
                    {
                        return;
                    }
                    if (this.textBoxIp.Text.Length == 0 && this.textBoxPuerto.Text.Length == 0)
                        return;


                    searchRustProcess(out rustProcess);
                    if(rustProcess == null)
                    {
                        MessageBox.Show("No se ha encontrado ningún proceso de Rust activo ...");
                        return;
                    }

                    overlay = Overlay.getInstance(rustProcess);
                    overlay.FormClosed += new FormClosedEventHandler(
                        (object enviador, FormClosedEventArgs even) =>
                        {
                            this.Show();
                        });

                    this.Hide();

                    overlay.start(this.textBoxIp.Text, Convert.ToInt32(this.textBoxPuerto.Text) );

                    //this.working = true;

                });

        }

        private void searchRustProcess(out Process proceso)
        {
            Process[] procesos = Process.GetProcessesByName(rustProcessName);
            proceso = null;
            if (procesos.Length > 0)
            {
                Console.WriteLine("Proceso encontrado");
                proceso = procesos[0];
            }
        }

        private delegate void resetTextCallback(Control elemento);
        private void resetText(Control elemento)
        {
            if (this.InvokeRequired) this.Invoke(new resetTextCallback(resetText), elemento);
            else
            {
                elemento.ResetText();
            }
        }
        private delegate void appendTextCallback(Control elemento,String cadena);
        private void appendText(Control elemento, String cadena)
        {
            if (this.InvokeRequired) this.Invoke(new appendTextCallback(appendText), elemento, cadena);
            else
            {
                elemento.Text += cadena;
            }
        }
        private delegate void resizeFormCallback(RECTANGULO rectangulo);
        public void resizeForm(RECTANGULO rectangulo)
        {
            if (this.InvokeRequired) this.Invoke(new resizeFormCallback(resizeForm),rectangulo);
            else
            {
                //this.Size = rectangulo.Size;
                this.Size = new Size(SystemInfo.getSystemInfo(SystemInfo.SystemMetric.SM_CXFULLSCREEN), SystemInfo.getSystemInfo(SystemInfo.SystemMetric.SM_CYFULLSCREEN));
                //Setting Window position;
                this.Top = (int)rectangulo.Top;
                this.Left = (int)rectangulo.Left;
            }
        }
        private delegate void genericCallback();
        public void Cerrar()
        {
            if (this.InvokeRequired) this.Invoke(new genericCallback(Cerrar));
            else
            {
                this.working = false;
                if (hook != null)
                {
                    if (hook.working)
                    {
                        hook.stop();
                        //sniffer.SavePackets();
                    }
                }
                this.Close();
            }
        }
    }
}
