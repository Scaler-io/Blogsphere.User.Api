using Swashbuckle.AspNetCore.Filters;

namespace Blogsphere.Swagger.Examples.HealthCheck;
public sealed class HealthCheckResponseExample : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new {
            Status = "Healthy",
            Checks = new List<object>
            {
                new {
                    Name = "Database",
                    Status = "Healthy"
                }
            }
        };
    }
}
