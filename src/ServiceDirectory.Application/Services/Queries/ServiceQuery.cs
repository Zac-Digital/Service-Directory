using ServiceDirectory.Domain.ServiceList;
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

    public ServiceList GetServicesByLocation(double latitude, double longitude, int pageNumber)
    {
        const double earthRadiusInMetres = 6378100.0d;
        const double degreesToRadians = Math.PI / 180.0d;
        const double metresToMiles = 0.000621371d;
        const double metresPerRadian = 111320.0d;

        const double twentyMilesInMetres = 32186.9d; // TODO: Temporary, will be capped by UI filters

        const double latitudeDelta = twentyMilesInMetres / metresPerRadian; // TODO: Temporary, will be capped by UI filters
        double longitudeDelta = twentyMilesInMetres / (111320.0 * Math.Cos(latitude * degreesToRadians));

        double minLatitude = latitude - latitudeDelta;
        double maxLatitude = latitude + latitudeDelta;
        double minLongitude = longitude - longitudeDelta;
        double maxLongitude = longitude + longitudeDelta;

        // Haversine Algorithm translated to LINQ
        // Credit to GitHub User https://github.com/hypersolutions
        // Repository https://github.com/hypersolutions/service-directory for the implementation :)
        IQueryable<Service> serviceList =
        (
            from s in _applicationDbContext.Services
            from l in s.Locations
            where l.Latitude >= minLatitude && l.Latitude <= maxLatitude && l.Longitude >= minLongitude &&
                  l.Longitude <= maxLongitude
            let d = 2 * earthRadiusInMetres * Math.Asin(
                Math.Sqrt(Math.Pow(Math.Sin((latitude - l.Latitude) * degreesToRadians / 2), 2) +
                          Math.Cos(l.Latitude * degreesToRadians) *
                          Math.Cos(latitude * degreesToRadians) *
                          Math.Pow(Math.Sin((longitude - l.Longitude) * degreesToRadians / 2), 2)))
            where d <= twentyMilesInMetres // TODO: Temporary, will be capped by UI filters
            orderby d
            select new Service
            {
                OrganisationId = s.OrganisationId,
                Name = s.Name,
                Description = s.Description,
                Cost = s.Cost,
                DistanceInMiles = d * metresToMiles,
                Schedule = s.Schedule
            }
        );

        int totalCount = serviceList.Count();
        IEnumerable<Service> result = serviceList.Skip((pageNumber - 1) * 10).Take(10);

        return new ServiceList(result, totalCount);
    }
}