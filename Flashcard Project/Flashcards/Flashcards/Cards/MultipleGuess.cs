using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Cards
{
    class MultipleGuess : Card
    {
        private List<Tuple<char, string>> answers;
        private char answer;

        public MultipleGuess(string question,
            List<Tuple<char, string>> answers, char answer,
            List<string> hints = null, int correctPoints = 1, int incorrectPoints = 0)
            :base(question, hints, correctPoints, incorrectPoints)
        {
            this.answers = answers;
            this.answer = answer;
        }

        public List<Tuple<char, string>> Choices { get => answers; set => answers = value; }
        public char Answer { get => answer; set => answer = value; }

        protected override bool IsCorrect(object answer)
        {
            return (char)answer == Answer;
        }
    }
}
