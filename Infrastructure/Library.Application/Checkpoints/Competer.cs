using System;

namespace Demo.Library.Checkpoints
{
    public class Competer
    {
        public string Id { get; set; }
        public Guid Discriminator { get; set; }
        public string Endpoint { get; set; }
        public int Bucket { get; set; }
        public DateTime Heartbeat { get; set; }
        public long Position { get; set; }
    }
}
