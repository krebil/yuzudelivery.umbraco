﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skybrud.Umbraco.GridData;
using Newtonsoft.Json;
using YuzuDelivery.Umbraco.Core;

namespace YuzuDelivery.Umbraco.Grid
{
    public class GridConfigConverter<T> : IYuzuTypeConvertor<Dictionary<string, object>, T>
    {
        public T Convert(Dictionary<string, object> source, UmbracoMappingContext context)
        {
            if (source == null)
                return default(T);

            var config = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(config);
        }
    }

    public class GridRowConvertor<TSource, TDest> : IYuzuFullPropertyResolver<TSource, TDest, GridDataModel, vmBlock_DataRows>
    {
        private readonly IGridService gridService;

        public GridRowConvertor(IGridService gridService)
        {
            this.gridService = gridService;
        }

        public vmBlock_DataRows Resolve(TSource source, TDest destination, GridDataModel sourceMember, string memberName, UmbracoMappingContext context)
        {
            return gridService.CreateRows(sourceMember, context);
        }
    }

    public class GridRowConvertor<TSource, TDest, TConfig> : IYuzuFullPropertyResolver<TSource, TDest, GridDataModel, vmBlock_DataRows>
    {
        private readonly IGridService gridService;

        public GridRowConvertor(IGridService gridService)
        {
            this.gridService = gridService;
        }

        public vmBlock_DataRows Resolve(TSource source, TDest destination, GridDataModel sourceMember, string memberName, UmbracoMappingContext context)
        {
            return gridService.CreateRows<TConfig>(sourceMember, context);
        }
    }

    public class GridRowColumnConvertor<TSource, TDest> : IYuzuFullPropertyResolver<TSource, TDest, GridDataModel, vmBlock_DataGrid>
    {
        private readonly IGridService gridService;

        public GridRowColumnConvertor(IGridService gridService)
        {
            this.gridService = gridService;
        }

        public vmBlock_DataGrid Resolve(TSource source, TDest destination, GridDataModel sourceMember, string memberName, UmbracoMappingContext context)
        {
            return gridService.CreateRowsColumns(sourceMember, context);
        }
    }

    public class GridRowColumnConvertor<TSource, TDest, TConfig> : IYuzuFullPropertyResolver<TSource, TDest, GridDataModel, vmBlock_DataGrid>
    {
        private readonly IGridService gridService;

        public GridRowColumnConvertor(IGridService gridService)
        {
            this.gridService = gridService;
        }

        public vmBlock_DataGrid Resolve(TSource source, TDest destination, GridDataModel sourceMember, string memberName, UmbracoMappingContext context)
        {
            return gridService.CreateRowsColumns<TConfig>(sourceMember, context);
        }
    }

    public class GridRowColumnConvertor<TSource, TDest, TConfigRow, TConfigCol> : IYuzuFullPropertyResolver<TSource, TDest, GridDataModel, vmBlock_DataGrid>
    {
        private readonly IGridService gridService;

        public GridRowColumnConvertor(IGridService gridService)
        {
            this.gridService = gridService;
        }

        public vmBlock_DataGrid Resolve(TSource source, TDest destination, GridDataModel sourceMember, string memberName, UmbracoMappingContext context)
        {
            return gridService.CreateRowsColumns<TConfigRow, TConfigCol>(sourceMember, context);
        }
    }

}
