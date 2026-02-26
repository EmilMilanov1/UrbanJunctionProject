namespace UrbanJunction.Data.Seeding
{
	public class PostDTO
	{
		public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public DateTime CreatedOn { get; set; }
		public int SubcategoryId { get; set; }
	}

    public static class PostSeeder
	{
        public static IEnumerable<PostDTO> GetPosts()
		{
			IEnumerable<PostDTO> posts = new List<PostDTO>()
			{
				new PostDTO
                {
					Title = "Best Graffiti Spots in Berlin",
					Content = "Check out the East Side Gallery and RAW Gelände!",
					CreatedOn = new DateTime(2026, 1, 19),
					SubcategoryId = 1           // Graffiti
				},
				new PostDTO
                {
					Title = "Underground Techno in Detroit",
					Content = "The scene is raw and authentic. Worth experiencing!",
					CreatedOn = DateTime.UtcNow,
					SubcategoryId = 2           // Techno
				},
				new PostDTO
                {
					Title = "Streetwear Trends for 2025",
					Content = "Baggy is back. Sneakers are getting chunkier than ever.",
					CreatedOn = DateTime.UtcNow,
					SubcategoryId = 3           // Streetwear
				},
				new PostDTO
                {
					Title = "Test test",
					Content = "This is a pure test.",
					CreatedOn = DateTime.UtcNow,
					SubcategoryId = 3           // Streetwear
				}
			};

			return posts;
		}
	}
}
