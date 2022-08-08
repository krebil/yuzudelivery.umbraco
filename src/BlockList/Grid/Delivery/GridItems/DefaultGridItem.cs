using System;
using System.Collections.Generic;
using YuzuDelivery.Core;

#if NETCOREAPP
using Umbraco.Cms.Core.Models.PublishedContent;

#else
using Umbraco.Core.Models.PublishedContent;
#endif


namespace YuzuDelivery.Umbraco.BlockList
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DefaultGridItem<M, V> : IGridItemInternal
        where M : PublishedElementModel
    {
        private string docTypeAlias;
        private readonly IMapper mapper;
        private readonly IYuzuTypeFactoryRunner typeFactoryRunner;
#if NETCOREAPP
        private readonly IPublishedValueFallback publishedValueFallback;
#endif

        public DefaultGridItem(string docTypeAlias, IMapper mapper, IYuzuTypeFactoryRunner typeFactoryRunner
#if NETCOREAPP
            , IPublishedValueFallback publishedValueFallback
#endif
            )
        {
            this.docTypeAlias = docTypeAlias;
            this.mapper = mapper;
            this.typeFactoryRunner = typeFactoryRunner;
#if NETCOREAPP
            this.publishedValueFallback = publishedValueFallback;
#endif
        }

        public Type ElementType => typeof(M);

        public virtual bool IsValid(IPublishedElement content)
        {
            if (content != null)
            {
                return content.ContentType.Alias == docTypeAlias;
            }
            return false;
        }

        public virtual object Apply(IPublishedElement model, IDictionary<string, object> contextItems)
        {
            var output = typeFactoryRunner.Run<V>(contextItems);
            if (output == null)
                output = mapper.Map<V>(model, contextItems);

            return output;
        }
    }
}