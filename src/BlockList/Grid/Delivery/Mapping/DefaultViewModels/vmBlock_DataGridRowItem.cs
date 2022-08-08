
namespace YuzuDelivery.Umbraco.BlockList
{
    #pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.0.23.0 (Newtonsoft.Json v12.0.0.0)")]
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once InconsistentNaming
    public partial class vmBlock_DataGridRowItem
    {

        [Newtonsoft.Json.JsonProperty("config", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public object Config { get; set; }

        [Newtonsoft.Json.JsonProperty("content", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public object Content { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        // ReSharper disable once ConvertToAutoProperty
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get => additionalProperties;
            set => additionalProperties = value;
        }
    }
}
