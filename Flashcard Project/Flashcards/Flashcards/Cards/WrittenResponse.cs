using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Cards
{
    public class WrittenResponse : Card
    {
        private string answer;

        public WrittenResponse(string question = "", string answer = "", List<string> hints = null)
            :base(question, hints)
        {
            this.answer = answer;
        }

        public string Answer { get => answer; set => answer = value; }

        protected override bool IsCorrect(object answer)
        {
            return (string)answer == Answer;
        }
    }
}
