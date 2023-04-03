using Microsoft.AspNetCore.Routing.Template;
using Multitenancy.Exceptions;
using Multitenancy.TenantProvider;

namespace Multitenancy.TenantExtractors;

public class PathTenantRetriever<TKey> : ITenantRetriever
{
    public  string PathTemplate { get; set; }

    private static readonly string[] ValidParams = new[] {"TenantId", "TenantName"};

    protected static void ValidatePathTemplate(RouteTemplate routeTemplate)
    {
        var valid =  routeTemplate.Parameters.Select(p => p.Name).Except(ValidParams).Any();
        if (!valid)
        {
            throw new InvalidPathTemplateException("Path template should only contain {TenantId} and/or {TenantName} placeholders");
        }
    }
    public string RetrieveTenantIdentifier(HttpContext context, bool throwExceptionIfNotExists = true)
    {
        string path = context.Request.Path;
        var template = TemplateParser.Parse(PathTemplate);
        ValidatePathTemplate(template);
        var matcher = new TemplateMatcher(template, new RouteValueDictionary());
        var routeValues = new RouteValueDictionary();
        bool isMatch = matcher.TryMatch(new PathString(path), routeValues);

        if (!isMatch)
        {
            throw new TenantExtractionException($"Path does not match to template path:{path} template:{PathTemplate}");
        }

        string tenantId = routeValues["TenantId"].ToString();
        if (String.IsNullOrEmpty(tenantId))
        {
            return tenantId;
        }
        string tenantName = routeValues["TenantName"].ToString();
        if (String.IsNullOrEmpty(tenantName))
        {
            return tenantName;
        }

        if (throwExceptionIfNotExists)
        {
            throw new TenantExtractionException("Tenant identifier is empty");
        }

        return null;

    }
}