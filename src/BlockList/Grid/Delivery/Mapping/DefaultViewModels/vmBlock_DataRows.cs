using System.Collections.Generic;


namespace YuzuDelivery.Umbraco.BlockList
{
    #pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.0.23.0 (Newtonsoft.Json v12.0.0.0)")]
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once InconsistentNaming
    public partial class vmBlock_DataRows
    {
        public vmBlock_DataRows()
        {
            Rows = new List<vmSub_DataRowsRow>();
        }

        [Newtonsoft.Json.JsonProperty("rows", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<vmSub_DataRowsRow> Rows { get; set; }

        [Newtonsoft.Json.JsonProperty("_ref", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        // ReSharper disable once InconsistentNaming
        public string _ref { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.0.23.0 (Newtonsoft.Json v12.0.0.0)")]
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once InconsistentNaming
    public partial class vmSub_DataRowsRow
    {
        public vmSub_DataRowsRow()
        {
            Items = new List<vmBlock_DataGridRowItem>();
        }

        [Newtonsoft.Json.JsonProperty("config", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public object Config { get; set; }

        [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<vmBlock_DataGridRowItem> Items { get; set; }

        private IDictionary<string, object> additionalProperties = new Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        // ReSharper disable once ConvertToAutoProperty
        public IDictionary<string, object> AdditionalProperties
        {
            get => additionalProperties;
            set => additionalProperties = value;
        }

    }
}
