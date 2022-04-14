using System.Collections.ObjectModel;

namespace ArticleManagementApi.Models.Database;

public class Article
{
	// Only for ef core
	public int Id { get; set; }

	public int ArticleNumber { get; set; }

	public string Brand { get; set; } = string.Empty;

	public bool IsApproved { get; set; }

	public bool IsBulky { get; set; }

	public DateTime LastChanged { get; set; } = DateTime.Now;

	public virtual ICollection<ArticleAttribute> Attributes { get; } = new Collection<ArticleAttribute>();
}

