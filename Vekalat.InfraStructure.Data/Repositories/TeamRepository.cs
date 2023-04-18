using Vekalat.Application.Features;
using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.TeamFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        private readonly VekalatDataContext _context;
        public TeamRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

    

        public IQueryable<Team> GetAllWithFilter(TeamFilterInput TeamFilterInput)
        {
            var items = _context.Teams
                .OrderByDescending(e => e.Id)
        .AsQueryable();

            if (!string.IsNullOrEmpty(TeamFilterInput.SearchFilter))
                items = items.Where(e => e.SurName.Contains(TeamFilterInput.SearchFilter));
            return items;
        }

    }
}
