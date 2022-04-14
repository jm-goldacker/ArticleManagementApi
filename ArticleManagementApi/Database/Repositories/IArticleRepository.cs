using System.Linq.Expressions;
using ArticleManagementApi.Models.Database;

namespace ArticleManagementApi.Database.Repositories;

public interface IArticleRepository
{
	Task<Article> GetAsync(int articleNumber);
	IAsyncEnumerable<Article> GetAllAsync(Expression<Func<Article, bool>> filter);
	void Add(Article articleToAddInDatabase);
	void Delete(Article article);
	Task<int>  SaveChangesAsync();
	Task<bool> IsExistingAsync(int articleNumber);

}
