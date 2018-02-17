using System;
using System.Collections.Generic;
using System.Text;

namespace PCGenPluginLibrary
{
    class PCGenStructs
    {
        public enum PCGenCharacterTypes { PC = 0, NPC, Monster}

        public struct SystemInformation
        {
            private string campaign;
            private string version;
            private string rollmethod;
            private bool purchasepoints;
            private PCGenCharacterTypes characterType;
            private string previewSheet;
            private int poolPoints;
            private int poolPointsAvail;
            private string gameMode;
            private string tabLabel;
            private bool autoSpells;
            private bool useHigherKnown;
            private bool useHigherPrepped;
            private bool loadCompanions;
            private bool useTempMods;
            private string skillsOutputOrder;
            private int skillFilter;
            private bool ignoreCost;
            private bool allowDebt;
            private bool autoResizeGear;

            public SystemInformation(string campaign, 
                string version, 
                string rollmethod, 
                bool purchasepoints, 
                PCGenCharacterTypes characterType, 
                string previewSheet, 
                int poolPoints, 
                int poolPointsAvail, 
                string gameMode, 
                string tabLabel, 
                bool autoSpells, 
                bool useHigherKnown, 
                bool useHigherPrepped, 
                bool loadCompanions, 
                bool useTempMods, 
                string skillsOutputOrder, 
                int skillFilter, 
                bool ignoreCost, 
                bool allowDebt, 
                bool autoResizeGear)
            {
                this.campaign = campaign;
                this.version = version;
                this.rollmethod = rollmethod;
                this.purchasepoints = purchasepoints;
                this.characterType = characterType;
                this.previewSheet = previewSheet;
                this.poolPoints = poolPoints;
                this.poolPointsAvail = poolPointsAvail;
                this.gameMode = gameMode;
                this.tabLabel = tabLabel;
                this.autoSpells = autoSpells;
                this.useHigherKnown = useHigherKnown;
                this.useHigherPrepped = useHigherPrepped;
                this.loadCompanions = loadCompanions;
                this.useTempMods = useTempMods;
                this.skillsOutputOrder = skillsOutputOrder;
                this.skillFilter = skillFilter;
                this.ignoreCost = ignoreCost;
                this.allowDebt = allowDebt;
                this.autoResizeGear = autoResizeGear;
            }

            public string Campaign { get => campaign; set => campaign = value; }
            public string Version { get => version; set => version = value; }
            public string Rollmethod { get => rollmethod; set => rollmethod = value; }
            public bool Purchasepoints { get => purchasepoints; set => purchasepoints = value; }
            public string PreviewSheet { get => previewSheet; set => previewSheet = value; }
            public int PoolPoints { get => poolPoints; set => poolPoints = value; }
            public int PoolPointsAvail { get => poolPointsAvail; set => poolPointsAvail = value; }
            public string GameMode { get => gameMode; set => gameMode = value; }
            public string TabLabel { get => tabLabel; set => tabLabel = value; }
            public bool AutoSpells { get => autoSpells; set => autoSpells = value; }
            public bool UseHigherKnown { get => useHigherKnown; set => useHigherKnown = value; }
            public bool UseHigherPrepped { get => useHigherPrepped; set => useHigherPrepped = value; }
            public bool LoadCompanions { get => loadCompanions; set => loadCompanions = value; }
            public bool UseTempMods { get => useTempMods; set => useTempMods = value; }
            public string SkillsOutputOrder { get => skillsOutputOrder; set => skillsOutputOrder = value; }
            public int SkillFilter { get => skillFilter; set => skillFilter = value; }
            public bool IgnoreCost { get => ignoreCost; set => ignoreCost = value; }
            public bool AllowDebt { get => allowDebt; set => allowDebt = value; }
            public bool AutoResizeGear { get => autoResizeGear; set => autoResizeGear = value; }
            public PCGenCharacterTypes CharacterType { get => characterType; set => characterType = value; }
        }

        public struct CharacterBio
        {
            private string characterName;
            private string tabName;
            private string playerName;
            private double height;
            private double weight;
            private long age;
            private string gender;
            private string handed;
            private string skinColor;
            private string eyeColor;
            private string hairColor;
            private string hairstyle;
            private string location;
            private string city;
            private string birthday;
            private string birthplace;
            private string personalityTrait;
            private string personalityTrait2;
            private string speechPattern;
            private string phobias;
            private string interests;
            private string catchphrase;
            private string portrait;

