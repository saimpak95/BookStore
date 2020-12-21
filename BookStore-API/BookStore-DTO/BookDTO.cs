using System.ComponentModel.DataAnnotations;

namespace BookStore_DTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int? AuthorID { get; set; }
        public virtual AuthorDTO Author { get; set; }
    }

    public class BookCreateDTO
    {
        [Required]
        public string Title { get; set; }

        public int? Year { get; set; }

        [Required]
        public string ISBN { get; set; }

        [MaxLength(150)]
        public string Summary { get; set; }

        public string Image { get; set; }
        public decimal Price { get; set; }

        [Required]
        public int AuthorID { get; set; }
    }

    public class BookUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

       
        public int? Year { get; set; }

        [Required]
        public string ISBN { get; set; }

        [MaxLength(150)]
        public string Summary { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int AuthorID { get; set; }
    }
}