using System.Collections.Generic;

namespace Zord.Core.Excel
{
    public class ExportExcelDataRequest
    {
        public IList<object> Data { get; set; } = null!;

        public string? SheetName { get; set; }
    }
}
