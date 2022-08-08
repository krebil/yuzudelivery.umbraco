using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using YuzuDelivery.Core;


namespace YuzuDelivery.Umbraco.Core
{
    public class DefaultGroupMapper : IYuzuGroupMapper
    {
        public MethodInfo MakeGenericMethod(YuzuMapperSettings baseSettings)
        {
            if (baseSettings is YuzuGroupMapperSettings settings)
            {
                var genericArguments = new List<Type>();
                genericArguments.Add(settings.Source);
                genericArguments.Add(settings.DestParent);
                genericArguments.Add(settings.DestChild);

                var method = GetType().GetMethod("CreateMap");
                return method.MakeGenericMethod(genericArguments.ToArray());
            }

            throw new Exception("Mapping settings not of type YuzuGroupMapperSettings");
        }

        public AddedMapContext CreateMap<Source, DestParent, DestChild>(MapperConfigurationExpression cfg, YuzuMapperSettings baseSettings, IFactory factory, AddedMapContext mapContext, IYuzuConfiguration config)
        {
            if (baseSettings is YuzuGroupMapperSettings settings)
            {
                var groupNameWithoutSpaces = settings.GroupName.Replace(" ", "");

                cfg.RecognizePrefixes(groupNameWithoutSpaces);

                mapContext.AddOrGet<Source, DestChild>(cfg);

                var parentMap = mapContext.AddOrGet<Source, DestParent>(cfg);
                parentMap.ForMember(settings.PropertyName, opt => opt.MapFrom(y => y));


                return mapContext;
            }

            throw new Exception("Mapping settings not of type YuzuGroupMapperSettings");
        }
    }
}
