using ArticleManagementApi.Managers;
using ArticleManagementApi.Models.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagementApi.Controllers;

[Controller]
[Route("api/[controller]")]
public class ArticlesController : Controller
{
	private readonly IArticleManager _articleManager;

	public ArticlesController(IArticleManager articleManager)
	{
		_articleManager = articleManager;
	}

	[HttpGet("{articleNumber}")]
	public async Task<IActionResult> GetAsync(int articleNumber)
	{
		var article = await _articleManager.GetAsync(articleNumber);
		return Ok(article);
	}

	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] DateRequestFilter dateRequestFilter, [FromQuery] string? title)
	{
		var articles = await _articleManager.GetAllAsync(dateRequestFilter, title);
		return Ok(articles);
	}

	[HttpPost]
	public async Task<IActionResult> AddArticleAsync([FromBody] ArticleRequestDto articleToRequest)
	{
		var addedArticle = await _articleManager.Add(articleToRequest);
		return Created(nameof(addedArticle), addedArticle);
	}

	[HttpGet("{articleNumber}/attributes")]
	public async Task<IActionResult> GetArticleAttributesAsync([FromRoute] int articleNumber)
	{
		var attributes = await _articleManager.GetAttributesAsync(articleNumber);
		return Ok(attributes);
	}

	[HttpPost("{articleNumber}/attributes")]
	public async Task<IActionResult> AddArticleAttributeAsync([FromRoute] int articleNumber,
		[FromBody] AttributeRequestDto attributeToRequest)
	{
		var addedAttribute = await _articleManager.AddAttribute(articleNumber, attributeToRequest);
		return Created(nameof(addedAttribute), addedAttribute);
	}
}
