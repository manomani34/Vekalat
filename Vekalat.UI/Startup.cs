using AryanShop.InfraStructure.Data.Repositories;
using AryanShop.InfraStructure.Persistant.Configurations;
using AutoMapper;
using Ehda.InfraStructure.Data.Repositories;
using FluentValidation;
using Vekalat.Application.Common.Helpers;
using Vekalat.Application.Common.InfraServices;
using Vekalat.InfraStructure.Data;
using Vekalat.InfraStructure.Data.Factory;
using Vekalat.InfraStructure.Data.Repositories;
using Vekalat.InfraStructure.Data.Repositories.InfraRepository;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using static Vekalat.Application.Features.AccountFeature;
using static Vekalat.Application.Features.BlogFeature;
using static Vekalat.Application.Features.BlogSubjectFeature;
using static Vekalat.Application.Features.BrandFeature;
using static Vekalat.Application.Features.CategoryFeature;
using static Vekalat.Application.Features.CityFeature;
using static Vekalat.Application.Features.ContactMessageFeature;
using static Vekalat.Application.Features.CountryFeature;
using static Vekalat.Application.Features.EquipmentFeature;
using static Vekalat.Application.Features.EquipmentGalleryFeature;
using static Vekalat.Application.Features.EquipmentItemFeature;
using static Vekalat.Application.Features.EquipmentReservationFeature;
using static Vekalat.Application.Features.LinkFeature;
using static Vekalat.Application.Features.OrderDetailFeature;
using static Vekalat.Application.Features.OrderFeature;
using static Vekalat.Application.Features.SettingFeature;
using static Vekalat.Application.Features.SlidFeature;
using static Vekalat.Application.Features.StudioFeature;
using static Vekalat.Application.Features.StudioGalleryFeature;
using static Vekalat.Application.Features.TeamFeature;
using static Vekalat.Application.Features.TeamGalleryFeature;
using static Vekalat.Application.Features.TeamLogoFeature;
using static Vekalat.Application.Features.UserFeature;

namespace Vekalat.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region DI

            services.AddSingleton<IDbContextSeed, DataContextSeed>();
            var dbContextOptions = new DbContextOptionsBuilder<VekalatDataContext>()
                .UseSqlServer(Configuration.GetConnectionString("VekalatConnection"), x => x.MigrationsAssembly("Vekalat.Infrastructure.Data.Migrations")).Options;
            services.AddSingleton(dbContextOptions);
            var assembly = AppDomain.CurrentDomain.Load("Vekalat.Application");
            var uiAassembly = AppDomain.CurrentDomain.Load("Vekalat.UI");


            services.AddSingleton<VekalatDbContextOptions>();
            services.AddSingleton<IVekalatDataContextFactory, VekalatDataContextFactory>();
            services.AddDbContext<VekalatDataContext>();
            services.AddMediatR(assembly);
            services.AddAutoMapper(assembly);
            services.AddAutoMapper(uiAassembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddSingleton<IMapper, Mapper>();
            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            #endregion

            #region Repositories
            services.AddScoped<ITeamLogoRepository, TeamLogoRepository>();
            services.AddScoped<ITeamGalleryRepository, TeamGalleryRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ISlidRepository, SlidRepository>();
            services.AddScoped<ILinkRepository, LinkRepository>();
            services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBlogSubjectRepository, BlogSubjectRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<IEquipmentReservationRepository, EquipmentReservationRepository>();
            services.AddScoped<IStudioRepository, StudioRepository>();
            services.AddScoped<IStudioGalleryRepository, StudioGalleryRepository>();
            services.AddScoped<IEquipmentItemRepository, EquipmentItemRepository>();
            services.AddScoped<IEquipmentGalleryRepository, EquipmentGalleryRepository>();
            #endregion

            #region DB Configurations

            services.AddSingleton<IEntityTypeMap, CategoryConfiguration>();

            #endregion

            #region Infra Repository

            services.AddScoped<IAuthUserService, AuthUserService>();
            services.AddScoped<IFileSaver, FileSaver>();
            services.AddScoped<IRandomCodeGenerator, RandomCodeGenerator>();
            services.AddScoped<ISenderService, SenderService>();
            services.AddScoped<IRenderViewToString, RenderViewToString>();
            services.AddScoped<IPasswordHashService, PasswordHashService>();
            services.AddScoped<IGoogleRecaptcha, GoogleRecaptcha>();
            services.AddScoped(typeof(IPagerService<,>), typeof(PagerService<,>));

            #endregion

            #region For Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
            });
            #endregion


            services.AddControllersWithViews();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //  app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapAreaControllerRoute(
                //    name: "User",
                //    areaName: "User",
                //    pattern: "User/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                    name: "admin",
                    areaName: "admin",
                    pattern: "admin/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
