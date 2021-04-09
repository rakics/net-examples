using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SafeAdmin.Model;
using Microsoft.EntityFrameworkCore;

namespace SafeAdmin.Data
{
    public class SafeContext : DbContext
    {
        public SafeContext() : base()
        {
        }
        public SafeContext(DbContextOptions<SafeContext> options) : base(options)
        {
        }

        public DbSet<Member> Member { get; set; }
        public DbSet<Facility> Facility { get; set; }
        public DbSet<MailTemplate> MailTemplate { get; set; }
        public DbSet<SignUp> SignUp { get; set; }
        public DbSet<TokenLoginData> TokenLoginData { get; set; }
        public DbSet<Sex> Sex { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<MaritalStatus> MaritalStatus { get; set; }
        public DbSet<Ethnicity> Ethnicity { get; set; }
        public DbSet<RaceOption> RaceOption { get; set; }
        public DbSet<Race> Race { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<PayerType> PayerType { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<City> City { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().ToTable("Member");
            modelBuilder.Entity<MailTemplate>().ToTable("MailTemplate");
            modelBuilder.Entity<SignUp>().ToTable("SignUp");
            modelBuilder.Entity<TokenLoginData>().ToTable("TokenLoginData");
            modelBuilder.Entity<Sex>().ToTable("Sex");
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<MaritalStatus>().ToTable("MaritalStatus");
            modelBuilder.Entity<Ethnicity>().ToTable("Ethnicity");
            modelBuilder.Entity<RaceOption>().ToTable("RaceOption");
            modelBuilder.Entity<Race>().ToTable("Race");
            modelBuilder.Entity<Ethnicity>().ToTable("Ethnicity");
            modelBuilder.Entity<Language>().ToTable("Language");
            modelBuilder.Entity<PayerType>().ToTable("PayerType");
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Result>().ToTable("Result");
            modelBuilder.Entity<Test>().ToTable("Test");
            modelBuilder.Entity<Facility>().ToTable("Facility");
            modelBuilder.Entity<City>().ToTable("City");
            //modelBuilder.Entity<Facility>().Property(f => f.Lat).HasPrecision(9, 6);
            //modelBuilder.Entity<Facility>().Property(f => f.Long).HasPrecision(9, 6);
        }

        
    }
}
