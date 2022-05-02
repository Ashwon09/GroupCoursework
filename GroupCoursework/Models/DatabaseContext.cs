using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GroupCoursework.Models
{
    public class DatabaseContext : DbContext

    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<CastMember> CastMembers { get; set; }
        public DbSet<DVDCategory> DVDCategorys { get; set; }
        public DbSet<DVDCopy> DVDCopys { get; set; }
        public DbSet<DVDTitle> DVDTitles { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipCategory> MembershipCategorys { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<User> Users { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CastMember>().HasKey(table => new
            {
                table.DVDNumber,
                table.ActorNumber
            });
        }
    }

}
