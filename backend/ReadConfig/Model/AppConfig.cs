using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadConfig
{
    public struct AppConfig
    {
        public string StationID { get; set; }
        public string ImportPath { get; set; }
        public string BackupPath { get; set; }
        public string LogPath { get; set; }
        public string ImagePath { get; set; }
        public string LastImportNumber { get; set; }

    }
}
