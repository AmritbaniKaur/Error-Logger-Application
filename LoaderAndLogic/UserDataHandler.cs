using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security.Cryptography;

using DatabaseEntityModel;
using Common;

namespace LoaderAndLogic
{
    public class UserDataHandler
    {
        private DatabaseModelFromDb context = new DatabaseModelFromDb();

        /// <summary>
        /// This is a SALT.. it should be "random" and complex, 
        /// while it should not change over time.
        /// 
        /// Best practice is to generate a new salt for each user; 
        /// in our case we do so by combidning the username w/ the salt, 
        /// and w/ the password itself
        /// 
        /// This salt, as well as the logic in the HashPassword method 
        /// should not be made public
        /// </summary>
        private const string SALT = "This.4i35i34890099+IsA Rand0m..<<>>//.,{)_@*$()&%&394802 98S alt390 @&$((*@()\":KFHDDF908r40932i;;efdjsklhfsd8oroy389ryhfsklddnfds218()*$()@^$*()@*YF";

        public UserDataHandler()
        {
        }

        // All user details should go from here
        public List<UserModel> GetAllUsers()
        {
            List<UserModel> users = context.Users.ToList();
            return users;
        }

        public UserModel GetUserById(int id)
        {
            UserModel user = null;
            if (context.Users.Any(x => x.UserId == id))
            {
                user = context.Users.Single(x => x.UserId == id);
            }
            return user;
        }

        public void EditUser(UserAppsModelView modifiedUser)
        {
            UserModel userToSave = modifiedUser.MVtoUserModel();

            UserModel user = context.Users.Single(x => x.UserId == userToSave.UserId);

            user.FirstName = userToSave.FirstName;
            user.LastName = userToSave.LastName;
            user.Email = userToSave.Email;

            string nonHashedPassword = userToSave.Password;
            string hashedPassword = HashPassword(userToSave.Email, nonHashedPassword);
            user.Password = hashedPassword;

            if (userToSave.Role == null)
            {
                user.Role = "user";
            }
            user.UserStatus = 1;
            user.LastLoginDate = DateTime.Now.ToString();
            user.Applications = userToSave.Applications;
            context.SaveChanges();
        }

