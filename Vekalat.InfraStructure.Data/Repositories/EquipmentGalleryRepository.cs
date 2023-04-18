using AryanShop.InfraStructure.Data.Repositories;
using Vekalat.Core.Entities;
using Vekalat.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Vekalat.Application.Features.EquipmentGalleryFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class EquipmentGalleryRepository : Repository<EquipmentGallery>, IEquipmentGalleryRepository
    {
        private readonly VekalatDataContext _context;
        public EquipmentGalleryRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable<EquipmentGallery> GetAllWithFilter(EquipmentGalleryFilterInput studioGalleryFilterInput)
        {
            var items = _context.EquipmentGalleries.Include(c=>c.EquipmentFk)
            .OrderByDescending(e => e.Id)
            .AsQueryable();


            if (studioGalleryFilterInput.EquipmentId != null)
                items = items.Where(e => e.EquipmentId == studioGalleryFilterInput.EquipmentId);

            return items;
        }

    }
}
