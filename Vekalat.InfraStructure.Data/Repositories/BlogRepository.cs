using Vekalat.Application.Features;
using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.BlogFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly VekalatDataContext _context;
        public BlogRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

    

        public IQueryable<Blog> GetAllWithFilter(BlogFilterInput blogFilterInput)
        {
            var items = _context.Blogs.Include(c => c.BlogSubjectFk)
           .OrderByDescending(e => e.Id)
        .AsQueryable();

            if (!string.IsNullOrEmpty(blogFilterInput.SearchFilter))
                items = items.Where(e => e.Title.Contains(blogFilterInput.SearchFilter));
            return items;
        }

    }
}
