using AryanShop.InfraStructure.Data.Repositories;
using Vekalat.Core.Entities;
using Vekalat.InfraStructure.Data;
using System.Linq;
using static Vekalat.Application.Features.TeamLogoFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class TeamLogoRepository : Repository<TeamLogo>, ITeamLogoRepository
    {
        private readonly VekalatDataContext _context;
        public TeamLogoRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable<TeamLogo> GetAllWithFilter(TeamLogoFilterInput studioLogoFilterInput)
        {
            var items = _context.TeamLogos
            .OrderByDescending(e => e.Id)
            .AsQueryable();


            if (studioLogoFilterInput.TeamId != null)
                items = items.Where(e => e.TeamId == studioLogoFilterInput.TeamId);

            return items;
        }

    }
}
