using Microsoft.AspNetCore.Mvc;
using Zord.Files;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
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

        [HttpGet("import")]
        public IActionResult Import()
        {
            var filePath = Path.Combine(@"D:\\", "test.xlsx");

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);

            Stream stream = new MemoryStream(bytes);

            var list = _excelService.Read<ImportTemp>(stream);

            return Ok(list);
        }
    }

    public enum ImportType
    {
        None,
        New,
        Update,
        Delete
    }

    public class ImportTemp
    {
        public int Id { get; set; }
        public ImportType Type { get; set; }
    }
}