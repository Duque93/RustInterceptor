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
        private Point lastLocation;
        //////////////////////////////////////////

        public volatile bool working = false;

        private RustInterceptor sniffer;
        private Process targetProcess;

        private Thread worker, cleaner;

        private ConcurrentDictionary<String,Entity> listaUsuarios = new ConcurrentDictionary<String, Entity>();
        private Entity localPlayer;

        private Graphics g;
        private DrawingUtils pintor;
        private RECTANGULO rectRadar;
        

        private static Overlay instance;
        public static Overlay getInstance(Process targetProcess)
        {
            if (instance == null) instance = new Overlay(targetProcess);
            return instance;
        }

        private Overlay(Process targetProcess)
        {
            this.targetProcess = targetProcess;
            InitializeComponent();
            
        }

        //Oculta todos los elementos para poder tener un overlayForm
        public void start(String server, int port)
        {
            sniffer = RustInterceptor.getInstance(server, port);
            this.hideControls();
            pintor = DrawingUtils.getInstance();
            //Hilo que se encargara de ir limpiando las entidades que esten muy lejos o no merezca la pena espiar.
            cleaner = new Thread(
                () =>
                {
                    do
                    {
                        if (localPlayer != null)
                        {
                            List<KeyValuePair<String, Entity>> filas = this.listaUsuarios.ToList<KeyValuePair<String, Entity>>();
                            foreach (KeyValuePair<String, Entity> fila in filas)
                            {
                                if (fila.Value == null) return;
                                Entity entidad = fila.Value;
                                
                                float distance = UnityEngine.Vector2.Distance(
                                    new UnityEngine.Vector2(entidad.Data.baseEntity.pos.x, entidad.Data.baseEntity.pos.z)
                                    ,
                                    new UnityEngine.Vector2(this.localPlayer.Data.baseEntity.pos.x, this.localPlayer.Data.baseEntity.pos.z));
                                if (distance > pintor.getController().getZoomValue() || entidad.Data.basePlayer.modelState.onground && !entidad.Data.basePlayer.modelState.sleeping )
                                    this.listaUsuarios.TryRemove(fila.Key, out entidad);
                            }
                        }
                        Thread.Sleep(5*1000);
                       
                    } while (working);
                });

            cleaner.SetApartmentState(ApartmentState.MTA);
            cleaner.IsBackground = true;
            cleaner.CurrentCulture = System.Globalization.CultureInfo.CurrentCulture;
            cleaner.Priority = ThreadPriority.BelowNormal;
            cleaner.Name = "OverlayCleanerThread";

            //Creo un Hilo que se encargara de hacer todo el curro
            worker = new Thread(
                () =>
                {
                    targetProcess.EnableRaisingEvents = true;
                    targetProcess.Exited += new EventHandler( //En cuanto se cierre Rust, cerramos OVerlay
                        (object sender, EventArgs e) =>
                        {
                            this.Stop();
                        });
                    
                    //¿Porque ha dejado de funcionar de pronto?
                    //Puede ser por el cambio de propiedades de int a float que hice en RECTANGULO
                    //new WindowHook(targetProcess.MainWindowHandle, this); //De momento esta clase se ocupa directamente de redimensionar la ventana
                    this.resizeForm(new RECTANGULO());


                    sniffer.Start();
                    while( this.localPlayer == null) //Esperamos hasta que tengamos el localplayer
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("No me he encontrado...Sigo buscandome");
                    }
                    Console.WriteLine("Me he encontrado");

                    working = true;
                    cleaner.Start();
                    //cleaner.Join();
                    do
                    {
                        this.worldToScreen();
                        Thread.Sleep(100);
                    } while (working);
                    
                });

            worker.SetApartmentState(ApartmentState.MTA);
            worker.IsBackground = true;
            worker.CurrentCulture = System.Globalization.CultureInfo.CurrentCulture;
            worker.Priority = ThreadPriority.Highest;
            worker.Name = "OverlayWorkerThread";

            worker.Start();
            //worker.Join();

            this.Show();
        }
        public void Stop()
        {
            if (this.InvokeRequired) this.Invoke(new genericCallback(Stop));
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
                instance = null;
            }
        }

        //Se ocupara de mostrar en el overlay todo las entidades que nos interesa
        private delegate void genericCallback();
        private void worldToScreen()
        {
            if (this.InvokeRequired) this.Invoke(new genericCallback(this.worldToScreen));
            else
            {
                if(g == null) g = this.CreateGraphics();
                g.Clear(Color.Black);
                pintor.drawCrosshair(g, Color.FromArgb(128, Color.Red), this.ClientRectangle);

                int radio = 75;
                POINT init = new POINT(this.Right - (radio * 2 + 5), this.Top + 8); //A la derecha con margenes para ver el radar al completo
                
                pintor.drawRadar(g, Color.FromArgb(10, Color.Gray), init, this.localPlayer, out this.rectRadar); // this.listaUsuarios.Values.ToList());

                if (this.localPlayer == null) return;

                pintor.drawRadarPlayer(g, this.localPlayer, this.localPlayer, rectRadar.CenterAbsolute);
                List<Entity> jugadores = this.listaUsuarios.Values.ToList();
                foreach(Entity jugador in jugadores)
                {
                    pintor.drawRadarPlayer(g,this.localPlayer,jugador,rectRadar.CenterAbsolute);
                    //pintor.drawPlayer(g, this.localPlayer, jugador, this.ClientRectangle);
                }

            }

        }
        private delegate void resizeFormCallback(RECTANGULO rectangulo);
        public void resizeForm(RECTANGULO rectangulo)
        {
            if (this.InvokeRequired) this.Invoke(new resizeFormCallback(resizeForm),rectangulo);
            else
            {
                int offsetWindowBar = 20;
                this.Size = rectangulo.Size;
                //this.Size = new Size(SystemInfo.getSystemInfo(SystemInfo.SystemMetric.SM_CXFULLSCREEN), SystemInfo.getSystemInfo(SystemInfo.SystemMetric.SM_CYFULLSCREEN)+ offsetWindowBar);
                //Setting Window position;
                this.Top = (int)rectangulo.Top;
                this.Left = (int)rectangulo.Left;
            }
        }
        private void hideControls()
        {
            foreach (Control ctrl in this.Controls) //Oculto todos los controles
            {
                ctrl.Visible = false;
            }
            this.BackColor = Color.Black; //Seteamos el color de fondo a negro.
            this.TransparencyKey = Color.Black; //Indicamos que todo lo que se encuentre en negro dentro del formulario sea transparente
            //Metodo para que pulsar atraves de él sea posible
            int initialStyle = DLLImports.GetWindowLong(this.Handle, -20);
            DLLImports.SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
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
            if (this.InvokeRequired) this.Invoke(new onEntityCallback(onEntity), entidad);
            else
            {
                if (entidad.IsPlayer)
                {
                    if (entidad.IsLocalPlayer)
                    {
                        this.localPlayer = entidad;
                        if (this.localPlayer.Data.basePlayer.metabolism.health == 0) //FIXME . No borra cuando mi vida llega a 0 
                        {

                            this.listaUsuarios.Clear();
                        }
                        return;
                    }
                    if (this.localPlayer != null)
                    {
                        lock (this.listaUsuarios)
                        {
                            Entity prev = null;
                            //Una especie de OnAcercarse()
                            float distance = UnityEngine.Vector2.Distance(
                                new UnityEngine.Vector2(entidad.Data.baseEntity.pos.x, entidad.Data.baseEntity.pos.z)
                                ,
                                new UnityEngine.Vector2(this.localPlayer.Data.baseEntity.pos.x, this.localPlayer.Data.baseEntity.pos.z));

                            if (distance < pintor.getController().getZoomValue())
                            {
                                this.listaUsuarios.TryGetValue(entidad.Data.basePlayer.name, out prev);
                                if (prev == null) this.listaUsuarios.TryAdd(entidad.Data.basePlayer.name, entidad);
                                else this.listaUsuarios.TryUpdate(entidad.Data.basePlayer.name, entidad, prev);
                            }
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
            }

        }



    }
}
