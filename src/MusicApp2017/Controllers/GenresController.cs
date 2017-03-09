using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicApp2017.Models;

namespace MusicApp2017.Controllers
{
    public class GenresController : Controller
    {
        private readonly MusicDbContext _context;
        //Db access context
        public GenresController(MusicDbContext context)
        {
            _context = context;
        }
        //Index view
        public IActionResult Index()
        {

            return View(_context.Genres.ToList());
        }
        //Create view
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind("Name,GenreID")] Genre genre)
        {
            var dbgenre = _context.Genres.FirstOrDefault(a => a.Name.ToLower() == genre.Name.ToLower());
            if (dbgenre == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Genres.Add(genre);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            else { ViewBag.Error = "Duplicate Entry Detected!"; }
            return View(genre);
        }
        //Details view
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var album = _context.Albums.Include(a => a.Artist).Include(a => a.Genre).Where(a => a.GenreID == id);

            if (album == null)
            {
                return NotFound();
            }
            ViewBag.NameList = _context.Artists.Where(a => a.ArtistID == id);
            ViewBag.BioList = _context.Artists.Where(a => a.ArtistID == id);
            return View(album.ToList());

        }

        //Delete view
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Genre genre = _context.Genres.FirstOrDefault(i => i.GenreID == id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(i => i.GenreID == id);
            _context.Genres.Remove(genre);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //edit view
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Genre genre = _context.Genres.First(i => i.GenreID == id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("GenreID,Name")] Genre genre)
        {
            //var dbgenre = _context.Genres.FirstOrDefault(a => a.Name.ToLower() == genre.Name.ToLower());
            //if (dbgenre == null)
            //{
            if (ModelState.IsValid)
            {
                _context.Entry(genre).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            //}
            //else { ViewBag.Error = "Duplicate Entry Detected!"; }
            return View(genre);
        }
    }
}