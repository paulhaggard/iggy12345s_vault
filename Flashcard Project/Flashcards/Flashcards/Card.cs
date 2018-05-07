using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Flashcards
{
    public class Card
    {
        // A parent class that can be inherited from to create custom card types.

        private string question;
        private List<string> hint;
        private List<string> answers;
        private int correctPoints;
        private int incorrectPoints;

        public string Question { get => question; set => question = value; }
        public List<string> Hint { get => hint; set => hint = value; }
        public List<string> Answers { get => answers; set => answers = value; }
        public string Answer { get => answers[0]; set => answers[0] = value; }

        public Card(string question, List<string> answers, List<string> hint = null,
            int correctPoints = 1, int incorrectPoints = 0)
        {
            this.question = question;
            this.answers = answers;
            this.hint = hint ?? new List<string>();
            this.correctPoints = correctPoints;
            this.incorrectPoints = incorrectPoints;
        }

        public Card(string question = "", string answer = "", List<string> hint = null,
            int correctPoints = 1, int incorrectPoints = 0)
        {
            this.question = question;
            answers = new List<string> { answer };
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

        public virtual int GetScore(string answer)
        {
            // Returns the score value of this card given the current answer.
            return answers.Contains(answer) ? correctPoints : incorrectPoints;
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
