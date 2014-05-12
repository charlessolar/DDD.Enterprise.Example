namespace Demo.Library.Security
{
    public interface IRule
    {
        bool IsAuthorized(object instance);

        string Description { get; }
    }
}