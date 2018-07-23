namespace CMSCore.Library.Repository.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Messages;

    public class ViewModelConverter
    {
        public static FeedItemViewModel GetFeedItemViewModel(FeedItem feedItem)
        {
            var tags = GetTags(feedItem.Tags);
            var content = GetContent(feedItem.Content);
            var comments = GetCommentViewModels(feedItem.Comments);

            var result = new FeedItemViewModel
            {
                Comments = comments,
                CommentsEnabled = feedItem.CommentsEnabled,
                Id = feedItem.Id,
                Date = feedItem.Created,
                Modified = feedItem.Modified,
                Description = feedItem.Description,
                Title = feedItem.Title,
                FeedId = feedItem.FeedId,
                Content = content,
                NormalizedTitle = feedItem.NormalizedTitle,
                Tags = tags
            };
            return result;
        }

        private static CommentViewModel[] GetCommentViewModels(IEnumerable<Comment> comments)
        {
            var results = comments?.Select(x => new CommentViewModel()
            {
                CommentId = x.Id,
                Date = x.Created,
                FullName = x.FullName,
                Text = x.Content?.ActiveContentValue
            });
            return results?.ToArray();
        }

        public static ContentViewModel GetContent(Content content)
        {
            var activeVersion = content.ActiveContentVersion;
            return new ContentViewModel
            {
                ContentId = content.Id,
                VersionNumber = activeVersion.VersionNumber,
                VersionId = activeVersion.Id,
                Value = activeVersion.Value
            };
        }

        public static FeedViewModel GetFeedViewModel(Feed feed)
        {
            var feedItems = GetPreviewModels(feed.FeedItems);
            return new FeedViewModel
            {
                Date = feed.Created,
                Modified = feed.Modified,
                Id = feed.Id,
                Name = feed.Name,
                NormalizedName = feed.NormalizedName,
                FeedItems = feedItems
            };
        }

        private static FeedItemPreviewViewModel[] GetPreviewModels(IEnumerable<FeedItem> feedItems)
        {
            var items = feedItems?.Select(x => new FeedItemPreviewViewModel
            {
                Date = x.Created,
                Modified = x.Modified,
                Id = x.Id,
                Description = x.Description,
                NormalizedTitle = x.NormalizedTitle,
                Title = x.Title,
                Tags = GetTags(x.Tags)
            });
            return items?.ToArray();
        }

        public static TagViewModel[] GetTags(IEnumerable<Tag> tags)
        {
            var results = tags?.Select(x => new TagViewModel { Id = x.Id, Name = x.Name, NormalizedName = x.NormalizedName });
            return results?.ToArray();
        }
    }
}