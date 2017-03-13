using Rust_Interceptor.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rust_Interceptor.Forms.Structs.WindowStruct;
using Vector3 = SharpDX.Vector3;
using Vector2 = SharpDX.Vector2;

namespace Rust_Interceptor.Forms.Utils
{
    static class DrawingUtils
    {
        private static int RADAR_RADIUS = 75;
        private static int PLAYER_BOX_SIZE = 8;
        private static int PLAYER_FOV_SIZE = 8;
        //public static Dictionary<Type, RECTANGULO[]> radarData = new Dictionary<Type, RECTANGULO[]>();

        public static void drawCrosshair(Graphics g, Color color, RECTANGULO windowRect, int longitudRecta = 5)
        {
            Pen lapiz = new Pen(color);
            lapiz.Width = 1;
            //lapiz.Color = ;
            //linea horizontal
            POINT middleMinorH = windowRect.CenterAbsolute - Vector2.UnitX*longitudRecta;
            POINT middleMayorH = windowRect.CenterAbsolute + Vector2.UnitX* longitudRecta;
            g.DrawLine(lapiz, middleMinorH, middleMayorH);
            //linea vertical
            POINT middleMinorV = windowRect.CenterAbsolute - Vector2.UnitY* longitudRecta;
            POINT middleMayorV = windowRect.CenterAbsolute + Vector2.UnitY* longitudRecta;
            g.DrawLine(lapiz, middleMinorV, middleMayorV);
        }
        public static void drawRadar(Graphics g, Color color, POINT init, Entity localPlayer, List<Entity> players)
        {
            RECTANGULO rect = new RECTANGULO( 
                Convert.ToInt32(init.X),
                Convert.ToInt32(init.Y),
                Convert.ToInt32(init.X+(RADAR_RADIUS * 2 - 1) ),
                Convert.ToInt32(init.Y+(RADAR_RADIUS * 2 - 1) )
                );
            //DrawingUtils.AddorUpdateRect(clase, rect);
            //if (filled) //FIXME Se rellena con el color de fondo del form
            {
                //SolidBrush pincel = new SolidBrush(color);
                //g.FillEllipse(pincel, rect);
            }
            Pen lapiz = new Pen(color);
            lapiz.Width = 2;
            lapiz.Color = Color.FromArgb(255,color.R, color.G, color.B);
            g.DrawEllipse(lapiz, rect);

            drawCrosshair(g, Color.FromArgb(128,Color.Red), rect, RADAR_RADIUS); //Para comprobar si el calculo del centro esta Ok

            if (localPlayer == null) return;
            //Empezamos dibujando los jugadores en el radar
            //POINT pos = calculateRelativePos(localPlayer, localPlayer, RADAR_RADIUS, rect.CenterAbsolute);
            //drawRadarPlayer(g, localPlayer, localPlayer, rect.CenterAbsolute, 2f);
            foreach ( Entity player in players)
            {
                //pos = calculateRelativePos(player, localPlayer, RADAR_RADIUS, rect.CenterAbsolute);
                drawRadarPlayer(g, localPlayer, player, rect.CenterAbsolute, 2f);
            }

        }

        public enum PlayerFlags
        {
            InBuildingPrivilege = 1,
            HasBuildingPrivilege = 2,
            IsAdmin = 4,
            ReceivingSnapshot = 8,
            Sleeping = 16,
            Spectating = 32,
            Wounded = 64,
            IsDeveloper = 128,
            Connected = 256,
            VoiceMuted = 512,
            ThirdPersonViewmode = 1024,
            EyesViewmode = 2048,
            ChatMute = 4096,
            NoSprint = 8192,
            Aiming = 16384,
            NotNoob = 32768, // Wut is this?
        }
        private static void drawRadarPlayer(Graphics g, Entity localPlayer, Entity player, Vector2 worldMid  , float escala)
        {
            Brush pincel = player.Data.basePlayer.playerFlags == (int)PlayerFlags.Sleeping ? Brushes.LightGray : Brushes.Red;
            if (player.IsLocalPlayer) pincel = Brushes.Gold;

            Vector2 pos = calculateRelativePos(localPlayer, player, worldMid);
            g.FillEllipse(
                pincel,
                pos.X - PLAYER_BOX_SIZE / escala,
                pos.Y - PLAYER_BOX_SIZE / escala,
                PLAYER_BOX_SIZE,
                PLAYER_BOX_SIZE
            );
            
            //Prepare FOV. Es decir, la cosa esa tan bonita que nos muestra la direcci´no de pa donde mira el jugador
            Vector2[] fov = new Vector2[3];
            fov[0] = pos;
            fov[1] = pos - (Vector2.UnitY * PLAYER_FOV_SIZE) + (Vector2.UnitX * PLAYER_FOV_SIZE);
            fov[2] = pos - (Vector2.UnitY * PLAYER_FOV_SIZE) - (Vector2.UnitX * PLAYER_FOV_SIZE);
            //Rotate FOV according to this player's and localPlayer's yaw
            fov[1] = RotatePoint(fov[1], pos, localPlayer.Data.baseEntity.rot.y - player.Data.baseEntity.rot.y);
            fov[2] = RotatePoint(fov[2], pos, localPlayer.Data.baseEntity.rot.y - player.Data.baseEntity.rot.y);
            //Draw FOV
            g.FillPolygon(
                pincel,
                ConverttoPOINTF(fov)
            );
        }

        private static Vector2 calculateRelativePos(Entity localPlayer, Entity player, Vector2 centroRadar)
        {
            Console.WriteLine(player.Data.basePlayer.name + " -> "+player.Data.baseEntity.pos+" Sleeping? "+ (player.Data.basePlayer.playerFlags == (int)PlayerFlags.Sleeping) );
            Vector2 screenPos = Vector3To2(player.Data.baseEntity.pos);
            screenPos = Vector3To2(localPlayer.Data.baseEntity.pos) - screenPos;
            float distance = screenPos.Length() * (0.02f * Controller.getInstance().getZoomValue());
            distance = Math.Min(distance, RADAR_RADIUS);
            screenPos.Normalize();
            screenPos *= distance;
            screenPos += centroRadar;
            screenPos = RotatePoint(screenPos, centroRadar, localPlayer.Data.baseEntity.rot.y - 90);

            return screenPos;
        }
        private static Vector2 RotatePoint(Vector2 pointToRotate, Vector2 centerPoint, float angle, bool angleInRadians = false)
        {
            if (!angleInRadians)
                angle = (float)(angle * (Math.PI / 180f));
            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);
            Vector2 returnVec = new Vector2(
                cosTheta * (pointToRotate.X - centerPoint.X) - sinTheta * (pointToRotate.Y - centerPoint.Y),
                sinTheta * (pointToRotate.X - centerPoint.X) + cosTheta * (pointToRotate.Y - centerPoint.Y)
            );
            returnVec += centerPoint;
            return returnVec;
        }

        private static Vector2 Vector3To2(UnityEngine.Vector3 pos)
        {
            return new Vector2(pos.x, pos.y);
        }

        public static PointF[] ConverttoPOINTF(params Vector2[] puntos)
        {
            PointF[] pts = new PointF[puntos.Length];
            for (int i = 0; i < puntos.Length; i++) pts[i] = new PointF(puntos[i].X, puntos[i].Y);
            return pts;
        }








    }
}
