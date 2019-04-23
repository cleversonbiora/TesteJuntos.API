using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteJuntos.Application.Secuity
{
    public static class TesteJuntosPolices
    {
        public static void UseAuthorizationOptions(this AuthorizationOptions options)
        {
            options.AddPolicy("SamplePolicy", policy => 
                policy.RequireAssertion(context => 
                    context.User.Identity.Name == "Amendoim"));
            options.AddPolicy("Action", policy =>
                policy.RequireAssertion(context => ControllerHandle(context)));
        }

        public static bool ControllerHandle(AuthorizationHandlerContext context)
        {
            var mvcContext = context.Resource as AuthorizationFilterContext;
            var descriptor = mvcContext?.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var actionName = descriptor.ActionName;
                var ctrlName = descriptor.ControllerName;
            }
            return true;
        }
    }
}
