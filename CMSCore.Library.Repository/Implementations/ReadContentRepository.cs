namespace CMSCore.Library.Repository.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Context;
    using Data.Models;
    using Extensions;
    using Messages;
    using Messages.Read;
    using Microsoft.EntityFrameworkCore;
    using Orleans.Concurrency;

    public class ReadContentRepository : IReadContentRepository
    {
        private readonly IContentDbContext _context;

        public ReadContentRepository(IContentDbContext context)
        {
            _context = context;
        }

        public async Task<List<PageTreeViewModel>> GetPageTree()
        {
            return await _context.Set<Page>()
                .Select(x => new PageTreeViewModel(x.Created, x.Id, x.Name, x.NormalizedName))
                .ToListAsync();
        }

        public async Task<List<TagViewModel>> GetTags()
        {
            //var eq = new TagComparer();
            var tagsDistinct = await _context.Set<Tag>()
                //.Distinct(eq)
                .ToListAsync();
            return ViewModelConverter.GetTags(tagsDistinct);
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            var results = await _context.Set<User>().Select(x => new UserViewModel(x.Created, x.Email, x.FirstName, x.Id, x.IdentityUserId, x.LastName, x.Modified)).ToListAsync();

            return results;
        }

        private IQueryable<FeedItem> FeedItemSetWithRelatedEntities()
        {
            var set1 = _context.Set<FeedItem>();
            set1.Include(x => x.Comments).ThenInclude(x => x.Content).ThenInclude(x => x.ContentVersions).Load();
            return set1;
        }
        public async Task<FeedItemViewModel> GetFeedItem(string feedItemId)
        {
            var feedItem = await FeedItemSetWithRelatedEntities().Include(x => x.Content).ThenInclude(x => x.ContentVersions).FirstOrDefaultAsync(x => x.Id == feedItemId);
            var model = ViewModelConverter.GetFeedItemViewModel(feedItem);
            return model;
        }

        public async Task<FeedItemViewModel> GetFeedItemByNormalizedName(string normalizedName)
        {
            var feedItem = await FeedItemSetWithRelatedEntities().Include(x => x.Content).ThenInclude(x => x.ContentVersions).FirstOrDefaultAsync(x => x.NormalizedTitle == normalizedName);
            var model = ViewModelConverter.GetFeedItemViewModel(feedItem);
            return model;
        }


        public async Task<PageViewModel> GetPageByNormalizedName(string normalizedName)
        {
            var pageSet = GetPageSetWithRelatedEntities();
            var page = await pageSet.FirstOrDefaultAsync(x => x.NormalizedName == normalizedName);

            if (page == null)
            {
                throw new Exception("Page could not be loaded");
            }

            var content = ViewModelConverter.GetContent(page.Content);
            var feed = ViewModelConverter.GetFeedViewModel(page.Feed);

            var result = new PageViewModel(page.Id, page.Name, page.NormalizedName, page.Created, page.Modified, content, feed);
            return result;
        }


        public async Task<PageViewModel> GetPageById(string pageId)
        {
            var pageSet = GetPageSetWithRelatedEntities();
            var page = await pageSet.FirstOrDefaultAsync(x => x.Id == pageId);

            if (page == null)
            {
                throw new Exception("Page could not be loaded");
            }

            var content = ViewModelConverter.GetContent(page.Content);
            var feed = ViewModelConverter.GetFeedViewModel(page.Feed);

            var viewModel = new PageViewModel(page.Id, page.Name, page.NormalizedName, page.Created, page.Modified, content, feed);
            return viewModel;
        }


        public async Task<List<TagViewModel>> GetTags(string feedItemId)
        {
            var tags = await _context.Set<Tag>().Where(x => x.FeedItemId == feedItemId).ToListAsync();

            return ViewModelConverter.GetTags(tags);
        }

        private IQueryable<Page> GetPageSetWithRelatedEntities()
        {
            return _context.Set<Page>().Include(x => x.Content).ThenInclude(x => x.ContentVersions).Include(x => x.Feed).ThenInclude(x => x.FeedItems).ThenInclude(x => x.Tags);
        }
    }
}

//{
//    Id = x.Id,
//    Created = x.Created,
//    Modified = x.Modified,
//    Email = x.Email,
//    FirstName = x.FirstName,
//    IdentityUserId = x.IdentityUserId,
//    LastName = x.LastName
//});
//var pageSet = _context.Set<Page>()
//    .Include(x => x.Feed)
//    .ThenInclude(x => x.FeedItems)
//    .ThenInclude(x => x.Tags)
//    .Include(x => x.Content)
//    .ThenInclude(x => x.ContentVersions);

//return new PageViewModel
//{
//    Content = content,
//    Id = page.Id,
//    Created = page.Created,
//    Modified = page.Modified,
//    Name = page.Name,
//    NormalizedName = page.NormalizedName,
//    Feed = feed
//};
//{
//    Content = content,
//    Id = page.Id,
//    Created = page.Created,
//    Modified = page.Modified,
//    Name = page.Name,
//    NormalizedName = page.NormalizedName,
//    Feed = feed
//};