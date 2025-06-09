using System.Diagnostics;

namespace Blogsphere.User.Application.Contracts.ActivityTracker;

public interface IActivityTracker : IDisposable
{
    Activity? TrackRedisActivity(string operationName, string cacheKey);
    Activity? TrackInMemoryActivity(string operationName, string cacheKey);
    Activity? TrackCommandActivity(string commandName, params (string key, object? value)[] tags);
}
