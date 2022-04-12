using System.Net;
using ArticleManagementApi.Exceptions;
using ArticleManagementApi.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ArticleManagementApi.Database;

public class ArticleContext : DbContext
{
	public DbSet<Article> Articles => Set<Article>();
	public DbSet<ArticleAttribute> ArticlesAttributes => Set<ArticleAttribute>();

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

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
