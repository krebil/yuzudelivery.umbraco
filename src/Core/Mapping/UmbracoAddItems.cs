﻿using System.Collections.Generic;
using YuzuDelivery.Core;

#if NETCOREAPP
using Umbraco.Cms.Core.Web;

#else
using Umbraco.Web;
#endif

namespace YuzuDelivery.Umbraco.Core
{
    public class UmbracoMapperAddItems : IMapperAddItem
    {
        private readonly IUmbracoContextAccessor umbracoContentAccessor;

        public UmbracoMapperAddItems(IUmbracoContextAccessor umbracoContentAccessor)
        {
            this.umbracoContentAccessor = umbracoContentAccessor;
        }

        public void Add(IDictionary<string, object> items)
        {
#if NETCOREAPP
            umbracoContentAccessor.TryGetUmbracoContext(out IUmbracoContext umbracoContext);
#else
            var umbracoContext = umbracoContentAccessor.UmbracoContext;
#endif
            if (umbracoContext != null && umbracoContext.PublishedRequest != null && umbracoContext.PublishedRequest.PublishedContent != null)
            {
                items.Add("Model", umbracoContext.PublishedRequest.PublishedContent);
            }
        }
    }
}
