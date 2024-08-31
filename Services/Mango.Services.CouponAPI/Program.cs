
using System.Runtime.CompilerServices;
using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("CouponAPI"));
        });

        IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
        builder.Services.AddSingleton(mapper);
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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

        ApplyMigrations(app);

        app.Run();
    }

    static void ApplyMigrations(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
        }
    }
}
