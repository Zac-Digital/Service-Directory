using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Domain.Service;
using ServiceDirectory.Infrastructure.Data;

namespace ServiceDirectory.Application.Services.Queries;

public class ServiceQuery : IServiceQuery
{
    private readonly IApplicationDbContext _applicationDbContext;

    public ServiceQuery(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public IEnumerable<Service> GetServicesByLocation(double latitude, double longitude)
    {
        const double earthRadiusInMetres = 6378100.0d;
        const double degreesToRadians = Math.PI / 180.0d;
        const double metersToMiles = 0.000621371d;

        // Haversine Algorithm translated to LINQ
        // Credit to GitHub User https://github.com/hypersolutions
        // Repository https://github.com/hypersolutions/service-directory for the implementation :)
        IQueryable<Service> serviceList =
            (
                from s in _applicationDbContext.Services
                from l in s.Locations
                let d = 2 * earthRadiusInMetres * Math.Asin(
                    Math.Sqrt(Math.Pow(Math.Sin((latitude - l.Latitude) * degreesToRadians / 2), 2) +
                              Math.Cos(l.Latitude * degreesToRadians) *
                              Math.Cos(latitude * degreesToRadians) *
                              Math.Pow(Math.Sin((longitude - l.Longitude) * degreesToRadians / 2), 2)))
                where d <= earthRadiusInMetres // TODO: Temporary, will be capped by UI filters
                orderby d
                select new Service
                {
                    OrganisationId = s.OrganisationId,
                    Name = s.Name,
                    Description = s.Description,
                    Cost = s.Cost,
                    DistanceInMiles = d * metersToMiles,
                    Schedule = s.Schedule
                }
            )
            .Take(10);

        foreach (Service service in serviceList)
        {
            yield return service;
        }
    }
}