            public CharacterBio(string characterName, 
                string tabName, 
                string playerName, 
                double height, double weight, 
                long age, 
                string gender, 
                string handed, 
                string skinColor, string eyeColor, string hairColor, string hairstyle, 
                string location, string city, 
                string birthday, string birthplace, 
                string personalityTrait, string personalityTrait2, 
                string speechPattern, 
                string phobias, string interests, 
                string catchphrase, string portrait)
            {
                this.characterName = characterName;
                this.tabName = tabName;
                this.playerName = playerName;
                this.height = height;
                this.weight = weight;
                this.age = age;
                this.gender = gender;
                this.handed = handed;
                this.skinColor = skinColor;
                this.eyeColor = eyeColor;
                this.hairColor = hairColor;
                this.hairstyle = hairstyle;
                this.location = location;
                this.city = city;
                this.birthday = birthday;
                this.birthplace = birthplace;
                this.personalityTrait = personalityTrait;
                this.personalityTrait2 = personalityTrait2;
                this.speechPattern = speechPattern;
                this.phobias = phobias;
                this.interests = interests;
                this.catchphrase = catchphrase;
                this.portrait = portrait;
            }

            public string CharacterName { get => characterName; set => characterName = value; }
            public string TabName { get => tabName; set => tabName = value; }
            public string PlayerName { get => playerName; set => playerName = value; }
            public double Height { get => height; set => height = value; }
            public double Weight { get => weight; set => weight = value; }
            public long Age { get => age; set => age = value; }
            public string Gender { get => gender; set => gender = value; }
            public string Handed { get => handed; set => handed = value; }
            public string SkinColor { get => skinColor; set => skinColor = value; }
            public string EyeColor { get => eyeColor; set => eyeColor = value; }
            public string Hairstyle { get => hairstyle; set => hairstyle = value; }
            public string Location { get => location; set => location = value; }
            public string City { get => city; set => city = value; }
            public string Birthday { get => birthday; set => birthday = value; }
            public string Birthplace { get => birthplace; set => birthplace = value; }
            public string PersonalityTrait { get => personalityTrait; set => personalityTrait = value; }
            public string PersonalityTrait2 { get => personalityTrait2; set => personalityTrait2 = value; }
            public string SpeechPattern { get => speechPattern; set => speechPattern = value; }
            public string Phobias { get => phobias; set => phobias = value; }
            public string Interests { get => interests; set => interests = value; }
            public string Catchphrase { get => catchphrase; set => catchphrase = value; }
            public string Portrait { get => portrait; set => portrait = value; }
        }

        public struct CharacterAttributes
        {
            private int strength;
            private int dexterity;
            private int constitution;
            private int intelligence;
            private int wisdom;
            private int charisma;
            private string alignment;
            private string race;

            public CharacterAttributes(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma, string alignment, string race)
            {
                this.strength = strength;
                this.dexterity = dexterity;
                this.constitution = constitution;
                this.intelligence = intelligence;
                this.wisdom = wisdom;
                this.charisma = charisma;
                this.alignment = alignment;
                this.race = race;
            }

            public int Strength { get => strength; set => strength = value; }
            public int Dexterity { get => dexterity; set => dexterity = value; }
            public int Constitution { get => constitution; set => constitution = value; }
            public int Intelligence { get => intelligence; set => intelligence = value; }
            public int Wisdom { get => wisdom; set => wisdom = value; }
            public int Charisma { get => charisma; set => charisma = value; }
            public string Alignment { get => alignment; set => alignment = value; }
            public string Race { get => race; set => race = value; }
        }

        public struct CharacterClass
        {
            private string name;
            private int level;
            private int skillPool;
            private ClassAbilitiesLevel classAbilities;

            public CharacterClass(string name, int level, int skillPool, ClassAbilitiesLevel classAbilities)
            {
                this.name = name;
                this.level = level;
                this.skillPool = skillPool;
                this.classAbilities = classAbilities;
            }

            public string Name { get => name; set => name = value; }
            public int Level { get => level; set => level = value; }
            public int SkillPool { get => skillPool; set => skillPool = value; }
            internal ClassAbilitiesLevel ClassAbilities { get => classAbilities; set => classAbilities = value; }
        }

        public struct ClassAbilitiesLevel
        {
            private int level;
            private int hitpoints;
            private int skillsGained;
            private int skillsRemaining;
            private List<string> specialties;
            private Dictionary<string, int> prestats;

