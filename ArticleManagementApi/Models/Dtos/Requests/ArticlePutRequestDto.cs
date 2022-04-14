using System.ComponentModel.DataAnnotations;

namespace ArticleManagementApi.Models.Dtos.Requests;

public record ArticlePutRequestDto
{
	[Required]
	public string Brand { get; set; } = string.Empty;

	[Required]
	public bool IsBulky { get; set; }
}
