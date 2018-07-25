namespace CMSCore.Library.Repository.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Context;
    using Data.Models;
    using Extensions;
    using Messages;
    using Microsoft.EntityFrameworkCore;

    public class ReadContentRepository : IReadContentRepository
    {
        private readonly IContentDbContext _context;

        public ReadContentRepository(IContentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PageTreeViewModel>> GetPageTree()
        {
            var pages = await _context.Set<Page>().ToListAsync();

            return pages.Select(x => new PageTreeViewModel
            {
                Id = x.Id,
                Date = x.Created,
                Name = x.Name,
                NormalizedName = x.NormalizedName
            });
        }

        public async Task<IEnumerable<TagViewModel>> GetTags()
        {
            var eq = new TagComparer();
            var tagsDistinct = await _context.Set<Tag>().Distinct(eq).ToListAsync();
            return ViewModelConverter.GetTags(tagsDistinct);
        }

        public async Task<IEnumerable<UserViewModel>> GetUsers()
        {
            var vms = _context.Set<User>().Select(x => new UserViewModel
            {
                Id = x.Id,
                Created = x.Created,
                Modified = x.Modified,
                Email = x.Email,
                FirstName = x.FirstName,
                IdentityUserId = x.IdentityUserId,
                LastName = x.LastName
            });

            return await vms.ToListAsync();
        }
 
         
        public async Task<FeedItemViewModel> GetFeedItem(string feedItemId)
        {
            var feedItem = await _context.Set<FeedItem>().FirstOrDefaultAsync(x => x.Id == feedItemId);
            var model = ViewModelConverter.GetFeedItemViewModel(feedItem);
            return model;
        }

        public async Task<FeedItemViewModel> GetFeedItemByNormalizedName(string normalizedName)
        {
            var feedItem = await _context.Set<FeedItem>().FirstOrDefaultAsync(x => x.NormalizedTitle == normalizedName);
            var model = ViewModelConverter.GetFeedItemViewModel(feedItem);
            return model;
        }

       

        public async Task<PageViewModel> GetPageByNormalizedName(string normalizedName)
        {
            var page = await _context.Set<Page>()
                .Include(x => x.Feed)
                .ThenInclude(x => x.FeedItems)
                .ThenInclude(x => x.Tags)
                .Include(x => x.Content)
                .FirstOrDefaultAsync(x => x.NormalizedName == normalizedName);

            if (page == null) return null;

            var content = ViewModelConverter.GetContent(page.Content);
            var feed = ViewModelConverter.GetFeedViewModel(page.Feed);

            return new PageViewModel
            {
                Content = content,
                Id = page.Id,
                Date = page.Created,
                Modified = page.Modified,
                Name = page.Name,
                NormalizedName = page.NormalizedName,
                Feed = feed
            };
        }

        public async Task<PageViewModel> GetPageById(string pageId)
        {
            var page = await _context.Set<Page>()
                .Include(x => x.Feed)
                .ThenInclude(x => x.FeedItems)
                .ThenInclude(x => x.Tags)
                .Include(x => x.Content)
                .FirstOrDefaultAsync(x => x.Id == pageId);

            if (page == null) return null;

            var content = ViewModelConverter.GetContent(page.Content);
            var feed = ViewModelConverter.GetFeedViewModel(page.Feed);

            return new PageViewModel
            {
                Content = content,
                Id = page.Id,
                Date = page.Created,
                Modified = page.Modified,
                Name = page.Name,
                NormalizedName = page.NormalizedName,
                Feed = feed
            };
        }


        public async Task<IEnumerable<TagViewModel>> GetTags(string feedItemId)
        {
            var tags = await _context.Set<Tag>().Where(x => x.FeedItemId == feedItemId).ToListAsync();

            return ViewModelConverter.GetTags(tags);
        }


        internal class TagComparer : IEqualityComparer<Tag>
        {
            public bool Equals(Tag x, Tag y)
            {
                if (x.NormalizedName.Equals(y.NormalizedName))
                {
                    return true;
                }

                return false;
            }

            public int GetHashCode(Tag obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}