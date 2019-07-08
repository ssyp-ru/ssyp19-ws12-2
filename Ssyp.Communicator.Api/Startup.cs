using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Ssyp.Communicator.Api
{
    internal sealed class Startup
    {
        public void ConfigureServices([NotNull] IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddMvc(options => options.EnableEndpointRouting = false).AddNewtonsoftJson();
        }

        public void Configure([NotNull] IApplicationBuilder app, [NotNull] IHostingEnvironment env)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection().UseMvc().UseAuthorization();
        }
    }
}