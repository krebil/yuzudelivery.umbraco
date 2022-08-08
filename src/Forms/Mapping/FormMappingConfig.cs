using YuzuDelivery.Core;
using YuzuDelivery.Umbraco.Core;

namespace YuzuDelivery.Umbraco.Forms
{
    public class FormMappingConfig : YuzuMappingConfig
    {
        public FormMappingConfig()
        {
            ManualMaps.AddTypeReplace<FormTypeConvertor>();
            ManualMaps.AddTypeReplace<FormBuilderTypeConverter>();
        }
    }
}
