using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vekalat.Core.Entities;
using Vekalat.Application.Features;
using static Vekalat.Application.Features.SettingFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class SettingRepository : Repository<Setting>, ISettingRepository
    {
        private readonly VekalatDataContext _context;
        public SettingRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }



    }
}
