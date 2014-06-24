namespace Manager_WCP
{
	partial class Process
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
            this.lProc = new System.Windows.Forms.ListBox();
            this.bgw2 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // lProc
            // 
            this.lProc.FormattingEnabled = true;
            this.lProc.Location = new System.Drawing.Point(93, 10);
            this.lProc.Name = "lProc";
            this.lProc.Size = new System.Drawing.Size(187, 251);
            this.lProc.TabIndex = 3;
            this.lProc.SelectedIndexChanged += new System.EventHandler(this.lProc_SelectedIndexChanged);
            // 
            // bgw2
            // 
            this.bgw2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw2_DoWork);
            // 
            // Process
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.lProc);
            this.Name = "Process";
            this.Text = "Process";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Process_FormClosed);
            this.ResumeLayout(false);

		}

		#endregion

        public System.Windows.Forms.ListBox lProc;
        private System.ComponentModel.BackgroundWorker bgw2;
	}
}