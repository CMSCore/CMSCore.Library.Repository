using System.Threading.Tasks;

namespace CMSCore.Library.Repository
{
    public interface IDeleteContentRepository
    {
        Task<int> DeleteComment(string id);
        Task<int> DeleteContent(string id);
        Task<int> DeleteContentVersion(string id);
        Task<int> DeleteFeed(string id);
        Task<int> DeleteFeedItem(string id);
        Task<int> DeletePage(string id);
        Task<int> DeleteTag(string id);
    }
}