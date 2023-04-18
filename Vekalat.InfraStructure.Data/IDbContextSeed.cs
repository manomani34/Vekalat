using Microsoft.EntityFrameworkCore;

namespace Vekalat.InfraStructure.Data
{
    public interface IDbContextSeed
    {
        void Seed(ModelBuilder builder);
    }
}
