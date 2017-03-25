using System;
using Rust_Interceptor.Forms.Hooks;
using Rust_Interceptor.Forms.Structs;
using System.Windows.Forms;
using static Rust_Interceptor.Forms.Structs.WindowStruct;
using System.Threading;

namespace Rust_Interceptor.Forms
{
    partial class Overlay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Overlay));
            this.SuspendLayout();
            // 
            // Overlay
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Overlay";
            this.ShowIcon = false;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        internal class KeyEventListener<T> : IKeyEventsListener where T : Overlay
        {
            private Overlay parent;
            private bool lDown = false, rDown = false;
            public KeyEventListener(T parent)
            {
                this.parent = parent;
            }

            public bool onHorizontalWheelMouseRotation(object sender, WindowStruct.MOUSEINPUT data)
            {
                return true;
            }

            public bool onLeftMouseButtonDown(object sender, WindowStruct.MOUSEINPUT data)
            {
                lDown = true;
                /*while(lDown && rDown)
                {
                    UKeyHook hook = (UKeyHook)sender;
                    RECTANGULO rect = parent.ClientRectangle;
                    hook.action.moveMouseToPoint(rect.CenterAbsolute + new POINT(100, 0));
                }*/
                return true;
            }

            public bool onLeftMouseButtonUp(object sender, WindowStruct.MOUSEINPUT data)
            {
                lDown = false;
                return true;
            }
            
            public bool onMiddleMouseButtonDown(object sender, WindowStruct.MOUSEINPUT data)
            {
               
                return true;
            }

            public bool onMiddleMouseButtonUp(object sender, WindowStruct.MOUSEINPUT data)
            {
                return true;
            }

            public bool onMouseMove(object sender, WindowStruct.MOUSEINPUT data)
            {
                return true;
            }

            public bool onRightMouseButtonDown(object sender, WindowStruct.MOUSEINPUT data)
            {
                rDown = true;
                return true;
            }

            public bool onRightMouseButtonUp(object sender, WindowStruct.MOUSEINPUT data)
            {
                rDown = false;
                return true;
            }

            public bool onVerticalWheelMouseRotation(object sender, WindowStruct.MOUSEINPUT data)
            {
                return true;
            }

            public bool onKeyDown(object sender, KEYBDINPUT data, Keys key)
            {
                return true;
            }
            public bool onKeyUp(object sender, KEYBDINPUT data, Keys key)
            {
                return true;
            }
        }
    }
}