            public ClassAbilitiesLevel(int level, int hitpoints, int skillsGained, int skillsRemaining, 
                List<string> specialties, Dictionary<string, int> prestats)
            {
                this.level = level;
                this.hitpoints = hitpoints;
                this.skillsGained = skillsGained;
                this.skillsRemaining = skillsRemaining;
                this.specialties = specialties;
                this.prestats = prestats;
            }

            public int Level { get => level; set => level = value; }
            public int Hitpoints { get => hitpoints; set => hitpoints = value; }
            public int SkillsGained { get => skillsGained; set => skillsGained = value; }
            public int SkillsRemaining { get => skillsRemaining; set => skillsRemaining = value; }
            public List<string> Specialties { get => specialties; set => specialties = value; }
            public Dictionary<string, int> Prestats { get => prestats; set => prestats = value; }
        }

        public struct CharacterExperience
        {
            private long experience;
            private string experienceTable;

            public CharacterExperience(long experience, string experienceTable)
            {
                this.experience = experience;
                this.experienceTable = experienceTable;
            }

            public long Experience { get => experience; set => experience = value; }
            public string ExperienceTable { get => experienceTable; set => experienceTable = value; }
        }

        public struct CharacterRegion
        {
            private string region;

            public string Region { get => region; set => region = value; }

            public CharacterRegion(string region)
            {
                this.region = region;
            }
        }

        public struct CharacterSkills
        {
            private List<CharacterSkill> skills;

            internal List<CharacterSkill> Skills { get => skills; set => skills = value; }

            public CharacterSkills(List<CharacterSkill> skills)
            {
                this.skills = skills;
            }
        }

        public struct CharacterSkill
        {
            private string name;
            private int outputOrder;
            private CharacterSkillClassBought classBought;

            public CharacterSkill(string name, int outputOrder, CharacterSkillClassBought classBought)
            {
                this.name = name;
                this.outputOrder = outputOrder;
                this.classBought = classBought;
            }

            public string Name { get => name; set => name = value; }
            public int OutputOrder { get => outputOrder; set => outputOrder = value; }
            internal CharacterSkillClassBought ClassBought { get => classBought; set => classBought = value; }
        }

        public struct CharacterSkillClassBought
        {
            private string className;
            private double rank;
            private int cost;
            private bool classSkill;

            public CharacterSkillClassBought(string className, double rank, int cost, bool classSkill)
            {
                this.className = className;
                this.rank = rank;
                this.cost = cost;
                this.classSkill = classSkill;
            }

            public string ClassName { get => className; set => className = value; }
            public double Rank { get => rank; set => rank = value; }
            public int Cost { get => cost; set => cost = value; }
            public bool ClassSkill { get => classSkill; set => classSkill = value; }
        }

        public struct CharacterLanguages
        {
            private List<string> languages;

            public List<string> Languages { get => languages; set => languages = value; }

            public CharacterLanguages(List<string> languages)
            {
                this.languages = languages;
            }
        }

        public struct CharacterAbilities
        {
            private List<CharacterAbilitie> abilities;
            private List<CharacterUserPool> userPools;
        }

        public struct CharacterAbilitie
        {
            private string name;
            private string type;
            private string category;
            private string key;
            private List<string> appliedTo;
            private string keyType;
            private string description;

            public CharacterAbilitie(string name, string type, string category, string key, List<string> appliedTo, string keyType, string description)
            {
                this.name = name;
                this.type = type;
                this.category = category;
                this.key = key;
                this.appliedTo = appliedTo;
                this.keyType = keyType;
                this.description = description;
            }

            public string Type { get => type; set => type = value; }
            public string Category { get => category; set => category = value; }
            public string Key { get => key; set => key = value; }
            public List<string> AppliedTo { get => appliedTo; set => appliedTo = value; }
            public string KeyType { get => keyType; set => keyType = value; }
            public string Description { get => description; set => description = value; }
            public string Name { get => name; set => name = value; }
        }

        public struct CharacterUserPool
        {
            private string name;
            private double poolPoints;

            public CharacterUserPool(string name, double poolPoints)
            {
                this.name = name;
                this.poolPoints = poolPoints;
            }

            public string Name { get => name; set => name = value; }
            public double PoolPoints { get => poolPoints; set => poolPoints = value; }
        }

        public struct CharacterWeaponProficiencies
        {
            private List<string> weapons;

            public CharacterWeaponProficiencies(List<string> weapons)
            {
                this.weapons = weapons;
            }

            public List<string> Weapons { get => weapons; set => weapons = value; }
        }

        public struct CharacterInventory
        {
            private List<CharacterEquipment> equipment;
            private List<CharacterEquipmentSet> equipSet;
            private double calcEquipSet;

