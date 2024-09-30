using System;
using System.Collections.Generic;
using System.Text;

namespace DataIO
{
    public class FileIOSettings
    {
        public bool OverwriteFile { get; set; }
        public bool IsProtected { get; set; }
        public FileIOSettings() 
        {
            OverwriteFile = true;
            IsProtected = false;
        }
    }
}
