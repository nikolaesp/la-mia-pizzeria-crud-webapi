using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaMiaPizzeria.Models
{
    public class PizzaCategory
    {
        public Pizza Pizza { get; set; }
        public List<Category>? Categories { get; set;}

        public List<SelectListItem>? Ingridients { get; set; }
        
        public List<string>? IngridientsSelected { get; set;}
    }
}
