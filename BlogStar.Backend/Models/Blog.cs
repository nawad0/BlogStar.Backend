using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BlogStar.Backend.Models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? UserName { get; set; }
        public int OwnerUserId { get; set; }
       
        public DateTime CreationDate { get; set; }

    }
}
