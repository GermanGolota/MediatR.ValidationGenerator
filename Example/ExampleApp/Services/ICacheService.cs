namespace ExampleApp.Services
{
    public interface ICacheService
    {
        void Add(string key, string value);
        bool Has(string key);
    }
}