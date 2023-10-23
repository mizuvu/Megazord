using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Zord.Api.Extensions
{
    public class LowercaseControllerNameConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerName = controller.ControllerName;

            if (controllerName != null)
            {
                controller.ControllerName = controllerName.ConvertName();
            }
        }
    }
}
