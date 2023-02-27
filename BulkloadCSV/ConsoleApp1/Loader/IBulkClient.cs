using System;
using System.Collections.Generic;
using System.Data;

namespace ConsoleApp1
{
    public enum BulkIsolationLevel
    {
        ReadCommitted = IsolationLevel.ReadCommitted,
        ReadUncommitted = IsolationLevel.ReadUncommitted,
        Serializable = IsolationLevel.Serializable,
        Snapshot = IsolationLevel.Snapshot,
    }

    public interface IBulkClient
    {
    
        void BeginTransaction(BulkIsolationLevel sqlIsolationLevel = BulkIsolationLevel.ReadCommitted);
        void Close();
        void Commit();
        IDataReader ExecuteReader(string command, Dictionary<string, object> commandParams, bool readUncommitted = false);
        void Open();
        void RollBack();
        void SqlBulkCopy(string tableName, IDataReader dbDataReader, Dictionary<string, string> columnsMapping, Action<string, long> sqlRowsCopied = null);

    }
}