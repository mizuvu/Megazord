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

    public override Task<IEnumerable<RetailLocation>> GetAllAsync(CancellationToken cancellationToken = default, bool tracking = false)
    {
        Console.WriteLine("Loaded data to cache");
        return base.GetAllAsync(cancellationToken, tracking);
    }
}
