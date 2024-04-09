using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using sakeny.DbContexts;
using sakeny.Entities;
using sakeny.Hubs;
using sakeny.Models.ChatDtos;
using sakeny.Services;
using Serilog;
using System.Text;

namespace sakeny
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().
                MinimumLevel.Debug().
                WriteTo.Console().
                WriteTo.File("logs\\sakeny.txt", rollingInterval: RollingInterval.Day).
                CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            options.ReturnHttpNotAcceptable = true
            ).AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddXmlDataContractSerializerFormatters();


            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen( );

            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new() { Title = "sakeny", Version = "v1" });
            //    c.SchemaFilter<IgnorePropertiesSchemaFilter>();
            //});

            builder.Services.AddDbContext<HOUSE_RENT_DBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:SakenyDbConnectionString"]);
            });

            builder.Services.AddAuthentication("Bearer")
                 .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme ,options =>
                   {
                       options.TokenValidationParameters = new()
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = builder.Configuration["Authentication:Issuer"],
                           ValidAudience = builder.Configuration["Authentication:Audience"],
                           IssuerSigningKey = new SymmetricSecurityKey(
                               Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
                       };
                   }
            );

            //builder.Services.AddDefaultIdentity<ApplicationUser>()
            //             .AddEntityFrameworkStores<HOUSE_RENT_DBContext>();


            //builder.Services.AddAuthorization();

            //builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            //{
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireDigit = false;
            //}).AddEntityFrameworkStores<HOUSE_RENT_DBContext>();

            builder.Services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.Services.AddSingleton<sharedDb>();
            builder.Services.AddScoped<IUserInfoRepository, UserInfoRepositorycs>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            if (true)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
           
            //}


            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("/chat");
            app.MapHub<NotificationHub>("/notification");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}