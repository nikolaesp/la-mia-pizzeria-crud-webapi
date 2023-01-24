using LaMiaPizzeria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace LaMiaPizzeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string? search)
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizzas = new List<Pizza>();

                if (search is null || search == "")
                {
                    pizzas = db.Pizza.Include(articolo => articolo.Ingridients).ToList<Pizza>();
                }
                else
                {
                    // converto tutto in stringa minuscola, non mi interessano le lettere maiuscole
                    search = search.ToLower();

                    pizzas = db.Pizza.Where(pizza => pizza.Title.ToLower().Contains(search))
                                       .Include(articolo => articolo.Ingridients)
                                       .ToList<Pizza>();
                }

                return Ok(pizzas);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzas = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzas is null)
                {
                    return NotFound("L'articolo con questo id non è stato trovato!");
                }

                return Ok(pizzas);
            }
        }




    }
}




