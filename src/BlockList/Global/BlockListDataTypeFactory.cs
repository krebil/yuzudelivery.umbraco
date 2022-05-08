﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YuzuDelivery.Umbraco.Import;
using YuzuDelivery.Core;

#if NETCOREAPP
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umb = Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Logging;
#else
using Umbraco.Core.Models;
using Umbraco.Core;
using Umbraco.Web.PropertyEditors;
using Umb = Umbraco.Core.Services;
using Umbraco.Core.Logging;
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

        public IDataType CreateOrUpdate(string dataTypeName, string[] subBlocks, Options options = null)
        {
            var blocks = new List<BlockListConfiguration.BlockConfiguration>();

            var dataTypeDefinition = dataTypeService.GetByName(dataTypeName);
            if(dataTypeDefinition == null) 
                dataTypeDefinition = dataTypeService.CreateDataType(dataTypeName, DataEditorName);
            else
            {
                var config = dataTypeDefinition.Umb().Configuration as BlockListConfiguration;
                if(config != null)
                {
                    blocks = config.Blocks.ToList();
                }
            }

            var blockListConfig = CreateBlockListConfig(subBlocks, blocks, options);

            dataTypeDefinition.Umb().Name = dataTypeName;
            dataTypeDefinition.Umb().Configuration = blockListConfig;

            return dataTypeService.Save(dataTypeDefinition);
        }

        private BlockListConfiguration CreateBlockListConfig(string[] subBlocks, List<BlockListConfiguration.BlockConfiguration> blocks, Options options)
        {
            options = options == null ? options = new Options() : options; 
            var blockListConfig = options.Config == null ? new BlockListConfiguration() : options.Config;

            blockListConfig.ValidationLimit = new BlockListConfiguration.NumberRange();
            blockListConfig.ValidationLimit.Min = options.Min;
            blockListConfig.ValidationLimit.Max = options.Max;

            foreach (var subBlock in subBlocks)
            {
                var alreadyExists = contentTypeService.GetByAlias(subBlock.AsAlias()) != null;
                if(!alreadyExists) blocks.Add(CreateBlockConfig(subBlock, options));
            }
            blockListConfig.Blocks = blocks.ToArray();

            return blockListConfig;
        }

        private BlockListConfiguration.BlockConfiguration CreateBlockConfig(string blockName, Options options)
        {
            var contentType = CreateContentType(blockName, options);
            var settingsType = CreateSettingsType(options);

            return new BlockListConfiguration.BlockConfiguration()
            {
                Label = blockName.RemoveAllVmPrefixes().CamelToSentenceCase(),
                View = options.CustomView == null ? DefaultCustomView : options.CustomView,
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

            if (options.CreatePropertiesAction != null) options.CreatePropertiesAction(contentType, documentTypePropertyService);

            return contentType;
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

            public Func<string, IContentTypeService, IContentType> CreateContentTypeAction { get; set; }
            public Action<IContentType, IDocumentTypePropertyService> CreatePropertiesAction { get; set; }
        }

    }
}
