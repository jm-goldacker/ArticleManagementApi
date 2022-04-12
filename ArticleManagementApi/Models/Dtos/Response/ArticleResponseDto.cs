namespace ArticleManagementApi.Models.Dtos.Response;

public record ArticleResponseDto
{
	public int ArticleNumber { get; init; }
	public string Brand { get; init; } = string.Empty;
	public bool IsBulky { get; init; }
	public bool IsApproved { get; init; }
}
