using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Vekalat.Application.Features.BrandFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly VekalatDataContext _context;
        public BrandRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetActive()
        {
            return await _context.Brands.Where(e => e.IsActive == true).ToListAsync();
        }

        public IQueryable<Brand> GetAllWithFilter(BrandFilterInput BrandFilterInput)
        {

            var items = _context.Brands
                .OrderByDescending(e => e.Id)
        .AsQueryable();

            if (!string.IsNullOrEmpty(BrandFilterInput.SearchFilter))
                items = items.Where(e => e.Title.Contains(BrandFilterInput.SearchFilter));
            return items;
        }
    }
}
