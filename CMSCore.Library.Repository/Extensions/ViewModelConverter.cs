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

            //var result = new FeedItemViewModel(feedItem.Id, feedItem.Title, feedItem.NormalizedTitle, feedItem.CommentsEnabled, feedItem.Description, feedItem.FeedId, feedItem.Created, feedItem.Modified, comments, content, tags);

            var result = new FeedItemViewModel
            {
                Comments = comments,
                CommentsEnabled = feedItem.CommentsEnabled,
                Id = feedItem.Id,
                Created = feedItem.Created,
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

        private static List<CommentViewModel> GetCommentViewModels(IEnumerable<Comment> comments)
        {
            var results = comments?.Select(x => new CommentViewModel(x.Id, x.Created, x.FullName, x.Content?.ActiveContentValue));

            var models = results?.ToList();
            return models;
            //{
            //    CommentId = x.Id,
            //    Created = x.Created,
            //    FullName = x.FullName,
            //    Text = x.Content?.ActiveContentValue
            //});
        }

        public static ContentViewModel GetContent(Content content)
        {
            var activeVersion = content.ActiveContentVersion;
            var model = new ContentViewModel(content.Id, activeVersion.Id, activeVersion.VersionNumber, activeVersion.Value);
            return model;
            //{
            //    ContentId = content.Id,
            //    VersionNumber = activeVersion.VersionNumber,
            //    VersionId = activeVersion.Id,
            //    Value = activeVersion.Value
            //};
        }

        public static FeedViewModel GetFeedViewModel(Feed feed)
        {
            var feedItems = GetPreviewModels(feed.FeedItems);
            //var feedViewModel = new FeedViewModel(feed.Id, feed.Name, feed.NormalizedName, feed.Created, feed.Modified, feedItemsImmutable);
            var feedViewModel = new FeedViewModel
            {
                Date = feed.Created,
                Modified = feed.Modified,
                Id = feed.Id,
                Name = feed.Name,
                NormalizedName = feed.NormalizedName,
                FeedItems = feedItems
            };

            return feedViewModel;
        }

        private static List<FeedItemPreviewViewModel> GetPreviewModels(IEnumerable<FeedItem> feedItems)
        {
            //var items = feedItems?.Select(x => new FeedItemPreviewViewModel(x.Id, x.Title, x.NormalizedTitle, x.Description, x.Created, x.Modified, GetTags(x.Tags))).ToArray();
            var items = feedItems?.Select(x => new FeedItemPreviewViewModel
            {
                Created = x.Created,
                Modified = x.Modified,
                Id = x.Id,
                Description = x.Description,
                NormalizedTitle = x.NormalizedTitle,
                Title = x.Title,
                Tags = GetTags(x.Tags)
            }).ToList();

            return items;
        }

        public static List<TagViewModel> GetTags(IEnumerable<Tag> tags)
        {
            var results = tags?.Select(x => new TagViewModel(x.Id, x.Name, x.NormalizedName)).ToList();
            return results;
        }
    }
}