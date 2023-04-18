using Vekalat.Core.Entities;
using Vekalat.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Vekalat.Application.Features.StudioFeature;

namespace AryanShop.InfraStructure.Data.Repositories
{
    public class StudioRepository : Repository<Studio>, IStudioRepository
    {
        private readonly VekalatDataContext _context;
        public StudioRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

        //public async Task DeleteSelectedCategories(int itemId, CancellationToken cancellationToken)
        //{
        //    _context.BlogSelectedCategories.Where(g => g.BlogId == itemId).ToList().ForEach(g => _context.BlogSelectedCategories.Remove(g));
        //    await _context.SaveChangesAsync(cancellationToken);
        //}

        public IQueryable<Studio> GetAllWithFilter(StudioFilterInput FilterInput)
        {
            var items = _context.Studios
              .OrderByDescending(e => e.Id)
           .AsQueryable();

            if (!string.IsNullOrEmpty(FilterInput.SearchFilter))
                items = items.Where(e => e.Title.Contains(FilterInput.SearchFilter));
            return items;
        }
    }
}
