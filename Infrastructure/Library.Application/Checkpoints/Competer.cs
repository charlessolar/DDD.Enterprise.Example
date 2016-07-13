using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Checkpoints
{
    public class Competer
    {
        public String Id { get; set; }
        public Guid Discriminator { get; set; }
        public String Endpoint { get; set; }
        public Int32 Bucket { get; set; }
        public DateTime Heartbeat { get; set; }
        public long Position { get; set; }
    }
}
