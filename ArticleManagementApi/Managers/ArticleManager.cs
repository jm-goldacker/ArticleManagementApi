using System.Net;
using ArticleManagementApi.Database.Repositories;
using ArticleManagementApi.Exceptions;
using ArticleManagementApi.Extensions;
using ArticleManagementApi.Models.Database;
using ArticleManagementApi.Models.Dtos.Requests;
using ArticleManagementApi.Models.Dtos.Response;

namespace ArticleManagementApi.Managers;

public class ArticleManager : IArticleManager
{
	private readonly IArticleRepository _articleRepository;

	public ArticleManager(IArticleRepository articleRepository)
	{
		_articleRepository = articleRepository;
	}
	public async Task<ArticleResponseDto> GetAsync(int articleNumber)
	{
		var article = await _articleRepository.GetAsync(articleNumber);
		return article.ToArticleDto();
	}

	public async Task<IReadOnlyCollection<ArticleResponseDto>> GetAllAsync(DateRequestFilter dateRequestFilter, string? title)
	{
		var articles = _articleRepository.GetAllAsync
		(article =>
			(dateRequestFilter.From == null || article.LastChanged >= dateRequestFilter.From) &&
			(dateRequestFilter.To == null || article.LastChanged <= dateRequestFilter.To) &&
			(title == null || article.Attributes.Any(attribute => attribute.Title.Contains(title)))
		);

		return await articles.Select(article => article.ToArticleDto()).ToListAsync();
	}

	public async Task<ArticleResponseDto> Add(ArticleRequestDto articleRequestDto)
	{
		var isAlreadyExisting = await _articleRepository.IsExistingAsync(articleRequestDto.ArticleNumber);

		if (isAlreadyExisting)
		{
			throw new HttpResponseException(HttpStatusCode.Conflict, "Article exists already");
		}

		var article = new Article()
		{
			ArticleNumber = articleRequestDto.ArticleNumber,
			Brand = articleRequestDto.Brand,
			IsBulky = articleRequestDto.IsBulky
		};

		_articleRepository.Add(article);
		await _articleRepository.SaveChangesAsync();

		return article.ToArticleDto();
	}

	public async Task<AttributeResponseDto?> AddAttribute(int articleNumber, AttributeRequestDto attributeRequestDto)
	{
		var article = await _articleRepository.GetAsync(articleNumber);

		if (article.Attributes.Any(attribute => attribute.Country == attributeRequestDto.Country))
		{
			throw new HttpResponseException(HttpStatusCode.Conflict,
				"Attribute for country and article exists already");
		}

		var attribute = new ArticleAttribute()
		{
			Color = attributeRequestDto.Color,
			Description = attributeRequestDto.Description,
			Title = attributeRequestDto.Title,
			Country = attributeRequestDto.Country,
		};

		article.AddAttribute(attribute);

		await _articleRepository.SaveChangesAsync();

		return attribute.ToArticleAttributeDto();
	}

	public async Task<IReadOnlyCollection<AttributeResponseDto>> GetAttributesAsync(int articleNumber)
	{
		var article = await _articleRepository.GetAsync(articleNumber);

		var result = article.Attributes.Select(attribute => attribute.ToArticleAttributeDto()).ToList();

		return result.AsReadOnly();
	}
}
