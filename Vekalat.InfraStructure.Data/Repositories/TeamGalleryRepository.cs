using AryanShop.InfraStructure.Data.Repositories;
using Vekalat.Core.Entities;
using Vekalat.InfraStructure.Data;
using System.Linq;
using static Vekalat.Application.Features.TeamGalleryFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class TeamGalleryRepository : Repository<TeamGallery>, ITeamGalleryRepository
    {
        private readonly VekalatDataContext _context;
        public TeamGalleryRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable<TeamGallery> GetAllWithFilter(TeamGalleryFilterInput studioGalleryFilterInput)
        {
            var items = _context.TeamGalleries
            .OrderByDescending(e => e.Id)
            .AsQueryable();


            if (studioGalleryFilterInput.TeamId != null)
                items = items.Where(e => e.TeamId == studioGalleryFilterInput.TeamId);

            return items;
        }

    }
}
