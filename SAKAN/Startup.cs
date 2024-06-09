using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SAKAN.Helpers;
using SAKAN.Models;
using SAKAN.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKAN
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

            services.AddControllers();

            services.AddSingleton(new PredictionService("D:\\SAKAN\\SAKAN\\MachineLearning\\kmeans_model.onnx"));

            services.AddCors(op =>
            {
                op.AddPolicy("Default", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                });
            });

            #region swagger config
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SAKAN", Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Asmaa Abdullah",
                        Email = "asmaa.abdullah.hussain@gmail.com"
                        
                    }
                });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
            {
                     new OpenApiSecurityScheme
                     {
                            Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
            });
            #endregion


            services.AddIdentity<ApplicationUser, IdentityRole>(options=> options.SignIn.RequireConfirmedAccount=true).AddEntityFrameworkStores<SakanEntity>();
            services.AddDbContext<SakanEntity>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<BuildingRepo, BuildingRepo>();
            services.AddScoped<FlatRepo, FlatRepo>();  
            services.AddScoped<RoomRepo, RoomRepo>();
            services.AddScoped<FileRepo,FileRepo>();
            services.AddScoped<BookingRepo, BookingRepo>();
            services.AddScoped<UserRepo, UserRepo>();
            services.AddScoped<HomeRepo, HomeRepo>();
            services.AddScoped<SearchRepo, SearchRepo>();


            services.AddAutoMapper(typeof(Startup));


            ////localization
            //services.AddLocalization(options => options.ResourcesPath = "Resources");
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var SuportedCultures = new[]
            //    {
            //        new CultureInfo("ar")
            //    };
            //    options.DefaultRequestCulture = new RequestCulture(culture: "ar");
            //    options.SupportedCultures = SuportedCultures;
            //}
            //);

            //change validation of user in identity schema
            services.Configure<IdentityOptions>(options =>
            {
                // Configure password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                
                
            });




            //[Authorize] check jwt in check authontication
            services.AddAuthentication(options =>
            {
                //check valid token only not check domain or cliams
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//check if Token valid
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//if not valid redirect to login
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;//mean any thing use scheme make default is jwt Bearer

            }).AddJwtBearer(options =>  //check cliams in Token like =>Expire, audience
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {

                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:Audience"],//check domian of Audience
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))

                };
            });

           // services.Configure<JWT>(Configuration.GetSection("JWT"));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                
                
            }
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SAKAN v1"));



            // Ensure the Uploads directory is used as the file provider root
            var uploadsDirectory = Path.Combine(env.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory); // Create directory if it doesn't exist
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(uploadsDirectory),
                RequestPath = "/images"// URL path to access files under Uploads directory
            });

            app.UseRouting();

            //var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            //app.UseRequestLocalization(localizationOptions.Value);

            app.UseCors("Default");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
