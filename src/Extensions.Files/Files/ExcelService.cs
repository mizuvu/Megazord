using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Zord.Extensions.Files.Excel;

namespace Zord.Extensions.Files
{
    public class ExcelService : IExcelService
    {
        public Stream Export<T>(DataTable dataTable, string? sheetName = null)
        {
            using var wb = new XLWorkbook();
            wb.Worksheets.Add(dataTable, sheetName ?? "data");

            foreach (var ws in wb.Worksheets)
            {
                ws.ColumnsUsed().AdjustToContents(); // fit columns width
            }

            return wb.AsStream();
        }

        public Stream Export<T>(IList<T> data, string? sheetName = null)
        {
            using var wb = new XLWorkbook();
            wb.Worksheets.Add(sheetName ?? "data").FirstCell().InsertTable(data, true);

            foreach (var ws in wb.Worksheets)
            {
                ws.ColumnsUsed().AdjustToContents(); // fit columns width
            }

            return wb.AsStream();
        }

        public Stream Export(ExportExcelDataRequest[] request)
        {
            using var wb = new XLWorkbook();

            foreach (var obj in request)
            {
                var typeOfObj = obj.Data.GetType().Name;

                wb.Worksheets.Add(obj.SheetName ?? typeOfObj).FirstCell().InsertTable(obj.Data, true);
            }

            foreach (var ws in wb.Worksheets)
            {
                ws.ColumnsUsed().AdjustToContents(); // fit columns width
            }

            return wb.AsStream();
        }

        public DataTable Read(Stream streamData, string? sheetName = null)
        {
            using IXLWorkbook workbook = new XLWorkbook(streamData);

            // set worksheet
            //var worksheet = workbook.Worksheets.Where(w => w.Name == sheetName).First();
            var worksheet = !string.IsNullOrEmpty(sheetName)
                ? workbook.Worksheet(sheetName)
                : workbook.Worksheet(1);

            // Create a new DataTable.
            var dt = new DataTable();

            // Loop through the Worksheet rows.
            bool firstRow = true;

            foreach (IXLRow row in worksheet.Rows()) // Skip first row which is used for column header texts
            {
                // Use the first row to add columns to DataTable.
                if (firstRow)
                {
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString());
                    }
                    firstRow = false;
                }
                else
                {
                    // Add rows to DataTable.
                    dt.Rows.Add();
                    int i = 0;
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Rows[^1][i] = cell.Value.ToString();
                        i++;
                    }
                }
            }

            return dt;
        }

        public IEnumerable<T> Read<T>(Stream streamData, string? sheetName = null, ColumnOptions<T>? options = null)
        {
            var list = new List<T>();

            Type typeOfObject = typeof(T);

            using (IXLWorkbook workbook = new XLWorkbook(streamData))
            {
                var worksheet = !string.IsNullOrEmpty(sheetName)
                    ? workbook.Worksheet(sheetName)
                    : workbook.Worksheet(1);

                var properties = typeOfObject.GetProperties();

                // header column texts
                var columns = worksheet.FirstRow().Cells().Select((v, i) =>
                    new { v.Value, Index = i + 1 }); // indexing of ClosedXml is 1 not 0

                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))// Skip first row which is used for column header texts
                {
                    T obj = (T)Activator.CreateInstance(typeOfObject)!;

                    foreach (var prop in properties)
                    {
                        // find column has same prop name of class
                        var propName = prop.Name.ToString();
                        if (options != null && options.ColumnNames.ContainsKey(propName))
                        {
                            propName = options.ColumnNames[propName];
                        }

                        var columnHasName = columns.SingleOrDefault(c => c.Value.ToString() == propName);
                        if (columnHasName != null)
                        {
                            int colIndex = columnHasName.Index;
                            var type = prop.PropertyType;
                            var val = row.Cell(colIndex).GetString(); // must .ToString() to fix error "object must implement Iconvertible"
                            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                if (string.IsNullOrEmpty(val))
                                    prop.SetValue(obj, null);
                                else
                                    prop.SetValue(obj, Convert.ChangeType(val, type.GetGenericArguments()[0]));
                            }
                            else if (prop.PropertyType.IsEnum)
                            {
                                var enumValue = Enum.Parse(prop.PropertyType, val);

                                prop.SetValue(obj, enumValue);
                            }
                            else
                            {
                                prop.SetValue(obj, Convert.ChangeType(val, type));
                            }
                        }
                    }

                    list.Add(obj);
                }
            }

            return list;
        }
    }
}