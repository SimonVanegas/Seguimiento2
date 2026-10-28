using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SGCAE.Infrastructure.Persistence;

public class SgcaeDbContextFactory : IDesignTimeDbContextFactory<SgcaeDbContext>
{
    public SgcaeDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SgcaeDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=SgcaeDb;User Id=sa;Password=TuPasswordSeguro123!;TrustServerCertificate=True;MultipleActiveResultSets=True;");

        return new SgcaeDbContext(optionsBuilder.Options);
    }
}
