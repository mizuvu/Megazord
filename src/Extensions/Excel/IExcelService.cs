using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Zord.Extensions.Excel
{
    public interface IExcelService
    {
        /// <summary>
        /// Export a list data to file
        /// </summary>
        public Stream Export<T>(IList<T> data, string? sheetName = null);

        /// <summary>
        /// Load excel file to DataTable
        /// </summary>
        public DataTable Read(Stream streamData, string? sheetName = null);

        /// <summary>
        /// Load excel file to list object
        /// </summary>
        public IEnumerable<T> Read<T>(Stream streamData, string? sheetName = null, ColumnOptions<T>? options = null);
    }
}
