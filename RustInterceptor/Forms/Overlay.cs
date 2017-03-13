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
    public partial class Overlay : Form
    {
        //Parametros necesarios para simular el desplazamiento del Form
        private bool mouseDown;
        private Point lastLocation;
        //////////////////////////////////////////

        public volatile bool working = false;
        private RustInterceptor sniffer;
        private Thread worker;
        private Process rustProcess;
        private readonly String rustProcessName = "RustClient";

        private ConcurrentDictionary<uint,Entity> listaUsuarios = new ConcurrentDictionary<uint, Entity>();
        //private HashSet<Entity> listaUsuarios = new HashSet<Entity>();
        private Entity localPlayer;

        Graphics g;

        public Overlay()
        {
            this.Location = new Point(0, 0);
            InitializeComponent();
        }

        private void Overlay_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.working = false;
        }

        private void Overlay_Load(object sender, EventArgs e)
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
                    if (this.textBoxIp.Text.Length == 0 && this.textBoxPuerto.Text.Length == 0)
                        return;
                    if (this.textBoxIp.Text.Equals("debug"))
                    {
                        this.hideControls();
                        this.Size = new Size(ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CXFULLSCREEN), ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CYFULLSCREEN));
                        
                        this.worldToScreen();
                        return;
                    }

                    //this.working = true;
                    sniffer = new RustInterceptor( this.textBoxIp.Text, Convert.ToInt32(this.textBoxPuerto.Text) );
                    sniffer.AddPacketsToFilter(Packet.Rust.Entities, Packet.Rust.EntityDestroy, Packet.Rust.EntityPosition);
                    sniffer.packetHandlerCallback = packetHandler;
                    sniffer.RememberPackets = true;
                    this.prepareOverlay();
                });

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

        //Oculta todos los elementos para poder tener un overlayForm
        public void prepareOverlay()
        {
            this.hideControls();
            this.labelPlayers.Visible = true;

            Controller.getInstance().mostrarse();
            //Creo un Hilo que se encargara de hacer todo el curro
            worker = new Thread(
                () =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Thread.CurrentThread.Name = "OverlayWorkerThread";
                    while( rustProcess == null ) // Iteramos hasta que el proceso Rust se haya inicializado y lo capturamos
                    {
                        Console.WriteLine("Buscando proceso de Rust...");
                        searchRustProcess(out rustProcess);
                        Thread.Sleep(1 * 1000);
                    }
                    rustProcess.EnableRaisingEvents = true;
                    rustProcess.Exited += new EventHandler( //En cuanto se cierre Rust, cerramos todo
                        (object sender, EventArgs e) =>
                        {
                            this.Cerrar();
                        });
                    
                    //¿Porque ha dejado de funcionar de pronto?
                    //Puede ser por el cambio de propiedades de int a float que hice en RECTANGULO
                    //new WindowHook(rustProcess.MainWindowHandle, this); //De momento esta clase se ocupa directamente de redimensionar la ventana
                    this.resizeForm(new RECTANGULO());

                    sniffer.Start();
                    while( this.localPlayer == null) //Esperamos hasta que tengamos el localplayer
                    {
                        Thread.Sleep(1000);
                    }
                    Console.WriteLine("Me he encontrado");
                    working = true;
                    
                    this.worldToScreen();
                    
                });

            worker.Start();
        }
        private void hideControls()
        {
            foreach (Control ctrl in this.Controls) //Oculto todos los controles
            {
                ctrl.Visible = false;
            }
            this.BackColor = Color.Green; //Seteamos el color de fondo a negro.
            this.TransparencyKey = Color.Green; //Indicamos que todo lo que se encuentre en negro dentro del formulario sea transparente

            int initialStyle = DLLImports.GetWindowLong(this.Handle, -20);
            DLLImports.SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
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
        private void packetHandler(Packet packet)
        {
            Entity entity;
            switch (packet.rustID)
            {
                case Packet.Rust.Entities:
                    ProtoBuf.Entity entityInfo;
                    uint num = Data.Entity.ParseEntity(packet, out entityInfo);
                    entity = Entity.CreateOrUpdate(num, entityInfo);
                    if (entity != null) onEntity(entity);
                    return;
                case Packet.Rust.EntityPosition:
                    List<Data.Entity.EntityUpdate> updates = Data.Entity.ParsePositions(packet);
                    List<Entity> entities = null;
                    if (updates.Count == 1)
                    {
                        entity = Entity.UpdatePosistion(updates[0]);
                        if (entity != null) (entities = new List<Entity>()).Add(entity);
                    }
                    else if (updates.Count > 1)
                    {
                        entities = Entity.UpdatePositions(updates);
                    }
                    if (entities != null) entities.ForEach(item => onEntity(item));
                    
                    return;
                case Packet.Rust.EntityDestroy:
                    EntityDestroy destroyInfo = new EntityDestroy(packet);
                    Entity.CreateOrUpdate(destroyInfo);
                    //onEntityDestroy(destroyInfo);
                    return;
            }
        }

        private delegate void onEntityCallback(Entity entidad);
        private void onEntity(Entity entidad)
        {
            if (this.InvokeRequired) this.Invoke(new onEntityCallback(onEntity) , entidad );
            else
            {
                if (entidad.IsPlayer)
                {
                    if (entidad.IsLocalPlayer)
                    {
                        this.localPlayer = entidad;
                        return;
                    }
                    if(this.localPlayer != null)
                    {
                        lock(this.listaUsuarios)
                        {
                            //Una especie de OnAcercarse()
                            //if (UnityEngine.Vector3.Distance(entidad.Data.baseEntity.pos, this.localPlayer.Data.baseEntity.pos) < 1000)
                            {
                                /*
                                if (this.listaUsuarios.Keys.Contains(entidad.UID)) //Si no lo contiene
                                    this.listaUsuarios.
                                    this.listaUsuarios.TryRemove(entidad.UID);*/
                                this.listaUsuarios.TryAdd(entidad.UID, entidad);
                            }
                            /*else
                                this.listaUsuarios.TryRemove(entidad.UID,out entidad);*/
                        }
                    }
                    
                }
            }
        }
        private delegate void onEntityDestroyCallback(EntityDestroy entidad);
        private void onEntityDestroy(EntityDestroy entidadDestruida) //Nunca se esta llamando actualmente
        {
            if (this.InvokeRequired) this.Invoke(new onEntityDestroyCallback(onEntityDestroy), entidadDestruida);
            else
            {
                lock (this.listaUsuarios)
                {
                    Entity entidad;
                    this.listaUsuarios.TryGetValue(entidadDestruida.UID,out entidad);
                    if (this.listaUsuarios.TryRemove(entidadDestruida.UID, out entidad))
                    {
                        //Console.WriteLine("Jugador con UID({0}) got destroyed ", entidadDestruida.UID);
                    }
                }
               
            }
                
        }
        //Se ocupara de mostrar en el overlay todo las entidades que nos interesa
        private delegate void genericCallback();
        private void worldToScreen()
        {
            //if (this.InvokeRequired) this.Invoke(new genericCallback(worldToScreen));
            //else
            do
            {
                g = this.CreateGraphics();
                g.Clear(Color.Green);
                DrawingUtils.drawCrosshair(g, Color.FromArgb(128, Color.Red), this.ClientRectangle);

                int radio = 75;
                POINT init = new POINT(this.Right - (radio * 2 + 5), this.Top + 8); //A la derecha con margenes para ver el radar al completo

                DrawingUtils.drawRadar( g, Color.FromArgb(10, Color.Gray), init, this.localPlayer, this.listaUsuarios.Values.ToList() );
                //DrawingUtils.drawRadar(g, Color.FromArgb(10, Color.Gray), init, this.localPlayer, Entity.GetPlayers().ToList() );
                Thread.Sleep(150);
            } while (working);


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
                this.Size = new Size(ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CXFULLSCREEN), ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CYFULLSCREEN));
                //Setting Window position;
                this.Top = rectangulo.Top;
                this.Left =rectangulo.Left;
            }
        }

        private void Cerrar()
        {
            if (this.InvokeRequired) this.Invoke(new genericCallback(Cerrar));
            else
            {
                this.working = false;
                if (sniffer != null)
                {
                    if (sniffer.IsAlive)
                    {
                        sniffer.Stop();
                        //sniffer.SavePackets();
                    }
                }
                this.Close();
            }
        }
    }
}
