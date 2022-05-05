using GroupCoursework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroupCoursework.Controllers
{
    public class DVDTitleController : Controller
    {
        private readonly DatabaseContext _context;

        public DVDTitleController(DatabaseContext context)
        {
            _context = context;
        }
        // GET: DVDTitleController
        public async Task<IActionResult> Index()
        {
            return View(await _context.DVDTitles.ToListAsync());
        }

        // GET: DVDTitleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        //function 9 Part 1
        // GET: DVDTitleController/Create
        public IActionResult Create()
        {
            var Studio = _context.Studios.ToList();
            var Producer = _context.Producers.ToList();
            var Category = _context.DVDCategorys.ToList();
            var Actors = _context.Actors.ToList();
            ViewBag.Studio = Studio;
            ViewBag.Producer = Producer;  
            ViewBag.Category = Category;
            ViewBag.Actors = Actors;
            return View();
        }
        //function 9 Part 2
        // POST: DVDTitleController/Create
        [HttpPost]
        public async Task<IActionResult> Create(DVDTitle DVD, CastMember cm, String DVDname, int producer, int category, int studio, DateTime DateReleased, string StandardCharge, string PenaltyCharge, int[] actors)
        {
            DVD.DvdTitle = DVDname;
            DVD.ProducerNumber = producer;
            DVD.CategoryNumber = category;
            DVD.StudioNumber = studio;
            DVD.DateReleased = DateReleased;
            DVD.StandardCharge = StandardCharge;
            DVD.PenaltyCharge = PenaltyCharge;
            var Studio_Details = _context.Studios.ToList();
            DVD.Studio = Studio_Details.Where(o => o.StudioNumber == studio).First();
            var Producer_Details = _context.Producers.ToList();
            DVD.Producer = Producer_Details.Where(_o => _o.ProducerNumber == producer).First();
            var DVDCategory_Details = _context.DVDCategorys.ToList();
            DVD.Category = DVDCategory_Details.Where(p => p.CategoryNumber == category).First();


            int[] actornumbers = actors.Distinct().ToArray();
            try
            {
                _context.DVDTitles.Add(DVD);
                await _context.SaveChangesAsync();
                var DVD_Details = _context.DVDTitles.ToList();
                var DVD_Detail = DVD_Details.Last();

                foreach (var actor in actornumbers) { 
                    cm.ActorNumber = actor;
                    cm.DVDNumber = DVD_Detail.DVDNumber;
                    cm.DVDTitle = DVD_Detail;
                    var Actor_Details = _context.Actors.ToList();
                    cm.Actor = Actor_Details.Where(o => o.ActorNumber == actor).First();

                    _context.CastMembers.Add(cm);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("", new { Issuccess = true });
            }
            catch
            {
                return View();
            }
        }

        // GET: DVDTitleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DVDTitleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DVDTitleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DVDTitleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
