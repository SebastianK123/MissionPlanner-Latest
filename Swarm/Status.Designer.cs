namespace MissionPlanner.Swarm
{
    partial class Status
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_gps = new System.Windows.Forms.Label();
            this.lbl_armed = new System.Windows.Forms.Label();
            this.lbl_mode = new System.Windows.Forms.Label();
            this.lbl_mav = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_guided = new System.Windows.Forms.Label();
            this.lbl_loc = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_spd = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.zedGraph_Probe = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "GPS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Armed";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Mode";
            // 
            // lbl_gps
            // 
            this.lbl_gps.AutoSize = true;
            this.lbl_gps.Location = new System.Drawing.Point(46, 64);
            this.lbl_gps.Name = "lbl_gps";
            this.lbl_gps.Size = new System.Drawing.Size(29, 13);
            this.lbl_gps.TabIndex = 3;
            this.lbl_gps.Text = "GPS";
            // 
            // lbl_armed
            // 
            this.lbl_armed.AutoSize = true;
            this.lbl_armed.Location = new System.Drawing.Point(46, 17);
            this.lbl_armed.Name = "lbl_armed";
            this.lbl_armed.Size = new System.Drawing.Size(37, 13);
            this.lbl_armed.TabIndex = 4;
            this.lbl_armed.Text = "Armed";
            // 
            // lbl_mode
            // 
            this.lbl_mode.AutoSize = true;
            this.lbl_mode.Location = new System.Drawing.Point(46, 40);
            this.lbl_mode.Name = "lbl_mode";
            this.lbl_mode.Size = new System.Drawing.Size(34, 13);
            this.lbl_mode.TabIndex = 5;
            this.lbl_mode.Text = "Mode";
            // 
            // lbl_mav
            // 
            this.lbl_mav.AutoSize = true;
            this.lbl_mav.Location = new System.Drawing.Point(3, 0);
            this.lbl_mav.Name = "lbl_mav";
            this.lbl_mav.Size = new System.Drawing.Size(29, 13);
            this.lbl_mav.TabIndex = 7;
            this.lbl_mav.Text = "GPS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Guided";
            // 
            // lbl_guided
            // 
            this.lbl_guided.Location = new System.Drawing.Point(46, 88);
            this.lbl_guided.MinimumSize = new System.Drawing.Size(81, 40);
            this.lbl_guided.Name = "lbl_guided";
            this.lbl_guided.Size = new System.Drawing.Size(81, 40);
            this.lbl_guided.TabIndex = 9;
            this.lbl_guided.Text = "0.00000,0.00000,0.00";
            // 
            // lbl_loc
            // 
            this.lbl_loc.Location = new System.Drawing.Point(46, 142);
            this.lbl_loc.MinimumSize = new System.Drawing.Size(81, 40);
            this.lbl_loc.Name = "lbl_loc";
            this.lbl_loc.Size = new System.Drawing.Size(81, 40);
            this.lbl_loc.TabIndex = 11;
            this.lbl_loc.Text = "0.00000,0.00000,0.00";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Location";
            // 
            // lbl_spd
            // 
            this.lbl_spd.Location = new System.Drawing.Point(46, 182);
            this.lbl_spd.Name = "lbl_spd";
            this.lbl_spd.Size = new System.Drawing.Size(81, 43);
            this.lbl_spd.TabIndex = 13;
            this.lbl_spd.Text = "0.00000 0.00000 0.00000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 182);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Probe";
            // 
            // zedGraph_Probe
            // 
            this.zedGraph_Probe.Location = new System.Drawing.Point(133, -1);
            this.zedGraph_Probe.Name = "zedGraph_Probe";
            this.zedGraph_Probe.ScrollGrace = 0D;
            this.zedGraph_Probe.ScrollMaxX = 0D;
            this.zedGraph_Probe.ScrollMaxY = 0D;
            this.zedGraph_Probe.ScrollMaxY2 = 0D;
            this.zedGraph_Probe.ScrollMinX = 0D;
            this.zedGraph_Probe.ScrollMinY = 0D;
            this.zedGraph_Probe.ScrollMinY2 = 0D;
            this.zedGraph_Probe.Size = new System.Drawing.Size(354, 226);
            this.zedGraph_Probe.TabIndex = 14;
            // 
            // Status
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.zedGraph_Probe);
            this.Controls.Add(this.lbl_spd);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lbl_loc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbl_guided);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl_mav);
            this.Controls.Add(this.lbl_mode);
            this.Controls.Add(this.lbl_armed);
            this.Controls.Add(this.lbl_gps);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Status";
            this.Size = new System.Drawing.Size(486, 224);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_gps;
        private System.Windows.Forms.Label lbl_armed;
        private System.Windows.Forms.Label lbl_mode;
        private System.Windows.Forms.Label lbl_mav;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_guided;
        private System.Windows.Forms.Label lbl_loc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_spd;
        private System.Windows.Forms.Label label8;
        private ZedGraph.ZedGraphControl zedGraph_Probe;
    }
}
