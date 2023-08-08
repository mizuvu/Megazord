using Zord.Core.Repositories;
using Zord.EntityFrameworkCore;

namespace Host.Data.Persistence;

public interface ILocationRepository : IRepository<RetailLocation>
{
}

public class LocationRepository : Repository<RetailLocation, AlphaDbContext>, IRepository<RetailLocation>
{
    public LocationRepository(AlphaDbContext context) : base(context)
    {
    }

    public override Task<IEnumerable<RetailLocation>> ToListAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Loaded data to cache");
        return base.ToListAsync(cancellationToken);
    }
}
