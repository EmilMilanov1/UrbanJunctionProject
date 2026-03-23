using UrbanJunction.Data;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Tests.Helpers
{
    public static class TestDataSeeder
    {
        public static UrbanUser CreateUser(string id = "user-1", string userName = "testuser")
        {
            return new UrbanUser
            {
                Id = id,
                UserName = userName,
                Email = $"{userName}@test.com",
                ProfilePicturePath = "/images/default.jpg",
                LastActiveOn = DateTime.UtcNow
            };
        }

        public static Topic CreateTopic(int id = 1, string name = "Art")
        {
            return new Topic
            {
                Id = id,
                Name = name,
                Description = $"Everything about {name}",
                ImageUrl = $"/img/{name.ToLower()}.jpg"
            };
        }

        public static Subcategory CreateSubcategory(int id = 1, string name = "Graffiti", int topicId = 1)
        {
            return new Subcategory
            {
                Id = id,
                Name = name,
                TopicId = topicId
            };
        }

        public static Post CreatePost(
            int id = 1,
            string title = "Test Post",
            string content = "Test content",
            string userId = "user-1",
            int subcategoryId = 1)
        {
            return new Post
            {
                Id = id,
                Title = title,
                Content = content,
                UserId = userId,
                SubcategoryId = subcategoryId,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Comment CreateComment(
            int id = 1,
            string content = "Test comment",
            string userId = "user-2",
            int postId = 1,
            int? parentCommentId = null)
        {
            return new Comment
            {
                Id = id,
                Content = content,
                UserId = userId,
                PostId = postId,
                ParentCommentId = parentCommentId,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Reaction CreateReaction(
            int id = 1,
            string userId = "user-2",
            int postId = 1,
            bool isUpvote = true)
        {
            return new Reaction
            {
                Id = id,
                UserId = userId,
                PostId = postId,
                IsUpvote = isUpvote,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Tag CreateTag(int id = 1, string name = "testtag")
        {
            return new Tag { Id = id, Name = name };
        }

        public static ContactMessage CreateContactMessage(
            int id = 1,
            string senderId = "user-1",
            string subject = "Test subject",
            string message = "Test message")
        {
            return new ContactMessage
            {
                Id = id,
                SenderId = senderId,
                Name = "Test User",
                Email = "test@test.com",
                Subject = subject,
                Message = message,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static async Task SeedBasicDataAsync(ApplicationDbContext context)
        {
            var user1 = CreateUser("user-1", "postauthor");
            var user2 = CreateUser("user-2", "commenter");
            var topic = CreateTopic(1, "Art");
            var subcat = CreateSubcategory(1, "Graffiti", 1);
            var post = CreatePost(1, "Test Post", "Test content here", "user-1", 1);

            context.Users.AddRange(user1, user2);
            context.Topics.Add(topic);
            context.Subcategories.Add(subcat);
            context.Posts.Add(post);
            await context.SaveChangesAsync();
        }
    }
}
