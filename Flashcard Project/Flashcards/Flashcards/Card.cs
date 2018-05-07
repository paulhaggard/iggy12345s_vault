using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Flashcards
{
    public abstract class Card
    {
        // A parent class that can be inherited from to create custom card types.

        private string question;
        private List<string> hint;
        //private abstract object answer;
        private int correctPoints;
        private int incorrectPoints;

        public string Question { get => question; set => question = value; }
        public List<string> Hint { get => hint; set => hint = value; }
        //public T Answer { get => answer; set => answer = value; }

        public Card(string question, List<string> hint = null,
            int correctPoints = 1, int incorrectPoints = 0)
        {
            this.question = question;
            //this.answer = answer;
            this.hint = hint ?? new List<string>();
            this.correctPoints = correctPoints;
            this.incorrectPoints = incorrectPoints;
        }

        public void AddHint(string hint)
        {
            // Adds a hint to the list of hints.
            this.hint.Add(hint);
        }

        public void RemoveHint(string hint)
        {
            // Removes a hint from the list of hints.
            if (this.hint.Contains(hint))
                this.hint.Remove(hint);
        }

        protected abstract bool IsCorrect(object answer);    // Determines if the supplied answer is correct.

        public virtual int GetScore(object answer)
        {
            // Returns the score value of this card given the current answer.
            return IsCorrect(answer) ? correctPoints : incorrectPoints;
        }

        public virtual XElement ConvertToXml()
        {
            XElement temp = new XElement("Card",
                new XAttribute("Correct", correctPoints),
                new XAttribute("Incorrect", incorrectPoints),
                new XElement("Question", question));

            XElement tempHints = new XElement("Hints");
            foreach (string h in hint)
                tempHints.Add(new XElement("Hint", h));

            temp.Add(tempHints);

            return temp;
        }
    }
}
