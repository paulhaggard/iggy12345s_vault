using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pathfinder_Workshop.Classes
{
    class SpecialAbility : File
    {
        /*
         * Description:
         * Describes a special ability in the game, can be read from the text file
         */

        // Property Definitions
        private string name;
        private string description;

        // Accessor Methods
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }

        // Constructor
        public SpecialAbility(string name="", string description="")
        {
            this.name = name;
            this.description = description;
        }

        public override void ConvertToClassData(XmlNode Node)
        {
            base.ConvertToClassData(Node);
            Name = Node.Attributes[0].Value; // Assign the name to the special ability.

            // If there is a description tag, then use it, otherwise, it's not necessary.
            if (Node.HasChildNodes)
            {
                Description = Node.ChildNodes[0].InnerText;
            }
            Console.WriteLine("Generated a new special ability with name {0} and description: {1}", Name, Description);
        }
    }
}
