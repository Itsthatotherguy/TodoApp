using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Services.Exceptions
{

    [Serializable]
    public class DbTransactionException : Exception
    {
        public DbTransactionException() { }
        public DbTransactionException(string message) : base(message) { }
        public DbTransactionException(string message, Exception inner) : base(message, inner) { }
        protected DbTransactionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
