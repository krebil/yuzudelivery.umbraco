using AutoMapper.Configuration;
using System;
using System.Linq;
using System.Reflection;
using YuzuDelivery.Core;
using System.Linq.Expressions;


namespace YuzuDelivery.Umbraco.Core
{
    public class DefaultPropertyAfterMapper : IYuzuPropertyAfterMapper
    {
        public MethodInfo MakeGenericMethod(YuzuMapperSettings baseSettings)
        {
            if (baseSettings is YuzuPropertyAfterMapperSettings settings)
            {
                var genericArguments =
                    settings.Resolver.GetInterfaces().FirstOrDefault()?.GetGenericArguments().ToList();
                genericArguments?.Add(settings.Dest);
                genericArguments?.Add(settings.Resolver);

                var method = GetType().GetMethod("CreateMap");
                if (genericArguments != null) return method?.MakeGenericMethod(genericArguments.ToArray());
            }

            throw new Exception("Mapping settings not of type YuzuPropertyAfterMapperSettings");
        }

        public AddedMapContext CreateMap<Source, DestMember, Dest, Resolver>(MapperConfigurationExpression cfg,
            YuzuMapperSettings baseSettings, IFactory factory, AddedMapContext mapContext, IYuzuConfiguration config)
            where Resolver : class, IYuzuPropertyAfterResolver<Source, DestMember>
        {
            if (baseSettings is YuzuPropertyAfterMapperSettings settings)
            {
                //need a fix here
                //config.AddActiveManualMap<Resolver, Dest>(settings.DestProperty);

                if (!string.IsNullOrEmpty(settings.GroupName))
                    cfg.RecognizePrefixes(settings.GroupName);

                Func<DestMember, DestMember> mappingFunction = input =>
                {
                    var propertyResolver = factory.GetInstance(typeof(Resolver)) as Resolver;
                    return propertyResolver.Apply(input);
                };

                var map = mapContext.AddOrGet<Source, Dest>(cfg);

                map.ForMember(settings.DestProperty as Expression<Func<Dest, DestMember>>,
                    opt => opt.AddTransform(x => mappingFunction(x)));

                return mapContext;
            }
            else
                throw new Exception("Mapping settings not of type YuzuPropertyMappingSettings");
        }
    }
}