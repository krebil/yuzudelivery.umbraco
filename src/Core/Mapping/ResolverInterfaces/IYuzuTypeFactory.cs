using YuzuDelivery.Core;

namespace YuzuDelivery.Umbraco.Core
{
    public interface IYuzuTypeFactory<Dest> : IYuzuTypeFactory, IYuzuMappingResolver
    {
        Dest Create(UmbracoMappingContext context);
    }
}
