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
        Task<List<PageTreeViewModel>> GetPageTree();
        Task<List<TagViewModel>> GetTags();
        Task<List<TagViewModel>> GetTags(string feedItemId);
        Task<List<UserViewModel>> GetUsers();
    }
}