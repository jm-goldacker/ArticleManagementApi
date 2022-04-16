using ArticleManagementApi.Configurations;
using ArticleManagementApi.Database;
using ArticleManagementApi.Database.Repositories;
using ArticleManagementApi.Managers;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagementApi.Extensions;

public static class ServiceExtensions
{
	public static void AddApiServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IArticleManager, ArticleManager>();
		serviceCollection.AddScoped<IArticleRepository, ArticleRepository>();
		serviceCollection.AddDbContext<ArticleContext>();
	}

	public static void AddVersioning(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddApiVersioning(setup =>
		{
			setup.DefaultApiVersion = new ApiVersion(1, 0);
			setup.AssumeDefaultVersionWhenUnspecified = true;
			setup.ReportApiVersions = true;
		});

		serviceCollection.AddVersionedApiExplorer(setup =>
		{
			setup.GroupNameFormat = "'v'VVV";
			setup.SubstituteApiVersionInUrl = true;
		});
	}

	public static void AddSwaggerVersioning(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSwaggerGen();
		serviceCollection.ConfigureOptions<SwaggerConfigurations>();

	}
}
