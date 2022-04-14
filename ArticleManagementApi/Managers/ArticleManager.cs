using System.Net;
using ArticleManagementApi.Database.Repositories;
using ArticleManagementApi.Exceptions;
using ArticleManagementApi.Extensions;
using ArticleManagementApi.Models;
using ArticleManagementApi.Models.Database;
using ArticleManagementApi.Models.Dtos.Requests;
using ArticleManagementApi.Models.Dtos.Response;

namespace ArticleManagementApi.Managers;

public class ArticleManager : IArticleManager
{
	private readonly IArticleRepository _articleRepository;
	private readonly ILogger<ArticleManager> _logger;

	public ArticleManager(IArticleRepository articleRepository, ILogger<ArticleManager> logger)
	{
		_articleRepository = articleRepository;
		_logger = logger;
	}
	public async Task<ArticleResponseDto> GetAsync(int articleNumber)
	{
		var article = await _articleRepository.GetAsync(articleNumber);
		return article.ToArticleDto();
	}

	public async Task<IReadOnlyCollection<ArticleResponseDto>> GetAllAsync(DateRequestFilter dateRequestFilter, string? title)
	{
		using var cancelTokenSource = new CancellationTokenSource();
		cancelTokenSource.CancelAfter(60000);
		var cancelToken = cancelTokenSource.Token;

		try
		{
			var articles = await _articleRepository.GetAllAsync
			(article =>
					(dateRequestFilter.From == null || article.LastChanged >= dateRequestFilter.From) &&
					(dateRequestFilter.To == null || article.LastChanged <= dateRequestFilter.To) &&
					(title == null || article.Attributes.Any(attribute => attribute.Title.Contains(title))),
				cancelToken
			);

			return articles.Select(article => article.ToArticleDto()).ToList().AsReadOnly();
		}
		catch (OperationCanceledException ex)
		{
			_logger.LogError("Timeout occured while querying database for all articles: ", ex);
			throw new HttpResponseException(HttpStatusCode.InternalServerError, "Timeout while querying database");
		}
	}

	public async Task<ArticleResponseDto> AddArticleAsync(ArticleRequestDto articleRequestDto)
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

	public async Task<ArticleResponseDto?> PutArticleAsync(int articleNumber, ArticlePutRequestDto articleDto)
	{
		ArticleResponseDto? createdArticle = null;
		var isExisting = await _articleRepository.IsExistingAsync(articleNumber);

		if (isExisting)
		{
			await UpdateExistingArticle(articleNumber, articleDto);
		}
		else
		{
			createdArticle = AddNewArticle(articleNumber, articleDto);
		}

		await _articleRepository.SaveChangesAsync();
		return createdArticle;
	}

	private async Task UpdateExistingArticle(int articleNumber, ArticlePutRequestDto articleDto)
	{
		var existingArticle = await _articleRepository.GetAsync(articleNumber);
		existingArticle.Brand = articleDto.Brand;
		existingArticle.IsBulky = articleDto.IsBulky;
	}

	private ArticleResponseDto AddNewArticle(int articleNumber, ArticlePutRequestDto articleDto)
	{
		var newArticle = new Article()
		{
			ArticleNumber = articleNumber,
			Brand = articleDto.Brand,
			IsBulky = articleDto.IsBulky
		};

		_articleRepository.Add(newArticle);
		return newArticle.ToArticleDto();
	}

	public async Task DeleteArticleAsync(int articleNumber)
	{
		var article = await _articleRepository.GetAsync(articleNumber);
		_articleRepository.Delete(article);
		await _articleRepository.SaveChangesAsync();
	}

	public async Task<AttributeResponseDto?> AddAttributeAsync(int articleNumber, AttributeRequestDto attributeRequestDto)
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
		article.LastChanged = DateTime.Now;

		await _articleRepository.SaveChangesAsync();

		return attribute.ToDto();
	}

	public async Task<AttributeResponseDto?> PutAttributeAsync(int articleNumber, Country country, AttributePutRequestDto attributeDto)
	{
		AttributeResponseDto? createdArticleDto = null;

		var article = await _articleRepository.GetAsync(articleNumber);

		try
		{
			var attribute = article.Attributes.SingleOrDefault(attribute => attribute.Country == country);

			if (attribute == null)
			{
				createdArticleDto = AddNewAttribute(country, attributeDto, article);
			}
			else
			{
				UpdateAttribute(attributeDto, attribute);
			}

			article.LastChanged = DateTime.Now;
			await _articleRepository.SaveChangesAsync();
			return createdArticleDto;
		}
		catch (InvalidOperationException ex)
		{
			_logger.LogError($"Detected multiple attributes with country {country} for article {articleNumber}: ", ex);
			throw new HttpResponseException(HttpStatusCode.InternalServerError,
				"There are multiple attributes with the same county. Please report this as a bug");
		}
	}

	private static AttributeResponseDto AddNewAttribute(Country country, AttributePutRequestDto attributeDto, Article article)
	{
		var newAttribute = new ArticleAttribute()
		{
			Color = attributeDto.Color,
			Country = country,
			Description = attributeDto.Description,
			Title = attributeDto.Title
		};

		article.Attributes.Add(newAttribute);

		return newAttribute.ToDto();
	}

	private static void UpdateAttribute(AttributePutRequestDto attributeDto, ArticleAttribute attribute)
	{
		attribute.Color = attributeDto.Color;
		attribute.Description = attributeDto.Description;
		attribute.Title = attributeDto.Title;
		attribute.LastChange = DateTime.Now;
	}

	public async Task<AttributeResponseDto> GetAttributeAsync(int articleNumber, Country country)
	{
		var article = await _articleRepository.GetAsync(articleNumber);
		var attribute = GetSingleAttribute(article, country);

		return attribute.ToDto();
	}

	public async Task DeleteAttributeAsync(int articleNumber, Country country)
	{
		var article = await _articleRepository.GetAsync(articleNumber);
		var attribute = GetSingleAttribute(article, country);

		article.Attributes.Remove(attribute);
		await _articleRepository.SaveChangesAsync();
	}

	private ArticleAttribute GetSingleAttribute(Article article, Country country)
	{
		var attributes = article.Attributes.Where(attribute => attribute.Country == country).ToList();

		return attributes.Count switch
		{
			0 => throw new HttpResponseException(HttpStatusCode.NotFound, $"Attribute not found"),
			> 1 => throw new HttpResponseException(HttpStatusCode.InternalServerError,
				"There are multiple attributes with the same county. Please report this as a bug"),
			_ => attributes.First()
		};
	}

	public async Task<IReadOnlyCollection<AttributeResponseDto>> GetAttributesAsync(int articleNumber)
	{
		var article = await _articleRepository.GetAsync(articleNumber);

		var result = article.Attributes.Select(attribute => attribute.ToDto()).ToList();

		return result.AsReadOnly();
	}
}
