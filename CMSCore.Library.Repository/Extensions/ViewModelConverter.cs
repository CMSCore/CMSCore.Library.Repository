namespace CMSCore.Library.Repository.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Messages;
    using Messages.Read;
    using Orleans.Concurrency;

    public class ViewModelConverter
    {
        public static FeedItemViewModel GetFeedItemViewModel(FeedItem feedItem)
        {
            var tags = GetTags(feedItem.Tags);
            var content = GetContent(feedItem.Content);
            var comments = GetCommentViewModels(feedItem.Comments);

            var result = new FeedItemViewModel(feedItem.Id, feedItem.Title, feedItem.NormalizedTitle, feedItem.CommentsEnabled, feedItem.Description, feedItem.FeedId, feedItem.Created, feedItem.Modified, comments, content, tags);
            return result;
            //{
            //    Comments = comments,
            //    CommentsEnabled = feedItem.CommentsEnabled,
            //    Id = feedItem.Id,
            //    Created = feedItem.Created,
            //    Modified = feedItem.Modified,
            //    Description = feedItem.Description,
            //    Title = feedItem.Title,
            //    FeedId = feedItem.FeedId,
            //    Content = content,
            //    NormalizedTitle = feedItem.NormalizedTitle,
            //    Tags = tags
            //};
        }

        private static CommentViewModel [ ] GetCommentViewModels(IEnumerable<Comment> comments)
        {
            var results = comments?.Select(x => new CommentViewModel(x.Id, x.Created, x.FullName, x.Content?.ActiveContentValue));
            //{
            //    CommentId = x.Id,
            //    Created = x.Created,
            //    FullName = x.FullName,
            //    Text = x.Content?.ActiveContentValue
            //});
            var models = results?.ToArray();
            return models;
        }

        public static ContentViewModel GetContent(Content content)
        {
            var activeVersion = content.ActiveContentVersion;
            var model = new ContentViewModel(content.Id, activeVersion.Id, activeVersion.VersionNumber, activeVersion.Value);
            //{
            //    ContentId = content.Id,
            //    VersionNumber = activeVersion.VersionNumber,
            //    VersionId = activeVersion.Id,
            //    Value = activeVersion.Value
            //};
            return model;
        }

        public static FeedViewModel GetFeedViewModel(Feed feed)
        {
            var feedItemsImmutable = GetPreviewModels(feed.FeedItems);
            var feedViewModel = new FeedViewModel(feed.Id, feed.Name, feed.NormalizedName, feed.Created, feed.Modified, feedItemsImmutable);
            return feedViewModel;


            //{
            //    Date = feed.Created,
            //    Modified = feed.Modified,
            //    Id = feed.Id,
            //    Name = feed.Name,
            //    NormalizedName = feed.NormalizedName,
            //    FeedItems = feedItems
            //};
        }

        private static FeedItemPreviewViewModel [ ] GetPreviewModels(IEnumerable<FeedItem> feedItems)
        {
            var items = feedItems?.Select(x => new FeedItemPreviewViewModel(x.Id, x.Title, x.NormalizedTitle, x.Description, x.Created, x.Modified, GetTags(x.Tags))).ToArray();
            return items;
            //{
            //    Created = x.Created,
            //    Modified = x.Modified,
            //    Id = x.Id,
            //    Description = x.Description,
            //    NormalizedTitle = x.NormalizedTitle,
            //    Title = x.Title,
            //    Tags = GetTags(x.Tags)
            //});
        }

        public static TagViewModel [ ] GetTags(IEnumerable<Tag> tags)
        {
            var results = tags?.Select(x => new TagViewModel(x.Id, x.Name, x.NormalizedName)).ToArray();
            return results;
        }
    }
}