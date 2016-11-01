namespace Demo.Presentation.ServiceStack.Checkpoints
{
    public class Position
    {
        public string Id { get; set; }
        public long CommitPosition { get; set; }
        public long PreparePosition { get; set; }
    }
}