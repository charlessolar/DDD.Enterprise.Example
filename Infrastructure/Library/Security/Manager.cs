namespace Demo.Library.Security
{
    public class Manager : IManager
    {
        public bool Authorize(string actor, string context, string action)
        {
            // Default manager - not a very good manager
            return true;
        }
    }
}