using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    public class SqlBulkClient : IBulkClient
    {

        private SqlConnection _sqlConnection;
        private SqlTransaction _sqlTransaction;

        private SqlBulkClient()
        {

        }

        public SqlBulkClient(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }

        private SqlCommand BuildCommand(string commandText, Dictionary<string, object> commandParams = null)
        {

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = commandText;
            sqlCommand.Parameters.Clear();

            if (commandParams != null)
            {
                foreach (KeyValuePair<string, object> commandParam in commandParams)
                {
                    sqlCommand.Parameters.AddWithValue(commandParam.Key, commandParam.Value ?? (object)DBNull.Value);
                }

            }

            return sqlCommand;
        }

        public void BeginTransaction(BulkIsolationLevel sqlIsolationLevel = BulkIsolationLevel.ReadCommitted)
        {
            IsolationLevel isolationLevel;
            switch (sqlIsolationLevel)
            {
                case BulkIsolationLevel.ReadCommitted:
                    isolationLevel = IsolationLevel.ReadCommitted;
                    break;
                case BulkIsolationLevel.ReadUncommitted:
                    isolationLevel = IsolationLevel.ReadUncommitted;
                    break;

                case BulkIsolationLevel.Serializable:
                    isolationLevel = IsolationLevel.Serializable;
                    break;

                case BulkIsolationLevel.Snapshot:
                    isolationLevel = IsolationLevel.Snapshot;
                    break;

                default:
                    isolationLevel = IsolationLevel.ReadCommitted;
                    break;
            }

            _sqlTransaction = _sqlConnection.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            if (_sqlTransaction != null && _sqlTransaction.Connection != null)
                _sqlTransaction.Commit();
        }

        public void RollBack()
        {
            if (_sqlTransaction != null && _sqlTransaction.Connection != null)
                _sqlTransaction.Rollback();
        }

        public void Open()
        {
            if (_sqlConnection.State == ConnectionState.Closed)
                _sqlConnection.Open();
        }

        public void Close()
        {
            if (_sqlTransaction != null && _sqlTransaction.Connection != null) _sqlTransaction.Commit();
            if (_sqlConnection.State != ConnectionState.Closed) _sqlConnection.Close();
        }

        public IDataReader ExecuteReader(string command, Dictionary<string, object> commandParams, bool readUncommitted = false)
        {
            SqlCommand sqlCommand = BuildCommand(command, commandParams);
            sqlCommand.Connection = _sqlConnection;

            if (_sqlConnection.State != ConnectionState.Open) _sqlConnection.Open();
            if (_sqlTransaction != null && _sqlTransaction.Connection != null) sqlCommand.Transaction = _sqlTransaction;

            IDataReader dr = sqlCommand.ExecuteReader();

            return dr;

        }

        public object ExecuteScalar(string command, Dictionary<string, object> commandParams)
        {
            SqlCommand sqlCommand = BuildCommand(command, commandParams);
            sqlCommand.Connection = _sqlConnection;

            if (_sqlConnection.State != ConnectionState.Open) _sqlConnection.Open();
            if (_sqlTransaction != null && _sqlTransaction.Connection != null) sqlCommand.Transaction = _sqlTransaction;

            object result = sqlCommand.ExecuteScalar();

            return result;

        }

        public void SqlBulkCopy(string tableName, IDataReader dbDataReader, Dictionary<string, string> columnsMapping, Action<string, long> sqlRowsCopied = null)
        {

            if (_sqlTransaction == null || _sqlTransaction.Connection == null)
                _sqlConnection.BeginTransaction(IsolationLevel.ReadCommitted);

            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(_sqlConnection, SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.TableLock, _sqlTransaction);
            foreach (KeyValuePair<string, string> columns in columnsMapping)
            {
                sqlBulkCopy.ColumnMappings.Add(columns.Key, columns.Value);
            }

            sqlBulkCopy.BatchSize = 500;
            sqlBulkCopy.EnableStreaming = true;
            sqlBulkCopy.DestinationTableName = tableName;
            sqlBulkCopy.NotifyAfter = 500;

            if (sqlRowsCopied != null)
                sqlBulkCopy.SqlRowsCopied += (s, e) => sqlRowsCopied(sqlBulkCopy.DestinationTableName, e.RowsCopied);

            sqlBulkCopy.WriteToServer(dbDataReader);
            sqlBulkCopy.Close();

        }

    }

}
