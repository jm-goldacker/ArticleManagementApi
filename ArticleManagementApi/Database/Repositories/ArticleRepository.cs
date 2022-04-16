using System.Linq.Expressions;
using System.Net;
using ArticleManagementApi.Exceptions;
using ArticleManagementApi.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ArticleManagementApi.Database.Repositories;

/// <inheritdoc/>
public class ArticleRepository : IArticleRepository
{
	private readonly ArticleContext _articleContext;
	private readonly ILogger<ArticleRepository> _logger;

	public ArticleRepository(ArticleContext articleContext, ILogger<ArticleRepository> logger)
	{
		_articleContext = articleContext;
		_logger = logger;
	}

	/// <inheritdoc/>
	/// <exception cref="HttpResponseException">thrown if article is not found</exception>
	public async Task<Article> GetAsync(int articleNumber)
	{
		try
		{
			var article = await _articleContext.Articles.FirstAsync(a => a.ArticleNumber == articleNumber);
			return article;
		}
		catch (InvalidOperationException ex)
		{
			_logger.LogError("User requested article {articleNumber} that is not found: {ex}", articleNumber, ex);
			throw new HttpResponseException(HttpStatusCode.NotFound, $"article number {articleNumber} not found");
		}

	}

	/// <inheritdoc/>
	public async Task<List<Article>> GetAllAsync(Expression<Func<Article, bool>> filter, CancellationToken cancelToken)
	{
		var articles = await _articleContext.Articles.Where(filter).ToListAsync(cancelToken);
		return articles;
	}

	/// <inheritdoc/>
	public async Task<bool> IsExistingAsync(int articleNumber)
	{
		var isExisting = await _articleContext.Articles.AnyAsync(a => a.ArticleNumber == articleNumber);
		return isExisting;
	}

	/// <inheritdoc/>
	public void Add(Article articleToAddInDatabase)
	{
		_articleContext.Articles.Add(articleToAddInDatabase);
	}

	/// <inheritdoc/>
	public void Delete(Article article)
	{
		_articleContext.Articles.Remove(article);
	}

	/// <inheritdoc/>
	/// <exception cref="HttpResponseException">thrown if concurrency is detected or other error occurs while saving</exception>
	public async Task<int> SaveChangesAsync()
	{
		try
		{
			var writtenChanges = await _articleContext.SaveChangesAsync();
			return writtenChanges;
		}
		catch (DbUpdateConcurrencyException ex)
		{
			_logger.LogError("Concurrency occured while saving to database: {ex}", ex);
			throw new HttpResponseException(HttpStatusCode.InternalServerError,
				"Cannot saves changes due to concurrent access to database. Please try again.");
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError("Error occured while saving to database: {ex}", ex);
			throw new HttpResponseException(HttpStatusCode.InternalServerError,
				"An error occured while saving to the database");
		}
	}
}
