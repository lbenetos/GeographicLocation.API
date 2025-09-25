using System.Reflection;
using AutoMapper;
using GeographicLocation.Core;
using GeographicLocation.Core.Service;
using Microsoft.EntityFrameworkCore;

namespace GeographicLocation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(i =>
            {
                var xmlComment = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentPath = Path.Combine(AppContext.BaseDirectory, xmlComment);

                i.IncludeXmlComments(xmlCommentPath);
            });

            builder.Services.AddDbContext<LocationContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration["ConnectionString:IPLocation"]);
            });

            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();

            builder.Services.AddMemoryCachingServices(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            //Comment on main!
        }
    }
}
