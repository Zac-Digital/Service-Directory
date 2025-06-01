using ServiceDirectory.Domain.Result;

namespace ServiceDirectory.Application.Services.Queries;

public interface IServiceQuery
{
    public Result GetServicesByLocation(double latitude, double longitude);
}
