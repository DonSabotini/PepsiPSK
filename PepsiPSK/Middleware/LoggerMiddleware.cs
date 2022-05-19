using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Pepsi.Data;
using PepsiPSK.Entities;

namespace PepsiPSK.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dbcontext)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                if (controllerActionDescriptor != null)
                {
                    var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);     
                    if (userId != null)
                    {
                        var user = context.User.Identity.Name;
                        var userRole = context.User.FindFirstValue(ClaimTypes.Role);
                        var controllerName = controllerActionDescriptor.ControllerName;
                        var actionName = controllerActionDescriptor.ActionName;

                        dbcontext.ActionRecords.Add(new ActionRecord()
                        {
                            UserName = user,
                            UserId = userId,
                            Role = userRole,
                            Time = DateTime.UtcNow,
                            UsedMethod = controllerName + "-" + actionName
                        }); 
                        dbcontext.SaveChanges();
                    }

                }
            }
            await _next(context);
            

        }
    }
}

