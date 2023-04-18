using System.Security.Policy;
using Vekalat.InfraStructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Vekalat.InfraStructure.Persistant.Configurations
{
    //public class UserConfiguration : BaseEntityMap<User>
    //{
    //    protected override void InternalMap(EntityTypeBuilder<User> builder)
    //    {
    //        builder.HasOne<Publisher>(s => s.Publisher).WithOne(ad => ad.User).HasForeignKey<Publisher>(ad => ad.UserId);

    //        builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
    //        builder.Property(u => u.NationalCode).IsRequired().HasMaxLength(150);
    //        builder.Property(u => u.Password).IsRequired().HasMaxLength(150);
    //    }
    //}

}
