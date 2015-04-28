using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Operations
{
    public interface IOperation
    {
        Task<Boolean> Seed();

        Boolean Done { get; }
    }
}