using ArticleManagementApi.Models;
using ArticleManagementApi.Models.Dtos.Requests;
using ArticleManagementApi.Models.Dtos.Response;

namespace ArticleManagementApi.Managers;

/// <summary>
/// Contains the logic to manipulate articles and their attributes.
/// </summary>
public interface IArticleManager
{
	/// <summary>
	/// Returns a single article.
	/// </summary>
	/// <param name="articleNumber">article number of the article</param>
	/// <returns>article</returns>
	Task<ArticleResponseDto> GetArticleAsync(int articleNumber);

	/// <summary>
	/// Returns all articles that match the given filters.
	/// </summary>
	/// <param name="dateRequestFilter">filter for the date when the article was last changed</param>
	/// <param name="title">filter for the title of the article</param>
	/// <returns>list of articles</returns>
	Task<IReadOnlyCollection<ArticleResponseDto>> GetAllArticlesAsync(DateRequestFilter dateRequestFilter, string? title);

	/// <summary>
	/// Creates an article.
	/// </summary>
	/// <param name="articleDto">dto of the article</param>
	/// <returns>added article</returns>
	Task<ArticleResponseDto> CreateArticleAsync(ArticlePostRequestDto articleDto);

	/// <summary>
	/// Creates a new article or updates an existing article.
	/// </summary>
	/// <param name="articleNumber">article number of the article</param>
	/// <param name="articleDto">dto of the article</param>
	/// <returns>created article</returns>
	Task<ArticleResponseDto?> CreateOrUpdateArticleAsync(int articleNumber, ArticlePutRequestDto articleDto);

	/// <summary>
	/// Deletes an article.
	/// </summary>
	/// <param name="articleNumber">article number of the article</param>
	/// <returns></returns>
	Task DeleteArticleAsync(int articleNumber);

	/// <summary>
	/// Returns all attributes of an article.
	/// </summary>
	/// <param name="articleNumber">article number of the article</param>
	/// <returns>all attributes for the given article</returns>
	Task<IReadOnlyCollection<AttributeResponseDto>> GetAttributesAsync(int articleNumber);

	/// <summary>
	/// Adds an attribute to an article.
	/// </summary>
	/// <param name="articleNumber">number of the article</param>
	/// <param name="attributeDto">attribute</param>
	/// <returns></returns>
	Task<AttributeResponseDto?> AddAttributeAsync(int articleNumber, AttributePostRequestDto attributeDto);

	/// <summary>
	/// Creates or updates an attribute of an article.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <param name="country">country of attribute</param>
	/// <param name="attributeDto">dto of attribute</param>
	/// <returns>created attribute</returns>
	Task<AttributeResponseDto?> CreateOrUpdateAttributeAsync(int articleNumber, Country country, AttributePutRequestDto attributeDto);

	/// <summary>
	/// Deletes an attribute.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <param name="country">country of attribute</param>
	/// <returns></returns>
	Task DeleteAttributeAsync(int articleNumber, Country country);

	/// <summary>
	/// Returns a specific attribute of an article.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <param name="country">country of attribute</param>
	/// <returns>attribute</returns>
	Task<AttributeResponseDto> GetAttributeAsync(int articleNumber, Country country);
}
