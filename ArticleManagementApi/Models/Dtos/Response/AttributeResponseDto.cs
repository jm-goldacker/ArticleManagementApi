namespace ArticleManagementApi.Models.Dtos.Response;

public class AttributeResponseDto
{
	public string Title { get; init; } = string.Empty;

	public string Description { get; init; } = string.Empty;

	public string Color { get; init; } = string.Empty;

	public Country Country { get; set; }
}
