using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Linq;
using System.Reflection;
using YuzuDelivery.Umbraco.Import;
using YuzuDelivery.Core;


namespace YuzuDelivery.Umbraco.Core
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DefaultTypeConvertorMapper : IYuzuTypeConvertorMapper
    {
        private readonly IYuzuDeliveryImportConfiguration importConfig;
        private readonly IMappingContextFactory contextFactory;

        public DefaultTypeConvertorMapper(IYuzuDeliveryImportConfiguration importConfig,
            IMappingContextFactory contextFactory)
        {
            this.importConfig = importConfig;
            this.contextFactory = contextFactory;
        }

        public MethodInfo MakeGenericMethod(YuzuMapperSettings baseSettings)
        {
            if (baseSettings is YuzuTypeConvertorMapperSettings settings)
            {
                var genericArguments =
                    settings.Convertor.GetInterfaces().FirstOrDefault()?.GetGenericArguments().ToList();
                genericArguments?.Add(settings.Convertor);

                var method = GetType().GetMethod("CreateMap");
                if (genericArguments != null && method != null)
                    return method.MakeGenericMethod(genericArguments.ToArray());
            }

            throw new Exception("Mapping settings not of type YuzuTypeMappingSettings");
        }

        public AddedMapContext CreateMap<Source, Dest, TService>(MapperConfigurationExpression cfg,
            YuzuMapperSettings baseSettings, IFactory factory, AddedMapContext mapContext, IYuzuConfiguration config)
            where TService : class, IYuzuTypeConvertor<Source, Dest>
        {
            if (baseSettings is YuzuTypeConvertorMapperSettings settings)
            {
                config.AddActiveManualMap<TService, Dest>();

                if (settings.IgnoreReturnType)
                    importConfig.IgnoreViewmodels.Add(typeof(Dest).Name);

                var map = mapContext.AddOrGet<Source, Dest>(cfg);

                Dest MappingFunction(Source source, Dest dest, ResolutionContext context)
                {
                    var typeConvertor = factory.GetInstance(typeof(TService)) as TService;
                    var yuzuContext = contextFactory.From<UmbracoMappingContext>(context.Items);

                    return typeConvertor.Convert(source, yuzuContext);
                }

                map.ConvertUsing(MappingFunction);

                return mapContext;
            }
            throw new Exception("Mapping settings not of type YuzuTypeMappingSettings");
        }
    }
}