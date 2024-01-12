using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public string? Text{ get; set; }
        public int? AuthorUserId { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorImagePath { get; set; }
        public int? ArticleId { get; set; }
        public string? PublicationDate { get; set; }
    }
}
