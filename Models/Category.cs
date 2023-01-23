using Microsoft.Extensions.Hosting;

namespace LaMiaPizzeria.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }

        public List<Pizza> Pizzas { get; set; }

        public Category()
        {

        }

        public Category(int id, string title, double price)
        {
            Id = id;
            Title = title;
            Price = price;
        }
    }
}
