﻿using System;
using System.Collections.Generic;
using System.Linq;
using YuzuDelivery.Core;

#if NETCOREAPP
using Umbraco.Cms.Core.PropertyEditors;
using Umb = Umbraco.Cms.Core.Services;

#else
using Umbraco.Web.PropertyEditors;
using Umb = Umbraco.Core.Services;
#endif


namespace YuzuDelivery.Umbraco.Import
{
    public class BlockListDataTypeFactory
    {
        private readonly IDataTypeService dataTypeService;
        private readonly IContentTypeService contentTypeService;
        private readonly IContentTypeForVmService contentTypeForVmTypeService;
        private readonly IDocumentTypePropertyService documentTypePropertyService;
        public const string DataEditorName = "Umbraco.BlockList";

        public const string DefaultCustomView = "~/App_Plugins/YuzuBlockList/GridContentItem.html";

        public BlockListDataTypeFactory(IDataTypeService dataTypeService, IDocumentTypePropertyService documentTypePropertyService, IContentTypeService contentTypeService, IContentTypeForVmService contentTypeForVmTypeService)
        {
            this.dataTypeService = dataTypeService;
            this.documentTypePropertyService = documentTypePropertyService;
            this.contentTypeService = contentTypeService;
            this.contentTypeForVmTypeService = contentTypeForVmTypeService;
        }

        public IDataType CreateOrUpdate(string dataTypeName, IEnumerable<string> subBlocks, Options options = null)
        {
            var blocks = new List<BlockListConfiguration.BlockConfiguration>();

            var dataTypeDefinition = dataTypeService.GetByName(dataTypeName);
            if(dataTypeDefinition == null) 
                dataTypeDefinition = dataTypeService.CreateDataType(dataTypeName, DataEditorName);
            else
            {
                if(dataTypeDefinition.Umb().Configuration is BlockListConfiguration config)
                {
                    blocks = config.Blocks.ToList();
                }
            }

            var blockListConfig = CreateBlockListConfig(subBlocks, blocks, options);

            dataTypeDefinition.Umb().Name = dataTypeName;
            dataTypeDefinition.Umb().Configuration = blockListConfig;

            return dataTypeService.Save(dataTypeDefinition);
        }

        private BlockListConfiguration CreateBlockListConfig(IEnumerable<string> subBlocks, List<BlockListConfiguration.BlockConfiguration> blocks, Options options)
        {
            options = options ?? (new Options()); 
            var blockListConfig = options.Config ?? new BlockListConfiguration();

            blockListConfig.ValidationLimit = new BlockListConfiguration.NumberRange
            {
                Min = options.Min,
                Max = options.Max
            };

            foreach (var subBlock in subBlocks)
            {
                if(!DoesBlockAlreadyExist(subBlock, blocks, options)) 
                    blocks.Add(CreateBlockConfig(subBlock, options));
            }
            blockListConfig.Blocks = blocks.ToArray();


            return blockListConfig;
        }

        private bool DoesBlockAlreadyExist(string subBlock, IEnumerable<BlockListConfiguration.BlockConfiguration> blocks, Options options)
        {
            var contentType = GetContentType(subBlock, options);
            return contentType != null && blocks.Any(x => x.ContentElementTypeKey == contentType.Umb().Key);
        }

        private BlockListConfiguration.BlockConfiguration CreateBlockConfig(string blockName, Options options)
        {
            var contentType = CreateContentType(blockName, options);
            var settingsType = CreateSettingsType(options);

            return new BlockListConfiguration.BlockConfiguration()
            {
                Label = blockName.RemoveAllVmPrefixes().CamelToSentenceCase(),
                View = options.CustomView ?? DefaultCustomView,
                EditorSize = "medium",
                ContentElementTypeKey = contentType.Umb().Key,
                SettingsElementTypeKey = settingsType?.Umb().Key,
                ForceHideContentEditorInOverlay = options.ForceHideContentEditor
            };
        }

        private IContentType CreateContentType(string blockName, Options options)
        {
            var contentType = options.CreateContentTypeAction != null ?
                options.CreateContentTypeAction(blockName, contentTypeService)
                :
                contentTypeForVmTypeService.CreateOrUpdate(blockName, null, true);

            options.CreatePropertiesAction?.Invoke(contentType, documentTypePropertyService);

            return contentType;
        }

        private IContentType GetContentType(string blockName, Options options)
        {
            return options.GetContentTypeAction != null ?
                options.GetContentTypeAction(blockName, contentTypeService)
                :
                contentTypeForVmTypeService.Get(blockName);
        }

        private IContentType CreateSettingsType(Options options)
        {
            return options.SettingsSubBlock != null ? contentTypeForVmTypeService.CreateOrUpdate(options.SettingsSubBlock, null, true) : null;
        }

        public class Options
        {
            public BlockListConfiguration Config { get; set; }
            public int? Min { get; set; }
            public int? Max  { get; set; }
            public string CustomView { get; set; }
            public string SettingsSubBlock { get; set; }
            public bool ForceHideContentEditor { get; set; }

            public Func<string, IContentTypeService, IContentType> GetContentTypeAction { get; set; }
            public Func<string, IContentTypeService, IContentType> CreateContentTypeAction { get; set; }
            public Action<IContentType, IDocumentTypePropertyService> CreatePropertiesAction { get; set; }
        }

    }
}
