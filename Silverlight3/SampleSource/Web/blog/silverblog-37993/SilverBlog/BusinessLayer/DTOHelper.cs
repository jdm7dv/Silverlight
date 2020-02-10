using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverBlog.BusinessLayer
{
    public static class DTOHelper
    {
        public static DTO.Entry AsDto(this DataLayer.Model.Entry entry)
        {
            return new DTO.Entry
            {
                EntryId = entry.EntryId,
                Title = entry.Title,
                Description = entry.Description,
                Content = entry.Content,
                CommentsEnabled = entry.CommentsEnabled,
                CreatedDate = entry.Created.HasValue ? entry.Created.Value.ToLongDateString() : "unknown",
                Modified = entry.Modified,
                Rating = entry.Rating,
                Published = entry.Published
            };
        }

        public static DataLayer.Model.Entry AsModel(this DTO.Entry entry)
        {
            return new DataLayer.Model.Entry
            {
                EntryId = entry.EntryId,
                Title = entry.Title,
                Description = entry.Description,
                Content = entry.Content,
                Published = entry.Published,
                Rating = entry.Rating,
                Modified = entry.Modified,
                CommentsEnabled = entry.CommentsEnabled
            };
        }

        public static IEnumerable<DTO.Entry> AsDTO(this IEnumerable<DataLayer.Model.Entry> entries)
        {
            return entries.Select(e => e.AsDto());
        }
    }
}
