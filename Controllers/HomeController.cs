using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDilicious.Models;

namespace CRUDilicious.Controllers;

public class HomeController : Controller
{
    private DishContext _context;
    public HomeController(DishContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Home()
    {
        ViewBag.AllDishes = _context.Dishes.ToList();
        return View("Home");
    }

    [HttpGet("new")]
    public IActionResult Create()
    {

        return View("DishForm");
    }

    [HttpPost("newdish")]
    public IActionResult NewDish(Dish newDish)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newDish);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }
        return View("DishForm");
    }

    [HttpPost("update/{id}")]
    public IActionResult UpdateDish(int id, Dish editDish)
    {
        if (ModelState.IsValid)
        {
            Dish oldDish = _context.Dishes.FirstOrDefault(dish => dish.DishId == id);
            oldDish.UpdatedAt = DateTime.Now;
            oldDish.Name = editDish.Name;
            oldDish.Chef = editDish.Chef;
            oldDish.Description = editDish.Description;
            oldDish.Tastiness = editDish.Tastiness;
            _context.SaveChanges();
            return RedirectToAction("Home");
        }
        return View("DishForm", editDish);
    }

    [HttpGet("/delete/{id}")]
    public IActionResult DeleteDish(int id){
        Dish deleteDish = _context.Dishes.FirstOrDefault(dish => dish.DishId == id);
        _context.Dishes.Remove(deleteDish);
        _context.SaveChanges();
        return RedirectToAction("Home");
    }


    [HttpGet("edit/{id}")]
    public IActionResult EditDish(int id)
    {
        Dish OneDish = _context.Dishes.FirstOrDefault(dish => dish.DishId == id);
        if (OneDish == null)
        {
            return RedirectToAction("Home");
        }
        return View("DishForm", OneDish);
    }

    [HttpGet("{id}")]
    public IActionResult SingleDish(int id, Dish editDish)
    {
        ViewBag.OneDish = _context.Dishes.FirstOrDefault(dish => dish.DishId == id);
        if (ViewBag.OneDish == null)
        {
            return RedirectToAction("Home");
        }
        return View("SingleDish");
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
