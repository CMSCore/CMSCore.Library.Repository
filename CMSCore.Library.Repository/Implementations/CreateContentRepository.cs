namespace CMSCore.Library.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Data.Context;
    using Data.Models;
    using Messages;

    public class CreateContentRepository : ICreateContentRepository
    {
        private readonly IContentDbContext _context;

        public CreateContentRepository(IContentDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateComment(CreateCommentViewModel model)
        {
            var comment = new Comment(model.FeedItemId, model.Text, model.FullName);

            _context.Add(comment);

            await _context.SaveChangesAsync();
            return comment.Id;
        }

        public async Task<string> CreateFeed(CreateFeedViewModel model)
        {
            var feed = new Feed(model.PageId, model.Name);
            _context.Add(feed);
            await _context.SaveChangesAsync();
            return feed.Id;
        }

        public async Task<string> CreatePage(CreatePageViewModel model)
        {
            var page = new Page(model.Name, model.FeedEnabled)
            {
                Name = model.Name,
                Content = new Content(model.Content)
            };
            _context.Add(page);

            if (model.FeedEnabled)
            {
                var feed = new Feed(page.Id, model.FeedName ?? model.Name + "-feed");
                page.Feed = feed;
                _context.Add(feed);
            }

            await _context.SaveChangesAsync();
            return page.Id;
        }

        public async Task<int> CreateTags(IList<string> tags, string feedItemId)
        {
            var tagsToCreate = tags.Select(name => new Tag(feedItemId, name));

            _context.AddRange(tagsToCreate);

            return await _context.SaveChangesAsync();
        }

        public async Task<string> CreateUser(CreateUserViewModel model)
        {
            var user = new User(model.IdentityUserId)
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            _context.Add(user);

            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<string> CreateFeedItem(CreateFeedItemViewModel model)
        {
            var tags = model.Tags?.Select(name => new Tag(name)).ToList();

            var feedItem = new FeedItem(model.FeedId, model.Title)
            {
                Content = new Content(model.Content),
                Description = model.Description,
                Tags = tags
            };

            var result = _context.Add(feedItem);
            await _context.SaveChangesAsync();
            return result.Id;
        }
    }
}