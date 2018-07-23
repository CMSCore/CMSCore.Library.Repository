namespace CMSCore.Library.Repository.Implementations
{
    using System.Threading.Tasks;
    using Data.Context;
    using Data.Models;
    using Messages;
    using Microsoft.EntityFrameworkCore;

    public class UpdateContentRepository : IUpdateContentRepository
    {
        private readonly IContentDbContext _context;

        public UpdateContentRepository(IContentDbContext context)
        {
            _context = context;
        }

        public async Task<string> UpdateContent(string contentId, string textContent)
        {
            var content = await _context.Set<Content>()
                .Include(x => x.ContentVersions)
                .FirstOrDefaultAsync(x => x.Id == contentId);

            content.AddContentVersion(textContent);

            var result = _context.Update(content);
            await _context.SaveChangesAsync();
            return result.Id;
        }


        public async Task<int> UpdatePage(UpdatePageViewModel model)
        {
            var page = await _context.Set<Page>().FindAsync(model.Id);
            page.FeedEnabled = model.FeedEnabled;
            page.Name = model.Name;

            _context.Update(page);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateFeedItem(UpdateFeedItemViewModel model)
        {
            var feedItem = await _context.Set<FeedItem>().FindAsync(model.Id);
            feedItem.CommentsEnabled = model.CommentsEnabled;
            feedItem.Description = model.Description;
            feedItem.Title = model.Title;

            _context.Update(feedItem);

            return await _context.SaveChangesAsync();
        }
    }
}