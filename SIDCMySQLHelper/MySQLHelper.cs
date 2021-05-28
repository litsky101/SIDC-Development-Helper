using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace SIDCMySQLHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class MySQLHelper : IDisposable
    {
        private System.ComponentModel.Component components = new System.ComponentModel.Component();
        private Dictionary<string, object> _argMySQLParam;
        private StringBuilder _argMySQLCommand;
        bool disposedValue = false;
        private int _timeOut = 3000;

        private MySqlConnection conn = null;
        private MySqlTransaction trans = null;
        private MySqlCommand cmd
        {
            get
            {
                var _cmd = new MySqlCommand(_argMySQLCommand.ToString(), conn, trans);

                if (_argMySQLParam != null)
                {
                    foreach (var param in _argMySQLParam)
                    {
                        _cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                _cmd.CommandTimeout = _timeOut;
                return _cmd;
            }
        }

        /// <summary>
        /// MySQLParameters 
        /// <para>Key: parameter in sql/script</para>
        /// <para>Value: value passing</para>
        /// <para>eg. Key = @id; Value = 1</para>
        /// </summary>
        public Dictionary<string, object> ArgMySQLParam
        {
            get { return _argMySQLParam; }
            set { _argMySQLParam = value; }
        }

        /// <summary>
        /// MySQL Command
        /// </summary>
        public StringBuilder ArgMySQLCommand
        {
            get { return _argMySQLCommand; }
            set { _argMySQLCommand = value; }
        }

        /// <summary>
        /// MySQLConnection Timout
        /// </summary>
        public int TimeOut
        {
            get { return TimeOut; }
            set { TimeOut = value; }
        }


        /// <summary>
        /// Call this class to initialize MySQL Data Access Layer (You can use any of the following MySQL parameters of your like.)
        /// </summary>
        /// <param name="connString">Connection String</param>
        /// <param name="argMySQLCommand">MySQL Query/Script</param>
        /// <param name="argMySQLParam">MySQL Parameters</param>
        public MySQLHelper(string connString, StringBuilder argMySQLCommand = null, Dictionary<string, object> argMySQLParam = null)
        {
            try
            {
                conn = new MySqlConnection(connString);
                _argMySQLParam = argMySQLParam;
                _argMySQLCommand = argMySQLCommand;
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Begin Transaction
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Commit Transaction Created
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                trans.Commit();
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Call this method function to return DataTable from MySQL
        /// </summary>
        /// <returns></returns>
        public DataTable GetMySQLDataTable()
        {
            try
            {
                conn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(dr);

                    return dt;
                }
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Call this method function to return object from MySQL
        /// </summary>
        /// <returns></returns>
        public object GetMySQLScalar()
        {
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Call this method function to return MySQLDataReader to create customize VO
        /// </summary>
        /// <returns></returns>
        public MySqlDataReader GetMySQLReader()
        {
            try
            {
                conn.Open();
                return cmd.ExecuteReader();
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Call this method function to bulk insert file synchronously
        /// </summary>
        /// <param name="targetTableName">Table Name</param>
        /// <param name="sourceFilePath">File path including filename and extension</param>
        /// <param name="columnTerminator">Field terminator</param>
        /// <param name="rowTerminator">Line Terminator</param>
        /// <param name="linesToSkip">Skip header or rows to skip</param>
        /// <returns></returns>
        public int BulkInsertFile(string targetTableName, string sourceFilePath, int linesToSkip = 0, string columnTerminator = ",", string rowTerminator = @"\r\n")
        {
            try
            {
                conn.Open();
                var blk = new MySqlBulkLoader(conn);

                blk.TableName = targetTableName;
                blk.FileName = sourceFilePath;
                blk.FieldTerminator = columnTerminator;
                blk.LineTerminator = rowTerminator;
                blk.NumberOfLinesToSkip = linesToSkip;
                blk.FieldQuotationCharacter = '"';
                blk.EscapeCharacter = ' ';
                int rowsAffected = blk.Load();
                return rowsAffected;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Call this method function to bulk insert file synchronously
        /// </summary>
        /// <param name="targetTableName">Table Name</param>
        /// <param name="sourceFilePath">File path including filename and extension</param>
        /// <param name="columnTerminator">Field terminator</param>
        /// <param name="rowTerminator">Line Terminator</param>
        /// <param name="linesToSkip">Skip header or rows to skip</param>
        /// <returns></returns>
        public int BulkInsertFileUnQuote(string targetTableName, string sourceFilePath, int linesToSkip = 0, string columnTerminator = ",", string rowTerminator = @"\r\n")
        {
            try
            {
                conn.Open();
                var blk = new MySqlBulkLoader(conn);

                blk.TableName = targetTableName;
                blk.FileName = sourceFilePath;
                blk.FieldTerminator = columnTerminator;
                blk.LineTerminator = rowTerminator;
                blk.NumberOfLinesToSkip = linesToSkip;

                int rowsAffected = blk.Load();
                return rowsAffected;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Call this method to execute MySQL query (eg. Save,Update,Delete)
        /// </summary>
        public int ExecuteMySQL()
        {
            try
            {
                int res = 0;

                if (conn.State.Equals(ConnectionState.Closed))
                {
                    trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                }

                res = cmd.ExecuteNonQuery();

                return res;
            }
            catch
            {
                throw;
            }
        }

        #region IDisposable Support

        /// <summary>
        /// Dispose resources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposedValue)
            {
                try
                {
                    cmd.Dispose();
                }
                catch
                {
                    
                }

                if (conn.State.Equals(ConnectionState.Open))
                    conn.Close();

                if (trans != null)
                    trans.Dispose();

                if (conn != null)
                    MySqlConnection.ClearPool(conn);

                components.Dispose();
            }

            disposedValue = true;
        }

        /// <summary>
        /// 
        /// </summary>
        ~MySQLHelper()
        {
            Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
