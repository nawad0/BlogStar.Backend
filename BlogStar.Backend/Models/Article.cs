using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
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
        public int? BlogId { get; set; }
        public string? PublicationDate { get; set; }
        public string? ContentHtml { get; set; }
        // Изображения и мультимедийный контент (опционально)
        // Количество просмотров и другие статистические данные
    }
}
