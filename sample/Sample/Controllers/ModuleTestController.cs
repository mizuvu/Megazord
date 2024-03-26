﻿using Microsoft.AspNetCore.Mvc;
using Sample.Modules;

namespace Sample.Controllers;

[Route("[controller]")]
[ApiController]
public class ModuleTestController(
    OrderModuleService orderModuleService,
    ProductModuleService productModuleService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var obj = new
        {
            ProductId = productModuleService.GetProductId,
            OrderId = orderModuleService.GetOrderId
        };

        return Ok(obj);
    }
}
