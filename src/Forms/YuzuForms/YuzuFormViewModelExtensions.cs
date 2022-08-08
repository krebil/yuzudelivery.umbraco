using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using YuzuDelivery.Umbraco.Core;

#if NETCOREAPP
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

#else
using System.Web.Mvc;
#endif

namespace YuzuDelivery.Umbraco.Forms
{
    public static class YuzuFormViewModelExtensions
    {
        public static void AddDataAppSettings(this YuzuFormViewModel model, string key, object value)
        {
            var keyName = $"data-{key}";
            var strValue = JsonConvert.SerializeObject(value);

            if (model.HtmlFormAttributes.ContainsKey(keyName))
            {
                model.HtmlFormAttributes[keyName] = strValue;
            }
            else
            {
                model.HtmlFormAttributes.Add(keyName, strValue);
            }
        }

        public static void AddFormField(this vmBlock_DataFormBuilder formBuilder, object field, int fieldsetIndex = 0)
        {
            //if (formBuilder.Fieldsets.ElementAt(fieldsetIndex) != null)
                //((List<Object>)formBuilder.Fieldsets[fieldsetIndex].Fields).Add(field);
        }

#if NETCOREAPP
        public static void AddHandler<C>(this YuzuFormViewModel formBuilder, Expression<Func<C, Task<IActionResult>>> actionLambda)
#else
        public static void AddHandler<C>(this YuzuFormViewModel formBuilder, Expression<Func<C, ActionResult>> actionLambda)
#endif
        {
            Type type = typeof(C);

            var methodName = actionLambda.GetMemberName();

            formBuilder.Controller = type;
            formBuilder.Action = methodName;
        }

        public static bool HasTempData(this Controller controller, string key)
        {
            return controller.TempData.ContainsKey(key) && controller.TempData[key].ToString() == "True";
        }

        public static void SetTempData(this Controller controller, string key)
        {
            controller.TempData[key] = "True";
        }

        public static (string Controller, string Action) ToControllerAndAction(this string endpoint)
        {
            var arrEndpoint = endpoint.Split(',');

            if (arrEndpoint.Length != 2)
                throw new Exception($"{endpoint} endpoint not valid");
            else
                return (arrEndpoint[0], arrEndpoint[1]);
        }
    }
}
