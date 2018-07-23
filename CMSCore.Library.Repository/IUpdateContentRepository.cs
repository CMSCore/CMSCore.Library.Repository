using System.Threading.Tasks;
using CMSCore.Library.Messages;

namespace CMSCore.Library.Repository
{
    public interface IUpdateContentRepository
    {
        Task<string> UpdateContent(string contentId, string textContent);
        Task<int> UpdateFeedItem(UpdateFeedItemViewModel model);
        Task<int> UpdatePage(UpdatePageViewModel model);
    }
}