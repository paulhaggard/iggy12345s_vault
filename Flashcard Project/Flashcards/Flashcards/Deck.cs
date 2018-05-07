using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Flashcards
{
    public class Deck
    {
        // A class used to seal a list of cards into collections.
        private string name;
        private List<Card> cards;

        public string Name { get => name; set => name = value; }
        public List<Card> Cards { get => cards; set => cards = value; }

        public Deck(string name = "")
        {
            this.name = name;
            cards = new List<Card>();
        }

        public virtual Card PickCard()
        {
            // Returns a card, selected at random.
            Random rng = new Random();
            return cards[rng.Next() % (cards.Count - 1)];
        }

        public virtual XElement ConvertToXml()
        {
            XElement temp = new XElement("Deck",
                new XAttribute("Name", name));
            XElement tempCards = new XElement("Cards");
            foreach (Card card in cards)
                tempCards.Add(card.ConvertToXml());
            temp.Add(tempCards);
            return temp;
        }
    }
}
