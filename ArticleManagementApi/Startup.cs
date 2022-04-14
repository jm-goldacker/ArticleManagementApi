using System.Text.Json.Serialization;
using ArticleManagementApi.Extensions;
using ArticleManagementApi.Filters;
using ArticleManagementApi.Middleware;

namespace ArticleManagementApi;

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
		services.AddVersioning();
		services.AddSwaggerVersioning();

		services.AddApiServices();

		services.AddControllers(options =>
			{
				options.Filters.Add<ValidationFilter>();
			})
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseHttpsRedirection();

		app.UseRouting();

		app.AddSwaggerVersioning();

		app.UseAuthorization();

		app.UseMiddleware<ExceptionMiddleware>();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}
