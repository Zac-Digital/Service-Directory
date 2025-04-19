using ServiceDirectory.Domain.Service;

namespace ServiceDirectory.Infrastructure.Data;

public interface IApplicationDbContext
{
    public IQueryable<Organisation> Organisations { get; }
    public IQueryable<Service> Services { get; }
    public IQueryable<Location> Locations { get; }
}