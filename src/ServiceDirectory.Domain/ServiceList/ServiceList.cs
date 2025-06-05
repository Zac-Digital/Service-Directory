namespace ServiceDirectory.Domain.ServiceList;

public record ServiceList(List<Service.Service> Services, int Total);
