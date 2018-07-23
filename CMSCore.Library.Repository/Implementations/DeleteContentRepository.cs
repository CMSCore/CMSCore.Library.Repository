namespace CMSCore.Library.Repository.Implementations
{
    using System.Threading.Tasks;
    using Data.Context;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class DeleteContentRepository : IDeleteContentRepository
    {
        private readonly IContentDbContext _context;

        public DeleteContentRepository(IContentDbContext context)
        {
            _context = context;
        }


        public async Task<int> DeleteFeedItem(string id)
        {
            return await Delete<FeedItem>(id);
        }

        public async Task<int> DeletePage(string id)
        {
            return await Delete<Page>(id);
        }

        public async Task<int> DeleteComment(string id)
        {
            return await Delete<Comment>(id);
        }

        public async Task<int> DeleteContent(string id)
        {
            return await Delete<Content>(id);
        }

        public async Task<int> DeleteContentVersion(string id)
        {
            return await Delete<ContentVersion>(id);
        }

        public async Task<int> DeleteTag(string id)
        {
            return await Delete<Tag>(id);
        }

        public async Task<int> DeleteFeed(string id)
        {
            return await Delete<Feed>(id);
        }


        private async Task<int> Delete<TEntity>(string id) where TEntity : EntityBase
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return 0;
            }

            _context.Remove(entity);
            return await _context.SaveChangesAsync();
        }
    }
}