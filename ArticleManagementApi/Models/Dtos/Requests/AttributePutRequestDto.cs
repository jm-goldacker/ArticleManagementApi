using System.ComponentModel.DataAnnotations;

namespace ArticleManagementApi.Models.Dtos.Requests;

public record AttributePutRequestDto
{
	[Required] public string Title { get; init; } = string.Empty;

	[Required]
	public string Description { get; init; } = string.Empty;

	[Required]
	public string Color { get; init; } = string.Empty;
}
