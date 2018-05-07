using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flashcards.Forms
{
    public partial class UserEntry : Form
    {
        private string text;

        public string TextEntry { get => text; set => text = value; }

        public delegate void FormFinishEventHandler(object sender, EventArgs e);

        public event FormFinishEventHandler FormFinish;

        protected void OnFormFinish()
        {
            FormFinish?.Invoke(this, new EventArgs());
        }

        public UserEntry(string label = "")
        {
            InitializeComponent();
            textLabel.Text = label;
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            OnFormFinish();
            Dispose();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            text = textBox.Text;
        }
    }
}
