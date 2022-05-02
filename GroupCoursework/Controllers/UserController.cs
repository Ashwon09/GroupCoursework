using GroupCoursework.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroupCoursework.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // https://localhost:7135/User/GetActorDetails?name=Holland
        // GET: NormalUserController/Details/5
        //FUNCTION 1
        public IActionResult GetActorDetails(String name)
        {

            var dvdTitle = _context.DVDTitles.ToList();
            var castMember = _context.CastMembers.ToList();
            var actorDetails = _context.Actors.ToList();
            var details =
                from d in dvdTitle
                join c in castMember
                on d.DVDNumber equals c.DVDNumber into table1
                from c in table1.Distinct().ToList().Where(c => c.DVDNumber == d.DVDNumber)
                join a in actorDetails on c.ActorNumber equals a.ActorNumber into table2
                from a in table2.Distinct().ToList().Where(a => a.ActorNumber == c.ActorNumber && a.ActorSurname == name)
                select new { dvdTitle = d, castMember = c, actorDetails = a };
            var r = _context.Actors.FirstOrDefault();
            ViewBag.last = r;
            ViewBag.name = details;
            ViewBag.Actors = actorDetails;
            return View(details);
        }
    }
}
