
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DapperExtensions.Extend.Wraps
{
    [System.ComponentModel.DesignerCategory("")]
    public class SimpleDbConnection : DbConnection
    {

        private DbConnection _connection;
        public List<DbCommand> Commands;

        public SimpleDbConnection(IDbConnection connection)
        {
            _connection = (DbConnection)connection;
            if (_connection.State == ConnectionState.Closed)
            {
                Open();
            }
            _connection.StateChange += StateChangeHandler;
            Commands = new List<DbCommand>();
        }

        public DbConnection WrappedConnection
        {
            get { return _connection; }
        }

        private void StateChangeHandler(object sender, StateChangeEventArgs stateChangeEventArguments)
        {
            OnStateChange(stateChangeEventArguments);
        }

        public override string ConnectionString
        {
            get
            {
                return _connection.ConnectionString;
            }

            set
            {
                _connection.ConnectionString = value;
            }
        }

        public override string Database
        {
            get
            {
                return _connection.Database;
            }
        }

        public override string DataSource
        {
            get
            {
                return _connection.DataSource;
            }
        }

        public override string ServerVersion
        {
            get
            {
                return _connection.ServerVersion;
            }
        }

        public override ConnectionState State
        {
            get
            {
                return _connection.State;
            }
        }

        public override void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            _connection.Close();
        }

        public override void Open()
        {
            _connection.Open();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new SimpleDbTransaction(_connection.BeginTransaction(isolationLevel), this);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new SimpleDbCommand(_connection.CreateCommand(), this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _connection != null)
            {
                _connection.StateChange -= StateChangeHandler;
                _connection.Dispose();
            }
            base.Dispose(disposing);
            _connection = null;
        }
    }
}