        public bool AddUser(UserAppsModelView newUser)
        {
            bool result = false;

            // convert newUser to DatabaseEntityModel.UserModel
            UserModel userToSave = newUser.MVtoUserModel();

            // check if user Id already exists?
            if (!context.Users.Any(x => x.UserId == userToSave.UserId))
            {
                userToSave.Role = "user";
                userToSave.UserStatus = 1;
                userToSave.LastLoginDate = DateTime.Now.ToString();
                userToSave.Applications = new List<ApplicationModel>();
                userToSave.Applications.Add(context.Applications.First(x => x.ApplicationName == "Note Pad"));

                // Handling the Hashing of Password
                //string userName = userToSave.FirstName;
                string email = userToSave.Email;
                string nonHashedPassword = userToSave.Password;
                try
                {
                    //string hashedPassword = HashPassword(userName, nonHashedPassword);
                    string hashedPassword = HashPassword(email, nonHashedPassword);
                    List<UserModel> savedUserInfo = GetAllUsers();

                    // if there are no existing users already saved.. save it
                    if (!savedUserInfo.Any(x => x.Password == hashedPassword && x.Email == email))
                    {
                        userToSave.Password = hashedPassword;
                        context.Users.Add(userToSave);
                        context.SaveChanges();
                        result = true;
                    }
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Hashes the password for the user, and returns the hashed password
        /// </summary>
        private static string HashPassword(string userName, string nonHashedPassword)
        {
            if (string.IsNullOrEmpty(userName)) { throw new ArgumentNullException("user.UserName"); }
            if (string.IsNullOrEmpty(nonHashedPassword)) { throw new ArgumentNullException("user.Password"); }

            string result = string.Empty;

            // we are using the SHA512 algorithm. 
            HashAlgorithm hash = new SHA512CryptoServiceProvider();

            // we break the salt into 3 parts, and then combine it w/ 
            // the username as well as the password to make hashtables much 
            // more difficult to use
            int thirdOfALength = SALT.Length / 3;

            string stringToHash = SALT.Substring(0, thirdOfALength)
                + userName + SALT.Substring(thirdOfALength - 1, thirdOfALength)
                + nonHashedPassword + SALT.Substring(thirdOfALength * 2 - 1);

            // do the hashing (convert string to bytes, do the hashing, 
            // convert hashed bytes back to string
            byte[] bytesToHash = Encoding.UTF8.GetBytes(stringToHash);
            byte[] hashedBytes = hash.ComputeHash(bytesToHash);
            result = Convert.ToBase64String(hashedBytes);

            return result;
        }

        // use in the views
        public bool DeleteUser(UserModel userToDelete)
        {
            // the using statement will make sure the object is disposed when it goes out of scope
            bool result = false;

            if (!context.Users.Any(x => x.UserId == userToDelete.UserId))
            {
                context.Users.Remove(userToDelete);
                context.SaveChanges();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Method authenticates the user.. returns true if the user's 
        /// credentials are correct, and false if they are not
        /// </summary>
        //public bool AuthenticateUser(string userName, string nonHashedPassword)
        public bool AuthenticateUser(string email, string nonHashedPassword)
        {
            bool result = false;

            try
            {
                // Reset Admin Password Here:
                string hashedPassword = HashPassword(email, nonHashedPassword);

                List<UserModel> savedUserInfo = GetAllUsers(); // Here, it will be loaded with the complete DB

                // we have to match exactly 1 user item
                result = savedUserInfo.Count(x => x.Password == hashedPassword && x.Email == email) == 1;

                UserModel data = savedUserInfo.Single(x => x.Email == email);
                data.LastLoginDate = DateTime.Now.ToString();
                context.SaveChanges();
            }
            catch
            {
                /* need to log */
                result = false;
            }
            return result;
        }

        public int getUserId(string email)
        {
            int userId = 0;

            List<UserModel> savedUserInfo = GetAllUsers(); // Here, it will be loaded with the complete DB

            // we have to match exactly 1 user item
            UserModel data = savedUserInfo.Single(x => x.Email == email);
            userId = data.UserId;

            return userId;
        }

        public bool AssignApplicationToUser(UserAppsModelView addApp)
        {
            bool status = false;
            try
            {
                UserModel appToAdd = addApp.MVtoUserModel();
                UserModel user = context.Users.Single(x => x.UserId == appToAdd.UserId);

                //if(context.Users.Count(x=>x.UserId == appToAdd.UserId) == 1)
                //{
                //    UserModel user = context.Users.Single(x => x.UserId == appToAdd.UserId);
                //user.Applications.Add(appToAdd.Applications);
                //ApplicationModel app = appToAdd.Applications.Last();
                //AppErrorsModelView mv = new AppErrorsModelView(app);
                context.Applications.Add(appToAdd.Applications.LastOrDefault());

                //context.Applications.Add();
                context.SaveChanges();
                    status = true;
                //}

                //user.Applications.Add(appToAdd.Applications);
                //user.Applications.Add(appToAdd.Applications.First());
                //context.Applications = (DatabaseEntityModel.DatabaseModelFromDb)appToAdd.Applications;
                //context.Applications.Add(appToAdd.Applications.First());
                //context.Users.Add(user.Applications.ToList()<ApplicationModel>);
            }
            catch(Exception ex)
            {
                status = false;
            }
            return status;
        }

        public bool DisableUser(UserAppsModelView userToDisable)
        {
            bool status = false;

            UserModel disableUser = userToDisable.MVtoUserModel();
            UserModel user = context.Users.Single(x => x.UserId == disableUser.UserId);

            //user.FirstName = disableUser.FirstName;
            //user.LastName = disableUser.LastName;
            //user.Email = disableUser.Email;
            //user.Password = disableUser.Password;
            
            // disable user
            user.UserStatus = 0;
            //userToSave.Role = disableUser.Role;
            //user.LastLoginDate = disableUser.LastLoginDate;
            //user.Applications = disableUser.Applications;

            context.SaveChanges();

            return status;
        }
    }
}
