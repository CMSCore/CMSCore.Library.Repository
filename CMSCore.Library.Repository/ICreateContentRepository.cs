using System.Collections.Generic;
using System.Threading.Tasks;
using CMSCore.Library.Messages;

namespace CMSCore.Library.Repository
{
    public interface ICreateContentRepository
    {
        Task<string> CreateComment(CreateCommentViewModel model);
        Task<string> CreateFeed(CreateFeedViewModel model);
        Task<string> CreateFeedItem(CreateFeedItemViewModel model);
        Task<string> CreatePage(CreatePageViewModel model);
        Task<int> CreateTags(IList<string> tags, string feedItemId);
        Task<string> CreateUser(CreateUserViewModel model);
    }
}