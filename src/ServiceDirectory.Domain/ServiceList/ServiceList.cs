namespace ServiceDirectory.Domain.ServiceList;

public sealed record ServiceList(List<Service.Service> Services, int Total);
