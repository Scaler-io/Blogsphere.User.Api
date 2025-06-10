using System.Diagnostics;
using Blogsphere.User.Application.Contracts.ActivityTracker;
using Blogsphere.User.Domain.Models.Constants;
using Blogsphere.User.Domain.Models.Enums;

namespace Blogsphere.User.Application.ActivityTracker;

public class ActivityTracker(ActivitySource activitySource) : ActivityTrackerBase, IActivityTracker
{
    private readonly ActivitySource _activitySource = activitySource;

    protected override Activity? StartActivity(string name, ActivityKind activityKind = ActivityKind.Internal, Action<Activity>? configure = null)
    {
        var activity = _activitySource.StartActivity(name, activityKind);
        if (activity != null && configure != null)
        {
            configure(activity);
        }
        return activity;
    }

    public Activity? TrackRedisActivity(string operationName, string cacheKey)
    {
        return StartActivity($"Redis caching {operationName}", ActivityKind.Client, activity =>
        {
            activity.SetTag(TrackerConstants.CacheType, CacheServiceTypes.Distributed);
            activity.SetTag(TrackerConstants.CacheOperation, operationName);
            activity.SetTag(TrackerConstants.CacheKey, cacheKey);
        });
    }
    
    public Activity? TrackInMemoryActivity(string operationName, string cacheKey)
    {
        return StartActivity($"Inmemory caching {operationName}", ActivityKind.Client, activity =>
        {
            activity.SetTag(TrackerConstants.CacheType, CacheServiceTypes.InMemory);
            activity.SetTag(TrackerConstants.CacheOperation, operationName);
            activity.SetTag(TrackerConstants.CacheKey, cacheKey);
        });
    }

    public Activity? TrackCommandActivity(string commandName, params (string key, object? value)[] tags)
    {
        return StartActivity(commandName, ActivityKind.Internal, activity =>
         {
             activity.SetTag(TrackerConstants.CommandName, commandName);
             foreach (var (key, value) in tags)
             {
                 activity.SetTag(key, value);
             }
         });
    }

    public void Dispose() => _activitySource.Dispose();
}
