using System;
using System.Data;

namespace ConsoleApp1
{
    public class DataReaderBase : IDataReader
    {
        public long RowCount { get; private set; } 
        private readonly IDataReader _dataReader;

        public DataReaderBase(IDataReader datareader)
        {
            _dataReader = datareader;
        }

        public object this[int i] => _dataReader[i];
        public object this[string name] => _dataReader[name];
        public int Depth => _dataReader.Depth;
        public bool IsClosed => _dataReader.IsClosed;
        public int RecordsAffected => _dataReader.RecordsAffected;
        public virtual int FieldCount => _dataReader.FieldCount;
        public void Close() => _dataReader.Close();
        public void Dispose() => _dataReader.Dispose();
        public bool GetBoolean(int i) => _dataReader.GetBoolean(i);
        public byte GetByte(int i) => _dataReader.GetByte(i);
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) 
            => _dataReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        public char GetChar(int i) => _dataReader.GetChar(i);
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) 
            => _dataReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        public IDataReader GetData(int i) => _dataReader.GetData(i);
        public string GetDataTypeName(int i) => _dataReader.GetDataTypeName(i);
        public DateTime GetDateTime(int i) => _dataReader.GetDateTime(i);
        public decimal GetDecimal(int i) => _dataReader.GetDecimal(i);
        public double GetDouble(int i) => _dataReader.GetDouble(i);
        public Type GetFieldType(int i) => _dataReader.GetFieldType(i);
        public float GetFloat(int i) => _dataReader.GetFloat(i);
        public Guid GetGuid(int i) => _dataReader.GetGuid(i);
        public short GetInt16(int i) => _dataReader.GetInt16(i);
        public int GetInt32(int i) => _dataReader.GetInt32(i);
        public long GetInt64(int i) => _dataReader.GetInt64(i);
        public string GetName(int i) => _dataReader.GetName(i);
        public int GetOrdinal(string name) => _dataReader.GetOrdinal(name);
        public DataTable GetSchemaTable() => _dataReader.GetSchemaTable();
        public string GetString(int i) => _dataReader.GetString(i);
        public virtual object GetValue(int i) => _dataReader.GetValue(i);
        public int GetValues(object[] values) => _dataReader.GetValues(values);
        public virtual bool IsDBNull(int i) => _dataReader.IsDBNull(i);
        public bool NextResult() => _dataReader.NextResult();
         
        public virtual bool Read()
        {

            bool isRead = _dataReader.Read();
            if (isRead == true)
                RowCount++;

            return isRead;
        }

    }
}
