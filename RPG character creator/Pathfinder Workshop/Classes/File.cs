using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pathfinder_Workshop.Classes
{
    class File
    {
        /*
         * Description:
         * Provides the file api for communicating with the text file.
         */

        // Property Definitions
        private XmlDocument doc;

        // Accessor Methods
        public XmlDocument Doc { get => doc; set => doc = value; }

        // Methods

        public void FileClose()
        {
            // Closes the currently open file
        }

        public virtual void ConvertToClassData(XmlNode Node)
        {
            // Converts the generic data read from the text file into the class-specific data of each of the readable classes.
        }

        // Constructor
        public File()
        {
            // Creates the file
            doc = new XmlDocument();
        }

        public void FileOpen(string path)
        {
            // Opens the file from the provided path
            doc.Load(path);
        }

        public void ReadSectionAndAttribute()
        {
            // searches for a specified section in the text file and then searches for all of its attributes
        }

        public void FileReadSection(string section)
        {
            ConvertToClassData(doc.SelectSingleNode(section));
        }

        public void FileWrite()
        {
            // Writes to the specified file
        }

        public int FileGetElementCount(string id)
        {
            int total = 0;
            try
            {
                total = GetXmlNode(id).ChildNodes.Count;
            }
            catch
            {
                Console.WriteLine("Could not find any instances of {0}", id);
            }
            Console.WriteLine("Found {0} {1}", total, id);
            return total;
        }

        public XmlNode GetXmlNode(string id)
        {
            return doc.SelectSingleNode(@"Pathfinder/" + id);
        }
    }
}
