using ArticleManagementApi.Models;
using ArticleManagementApi.Models.Database;
using ArticleManagementApi.Models.Dtos.Response;

namespace ArticleManagementApi.Extensions;

public static class ArticleExtensions
{
	public static ArticleResponseDto ToArticleDto(this Article article)
	{
		return new ArticleResponseDto(article.ArticleNumber, article.Brand, article.IsBulky, article.IsApproved);
	}

	public static bool IsAttributeForEachCountrySet(this Article article)
	{
		var attributeCountries = article.Attributes.Select(attribute => attribute.Country);
		var allCountries = Enum.GetValues<Country>();
		var areAttributesForAllCountriesSet = allCountries.All(c => attributeCountries.Contains(c));
		return areAttributesForAllCountriesSet;
	}
}
