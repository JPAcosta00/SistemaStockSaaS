using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infraestructure.Data;

public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../API")) 
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Configura la base de datos
        builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        var dummyTenantProvider = new DummyTenantProvider();

        return new ApplicationDbContext(builder.Options, dummyTenantProvider);
    }
}


//esto se hizo para resolver un bug del EntityFramework
internal class DummyTenantProvider : Application.Interfaces.ITenantProvider
{
    public Guid? GetTenantId() => Guid.Empty; 
}