using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Manager_WCP
{
	public partial class settings : Form
    {
        Manager_WCP global = new Manager_WCP();
		
        public settings()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            
			Manager_WCP.pSyncTime = (checkTime.Checked) ? true : false;
			Manager_WCP.pTaskMgrOff = (checkTmgr.Checked) ? true : false;
			Manager_WCP.pBlockUsb = (checkUsb.Checked) ? true : false;
			Manager_WCP.pBlockKeyBoard = (checkBlockKeyBoard.Checked) ? true : false;
			Manager_WCP.pKillProc = (checkKillProcess.Checked) ? true : false;
            ThreadPool.QueueUserWorkItem(_ => global.Send("setting:" + 
				Manager_WCP.pSyncTime + "/" + 
				Manager_WCP.pTaskMgrOff + "/" + 
				Manager_WCP.pBlockUsb + "/" + 
				Manager_WCP.pBlockKeyBoard + "/" + 
				Manager_WCP.pKillProc));
			

            Manager_WCP.active = false;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Manager_WCP.active = false;
            this.Close();
        }

        private void settings_FormClosed(object sender, FormClosedEventArgs e)
        {
			Manager_WCP.active = false;
        }
    }
}
