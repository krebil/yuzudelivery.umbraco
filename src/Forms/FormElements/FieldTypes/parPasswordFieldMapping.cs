﻿using System.Linq;
using Umbraco.Forms.Mvc.Models;

namespace YuzuDelivery.Umbraco.Forms
{
    public class parPasswordFieldMapping : IFormFieldMappingsInternal
    {
        public bool IsValid(string name)
        {
            return name == "Password";
        }

        public object Apply(FieldViewModel model)
        {
            return new vmBlock_FormTextInput()
            {
                Name = model.Id,
                Type = "password",
                Label = model.Caption,
                Value = model.Values.Select(x => x.ToString()).FirstOrDefault(),
                IsRequired = model.Mandatory,
                RequiredMessage = model.RequiredErrorMessage,
                Placeholder = model.PlaceholderText,
                Pattern = model.Regex,
                PatternMessage = model.InvalidErrorMessage,
                Conditions = model.Condition != null && model.Condition.Rules.Any() ? model.Condition : null,
                _ref = "parFormTextInput"
            };
        }

    }
}
