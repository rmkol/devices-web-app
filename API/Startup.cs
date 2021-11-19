using API.Data;
using API.MVC.Filters;
using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(options => options.Filters.Add(new CommonExceptionFilter()))
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			RegisterDbContexts(services);
			RegisterServiceClasses(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}

		private static void RegisterDbContexts(IServiceCollection services)
		{
			services.AddDbContext<DevicesDbContext>();
		}

		private static void RegisterServiceClasses(IServiceCollection services)
		{
			services.AddScoped<IDeviceService, DeviceService>();
		}
	}
}