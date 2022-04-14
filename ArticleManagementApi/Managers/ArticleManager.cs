using System.Net;
using ArticleManagementApi.Database.Repositories;
using ArticleManagementApi.Exceptions;
using ArticleManagementApi.Extensions;
using ArticleManagementApi.Models;
using ArticleManagementApi.Models.Database;
using ArticleManagementApi.Models.Dtos.Requests;
using ArticleManagementApi.Models.Dtos.Response;

namespace ArticleManagementApi.Managers;

/// <inheritdoc/>
public class ArticleManager : IArticleManager
{
	private readonly IArticleRepository _articleRepository;
	private readonly ILogger<ArticleManager> _logger;

	public ArticleManager(IArticleRepository articleRepository, ILogger<ArticleManager> logger)
	{
		_articleRepository = articleRepository;
		_logger = logger;
	}

	/// <inheritdoc/>
	public async Task<ArticleResponseDto> GetArticleAsync(int articleNumber)
	{
		var article = await _articleRepository.GetAsync(articleNumber);
		return article.ToArticleDto();
	}

	/// <inheritdoc/>
	/// <exception cref="HttpResponseException">thrown if timeout occurs while querying database</exception>
	public async Task<IReadOnlyCollection<ArticleResponseDto>> GetAllArticlesAsync(DateRequestFilter dateRequestFilter, string? title)
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

	/// <inheritdoc/>
	public async Task<ArticleResponseDto> CreateArticleAsync(ArticlePostRequestDto articlePostRequestDto)
	{
		var isAlreadyExisting = await _articleRepository.IsExistingAsync(articlePostRequestDto.ArticleNumber);

		if (isAlreadyExisting)
		{
			throw new HttpResponseException(HttpStatusCode.Conflict, "Article exists already");
		}

		var article = new Article()
		{
			ArticleNumber = articlePostRequestDto.ArticleNumber,
			Brand = articlePostRequestDto.Brand,
			IsBulky = articlePostRequestDto.IsBulky
		};

		_articleRepository.Add(article);
		await _articleRepository.SaveChangesAsync();

		return article.ToArticleDto();
	}

	/// <inheritdoc/>
	public async Task<ArticleResponseDto?> CreateOrUpdateArticleAsync(int articleNumber, ArticlePutRequestDto articleDto)
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

	/// <inheritdoc/>
	public async Task DeleteArticleAsync(int articleNumber)
	{
		var article = await _articleRepository.GetAsync(articleNumber);
		_articleRepository.Delete(article);
		await _articleRepository.SaveChangesAsync();
	}

	/// <inheritdoc/>
	/// <exception cref="HttpResponseException">thrown if attribute exists already for country and article</exception>
	public async Task<AttributeResponseDto?> AddAttributeAsync(int articleNumber, AttributePostRequestDto attributePostRequestDto)
	{
		var article = await _articleRepository.GetAsync(articleNumber);

		if (article.Attributes.Any(attribute => attribute.Country == attributePostRequestDto.Country))
		{
			throw new HttpResponseException(HttpStatusCode.Conflict,
				"Attribute for country and article exists already");
		}

		var attribute = new ArticleAttribute()
		{
			Color = attributePostRequestDto.Color,
			Description = attributePostRequestDto.Description,
			Title = attributePostRequestDto.Title,
			Country = attributePostRequestDto.Country,
		};

		article.AddAttribute(attribute);
		article.LastChanged = DateTime.Now;

		await _articleRepository.SaveChangesAsync();

		return attribute.ToDto();
	}

	/// <inheritdoc/>
	/// <exception cref="HttpResponseException">thrown if article has multiple attributes</exception>
	public async Task<AttributeResponseDto?> CreateOrUpdateAttributeAsync(int articleNumber, Country country, AttributePutRequestDto attributeDto)
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

	/// <inheritdoc/>
	public async Task<AttributeResponseDto> GetAttributeAsync(int articleNumber, Country country)
	{
		var article = await _articleRepository.GetAsync(articleNumber);
		var attribute = GetSingleAttribute(article, country);

		return attribute.ToDto();
	}

	/// <inheritdoc/>
	/// <exception cref="HttpResponseException">thrown if attribute is not found or article has multiple attributes for same country</exception>
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

	/// <inheritdoc/>
	public async Task<IReadOnlyCollection<AttributeResponseDto>> GetAttributesAsync(int articleNumber)
	{
		var article = await _articleRepository.GetAsync(articleNumber);

		var result = article.Attributes.Select(attribute => attribute.ToDto()).ToList();

		return result.AsReadOnly();
	}
}
