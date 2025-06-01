using ServiceDirectory.Domain.Service;

namespace ServiceDirectory.Application.Services.Queries;

public interface IServiceQuery
{
    public IEnumerable<Service> GetServicesByLocation(double latitude, double longitude);
}
