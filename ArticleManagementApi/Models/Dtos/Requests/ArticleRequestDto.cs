using System.ComponentModel.DataAnnotations;

namespace ArticleManagementApi.Models.Dtos.Requests;

public record ArticleRequestDto
{
	[Required]
	public int ArticleNumber { get; set; }

	[Required]
	public string Brand { get; set; } = string.Empty;

	[Required]
	public bool IsBulky { get; set; }
}
