using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Core.Entities;
using Office.Web;
using Office.Web.Data;

namespace Office.Core.Services
{
    public class SpotService : ISpotService
    {
        private readonly ApplicationDbContext _context;

        public SpotService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Spot>> GetSpotsAsync(int page, int pageSize)
        {
            var spots = await _context.Spots.AsNoTracking()
                .Where(x => x.IsActive == true)
                .GetPagedAsync(page, pageSize);
            return spots;
        }

        public async Task<PagedResult<Spot>> GetSpotsByNameAsync(string name, int page, int pageSize)
        {
            IQueryable<Spot> spotQuery = _context.Spots.AsNoTracking();
            spotQuery = spotQuery.Where(x => x.IsActive == true);
            if (!string.IsNullOrEmpty(name))
            {
                string match = name.ToLower().Trim() + "%";
                //spotQuery = spotQuery.Where(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
                spotQuery = spotQuery.Where(x => EF.Functions.Like(x.Name.ToLower(), match));                
            }

            return await spotQuery.GetPagedAsync(page, pageSize); 
        }

        public async Task<bool> AddSpotAsync(Spot spot)
        {
            spot.Id = Guid.NewGuid();
            spot.IsActive = true;
            spot.Created = DateTimeOffset.Now;

            _context.Spots.Add(spot);

           // spot.DirectReports

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> EditSpotAsync(Spot spot)
        {
            var editspot = await _context.Spots.FindAsync(spot.Id);
            editspot.Name = spot.Name;
            editspot.Title = spot.Title;
            editspot.Bonus = spot.Bonus;
            editspot.ManagerId = spot.ManagerId;

            if (spot.Image != null)
            {
                editspot.Image = spot.Image;
            }

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<Spot> GetSpotByIdAsync(Guid id)
        {
            var spot = await _context.Spots.FindAsync(id);
            Guid? myId = id;
            Guid? mgrId = spot.ManagerId;
            spot.DirectReports = _context.Spots.Where(x => x.ManagerId == myId).ToList();
            spot.Manager = _context.Spots.Where(x => x.Id == mgrId).FirstOrDefault();
            return spot;   
        }

        public async Task<bool> DeleteSpotAsnc(Guid id)
        {
            var spot = await _context.Spots.FindAsync(id);
            Guid? myId = id;
            Guid? mgrId = spot.ManagerId;
            var directReports = _context.Spots.Where(x => x.ManagerId == myId).ToList();
            directReports.ForEach(x => x.ManagerId = mgrId);

            _context.Spots.Remove(spot);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<Spot[]> GetManagersAsync()
        {
            var spots = await _context.Spots.AsNoTracking()
                .Where(x => x.IsActive == true)
                .OrderBy(x => x.Name)
                .ToArrayAsync();
            return spots;
        }
       
    }
}