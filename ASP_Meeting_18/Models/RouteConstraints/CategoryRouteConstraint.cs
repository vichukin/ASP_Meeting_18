namespace ASP_Meeting_18.Models.RouteConstraints
{
    public class CategoryRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string? str = values["category"].ToString().First().ToString();
            //if (int.TryParse( str,out int res))
            //{
            //    return false;
            //}
            
            return true;
        }
    }
}
