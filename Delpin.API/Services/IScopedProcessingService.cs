using System.Threading.Tasks;

namespace Delpin.API.Services
{
    // the scope 
    public interface IScopedProcessingService
    {
        Task DoWork();
    }
}