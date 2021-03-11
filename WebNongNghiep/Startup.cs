
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Security.Claims;
using WebNongNghiep.Admin.InterfaceService;
using WebNongNghiep.Admin.Services;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.MessageMailView;
using WebNongNghiep.Client.Services;
using WebNongNghiep.Database;
using WebNongNghiep.Helper;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.Services;

namespace WebNongNghiep
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings")); // <--- added ;
            services.AddDbContext<MasterData>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //Admin service scoped
            services.AddScoped<IPhotoService, PhotoServices>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IBannerServices, BannerServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IBlogServices, BlogServices>();
            services.AddScoped<ICategoryBlogServices, CategoryBlogServices>();
            //CLient service scoped
            services.AddScoped<IClientAuthServices, ClientAuthServices>();
            services.AddScoped<IClientCategoryServices, ClientCategoryServices>();
            services.AddScoped<IClientGetFilterParamsServices, ClientGetFilterParamsServices>();
            services.AddScoped<IClientProductServices, ClientProductServices>();
            services.AddScoped<IClientOrderServices, ClientOrderServices>();
            services.AddScoped<IClientBlogServices, ClientBlogServices>();
            services.AddScoped<IClientEmailSenderServices, ClientEmailSenderServices>();

            //
            services.AddIdentity<User, IdentityRole>(opt =>
               {
                   opt.User.RequireUniqueEmail = true;

                   opt.SignIn.RequireConfirmedEmail = true; 
               })
                .AddEntityFrameworkStores<MasterData>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => { options.SlidingExpiration = true; options.ExpireTimeSpan = new TimeSpan(48, 0, 0); });

            services.AddControllers();
            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //Email Config
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);


            services.AddControllersWithViews();

            //Add authorization
            services.AddAuthorization(config =>
            {
                var userAuthPolicyBuilder = new AuthorizationPolicyBuilder();
                config.DefaultPolicy = userAuthPolicyBuilder
                                    .RequireAuthenticatedUser()
                                    .RequireClaim(ClaimTypes.Role)
                                    .Build();
            });
            services.AddControllersWithViews();
            //End

            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(options =>
            options.WithOrigins("https://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()); // allow credentials

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
