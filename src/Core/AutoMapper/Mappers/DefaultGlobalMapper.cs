using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using YuzuDelivery.Core;


namespace YuzuDelivery.Umbraco.Core
{
    public class DefaultGlobalMapper : IYuzuGlobalMapper
    {
        public MethodInfo MakeGenericMethod(YuzuMapperSettings baseSettings)
        {
            if (baseSettings is YuzuGlobalMapperSettings settings)
            {
                var genericArguments = new List<Type>
                {
                    settings.Source,
                    settings.Dest
                };

                var method = GetType().GetMethod("CreateMap");
                return method.MakeGenericMethod(genericArguments.ToArray());
            }

            throw new Exception("Mapping settings not of type YuzuGlobalMapperSettings");
        }

        public AddedMapContext CreateMap<Source, Dest>(MapperConfigurationExpression cfg, YuzuMapperSettings baseSettings, IFactory factory, AddedMapContext mapContext, IYuzuConfiguration config)
        {
            if (baseSettings is YuzuGlobalMapperSettings settings)
            {
                if (settings.GroupName != null)
                    cfg.RecognizePrefixes(settings.GroupName);

                mapContext.AddOrGet<Source, Dest>(cfg);

                return mapContext;
            }
            throw new Exception("Mapping settings not of type YuzuGlobalMapperSettings");
        }
    }
}

