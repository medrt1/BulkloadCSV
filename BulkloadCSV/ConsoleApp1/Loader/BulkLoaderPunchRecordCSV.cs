using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
   
    public class PunchRecordMap : ClassMap<PunchRecord>
    {
        public PunchRecordMap()
        {

            Map(m => m.MachineID).Convert(row =>
                Guid.Parse(row.Row["MachineID"]
            ));
            Map(m => m.CardID).Convert(row =>
                Guid.Parse(row.Row["CardID"]
            ));
            Map(m => m.PunchIn).Convert(row => 
                DateTime.Parse(row.Row["PunchIn"]
            ));
            Map(m => m.PunchOut).Convert(row =>
                DateTime.Parse(row.Row["PunchOut"]
            ));

        }
    }

    public class BulkLoaderPunchRecordCSV
    {
        private string _filePath;
        private CsvConfiguration _csvConfigure = null;

        public BulkLoaderPunchRecordCSV(string filePath)
        {
            _filePath = filePath;
        }

        public void Prepare(string delimiter=",")
        {
            _csvConfigure = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                HasHeaderRecord = true,
                Delimiter = delimiter,
            };

            if (!File.Exists(_filePath))
                throw new Exception("File does not exist.");
        }

        private List<PunchRecord> LoadFromCSV()
        {
            List<PunchRecord> punchRecords = new List<PunchRecord>();

            try
            {
                
                using (var reader = new StreamReader(_filePath))
                using (var csv = new CsvReader(reader, _csvConfigure))
                {
                    csv.Context.RegisterClassMap<PunchRecordMap>();
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        var record = csv.GetRecord<PunchRecord>();
                        punchRecords.Add(record);
                    };

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return punchRecords;
        }

        private long BulkLoadToDb(List<PunchRecord> punchRecords)
        {

            var dr = new DataReaderPunchRecord(punchRecords, _filePath);

            Dictionary<string, string> mapping = new Dictionary<string, string> {
                    { "FileName","FileName"},
                    { "LineNumber","LineNumber"},
                    { "DateOfImport","DateOfImport"},
                    { "MachineID","MachineID"},
                    { "CardID","CardID"},
                    { "PunchIn","PunchIn"},
                    { "PunchOut","PunchOut"},
            };

            var bulk = new SqlBulkClient(Configs.Db);

            try
            {
                bulk.Open();

                bulk.BeginTransaction(BulkIsolationLevel.ReadCommitted);

                bulk.SqlBulkCopy("ImportedPunchRecord", dr, mapping);

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                bulk.Commit();
                bulk.Close();
            }

            return dr.RowCount;

        }

        public void Run()
        {
            
            if(_csvConfigure == null)
                throw new Exception("Loader not configured.");

            var punchRecords = LoadFromCSV();
            var rowProcesed = BulkLoadToDb(punchRecords);
            System.Diagnostics.Debug.WriteLine($"Number of rows processed {rowProcesed}");


        }

    }

}
