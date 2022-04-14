using System.Net;
using ArticleManagementApi.Exceptions;
using ArticleManagementApi.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ArticleManagementApi.Database;

public class ArticleContext : DbContext
{
	private readonly IConfiguration _configuration;
	public DbSet<Article> Articles => Set<Article>();
	public DbSet<ArticleAttribute> ArticlesAttributes => Set<ArticleAttribute>();

	public ArticleContext(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionString = _configuration.GetConnectionString("ArticleDb");

		if (connectionString == null)
		{
			throw new HttpResponseException(HttpStatusCode.InternalServerError,
				"Can't get connection string to database from environment variable");
		}

		optionsBuilder
			.UseLazyLoadingProxies()
			.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
	}
}
