using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

namespace ErrorLoggerDatabaseModel //.Initializers
{
    public class ApplicationInitializer : DropCreateDatabaseIfModelChanges<DatabaseModelFromDb>
    {
        /// <summary>
        /// Seeds data into the DB
        /// </summary>
        protected override void Seed(DatabaseModelFromDb context)
        {
            Console.WriteLine(" ### Seeding for the 2nd code first stuff ###");

            User user1 = new User() { UserId = 101, FirstName = "Amritbani", LastName = "Sondhi", Email = "asondhi@syr.edu", Password = "amrit", Role = "admin", UserStatus = "1", LastLoginDate = "Mar 23 2017" }; //check how to 
            User user2 = new User() { UserId = 102, FirstName = "Santhosh", LastName = "Thomas", Email = "sthomas@syr.edu", Password = "santhosh", Role = "user", UserStatus = "1", LastLoginDate = "Mar 23 2017" };
            User user3 = new User { UserId = 103, FirstName = "Joy", LastName = "Shalom", Email = "jshalom@syr.edu", Password = "shalom", Role = "user", UserStatus = "1", LastLoginDate = "Mar 23 2017" };

            Application notepadApp = new Application()
            {
                ApplicationId = 1,
                ApplicationName = "Note Pad",
                ApplicationType = "Text",
                ApplicationStatus = "1",
                Users = new List<User>() { user1, user3 }
            };

            Application calculatorApp = new Application()
            {
                ApplicationId = 1,
                ApplicationName = "Calculator",
                ApplicationType = "Arithmetic",
                ApplicationStatus = "1",
                Users = new List<User>() { user2 }
            };

            // The order is important, since we are setting up references
            context.Users.Add(user1);
            context.Users.Add(user2);
            context.Users.Add(user3);
            context.Applications.Add(notepadApp);
            context.Applications.Add(calculatorApp);

            // letting the base method do anything it needs to get done
            base.Seed(context);

            // Save the changes you made, when adding the data above
            context.SaveChanges();

        }
    }
}
