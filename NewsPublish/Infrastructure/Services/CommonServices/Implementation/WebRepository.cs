using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Database.Data;
using NewsPublish.Database.Entities.WebEntities;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.Infrastructure.Services.CommonServices.Implementation
{
    public class WebRepository : IWebRepository
    {
        private readonly RoutineDbContext _context;

        public WebRepository(RoutineDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public void AddBanner(Banner banner)
        {
            MyTools.ArgumentDispose(banner);
            _context.Banners.Add(banner);
        }

        public void UpdateBanner(Banner banner)
        {
            _context.Entry(banner).State = EntityState.Modified;
        }

        public void DeleteBanner(Banner banner)
        {
            _context.Remove(banner);
        }

        public async Task<Banner> GetBanner(Guid id)
        {
            MyTools.ArgumentDispose(id);
            return await _context.Banners.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Banner>> GetBanners()
        {
            return await _context.Banners.ToListAsync();
        }

        public async Task<bool> BannerIsExits(Guid id)
        {
            MyTools.ArgumentDispose(id);
            return await _context.Banners.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}