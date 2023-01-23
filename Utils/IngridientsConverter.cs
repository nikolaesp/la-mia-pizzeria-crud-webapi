using Azure;
using LaMiaPizzeria.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaMiaPizzeria.Utils
{
    public class IngridientsConverter
    {

        public static List<SelectListItem> getListTagsForMultipleSelect()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Ingridient> ingridientFromDb = db.Ingridients.ToList();

                // Creare una lista di SelectListItem e tradurci al suo interno tutti i nostri Tag che provengono da Db
                List<SelectListItem> listaPerLaSelectMultipla = new List<SelectListItem>();

                foreach (Ingridient ingridient in ingridientFromDb)
                {
                    SelectListItem opzioneSingolaSelectMultipla = new SelectListItem() { Text = ingridient.Name , Value = ingridient.Id.ToString() };
                    listaPerLaSelectMultipla.Add(opzioneSingolaSelectMultipla);
                }

                return listaPerLaSelectMultipla;
            }
        }
    }
}
