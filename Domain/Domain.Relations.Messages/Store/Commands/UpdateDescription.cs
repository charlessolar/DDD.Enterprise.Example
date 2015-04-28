using Demo.Library.Command;
using System;

namespace Demo.Domain.Relations.Store.Commands
{
    public class UpdateDescription : DemoCommand
    {
        public Guid StoreId { get; set; }

        public String Description { get; set; }
    }
}