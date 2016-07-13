using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Presentation.ServiceStack.Checkpoints
{
    public class Position
    {
        public String Id { get; set; }
        public Int64 CommitPosition { get; set; }
        public Int64 PreparePosition { get; set; }
    }
}