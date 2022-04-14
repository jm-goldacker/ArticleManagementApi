using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ArticleManagementApi.Configurations;

public class SwaggerConfigurations : IConfigureOptions<SwaggerGenOptions>
{
	private readonly IApiVersionDescriptionProvider _provider;

	public SwaggerConfigurations(IApiVersionDescriptionProvider provider) => _provider = provider;

	public void Configure(SwaggerGenOptions options)
	{
		string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
		options.IncludeXmlComments(xmlPath);

		foreach (var desc in _provider.ApiVersionDescriptions)
		{
			options.SwaggerDoc(desc.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
			{
				Title = "Article Management API",
				Version = desc.ApiVersion.ToString(),
			});
		}
	}
}
