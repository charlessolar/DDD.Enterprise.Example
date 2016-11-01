using System.Threading.Tasks;

namespace Demo.Library.Setup
{
    public interface IImport
    {
        Task<bool> Import();
        bool Started { get; }
    }
}
