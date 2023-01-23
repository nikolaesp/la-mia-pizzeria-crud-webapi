using LaMiaPizzeria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaMiaPizzeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            using( PizzaContext db =new PizzaContext() )
            {
                List<Pizza> pizzas = db.Pizza.Include(pizza => pizza.Ingridients).ToList<Pizza>(); 
                return Ok( pizzas );
            }
        }
    }
}
