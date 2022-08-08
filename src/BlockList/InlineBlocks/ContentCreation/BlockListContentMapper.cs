using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using YuzuDelivery.Umbraco.Import;

#if NETCOREAPP

#else
#endif


namespace YuzuDelivery.Umbraco.BlockList
{
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class BlockListContentMapper : IContentMapper
    {
        protected IVmHelperService vmHelperService;
        protected BlockListDbModelFactory blockListDbModelFactory;

        public BlockListContentMapper(IVmHelperService vmHelperService, BlockListDbModelFactory blockListDbModelFactory)
        {
            this.vmHelperService = vmHelperService;
            this.blockListDbModelFactory = blockListDbModelFactory;
        }

        public bool IsValid(string propertyEditorAlias, ContentPropertyConfig config)
        {
            return propertyEditorAlias == "Umbraco.BlockList" && !config.IsGrid;
        }

        public string GetImportValue(VmToContentPropertyLink mapping, ContentPropertyConfig config, string content, string source, IContentMapperFactory factory, IContentImportPropertyService import)
        {
            var arrObjects = new JArray();

            //is array or single object
            var token = JToken.Parse(content);
            if (token is JArray)
            {
                arrObjects = JsonConvert.DeserializeObject<JArray>(content);
            }
            else if (token is JObject)
            {
                var obj = JsonConvert.DeserializeObject<JObject>(content);
                arrObjects.Add(obj);
            }

            var configVmLink = config.TypeName != null && config.TypeName.ToLower() != "object" ? vmHelperService.GetWithMapping(config.TypeName) : null;

            var arrOutput = blockListDbModelFactory.Create(arrObjects, factory, import, configVmLink);
            return JsonConvert.SerializeObject(arrOutput, Formatting.Indented);
        }
    }
}
