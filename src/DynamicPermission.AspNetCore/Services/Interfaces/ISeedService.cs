using System.Threading.Tasks;

namespace DynamicPermission.AspNetCore.Services
{
    public interface ISeedService
    {
        Task SeedAsync();
    }
}