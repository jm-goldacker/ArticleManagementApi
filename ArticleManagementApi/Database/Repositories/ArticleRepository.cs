using System.Linq.Expressions;
using System.Net;
using ArticleManagementApi.Exceptions;
using ArticleManagementApi.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ArticleManagementApi.Database.Repositories;

public class ArticleRepository : IArticleRepository
{
	private readonly ArticleContext _articleContext;

	public ArticleRepository(ArticleContext articleContext)
	{
		_articleContext = articleContext;
	}

	public async Task<Article> GetAsync(int articleNumber)
	{
		try
		{
			var article = await _articleContext.Articles.FirstAsync(a => a.ArticleNumber == articleNumber);
			return article;
		}
		catch (InvalidOperationException)
		{
			throw new HttpResponseException(HttpStatusCode.NotFound, $"article number {articleNumber} not found");
		}

	}

	public async Task<List<Article>> GetAllAsync(Expression<Func<Article, bool>> filter, CancellationToken cancelToken)
	{
		var articles = await _articleContext.Articles.Where(filter).ToListAsync(cancelToken);
		return articles;
	}

	public async Task<bool> IsExistingAsync(int articleNumber)
	{
		var isExisting = await _articleContext.Articles.AnyAsync(a => a.ArticleNumber == articleNumber);
		return isExisting;
	}

	public void Add(Article articleToAddInDatabase)
	{
		_articleContext.Articles.Add(articleToAddInDatabase);
	}

	public void Delete(Article article)
	{
		_articleContext.Articles.Remove(article);
	}

	public async Task<int> SaveChangesAsync()
	{
		try
		{
			var writtenChanges = await _articleContext.SaveChangesAsync();
			return writtenChanges;
		}
		catch (DbUpdateConcurrencyException)
		{
			throw new HttpResponseException(HttpStatusCode.InternalServerError,
				"Cannot saves changes due to concurrent access to database. Please try again.");
		}
		catch (DbUpdateException)
		{
			throw new HttpResponseException(HttpStatusCode.InternalServerError,
				"An error occured while saving to the database");
		}
	}
}
