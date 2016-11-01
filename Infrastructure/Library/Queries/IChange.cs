namespace Demo.Library.Queries
{
    public interface IChange<in TDto, in TQuery> where TQuery : IPaged
    {
        bool Satisfied(TDto dto, TQuery query);
    }
}
