using ArticleManagementApi.Models.Database;
using ArticleManagementApi.Models.Dtos.Response;

namespace ArticleManagementApi.Extensions;

public static class ArticleAttributeExtensions
{
	public static AttributeResponseDto ToDto(this ArticleAttribute articleAttribute)
	{
		return new AttributeResponseDto()
		{
			Color = articleAttribute.Color,
			Country = articleAttribute.Country,
			Description = articleAttribute.Description,
			Title = articleAttribute.Title
		};
	}
}
