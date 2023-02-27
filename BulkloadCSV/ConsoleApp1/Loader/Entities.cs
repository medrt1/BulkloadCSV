using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public class PunchRecord
    {
        public Guid MachineID { get; set; }
        public Guid CardID { get; set; }
        public DateTime PunchIn { get; set; }
        public DateTime PunchOut { get; set; }
    }

    public class ImportPunchRecord
    {
        public Guid MachineID { get; set; }
        public Guid CardID { get; set; }
        public DateTime PunchIn { get; set; }
        public DateTime PunchOut { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public DateTime ImportTime { get; set; }
    }

}
