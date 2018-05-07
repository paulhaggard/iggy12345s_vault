using Flashcards.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flashcards
{
    public partial class MainHost : Form
    {
        private DeckBuilder builderForm;
        private List<Deck> decks;

        public MainHost()
        {
            InitializeComponent();
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            builderForm = new DeckBuilder();
            builderForm.FormFinish += OnDeckBuilderFormFinish;
            builderForm.Show();
        }

        public virtual void OnDeckBuilderFormFinish(object sender, FormFinishEventArgs e)
        {
            if (e.Load)
            {
                deckList.Items.Add(e.Deck.Name);
                decks.Add(e.Deck);
            }
        }
    }
}
