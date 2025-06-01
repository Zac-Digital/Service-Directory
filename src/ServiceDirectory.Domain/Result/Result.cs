namespace ServiceDirectory.Domain.Result;

public record Result(IEnumerable<Service.Service> Services, int Total);
