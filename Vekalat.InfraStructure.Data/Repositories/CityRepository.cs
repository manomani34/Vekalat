using Vekalat.Core.Entities;
using Vekalat.Application.Common;
using Vekalat.Application.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Vekalat.Application.Features.CityFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly VekalatDataContext _context;
        public CityRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable<CityFeature.CityDto> GetCityQuery(string Title)
        {
            var query = _context.Cities.AsQueryable();
            bool l = false;

            if (!string.IsNullOrEmpty(Title))
            {
                query = query.Where(b => b.Title.Contains(Title));
                l = true;
            }
            if (l == false)
                query = query.Where(b => b.Id > 0);
            return query.Select(m => new CityFeature.CityDto()
            {
                Id = m.Id,
                Title = m.Title
            });
        }        
    }
}
