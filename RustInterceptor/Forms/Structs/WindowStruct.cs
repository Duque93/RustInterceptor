using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Rust_Interceptor.Forms.Structs
{
    public static class WindowStruct
    {
        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECTANGULO
        {
            public int Left, Top, Right, Bottom;

            public RECTANGULO(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECTANGULO(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point((int)Left, (int)Top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size((int)Width, (int)Height); }
                set { Width = value.Width; Height = value.Height; }
            }
            public SharpDX.Vector2 CenterRelative
            {
                get { return new SharpDX.Vector2(this.Width/2, this.Height/2); }
            }

            public SharpDX.Vector2 CenterAbsolute
            {
                get { return new SharpDX.Vector2( (this.Width / 2)+this.Left , (this.Height / 2)+this.Top ) ; }
            }

            public static implicit operator RECTANGULO(System.Drawing.Size r)
            {
                return new RECTANGULO(0, 0, r.Width, r.Height);
            }
            public static implicit operator System.Drawing.Rectangle(RECTANGULO r)
            {
                return new System.Drawing.Rectangle((int)r.Left, (int)r.Top, (int)r.Width, (int)r.Height);
            }

            public static implicit operator System.Drawing.RectangleF(RECTANGULO r)
            {
                return new System.Drawing.RectangleF(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECTANGULO(System.Drawing.Rectangle r)
            {
                return new RECTANGULO(r);
            }

            public static bool operator ==(RECTANGULO r1, RECTANGULO r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECTANGULO r1, RECTANGULO r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECTANGULO r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECTANGULO)
                    return Equals((RECTANGULO)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECTANGULO((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        public enum EnumShowWindowCommands
        {
            /// <summary>
            /// Hides the window and activates another window.
            /// </summary>
            Hide = 0,
            /// <summary>
            /// Activates and displays a window. If the window is minimized or
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when displaying the window
            /// for the first time.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// Activates the window and displays it as a minimized window.
            /// </summary>
            ShowMinimized = 2,
            /// <summary>
            /// Maximizes the specified window.
            /// </summary>
            Maximize = 3, // is this the right value?
                          /// <summary>
                          /// Activates the window and displays it as a maximized window.
                          /// </summary>      
            ShowMaximized = 3,
            /// <summary>
            /// Displays a window in its most recent size and position. This value
            /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except
            /// the window is not activated.
            /// </summary>
            ShowNoActivate = 4,
            /// <summary>
            /// Activates the window and displays it in its current size and position.
            /// </summary>
            Show = 5,
            /// <summary>
            /// Minimizes the specified window and activates the next top-level
            /// window in the Z order.
            /// </summary>
            Minimize = 6,
            /// <summary>
            /// Displays the window as a minimized window. This value is similar to
            /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the
            /// window is not activated.
            /// </summary>
            ShowMinNoActive = 7,
            /// <summary>
            /// Displays the window in its current size and position. This value is
            /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the
            /// window is not activated.
            /// </summary>
            ShowNA = 8,
            /// <summary>
            /// Activates and displays the window. If the window is minimized or
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 9,
            /// <summary>
            /// Sets the show state based on the SW_* value specified in the
            /// STARTUPINFO structure passed to the CreateProcess function by the
            /// program that started the application.
            /// </summary>
            ShowDefault = 10,
            /// <summary>
            ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread
            /// that owns the window is not responding. This flag should only be
            /// used when minimizing windows from a different thread.
            /// </summary>
            ForceMinimize = 11
        }
        /// <summary>
        /// Contains information about the placement of a window on the screen.
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            /// <summary>
            /// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
            /// <para>
            /// GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
            /// </para>
            /// </summary>
            public int length;

            /// <summary>
            /// Specifies flags that control the position of the minimized window and the method by which the window is restored.
            /// </summary>
            public int flags;

            /// <summary>
            /// The current show state of the window.
            /// </summary>
            public EnumShowWindowCommands showCmd;

            /// <summary>
            /// The coordinates of the window's upper-left corner when the window is minimized.
            /// </summary>
            public POINT minPosition;

            /// <summary>
            /// The coordinates of the window's upper-left corner when the window is maximized.
            /// </summary>
            public POINT maxPosition;

            /// <summary>
            /// The window's coordinates when the window is in the restored position.
            /// </summary>
            public RECTANGULO normalPosition;

            /// <summary>
            /// Gets the default (empty) value.
            /// </summary>
            public static WINDOWPLACEMENT Default
            {
                get
                {
                    WINDOWPLACEMENT result = new WINDOWPLACEMENT();
                    result.length = Marshal.SizeOf(result);
                    return result;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X,Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) { }

            public double Length()
            {
                return Math.Sqrt( Math.Pow(this.X,2) + Math.Pow(this.Y,2) );
            }

            public POINT Normalize()
            {
                return new POINT(this.X,this.Y) / (float)this.Length();
            }

            public static POINT Vector2Y = new POINT(0,1);
            public static POINT Vector2X = new POINT(1,0);

            
            public static POINT operator -(POINT a, POINT b)
            {
                return new POINT(a.X - b.X, a.Y - b.Y);
            }
            public static POINT operator *(POINT a,double operador)
            {
                return new POINT( (int)(a.X*operador), (int)(a.Y*operador));
            }
            public static POINT operator +(POINT a, POINT b)
            {
                return new POINT(a.X + b.X, a.Y + b.Y);
            }
            public static POINT operator /(POINT a, double operador)
            {
                return new POINT((int)(a.X/operador), (int)(a.Y/operador));
            }

            //Point to POINT and viceversa
            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point( (int)(p.X), (int)(p.Y));
            }
            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
            //
            //SharpDX.Vector2 to POINT
            public static implicit operator POINT(SharpDX.Vector2 p)
            {
                return new POINT((int)p.X, (int)p.Y);
            }

            public static implicit operator System.Drawing.PointF(POINT p)
            {
                return new System.Drawing.PointF(p.X, p.Y);
            }

            public override string ToString()
            {
                return "{X = "+this.X+" , Y = "+this.Y+"}";
            }
        }

    }
}
