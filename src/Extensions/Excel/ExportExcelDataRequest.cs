using System.Collections.Generic;

namespace Zord.Extensions.Excel
{
    public class ExportExcelDataRequest
    {
        public IList<object> Data { get; set; } = null!;

        public string? SheetName { get; set; }
    }
}
