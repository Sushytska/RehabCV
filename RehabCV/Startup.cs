using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RehabCV.Database;
using Microsoft.EntityFrameworkCore;
using RehabCV.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using RehabCV.Repositories;
using RehabCV.Interfaces;
using RehabCV.DTO;
using RehabCV.HostServices;
using RehabCV.Working;
using RehabCV.Services;
using RehabCV.Configurations;

namespace RehabCV
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
            services.Configure<ApplicationConfiguration>(Configuration.GetSection("ApplicationConfiguration"));
            services.AddDbContext<RehabCVContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();

            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<RehabCVContext>()
            .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });

            services.AddScoped<IClild<Child>, ChildRepository>();
            services.AddScoped<IEvent<Event>, EventRepository>();
            services.AddScoped<IRehabilitation<Rehabilitation>, RehabRepository>();
            services.AddScoped<IQueue<Queue>, QueueRepository>();
            services.AddScoped<IGroup<Group>, GroupRepository>();
            services.AddScoped<INumberOfCh<NumberOfChildren>, NumberOfChRepository>();
            services.AddScoped<IReserve<Reserve>, ReserveRepository>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddHostedService<OneDayHostedService>();
            services.AddSingleton<IWorker, Worker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            CreateRoles(serviceProvider).GetAwaiter().GetResult();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { "Admin", "Parent" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var loginOfAdmin = Configuration["ApplicationConfiguration:LoginOfAdmin"];
            var passwordOfAdmin = Configuration["ApplicationConfiguration:PasswordOfAdmin"];

            var _user = await UserManager.FindByEmailAsync(loginOfAdmin);

            if (_user == null)
            {
                var poweruser = new User
                {
                    UserName = loginOfAdmin,
                    Email = loginOfAdmin,
                    EmailConfirmed = true
                };

                string adminPassword = passwordOfAdmin;

                var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword);

                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }
    }
}
