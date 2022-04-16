using ArticleManagementApi.Models.Database;
using ArticleManagementApi.Models.Dtos.Response;

namespace ArticleManagementApi.Extensions;

public static class ArticleAttributeExtensions
{
	public static AttributeResponseDto ToDto(this ArticleAttribute articleAttribute)
	{
		return new AttributeResponseDto(articleAttribute.Title,
			articleAttribute.Description,
			articleAttribute.Color,
			articleAttribute.Country);
	}
}
