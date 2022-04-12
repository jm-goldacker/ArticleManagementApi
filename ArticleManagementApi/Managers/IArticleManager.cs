using ArticleManagementApi.Models.Dtos.Requests;
using ArticleManagementApi.Models.Dtos.Response;

namespace ArticleManagementApi.Managers;

public interface IArticleManager
{
	Task<ArticleResponseDto> GetAsync(int articleNumber);
	Task<IReadOnlyCollection<ArticleResponseDto>> GetAllAsync(DateRequestFilter dateRequestFilter, string? title);
	Task<ArticleResponseDto> Add(ArticleRequestDto articleRequestDto);
	Task<IReadOnlyCollection<AttributeResponseDto>> GetAttributesAsync(int articleNumber);
	Task<AttributeResponseDto?> AddAttribute(int articleNumber, AttributeRequestDto attributeRequestDto);
}
