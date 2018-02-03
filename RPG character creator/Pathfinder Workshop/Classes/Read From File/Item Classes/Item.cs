using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pathfinder_Workshop.Classes.Item_Classes
{
    class Item : File
    {
        /*Description:
         * Describes any item in the game that a player can obtain.*/

        // Property Declarations
        private string name;
        private string description;

        // Accessor Methods
        public string Description { get => description; set => description = value; }
        public string Name { get => name; set => name = value; }

        // Constructor
        public Item(string name="", string description="")
        {
            this.name = name;
            this.description = description;
        }

        public override void ConvertToClassData(XmlNode Node)
        {
            base.ConvertToClassData(Node);
            Name = Node.Attributes[0].Value; // Assign the name to the item.

            // If there is a description tag, then use it, otherwise, it's not necessary.
            if (Node.HasChildNodes)
            {
                Description = Node.ChildNodes[0].InnerText;
            }
            Console.WriteLine("Generated a new item with name {0} and description: {1}", Name, Description);
        }
    }
}
