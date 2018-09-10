using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Core
{
    public abstract class BusinessBase : IDisposable
    {
        private List<string> _ownedContexts = new List<string>();

        protected IDbConnection DbConnection
        {
            get
            {
                DataContextAttribute dca = ExtractNestedContextAttribute();
                if (dca == null)
                    return null;

                IDbConnection connection = ContextManager.Instance.GetConnection(dca.ContextName);
                if (connection != null)
                    return connection;

                connection = OpenConnection(dca.ContextName);
                if (connection != null)
                {
                    ContextManager.Instance.AddNewContext(dca.ContextName, connection);
                    _ownedContexts.Add(dca.ContextName);
                }

                return connection;
            }
        }

        protected IDbTransaction DbTransaction
        {
            get
            {
                DataContextAttribute dca = ExtractNestedContextAttribute();
                if (dca == null)
                    return null;

                IDbTransaction transaction = ContextManager.Instance.GetTransaction(dca.ContextName);
                if (transaction == null && dca.Transactional)
                {
                    IDbConnection connection = ContextManager.Instance.GetConnection(dca.ContextName);
                    if (connection != null)
                    {
                        transaction = connection.BeginTransaction();
                        ContextManager.Instance.SetTransaction(dca.ContextName, transaction);
                    }
                }

                return transaction;
            }
        }

        protected void SetAbort()
        {
            DataContextAttribute dca = ExtractNestedContextAttribute();
            if (dca == null)
                return;

            ContextManager.Instance.SetAbort(dca.ContextName);
        }

        protected abstract IDbConnection OpenConnection(string contextName);

        private DataContextAttribute ExtractNestedContextAttribute()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();
            foreach (StackFrame stackFrame in stackFrames)
            {
                MethodBase method = stackFrame.GetMethod();
                object[] attrs = method.GetCustomAttributes(typeof(DataContextAttribute), false);
                if (attrs.Length > 0)
                {
                    DataContextAttribute dca = attrs[0] as DataContextAttribute;
                    return dca;
                }
            }
            return null;
        }

        public void Dispose()
        {
            foreach (string contextName in _ownedContexts)
            {
                IDbConnection connection = null;
                IDbTransaction transaction = null;
                bool isAborted = false;
                try
                {
                    connection = ContextManager.Instance.GetConnection(contextName);
                    transaction = ContextManager.Instance.GetTransaction(contextName);
                    isAborted = ContextManager.Instance.IsAborted(contextName);
                }
                finally
                {
                    ContextManager.Instance.RemoveContext(contextName);
                }

                if (transaction != null)
                {
                    if (!isAborted)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }

                if (connection != null)
                    connection.Close();
            }
        }
    }
}
