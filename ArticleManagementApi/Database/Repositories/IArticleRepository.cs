using System.Linq.Expressions;
using ArticleManagementApi.Models.Database;

namespace ArticleManagementApi.Database.Repositories;

public interface IArticleRepository
{
	/// <summary>
	/// Retrieves an article from the database.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <returns>article</returns>
	Task<Article> GetAsync(int articleNumber);

	/// <summary>
	/// Retrieves all articles from the database that fulfill the filter.
	/// </summary>
	/// <param name="filter">filter</param>
	/// <param name="cancelToken">cancellation token</param>
	/// <returns>list of all articles</returns>
	Task<List<Article>> GetAllAsync(Expression<Func<Article, bool>> filter, CancellationToken cancelToken);

	/// <summary>
	/// Adds an article to the database.
	/// </summary>
	/// <param name="articleToAddInDatabase">article</param>
	void Add(Article articleToAddInDatabase);

	/// <summary>
	/// Deletes an article from the database.
	/// </summary>
	/// <param name="article">article</param>
	void Delete(Article article);

	/// <summary>
	/// Saves changes to database.
	/// </summary>
	/// <returns>written changes</returns>
	Task<int>  SaveChangesAsync();

	/// <summary>
	/// Checks if article exists in database.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <returns>true if article exists, otherwise false</returns>
	Task<bool> IsExistingAsync(int articleNumber);

}
