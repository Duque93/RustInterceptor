namespace Rust_Interceptor.Forms
{
    partial class Controller
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
            this.trackBarZoom = new System.Windows.Forms.TrackBar();
            this.labelZoom = new System.Windows.Forms.Label();
            this.labelZoomValue = new System.Windows.Forms.Label();
            this.trackBarAngle = new System.Windows.Forms.TrackBar();
            this.labelAngle = new System.Windows.Forms.Label();
            this.trackBarYCrosshairOffset = new System.Windows.Forms.TrackBar();
            this.trackBarXCrosshairOffset = new System.Windows.Forms.TrackBar();
            this.labelXOffset = new System.Windows.Forms.Label();
            this.labelYOffset = new System.Windows.Forms.Label();
            this.buttonCerrar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYCrosshairOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarXCrosshairOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarZoom
            // 
            this.trackBarZoom.LargeChange = 50;
            this.trackBarZoom.Location = new System.Drawing.Point(13, 23);
            this.trackBarZoom.Maximum = 350;
            this.trackBarZoom.Name = "trackBarZoom";
            this.trackBarZoom.Size = new System.Drawing.Size(259, 45);
            this.trackBarZoom.SmallChange = 5;
            this.trackBarZoom.TabIndex = 0;
            this.trackBarZoom.TickFrequency = 5;
            this.trackBarZoom.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarZoom.Value = 150;
            this.trackBarZoom.ValueChanged += new System.EventHandler(this.trackBarZoom_ValueChanged);
            // 
            // labelZoom
            // 
            this.labelZoom.AutoSize = true;
            this.labelZoom.Location = new System.Drawing.Point(13, 4);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new System.Drawing.Size(34, 13);
            this.labelZoom.TabIndex = 1;
            this.labelZoom.Text = "Zoom";
            // 
            // labelZoomValue
            // 
            this.labelZoomValue.AutoSize = true;
            this.labelZoomValue.Location = new System.Drawing.Point(124, 58);
            this.labelZoomValue.Name = "labelZoomValue";
            this.labelZoomValue.Size = new System.Drawing.Size(36, 13);
            this.labelZoomValue.TabIndex = 2;
            this.labelZoomValue.Text = "150 m";
            // 
            // trackBarAngle
            // 
            this.trackBarAngle.LargeChange = 1;
            this.trackBarAngle.Location = new System.Drawing.Point(13, 501);
            this.trackBarAngle.Maximum = 360;
            this.trackBarAngle.Minimum = -360;
            this.trackBarAngle.Name = "trackBarAngle";
            this.trackBarAngle.Size = new System.Drawing.Size(259, 45);
            this.trackBarAngle.TabIndex = 3;
            this.trackBarAngle.TickFrequency = 45;
            this.trackBarAngle.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarAngle.Visible = false;
            this.trackBarAngle.ValueChanged += new System.EventHandler(this.trackBarAngle_ValueChanged);
            // 
            // labelAngle
            // 
            this.labelAngle.AutoSize = true;
            this.labelAngle.Location = new System.Drawing.Point(124, 533);
            this.labelAngle.Name = "labelAngle";
            this.labelAngle.Size = new System.Drawing.Size(20, 13);
            this.labelAngle.TabIndex = 4;
            this.labelAngle.Text = "0 º";
            this.labelAngle.Visible = false;
            // 
            // trackBarYCrosshairOffset
            // 
            this.trackBarYCrosshairOffset.Location = new System.Drawing.Point(17, 391);
            this.trackBarYCrosshairOffset.Minimum = -1000;
            this.trackBarYCrosshairOffset.Name = "trackBarYCrosshairOffset";
            this.trackBarYCrosshairOffset.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarYCrosshairOffset.Size = new System.Drawing.Size(45, 104);
            this.trackBarYCrosshairOffset.TabIndex = 5;
            this.trackBarYCrosshairOffset.Value = -13;
            this.trackBarYCrosshairOffset.Visible = false;
            this.trackBarYCrosshairOffset.ValueChanged += new System.EventHandler(this.trackBarYCrosshairOffset_ValueChanged);
            // 
            // trackBarXCrosshairOffset
            // 
            this.trackBarXCrosshairOffset.Location = new System.Drawing.Point(68, 391);
            this.trackBarXCrosshairOffset.Name = "trackBarXCrosshairOffset";
            this.trackBarXCrosshairOffset.Size = new System.Drawing.Size(189, 45);
            this.trackBarXCrosshairOffset.TabIndex = 6;
            this.trackBarXCrosshairOffset.Visible = false;
            this.trackBarXCrosshairOffset.ValueChanged += new System.EventHandler(this.trackBarXCrosshairOffset_ValueChanged);
            // 
            // labelXOffset
            // 
            this.labelXOffset.AutoSize = true;
            this.labelXOffset.Location = new System.Drawing.Point(141, 423);
            this.labelXOffset.Name = "labelXOffset";
            this.labelXOffset.Size = new System.Drawing.Size(27, 13);
            this.labelXOffset.TabIndex = 7;
            this.labelXOffset.Text = "0 px";
            this.labelXOffset.Visible = false;
            // 
            // labelYOffset
            // 
            this.labelYOffset.AutoSize = true;
            this.labelYOffset.Location = new System.Drawing.Point(21, 391);
            this.labelYOffset.Name = "labelYOffset";
            this.labelYOffset.Size = new System.Drawing.Size(36, 13);
            this.labelYOffset.TabIndex = 8;
            this.labelYOffset.Text = "-13 px";
            this.labelYOffset.Visible = false;
            // 
            // buttonCerrar
            // 
            this.buttonCerrar.BackColor = System.Drawing.Color.DarkRed;
            this.buttonCerrar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonCerrar.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.buttonCerrar.FlatAppearance.BorderSize = 0;
            this.buttonCerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.buttonCerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCerrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCerrar.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonCerrar.Location = new System.Drawing.Point(258, 5);
            this.buttonCerrar.Name = "buttonCerrar";
            this.buttonCerrar.Size = new System.Drawing.Size(15, 15);
            this.buttonCerrar.TabIndex = 9;
            this.buttonCerrar.Text = "X";
            this.buttonCerrar.UseVisualStyleBackColor = false;
            // 
            // Controller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 76);
            this.Controls.Add(this.buttonCerrar);
            this.Controls.Add(this.labelYOffset);
            this.Controls.Add(this.labelXOffset);
            this.Controls.Add(this.trackBarXCrosshairOffset);
            this.Controls.Add(this.trackBarYCrosshairOffset);
            this.Controls.Add(this.labelAngle);
            this.Controls.Add(this.trackBarAngle);
            this.Controls.Add(this.labelZoomValue);
            this.Controls.Add(this.labelZoom);
            this.Controls.Add(this.trackBarZoom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Controller";
            this.ShowIcon = false;
            this.Text = "Controller";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYCrosshairOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarXCrosshairOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelZoom;
        private System.Windows.Forms.Label labelZoomValue;
        public System.Windows.Forms.TrackBar trackBarZoom;
        public System.Windows.Forms.TrackBar trackBarAngle;
        private System.Windows.Forms.Label labelAngle;
        private System.Windows.Forms.TrackBar trackBarYCrosshairOffset;
        private System.Windows.Forms.TrackBar trackBarXCrosshairOffset;
        private System.Windows.Forms.Label labelXOffset;
        private System.Windows.Forms.Label labelYOffset;
        private System.Windows.Forms.Button buttonCerrar;
    }
}