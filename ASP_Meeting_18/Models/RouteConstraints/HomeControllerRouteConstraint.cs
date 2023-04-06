namespace ASP_Meeting_18.Models.RouteConstraints
{
    public class HomeControllerRouteConstraint : IRouteConstraint
    {

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values["controller"] != null)
            {
                string str = values["controller"].ToString().ToLower();
                return str != "home" && str != "account";
            }
            return false;
        }
    }
}
