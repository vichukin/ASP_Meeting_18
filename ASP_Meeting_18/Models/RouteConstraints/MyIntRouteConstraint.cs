namespace ASP_Meeting_18.Models.RouteConstraints
{
    public class MyIntRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string str = values["page"].ToString();
            if (int.TryParse(str, out int res))
            {
                return true;
            }
            return false;
        }
    }
}
