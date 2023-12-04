using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorUserId { get; set; }
        public int BlogId { get; set; }
        public DateTime PublicationDate { get; set; }
        // Изображения и мультимедийный контент (опционально)
        // Количество просмотров и другие статистические данные
    }
}
