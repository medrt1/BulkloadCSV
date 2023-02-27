using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace ConsoleApp1
{
    public class DataReaderPunchRecord : IDataReader
    {

        private IEnumerator<PunchRecord> _data;
        private string _fileName;
        private DateTime _importTime;

        public DataReaderPunchRecord(List<PunchRecord> punchRecord,string fileName)
        {
            _data = punchRecord.GetEnumerator();
            _fileName = fileName;
            _importTime = DateTime.Now;

        }

        public long RowCount { get; private set; }

        public int RecordsAffected => 
            -1;

        public int FieldCount => 
            7;

        public bool IsClosed =>
            _data == null;

        public int GetOrdinal(string name)
        {
            //
            if (name == "MachineID")
                return 0;
            if (name == "CardID")
                return 1;
            if (name == "PunchIn")
                return 2;
            if (name == "PunchOut")
                return 3;
            if (name == "FileName")
                return 4;
            if (name == "LineNumber")
                return 5;
            if (name == "DateOfImport")
                return 6;

            throw new Exception($"the propery name:{name} does not exit.");
        }

        public object GetValue(int i)
        {
            if (i == 0)
                return _data.Current.MachineID; 
            if (i == 1)
                return _data.Current.CardID; 
            if (i == 2)
                return _data.Current.PunchIn; 
            if (i == 3)
                return _data.Current.PunchOut;
            if (i == 4)
                return _fileName;
            if (i == 5)
                return RowCount;
            if (i == 6)
                return _importTime;

            throw new Exception($"the propery id:{i} does not exit.");
        }

        public bool IsDBNull(int i)
        {
            return false;
        }

        public bool Read()
        {
            bool isRead = _data != null && _data.MoveNext();
            if(isRead) 
                RowCount++;
            
            return isRead; 
        }

        public void Close()
        {
            Dispose();
        }
        
        public void Dispose()
        {
            _data =null;
            _fileName = null;
            _importTime = DateTime.MinValue;
        }

        #region throw new NotImplementedException();

        public object this[int i] =>
            throw new NotImplementedException();

        public object this[string name] =>
            throw new NotImplementedException();

        public int Depth =>
            throw new NotImplementedException();

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
