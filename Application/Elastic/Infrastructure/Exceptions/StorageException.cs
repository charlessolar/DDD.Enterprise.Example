using Aggregates.Exceptions;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Elastic.Infrastructure.Exceptions
{
    public class StorageException : Aggregates.PersistenceException
    {
        public StorageException(IBulkResponse response) : base(BuildMessage(response)) {
            
        }

        private static string BuildMessage(IBulkResponse response)
        {
            try
            {
                // DebugInformation can throw. ... .......
                var builder = new StringBuilder();
                builder.AppendLine(response.DebugInformation);
                foreach (var item in response.ItemsWithErrors)
                    builder.AppendLine(item.Error.Reason);
                return builder.ToString();
            }
            catch { }
            return "";
        }
    }
}
