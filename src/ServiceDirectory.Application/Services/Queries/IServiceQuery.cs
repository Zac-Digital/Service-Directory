using ServiceDirectory.Domain.ServiceList;

namespace ServiceDirectory.Application.Services.Queries;

public interface IServiceQuery
{
    public ServiceList GetServicesByLocation(double latitude, double longitude, int pageNumber);
}
