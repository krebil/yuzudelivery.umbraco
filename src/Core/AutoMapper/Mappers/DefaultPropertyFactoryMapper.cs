using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Linq;
using System.Reflection;
using YuzuDelivery.Core;



namespace YuzuDelivery.Umbraco.Core
{
    public class DefaultPropertyFactoryMapper : IYuzuPropertyFactoryMapper
    {
        private readonly IMappingContextFactory contextFactory;

        public DefaultPropertyFactoryMapper(IMappingContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public MethodInfo MakeGenericMethod(YuzuMapperSettings baseSettings)
        {
            var settings = baseSettings as YuzuPropertyFactoryMapperSettings;

            if (settings != null)
            {
                var genericArguments =
                    settings.Factory.GetInterfaces().FirstOrDefault()?.GetGenericArguments().ToList();
                genericArguments?.Add(settings.Source);
                genericArguments?.Add(settings.Dest);
                genericArguments?.Add(settings.Factory);

                var method = GetType().GetMethod("CreateMap");
                if (genericArguments != null && method != null)
                    return method.MakeGenericMethod(genericArguments.ToArray());
            }
            throw new Exception("Mapping settings not of type YuzuPropertyFactoryMapperSettings");
        }

        public AddedMapContext CreateMap<DestMember, Source, Dest, TService>(MapperConfigurationExpression cfg,
            YuzuMapperSettings baseSettings, IFactory factory, AddedMapContext mapContext, IYuzuConfiguration config)
            where TService : class, IYuzuTypeFactory<DestMember>
        {
            if (baseSettings is YuzuPropertyFactoryMapperSettings settings)
            {
                config.AddActiveManualMap<TService, Dest>(settings.DestPropertyName);

                DestMember MappingFunction(Source m, Dest v, object o, ResolutionContext context)
                {
                    var propertyResolver = factory.GetInstance(typeof(TService)) as TService;
                    var yuzuContext = contextFactory.From<UmbracoMappingContext>(context.Items);
                    return propertyResolver.Create(yuzuContext);
                }

                var map = mapContext.AddOrGet<Source, Dest>(cfg);

                map.ForMember(settings.DestPropertyName, opt => opt.MapFrom(MappingFunction));

                return mapContext;
            }
            throw new Exception("Mapping settings not of type YuzuPropertyFactoryMapperSettings");
        }
    }
}