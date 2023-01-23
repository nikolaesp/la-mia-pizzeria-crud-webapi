using Azure;
using LaMiaPizzeria.Models;
using LaMiaPizzeria.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;

namespace LaMiaPizzeria.Controllers
{
    public class PizzaController : Controller
    {


        public IActionResult Index()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> listaPizza = db.Pizza.ToList<Pizza>();
                return View("Index", listaPizza);
            }
        }

        public IActionResult Details(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                PizzaCategory pizzatrovata = new PizzaCategory();
                    pizzatrovata.Pizza = db.Pizza
                    .Where(SingoloPizzaNelDb => SingoloPizzaNelDb.Id == id).Include(pizza => pizza.Category)
                    .Include(pizza => pizza.Ingridients)
                    .FirstOrDefault();
                pizzatrovata.Categories = db.Categories.ToList();
               
                if (pizzatrovata.Pizza != null)
                {
                    return View(pizzatrovata);
                }
                return NotFound("Non ci sono pizze presenti");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Category> categoriesFromDb = db.Categories.ToList<Category>();

                PizzaCategory modelForView = new PizzaCategory();
                modelForView.Pizza = new Pizza();

                modelForView.Categories = categoriesFromDb;
                modelForView.Ingridients = IngridientsConverter.getListTagsForMultipleSelect();
                return View("Create", modelForView);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategory formData)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();
                    formData.Categories = categories;


                    formData.Ingridients = IngridientsConverter.getListTagsForMultipleSelect();
                }

                return View("Create", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {
                if (formData.IngridientsSelected != null)
                {
                    formData.Pizza.Ingridients = new List<Ingridient>();
                    double fprice = 0;
                    foreach (string ingridientid in formData.IngridientsSelected)
                    {
                        int ingIdIntFromSelect = int.Parse(ingridientid);

                        Ingridient ing = db.Ingridients.Where(ingDB => ingDB.Id == ingIdIntFromSelect).FirstOrDefault();

                        // todo controllare eventuali altri errori tipo l'id del tag non esiste
                        fprice += ing.Price;
                        formData.Pizza.Ingridients.Add(ing);
                    }
                    Category catprice = db.Categories.Where(catDB => catDB.Id == formData.Pizza.CategoryId).FirstOrDefault();

                    formData.Pizza.Price = fprice + catprice.Price;
                }
               
                db.Pizza.Add(formData.Pizza);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
               
                Pizza pizzaupdate = db.Pizza.Where(pizzas => pizzas.Id == id).Include(pizza => pizza.Ingridients).FirstOrDefault();
                   
                if (pizzaupdate == null)
                {
                    return NotFound("Il post non è stato trovato");
                }
                List<Category> categories = db.Categories.ToList<Category>();

                PizzaCategory modelForView = new PizzaCategory();
                modelForView.Pizza = pizzaupdate;
                modelForView.Categories = categories;
                List<Ingridient> listIngFromDb = db.Ingridients.ToList<Ingridient>();

                List<SelectListItem> listaOpzioniPerLaSelect = new List<SelectListItem>();

                foreach (Ingridient ing in listIngFromDb)
                {
                    // Ricerco se il tag che sto inserindo nella lista delle opzioni della select era già stato selezionato dall'utente
                    // all'interno della lista dei tag del post da modificare
                    bool eraStatoSelezionato = pizzaupdate.Ingridients.Any(ingSelect => ingSelect.Id == ing.Id);

                    SelectListItem opzioneSingolaSelect = new SelectListItem() { Text = ing.Name, Value = ing.Id.ToString(), Selected = eraStatoSelezionato };
                    listaOpzioniPerLaSelect.Add(opzioneSingolaSelect);
                }

                modelForView.Ingridients = listaOpzioniPerLaSelect;


                return View("Update", modelForView);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id ,PizzaCategory formData)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();

                    formData.Categories = categories;
                }
                return View("Update", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaupdate = db.Pizza.Where(articolo => articolo.Id == id).Include(pizza => pizza.Ingridients).FirstOrDefault();

                if (pizzaupdate != null)
                {
                  
                    pizzaupdate.Title = formData.Pizza.Title;
                    pizzaupdate.Description = formData.Pizza.Description;
                    pizzaupdate.Image = formData.Pizza.Image;
                    pizzaupdate.Category = formData.Pizza.Category;
                    pizzaupdate.CategoryId = formData.Pizza.CategoryId;
                    
                    pizzaupdate.Ingridients.Clear();

                    Category catprice = db.Categories.Where(catDB => catDB.Id == formData.Pizza.CategoryId).FirstOrDefault();
                    double fprice = catprice.Price;

                    if (formData.IngridientsSelected != null)
                    {

                        foreach (string ingID in formData.IngridientsSelected)
                        {
                            int ingIDSelect = int.Parse(ingID);

                            Ingridient ing = db.Ingridients.Where(ingDb => ingDb.Id == ingIDSelect).FirstOrDefault();

                            // todo controllare eventuali altri errori tipo l'id del tag non esiste
                            fprice += ing.Price;

                            pizzaupdate.Ingridients.Add(ing);
                        }
                    }

                    pizzaupdate.Price = fprice;
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
                else
                {
                    return NotFound("Il post che volevi modificare non è stato trovato!");
                }
            }

        }

        //[HttpDelete] // se qui metti metodo delete devi mettere metodo delete anche nella view. Oppure usi metodo post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzadelete = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzadelete != null)
                {
                    db.Pizza.Remove(pizzadelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Il post da eliminare non è stato trovato!");
                }
            }
        }
    }

}


    

