using AutoMapper.Configuration;
using System;
using System.Linq;
using System.Reflection;
using YuzuDelivery.Core;


namespace YuzuDelivery.Umbraco.Core
{
    public class DefaultTypeFactoryMapper : IYuzuTypeFactoryMapper
    {
        public MethodInfo MakeGenericMethod(YuzuMapperSettings baseSettings)
        {
            if (baseSettings is YuzuTypeFactoryMapperSettings settings)
            {
                var genericArguments =
                    settings.Factory.GetInterfaces().FirstOrDefault()?.GetGenericArguments().ToList();
                genericArguments?.Add(settings.Factory);

                var method = GetType().GetMethod("CreateMap");
                if (genericArguments != null && method != null)
                    return method.MakeGenericMethod(genericArguments.ToArray());
            }
            throw new Exception("Mapping settings not of type YuzuTypeFactoryMapperSettings");
        }

        public AddedMapContext CreateMap<Dest, TService>(MapperConfigurationExpression cfg,
            YuzuMapperSettings baseSettings, IFactory factory, AddedMapContext mapContext, IYuzuConfiguration config)
            where TService : class, IYuzuTypeFactory<Dest>
        {
            if (baseSettings is YuzuTypeFactoryMapperSettings settings)
            {
                IYuzuTypeFactory GetFactory() => factory.GetInstance(typeof(TService)) as TService;

                if (!config.ViewmodelFactories.ContainsKey(settings.Dest))
                    config.ViewmodelFactories.Add(settings.Dest, GetFactory);
                config.AddActiveManualMap<TService, Dest>();

                return mapContext;
            }
            throw new Exception("Mapping settings not of type YuzuTypeFactoryMapperSettings");
        }
    }
}