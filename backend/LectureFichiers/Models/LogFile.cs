using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireFichiers.Models
{
    public struct LogFile
    {
        public DateTime TimeStamp { get; set; }
        public string Description { get; set; }

        public LogFile(DateTime now, string msg) : this()
        {
            TimeStamp = now;
            Description = msg;
        }
    }
}
