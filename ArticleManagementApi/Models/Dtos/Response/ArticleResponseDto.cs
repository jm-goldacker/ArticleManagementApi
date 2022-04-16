namespace ArticleManagementApi.Models.Dtos.Response;

/// <summary>
/// Response model for articles
/// </summary>
/// <param name="ArticleNumber">article number</param>
/// <param name="Brand">brand</param>
/// <param name="IsBulky">if article is bulky</param>
/// <param name="IsApproved">if article is approved</param>
public record ArticleResponseDto(int ArticleNumber, string Brand, bool IsBulky, bool IsApproved);
