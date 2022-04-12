using ArticleManagementApi.Database;
using ArticleManagementApi.Database.Repositories;
using ArticleManagementApi.Managers;

namespace ArticleManagementApi.Extensions;

public static class ServiceExtensions
{
	public static void AddApiServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IArticleManager, ArticleManager>();
		serviceCollection.AddScoped<IArticleRepository, ArticleRepository>();
		serviceCollection.AddDbContext<ArticleContext>();
	}
}
