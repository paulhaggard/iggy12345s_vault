using System;
using System.Collections.Generic;
using System.IO;

namespace PCGenPluginLibrary
{
    public abstract class PCGenFile
    {
        /*
         * Provides functionality for reading from, and writing to, PCGen character files
         */

        private StreamReader reader;            // File handle
        private List<string> fileText;          // Raw text of the file

        // Constructor
        public PCGenFile(string filepath)
        {
            reader = new StreamReader(filepath);
            fileText = new List<string>();

            try
            {
                do
                {
                    fileText.Add(reader.ReadLine());    // Reads and appends all of the lines of text to the raw text list
                }
                while (reader.Peek() != -1);
            }
            catch
            {
                fileText.Add("File is empty");
            }

            finally
            {
                reader.Close();
            }
        }

        // Accessors
        public List<string> FileText { get => fileText; set => fileText = value; }

        public abstract void GenObject();

        // Methods
        protected List<string> getHeaderList()
        {
            // returns all of the headers from the file, (lines that start with '#')
            List<string> temp = new List<string>();
            for(int i = 0; i < fileText.Count; i++)
            {
                if (fileText[i].StartsWith('#'))
                    temp.Add(fileText[i]);
            }
            return temp;
        }

        protected List<List<string>> getSectionInfo()
        {
            // returns all of the lines of text below each of the headers in the file (lines that don't start with '#')
            List<List<string>> temp = new List<List<string>>();
            int header = 0;
            for(int i = 0; i < fileText.Count; i++)
            {
                if(fileText[i].StartsWith('#'))
                {
                    temp.Add(new List<string>());
                    int n = 1;
                    while(!fileText[i+n].Contains("#") && i + n < fileText.Count)
                    {
                        temp[header].Add(fileText[i + n++]);
                    }
                    i += n - 1;
                    header++;
                }
            }
            return temp;
        }
    }
}
