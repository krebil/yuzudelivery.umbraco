#if NETCOREAPP
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using YuzuDelivery.Umbraco.Core;
using YuzuDelivery.Umbraco.Import;

namespace Standalone
{
    public class YuzuExampleComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<GenerateController>();
            builder.Services.AddTransient<YuzuContentImportController>();
            builder.Services.AddTransient<Umbraco.Cms.Web.BackOffice.ModelsBuilder.ModelsBuilderDashboardController>();
        }
    }
}
#endif