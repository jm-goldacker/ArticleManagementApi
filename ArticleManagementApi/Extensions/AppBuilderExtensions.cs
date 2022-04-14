using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ArticleManagementApi.Extensions;

public static class AppBuilderExtensions
{
	public static void AddSwaggerVersioning(this IApplicationBuilder app)
	{
		IServiceProvider services = app.ApplicationServices;
		var provider = services.GetRequiredService<IApiVersionDescriptionProvider>();

		app.UseSwagger();

		app.UseSwaggerUI(options =>
		{
			foreach (var description in provider.ApiVersionDescriptions)
			{
				options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
			}
		});
	}
}
