using System;
using Pathfinder_Workshop.Classes.Item_Classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder_Workshop.Classes
{
    // General STYPs
    struct DiceSTYP
    {
        private int d4;
        private int d6;
        private int d8;
        private int d10;
        private int d12;
        private int d20;

        public int D4 { get => d4; set => d4 = value; }
        public int D6 { get => d6; set => d6 = value; }
        public int D8 { get => d8; set => d8 = value; }
        public int D10 { get => d10; set => d10 = value; }
        public int D12 { get => d12; set => d12 = value; }
        public int D20 { get => d20; set => d20 = value; }
    }

    // Command type for the Command Queue
    struct CommandSTYP
    {
        private string command;
        private object data;

        public CommandSTYP(string command, object data = null)
        {
            this.command = command;
            this.data = data;
        }

        public string Command { get => command; set => command = value; }
        public object Data { get => data; set => data = value; }
    }


    // Game Class STYPS
    struct MasterlistCluster
    {
        // These lists are populated when the program starts or whenever the player refreshes it
        // They contain all of the character options for the game.
        private List<Language> languages;
        private List<Spell> spells;
        private List<Race> races;
        private List<Class> classes;
        private List<SpecialAbility> specialAbilities;
        private List<Feat> feats;
        private List<Item> items;
        private Dictionary<string, Dictionary<string, List<string>>> nameSuggestions;

        // Accessor Methods
        public List<Language> Languages { get => languages; set => languages = value; }
        public List<Spell> Spells { get => spells; set => spells = value; }
        public List<Race> Races { get => races; set => races = value; }
        public List<Class> Classes { get => classes; set => classes = value; }
        public List<SpecialAbility> SpecialAbilities { get => specialAbilities; set => specialAbilities = value; }
        public List<Feat> Feats { get => feats; set => feats = value; }
        public List<Item> Items { get => items; set => items = value; }
        public Dictionary<string, Dictionary<string, List<string>>> NameSuggestions { get => nameSuggestions; set => nameSuggestions = value; }

        // Constructor
        public void InitializeLists(int Lang = 0, int spell = 0, int race = 0, int classLen = 0, int Ability = 0, int feat = 0, int item = 0)
        {
            languages = new List<Language>();
            languages.Capacity = Lang;
            spells = new List<Spell>();
            spells.Capacity = spell;
            races = new List<Race>();
            races.Capacity = race;
            classes = new List<Class>();
            classes.Capacity = classLen;
            specialAbilities = new List<SpecialAbility>();
            specialAbilities.Capacity = Ability;
            feats = new List<Feat>();
            feats.Capacity = feat;
            items = new List<Item>();
            items.Capacity = item;
            nameSuggestions = new Dictionary<string, Dictionary<string, List<string>>>();
        }
    }


    // Character Class STYPs
    struct DescriptionsCluster
    {
        // Overall character descriptions
        private string name;
        private string Alignment;
        private int Level;
        private Int32 Experience;
        private Int32 ExperienceToNextLevel;
        private string Deity;
        private string Homeland;
        private Body bodyDescription;

        public string Name { get => name; set => name = value; }
        public string Alignment1 { get => Alignment; set => Alignment = value; }
        public int Level1 { get => Level; set => Level = value; }
        public int Experience1 { get => Experience; set => Experience = value; }
        public int ExperienceToNextLevel1 { get => ExperienceToNextLevel; set => ExperienceToNextLevel = value; }
        public string Deity1 { get => Deity; set => Deity = value; }
        public string Homeland1 { get => Homeland; set => Homeland = value; }
        public Body BodyDescription { get => bodyDescription; set => bodyDescription = value; }
    }

    struct Body
    {
        // Overall Body Description
        private string gender;
        private int age;
        private int height;
        private int weight;
        private string hair;
        private string eyes;

        public string Gender { get => gender; set => gender = value; }
        public int Age { get => age; set => age = value; }
        public int Height { get => height; set => height = value; }
        public int Weight { get => weight; set => weight = value; }
        public string Hair { get => hair; set => hair = value; }
        public string Eyes { get => eyes; set => eyes = value; }
    }

    struct Stats
    {
        // Overall Stats
        private Int32 Hitpoints;
        private int Initiative;
        // Ability Scores
        private AbilityScores Abilities;
        // Armor Classes
        private ArmorClasses ArmorClass;

        // Accessor Methods
        public int Hitpoints1 { get => Hitpoints; set => Hitpoints = value; }
        public int Initiative1 { get => Initiative; set => Initiative = value; }
        public AbilityScores Abilities1 { get => Abilities; set => Abilities = value; }
        public ArmorClasses ArmorClass1 { get => ArmorClass; set => ArmorClass = value; }
    }

    struct AbilityScores
    {
        // Ability Score Definitions
        private int Strength;
        private int Dexterity;
        private int Constitution;
        private int Intelligence;
        private int Wisdom;
        private int Charisma;

        // Accessor Methods
        public int Strength1 { get => Strength; set => Strength = value; }
        public int Dexterity1 { get => Dexterity; set => Dexterity = value; }
        public int Constitution1 { get => Constitution; set => Constitution = value; }
        public int Intelligence1 { get => Intelligence; set => Intelligence = value; }
        public int Wisdom1 { get => Wisdom; set => Wisdom = value; }
        public int Charisma1 { get => Charisma; set => Charisma = value; }
    }

    struct ArmorClasses
    {
        // General Armor Class Information
        private int Base;
        private int Touch;
        private int FlatFooted;
        private string Modifiers;
        private ACItemsCluster[] ACItems;

        // Accessor Methods
        public ACItemsCluster[] ACItems1 { get => ACItems; set => ACItems = value; }
        public int Touch1 { get => Touch; set => Touch = value; }
        public int FlatFooted1 { get => FlatFooted; set => FlatFooted = value; }
        public string Modifiers1 { get => Modifiers; set => Modifiers = value; }
        public int Base1 { get => Base; set => Base = value; }
    }

    struct ACItemsCluster
    {
        // cluster for the ACItemsCluster[] Array
        ACItem item;
        bool isEquipped;
    }

    struct SavingThrows
    {
        // Saving Throw Information
        private int fortitude;
        private int reflex;
        private int will;

        // Accessor Methods
        public int Fortitude { get => fortitude; set => fortitude = value; }
        public int Reflex { get => reflex; set => reflex = value; }
        public int Will { get => will; set => will = value; }
    }

    struct Combat
    {
        // Combat Information
        private int BaseAttackBonus;
        private int SpellResistance;
        private int CMB;
        private int CMD;
        private string Modifiers;
        private Weapon[] Weapons;

        // Accessor Methods
        public int BaseAttackBonus1 { get => BaseAttackBonus; set => BaseAttackBonus = value; }
        public int SpellResistance1 { get => SpellResistance; set => SpellResistance = value; }
        public int CMB1 { get => CMB; set => CMB = value; }
        public int CMD1 { get => CMD; set => CMD = value; }
        public string Modifiers1 { get => Modifiers; set => Modifiers = value; }
        public Weapon[] Weapons1 { get => Weapons; set => Weapons = value; }
    }

    struct SpellDataCluster
    {
        private SpellLevelDataCluster[] SpellLevelInfo;
        private string ConditionalModifiers;
        private Spell[][] Spells;

        public string ConditionalModifiers1 { get => ConditionalModifiers; set => ConditionalModifiers = value; }
        public Spell[][] Spells1 { get => Spells; set => Spells = value; }
        public SpellLevelDataCluster[] SpellLevelInfo1 { get => SpellLevelInfo; set => SpellLevelInfo = value; }
    }

    struct SpellLevelDataCluster
    {
        // Cluster definition for SpellData
        int spellsKnown;
        int spellsaveDC;
        int spellsperday;
        int bonusspells;
    }

    struct MoneyCluster
    {
        // Cluster definition for the money pouch
        private Int32 cp;
        private Int32 sp;
        private Int32 gp;
        private Int32 pp;

        // Accessor Methods
        public int Cp { get => cp; set => cp = value; }
        public int Sp { get => sp; set => sp = value; }
        public int Gp { get => gp; set => gp = value; }
        public int Pp { get => pp; set => pp = value; }
    }

    class SkillsCluster
    {
        // Skills Cluster, defines the skills section.
        private SkillCluster[] Skills;
        private string conditionalModifiers;

        // Accessor Methods
        public string ConditionalModifiers { get => conditionalModifiers; set => conditionalModifiers = value; }
        public SkillCluster[] Skills1 { get => Skills; set => Skills = value; }

        // Constructor
        public SkillsCluster()
        {
            // Allocates memory
            Skills = new SkillCluster[(int)SkillNames.UseMagicDevice];
            conditionalModifiers = "";
            // Initializes a case in Skills for each skill in the SkillNamesEnum
            for (int i = 0; i <= (int)SkillNames.UseMagicDevice; i++)
            {
                Skills[i] = new SkillCluster((SkillNames)i);
            }
        }
    }

    struct SkillCluster
    {
        // Skill Cluster
        private SkillNames name;
        private string secondaryName;
        private bool classSkill;
        private bool trainedOnly;
        private int bonus;

        // Accessor Methods
        public SkillNames Name { get => name; set => name = value; }
        public string SecondaryName { get => secondaryName; set => secondaryName = value; }
        public bool ClassSkill { get => classSkill; set => classSkill = value; }
        public bool TrainedOnly { get => trainedOnly; set => trainedOnly = value; }
        public int Bonus { get => bonus; set => bonus = value; }

        public SkillCluster(SkillNames name, string secondaryName = "", bool classSkill = false, bool trainedOnly = false, int bonus = 0)
        {
            this.name = name;
            this.secondaryName = secondaryName;
            this.classSkill = classSkill;
            this.trainedOnly = trainedOnly;
            this.bonus = bonus;
        }
    }

    // This is the enum for all skills in the game
    enum SkillNames : int
    {
        Acrobatics = 0, Appraise, Bluff, Climb, Craft, Diplomacy, DisableDevice, Disguise, EscapeArtist, Fly, HandleAnimal,
        Heal, Intimidate, Knowledge_Arcana, Knowledge_Dungeoneering, Knowledge_Engineering, Knowledge_Geography, Knowledge_History, Knowledge_Local,
        Knowledge_Nature, Knowledge_Nobility, Knowledge_Planes, Knowledge_Religion, Linguistics, Perception, Perform, Profession, Ride, SenseMotive,
        SleightOfHand, Spellcraft, Stealth, Survival, Swim, UseMagicDevice
    }
}
