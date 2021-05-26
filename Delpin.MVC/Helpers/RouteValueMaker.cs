using Microsoft.AspNetCore.Routing;

namespace Delpin.Mvc.Helpers
{
    // Helper to make a short inline route value dictionary 
    public class RouteValueMaker
    {
        public RouteValueDictionary GetRoute(string key, string value)
        {
            return new RouteValueDictionary { { key, value } };
        }
    }
}
