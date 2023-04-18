using Vekalat.Core.Entities;
using System.Linq;
using static Vekalat.Application.Features.CategoryFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly VekalatDataContext _context;
        public CategoryRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Category> GetAllWithFilter(CategoryFilterInput FilterInput)
        {
            var items = _context.Categories
                .OrderByDescending(e => e.Id)
             .AsQueryable();

            if (FilterInput.ParentId != null)
                items = items.Where(e => e.ParentId == FilterInput.ParentId);

            if (!string.IsNullOrEmpty(FilterInput.SearchFilter))
                items = items.Where(e => e.Title.Contains(FilterInput.SearchFilter));
            return items;
        }

        //public async Task<List<BlogSelectedCategory>> GetBlogSelectedCategory(int blogId)
        //{
        //    return await _context.BlogSelectedCategories.Where(c => c.BlogId == blogId).ToListAsync();
        //}

    }
}
