using Vekalat.Core.Entities;
using Vekalat.InfraStructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AryanShop.InfraStructure.Persistant.Configurations
{
    public class CategoryConfiguration : BaseEntityMap<Category>
    {
        protected override void InternalMap(EntityTypeBuilder<Category> builder)
        {       

            builder.HasOne<Category>().WithMany(p => p.Categories).HasForeignKey(p => p.ParentId);
        }
    }

}
