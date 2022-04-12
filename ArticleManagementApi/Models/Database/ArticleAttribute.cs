
namespace ArticleManagementApi.Models.Database;

public class ArticleAttribute
{
	public int Id { get; set; }

	public string Title { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public string Color { get; set; } = string.Empty;

	public Country Country { get; set; }

	public DateTime LastChange { get; set; } = DateTime.Now;

	public virtual int ArticleId { get; set; }
}
