using CuttingEdge.Conditions;
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
            Condition.Requires(services, nameof(services)).IsNotNull();
            services.AddMvc().AddNewtonsoftJson();
        }

        public void Configure([NotNull] IApplicationBuilder app, [NotNull] IHostingEnvironment env)
        {
            Condition.Requires(app, nameof(app)).IsNotNull();
            Condition.Requires(env, nameof(env)).IsNotNull();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection().UseMvc().UseRouting(routes => { routes.MapApplication(); }).UseAuthorization();
        }
    }
}