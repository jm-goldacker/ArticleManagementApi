namespace ArticleManagementApi.Models.Dtos.Response;

/// <summary>
/// Response model for attributes of articles.
/// </summary>
/// <param name="Title">title of article</param>
/// <param name="Description">description of article</param>
/// <param name="Color">color of article</param>
/// <param name="Country">country for which this attribute is</param>
public record AttributeResponseDto(string Title, string Description, string Color, Country Country);
