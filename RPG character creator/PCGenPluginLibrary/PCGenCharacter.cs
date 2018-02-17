using System;
using System.Collections.Generic;
using System.Text;

namespace PCGenPluginLibrary
{
    class PCGenCharacter : PCGenFile
    {
        private List<string> headers;
        private List<List<string>> sectionInfo;

        public PCGenCharacter(string filepath) : base(filepath)
        {
            headers = getHeaderList();
            sectionInfo = getSectionInfo();
        }

        public override void GenObject()
        {
            
        }
    }
}
