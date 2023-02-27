using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    
    public class Configs
    {
        public static string filePath = @"..\..\punchfile.csv";
        public static string Db =  @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Articles\BulkloadCSV\db.mdf;Integrated Security=True;Connect Timeout=30";
        public static string localDB = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=..\..\punchDB.mdf;Integrated Security=True;Connect Timeout=30";
    }
}
