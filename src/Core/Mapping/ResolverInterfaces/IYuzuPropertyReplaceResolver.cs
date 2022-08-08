using YuzuDelivery.Core;

namespace YuzuDelivery.Umbraco.Core
{
    public interface IYuzuPropertyReplaceResolver<M, Type> : IYuzuPropertyReplaceResolver, IYuzuMappingResolver
    {
        Type Resolve(M source, UmbracoMappingContext context);
    }
}
