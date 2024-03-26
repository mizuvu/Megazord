﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Zord.Modules;

public interface IModuleApplicationBuilder
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder) => builder;

    /// <summary>
    /// Configure Module Application Builder with IConfiguration
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    IApplicationBuilder ConfigurePipelines(IApplicationBuilder builder, IConfiguration configuration) => builder;
}