            public CharacterInventory(List<CharacterEquipment> equipment, List<CharacterEquipmentSet> equipSet, double calcEquipSet)
            {
                this.equipment = equipment;
                this.equipSet = equipSet;
                this.calcEquipSet = calcEquipSet;
            }

            public double CalcEquipSet { get => calcEquipSet; set => calcEquipSet = value; }
            internal List<CharacterEquipment> Equipment { get => equipment; set => equipment = value; }
            internal List<CharacterEquipmentSet> EquipSet { get => equipSet; set => equipSet = value; }
        }

        public struct CharacterEquipmentSet
        {
            private string location;
            private string id;
            private string value;
            private double quantity;

            public CharacterEquipmentSet(string location, string id, string value, double quantity)
            {
                this.location = location;
                this.id = id;
                this.value = value;
                this.quantity = quantity;
            }

            public string Location { get => location; set => location = value; }
            public string Id { get => id; set => id = value; }
            public string Value { get => value; set => this.value = value; }
            public double Quantity { get => quantity; set => quantity = value; }
        }

        public struct CharacterEquipment
        {
            private string name;
            private int outputOrder;
            private double cost;
            private double weight;
            private double quantity;
            private string note;
            private CharacterEquipmentCustomization customization;

            public CharacterEquipment(string name, int outputOrder, 
                double cost, double weight, double quantity, 
                string note, CharacterEquipmentCustomization customization)
            {
                this.name = name;
                this.outputOrder = outputOrder;
                this.cost = cost;
                this.weight = weight;
                this.quantity = quantity;
                this.note = note;
                this.customization = customization;
            }

            public string Name { get => name; set => name = value; }
            public int OutputOrder { get => outputOrder; set => outputOrder = value; }
            public double Cost { get => cost; set => cost = value; }
            public double Weight { get => weight; set => weight = value; }
            public double Quantity { get => quantity; set => quantity = value; }
            public string Note { get => note; set => note = value; }
            internal CharacterEquipmentCustomization Customization { get => customization; set => customization = value; }
        }

        public struct CharacterEquipmentCustomization
        {
            private string baseItem;
            private string data;

            public CharacterEquipmentCustomization(string baseItem, string data)
            {
                this.baseItem = baseItem;
                this.data = data;
            }

            public string BaseItem { get => baseItem; set => baseItem = value; }
            public string Data { get => data; set => data = value; }
        }

        public struct TemporaryBonuses
        {
            private List<string> tempBonuses;
            private List<string> tempEquipSetBonuses;

            public TemporaryBonuses(List<string> tempBonuses, List<string> tempEquipSetBonuses)
            {
                this.tempBonuses = tempBonuses;
                this.tempEquipSetBonuses = tempEquipSetBonuses;
            }

            public List<string> TempBonuses { get => tempBonuses; set => tempBonuses = value; }
            public List<string> TempEquipSetBonuses { get => tempEquipSetBonuses; set => tempEquipSetBonuses = value; }
        }

        public struct CharacterDeitiesDomain
        {
            private List<Deity> deities;
            private List<Domain> domains;

            public CharacterDeitiesDomain(List<Deity> deities, List<Domain> domains)
            {
                this.deities = deities;
                this.domains = domains;
            }

            internal List<Deity> Deities { get => deities; set => deities = value; }
            internal List<Domain> Domains { get => domains; set => domains = value; }
        }

        public struct Domain
        {
            private string name;
            private string grants;
            private DomainSource source;

            public Domain(string name, string grants, DomainSource source)
            {
                this.name = name;
                this.grants = grants;
                this.source = source;
            }

            public string Name { get => name; set => name = value; }
            public string Grants { get => grants; set => grants = value; }
            internal DomainSource Source { get => source; set => source = value; }
        }

        public struct DomainSource
        {
            private string type;
            private string name;
            private int level;

            public DomainSource(string type, string name, int level)
            {
                this.type = type;
                this.name = name;
                this.level = level;
            }

            public string Type { get => type; set => type = value; }
            public string Name { get => name; set => name = value; }
            public int Level { get => level; set => level = value; }
        }

        public struct Deity
        {
            private string name;
            private List<string> domains;

            public Deity(string name, List<string> domains)
            {
                this.name = name;
                this.domains = domains;
            }

            public string Name { get => name; set => name = value; }
            public List<string> Domains { get => domains; set => domains = value; }
        }

        public struct CharacterSpellsInformation
        {

        }
    }
}
