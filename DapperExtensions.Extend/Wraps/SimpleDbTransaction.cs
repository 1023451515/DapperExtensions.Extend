
using System.Data;
using System.Data.Common;

namespace DapperExtensions.Extend.Wraps
{
    public class SimpleDbTransaction : DbTransaction
    {
        private SimpleDbConnection _connection;
        private DbTransaction _transaction;

        public SimpleDbTransaction(DbTransaction transaction, SimpleDbConnection connection)
        {
            _transaction = transaction;
            _connection = connection;
        }
        public DbTransaction WrappedTransaction { get { return _transaction; } }

        public override IsolationLevel IsolationLevel
        {
            get
            {
                return _transaction.IsolationLevel;
            }
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return _connection;
            }
        }

        public override void Commit()
        {
            _transaction.Commit();
        }

        public override void Rollback()
        {
            _transaction.Rollback();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _transaction != null)
            {
                _transaction.Dispose();
            }
            _transaction = null;
            _connection = null;
            base.Dispose(disposing);
        }
    }
}
