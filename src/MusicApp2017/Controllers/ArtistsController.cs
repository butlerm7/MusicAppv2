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
    public class ArtistsController : Controller
    {
        private readonly MusicDbContext _context;
        //Db access context
        public ArtistsController(MusicDbContext context)
        {
            _context = context;
        }
        //Index view
        public IActionResult Index()
        {
    
            return View(_context.Artists.ToList());
        }
        //Create view
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind("Name,Bio,ArtistID")] Artist artist)
        {
            var dbartist = _context.Artists.FirstOrDefault(a => a.Name.ToLower() == artist.Name.ToLower());
            if (dbartist == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Artists.Add(artist);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            else { ViewBag.Error = "Duplicate Entry Detected!"; }
            return View(artist);
        }
        //Details view
        public ActionResult Details(int? id)
        {
            {
                if (id == null)
                {
                    return NotFound();
                }
                var album = _context.Albums.Include(a => a.Artist).Include(a => a.Genre).Where(a => a.ArtistID == id);

                if (album == null)
                {
                    return NotFound();
                }
                ViewBag.NameList = _context.Artists.Where(a => a.ArtistID == id);
                ViewBag.BioList = _context.Artists.Where(a => a.ArtistID == id);
                return View(album.ToList());
                


            }

        }

        //Delete view
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Artist artist = _context.Artists.FirstOrDefault(i => i.ArtistID == id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = _context.Artists.FirstOrDefault(i => i.ArtistID == id);
            _context.Artists.Remove(artist);
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
            Artist artist = _context.Artists.FirstOrDefault(i => i.ArtistID == id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ArtistID,Name,Bio")] Artist artist)
        {
            //var dbartist = _context.Genres.FirstOrDefault(a => a.Name.ToLower() == artist.Name.ToLower());
            //if (dbartist == null)
            //{
                if (ModelState.IsValid)
                {
                    _context.Entry(artist).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            //}
            //else { ViewBag.Error = "Duplicate Entry Detected!"; }
            return View(artist);
        }
    }
}