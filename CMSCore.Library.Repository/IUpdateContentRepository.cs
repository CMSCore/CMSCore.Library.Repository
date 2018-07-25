using System.Threading.Tasks;
using CMSCore.Library.Messages;

namespace CMSCore.Library.Repository
{
    public interface IUpdateContentRepository
    {
        Task<string> UpdateContent(UpdateContentViewModel model);
        Task<string> UpdateFeedItem(UpdateFeedItemViewModel model);
        Task<string> UpdatePage(UpdatePageViewModel model);
    }
}