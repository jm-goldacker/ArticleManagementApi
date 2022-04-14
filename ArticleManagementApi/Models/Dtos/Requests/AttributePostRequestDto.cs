using System.ComponentModel.DataAnnotations;

namespace ArticleManagementApi.Models.Dtos.Requests;

public record AttributePostRequestDto([Required] string Title, [Required] string Description, [Required] string Color, [Required] Country Country);
