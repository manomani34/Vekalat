using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Vekalat.Application.Features.SlidFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class SlidRepository : Repository<Slid>, ISlidRepository
    {
        private readonly VekalatDataContext _context;
        public SlidRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }
       
        public async Task<List<Slid>> GetActive()
        {
            return await _context.Slids.Where(e => e.IsActive == true).Take(3).ToListAsync();
        }

       
    }
}
