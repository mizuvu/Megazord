using Microsoft.AspNetCore.Mvc;
using Zord.Extensions.Excel;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExportController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpGet]
        public IActionResult Export()
        {
            var list = new List<object>
            {
                new
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Description for Product 1"
                },
                new
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "Description for Product 2"
                },
                new
                {
                    Id = 3,
                    Name = "Product 3",
                    Description = "Description for Product 3"
                }
            };

            Stream stream = _excelService.Export(list);
            return File(stream, "application/octet-stream", "DataExport.xlsx"); // returns a FileStreamResult
        }
    }
}