using Vekalat.Core.Entities;
using Vekalat.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static Vekalat.Application.Features.EquipmentItemFeature;

namespace AryanShop.InfraStructure.Data.Repositories
{
    public class EquipmentItemRepository : Repository<EquipmentItem>, IEquipmentItemRepository
    {
        private readonly VekalatDataContext _context;
        public EquipmentItemRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task CalculateEquipmentQuantity(int equipmentId)
        {
            var quantity = await _context.EquipmentItems.CountAsync(c => c.EquipmentId == equipmentId);
            var equipment = await _context.Equipments.FindAsync(equipmentId);
            if (equipment != null)
            {
                equipment.Quantity = quantity;
                _context.Update(equipment);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task DeleteSelectedCategories(int itemId, CancellationToken cancellationToken)
        //{
        //    _context.BlogSelectedCategories.Where(g => g.BlogId == itemId).ToList().ForEach(g => _context.BlogSelectedCategories.Remove(g));
        //    await _context.SaveChangesAsync(cancellationToken);
        //}

        public IQueryable<EquipmentItem> GetAllWithFilter(EquipmentItemFilterInput FilterInput)
        {
            var items = _context.EquipmentItems.Include(c => c.EquipmentFk)
              .OrderByDescending(e => e.Id)
           .AsQueryable();

            if (!string.IsNullOrEmpty(FilterInput.SearchFilter))
                items = items.Where(e => e.SerialNumber.Contains(FilterInput.SearchFilter));

            if (FilterInput.EquipmentId is not null)
                items = items.Where(e => e.EquipmentId == FilterInput.EquipmentId);
            return items;
        }
    }
}
