using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SafeAdmin.Model;

namespace SafeAdmin.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SafeContext context)
        {
            context.Database.EnsureCreated();
            
            // Look for any member.
            if (!context.Member.Any())
            {
                var members = new Member[]
                {
                new Member{ID=1, FirstName="Ken",LastName="Mayer",Email="",MobileNumber="", State="",
                    Zip = "55555", BirthDate =DateTime.Parse("2002-09-01"), Sex = "Male" , IsActive = true },
                new Member{ID=2, FirstName="Bob",LastName="Hall",Email="",MobileNumber="", State="",
                    Zip = "55555", BirthDate =DateTime.Parse("2004-09-01"), Sex = "Male" , IsActive = true },
                new Member{ID=3, FirstName="Djuro",LastName="Alfirevic",Email="",MobileNumber="", State="",
                    Zip = "55555", BirthDate =DateTime.Parse("2007-09-01"), Sex = "Male" , IsActive = true },
                new Member{ID=4, FirstName="Ana",LastName="Popovic",Email="",MobileNumber="", State="",
                    Zip = "55555", BirthDate =DateTime.Parse("1994-09-01"), Sex = "Female" , IsActive = true },
                new Member{ID=5, FirstName="Mika",LastName="Petrovic",Email="",MobileNumber="", State="",
                    Zip = "55555", BirthDate =DateTime.Parse("1954-09-01"), Sex = "Other" , IsActive = true }
                };
                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Member] ON");

                    foreach (Member s in members)
                    {
                        context.Member.Add(s);
                    }

                    context.SaveChanges();

                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Member] OFF");

                    transaction.Commit();
                }
            }

            if (!context.MailTemplate.Any())
            {
                var mailTemplats = new MailTemplate[]
                {
                    new MailTemplate {TemplateName = "SignUp", Subject="Safe SignUp Confirmation", Body="You or somebody has requested to join our beta, Click <a href='$LINK$'>Here</a> if it was you , otherwise ignore this email" }
                };

                foreach (MailTemplate m in mailTemplats)
                {
                    context.MailTemplate.Add(m);
                }
                context.SaveChanges();
            }

            if (!context.Sex.Any())
            {
                var sexs = new Sex[]
                {
                    new Sex {Value = "Male" },
                    new Sex {Value = "Female" },
                    new Sex {Value = "Other" }
                };
                foreach (Sex s in sexs)
                {
                    context.Sex.Add(s);
                }
                context.SaveChanges();
            }

           
            
            
        }
    }
}