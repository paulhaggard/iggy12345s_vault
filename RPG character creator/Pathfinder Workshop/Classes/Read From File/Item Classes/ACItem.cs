using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder_Workshop.Classes.Item_Classes
{
    class ACItem : Item
    {
        // Description:
        /* Used to describe items that improve, or affect the character's Armour class */

        // Property declarations
        private int bonus;
        private string type;
        private int CheckPenalty;
        private int SpellFailure;
        private string Properties;

        // Accessor Methods
        public int Bonus { get => bonus;}
        public string Type { get => type;}
        public int CheckPenalty1 { get => CheckPenalty;}
        public int SpellFailure1 { get => SpellFailure;}
        public string Properties1 { get => Properties;}

        // Constructor
        public ACItem(string name, string description="", int bonus = 0, string type="", int CheckPenalty=0, int SpellFailure=0, string Properties="")
            : base(name, description) // Specifies the constructor arguments for the parent class (Item)
        {
            this.bonus = bonus;
            this.type = type;
            this.CheckPenalty = CheckPenalty;
            this.SpellFailure = SpellFailure;
            this.Properties = Properties;
        }
    }
}
