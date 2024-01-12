using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogStar.Backend.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? AuthorUserId { get; set; }
        public string? AuthorUserName { get; set; }
        public string? AuthorImagePath { get; set; }

        public int? BlogId { get; set; }
        public string? PublicationDate { get; set; }
        public string? ContentHtml { get; set; }

        // Store likes as a JSON string
        public string? LikesJson { get; set; }
    }

    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public int UserId { get; set; }
    }


}
