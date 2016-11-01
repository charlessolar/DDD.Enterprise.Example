namespace Demo.Library.Security
{
    public interface IManager
    {
        bool Authorize(string actor, string context, string action);
    }
}