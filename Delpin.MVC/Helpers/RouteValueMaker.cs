using Microsoft.AspNetCore.Routing;

namespace Delpin.Mvc.Helpers
{
    public class RouteValueMaker
    {
        public RouteValueDictionary GetRoute(string key, string value)
        {
            return new RouteValueDictionary { { key, value } };
        }
    }
}
