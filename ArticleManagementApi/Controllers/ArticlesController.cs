using ArticleManagementApi.Managers;
using ArticleManagementApi.Models;
using ArticleManagementApi.Models.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagementApi.Controllers;

/// <summary>
/// Endpoints for articles.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ArticlesController : ControllerBase
{
	private readonly IArticleManager _articleManager;

	public ArticlesController(IArticleManager articleManager)
	{
		_articleManager = articleManager;
	}

	/// <summary>
	/// Returns a single article
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <returns>article</returns>
	[MapToApiVersion("1.0")]
	[HttpGet("{articleNumber}")]
	public async Task<IActionResult> GetAsync(int articleNumber)
	{
		var article = await _articleManager.GetArticleAsync(articleNumber);
		return Ok(article);
	}

	/// <summary>
	/// Returns multiple articles that fulfill filters.
	/// </summary>
	/// <param name="dateRequestFilter">filter for date when article was last changed</param>
	/// <param name="title">title of article</param>
	/// <returns>list of articles</returns>
	[MapToApiVersion("1.0")]
	[HttpGet]
	public async Task<IActionResult> GetAllAsync([FromQuery] DateRequestFilter dateRequestFilter, [FromQuery] string? title)
	{
		var articles = await _articleManager.GetAllArticlesAsync(dateRequestFilter, title);
		return Ok(articles);
	}

	/// <summary>
	/// Creates an article.
	/// </summary>
	/// <param name="articlePostToRequest">article to create</param>
	/// <returns>created article</returns>
	[MapToApiVersion("1.0")]
	[HttpPost]
	public async Task<IActionResult> AddArticleAsync([FromBody] ArticlePostRequestDto articlePostToRequest)
	{
		var addedArticle = await _articleManager.CreateArticleAsync(articlePostToRequest);
		return Created(nameof(addedArticle), addedArticle);
	}

	/// <summary>
	/// Returns all attributes of article.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <returns>list of attributes</returns>
	[MapToApiVersion("1.0")]
	[HttpGet("{articleNumber}/attributes")]
	public async Task<IActionResult> GetArticleAttributesAsync([FromRoute] int articleNumber)
	{
		var attributes = await _articleManager.GetAttributesAsync(articleNumber);
		return Ok(attributes);
	}

	/// <summary>
	/// Creates or Updates an article.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <param name="article">article to create or update</param>
	/// <returns>no content or article that is created</returns>
	[MapToApiVersion("1.0")]
	[HttpPut("{articleNumber}")]
	public async Task<IActionResult> CreateOrUpdateArticleAsync([FromRoute] int articleNumber, ArticlePutRequestDto article)
	{
		var createdArticle = await _articleManager.CreateOrUpdateArticleAsync(articleNumber, article);
		return createdArticle == null ? NoContent() : Created(nameof(createdArticle), createdArticle);
	}

	/// <summary>
	/// Deletes an article.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <returns>no content</returns>
	[MapToApiVersion("1.0")]
	[HttpDelete("{articleNumber}")]
	public async Task<IActionResult> DeleteArticleAsync([FromRoute] int articleNumber)
	{
		await _articleManager.DeleteArticleAsync(articleNumber);
		return NoContent();
	}

	/// <summary>
	/// Returns single attribute of article.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <param name="country">country of attribute</param>
	/// <returns>attribute</returns>
	[MapToApiVersion("1.0")]
	[HttpGet("{articleNumber}/attributes/{country}")]
	public async Task<IActionResult> GetArticleAttributeAsync([FromRoute] int articleNumber, [FromRoute] Country country)
	{
		var attribute = await _articleManager.GetAttributeAsync(articleNumber, country);
		return Ok(attribute);
	}

	/// <summary>
	/// Creates an attribute for an article.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <param name="attributePostToRequest">attribute to create</param>
	/// <returns>created attribute</returns>
	[MapToApiVersion("1.0")]
	[HttpPost("{articleNumber}/attributes")]
	public async Task<IActionResult> AddArticleAttributeAsync([FromRoute] int articleNumber,
		[FromBody] AttributePostRequestDto attributePostToRequest)
	{
		var addedAttribute = await _articleManager.AddAttributeAsync(articleNumber, attributePostToRequest);
		return Created(nameof(addedAttribute), addedAttribute);
	}

	/// <summary>
	/// Creates or updates an article attribute.
	/// </summary>
	/// <param name="articleNumber">article number of attribute</param>
	/// <param name="country">country of attribute</param>
	/// <param name="attribute">attribute to create or update</param>
	/// <returns>created article or no content</returns>
	[MapToApiVersion("1.0")]
	[HttpPut("{articleNumber}/attributes/{country}")]
	public async Task<IActionResult> CreateOrUpdateAttributeAsync([FromRoute] int articleNumber, [FromRoute] Country country,
		[FromBody] AttributePutRequestDto attribute)
	{
		var createdAttribute = await _articleManager.CreateOrUpdateAttributeAsync(articleNumber, country, attribute);

		return createdAttribute == null ? NoContent() : Created(nameof(createdAttribute), createdAttribute);
	}

	/// <summary>
	/// Deletes an attribute of an article.
	/// </summary>
	/// <param name="articleNumber">article number of article</param>
	/// <param name="country">country of attribute</param>
	/// <returns>no content</returns>
	[MapToApiVersion("1.0")]
	[HttpDelete("{articleNumber}/attributes/{country}")]
	public async Task<IActionResult> DeleteAttributeAsync([FromRoute] int articleNumber, [FromRoute] Country country)
	{
		await _articleManager.DeleteAttributeAsync(articleNumber, country);
		return NoContent();
	}
}
