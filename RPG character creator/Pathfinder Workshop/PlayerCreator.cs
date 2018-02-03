using Pathfinder_Workshop.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pathfinder_Workshop
{
    public partial class PlayerCreator : Form
    {
        private Main_host Caller;
        public PlayerCreator(Main_host caller)
        {
            Caller = (Main_host)caller;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Game.Players.Add(new Player(PlayerName.Text));
            Close();
        }

        private void PlayerCreator_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.CommandQueue.Enqueue(new CommandSTYP("UpdatePlayers"));
        }
    }
}
