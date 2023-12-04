using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Zord.Host.Extensions
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
