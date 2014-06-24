using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Manager_WCP;
using System.Threading;

namespace Manager_WCP
{
    public partial class run : Form
    {
        Manager_WCP global = new Manager_WCP();
        public string used;
        public run()
        {
            InitializeComponent();
            pUse.Items.Add("Перезагрузка");
            pUse.Items.Add("Перезагрузка(HARD)");
            pUse.Items.Add("Выключение");
            pUse.Items.Add("Выключение(HARD)");
			pUse.Items.Add("Блокировка ОС");
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            //global.Send(used);
            ThreadPool.QueueUserWorkItem(_ => global.Send(used));
            Manager_WCP.active = false;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Manager_WCP.active = false;
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pUse.SelectedIndices[0] != -1)
            {
				switch (pUse.SelectedIndices[0])
				{
					case 0: { used = "param:reboot"; } break;
					case 1: { used = "param:hreboot"; } break;
					case 2: { used = "param:halt"; } break;
					case 3: { used = "param:hhalt"; } break;
					case 4: { used = "param:lockos"; } break;
				}
            }
        }

		private void run_FormClosed(object sender, FormClosedEventArgs e)
		{
			Manager_WCP.active = false;
		}
    }
}
