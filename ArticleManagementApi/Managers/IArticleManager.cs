using ArticleManagementApi.Models;
using ArticleManagementApi.Models.Database;
using ArticleManagementApi.Models.Dtos.Requests;
using ArticleManagementApi.Models.Dtos.Response;

namespace ArticleManagementApi.Managers;

public interface IArticleManager
{
	Task<ArticleResponseDto> GetAsync(int articleNumber);
	Task<IReadOnlyCollection<ArticleResponseDto>> GetAllAsync(DateRequestFilter dateRequestFilter, string? title);
	Task<ArticleResponseDto> AddArticleAsync(ArticlePostRequestDto articlePostRequestDto);
	Task<ArticleResponseDto?> PutArticleAsync(int articleNumber, ArticlePutRequestDto articleDto);
	Task DeleteArticleAsync(int articleNumber);
	Task<IReadOnlyCollection<AttributeResponseDto>> GetAttributesAsync(int articleNumber);
	Task<AttributeResponseDto?> AddAttributeAsync(int articleNumber, AttributeRequestDto attributeRequestDto);
	Task<AttributeResponseDto?> PutAttributeAsync(int articleNumber, Country country, AttributePutRequestDto attributeDto);
	Task DeleteAttributeAsync(int articleNumber, Country country);
	Task<AttributeResponseDto> GetAttributeAsync(int articleNumber, Country country);
}
