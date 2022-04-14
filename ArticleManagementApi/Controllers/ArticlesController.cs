using ArticleManagementApi.Managers;
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
	public async Task<IActionResult> AddArticleAsync([FromBody] ArticleRequestDto articleToRequest)
	{
		var addedArticle = await _articleManager.Add(articleToRequest);
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
	[HttpPost("{articleNumber}/attributes")]
	public async Task<IActionResult> AddArticleAttributeAsync([FromRoute] int articleNumber,
		[FromBody] AttributeRequestDto attributeToRequest)
	{
		var addedAttribute = await _articleManager.AddAttribute(articleNumber, attributeToRequest);
		return Created(nameof(addedAttribute), addedAttribute);
	}
}
