using System;
using System.Linq;
using YuzuDelivery.Core;
using YuzuDelivery.Umbraco.Core;
using YuzuDelivery.Umbraco.Import;

#if NETCOREAPP
using Umbraco.Cms.Core.Models.Blocks;

#else
using Umbraco.Core.Models.Blocks;
#endif


namespace YuzuDelivery.Umbraco.BlockList
{
    public class BlockListAutoMapping : YuzuMappingConfig
    {
        public BlockListAutoMapping(IVmPropertyMappingsFinder vmPropertyMappingsFinder)
        {
            var mappings = vmPropertyMappingsFinder.GetBlockMappings(typeof(BlockListModel));

            foreach (var convertorType in from i in mappings where i.SourceType != null select i.DestProperty.PropertyType into destPropertyType let generics = destPropertyType.GetGenericArguments() let convertorType = (Type)null select generics.Any() ? typeof(BlockListToListOfTypesConvertor<>).MakeGenericType(generics.FirstOrDefault()) : typeof(BlockListToTypeConvertor<>).MakeGenericType(destPropertyType))
            {
                ManualMaps.Add(new YuzuTypeConvertorMapperSettings()
                {
                    Mapper = typeof(IYuzuTypeConvertorMapper),
                    Convertor = convertorType
                });
            }
        }
    }
}