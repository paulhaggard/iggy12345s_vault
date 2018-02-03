using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pathfinder_Workshop.Classes
{
    class Class : File
    {
        /*
         * Description:
         * Defines a class, which is contained by each character.
         */

        // Property Definition
        private string name;
        private string description;
        private string role;
        private string alignment;
        private string hitDice;
        private List<string> classSkills;
        private string skillRanksPerLevel;
        private List<string> weaponProficiencies;
        private List<string> armorProficiencies;
        private string movementModifier;

        // Accessor Methods
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Role { get => role; set => role = value; }
        public string Alignment { get => alignment; set => alignment = value; }
        public string HitDice { get => hitDice; set => hitDice = value; }
        public List<string> ClassSkills { get => classSkills; set => classSkills = value; }
        public string SkillRanksPerLevel { get => skillRanksPerLevel; set => skillRanksPerLevel = value; }
        public List<string> WeaponProficiencies { get => weaponProficiencies; set => weaponProficiencies = value; }
        public List<string> ArmorProficiencies { get => armorProficiencies; set => armorProficiencies = value; }
        public string MovementModifier { get => movementModifier; set => movementModifier = value; }

        // Constructor
        public Class(string name="", string description = "")
        {
            this.name = name;
            this.description = description;
            classSkills = new List<string>();
            weaponProficiencies = new List<string>();
            armorProficiencies = new List<string>();
        }

        public override void ConvertToClassData(XmlNode Node)
        {
            base.ConvertToClassData(Node);
            Name = Node.Attributes[0].Value; // Assign the name to the class.

            // If there is a description tag, then use it, otherwise, it's not necessary.
            if (Node.HasChildNodes)
            {
                foreach(XmlNode Child in Node.ChildNodes)
                {
                    switch(Child.Name)
                    {
                        // Controls how the description is added to the class.
                        case ("Description"):
                            Description = Child.InnerText;
                            break;

                        // Controls how the Role is added.
                        case ("Role"):
                            Role = Child.InnerText;
                            break;

                        // Controls how the Alignment is added.
                        case ("Alignment"):
                            Alignment = Child.InnerText;
                            break;

                        // Controls how the HitDice is added.
                        case ("HitDice"):
                            HitDice = Child.InnerText;
                            break;

                        // Controls how the ClassSkills are added.
                        case ("ClassSkills"):
                            foreach(XmlNode AttributeNode in Child.ChildNodes)
                            {
                                ClassSkills.Add(AttributeNode.Attributes[0].Value);
                            }
                            break;

                        // Controls how weapon proficiencies are added.
                        case ("WeaponProficiencies"):
                            foreach (XmlNode AttributeChild in Child.ChildNodes)
                            {
                                WeaponProficiencies.Add(AttributeChild.Attributes[0].Value);
                            }
                            break;

                        // Controls how armor proficiencies are added.
                        case ("ArmorProficiencies"):
                            foreach (XmlNode AttributeChild in Child.ChildNodes)
                            {
                                ArmorProficiencies.Add(AttributeChild.Attributes[0].Value);
                            }
                            break;

                        // Controls how the MovementModifier is added.
                        case ("MovementModifier"):
                            MovementModifier = Child.InnerText;
                            break;

                        // Controls how the Skill Ranks per level is added.
                        case ("SkillRanksPerLevel"):
                            SkillRanksPerLevel = Child.InnerText;
                            break;

                        default:
                            Console.WriteLine("Did not understand class attribute: {0}", Child.Name);
                            break;
                    }
                }
            }
            Console.WriteLine("Generated a new class with name {0} and description: {1}", Name, Description);
        }
    }
}
