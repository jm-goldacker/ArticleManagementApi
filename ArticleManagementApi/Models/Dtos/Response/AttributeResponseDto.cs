namespace ArticleManagementApi.Models.Dtos.Response;

public class AttributeResponseDto
{
	public string Title { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public string Color { get; set; } = string.Empty;

	public Country Country { get; set; }
}
