using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ErrorLoggerDatabaseModel;

namespace DatabaseConsole //.DataHandler
{
    public class DatabaseHandler
    {
        public static void CreateDB()
        {
            Console.WriteLine("~~~~ Creating the DB ~~~~");
            Console.WriteLine();

            using (DatabaseModelFromDb db = new DatabaseModelFromDb())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                // Seeding runs the first time you try to use the DB, so we make it seed here..
                // It only runs IF the initializer condition is met, regardless of the True/False above
                db.Applications.Count();
            }
        }

        /// <summary>
        /// Deletes the DB
        /// </summary>
        public static void DeleteDB()
        {
            Console.WriteLine("~~~~ Deleting the DB ~~~~");
            Console.WriteLine();

            using (DatabaseModelFromDb db = new DatabaseModelFromDb())
            {
                if (db.Database.Exists())
                {
                    db.Database.Delete();
                }
            }
        }

        /// <summary>
        /// Used for formatting output
        /// </summary>
        public static string SPACE = "   ";

        /// <summary>
        /// Gets the data grouped by Courses
        /// </summary>
        public static void GetDataByApplications()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Data in the DB (by applications): ~~~~");
            Console.WriteLine();

            // the using statement will make sure the object is disposed when it goes out of scope
            using (DatabaseModelFromDb context = new DatabaseModelFromDb())
            {
                foreach (Application app in context.Applications.ToList())
                {
                    Console.WriteLine(String.Format(SPACE + "Application Id: {0}, Application Name: {1}, Application Status: {2}, Application Type: {3}",
                        app.ApplicationId, app.ApplicationName, app.ApplicationStatus, app.ApplicationType));

                    foreach (User user in app.Users)
                    {
                        Console.WriteLine(String.Format(SPACE + SPACE + "User Id: {0}, Name: {1}",
                            user.UserId, user.FirstName + " " + user.LastName));
                        //(student.MiddleName ?? "<no middle name>") + " " +

                    }
                }
            }
        }

        /// <summary>
        /// Gets the data grouped by Students
        /// </summary>
        public static void GetDataByUsers()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Data in the DB (by users): ~~~~");
            Console.WriteLine();

            // the using statement will make sure the object is disposed when it goes out of scope
            using (DatabaseModelFromDb context = new DatabaseModelFromDb())
            {
                foreach (User user in context.Users.ToList())
                {
                    Console.WriteLine(String.Format(SPACE + "Student Id: {0}, Name: {1}, Role: {2}, Last Login Date: {3}",
                        user.UserId, user.FirstName + " " + user.LastName, user.Role, user.LastLoginDate));
                    //(student.MiddleName ?? "<no middle name>") + " " +

                    foreach (Application app in user.Applications)
                    {
                        Console.WriteLine(String.Format(SPACE + SPACE + "Application Id: {0}, Application Name: {1}",
                            app.ApplicationId, app.ApplicationName));
                    }
                }
            }
        }

        /// <summary>
        /// Inserts a dummy student & repulls the data
        /// </summary>
        public static void InsertADummyUser()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Inserting a dummy User ~~~~");

            // the using statement will make sure the object is disposed when it goes out of scope
            using (DatabaseModelFromDb context = new DatabaseModelFromDb())
            {
                User dummy = new User()
                {
                    FirstName = "Dummy",
                    LastName = "User"
                };

                context.Users.Add(dummy);
                context.SaveChanges();
            }

            GetDataByUsers();
        }

        /// <summary>
        /// Deletes the dummy student & repulls the data
        /// </summary>
        public static void DeleteADummyUser()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Deleting a dummy User ~~~~");

            // the using statement will make sure the object is disposed when it goes out of scope
            using (DatabaseModelFromDb context = new DatabaseModelFromDb())
            {
                User dummy = context.Users.First(x => x.FirstName == "Dummy");

                dummy.Applications = new List<Application>();
                dummy.Applications.Add(context.Applications.First(x => x.ApplicationName == "Note Pad"));

                context.Users.Remove(dummy);
                context.SaveChanges();
            }
            GetDataByUsers();
        }

    }
}
