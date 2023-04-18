using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Vekalat.Application.Features.LinkFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class LinkRepository : Repository<Link>, ILinkRepository
    {
        private readonly VekalatDataContext _context;
        public LinkRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }



        public async Task<List<Link>> GetActive()
        {
            return await _context.Links.Where(e => e.IsActive == true).ToListAsync();
        }


    }
}
