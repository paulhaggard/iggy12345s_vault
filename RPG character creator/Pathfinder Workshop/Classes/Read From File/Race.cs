using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pathfinder_Workshop.Classes
{
    class Race : Character
    {
        /*
         * Description:
         * Each character has a race
         */

        // Property Definition
        private string name;
        private string description;
        private AbilityScores AbilityScoreModifiers;
        private string Size;
        private List<Language> Languages;
        private int speed;
        private List<SpecialAbility> specialAbilities;
        private List<string> weaponProficiencies;

        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Size1 { get => Size; set => Size = value; }
        public int Speed { get => speed; set => speed = value; }
        public List<string> WeaponProficiencies { get => weaponProficiencies; set => weaponProficiencies = value; }
        public AbilityScores AbilityScoreModifiers1 { get => AbilityScoreModifiers; set => AbilityScoreModifiers = value; }
        public List<Language> Languages1 { get => Languages; set => Languages = value; }
        public List<SpecialAbility> SpecialAbilities { get => specialAbilities; set => specialAbilities = value; }

        //Constructor
        public Race()
        {
            AbilityScoreModifiers = new AbilityScores();
            Languages = new List<Language>();
            specialAbilities = new List<SpecialAbility>();
            weaponProficiencies = new List<string>();
        }

        // Converts data read from the xml file into this classes specific data types.
        public override void ConvertToClassData(XmlNode Node)
        {
            base.ConvertToClassData(Node);
            name = Node.Attributes[0].Value;
            XmlNodeList ChildList = Node.ChildNodes;
            foreach(XmlNode Child in ChildList)
            {
                switch(Child.Name)
                {
                    // Controls how the descriptions are added.
                    case ("Adventurers"):
                    case ("AlignmentReligion"):
                    case ("Relations"):
                    case ("Society"):
                    case ("PhysicalDescription"):
                    case ("Description"):
                        description += Child.Name + ": " + Child.Value + @"\n";
                        Console.WriteLine("Added a description: {0}", Child.Value);
                        break;

                    // Controls how the male and female names are added to the Race's data.
                    case ("Names"):
                        List<string> maleNames = new List<string>();
                        List<string> femaleNames = new List<string>();
                        foreach (XmlNode NameChild in Child.ChildNodes)
                        {
                            if(NameChild.Attributes[0].Value == "Male")
                            {
                                maleNames.Add(NameChild.Value);
                            }
                            else
                            {
                                femaleNames.Add(NameChild.Value);
                            }
                            Console.WriteLine("Added a name: {0}", NameChild.Value);
                        }
                        Dictionary<string, List<string>> Temp = new Dictionary<string, List<string>>();
                        Temp.Add("Male", maleNames);
                        Temp.Add("Female", femaleNames);
                        Game.MasterList1.NameSuggestions.Add(name, Temp);
                        break;
                    
                    // Controls how the attribute modifiers are added.
                    case ("Attributes"):
                        foreach (XmlNode AttributeChild in Child.ChildNodes)
                        {
                            switch(AttributeChild.Name)
                            {
                                case ("Description"):
                                    description += "Attribute Description: " + AttributeChild.Value;
                                    break;
                                case ("Strength"):
                                    AbilityScoreModifiers.Strength1 = Convert.ToInt32(AttributeChild.InnerText);
                                    break;
                                case ("Dexterity"):
                                    AbilityScoreModifiers.Dexterity1 = Convert.ToInt32(AttributeChild.InnerText);
                                    break;
                                case ("Constitution"):
                                    AbilityScoreModifiers.Constitution1 = Convert.ToInt32(AttributeChild.InnerText);
                                    break;
                                case ("Wisdom"):
                                    AbilityScoreModifiers.Wisdom1 = Convert.ToInt32(AttributeChild.InnerText);
                                    break;
                                case ("Intelligence"):
                                    AbilityScoreModifiers.Intelligence1 = Convert.ToInt32(AttributeChild.InnerText);
                                    break;
                                case ("Charisma"):
                                    AbilityScoreModifiers.Charisma1 = Convert.ToInt32(AttributeChild.InnerText);
                                    break;
                                default:
                                    Console.WriteLine("Did not understand attribute modifier: {0}", AttributeChild.Name);
                                    break;
                            }
                        }
                        break;

                    // Controls how the size of the race is interpreted
                    case ("Size"):
                        Size = Child.InnerText;
                        break;

                    // Controls how languages are added.
                    case ("Languages"):
                        foreach (XmlNode AttributeChild in Child.ChildNodes)
                        {
                            string langName;
                            if(AttributeChild.Name != "Modifier")
                            {
                                langName = AttributeChild.Attributes[0].Value;
                                foreach (Language lang in Game.MasterList1.Languages)
                                {
                                    if (lang.Name == langName)
                                    {
                                        Languages.Add(lang);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Language lang = new Language("Modifier", AttributeChild.InnerText);
                                Languages.Add(lang);
                            }
                        }
                        break;

                    // Controls how the speed is added
                    case ("Speed"):
                        Speed = Convert.ToInt32(Child.InnerText);
                        break;

                    // Controls how special abilities are added.
                    case ("SpecialAbilities"):
                        foreach (XmlNode AttributeChild in Child.ChildNodes)
                        {
                            string abilityname = AttributeChild.Attributes[0].Value;
                            SpecialAbility Ability = new SpecialAbility(abilityname, AttributeChild.InnerText);
                            SpecialAbilities.Add(Ability);
                        }
                        break;

                    // Controls how weapon proficiencies are added.
                    case ("WeaponProficiencies"):
                        foreach(XmlNode AttributeChild in Child.ChildNodes)
                        {
                            WeaponProficiencies.Add(AttributeChild.Attributes[0].Value);
                        }
                        break;

                    default:
                        Console.WriteLine("Did not understand race attribute: {0}", Child.Name);
                        break;
                }
            }
        }
    }
}
