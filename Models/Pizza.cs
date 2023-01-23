using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace LaMiaPizzeria.Models
{
    public class Pizza
    {
   

        public int Id { get; set; }
        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il nome non può avere più di 100 caratteri")]
        [Column(TypeName = "varchar(100)")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Url]
        [Required]
        public string Image { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public double? Price { get; set; }
        public List<Ingridient>? Ingridients { get; set; }

        public Pizza() { }  
        public Pizza( string title, string description, string image)
        {
            
            Title = title;
            Description = description;
            Image = image;
        }
    }
}
