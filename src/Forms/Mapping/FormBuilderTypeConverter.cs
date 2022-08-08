﻿using System.Linq;
using Umbraco.Forms.Web.Models;
using YuzuDelivery.Umbraco.Core;

namespace YuzuDelivery.Umbraco.Forms
{
    public class FormBuilderTypeConverter : IYuzuTypeConvertor<FormViewModel, vmBlock_DataFormBuilder>
    {
        private readonly IFormElementMapGetter formElementMapGetter;

        public FormBuilderTypeConverter(IFormElementMapGetter formElementMapGetter)
        {
            this.formElementMapGetter = formElementMapGetter;
        }

        public virtual vmBlock_DataFormBuilder Convert(FormViewModel source, UmbracoMappingContext context)
        {
            if (source != null)
            {
                return new vmBlock_DataFormBuilder()
                {
                    Title = source.CurrentPage.Caption,
                    Fieldsets = source.CurrentPage.Fieldsets.Select(x => new vmSub_DataFormBuilderFieldset()
                    {
                        Legend = x.Caption,
                        Fields = x.Containers.Count == 1 ? formElementMapGetter.UmbracoFormParseFieldMappings(x.Containers) : null,
                        Rows = x.Containers.Count > 1 ? formElementMapGetter.UmbracoFormParseGridMappings(x.Containers) : null
                    }).ToList(),
                    Pages = source.Pages.Count > 1 ? source.Pages.Select(x => new vmSub_DataFormBuilderPage() { Title = x.Caption, Active = x == source.CurrentPage }).ToList() : null,
                    PreviousButtonText = !source.IsFirstPage ? source.PreviousCaption : string.Empty,
                    NextButtonText = !source.IsLastPage ? source.NextCaption : string.Empty,
                    SubmitButtonText = source.IsLastPage ? source.SubmitCaption : string.Empty
                };
            }
            return new vmBlock_DataFormBuilder();
        }
    }
}
