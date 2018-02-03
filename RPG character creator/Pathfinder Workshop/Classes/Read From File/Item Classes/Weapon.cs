using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder_Workshop.Classes.Item_Classes
{
    class Weapon : Item
    {
        /*
         * Description:
         * Defines a weapon, can be read in from the text file.
         */

        // Property Definitions
        private int AttackBonus;
        private int Critical;
        private string Type;
        private int Range;
        private string Ammunition;
        private int Damage;

        // Accessor Methods
        public int AttackBonus1 { get => AttackBonus; set => AttackBonus = value; }
        public int Critical1 { get => Critical; set => Critical = value; }
        public string Type1 { get => Type; set => Type = value; }
        public int Range1 { get => Range; set => Range = value; }
        public string Ammunition1 { get => Ammunition; set => Ammunition = value; }
        public int Damage1 { get => Damage; set => Damage = value; }

        // Constructor
        public Weapon(string name, string description="Bird",int attackBonus=0,int critical=0,string type="",int range=0,string ammunition="",int damage=0)
            : base(name, description)
        {
            AttackBonus = attackBonus;
            Critical = critical;
            Type = type;
            Range = range;
            Ammunition = ammunition;
            Damage = damage;
        }
    }
}
