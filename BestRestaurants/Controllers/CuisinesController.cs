using Microsoft.AspNetCore.Mvc;
using BestRestaurants.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BestRestaurants.Controllers
{
  public class CuisinesController : Controller
  {
    private readonly BestRestaurantsContext _db;

    public CuisinesController(BestRestaurantsContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Cuisine> model = _db.Cuisines.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Cuisine cuisine)
    {
      _db.Cuisines.Add(cuisine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Cuisine thisCuisine = _db.Cuisines.FirstOrDefault(cuisines => cuisines.CuisineId == id);
      return View(thisCuisine);
    }

    public ActionResult Edit(int id)
    {
      var thisCuisine = _db.Cuisines.FirstOrDefault(cuisines => cuisines.CuisineId == id);
      return View(thisCuisine);
    }

    [HttpPost]
    public ActionResult Edit(Cuisine cuisine)
    {
      _db.Entry(cuisine).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisCuisine = _db.Cuisines.FirstOrDefault(cuisines => cuisines.CuisineId == id);
      return View(thisCuisine);
    }

    [HttpPost, ActionName("Delete")] // so we can utilize the "Delete" action still.
    public ActionResult DeleteConfirmed(int id) // different name because GET & POST have same signature (method name and parameters)
    {
      var thisCuisine = _db.Cuisines.FirstOrDefault(cuisines => cuisines.CuisineId == id);
      _db.Cuisines.Remove(thisCuisine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpGet("/search")]
    public ActionResult Search(string search)
    {
      List<Cuisine> model = _db.Cuisines.Include(cuisines => cuisines.Restaurants).ToList();

      Cuisine match = new Cuisine();

      if (!string.IsNullOrEmpty(search))
      {
       foreach(Cuisine cuisine in model)
       {
         if (cuisine.Name == search)
         {
           match = cuisine;
         }
       } 
      }
      List<Restaurant> matches = match.Restaurants.ToList();
      return View(matches);
    }
  }
}