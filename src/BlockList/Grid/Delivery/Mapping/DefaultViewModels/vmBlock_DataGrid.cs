using System.Collections.Generic;


namespace YuzuDelivery.Umbraco.BlockList
{
#pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.0.23.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class vmBlock_DataGrid
    {
        public vmBlock_DataGrid()
        {
            Rows = new List<vmSub_DataGridRow>();
        }

        [Newtonsoft.Json.JsonProperty("rows", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<vmSub_DataGridRow> Rows { get; set; }

        [Newtonsoft.Json.JsonProperty("_ref", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string _ref { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.0.23.0 (Newtonsoft.Json v12.0.0.0)")]
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class vmSub_DataGridRow
    {
        public vmSub_DataGridRow()
        {
            Columns = new List<vmSub_DataGridColumn>();
        }

        [Newtonsoft.Json.JsonProperty("config", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public object Config { get; set; }

        [Newtonsoft.Json.JsonProperty("columns", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<vmSub_DataGridColumn> Columns { get; set; }

        private IDictionary<string, object> additionalProperties = new Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get => additionalProperties;
            set => additionalProperties = value;
        }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.0.23.0 (Newtonsoft.Json v12.0.0.0)")]
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once InconsistentNaming
    public partial class vmSub_DataGridColumn
    {
        public vmSub_DataGridColumn()
        {
            Items = new List<vmBlock_DataGridRowItem>();
        }

        [Newtonsoft.Json.JsonProperty("gridSize", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public object GridSize { get; set; }

        [Newtonsoft.Json.JsonProperty("config", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public object Config { get; set; }

        [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<vmBlock_DataGridRowItem> Items { get; set; }

        private IDictionary<string, object> additionalProperties = new Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get => additionalProperties;
            set => additionalProperties = value;
        }


    }


}
