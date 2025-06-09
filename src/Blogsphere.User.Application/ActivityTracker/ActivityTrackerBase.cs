using System.Diagnostics;

namespace Blogsphere.User.Application.ActivityTracker;

public abstract class ActivityTrackerBase
{
    protected abstract Activity? StartActivity(string name, ActivityKind activityKind = ActivityKind.Internal, Action<Activity>? configure = null);
}
