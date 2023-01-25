using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace LaMiaPizzeria.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        [JsonIgnore]
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
