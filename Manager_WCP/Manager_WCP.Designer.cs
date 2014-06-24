namespace Manager_WCP
{
    partial class Manager_WCP
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
            this.components = new System.ComponentModel.Container();
            this.bupdateall = new System.Windows.Forms.Button();
            this.bsettings = new System.Windows.Forms.Button();
            this.brun = new System.Windows.Forms.Button();
            this.bproc = new System.Windows.Forms.Button();
            this.bgw = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.aPc = new System.Windows.Forms.Label();
            this.sPc = new System.Windows.Forms.Label();
            this.Subnet = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bupdateall
            // 
            this.bupdateall.Enabled = false;
            this.bupdateall.Location = new System.Drawing.Point(12, 12);
            this.bupdateall.Name = "bupdateall";
            this.bupdateall.Size = new System.Drawing.Size(80, 52);
            this.bupdateall.TabIndex = 0;
            this.bupdateall.Text = "Обновить настройки у всех ПК";
            this.bupdateall.UseVisualStyleBackColor = true;
            this.bupdateall.Click += new System.EventHandler(this.bUpdateall_Click);
            // 
            // bsettings
            // 
            this.bsettings.Enabled = false;
            this.bsettings.Location = new System.Drawing.Point(98, 12);
            this.bsettings.Name = "bsettings";
            this.bsettings.Size = new System.Drawing.Size(80, 23);
            this.bsettings.TabIndex = 2;
            this.bsettings.Text = "Настройки";
            this.bsettings.UseVisualStyleBackColor = true;
            this.bsettings.Click += new System.EventHandler(this.bSetting_Click);
            // 
            // brun
            // 
            this.brun.Enabled = false;
            this.brun.Location = new System.Drawing.Point(98, 41);
            this.brun.Name = "brun";
            this.brun.Size = new System.Drawing.Size(80, 23);
            this.brun.TabIndex = 3;
            this.brun.Text = "Выполноить";
            this.brun.UseVisualStyleBackColor = true;
            this.brun.Click += new System.EventHandler(this.bRun_Click);
            // 
            // bproc
            // 
            this.bproc.Enabled = false;
            this.bproc.Location = new System.Drawing.Point(184, 12);
            this.bproc.Name = "bproc";
            this.bproc.Size = new System.Drawing.Size(80, 23);
            this.bproc.TabIndex = 4;
            this.bproc.Text = "Процессы";
            this.bproc.UseVisualStyleBackColor = true;
            this.bproc.Click += new System.EventHandler(this.bProc_Click);
            // 
            // bgw
            // 
            this.bgw.WorkerReportsProgress = true;
            this.bgw.WorkerSupportsCancellation = true;
            this.bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_DoWork);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // aPc
            // 
            this.aPc.AutoSize = true;
            this.aPc.Location = new System.Drawing.Point(9, 82);
            this.aPc.Name = "aPc";
            this.aPc.Size = new System.Drawing.Size(108, 13);
            this.aPc.TabIndex = 5;
            this.aPc.Text = "Всего ПК в сети = 0";
            // 
            // sPc
            // 
            this.sPc.AutoSize = true;
            this.sPc.Location = new System.Drawing.Point(9, 98);
            this.sPc.Name = "sPc";
            this.sPc.Size = new System.Drawing.Size(161, 13);
            this.sPc.TabIndex = 6;
            this.sPc.Text = "Всего ПК в вашей подсети = 0";
            // 
            // Subnet
            // 
            this.Subnet.AutoSize = true;
            this.Subnet.Location = new System.Drawing.Point(9, 113);
            this.Subnet.Name = "Subnet";
            this.Subnet.Size = new System.Drawing.Size(59, 13);
            this.Subnet.TabIndex = 7;
            this.Subnet.Text = "Подсеть =";
            // 
            // Manager_WCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 134);
            this.Controls.Add(this.Subnet);
            this.Controls.Add(this.sPc);
            this.Controls.Add(this.aPc);
            this.Controls.Add(this.bproc);
            this.Controls.Add(this.brun);
            this.Controls.Add(this.bsettings);
            this.Controls.Add(this.bupdateall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Manager_WCP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manager panel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Button bupdateall;
        private System.Windows.Forms.Button bsettings;
        private System.Windows.Forms.Button brun;
        private System.Windows.Forms.Button bproc;
        public System.ComponentModel.BackgroundWorker bgw;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label aPc;
        private System.Windows.Forms.Label sPc;
        private System.Windows.Forms.Label Subnet;
    }
}

