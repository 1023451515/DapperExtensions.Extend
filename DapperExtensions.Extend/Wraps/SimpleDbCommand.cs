
using System.Data;
using System.Data.Common;

namespace DapperExtensions.Extend.Wraps
{
    [System.ComponentModel.DesignerCategory("")]
    public class SimpleDbCommand : DbCommand
    {

        private DbCommand _command;
        private DbConnection _connection;
        private DbTransaction _transaction;


        public SimpleDbCommand(DbCommand command, DbConnection connection)
        {
            _command = command;
            _connection = connection;
            if (_connection is SimpleDbConnection)
            {
                var s_con = _connection as SimpleDbConnection;
                s_con.Commands.Add(command);
            }
        }

        public override string CommandText
        {
            get
            {
                return _command.CommandText;
            }

            set
            {
                _command.CommandText = value;
            }
        }

        public override int CommandTimeout
        {
            get
            {
                return _command.CommandTimeout;
            }

            set
            {
                _command.CommandTimeout = value;
            }
        }

        public override CommandType CommandType
        {
            get
            {
                return _command.CommandType;
            }

            set
            {
                _command.CommandType = value;
            }
        }

        public override bool DesignTimeVisible
        {
            get
            {
                return _command.DesignTimeVisible;
            }

            set
            {
                _command.DesignTimeVisible = value;
            }
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                return _command.UpdatedRowSource;
            }

            set
            {
                _command.UpdatedRowSource = value;
            }
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return _connection;
            }

            set
            {
                _connection = value;
                if (value is SimpleDbConnection)
                {
                    var s_conn = value as SimpleDbConnection;
                    _command.Connection = s_conn.WrappedConnection;
                }
                else
                {
                    _command.Connection = value;
                }
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                return _command.Parameters;
            }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                return _transaction;
            }

            set
            {
                _transaction = value;
                var awesomeTran = value as SimpleDbTransaction;
                _command.Transaction = awesomeTran == null ? value : awesomeTran.WrappedTransaction;
            }
        }

        public override void Cancel()
        {
            _command.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }

        public override object ExecuteScalar()
        {
            return _command.ExecuteScalar();
        }

        public override void Prepare()
        {
            _command.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return _command.CreateParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return _command.ExecuteReader(behavior);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _command != null)
            {
                _command.Dispose();
            }
            _command = null;
            base.Dispose(disposing);
        }

    }
}
