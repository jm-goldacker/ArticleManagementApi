using System.ComponentModel.DataAnnotations;

namespace ArticleManagementApi.Models.Dtos.Requests;

public record ArticlePutRequestDto([Required] string Brand, [Required] bool IsBulky);
