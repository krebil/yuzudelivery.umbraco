using System.Collections.Generic;

namespace YuzuDelivery.Umbraco.PositionalContent
{
    public class BreakpointDimension_old
    {
        public string breakpointName { get; set; }
        public Dictionary<string, string> styles { get; set; }
        public object content { get; set; }
        public bool hidden { get; set; }
    }
}
