using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using static works.ei8.Identity.Constants;

namespace works.ei8.Identity
{
    public class HostNameMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public HostNameMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if (Environment.GetEnvironmentVariable(EnvironmentVariableKeys.HostNameExpected) == context.Request.Host.ToString())
                context.Request.Host = new HostString(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.HostNameReplacement));
            await requestDelegate.Invoke(context);
        }
    }
}
