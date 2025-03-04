﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.WebApi;
using YuzuDelivery.Core.ViewModelBuilder;
using System.IO;
using YuzuDelivery.Core;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using System.Reflection;
using YuzuDelivery.Umbraco.Core;
using YuzuDelivery.Umbraco.Import;
using Umb = Umbraco.Core.Services;
using Umbraco.Core.Models;
using Umbraco.ModelsBuilder.Embedded.BackOffice;

namespace YuzuDelivery.Umbraco.TestProject
{
    /* /umbraco/backoffice/YuzuDeliveryExamples/ExampleBuild/GenerateViewModels */
    /* /umbraco/backoffice/YuzuDeliveryExamples/ExampleBuild/GenerateDocumentTypes */
    /* /umbraco/backoffice/YuzuDeliveryExamples/ExampleBuild/AddTemplates */
    /* /umbraco/backoffice/YuzuDeliveryExamples/ExampleBuild/CreateContent */

    [PluginController("YuzuDeliveryExamples")]
    public class ExampleBuildController : UmbracoAuthorizedApiController
    {

        private readonly GenerateController generate;
        private readonly SchemaChangeController schemaChange;
        private readonly ModelsBuilderDashboardController modelsBuilder;
        private readonly YuzuContentImportController contentImport;

        private readonly Umb.IFileService fileService;
        private readonly Umb.IContentTypeService umbContentTypeService;
        private readonly Umb.IContentService contentService;
        private readonly IContentTypeService contentTypeService;

        public ExampleBuildController(GenerateController generate, SchemaChangeController schemaChange, Umb.IFileService fileService, IContentTypeService contentTypeService, Umb.IContentTypeService umbContentTypeService, Umb.IContentService contentService, YuzuContentImportController contentImport, ModelsBuilderDashboardController modelsBuilder)
        {
            this.generate = generate;
            this.schemaChange = schemaChange;
            this.modelsBuilder = modelsBuilder;
            this.contentImport = contentImport;
            this.fileService = fileService;
            this.contentTypeService = contentTypeService;
            this.umbContentTypeService = umbContentTypeService;
            this.contentService = contentService;
        }


        [System.Web.Http.HttpGet]
        public string GenerateViewModels()
        {

            return generate.Build();
        
        }

        [System.Web.Http.HttpGet]
        public bool GenerateDocumentTypes()
        {
            schemaChange.ChangeDocumentTypes();

            try
            {
                modelsBuilder.BuildModels();
            }
            catch(Exception ex)
            {
                if(!(ex is NullReferenceException))
                    throw new Exception("Error creating models", ex);
            }
            return true;
        }

        [System.Web.Http.HttpGet]
        public bool AddTemplates()
        {
            var home = contentTypeService.Create("Home", "home", false);
            home.AllowedAsRoot = true;
            AddTemplateToContentType(home);

            var rowPage = GetContentAddTemplate("rowPage");
            var gridPage = GetContentAddTemplate("gridPage");
            var basicPage = GetContentAddTemplate("basicPage");

            contentTypeService.AddPermissions(home.Id, new List<IContentType>() { rowPage, gridPage, basicPage });

            return true;
        }

        [System.Web.Http.HttpGet]
        public bool CreateContent()
        {

            var homeContent = contentService.Create("Home", -1, "home");
            contentService.SaveAndPublish(homeContent);

            CreateContent("gridPage", homeContent);
            CreateContent("rowPage", homeContent);
            CreateContent("basicPage", homeContent);

            return true;

        }

        public void CreateContent(string name, IContent parent)
        {
            var type = contentTypeService.GetByAlias(name);
            var content = new Content(name.FirstCharacterToUpper(), parent, type);
            content.TemplateId = GetTemplateId(name);
            contentService.SaveAndPublish(content);

            ImportContent(name, content);
        }

        public void ImportContent(string name, IContent content)
        {
            contentImport.Import(new ImportContentFromFileVm()
            {
                Content = content.Id,
                DocumentTypeName = name,
                File = new DataFileLocationsVm()
                {
                    Location = "Main",
                    Filename = $"{name}.json"
                },
                Viewmodel = $"vmPage_{name.FirstCharacterToUpper()}"
            });
        }

        public IContentType GetContentAddTemplate(string alias)
        {
            var contentType = umbContentTypeService.Get(alias);
            AddTemplateToContentType(contentType);
            return contentType;
        }

        public void AddTemplateToContentType(IContentType contentType)
        {
            var result = fileService.CreateTemplateForContentType(contentType.Alias, contentType.Name);
            contentType.AllowedTemplates = new List<ITemplate>() { result.Result.Entity };
            contentTypeService.Save(contentType);
            contentType.SetDefaultTemplate(result.Result.Entity);
        }

        public int GetTemplateId(string alias)
        {
            var template = fileService.GetTemplate(alias);
            return template != null ? template.Id : -1;
        }
    }
}
