﻿using System;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using YuzuDelivery.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace YuzuDelivery.Umbraco.Core
{

    public class ImageMappings : YuzuMappingConfig
    {
        public ImageMappings()
        {
            ManualMaps.AddTypeReplace<ImageConvertor>(false);
            ManualMaps.AddTypeReplace<MediWithCropsConvertor>(false);
        }
    }

    public class MediWithCropsConvertor : IYuzuTypeConvertor<MediaWithCrops, vmBlock_DataImage>
    {
        private ImageFactory imageFactory;

        public MediWithCropsConvertor(ImageFactory imageFactory)
        {
            this.imageFactory = imageFactory;
        }

        public vmBlock_DataImage Convert(MediaWithCrops source, UmbracoMappingContext context)
        {
            if (source != null && source.ContentType != null)
            {
                if (source.ContentType.Alias == "Image")
                    return imageFactory.CreateImage(source.Content);
                if (source.ContentType.Alias == "File")
                    return imageFactory.CreateFile(source.Content);
            }
            return new vmBlock_DataImage();
        }
    }

    public class ImageConvertor : IYuzuTypeConvertor<IPublishedContent, vmBlock_DataImage>
    {
        private ImageFactory imageFactory;

        public ImageConvertor(ImageFactory imageFactory)
        {
            this.imageFactory = imageFactory;
        }

        public vmBlock_DataImage Convert(IPublishedContent source, UmbracoMappingContext context)
        {
            if(source != null && source.ContentType != null)
            {
                if (source.ContentType.Alias == "Image")
                    return imageFactory.CreateImage(source);
                if (source.ContentType.Alias == "File")
                    return imageFactory.CreateFile(source);
            }
            return new vmBlock_DataImage();
        }
    }

    public class ImageFactory
    {
        public vmBlock_DataImage CreateImage(IPublishedContent image)
        {
            if (image != null)
            {
                return new vmBlock_DataImage
                {
                    Src = image.Url(),
                    Alt = image.Value<string>("alt"),
                    Height = image.Value<int>("umbracoHeight"),
                    Width = image.Value<int>("umbracoWidth"),
                    FileSize = image.Value<int>("umbracoBytes")
                };
            }
            return new vmBlock_DataImage();
        }

        public vmBlock_DataImage CreateFile(IPublishedContent image)
        {
            if (image != null)
            {
                return new vmBlock_DataImage
                {
                    Src = image.Url(),
                    Alt = image.Value<string>("alt"),
                    Height = image.Value<int>("umbracoHeight"),
                    Width = image.Value<int>("umbracoWidth"),
                    FileSize = image.Value<int>("umbracoBytes")
                };
            }
            return new vmBlock_DataImage();
        }
    }

}
