using Microsoft.EntityFrameworkCore;

namespace Owniverse.Persistence;

public sealed class OwniverseDbContext(DbContextOptions<OwniverseDbContext> options)
    : DbContext(options)
{
}
