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
    public partial class Main_host : Form
    {
        private CharacterCreator CharacterDaemon;
        private PlayerCreator PlayerDaemon;

        public Main_host()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initializes the game object
            ProgramStatusLabel.Text = "Loading Data...";
            Game.Initialize();
            ProgramStatusLabel.Text = "Generating Daemons...";
            CharacterDaemon = new CharacterCreator();
            PlayerDaemon = new PlayerCreator(this);
            ProgramStatusLabel.Text = "Ready!";
        }

        private void characterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProgramStatusLabel.Text = "Launching Character Creation Daemon!";
            CharacterDaemon.Show();
            ProgramStatusLabel.Text = "Ready!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void playerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProgramStatusLabel.Text = "Launching Player Creation Daemon!";
            if (PlayerDaemon != null)
            {
                PlayerDaemon.Close();
            }
            PlayerDaemon = new PlayerCreator(this);
            PlayerDaemon.Show();
            ProgramStatusLabel.Text = "Ready!";
        }

        private void PlayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.SelectedPlayer = (Player)PlayerList.SelectedItem;
        }

        public virtual void UpdatePlayerList()
        {
            PlayerList.Items.Clear();
            foreach(Player temp in Game.Players)
            {
                PlayerList.Items.Add(temp);
                PlayerList.Update();
            }
        }

        private void Main_host_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.CommandQueue.Enqueue(new CommandSTYP("PROG EXIT"));
        }
    }
}
