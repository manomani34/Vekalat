using Vekalat.Core.Entities;
using Vekalat.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Vekalat.Application.Features.EquipmentReservationFeature;

namespace AryanShop.InfraStructure.Data.Repositories
{
    public class EquipmentReservationRepository : Repository<EquipmentReservation>, IEquipmentReservationRepository
    {
        private readonly VekalatDataContext _context;
        public EquipmentReservationRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

        //public async Task DeleteSelectedCategories(int itemId, CancellationToken cancellationToken)
        //{
        //    _context.BlogSelectedCategories.Where(g => g.BlogId == itemId).ToList().ForEach(g => _context.BlogSelectedCategories.Remove(g));
        //    await _context.SaveChangesAsync(cancellationToken);
        //}

        public IQueryable<EquipmentReservation> GetAllWithFilter(EquipmentReservationFilterInput FilterInput)
        {
            var items = _context.EquipmentReservations.AsNoTracking().Include(c => c.UserFk).Include(c => c.EquipmentFk)
              .OrderByDescending(e => e.Id)
           .AsQueryable();

            //if (!string.IsNullOrEmpty(FilterInput.SearchFilter))
            //    items = items.Where(e => e.Title.Contains(FilterInput.SearchFilter));
            return items;
        }

        public async Task<bool> IsDateAlreadyReserved(int equipmentId, DateTime ReservedDate, DateTime ReturnDate)
        {
            return await _context.EquipmentReservations.AnyAsync(c => c.EquipmentId == equipmentId
            && (c.ReservedDate >= ReservedDate
           && c.ReturnDate <= ReturnDate));
        }
    }
}
