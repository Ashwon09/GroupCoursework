﻿using GroupCoursework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroupCoursework.Controllers
{
    [Authorize(Roles ="Admin,Staff")]
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

        //Function 3
        public IActionResult GetDateOut(int memberNumber)
        {
            /**
select distinct DT.DVDTitleName,DC.CopyNumber,L.DateOut,DATEDIFF(DAY,L.DateOut,'2022-04-14') aS dIFF from DVDTitles dt
inner join DVDCopys dc 
on dt.DVDNumber = dc.DVDNumber
inner join Loans l
on dc.CopyNumber = l.CopyNumber
inner join CastMembers c
on c.DVDNumber = dc.DVDNumber
inner join Members m
on l.MemberNumber = m.MembershipNumber
where (L.DateOut >= (GETDATE()-31) and m.MembershipNumber = 3);

             DateTime currentDate = DateTime.Now.Date;
            */

            DateTime currentDate = DateTime.Now.Date;
            DateTime lastDate = currentDate.Subtract(new TimeSpan(31, 0, 0, 0, 0));

            var dvdTitle = _dbcontext.DVDTitles.ToList();
            var dvdCopy = _dbcontext.DVDCopys.ToList();
            var castMember = _dbcontext.CastMembers.ToList();
            var member = _dbcontext.Members.ToList();
            var loan = _dbcontext.Loans.ToList();


            var details = from d in dvdTitle
                          join dc in dvdCopy
                          on d.DVDNumber equals dc.DVDNumber into table1
                          from dc in table1.ToList().Distinct().Where(dc => dc.DVDNumber == d.DVDNumber)
                          join l in loan on dc.CopyNumber equals l.CopyNumber into table2
                          from l in table2.ToList().Distinct().Where(l => l.CopyNumber == dc.CopyNumber)
                          join c in castMember
                          on dc.DVDNumber equals c.DVDNumber into table3
                          from c in table3.ToList().Distinct().Where(c => c.DVDNumber == dc.DVDNumber)
                          join m in member
                          on l.MemberNumber equals m.MembershipNumber into table4
                          from m in table4.ToList().Distinct().Where(m => m.MembershipNumber == l.MemberNumber && m.MembershipNumber == memberNumber && DateTime.Parse(l.DateOut) >= lastDate)
                          select new { dvdTitle = d, castMember = c, dvdCopy = dc, loan = l, member = m };

            //var r = _context.Actors.FirstOrDefault();
            //ViewBag.last = r;
            ViewBag.date = details;
            ViewBag.memberNumbers = _dbcontext.Members.ToArray();
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

            return View();
        }

        //last loan data not coming
        //FUNCTION 5
        //https://localhost:7135/Assistant/GetLoanDetails?copynumber=4
        public IActionResult GetLoanDetails(int copynumber)
        {
            /**
                   select dc.CopyNumber,m.MembershipFirstName,dt.DVDTitleName,l.DateOut,l.DateDue,l.DateReturned from Members m
            inner join loans l on l.MemberNumber= m.MembershipNumber
            inner join DVDCopys dc on dc.CopyNumber = l.CopyNumber
            inner join DVDTitles dt on dt.DVDNumber = dc.DVDNumber
            where l.CopyNumber = 2
            */
            var dvdTitle = _dbcontext.DVDTitles.ToList();
            var loan = _dbcontext.Loans.ToList();
            var lastloan = loan.LastOrDefault();
            var member = _dbcontext.Members.ToList();
            var dvdCopy = _dbcontext.DVDCopys.ToList();
            var loanDetails = (from l in loan
                               join m in member on l.MemberNumber equals m.MembershipNumber into table1
                               from m in table1.ToList().Where(m => m.MembershipNumber == l.MemberNumber).ToList()

                               join dc in dvdCopy on l.CopyNumber equals dc.CopyNumber into table2
                               from dc in table2.ToList().Where(dc => dc.CopyNumber == l.CopyNumber && l.CopyNumber == copynumber).ToList()

                               join dt in dvdTitle on dc.DVDNumber equals dt.DVDNumber into table3
                               from dt in table3.ToList().Where(dt => dt.DVDNumber == dc.DVDNumber).ToList()
                               group new { dt, l, m, dc } by new { l.CopyNumber, dt.DvdTitle, m.MembershipFirstName, m.MembershipLastName, m.MemberDOB, m.MembershipAddress, l.DateDue, l.DateOut, l.DateReturned }
                              into grp
                               select new
                               {
                                   grp.Key.CopyNumber,
                                   grp.Key.DvdTitle,
                                   grp.Key.MembershipFirstName,
                                   grp.Key.MembershipLastName,
                                   grp.Key.MemberDOB,
                                   grp.Key.MembershipAddress,
                                   grp.Key.DateDue,
                                   grp.Key.DateOut,
                                   grp.Key.DateReturned,
                               });
            //var r = _context.Actors.FirstOrDefault();
            //ViewBag.last = r;
            ViewBag.loanDetails = loanDetails;
            ViewBag.copyNumber = _dbcontext.DVDCopys.ToArray();
            return View();
        }


        //Function 6 
        public IActionResult AddDVDCopy() {
            var dvdcopy = _dbcontext.DVDCopys.ToList();
            var dvdtitle = _dbcontext.DVDTitles.ToList();

            var members = _dbcontext.Members.ToList();
            
            var loanType = _dbcontext.LoanTypes.ToList();

            ViewBag.member = members;
            ViewBag.loanType = loanType;

            var dvd = from dc in dvdcopy
                      join dt in dvdtitle on dc.DVDNumber equals dt.DVDNumber
                       select new { 
                       dvdtitle = dt,
                       dvdcopy = dc,
                       };
            ViewBag.dvd = dvd;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDVDCopy(Loan Loan, int member, int loantype, int copynumber)
        {
            var dvdcopy = _dbcontext.DVDCopys.ToList();
            var dvdtitle = _dbcontext.DVDTitles.ToList();

            var members = _dbcontext.Members.ToList();

            var loanType = _dbcontext.LoanTypes.ToList();

            ViewBag.member = members;
            ViewBag.loanType = loanType;

            var dvdcd = from dc in dvdcopy
                      join dt in dvdtitle on dc.DVDNumber equals dt.DVDNumber
                      select new
                      {
                          dvdtitle = dt,
                          dvdcopy = dc,
                      };
            ViewBag.dvd = dvdcd;


            var memberInfo = _dbcontext.Members.Where(x => x.MembershipNumber == member).First();
            String dob = memberInfo.MemberDOB;//GETTING MEMBER DOB
            String todaysDate = DateTime.Now.ToShortDateString();

            var today = DateTime.Now.ToShortDateString();


            //CONVERTING IN DATE TIME
            DateTime cDOB = DateTime.Parse(dob);
            DateTime ctodaysDate = DateTime.Parse(todaysDate);

            TimeSpan dayDiff = ctodaysDate.Subtract(cDOB);
            Console.Write(dayDiff.Days.ToString());
            var age = dayDiff.Days / 365;
            Console.Write(age);

            

            var dvd = _dbcontext.DVDTitles.ToList();
            var catogory = _dbcontext.DVDCategorys.ToList();
            // var dvdCopy = _dbcontext.DVDCopys.ToList();
            var dvdCopy = _dbcontext.DVDCopys.Where(x => x.CopyNumber == copynumber).First();
            var dvdInfo = _dbcontext.DVDTitles.Where(x => x.DVDNumber == dvdCopy.DVDNumber).First();

            var agerestriction = dvdInfo.Category.AgeRestricted;

            Loan.MemberNumber = member;
            Loan.LoanTypeNumber = loantype;
            Loan.CopyNumber = copynumber;
            Loan.DateOut = DateTime.Now.ToShortDateString();
            var loantypeinfo = _dbcontext.LoanTypes.Where(x => x.LoanTypeNumber == loantype).First();
          
            Loan.DateDue = DateTime.Now.AddMonths(int.Parse(loantypeinfo.LoanDuration)).ToShortDateString();
            Loan.DateReturned = "0";
            var price = int.Parse(loantypeinfo.LoanDuration) * int.Parse(dvdInfo.StandardCharge);
       
            if (!agerestriction)
            {
                _dbcontext.Loans.Add(Loan);
                await _dbcontext.SaveChangesAsync();
                ViewBag.Price = price;
                return View("AddDVDCopy2");
            }
            if (agerestriction) {
                if (age > 18)
                {
                    _dbcontext.Loans.Add(Loan);
                    await _dbcontext.SaveChangesAsync();
                    ViewBag.Price = price;

                    return View("AddDVDCopy2");

                }
                else {
                return RedirectToAction("AddDVDCopyMessage", "Assistant");
                    //cannot loan the dvd due to age restriction
                }
            }
                return RedirectToAction("AddDVDCopy","Assistant");

        }


        public IActionResult AddDVDCopyMessage()
        {
            var dvdcopy = _dbcontext.DVDCopys.ToList();
            var dvdtitle = _dbcontext.DVDTitles.ToList();

            var members = _dbcontext.Members.ToList();

            var loanType = _dbcontext.LoanTypes.ToList();

            ViewBag.member = members;
            ViewBag.loanType = loanType;

            var dvd = from dc in dvdcopy
                      join dt in dvdtitle on dc.DVDNumber equals dt.DVDNumber
                      select new
                      {
                          dvdtitle = dt,
                          dvdcopy = dc,
                      };
            ViewBag.dvd = dvd;

            return View();
        }


        //FUNCTION 7 SHOWING ALL 
        public IActionResult ListAllLoans()
        {
            /*
            select l.CopyNumber,m.MembershipFirstName,l.DateDue,l.DateReturned from loans l
            inner join members m on l.MemberNumber = m.MembershipNumber
            where l.DateReturned = '0'
             */

            DateTime currentDate = DateTime.Now.Date;
            DateTime lastDate = currentDate.Subtract(new TimeSpan(365, 0, 0, 0, 0));
            String d = "0";
            var loan = _dbcontext.Loans.ToList();
            var member = _dbcontext.Members.ToList();
            var loanDetail = (from l in loan
                              join m in member on l.MemberNumber equals m.MembershipNumber into table1
                              from m in table1.ToList().Where(m => m.MembershipNumber == l.MemberNumber && l.DateReturned == d)
                              orderby l.CopyNumber ascending
                              select new { loan = l, member = m });
            ViewBag.loanDetails = loanDetail;
            return View();
        }


        public IActionResult EditDVDCopyDetails(int LoanNumber)
        {
            //GET LOAN DETAILS OF THE THE CURRENT LOAN THAT SHOULD BE UPDATED
           var details = _dbcontext.Loans.Where(l => l.LoanNumber == LoanNumber).First();
            ViewBag.UserLoanDetails = details;
            var cop = ViewBag.UserLoanDetails;

            //GET DVD OF THE COPY NUMBER
            ViewBag.CopyDVDNumber = _dbcontext.DVDCopys.Where(c => c.CopyNumber == details.CopyNumber).First();
            int copydvdnum = ViewBag.CopyDVDNumber.DVDNumber;

            //GET PENALTY CHARGE OF DVD NUMBER
            ViewBag.DVDNumber = _dbcontext.DVDTitles.Where(d => d.DVDNumber == copydvdnum).First();
            ViewBag.PenaltyCharge = ViewBag.DVDNumber.PenaltyCharge;
            int pCharge = int.Parse(ViewBag.PenaltyCharge);

            //CALCULATING DATE OF RETURN
            DateTime dueDate = DateTime.Parse(ViewBag.UserLoanDetails.DateDue);
            DateTime returnDate = DateTime.Now.Date.Date;

            //GETTING ONLY DATE
            var onlydate = returnDate.ToShortDateString();

            //GETTING DAY DIFFERENCE
            TimeSpan difference = returnDate.Subtract(dueDate);
            int dueDay = difference.Days;

            ViewBag.ReturnDate = onlydate;
            if (dueDay < 0)
            {
                ViewBag.OverDue = "0";
                ViewBag.TotalCharge = "0";
            }
            else
            {
                ViewBag.OverDue = dueDay;
                int totalCharge = dueDay * pCharge;
                ViewBag.TotalCharge = totalCharge;
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecordDVDCopy
       (Loan loan, int LoanNumber, int loantypenumber, int copynumber, int membernumber, string dateOut, string dateReturned, string dateDue)
        {
            loan = _dbcontext.Loans.Where(l => l.LoanNumber == LoanNumber).First();

            loan.DateReturned = dateReturned;
                _dbcontext.Loans.Update(loan);
              await _dbcontext.SaveChangesAsync();
                return RedirectToAction("ListAllLoans");
        }

        //FUNCTION 8 
        // Assistant/GetLoans
        //https://localhost:44344/Assistant/GetTotalLoans
        public IActionResult GetTotalLoans()
        {
            /**
    select distinct m.MembershipFirstName,m.MembershipCategoryNumber,mc.MembershipCategoryTotalLoans,count(l.CopyNumber) as 'Total Loan' 
    from Members m
    inner join loans l on m.MembershipNumber = l.MemberNumber
    inner join MembershipCategorys mc on m.MembershipCategoryNumber = mc.MembershipCategoryNumber
    where l.DateReturned <> '0'
    group by m.MembershipFirstName,m.MembershipCategoryNumber,mc.MembershipCategoryTotalLoans
    ORDER BY M.MembershipFirstName ASC
            */
            String c = "0";
            var member = _dbcontext.Members.ToList();
            var loan = _dbcontext.Loans.ToList();
            var membercat = _dbcontext.MembershipCategorys.ToList();

            var dvd = (from m in member
                       join l in loan on m.MembershipNumber equals l.MemberNumber into table1
                       from l in table1.Distinct().ToList().Where(l => l.MemberNumber == m.MembershipNumber).Distinct().ToList()

                       join mc in membercat on m.MembershipCategoryNumber equals mc.MembershipCategoryNumber into table2
                       from mc in table2.Distinct().ToList().Where(mc => mc.MembershipCategoryNumber == m.MembershipCategoryNumber)
                       group new { l, m, mc } by new { m.MembershipFirstName, m.MembershipCategoryNumber, mc.MembershipCategoryTotalLoans }
                      into grp
                       select new
                       {
                           //
                           grp.Key.MembershipFirstName,
                           grp.Key.MembershipCategoryNumber,
                           grp.Key.MembershipCategoryTotalLoans,
                           TotalLoans = grp.Count(),
                       }).OrderBy(x => x.MembershipFirstName);
            ViewBag.totalloans = dvd;
            return View();
        }

        //FUNCTION 10 PART 1
        //https://localhost:44344/Assistant/GetListOfDVDCopy
        public IActionResult GetListOfDVDCopy(bool copyDeleted = false)
        {
            ViewBag.copyDeleted = copyDeleted;

            /**
          select dc.CopyNumber,dc.DVDNumber,l.DateReturned,dc.DatePurchased from DVDCopys dc
INNER JOIN Loans l
on dc.CopyNumber = l.CopyNumber
where (dc.DatePurchased < (GETDATE()-365) and l.DateReturned <> '0')
          */
            DateTime currentDate = DateTime.Now.Date;
            DateTime lastDate = currentDate.Subtract(new TimeSpan(365, 0, 0, 0, 0));
            String d = "0";
            var loan = _dbcontext.Loans.ToList();
            var dvdCopy = _dbcontext.DVDCopys.ToList();

            var dvd = from dc in dvdCopy
                      join l in loan on dc.CopyNumber equals l.CopyNumber into table1
                      from l in table1.Distinct().Where(l => l.CopyNumber == dc.CopyNumber && l.DateReturned != d && dc.DatePurchased < lastDate)

                      select new { loan = l, dvdCopy = dc };
            ViewBag.dvdList = dvd;
            return View();
        }

        //FUNCTION 10 PART2
        [HttpGet]
        public async Task<IActionResult> DeleteCopy(int copynumber)
        {
            var copy = _dbcontext.DVDCopys.Where(l => l.CopyNumber == copynumber).First();
            _dbcontext.DVDCopys.Remove(copy);
            _dbcontext.SaveChanges();

            return RedirectToAction("GetListOfDVDCopy", new { copyDeleted = true });
        }


        //FOR FUNCTION 11 
        //https://localhost:44344/Assistant/GetDVDCopyListNotLoaned
        public IActionResult GetDVDCopyListNotLoaned()
        {
            /**
            select dt.DVDTitleName,dc.CopyNumber,m.MembershipFirstName,l.DateOut,count(l.DateOut) as "Total Loans" from Members m
            inner join loans l on l.MemberNumber= m.MembershipNumber
            inner join DVDCopys dc on dc.CopyNumber = l.CopyNumber
            inner join DVDTitles dt on dt.DVDNumber = dc.DVDNumber
			where l.DateReturned <> '0'
			group by dt.DVDTitleName,dc.CopyNumber,m.MembershipFirstName,l.DateOut
            
            */
            String c = "0";
            var member = _dbcontext.Members.ToList();
            var loan = _dbcontext.Loans.ToList();
            var dvdTitle = _dbcontext.DVDTitles.ToList();
            var dvdCopy = _dbcontext.DVDCopys.ToList();

            var copyloan = (from l in loan
                            join m in member on l.MemberNumber equals m.MembershipNumber into table1
                            from m in table1.Distinct().ToList().Where(m => m.MembershipNumber == l.MemberNumber).Distinct().ToList()
                            join dc in dvdCopy on l.CopyNumber equals dc.CopyNumber into table2
                            from dc in table2.Distinct().ToList().Where(dc => dc.CopyNumber == l.CopyNumber).Distinct().ToList()
                            join dt in dvdTitle on dc.DVDNumber equals dt.DVDNumber into table3
                            from dt in table3.Distinct().ToList().Where(dt => dt.DVDNumber == dc.DVDNumber && l.DateReturned == c).Distinct().ToList()
                            group new { l, m, dc, dt } by new { dt.DvdTitle, dc.CopyNumber, m.MembershipFirstName, l.DateOut }
                           into grp
                            select new
                            {
                                TotalLoans = grp.Count(),
                                grp.Key.DvdTitle,
                                grp.Key.CopyNumber,
                                grp.Key.MembershipFirstName,
                                grp.Key.DateOut,

                            }).OrderBy(X => X.TotalLoans);
            ViewBag.totalloans = copyloan;
            return View();
        }


        //FUNCTION 12
        //Select DATEDIFF(day, max(l.dateout), GETDATE()) from loans l
        public IActionResult MemberListNotBorrowed()
        {
            //Select l.MemberNumber, max(l.dateout) as "Member Recent Loan", DATEDIFF(day, max(l.dateout), GETDATE()) as "My Date" from loans l where(31 > (Select DATEDIFF(day, max(l.dateout), GETDATE()) as "My Date" from loans l)) group by l.MemberNumber
            /** 
             Select m.MembershipFirstName,m.MembershipLastName,m.MembershipAddress,
            max(l.dateout) as "Date out of recent loan", 
            DATEDIFF(day, max(l.dateout), GETDATE()) as "No of days since the last loan"
            from loans l 
            inner join Members m on l.MemberNumber = m.MembershipNumber
            inner join DVDCopys dc on dc.CopyNumber = l.CopyNumber
            inner join DVDTitles dt on dt.DVDNumber = dc.DVDNumber
            where(31 > (Select DATEDIFF(day, max(l.dateout), GETDATE()) as "My Date" from loans l))
            group by m.MembershipFirstName,m.MembershipLastName,m.MembershipAddress

             */
            var loan = _dbcontext.Loans.ToList();
            var maxDate = from l in loan
                          group l by l.MemberNumber
                          into g
                          select new
                          {
                              MaxDates = (from l in g select l.DateOut).Max(),
                          };
            ViewBag.dates = maxDate.ToList();
            var members = _dbcontext.Members.ToList();
            var dvdCopy = _dbcontext.DVDCopys.ToList();
            var dvdTitle = _dbcontext.DVDTitles.ToList();
            List<int> difference = new List<int>();
            dynamic details = null;
            //foreach (var m in ViewBag.dates)
            //{
            //    DateTime today = DateTime.Now;
            //    var dates = DateTime.Parse(m.MaxDates.ToString());
            //    var diff = (today - dates).Days;
            //    difference.Add(diff);
            //    Console.WriteLine(diff.ToString());
            //    ViewBag.listDiff = difference;
            //}

            foreach (var dd in ViewBag.dates)
            {

                DateTime today = DateTime.Now;
                var dates = DateTime.Parse(dd.MaxDates.ToString());
                var diff = (today - dates).Days;
                var data = (from l in loan
                            join m in members on l.MemberNumber equals m.MembershipNumber
                            join dc in dvdCopy on l.CopyNumber equals dc.CopyNumber
                            join dt in dvdTitle on dc.DVDNumber equals dt.DVDNumber
                            where (31 > diff)
                            group new { l, m, dc, dt } by new { m.MembershipFirstName, m.MembershipLastName, m.MembershipAddress, ViewBag.dates }
                            into grp
                            select new
                            {
                                grp.Key.MembershipLastName,
                                grp.Key.MembershipFirstName,
                                grp.Key.MembershipAddress,
                                MaxDates = (from l in grp select l.l.DateOut).Max(),
                                Difference = diff
                            }).OrderBy(x => x.MembershipFirstName);
                ViewBag.details = data;
                return View();
            }
            return View();
        }

        //FOR FUNCTION 13
        //https://localhost:44344/Assistant/GetDVDofNoLoan
        public IActionResult GetDVDofNoLoan()
        {
            /**
        select distinct dt.DVDTitleName  from DVDTitles dt
        inner join DVDCopys dc on dt.DVDNumber = dc.DVDNumber
        inner join loans l on dc.CopyNumber = l.CopyNumber
        where (l.DateReturned = '0' and L.DateOut >= (GETDATE()-31))
            */
            DateTime currentDate = DateTime.Now.Date;
            DateTime lastDate = currentDate.Subtract(new TimeSpan(31, 0, 0, 0, 0));
            String d = "0";
            var dvdTitle = _dbcontext.DVDTitles.ToList();
            var loan = _dbcontext.Loans.ToList();
            var dvdCopy = _dbcontext.DVDCopys.ToList();

            var dvd = from dt in dvdTitle
                      join dc in dvdCopy on dt.DVDNumber equals dc.DVDNumber into table1
                      from dc in table1
                      join l in loan on dc.CopyNumber equals l.CopyNumber into table2
                      from l in table2.Distinct().ToList().Where(l => l.CopyNumber == dc.CopyNumber && l.DateReturned == d && DateTime.Parse(l.DateOut) >= lastDate).Distinct().ToList()

                      select new { dvdTitle = dt, loan = l, dvdCopy = dc };
            //var r = _context.Actors.FirstOrDefault();
            //ViewBag.last = r;
            ViewBag.dvd = dvd;
            return View();
        }
    }
}
