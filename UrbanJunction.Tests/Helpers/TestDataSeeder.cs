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

        public static Topic CreateTopic(string name = "Art")
        {
            return new Topic
            {
                Name = name,
                Description = $"Everything about {name}",
                ImageUrl = $"/img/{name.ToLower()}.jpg"
            };
        }

        public static Subcategory CreateSubcategory(string name = "Graffiti", int topicId = 0)
        {
            return new Subcategory
            {
                Name = name,
                TopicId = topicId
            };
        }

        public static Post CreatePost(
            string title = "Test Post",
            string content = "Test content",
            string userId = "user-1",
            int subcategoryId = 0)
        {
            return new Post
            {
                Title = title,
                Content = content,
                UserId = userId,
                SubcategoryId = subcategoryId,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Comment CreateComment(
            string content = "Test comment",
            string userId = "user-2",
            int postId = 0,
            int? parentCommentId = null)
        {
            return new Comment
            {
                Content = content,
                UserId = userId,
                PostId = postId,
                ParentCommentId = parentCommentId,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Reaction CreateReaction(
            string userId = "user-2",
            int postId = 0,
            bool isUpvote = true)
        {
            return new Reaction
            {
                UserId = userId,
                PostId = postId,
                IsUpvote = isUpvote,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Tag CreateTag(string name = "testtag")
        {
            return new Tag { Name = name };
        }

        public static ContactMessage CreateContactMessage(
            string senderId = "user-1",
            string subject = "Test subject",
            string message = "Test message")
        {
            return new ContactMessage
            {
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
            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();

            var topic = CreateTopic("Art");
            context.Topics.Add(topic);
            await context.SaveChangesAsync();

            var subcat = CreateSubcategory("Graffiti", topic.Id);
            context.Subcategories.Add(subcat);
            await context.SaveChangesAsync();

            var post = CreatePost("Test Post", "Test content here", "user-1", subcat.Id);
            context.Posts.Add(post);
            await context.SaveChangesAsync();
        }
    }
}