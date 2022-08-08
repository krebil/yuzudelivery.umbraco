﻿using YuzuDelivery.Core;

namespace YuzuDelivery.Umbraco.Core
{
    public interface IYuzuPropertyAfterResolver<M, Type> : IYuzuPropertyAfterResolver, IYuzuMappingResolver
    {
        Type Apply(Type value);
    }
}
