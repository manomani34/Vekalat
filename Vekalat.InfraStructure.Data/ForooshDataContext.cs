using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Vekalat.InfraStructure.Data
{

    public class VekalatDbContextOptions
    {
        public readonly DbContextOptions<VekalatDataContext> Options;
        public readonly IDbContextSeed DBContextSeed;
        public readonly IEnumerable<IEntityTypeMap> Mappings;

        public VekalatDbContextOptions(DbContextOptions<VekalatDataContext> options, IDbContextSeed dbContextSeed, IEnumerable<IEntityTypeMap> mappings)
        {
            this.Options = options;
            this.DBContextSeed = dbContextSeed;
            this.Mappings = mappings;
        }
    }

    public class VekalatDataContext : DbContext
    {
        private readonly VekalatDbContextOptions options;
        public VekalatDataContext(VekalatDbContextOptions options) : base(options.Options)
        {
            this.options = options;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            foreach (var mapping in options.Mappings)
            {
                mapping.Map(builder);
            }
            options.DBContextSeed.Seed(builder);

        }
        #region DBSETS
        public DbSet<TeamLogo>  TeamLogos { get; set; }
        public DbSet<TeamGallery>  TeamGalleries { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Slid> Slids { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<BlogSubject> BlogSubjects { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentGallery> EquipmentGalleries { get; set; }
        public DbSet<EquipmentItem> EquipmentItems { get; set; }
        public DbSet<EquipmentReservation> EquipmentReservations { get; set; }
        public DbSet<EquipmentReservationHistory> EquipmentReservationHistories { get; set; }
        public DbSet<AccountName> AccountNames { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudioGallery> StudioGalleries { get; set; }
        public DbSet<StudioReservation> StudioReservations { get; set; }
        public DbSet<StudioReservationHistory> StudioReservationHistories { get; set; }

        #endregion
    }
    public static class VekalatDbContextExtensions
    {
        public static DbSet<TEntityType> DbSet<TEntityType>(this VekalatDataContext context)
            where TEntityType : class
        {
            return context.Set<TEntityType>();
        }
    }
}
