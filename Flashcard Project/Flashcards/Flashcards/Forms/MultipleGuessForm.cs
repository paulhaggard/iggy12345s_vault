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
    public partial class MultipleGuessForm : Form
    {
        private MultipleGuess card;

        public MultipleGuessForm()
        {
            InitializeComponent();
        }

        private void addAnswerBtn_Click(object sender, EventArgs e)
        {
            UserEntry temp = new UserEntry("Answer:");
            temp.Show();
            temp.FormFinish += answerEntry_FormFinish;
        }

        private void answerEntry_FormFinish(object sender, EventArgs e)
        {
            UserEntry temp = (UserEntry)sender;
            
        }
    }
}
