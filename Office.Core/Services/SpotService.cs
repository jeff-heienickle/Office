using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Office.Core.Models;

namespace Office.Core.Services
{
    public class SpotService : ISpotService
    {
        private readonly ApplicationDbContext _context;

        public SpotService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Spot[]> GetSpotsAsync()
        {
            var items = await _context.Items
                .Where(x => x.IsDone == false)
                .ToArrayAsync();
            return items;
        }
    }
}