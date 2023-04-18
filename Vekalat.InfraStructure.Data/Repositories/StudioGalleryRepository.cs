using AryanShop.InfraStructure.Data.Repositories;
using Vekalat.Core.Entities;
using Vekalat.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Vekalat.Application.Features.StudioGalleryFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class StudioGalleryRepository : Repository<StudioGallery>, IStudioGalleryRepository
    {
        private readonly VekalatDataContext _context;
        public StudioGalleryRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable<StudioGallery> GetAllWithFilter(StudioGalleryFilterInput studioGalleryFilterInput)
        {
            var items = _context.StudioGalleries.Include(c=>c.StudioFk)
            .OrderByDescending(e => e.Id)
            .AsQueryable();


            if (studioGalleryFilterInput.StudioId != null)
                items = items.Where(e => e.StudioId == studioGalleryFilterInput.StudioId);

            if (studioGalleryFilterInput.DisplayFront != null)
                items = items.Where(e => e.DisplayFront == studioGalleryFilterInput.DisplayFront);

            return items;
        }

    }
}
