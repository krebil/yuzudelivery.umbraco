﻿using System;
using YuzuDelivery.Core;

namespace YuzuDelivery.Umbraco.Core
{
    public class YuzuGlobalMapperSettings : YuzuMapperSettings
    {
        public Type Source { get; set; }
        public Type Dest { get; set; }
        public string GroupName { get; set; }
    }
}
