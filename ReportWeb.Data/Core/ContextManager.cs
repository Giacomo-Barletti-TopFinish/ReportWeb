using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Core
{
    internal class ContextManager
    {
        public static ContextManager Instance { get; private set; }

        static ContextManager()
        {
            Instance = new ContextManager();
        }

        private class Context
        {
            public IDbConnection Connection { get; set; }
            public IDbTransaction Transaction { get; set; }
            public bool IsAborted { get; set; }
        }

        private object _syncRoot;
        [ThreadStatic]
        static private Dictionary<string, Context> _contexts;

        private static Dictionary<string, Context> Contexts
        {
            get
            {
                if (_contexts == null)
                    _contexts = new Dictionary<string, Context>();
                return _contexts;
            }
        }


        private ContextManager()
        {
            _syncRoot = new object();
        }

        public IDbConnection GetConnection(string contextName)
        {
            lock (_syncRoot)
            {
                if (contextName == null)
                    contextName = string.Empty;
                if (!Contexts.ContainsKey(contextName))
                    return null;
                Context context = Contexts[contextName];
                return context.Connection;
            }
        }

        public IDbTransaction GetTransaction(string contextName)
        {
            lock (_syncRoot)
            {
                if (contextName == null)
                    contextName = string.Empty;
                if (!Contexts.ContainsKey(contextName))
                    return null;
                Context context = Contexts[contextName];
                return context.Transaction;
            }
        }

        public void SetTransaction(string contextName, IDbTransaction transaction)
        {
            lock (_syncRoot)
            {
                if (contextName == null)
                    contextName = string.Empty;
                if (!Contexts.ContainsKey(contextName))
                    throw new ArgumentException("Context " + contextName + " does not exist");
                Contexts[contextName].Transaction = transaction;
            }
        }

        public bool IsAborted(string contextName)
        {
            lock (_syncRoot)
            {
                if (contextName == null)
                    contextName = string.Empty;
                if (!Contexts.ContainsKey(contextName))
                    throw new ArgumentException("Context " + contextName + " does not exist");
                return Contexts[contextName].IsAborted;
            }
        }

        public void SetAbort(string contextName)
        {
            lock (_syncRoot)
            {
                if (contextName == null)
                    contextName = string.Empty;
                if (!Contexts.ContainsKey(contextName))
                    throw new ArgumentException("Context " + contextName + " does not exist");
                Contexts[contextName].IsAborted = true;
            }
        }

        public void AddNewContext(string contextName, IDbConnection connection)
        {
            lock (_syncRoot)
            {
                if (contextName == null)
                    contextName = string.Empty;
                if (Contexts.ContainsKey(contextName))
                    throw new ArgumentException("Context " + contextName + " already exists");
                Context context = new Context() { Connection = connection };
                Contexts.Add(contextName, context);
            }
        }


        public void RemoveContext(string contextName)
        {
            lock (_syncRoot)
            {
                if (contextName == null)
                    contextName = string.Empty;
                if (!Contexts.ContainsKey(contextName))
                    throw new ArgumentException("Context " + contextName + " does not exist");
                Contexts.Remove(contextName);
            }
        }
    }
}
