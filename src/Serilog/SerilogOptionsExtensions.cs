﻿using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Zord.Serilog
{
    public static class SerilogOptionsExtensions
    {
        public static IEnumerable<WriteToOptions>? GetWriteToOptions(IConfiguration configuration)
        {
            return configuration.GetSection("Serilog:WriteTo").Get<IEnumerable<WriteToOptions>>();
        }

        public static WriteToOptions? GetWriteToOptions(IConfiguration configuration, string firstElementName)
        {
            var options = GetWriteToOptions(configuration);

            return options?.FirstOrDefault(x => x.Name == firstElementName);
        }
    }
}