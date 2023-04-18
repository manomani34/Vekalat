using Vekalat.Application.Enums;
using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Vekalat.InfraStructure.Data.Factory
{
    public interface IVekalatDataContextFactory
    {
        VekalatDataContext Create();
    }
    public class VekalatDataContextFactory : IVekalatDataContextFactory
    {
        private readonly VekalatDbContextOptions options;
        public VekalatDataContextFactory(VekalatDbContextOptions options)
        {
            this.options = options;
        }

        public VekalatDataContext Create()
        {
            return new VekalatDataContext(options);
        }

        public VekalatDataContext CreateDbContext(string[] args)
        {
            return new VekalatDataContext(options);
        }
    }

    public class DataContextSeed : IDbContextSeed
    {
        private readonly IConfiguration configuration;
        public DataContextSeed(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void Seed(ModelBuilder builder)
        {
            var Brands = new List<Brand>()
            {
                new Brand { Id = 1, Title = "Sony ", IsActive = true, IsDeleted = false },
                new Brand { Id = 2, Title = "Canon", IsActive = true, IsDeleted = false }
            };
            builder.Entity<Brand>().HasData(Brands);

            var categories = new List<Category>()
            {
               new Category{Id= 1,Title="Digital Cameras",ParentId=null,LTitle="",IsDeleted=false,},
               new Category{Id= 2,Title="Mirrorless Cameras",ParentId=1,LTitle="",IsDeleted=false,},
               new Category{Id= 3,Title="Lens",ParentId=null,LTitle="",IsDeleted=false,},
            };
            builder.Entity<Category>().HasData(categories);



            var Links = new List<Link>()
            {
               new Link{Id= 1,Title="Book House", Order= 1, IsActive=true,Url="https://ketab.ir/",IsDeleted=false },
               new Link{Id= 2,Title="filmr Movie Collection",Order= 2,IsActive= true,Url="https://filmr.ir/",IsDeleted=false },
               new Link{Id= 3,Title="youtube",Order= 3,IsActive= true ,Url=" https://www.youtube.com/",IsDeleted=false },
               new Link{Id= 4,Title="Angular Js Material",Order= 4,IsActive= true,Url="https://material.angularjs.org/latest/demo/slider",IsDeleted=false },
            };
            builder.Entity<Link>().HasData(Links);

            var BlogSubjects = new List<BlogSubject>()
            {
                new BlogSubject{Id=1,Title="Camera ", IsDeleted=false },
                new BlogSubject{Id=2,Title="Lens   ", IsDeleted=false},
                new BlogSubject{Id=3,Title="Digital", IsDeleted = false}
            };
            builder.Entity<BlogSubject>().HasData(BlogSubjects);

            var Slids = new List<Slid>()
            {
                new Slid{Id=1,PictureURL="dd3b2f218e784547aa95fb0978fd4b60.jpg",Title="First Slide", IsActive= true,   IsDeleted=false },
                new Slid{Id=2,PictureURL="bbc1e72002284ffa8b217b07352aad84.jpg",Title="Slide Title 1", IsActive= true, IsDeleted=false},
                new Slid{Id=3,PictureURL="addd815ad89e400591090d7ae4fda3bf.jpg",Title="Slide Title 2", IsActive= true, IsDeleted=false}
            };
            builder.Entity<Slid>().HasData(Slids);

            var users = new List<User>()
            {
                new User { Id = 1,CreatorId=1,Email="admin@admin.com",UserType=UserType.Admin
                ,CreationTime=DateTime.Now,Address="",Firstname="admin",
                IsActive=true,IsDeleted=false,Lastname="admin",Mobil="09309759014"
                ,Notes="",PostCode="",Tel=""
                ,Password="E1-0A-DC-39-49-BA-59-AB-BE-56-E0-57-F2-0F-88-3E"},

                new User
                {
                    Id=2,Firstname="mohammad" ,Lastname="baher",Address="teh ",
                    Tel="02144545555",PostCode="0123456789",Email="baheri1389@gmail.com",
                    Mobil="09125025044",IsActive=true,IsDeleted=false,CreationTime=DateTime.Now,
                    Notes="",Password="E1-0A-DC-39-49-BA-59-AB-BE-56-E0-57-F2-0F-88-3E",
                    UserType = UserType.Admin
                }
            };
            builder.Entity<User>().HasData(users);
        }
    }
}
