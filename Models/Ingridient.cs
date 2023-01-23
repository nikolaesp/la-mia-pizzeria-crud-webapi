namespace LaMiaPizzeria.Models
{
    public class Ingridient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public List<Pizza> Pizzas { get; set; }

        public Ingridient()
        { 
        }

        public Ingridient(int id, string name, double price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
