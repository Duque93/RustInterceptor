using System;
using Rust_Interceptor.Forms.Hooks;
using Rust_Interceptor.Forms.Structs;
using System.Windows.Forms;
using static Rust_Interceptor.Forms.Structs.WindowStruct;
using System.Threading;

namespace Rust_Interceptor.Forms
{
    partial class LoadForm : IKeyEventsListener
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadForm));
            this.textBoxIp = new System.Windows.Forms.TextBox();
            this.labelIp = new System.Windows.Forms.Label();
            this.textBoxPuerto = new System.Windows.Forms.TextBox();
            this.labelPuerto = new System.Windows.Forms.Label();
            this.buttonEmpezar = new System.Windows.Forms.Button();
            this.labelPlayers = new System.Windows.Forms.Label();
            this.listViewRecordatorio = new System.Windows.Forms.ListView();
            this.ColumnHeaderServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonCerrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxIp
            // 
            this.textBoxIp.AutoCompleteCustomSource.AddRange(new string[] {
            resources.GetString("textBoxIp.AutoCompleteCustomSource"),
            resources.GetString("textBoxIp.AutoCompleteCustomSource1"),
            resources.GetString("textBoxIp.AutoCompleteCustomSource2"),
            resources.GetString("textBoxIp.AutoCompleteCustomSource3"),
            resources.GetString("textBoxIp.AutoCompleteCustomSource4")});
            this.textBoxIp.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.textBoxIp, "textBoxIp");
            this.textBoxIp.Name = "textBoxIp";
            // 
            // labelIp
            // 
            resources.ApplyResources(this.labelIp, "labelIp");
            this.labelIp.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelIp.Name = "labelIp";
            // 
            // textBoxPuerto
            // 
            this.textBoxPuerto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.textBoxPuerto, "textBoxPuerto");
            this.textBoxPuerto.Name = "textBoxPuerto";
            // 
            // labelPuerto
            // 
            resources.ApplyResources(this.labelPuerto, "labelPuerto");
            this.labelPuerto.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelPuerto.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelPuerto.Name = "labelPuerto";
            // 
            // buttonEmpezar
            // 
            resources.ApplyResources(this.buttonEmpezar, "buttonEmpezar");
            this.buttonEmpezar.Name = "buttonEmpezar";
            this.buttonEmpezar.UseVisualStyleBackColor = true;
            // 
            // labelPlayers
            // 
            resources.ApplyResources(this.labelPlayers, "labelPlayers");
            this.labelPlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPlayers.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelPlayers.Name = "labelPlayers";
            // 
            // listViewRecordatorio
            // 
            this.listViewRecordatorio.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewRecordatorio.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeaderServer});
            this.listViewRecordatorio.FullRowSelect = true;
            this.listViewRecordatorio.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewRecordatorio.HoverSelection = true;
            this.listViewRecordatorio.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listViewRecordatorio.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listViewRecordatorio.Items1"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listViewRecordatorio.Items2"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listViewRecordatorio.Items3")))});
            resources.ApplyResources(this.listViewRecordatorio, "listViewRecordatorio");
            this.listViewRecordatorio.MultiSelect = false;
            this.listViewRecordatorio.Name = "listViewRecordatorio";
            this.listViewRecordatorio.UseCompatibleStateImageBehavior = false;
            this.listViewRecordatorio.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeaderServer
            // 
            resources.ApplyResources(this.ColumnHeaderServer, "ColumnHeaderServer");
            // 
            // buttonCerrar
            // 
            this.buttonCerrar.BackColor = System.Drawing.Color.DarkRed;
            resources.ApplyResources(this.buttonCerrar, "buttonCerrar");
            this.buttonCerrar.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.buttonCerrar.FlatAppearance.BorderSize = 0;
            this.buttonCerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.buttonCerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonCerrar.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonCerrar.Name = "buttonCerrar";
            this.buttonCerrar.UseVisualStyleBackColor = false;
            // 
            // LoadForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.buttonCerrar);
            this.Controls.Add(this.listViewRecordatorio);
            this.Controls.Add(this.labelPlayers);
            this.Controls.Add(this.buttonEmpezar);
            this.Controls.Add(this.labelPuerto);
            this.Controls.Add(this.textBoxPuerto);
            this.Controls.Add(this.labelIp);
            this.Controls.Add(this.textBoxIp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadForm";
            this.ShowIcon = false;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxIp;
        private System.Windows.Forms.Label labelIp;
        private System.Windows.Forms.TextBox textBoxPuerto;
        private System.Windows.Forms.Label labelPuerto;
        private System.Windows.Forms.Button buttonEmpezar;
        private System.Windows.Forms.Label labelPlayers;
        private System.Windows.Forms.ListView listViewRecordatorio;
        private System.Windows.Forms.ColumnHeader ColumnHeaderServer;
        private Button buttonCerrar;

        #region ImplKeyEventListener
        private bool lDown = false, rDown = false;

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
        #endregion ImplKeyEventListener


    }
}