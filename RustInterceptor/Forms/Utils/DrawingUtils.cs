using Rust_Interceptor.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rust_Interceptor.Forms.Structs.WindowStruct;
using Vector3DX = SharpDX.Vector3;
using Vector2DX = SharpDX.Vector2;
using Vector3DU = UnityEngine.Vector3;
using Vector2DU = UnityEngine.Vector2;

namespace Rust_Interceptor.Forms.Utils
{
    class DrawingUtils
    {
        #region Constructor
        private static DrawingUtils instance;
        public static DrawingUtils getInstance()
        {
            if (instance == null)
                instance = new DrawingUtils();
            return instance;
        }

        private Controller manejador; 
        public Controller getController()
        {
            return manejador;
        }
        private DrawingUtils()
        {
            manejador = Controller.getInstance();
            manejador.mostrarse();
        }
        #endregion Constructo

        public void drawCrosshair(Graphics g, Color color, RECTANGULO targetRect, int longitudRecta = 5, bool ignoreOffset = false)
        {
            Pen lapiz = new Pen(color);
            lapiz.Width = 1;
            //linea horizontal
            Vector2DX offsetCentro = ignoreOffset ? new Vector2DX() : new Vector2DX(manejador.getXCrosshairOffsetValue(), manejador.getYCrosshairOffsetValue());
            Vector2DX centro = targetRect.CenterAbsolute - offsetCentro;
            POINT middleMinorH = centro - Vector2DX.UnitX * longitudRecta;
            POINT middleMayorH = centro + Vector2DX.UnitX * longitudRecta;
            g.DrawLine(lapiz, middleMinorH, middleMayorH);
            //linea vertical
            POINT middleMinorV = centro - Vector2DX.UnitY * longitudRecta;
            POINT middleMayorV = centro + Vector2DX.UnitY * longitudRecta;
            g.DrawLine(lapiz, middleMinorV, middleMayorV);
        }

        #region Radar

        private int RADAR_RADIUS = 75;
        private int PLAYER_BOX_SIZE = 4;
        private int PLAYER_FOV_SIZE = 4;
        public void drawRadar(Graphics g, Color color, POINT init, Entity localPlayer , out RECTANGULO radarRect) //, List<Entity> players)
        {
            radarRect = new RECTANGULO( 
                Convert.ToInt32(init.X),
                Convert.ToInt32(init.Y),
                Convert.ToInt32(init.X+(RADAR_RADIUS * 2 - 1) ),
                Convert.ToInt32(init.Y+(RADAR_RADIUS * 2 - 1) )
                );
            //DrawingUtils.AddorUpdateRect(clase, rect);
            //if (filled) //FIXME Se rellena con el color de fondo del form
            {
                SolidBrush pincel = new SolidBrush(color);
                g.FillEllipse(pincel, radarRect);
            }
            Pen lapiz = new Pen(color);
            lapiz.Width = 2;
            lapiz.Color = Color.FromArgb(255,color.R, color.G, color.B);
            g.DrawEllipse(lapiz, radarRect);

            drawCrosshair(g, Color.FromArgb(128,Color.Red), radarRect, RADAR_RADIUS,true); //Para comprobar si el calculo del centro esta Ok

        }
        
        public void drawRadarPlayer(Graphics g, Entity localPlayer, Entity player, Vector2DX worldMid  , float escala = 2f)
        {
            Brush pincel = Brushes.Red;
            if (player.Data.basePlayer.modelState.sleeping) pincel = Brushes.Gray;
            if (player.IsLocalPlayer) pincel = Brushes.Gold;

            Vector2DX pos = calculateRelativePos(localPlayer, player, worldMid);
            g.FillEllipse(
                pincel,
                pos.X - PLAYER_BOX_SIZE / escala,
                pos.Y - PLAYER_BOX_SIZE / escala,
                PLAYER_BOX_SIZE,
                PLAYER_BOX_SIZE
            );
            //Dibujo metros entre yo y el personaje
            String metros = player.IsLocalPlayer ? "" : (int)Vector2DX.Distance(Vector3To2(localPlayer.Data.baseEntity.pos), Vector3To2(player.Data.baseEntity.pos) )+" m";
            Font fuente = new Font(FontFamily.GenericMonospace, 8);
            g.DrawString(metros, fuente, pincel, new PointF(pos.X,pos.Y) );
            
            //Prepare FOV. Es decir, la cosa esa tan bonita que nos muestra la direcci´no de pa donde mira el jugador
            Vector2DX[] fov = new Vector2DX[3];
            fov[0] = pos;
            fov[1] = pos - (Vector2DX.UnitY * PLAYER_FOV_SIZE) + (Vector2DX.UnitX * PLAYER_FOV_SIZE);
            fov[2] = pos - (Vector2DX.UnitY * PLAYER_FOV_SIZE) - (Vector2DX.UnitX * PLAYER_FOV_SIZE);
            //Rotate FOV according to this player's and localPlayer's yaw //PReviamente en lugar de z era y
            fov[1] = RotatePoint(fov[1], pos, localPlayer.Data.baseEntity.rot.y - player.Data.baseEntity.rot.y);
            fov[2] = RotatePoint(fov[2], pos, localPlayer.Data.baseEntity.rot.y - player.Data.baseEntity.rot.y);
            //Draw FOV
            g.FillPolygon(
                pincel,
                ConverttoPOINTF(fov)
            );
        }
        // escala = min/value  0,001
        #endregion Radar

