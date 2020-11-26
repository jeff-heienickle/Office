using Office.Core.Models;
using System.Threading.Tasks;

namespace Office.Core.Services
{
    public interface ISpotService
    {
        Task<Spot[]> GetSpotsAsync();
    }
}