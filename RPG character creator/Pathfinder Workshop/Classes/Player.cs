using Pathfinder_Workshop.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder_Workshop
{
    class Player
    {
        private string name;
        private List<Character> characters;

        public string Name { get => name; set => name = value; }
        public List<Character> Characters { get => characters; set => characters = value; }

        // Constructor
        public Player(string name, Character[] characters = null)
        {
            this.name = name;
            if (characters != null)
            {
                foreach(Character tempchar in characters)
                {
                    Characters.Add(tempchar);
                }
            }
            Console.WriteLine("Created player {0}!", Name);
        }
    }
}
