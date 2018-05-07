﻿using System;
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
    public partial class DeckBuilder : Form
    {
        private Deck deck;

        public delegate void FormFinishEventHandler(object sender, FormFinishEventArgs e);

        public event FormFinishEventHandler FormFinish;

        protected void OnFormFinish()
        {
            FormFinish?.Invoke(this, new FormFinishEventArgs(loadChkBx.Checked, deck));
        }

        public DeckBuilder()
        {
            InitializeComponent();
            deck = new Deck();
        }

        private void okayBtn_Click(object sender, EventArgs e)
        {
            OnFormFinish();
            Dispose();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void addWritten_Click(object sender, EventArgs e)
        {
            WrittenResponseForm form = new WrittenResponseForm();
            form.FormFinish += written_FormFinish;
            form.Show();
        }

        private void written_FormFinish(object sender, EventArgs e)
        {
            WrittenResponseForm form = (WrittenResponseForm)sender;
            deck.Cards.Add(form.Card);
        }

        private void AddMultGuess_Click(object sender, EventArgs e)
        {

        }
    }

    public class FormFinishEventArgs : EventArgs
    {
        private bool load;
        private Deck deck;

        public Deck Deck { get => deck; set => deck = value; }
        public bool Load { get => load; set => load = value; }

        public FormFinishEventArgs(bool load, Deck d)
        {
            this.load = load;
            deck = d;
        }
    }
}
