using NUnit.Framework;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Implementations;
using UrbanJunction.Tests.Helpers;

namespace UrbanJunction.Tests.Services
{
    [TestFixture]
    public class TagServiceTests
    {
        private TagService CreateService(UrbanJunction.Data.ApplicationDbContext context)
        {
            return new TagService(context);
        }

        [Test]
        public async Task GetOrCreateAsync_CreatesNewTag_WhenNotFound()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var tag = await service.GetOrCreateAsync("graffiti");

            Assert.That(tag, Is.Not.Null);
            Assert.That(tag.Name, Is.EqualTo("graffiti"));
            Assert.That(context.Tags.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetOrCreateAsync_ReturnsExistingTag_WhenFound()
        {
            var context = TestDbContextFactory.Create();
            context.Tags.Add(TestDataSeeder.CreateTag("graffiti"));
            await context.SaveChangesAsync();

            var existing = context.Tags.First();
            var service = CreateService(context);
            var tag = await service.GetOrCreateAsync("graffiti");

            Assert.That(tag.Id, Is.EqualTo(existing.Id));
            Assert.That(context.Tags.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetOrCreateAsync_NormalizesName_ToLowercase()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var tag = await service.GetOrCreateAsync("Graffiti");

            Assert.That(tag.Name, Is.EqualTo("graffiti"));
        }

        [Test]
        public async Task GetOrCreateAsync_StripsPoundSign_FromTagName()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var tag = await service.GetOrCreateAsync("#graffiti");

            Assert.That(tag.Name, Is.EqualTo("graffiti"));
        }

        [Test]
        public async Task GetOrCreateAsync_TrimsWhitespace_FromTagName()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var tag = await service.GetOrCreateAsync("  graffiti  ");

            Assert.That(tag.Name, Is.EqualTo("graffiti"));
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllTags()
        {
            var context = TestDbContextFactory.Create();
            context.Tags.AddRange(
                TestDataSeeder.CreateTag("graffiti"),
                TestDataSeeder.CreateTag("techno"),
                TestDataSeeder.CreateTag("streetwear")
            );
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var tags = await service.GetAllAsync();

            Assert.That(tags.Count(), Is.EqualTo(3));
        }
    }

    [TestFixture]
    public class TopicServiceTests
    {
        private TopicService CreateService(UrbanJunction.Data.ApplicationDbContext context)
        {
            return new TopicService(context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllTopicsWithSubcategories()
        {
            var context = TestDbContextFactory.Create();

            var topic1 = TestDataSeeder.CreateTopic("Art");
            var topic2 = TestDataSeeder.CreateTopic("Music");
            context.Topics.AddRange(topic1, topic2);
            await context.SaveChangesAsync();

            var subcat = TestDataSeeder.CreateSubcategory("Graffiti", topic1.Id);
            context.Subcategories.Add(subcat);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var topics = await service.GetAllAsync();

            Assert.That(topics.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByNameAsync_ReturnsTopic_WhenFound()
        {
            var context = TestDbContextFactory.Create();
            context.Topics.Add(TestDataSeeder.CreateTopic("Art"));
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var topic = await service.GetByNameAsync("Art");

            Assert.That(topic, Is.Not.Null);
            Assert.That(topic!.Name, Is.EqualTo("Art"));
        }

        [Test]
        public async Task GetByNameAsync_ReturnsNull_WhenNotFound()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var topic = await service.GetByNameAsync("Nonexistent");

            Assert.That(topic, Is.Null);
        }
    }
}