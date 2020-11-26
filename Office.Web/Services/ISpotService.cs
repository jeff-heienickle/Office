using Office.Core.Entities;
using Office.Web;
using System;
using System.Threading.Tasks;

namespace Office.Core.Services
{
    public interface ISpotService
    {
        Task<PagedResult<Spot>> GetSpotsAsync(int page, int pageSize);

        Task<PagedResult<Spot>> GetSpotsByNameAsync(string name, int page, int pageSize);

        Task<Spot> GetSpotByIdAsync(Guid Id);

        Task<bool> AddSpotAsync(Spot spot);

        Task<Spot[]> GetManagersAsync();

        Task<bool> DeleteSpotAsnc(Guid id);
    }
}