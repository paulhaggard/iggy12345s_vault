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
using System.Xml.Linq;

namespace Flashcards
{
    public partial class MainHost : Form
    {
        private DeckBuilder builderForm;
        private List<Deck> decks;           // List of decks available.
        private List<bool> loadedDecks;     // Flags stating whether the decks are loaded or not.

        public MainHost()
        {
            InitializeComponent();
            decks = new List<Deck>();
            loadedDecks = new List<bool>();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            builderForm = new DeckBuilder();
            builderForm.FormFinish += OnDeckBuilderFormFinish;
            builderForm.Show();
        }

        public virtual void OnDeckBuilderFormFinish(object sender, FormFinishEventArgs e)
        {
            decks.Add(e.Deck);
            loadedDecks.Add(e.Load);
            if (e.Load)
            {
                deckList.Items.Add(e.Deck.Name);
                deckList.Invalidate();
            }
        }

        public virtual XElement ConvertToXml()
        {
            // Converts the current deckset into the current xml schema.
            XElement temp = new XElement("FlashCardFile",
                new XAttribute("Version:", "1.0"));

            foreach (Deck d in decks)
                temp.Add(d.ConvertToXml());

            return temp;
        }

        public virtual void ConvertFromXml(XElement e)
        {
            decks = new List<Deck>();
            // TODO: FIGURE OUT HOW TO READ IN THE OBJECT FROM AN XML FILE.
        }

        public virtual void Save(string path)
        {
            // Saves the current deckset to the file.
            ConvertToXml().Save(path);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowser.ShowDialog();
            folderBrowser.Disposed += SaveFolderBrowser_Disposed;
        }

        private void SaveFolderBrowser_Disposed(object sender, EventArgs e)
        {
            Save(((FolderBrowserDialog)sender).SelectedPath);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowser.ShowDialog();
            folderBrowser.Disposed += LoadFolderBrowser_Disposed;
        }

        private void LoadFolderBrowser_Disposed(object sender, EventArgs e)
        {
            //Save(((FolderBrowserDialog)sender).SelectedPath);
        }
    }
}
