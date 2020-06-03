using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsPublish.Database.Entities.WebEntities;

namespace NewsPublish.Infrastructure.Services.CommonServices.Interface
{
    public interface IWebRepository
    {
        void AddBanner(Banner banner);
        void UpdateBanner(Banner banner);
        void DeleteBanner(Banner banner);
        Task<Banner> GetBanner(Guid id);
        Task<IEnumerable<Banner>> GetBanners();
        Task<bool> BannerIsExits(Guid id);
        Task<bool> SaveAsync();
    }
}