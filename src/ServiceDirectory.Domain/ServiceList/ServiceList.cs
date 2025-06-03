namespace ServiceDirectory.Domain.ServiceList;

public record ServiceList(IEnumerable<Service.Service> Services, int Total);
