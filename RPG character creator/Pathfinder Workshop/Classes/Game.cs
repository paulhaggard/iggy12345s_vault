using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Pathfinder_Workshop.Classes.Item_Classes;

namespace Pathfinder_Workshop.Classes
{
    static class Game
    {
        /*
         * Description:
         * Holds all of the information for the entire game, the GM will interface with this directly.
         */

        // Property Definitions
        private static List<Player> players;
        private static Player selectedPlayer;
        private static MasterlistCluster MasterList;
        private static File Database;
        private static Thread EventThread;
        
        // Accessor Methods
        public static List<Player> Players { get => players; set => players = value; }
        public static MasterlistCluster MasterList1 { get => MasterList;}
        public static Player SelectedPlayer { get => selectedPlayer; set => selectedPlayer = value; }

        // Constructor
        public static void Initialize()
        {
            // Initializes the player list
            players = new List<Player>();

            // Initializes the commandQueue
            Threadhelper.MainForm = Program.MainForm; // Initializes the threadhelper class
            EventThread = new Thread(new ThreadStart(EventManager)); // Launches a parallel process that runs the event manager
            EventThread.Start();

            // Loads the xml file into memory
            Database = new File();
            Database.FileOpen(@"G:\Projects\Software Projects\C#\Pathfinder Workshop\Pathfinder Workshop\Pathfinder Workshop\Database.xml");
            XmlDocument doc = Database.Doc;
            MasterList = new MasterlistCluster();
            Console.WriteLine("Loaded the database file successfully!");

            // Initializes the master lists
            MasterList.InitializeLists(
                Database.FileGetElementCount("Languages"),
                Database.FileGetElementCount("Spells"),
                Database.FileGetElementCount("Races"),
                Database.FileGetElementCount("Classes"),
                Database.FileGetElementCount("SpecialAbilities"),
                Database.FileGetElementCount("Feats"),
                Database.FileGetElementCount("Items"));
            Console.WriteLine("Initialized the MasterLists successfully!");

            // Initialize Languages
            if(Database.FileGetElementCount("Languages") > 0)
            {
                XmlNode Sublist = Database.GetXmlNode("Languages");
                foreach (XmlNode Node in Sublist.ChildNodes)
                {
                    Language Temp = new Language(); // Creates a new language
                    Temp.ConvertToClassData(Node);
                    MasterList.Languages.Add(Temp);
                    Console.WriteLine("ChildNode of Languages id: {0}", Node.Name);
                }
            }
            else
            {
                Console.WriteLine("Did not find any Languages to Initialize.");
            }

            // Initialize Spells
            if (Database.FileGetElementCount("Spells") > 0)
            {
                XmlNode Sublist = Database.GetXmlNode("Spells");
                foreach (XmlNode Node in Sublist.ChildNodes)
                {
                    Spell Temp = new Spell(); // Creates a new Spell
                    Temp.ConvertToClassData(Node);
                    MasterList.Spells.Add(Temp);
                    Console.WriteLine("ChildNode of Spells id: {0}", Node.Name);
                }
            }
            else
            {
                Console.WriteLine("Did not find any Spells to Initialize.");
            }

            // Initialize feats
            if (Database.FileGetElementCount("Feats") > 0)
            {
                XmlNode Sublist = Database.GetXmlNode("Feats");
                foreach (XmlNode Node in Sublist.ChildNodes)
                {
                    Feat Temp = new Feat(); // Creates a new feat
                    Temp.ConvertToClassData(Node);
                    MasterList.Feats.Add(Temp);
                    Console.WriteLine("ChildNode of Feats id: {0}", Node.Name);
                }
            }
            else
            {
                Console.WriteLine("Did not find any Feats to Initialize.");
            }

            // Initialize Special Abilities
            if (Database.FileGetElementCount("SpecialAbilities") > 0)
            {
                XmlNode Sublist = Database.GetXmlNode("SpecialAbilities");
                foreach (XmlNode Node in Sublist.ChildNodes)
                {
                    SpecialAbility Temp = new SpecialAbility(); // Creates a new special ability
                    Temp.ConvertToClassData(Node);
                    MasterList.SpecialAbilities.Add(Temp);
                    Console.WriteLine("ChildNode of Special Abilities id: {0}", Node.Name);
                }
            }
            else
            {
                Console.WriteLine("Did not find any Special Abilities to Initialize.");
            }

            // Initialize Items
            if (Database.FileGetElementCount("Items") > 0)
            {
                XmlNode Sublist = Database.GetXmlNode("Items");
                foreach (XmlNode Node in Sublist.ChildNodes)
                {
                    Item Temp = new Item(); // Creates a new item
                    Temp.ConvertToClassData(Node);
                    MasterList.Items.Add(Temp);
                    Console.WriteLine("ChildNode of Items id: {0}", Node.Name);
                }
            }
            else
            {
                Console.WriteLine("Did not find any Items to Initialize.");
            }

            // Initialize Races
            if (Database.FileGetElementCount("Races") > 0)
            {
                XmlNode Sublist = Database.GetXmlNode("Races");
                foreach (XmlNode Node in Sublist.ChildNodes)
                {
                    Race Temp = new Race(); // Creates a new race
                    Temp.ConvertToClassData(Node);
                    MasterList.Races.Add(Temp);
                    Console.WriteLine("ChildNode of Races id: {0}", Node.Name);
                }
            }
            else
            {
                Console.WriteLine("Did not find any Races to Initialize.");
            }

            // Initialize Classes
            if (Database.FileGetElementCount("Classes") > 0)
            {
                XmlNode Sublist = Database.GetXmlNode("Classes");
                foreach (XmlNode Node in Sublist.ChildNodes)
                {
                    Class Temp = new Class(); // Creates a new race
                    Temp.ConvertToClassData(Node);
                    MasterList.Classes.Add(Temp);
                    Console.WriteLine("ChildNode of Classes id: {0}", Node.Name);
                }
            }
            else
            {
                Console.WriteLine("Did not find any Classes to Initialize.");
            }
        }

        private static void EventManager()
        {
            bool exit = false;
            Console.WriteLine("Starting event manager poll...");
            while(!exit)
            {
                if(Program.CommandQueue.Count > 0)
                {
                    CommandSTYP temp = Program.CommandQueue.Dequeue();
                    switch(temp.Command)
                    {
                        case ("UpdatePlayers"):
                            // Updates the player listbox on the main form.
                            
                            break;

                        case ("PROG EXIT"):
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Unrecognized command: {0}", temp.Command);
                            break;
                    }
                }
            }
        }
    }
}
