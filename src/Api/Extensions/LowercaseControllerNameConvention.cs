using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Zord.Api.Extensions
{
    public class LowercaseControllerNameConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var oldControllerName = controller.ControllerName;

            if (oldControllerName != null)
            {
                string newControllerName = "";

                for (int i = 0; i < oldControllerName.Length; i++)
                    if (char.IsUpper(oldControllerName[i]))
                    {
                        if (i == 0) // first char
                        {
                            newControllerName += char.ToLower(oldControllerName[i]);
                        }
                        else
                        {
                            newControllerName += "_" + char.ToLower(oldControllerName[i]); // add prefix to upper chars
                        }
                    }
                    else
                        newControllerName += oldControllerName[i];

                controller.ControllerName = newControllerName;
            }
        }
    }
}
