using YuzuDelivery.Umbraco.BlockList;


namespace YuzuDelivery.Umbraco.Core
{
    public static class BlockListGridExtensions
    {

        public static V PluckFromGrid<V>(this vmBlock_DataGrid grid, int rowIndex = 0, int columnIndex = 0, int itemIndex = 0)
        {
            var rows = grid.Rows;

            if (rows[rowIndex] == null) 
                return default(V);
            
            var row = rows[rowIndex];
            if (row.Columns[columnIndex] == null) 
                return default(V);
            
            var column = row.Columns[columnIndex];
            if (column.Items[itemIndex] == null) 
                return default(V);
            
            var item = column.Items[itemIndex];
            if (item.Content is V content)
            {
                grid.Rows.RemoveAt(rowIndex);
                return content;
            }
            return default(V);
        }

        public static V PluckFromGrid<V>(this vmBlock_DataRows grid, int rowIndex = 0, int itemIndex = 0)
        {
            var rows = grid.Rows;

            if (rows[rowIndex] == null) 
                return default(V);
            
            var row = rows[rowIndex];
            if (row.Items[itemIndex] == null) 
                return default(V);
            
            var item = row.Items[itemIndex];
            if (item.Content is V content)
            {
                grid.Rows.RemoveAt(rowIndex);
                return content;
            }
            return default(V);
        }
    }
}
