using System.Collections.Generic;
using System.Threading.Tasks;
using CMSCore.Library.Messages;

namespace CMSCore.Library.Repository
{
    using Messages.Read;
    using Orleans.Concurrency;

    public interface IReadContentRepository
    {
        Task<FeedItemViewModel> GetFeedItem(string feedItemId);
        Task<FeedItemViewModel> GetFeedItemByNormalizedName(string normalizedName);
        Task<PageViewModel> GetPageById(string pageId);
        Task<PageViewModel> GetPageByNormalizedName(string normalizedName);
        Task<PageTreeViewModel [ ]> GetPageTree();
        Task<TagViewModel [ ]> GetTags();
        Task<TagViewModel [ ]> GetTags(string feedItemId);
        Task<UserViewModel [ ]> GetUsers();
    }
}