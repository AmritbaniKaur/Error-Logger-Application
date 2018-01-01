/* This will have all the files related to
 * Login and Registering the user
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Common;

namespace ErrorLogger_Amrit.Controllers
{
    using DatabaseEntityModel;
    using LoaderAndLogic;
    using System.Web.Mvc;

    public class AccountController : Controller // BaseController
    {
        private DatabaseModelFromDb context = new DatabaseModelFromDb();

        public ActionResult AdminPage()
        {
            return View();
        }

        public ActionResult ViewUsers()
        {
            UserDataHandler dataSource = new UserDataHandler();
            var data = dataSource.GetAllUsers();
            return View(data);
        }

        public ActionResult ViewDetails(int id)
        {

            UserDataHandler dataSource = new UserDataHandler();
            UserModel data = dataSource.GetUserById(id);

            UserAppsModelView mv = new UserAppsModelView(data); ;

            return View(mv);
        }

        // to show the initial data
        public ActionResult EditDetails(int id)
        {
            UserDataHandler dataSource = new UserDataHandler();
            UserModel data = dataSource.GetUserById(id);

            UserAppsModelView mv = new UserAppsModelView(data); ;

            return View(mv);
        }

        // to edit the data
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditDetails(UserAppsModelView modifiedUser)
        {
            UserDataHandler dataSource = new UserDataHandler();

            // you have to check ModelState in every method
            // cause even if a single parameter is not there, it will result in an exception
            if (ModelState.IsValid)
            {
                dataSource.EditUser(modifiedUser);

                // Amrit - added Webconstants
                return RedirectToAction(WebConstants.ACCOUNT_VIEW_USER_DETAILS, new { id = modifiedUser._userId });
            }

            return View(modifiedUser);
        }

        public ActionResult AddUser()
        {
            return View();
        }

        /// <summary>
        /// Handles the Post action of an Add
        /// </summary>
        /// <param name="newUser">new User</param>
        /// <returns>The correct view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddUser(UserAppsModelView newUser)
        {
            if (ModelState.IsValid)
            {
                UserDataHandler dataSource = new UserDataHandler();

                dataSource.AddUser(newUser);

                return RedirectToAction(WebConstants.HOME_PAGE);
            }
            return View(newUser);
        }

        public ActionResult LoginUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult LoginUser(UserAppsModelView userToAuth) // should receive userName and Password from the fields
        {
            if (ModelState.IsValid)
            {
                UserDataHandler dataSource = new UserDataHandler();
                //UserModel data = dataSource.GetUserById(userToAuth._userId);
                //UserAppsModelView mvOfUserToAuth = new UserAppsModelView(data);

                Session["LoggedIn"] = false;

                //string userName = userToAuth._firstName;
                string email = userToAuth._email;
                string nonHashedPassword = userToAuth._password;

                // check if the user is valid or not
                if (dataSource.AuthenticateUser(email, nonHashedPassword))
                {
                    Session["UserId"] = dataSource.getUserId(email);
                    Session["Role"] = userToAuth._role;

                    Session["LoggedIn"] = true;

                    UserModel data = null;
                    List<UserModel> savedUserInfo = dataSource.GetAllUsers(); // Here, it will be loaded with the complete DB
                    // we have to match exactly 1 user item
                    if (savedUserInfo.Count(x => x.Email == email) == 1)
                    {
                        data = savedUserInfo.Single(x => x.Email == email);
                        //context.Users.Single(x => x.UserId == id);
                        
                    }
                    //UserModel data = dataSource.GetUserById(userToAuth._userId); // cannot use this as id is null
                    UserAppsModelView mv = new UserAppsModelView(data);

                    // change the login time to Now


                    // if role is user / admin
                    if (mv._role == "admin") // if admin, redirect to AdminPage
                    {
                        mv._lastLoginDate = DateTime.Now.ToString();
                        return RedirectToAction(WebConstants.ACCOUNT_ADMIN_PAGE, mv._userId);
                    }
                    // if user, redirect to UserPage
                    else if (mv._role == "user")
                    {
                        mv._lastLoginDate = DateTime.Now.ToString();
                        return RedirectToAction(WebConstants.ACCOUNT_USER_PAGE, "User"/*, mv._userId*/);
                    }
                    else
                    {
                        ViewBag.Message = "Invalid User";
                        return RedirectToAction(WebConstants.ERROR_PAGE);
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid User";
                    //return RedirectToAction(WebConstants.ERROR_PAGE);
                }
            }
            return View(userToAuth);
        }

        // to assign Application to the user
        public ActionResult AssignApps(/*int id*/)
        {
            UserDataHandler dataSource = new UserDataHandler();
            List<UserModel> data = dataSource.GetAllUsers();

            Dictionary<string, string> users = new Dictionary<string, string>();

            foreach (var item in data)
            {
                users.Add(item.Email, item.FirstName);
            }

            var model = new selectModelView
            {
                //values = new SelectList(data, "Key", "Value")
                values = new SelectList(users, "Key", "Value")
            };


            //List<UserModel> data = dataSource.GetAllUsers();
            //List<string> usernames = new List<string>();

            //foreach (var item in data)
            //{
            //    usernames.Add(item.FirstName);
            //}
            //AppErrorsModelView usernameMV = new AppErrorsModelView();
            ///*usernameMV._appUsers*/
            //SelectList s1= new SelectList(usernames);

            //return View(s1);

            return View(model);
        }

        // to assign Application to the user
        [HttpPost]
        [ValidateAntiForgeryToken()]
        //public ActionResult AssignApps(int id) // returns User Id
        //public ActionResult AssignApps(UserModel user)
        public ActionResult AssignApps(selectModelView user) // returns User Id
        {
            //userEmail = ViewBag.Users; //= years;
            string userEmail = user.SelectedValue;
            //string userEmail = user.Email;
            if (ModelState.IsValid)
            {
                //var email = ModelState.Values.ToList();

                UserDataHandler dataSource = new UserDataHandler();
                int userId = dataSource.getUserId(userEmail);

                UserModel data = dataSource.GetUserById(userId);
                UserAppsModelView mv = new UserAppsModelView(data);

                TempData["assignUser"] = userId;

                return RedirectToAction("SelectApps", mv._userId);

                //UserDataHandler dataSource = new UserDataHandler();
                //UserModel data = dataSource.GetUserById(id);

                //UserAppsModelView mv = new UserAppsModelView(data); ;

                //return View(mv);
            }
            else
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        public ActionResult SelectApps(/*int id*/)
        {
            ApplicationDataHandler dataSource = new ApplicationDataHandler();
            List<ApplicationModel> data = dataSource.GetAllApplications();

            Dictionary<string, string> apps = new Dictionary<string, string>();

            foreach (var item in data)
            {
                apps.Add(item.ApplicationId.ToString(), item.ApplicationName);
            }

            var model = new selectModelView
            {
                //values = new SelectList(data, "Key", "Value")
                values = new SelectList(apps, "Key", "Value")
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SelectApps(selectModelView app) // returns Application Id
        {
            int appId = Convert.ToInt32(app.SelectedValue);

            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(TempData["assignUser"]);

                UserDataHandler dataSource = new UserDataHandler();
                UserModel data = dataSource.GetUserById(userId); // needs user Id

                ApplicationDataHandler appDataSource = new ApplicationDataHandler();
                ApplicationModel appData = appDataSource.GetApplicationById(appId);

                UserAppsModelView mv = new UserAppsModelView(data);
                mv.allApplications.Add(appData);

                bool status = dataSource.AssignApplicationToUser(mv);
                if (status)
                {
                    return RedirectToAction("ViewUsers", "Account");
                }
                else
                {
                    return RedirectToAction("Error", "Shared");
                }
                //UserDataHandler dataSource = new UserDataHandler();
                //UserModel data = dataSource.GetUserById(id);

                //UserAppsModelView mv = new UserAppsModelView(data); ;

                //return View(mv);
            }
            else
            {
                return RedirectToAction("Error", "Shared");
            }
        }


    }

    public class selectModelView
    {
        public string SelectedValue { get; set; }
        public SelectList values { get; set; }
    }

}

