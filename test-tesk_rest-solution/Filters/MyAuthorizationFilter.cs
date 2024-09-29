using Hangfire.Dashboard;

namespace test_tesk_rest_solution.Filters;

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}