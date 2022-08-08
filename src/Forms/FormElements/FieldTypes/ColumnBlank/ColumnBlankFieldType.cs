using System;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;

namespace YuzuDelivery.Umbraco.Forms
{
    public static class _FormsConstant
    {
        public const string ColumnBlank_Name = "Column Blank";
        public const string ColumnBlank_Blank_Type = "BlankType";
        public const string ColumnBlank_SpanAll = "Span all";
        public const string ColumnBlank_BlankSpace = "Blank Space";
    }

    public class ColumnBlank : FieldType
    {
        public ColumnBlank()
        {
            Id = new Guid("21937a5a-42a0-49a4-b37f-dae3d1a65cf1");
            Name = _FormsConstant.ColumnBlank_Name;
            Description = "A blank field for spacing out fields in columns";
            Icon = "icon-backspace";
            DataType = FieldDataType.String;
            FieldTypeViewName = "Textstring.html";
            SortOrder = 123;
            SupportsRegex = true;
        }

        [Setting(_FormsConstant.ColumnBlank_Blank_Type, PreValues = "Span all,Blank Space", Description = "Does the blank force the field in the other column to span or is there a blank space", View = "Dropdownlist")]
        public string BlankType { get; set; }

    }
}