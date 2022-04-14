using System.ComponentModel.DataAnnotations;

namespace ArticleManagementApi.Models.Dtos.Requests;

public record AttributeRequestDto([Required] string Title, [Required] string Description, [Required] string Color, [Required] Country Country);
