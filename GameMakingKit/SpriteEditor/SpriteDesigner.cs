using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpriteEditor
{
    public partial class SpriteDesigner : Form
    {
        /// <summary>
        /// Drawing mode of the designer, true = pencil, false = eraser
        /// </summary>
        private bool drawingMode { get; set; } = true;

        public SpriteDesigner()
        {
            InitializeComponent();
            
        }



        private void toolStripButtonPencil_Click(object sender, EventArgs e)
        {
            drawingMode = true;
            toolStripButtonPencil.Checked = true;
            toolStripButtonEraser.Checked = false;
        }

        private void toolStripButtonEraser_Click(object sender, EventArgs e)
        {
            drawingMode = false;
            toolStripButtonPencil.Checked = false;
            toolStripButtonEraser.Checked = true;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void pictureBoxCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
