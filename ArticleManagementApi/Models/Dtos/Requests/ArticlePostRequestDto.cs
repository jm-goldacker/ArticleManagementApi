using System.ComponentModel.DataAnnotations;

namespace ArticleManagementApi.Models.Dtos.Requests;

public record ArticlePostRequestDto([Required] int ArticleNumber, [Required] string Brand, [Required] bool IsBulky);
