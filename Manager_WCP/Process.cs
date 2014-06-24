using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Manager_WCP
{
	public partial class Process : Form
	{
		
		public Process()
		{
			InitializeComponent();
			bgw2.RunWorkerAsync();			
		}

		private void Process_FormClosed(object sender, FormClosedEventArgs e)
		{
            Manager_WCP.active = false;
		}

		private void lProc_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (lProc.SelectedIndices[0] != -1)
            {
                //switch (lProc.SelectedIndices[0])
                //{
                    
                //}
            }            
        }

		private void bgw2_DoWork(object sender, DoWorkEventArgs e)
		{
            ThreadPool.QueueUserWorkItem(_ => lProc.BeginUpdate());            
		}        
	}
}
