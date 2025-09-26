using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Kms.Security.Api
{
   
    public class LabXandApiControllers
    {
        public static string GetApiControllers()
        {

            string Items = "";
            Assembly FoundAssembly = Assembly.GetExecutingAssembly();

            string AssemblyName = FoundAssembly.FullName;
            IEnumerable<TypeInfo> Types = FoundAssembly.DefinedTypes.Where(type => type != null && type.IsPublic && type.IsClass && !type.IsAbstract && typeof(ApiController).IsAssignableFrom(type));
            foreach (TypeInfo ControllerType in Types)
            {
                System.Web.Http.Controllers.ApiControllerActionSelector ApiControllerSelection = new System.Web.Http.Controllers.ApiControllerActionSelector();
                System.Web.Http.Controllers.HttpControllerDescriptor ApiDescriptor = new System.Web.Http.Controllers.HttpControllerDescriptor(new System.Web.Http.HttpConfiguration(), ControllerType.Name, ControllerType);
                ILookup<string, System.Web.Http.Controllers.HttpActionDescriptor> ApiMappings = ApiControllerSelection.GetActionMapping(ApiDescriptor);

                foreach (var Maps in ApiMappings)
                {
                    foreach (System.Web.Http.Controllers.HttpActionDescriptor Actions in Maps)
                    {
                        var temp = ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)(Actions)).GetCustomAttributes<DisplayNameAttribute>();
                        if (temp.Count != 0)
                            Items += "[ controller=" + ControllerType.Name + " action=" + ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)(Actions)).ActionName + temp.FirstOrDefault().DisplayName + "]" + Environment.NewLine;
                    }
                }
            }

            return Items;
        }

        public static List<DynamicPermission> GetLsitOfApiControllers()
        {
            List<DynamicPermission> result = new List<DynamicPermission>();
            // string Items = "";
            Assembly FoundAssembly = Assembly.GetExecutingAssembly();

            string AssemblyName = FoundAssembly.FullName;
            IEnumerable<TypeInfo> Types = FoundAssembly.DefinedTypes.Where(type => type != null && type.IsPublic && type.IsClass && !type.IsAbstract && typeof(ApiController).IsAssignableFrom(type));
            foreach (TypeInfo ControllerType in Types)
            {
                System.Web.Http.Controllers.ApiControllerActionSelector ApiControllerSelection = new System.Web.Http.Controllers.ApiControllerActionSelector();
                System.Web.Http.Controllers.HttpControllerDescriptor ApiDescriptor = new System.Web.Http.Controllers.HttpControllerDescriptor(new System.Web.Http.HttpConfiguration(), ControllerType.Name, ControllerType);
                ILookup<string, System.Web.Http.Controllers.HttpActionDescriptor> ApiMappings = ApiControllerSelection.GetActionMapping(ApiDescriptor);
                var test = ControllerType.CustomAttributes.Where(p => p.AttributeType == typeof(DisplayNameAttribute)).FirstOrDefault();//.ConstructorArguments.FirstOrDefault().Value;
                var r = new DynamicPermission { ControllerName = ControllerType.Name, Title = test != null ? test.ConstructorArguments.FirstOrDefault().Value.ToString() : ControllerType.Name, Name = ControllerType.Name };
                int item = 0;
                foreach (var Maps in ApiMappings)
                {
                    foreach (System.Web.Http.Controllers.HttpActionDescriptor Actions in Maps)
                    {
                        var temp = ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)(Actions)).GetCustomAttributes<DynamicClaimsAuthorizationAttribute>();
                        if (temp.Count != 0)
                        {
                            var displayNameAtr = ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)(Actions)).GetCustomAttributes<DisplayNameAttribute>();
                            var actionName = ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)(Actions)).ActionName;
                            var returnType = ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)(Actions)).ReturnType != null ? ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)(Actions)).ReturnType.ToString():"void";
                            var dispalayName = displayNameAtr.Count != 0 ? displayNameAtr.FirstOrDefault().DisplayName : actionName;
                            result.Add(new DynamicPermission { ControllerName = r.ControllerName, Name = actionName, ParentId = r.Id, Title = dispalayName, ReturnType = returnType });
                            // Items += "[ controller=" + ControllerType.Name + " action=" + ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)(Actions)).ActionName + temp.FirstOrDefault().DisplayName + "]" + Environment.NewLine;
                            item++;
                        }
                    }
                }
                if (item > 0)
                    result.Add(r);
            }

            return result;
        }
    }
}