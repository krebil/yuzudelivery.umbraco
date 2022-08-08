using System.Reflection;
using YuzuDelivery.Umbraco.BlockList;

#if NETCOREAPP
using Microsoft.Extensions.DependencyInjection;
#else
using Umbraco.Core.Composing;
#endif

namespace YuzuDelivery.Umbraco.Core
{
    public static class CompositionExtensions
    {
#if NETCOREAPP
        public static void RegisterBlockListStrategies(this IServiceCollection services, Assembly assembly)
        {
            services.RegisterAll<IGridItem>(assembly);
        }
#else
        public static void RegisterBlockListStrategies(this Composition composition, Assembly assembly)
        {
            composition.RegisterAll<IGridItem>(assembly);
        }
#endif
    }
}
