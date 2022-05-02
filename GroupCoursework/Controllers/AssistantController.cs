using GroupCoursework.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroupCoursework.Controllers
{
    public class AssistantController : Controller
    {
        private readonly DatabaseContext _dbcontext;

        public AssistantController(DatabaseContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }

        //FUNCTION 4
        public IActionResult GetList()
        {
            /**
        select dt.DVDTitleName,dt.DateReleased,p.ProducerName,s.StudioName,c.CastMemberNo,a.ActorFirstName,a.ActorSurname
            from DVDTitles dt
inner join CastMembers c on c.DVDNumber = dt.DVDNumber
inner join Producers p on p.ProducerNumber = dt.ProducerNumber
inner join Studios s on s.StudioNumber = dt.StudioNumber
inner join Actors a on a.ActorNumber = c.ActorNumber
order by  dt.DateReleased asc,a.ActorSurname asc
            */
            var dvdTitle = _dbcontext.DVDTitles.ToList();
            var producer = _dbcontext.Producers.ToList();
            var castMember = _dbcontext.CastMembers.ToList();
            var studio = _dbcontext.Studios.ToList();
            var actor = _dbcontext.Actors.ToList();
            var listProducer = from dt in dvdTitle
                               join c in castMember on dt.DVDNumber equals c.DVDNumber into table1
                               from c in table1.ToList().Where(c => c.DVDNumber == dt.DVDNumber).ToList()

                               join p in producer on dt.ProducerNumber equals p.ProducerNumber into table2
                               from p in table2.ToList().Where(p => p.ProducerNumber == dt.ProducerNumber).ToList()

                               join s in studio on dt.StudioNumber equals s.StudioNumber into table3
                               from s in table3.ToList().Where(s => s.StudioNumber == dt.StudioNumber).ToList()

                               join a in actor on c.ActorNumber equals a.ActorNumber into table4
                               from a in table4.ToList().Where(a => a.ActorNumber == c.ActorNumber).ToList()
                               orderby dt.DateReleased ascending, a.ActorSurname ascending
                               select new { dvdTitle = dt, castMember = c, actorDetails = a, studio = s, producer = p };
            //var r = _context.Actors.FirstOrDefault();
            //ViewBag.last = r;
            ViewBag.listProducer = listProducer;

            return View(listProducer);
        }




    }
}
