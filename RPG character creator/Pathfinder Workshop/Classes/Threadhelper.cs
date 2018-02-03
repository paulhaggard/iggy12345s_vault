using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pathfinder_Workshop.Classes
{
    public static class Threadhelper
    {
        delegate void UpdatePlayerListCallback();
        private static Main_host mainForm;

        public static Main_host MainForm { get => mainForm; set => mainForm = value; }

        public static void UpdatePlayerList()
        {
            if(mainForm.InvokeRequired)
            {
                UpdatePlayerListCallback d = new UpdatePlayerListCallback(UpdatePlayerList);
                mainForm.Invoke(d);
            }
        }
    }
}
