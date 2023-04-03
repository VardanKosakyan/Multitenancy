using Microsoft.AspNetCore.Routing.Template;
using Multitenancy.Exceptions;

namespace Multitenancy.TenantExtractors.PolymorphicTenantRetriver;

public class PathMatchCondition : PolymorphicCondition
{
    //TODO may be properpies should me moved up in hierarchy
    protected string Target { get; set; }
    protected object ForwardData { get; set; }
    public override bool Satisfy(HttpContext context)
    {
        string path = context.Request.Path;
        var template = TemplateParser.Parse(Target);
        var matcher = new TemplateMatcher(template, new RouteValueDictionary());
        var routeValues = new RouteValueDictionary();
        bool isMatch = matcher.TryMatch(new PathString(path), routeValues);
        ForwardData = routeValues.ToDictionary(rv => rv.Key, rv => rv.Value.ToString());
        return isMatch;
    }
}