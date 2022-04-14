using System.Net;
using ArticleManagementApi.Managers;
using ArticleManagementApi.Models;
using ArticleManagementApi.Models.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagementApi.Controllers;

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

	[MapToApiVersion("1.0")]
	[HttpGet("{articleNumber}")]
	public async Task<IActionResult> GetAsync(int articleNumber)
	{
		var article = await _articleManager.GetAsync(articleNumber);
		return Ok(article);
	}

	[MapToApiVersion("1.0")]
	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] DateRequestFilter dateRequestFilter, [FromQuery] string? title)
	{
		var articles = await _articleManager.GetAllAsync(dateRequestFilter, title);
		return Ok(articles);
	}

	[MapToApiVersion("1.0")]
	[HttpPost]
	public async Task<IActionResult> AddArticleAsync([FromBody] ArticlePostRequestDto articlePostToRequest)
	{
		var addedArticle = await _articleManager.AddArticleAsync(articlePostToRequest);
		return Created(nameof(addedArticle), addedArticle);
	}

	[MapToApiVersion("1.0")]
	[HttpGet("{articleNumber}/attributes")]
	public async Task<IActionResult> GetArticleAttributesAsync([FromRoute] int articleNumber)
	{
		var attributes = await _articleManager.GetAttributesAsync(articleNumber);
		return Ok(attributes);
	}

	[MapToApiVersion("1.0")]
	[HttpPut("{articleNumber}")]
	public async Task<IActionResult> UpdateArticle([FromRoute] int articleNumber, ArticlePutRequestDto article)
	{
		var createdArticle = await _articleManager.PutArticleAsync(articleNumber, article);
		return createdArticle == null ? NoContent() : Created(nameof(createdArticle), createdArticle);
	}

	[MapToApiVersion("1.0")]
	[HttpDelete("{articleNumber}")]
	public async Task<IActionResult> DeleteArticle([FromRoute] int articleNumber)
	{
		await _articleManager.DeleteArticleAsync(articleNumber);
		return NoContent();
	}

	[MapToApiVersion("1.0")]
	[HttpGet("{articleNumber}/attributes/{country}")]
	public async Task<IActionResult> GetArticleAttributeAsync([FromRoute] int articleNumber, [FromRoute] Country country)
	{
		var attribute = await _articleManager.GetAttributeAsync(articleNumber, country);
		return Ok(attribute);
	}

	[MapToApiVersion("1.0")]
	[HttpPost("{articleNumber}/attributes")]
	public async Task<IActionResult> AddArticleAttributeAsync([FromRoute] int articleNumber,
		[FromBody] AttributeRequestDto attributeToRequest)
	{
		var addedAttribute = await _articleManager.AddAttributeAsync(articleNumber, attributeToRequest);
		return Created(nameof(addedAttribute), addedAttribute);
	}

	[MapToApiVersion("1.0")]
	[HttpPut("{articleNumber}/attributes/{country}")]
	public async Task<IActionResult> UpdateAttribute([FromRoute] int articleNumber, [FromRoute] Country country,
		[FromBody] AttributePutRequestDto attribute)
	{
		var createdAttribute = await _articleManager.PutAttributeAsync(articleNumber, country, attribute);

		return createdAttribute == null ? NoContent() : Created(nameof(createdAttribute), createdAttribute);
	}

	[MapToApiVersion("1.0")]
	[HttpDelete("{articleNumber}/attributes/{country}")]
	public async Task<IActionResult> DeleteAttribute([FromRoute] int articleNumber, [FromRoute] Country country)
	{
		await _articleManager.DeleteAttributeAsync(articleNumber, country);
		return NoContent();
	}
}
