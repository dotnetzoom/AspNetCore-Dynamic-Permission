using System.Threading.Tasks;

namespace DynamicPermission.CQRS.Services
{
    public interface ISeedService
    {
        Task SeedAsync();
    }
}