using Demo.Library.Command;
using System;

namespace Demo.Domain.Relations.Store.Commands
{
    public class Destroy : DemoCommand
    {
        public Guid StoreId { get; set; }
    }
}