        #region Utils
        private Vector2DX calculateRelativePos(Entity localPlayer, Entity player, Vector2DX centroRadar)
        {
            //Console.WriteLine(player.Data.basePlayer.name + " -> "+player.Data.baseEntity.pos );
            Vector2DX screenPos = Vector3To2(player.Data.baseEntity.pos);
            screenPos = Vector3To2(localPlayer.Data.baseEntity.pos) - screenPos;
            
            float distance = screenPos.Length()/manejador.getZoomValue() * RADAR_RADIUS;//float distance = screenPos.Length() * (0.02f * manejador.getZoomValue());

            distance = Math.Min( distance, RADAR_RADIUS);
            screenPos.Normalize();
            screenPos *= distance;
            screenPos += centroRadar;
            screenPos = RotatePoint(screenPos, centroRadar, localPlayer.Data.baseEntity.rot.y + manejador.getAngleValue());

            return screenPos;
        }
        private Vector2DX RotatePoint(Vector2DX pointToRotate, Vector2DX centerPoint, float angle, bool angleInRadians = false)
        {
            if (!angleInRadians)
                angle = (float)(angle * (Math.PI / 180f));
            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);

            /*Vector2 returnVec = new Vector2(
                cosTheta * (centerPoint.X - pointToRotate.X) - sinTheta * (centerPoint.Y - pointToRotate.Y) 
                ,
                sinTheta * (centerPoint.X - pointToRotate.X) + cosTheta * (centerPoint.Y - pointToRotate.Y) 
            );*/
            Vector2DX returnVec = new Vector2DX(
                sinTheta * (pointToRotate.Y - centerPoint.Y) - cosTheta * (pointToRotate.X - centerPoint.X),
                sinTheta * (pointToRotate.X - centerPoint.X) + cosTheta * (pointToRotate.Y - centerPoint.Y)
            );
            returnVec += centerPoint;
            return returnVec;
        }

        private Vector2DX Vector3To2(UnityEngine.Vector3 pos)
        {
            return new Vector2DX(pos.x, pos.z);
        }

        public PointF[] ConverttoPOINTF(params Vector2DX[] puntos)
        {
            PointF[] pts = new PointF[puntos.Length];
            for (int i = 0; i < puntos.Length; i++) pts[i] = new PointF(puntos[i].X, puntos[i].Y);
            return pts;
        }

        #endregion Utils

        #region 3D World
        public void drawPlayer(Graphics g, Entity localPlayer, Entity player, RECTANGULO windowRect)
        {
            /*Translate by - camerapos*/
            var camerapos = new Vector3DU(localPlayer.Data.baseEntity.pos.x, localPlayer.Data.baseEntity.pos.y, -localPlayer.Data.baseEntity.pos.z);
            var point = new Vector3DU(player.Data.baseEntity.pos.x, player.Data.baseEntity.pos.y, -player.Data.baseEntity.pos.z);

            point = camerapos - point;

            Vector3DU camRotation = localPlayer.Data.baseEntity.rot;

            /*Construct rotation matrix
            Rotations would be negative here to account for opposite direction of     rotations in Unity's left-handed coordinate system, 
            but since we're making them negative already this cancels out*/
            //Console.WriteLine(camRotation);
            float P = (float)Math.PI / 180;

            float cosX = (float)Math.Cos(camRotation.x * P); //Hay que convertir grados a radianes
            float cosY = (float)Math.Cos(camRotation.y * P);
            float cosZ = (float)Math.Cos(camRotation.z * P);
            float sinX = (float)Math.Sin(camRotation.x * P);
            float sinY = (float)Math.Sin(camRotation.y * P);
            float sinZ = (float)Math.Sin(camRotation.z * P);

            float[,] matrix = new float[3, 3];
            matrix[0, 0] = cosZ * cosY - sinZ * sinX * sinY;
            matrix[0, 1] = -cosX * sinZ;
            matrix[0, 2] = cosZ * sinY + cosY * sinZ * sinX;
            matrix[1, 0] = cosY * sinZ + cosZ * sinX * sinY;
            matrix[1, 1] = cosZ * cosX;
            matrix[1, 2] = sinZ * sinY - cosZ * cosY * sinX;
            matrix[2, 0] = -cosX * sinY;
            matrix[2, 1] = sinX;
            matrix[2, 2] = cosX * cosY;

            /*Apply rotation matrix to target point*/
            Vector3DU rotatedPoint;

            rotatedPoint.x = matrix[0, 0] * point.x + matrix[0, 1] * point.y + matrix[0, 2] * point.z;
            rotatedPoint.y = matrix[1, 0] * point.x + matrix[1, 1] * point.y + matrix[1, 2] * point.z;
            rotatedPoint.z = matrix[2, 0] * point.x + matrix[2, 1] * point.y + matrix[2, 2] * point.z;

            Vector3DU resultPoint = new Vector3DU(rotatedPoint.x, rotatedPoint.y, -rotatedPoint.z);

            if (resultPoint.z > 0)
            {
                float focalLength = 540f / (float)Math.Tan(85f * P / 2f);
                float screenX = focalLength * resultPoint.x / resultPoint.z + windowRect.Width / 2;
                float screenY = focalLength * resultPoint.y / resultPoint.z + windowRect.Height / 2;
                RECTANGULO rect = new RECTANGULO(screenX, screenY, screenX + 10, screenY + 10);
                Pen lapiz = new Pen(Color.Red);
                g.DrawRectangle(lapiz, rect);
                //device.DrawEllipse(new Ellipse(new RawVector2(screenX, screenY), 6, 6), greenBrush);
            }
        }
        #endregion 3DWorld




    }
}
