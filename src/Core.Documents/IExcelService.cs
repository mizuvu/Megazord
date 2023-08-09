using System.Collections.Generic;
using System.Data;
using System.IO;
using Zord.Core.Excel;

namespace Zord.Core
{
    public interface IExcelService
    {
        /// <summary>
        /// Export a DataTable to file
        /// </summary>
        public Stream Export<T>(DataTable dataTable, string? sheetName = null);

        /// <summary>
        /// Export a list data to file
        /// </summary>
        public Stream Export<T>(IList<T> data, string? sheetName = null);

        /// <summary>
        /// Export multi list data to file
        /// </summary>
        public Stream Export(ExportExcelDataRequest[] request);

        /// <summary>
        /// Load excel file to DataTable
        /// </summary>
        public DataTable Read(Stream streamData, string? sheetName = null);

        /// <summary>
        /// Load excel file to list object with manual set column names
        /// </summary>
        public IEnumerable<T> Read<T>(Stream streamData, string? sheetName = null, ColumnOptions<T>? options = null);
    }
}
