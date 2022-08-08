using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Linq;
using System.Reflection;
using YuzuDelivery.Core;


namespace YuzuDelivery.Umbraco.Core
{
    public class DefaultTypeAfterMapper : IYuzuTypeAfterMapper
    {
        private readonly IMappingContextFactory contextFactory;

        public DefaultTypeAfterMapper(IMappingContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public MethodInfo MakeGenericMethod(YuzuMapperSettings baseSettings)
        {
            if (baseSettings is YuzuTypeAfterMapperSettings settings)
            {
                var genericArguments = settings.Action.GetInterfaces().FirstOrDefault()?.GetGenericArguments().ToList();
                genericArguments?.Add(settings.Action);

                var method = GetType().GetMethod("CreateMap");
                if (genericArguments != null && method != null)
                    return method.MakeGenericMethod(genericArguments.ToArray());
            }

            throw new Exception("Mapping settings not of type YuzuTypeMappingSettings");
        }

        public AddedMapContext CreateMap<Source, Dest, Resolver>(MapperConfigurationExpression cfg,
            YuzuMapperSettings baseSettings, IFactory factory, AddedMapContext mapContext, IYuzuConfiguration config)
            where Resolver : class, IYuzuTypeAfterConvertor<Source, Dest>
        {
            if (baseSettings is YuzuTypeAfterMapperSettings)
            {
                config.AddActiveManualMap<Resolver, Dest>();

                var map = mapContext.AddOrGet<Source, Dest>(cfg);

                void MappingFunction(Source source, Dest dest, ResolutionContext context)
                {
                    var typeConvertor = factory.GetInstance(typeof(Resolver)) as Resolver;
                    var yuzuContext = contextFactory.From<UmbracoMappingContext>(context.Items);

                    typeConvertor.Apply(source, dest, yuzuContext);
                }

                map.AfterMap(MappingFunction);

                return mapContext;
            }

            throw new Exception("Mapping settings not of type YuzuTypeMappingSettings");
        }
    }
}