using System.Collections.ObjectModel;

namespace ArticleManagementApi.Models.Database;

public class Article
{
	public Article(int articleNumber, string brand, bool isBulky)
	{
		ArticleNumber = articleNumber;
		Brand = brand;
		IsBulky = isBulky;
		IsApproved = false;
		LastChanged = DateTime.Now;
	}

	// Only for ef core
	// private setter for ef core
	public int Id { get; private set; }

	// private setter for ef core
	public int ArticleNumber { get; private set; }

	public string Brand { get; set; }

	public bool IsApproved { get; set; }

	public bool IsBulky { get; set; }

	public DateTime LastChanged { get; set; }

	public virtual ICollection<ArticleAttribute> Attributes { get; } = new Collection<ArticleAttribute>();
}

