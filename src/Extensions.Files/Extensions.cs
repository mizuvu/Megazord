﻿using ClosedXML.Excel;
using System.IO;

namespace Zord.Extensions.Files
{
    internal static class Extensions
    {
        internal static Stream AsStream(this XLWorkbook workbook)
        {
            Stream stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
