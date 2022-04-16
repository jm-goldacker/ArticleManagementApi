
namespace ArticleManagementApi.Models.Database;

public class ArticleAttribute
{
	public ArticleAttribute(string title, string description, string color, Country country)
	{
		Color = color;
		Description = description;
		Title = title;
		Country = country;
		LastChange = DateTime.Now;
	}

	// Only for ef core
	public int Id { get; private set; }

	public string Title { get; set; }

	public string Description { get; set; }

	public string Color { get; set; }

	// private setter for ef core
	public Country Country { get; private set; }

	public DateTime LastChange { get; set; }

	public virtual Article Article { get; set; }
}
