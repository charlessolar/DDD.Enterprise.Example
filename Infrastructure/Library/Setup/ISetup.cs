using System.Threading.Tasks;

namespace Demo.Library.Setup
{
    public interface ISetup
    {
        Task<bool> Initialize();

        bool Done { get; }
    }
}
