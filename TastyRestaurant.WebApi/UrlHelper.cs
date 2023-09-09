namespace TastyRestaurant.WebApi;

public static class UrlHelper
{
    public static string GetResourceLocationUrl(HttpContext context, string relativePath)
    {
        var baseUrl = $"{context.Request.Scheme}://{context.Request.Host.ToUriComponent()}";
        var locationUrl = baseUrl + "/" + relativePath;

        return locationUrl;
    }
}