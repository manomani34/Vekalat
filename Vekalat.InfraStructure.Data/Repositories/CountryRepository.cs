using Vekalat.Application.Features;
using Vekalat.Core.Entities;
using System.Linq;
using static Vekalat.Application.Features.CountryFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {

        private readonly VekalatDataContext _context;
        public CountryRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable<CountryFeature.CountryDto> GetCountryQuery(string Title)
        {
            var query = _context.Countries.AsQueryable();
            bool l = false;

            if (!string.IsNullOrEmpty(Title))
            {
                query = query.Where(b => b.Title.Contains(Title));
                l = true;
            }
            if (l == false)
                query = query.Where(b => b.Id > 0);
            return query.Select(m => new CountryFeature.CountryDto()
            {
                Id = m.Id,
                Title = m.Title
            });
        }
    }
}
