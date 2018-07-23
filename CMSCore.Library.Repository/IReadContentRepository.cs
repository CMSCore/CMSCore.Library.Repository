using System.Collections.Generic;
using System.Threading.Tasks;
using CMSCore.Library.Messages;

namespace CMSCore.Library.Repository
{
    public interface IReadContentRepository
    {
        Task<FeedItemViewModel> GetFeedItem(string feedItemId);
        Task<FeedItemViewModel> GetFeedItemByNormalizedName(string normalizedName);
        Task<PageViewModel> GetPageById(string pageId);
        Task<PageViewModel> GetPageByNormalizedName(string normalizedName);
        Task<IEnumerable<PageTreeViewModel>> GetPageTree();
        Task<IEnumerable<TagViewModel>> GetTags();
        Task<IEnumerable<TagViewModel>> GetTags(string feedItemId);
        Task<IEnumerable<UserViewModel>> GetUsers();
    }
}