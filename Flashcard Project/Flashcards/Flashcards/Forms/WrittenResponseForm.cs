using Flashcards.Cards;
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
    public partial class WrittenResponseForm : Form
    {
        private WrittenResponse card;

        public WrittenResponse Card { get => card; set => card = value; }

        public delegate void FormFinishEventHandler(object sender, EventArgs e);

        public event FormFinishEventHandler FormFinish;

        protected void OnFormFinish()
        {
            FormFinish?.Invoke(this, new EventArgs());
        }

        public WrittenResponseForm()
        {
            InitializeComponent();
            card = new WrittenResponse();
        }

        private void addHintBtn_Click(object sender, EventArgs e)
        {
            UserEntry temp = new UserEntry("Hint:");
            temp.FormFinish += Temp_FormFinish;
            temp.Show();
        }

        private void Temp_FormFinish(object sender, EventArgs e)
        {
            UserEntry temp = (UserEntry)sender;
            Card.AddHint(temp.TextEntry);
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            OnFormFinish();
            Dispose();
        }

        private void questionText_TextChanged(object sender, EventArgs e)
        {
            Card.Question = questionText.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Card.Answer = textBox1.Text;
        }

        private void removeHintBtn_Click(object sender, EventArgs e)
        {
            Card.Hint.RemoveAt(hintList.SelectedIndices[0]);
            hintList.Items.RemoveAt(hintList.SelectedIndices[0]);
            hintList.Invalidate();
        }
    }
}
