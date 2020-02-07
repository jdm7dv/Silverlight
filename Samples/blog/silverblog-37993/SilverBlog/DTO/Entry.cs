using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverBlog.DTO
{
    public class Entry
    {
        public int EntryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public bool? CommentsEnabled { get; set; }
        public decimal? Rating { get; set; }
        public DateTime? Modified { get; set; }
        public bool? Published { get; set; }
    }
}
