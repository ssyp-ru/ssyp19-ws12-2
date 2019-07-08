using CuttingEdge.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Ssyp.Communicator.Api
{
    internal sealed class Startup
    {
        internal void ConfigureServices([NotNull] IServiceCollection services)
        {
            Condition.Requires(services, "services").IsNotNull();
            services.AddMvc().AddNewtonsoftJson();
        }

        internal void Configure([NotNull] IApplicationBuilder app, [NotNull] IHostingEnvironment env)
        {
            Condition.Requires(app, "app").IsNotNull();
            Condition.Requires(env, "env").IsNotNull();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseRouting(routes => { routes.MapApplication(); });
            app.UseAuthorization();
        }
    }
}