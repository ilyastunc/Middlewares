using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Middlewares
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.Use ile middleware ekleyebiliyoruz. 
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/products")
                {
                    await context.Response.WriteAsync("Product List");
                } 
                else
                {
                    await next();
                }
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/details")
                {
                    await context.Response.WriteAsync("Product Details");
                } 
                else
                {
                    await next();
                }
            });

            //Map Use'dan daha kullanışlı. aynı işlevi görüyor
            app.Map("/profile", config =>
                config.Use(async (context, next) =>
                    await context.Response.WriteAsync("User Profile")));

            //Map'e koşul eklemek için
            app.MapWhen(
                context => context.Request.Method == "POST" && context.Request.Path == "/account",
                config => config.Use(async (context, next) => await context.Response.WriteAsync("Post-Account"))
            );

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
