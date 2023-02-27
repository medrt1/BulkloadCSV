using CsvHelper;
using NUnit.Framework;
using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    //https://stackoverflow.com/questions/194863/random-date-in-c-sharp
    class RandomDateTime
    {
        static Random gen = new Random();
        DateTime start;        
        
        public RandomDateTime()
        {
            start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,0,0,0);
        }

        public RandomDateTime(DateTime startTime)
        {
            start = startTime;
        }

        public DateTime Next()
        {
            var dateTime = new DateTime(start.Year, start.Month, start.Day, gen.Next(0, 24), gen.Next(0, 60), gen.Next(0, 60));
            return dateTime;
        }

        public DateTime Next(DateTime startTime)
        {
            var dateTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, gen.Next(startTime.Hour, 23), gen.Next(0, 60), gen.Next(0, 60));
            return dateTime;

        }
    }

    [TestFixture]
    public class PunchTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        
            CreateLocalDBTest();
            CreateImportTableTest();
            InitializeCSVDataTest();
        
        }

        public string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(Configs.localDB);
            var currentDir = Directory.GetParent("./");
            var parentDirectory = currentDir.Parent.Parent.FullName;
            var dbPath = $"{parentDirectory}\\punchDB.mdf";

            builder.AttachDBFilename = dbPath;
            return builder.ConnectionString;
        }

        public void CreateImportTableTest()
        {
            
            var bulk = new SqlBulkClient(GetConnectionString());
            bulk.Open();
            var check = bulk.ExecuteScalar(SqlScripts.CheckTable, null);
            bulk.Close();
            if (check != null)
                return;

            bulk.Open();            
            bulk.ExecuteScalar(SqlScripts.CreateTable, null);
            bulk.Close();

        }

        public void CreateLocalDBTest()
        {
            var builder = new SqlConnectionStringBuilder(Configs.localDB);
            
            var currentDir = Directory.GetParent("./");
            var parentDirectory = currentDir.Parent.Parent.FullName;

            var dbPath = $"{parentDirectory}\\punchDB.mdf";
            if (File.Exists(dbPath))
                return;

            var sql = string.Format(SqlScripts.CreatelocalDB, parentDirectory, parentDirectory);
            

            var bulk = new SqlBulkClient($"Data Source={builder.DataSource}");
            bulk.Open();            
            bulk.ExecuteScalar(sql, null);
            bulk.Close();

        }

        public void InitializeCSVDataTest()
        {
            if (File.Exists(Configs.filePath))
                return;

            var randomTimeGen = new RandomDateTime();
            Guid machineID = Guid.NewGuid();
            
            List<PunchRecord> punchRecords = new List<PunchRecord>();
            PunchRecord punchRecord = null;

            for (var i=0; i<100;i++)
            {
                
                var startTime = randomTimeGen.Next();
                var endTime = randomTimeGen.Next(startTime);

                punchRecord = new PunchRecord();
                punchRecord.MachineID = machineID;
                punchRecord.CardID = Guid.NewGuid();
                punchRecord.PunchIn = startTime;
                punchRecord.PunchOut = endTime;
                punchRecords.Add(punchRecord);
            }
                        
            Assert.IsNotNull(punchRecord);

            StringBuilder sbCSV = new StringBuilder();
            string header = string.Join(",", punchRecord.MachineID.GetType().Name, punchRecord.CardID.GetType().Name, punchRecord.PunchIn.GetType().Name, punchRecord.PunchOut.GetType().Name);
            sbCSV.AppendLine(header);
            string line = string.Join(",", punchRecord.MachineID, punchRecord.CardID, punchRecord.PunchIn, punchRecord.PunchOut);
            sbCSV.AppendLine(line);

            string csv = sbCSV.ToString();

            string filePath = Configs.filePath;
            using (var writer = new StreamWriter(filePath))
            using (var csvOut = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvOut.WriteRecords(punchRecords);
            }

            Assert.IsNotNull(csv);
        }

        [Test]
        public void BulkloadPunchCSVTest()
        {
            var bulkLoader = new BulkLoaderPunchRecordCSV(Configs.filePath);
            bulkLoader.Prepare();
            bulkLoader.Run();
        }

    }
}
