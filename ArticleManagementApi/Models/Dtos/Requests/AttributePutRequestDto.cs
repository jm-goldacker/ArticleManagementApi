using System.ComponentModel.DataAnnotations;

namespace ArticleManagementApi.Models.Dtos.Requests;

public record AttributePutRequestDto([Required] string Title, [Required] string Description, [Required] string Color);
