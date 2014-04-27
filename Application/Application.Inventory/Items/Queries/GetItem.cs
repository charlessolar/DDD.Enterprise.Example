using Library.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.Items.Queries
{
    public class GetItem : BasicQuery
    {
        public Guid Id { get; set; }
    }
}