using Demo.Library.Command;
using System;

namespace Demo.Domain.Relations.Store.Commands
{
    public class Create : DemoCommand
    {
        public Guid StoreId { get; set; }

        public String Identity { get; set; }

        public String Name { get; set; }
    }
}