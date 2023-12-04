using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int AuthorUserId { get; set; }
        public int ArticleId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
