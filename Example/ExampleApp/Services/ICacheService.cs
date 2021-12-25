using System.Threading.Tasks;

namespace ExampleApp.Services
{
    public interface ICacheService
    {
        void Add(string key, string value);
        Task<bool> Has(string key);
    }
}