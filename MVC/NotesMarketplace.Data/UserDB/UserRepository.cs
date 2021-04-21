using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NotesMarketplace.Models.RegisteredUserModels;
using NotesMarketplace.Models.AuthenticationModels;
using System.Data.Entity;
using System.Text;
using System.IO;

namespace NotesMarketplace.Data.UserDB
{
    public class UserRepository
    {
        public static int AuthenticateUser(Login Creadentials)
        {
            using(var context = new NotesMarketPlaceEntities())
            {
                //check if user exists and credentials match
                User u = context.Users.FirstOrDefault(m => m.Email == Creadentials.Email && m.Passwd == Creadentials.Password && m.IsActive);

                //if user not found or credentials are wrong or user is inactive return 0
                if (u == null)
                    return 0;

                //else return UserID to store it in AuthCookie 
                else
                    return u.UserID;
            }
        }

        public static int AddUser(SignUp NewUser)
        {
            using (var context = new NotesMarketPlaceEntities())
            { 
                if (context.Users.Any(m => m.Email == NewUser.Email && m.IsActive))
                {
                    return 0;
                }

                User u = new User()
                {
                    FirstName = NewUser.FirstName,
                    LastName = NewUser.LastName,
                    Email = NewUser.Email,
                    IsEmailVerified = false,
                    RoleID = (int)NewUser.RoleID,
                    ModifiedDate = DateTime.Now,
                    Passwd = NewUser.Password,
                    IsActive = true,
                    GUID = NewUser.GUID,
                    CreatedBy = 1,
                    ModifiedBy = 1,
                    CreatedDate = System.DateTime.Now,                    
                };

                context.Users.Add(u);

                context.SaveChanges();

                return u.UserID;
            }
        }


        /* return value : meaning 
         * -1 : Failed
         * 1 : Successfully created or changed
         */
        public static UserProfileModel addUserProfile(UserProfileModel up, int UserID, string AppRoot)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                //if contry or gender info not match with DB return failed.
                Nullable<int> GenderID = null;
                bool IsValidCountryCode = false;
                int CountryID = 0;

                if (up.Gender != null)
                {
                    var Gender = context.ReferenceDatas.FirstOrDefault(r => r.RefCategory == "gender" && r.DataKey == up.Gender);
                    if (Gender != null)
                        GenderID = Gender.DataID;
                    else
                        GenderID = 0;
                }

                var Country = context.Countries.FirstOrDefault(c => c.CountryName == up.Country);

                CountryID = Country != null ? Country.CountryID : 0;

                if (up.CountryCode != null)
                {
                    IsValidCountryCode = context.Countries.Any(c => c.CountryCode == up.CountryCode);
                }
                else
                {
                    IsValidCountryCode = true;
                }

                if (!IsValidCountryCode || CountryID == 0 || GenderID == 0)
                    return null;

                UserProfile UpInDB = context.UserProfiles.FirstOrDefault(u => u.UserID == UserID);
                


                if (up.ProfilePictureFile != null)
                {
                    string UserDir = AppRoot + "\\Members\\" + UserID + "\\";
                    string FileExtenstion = Path.GetExtension(up.ProfilePictureFile.FileName).Replace('.','$');
                    Directory.CreateDirectory(UserDir);
                    up.ProfilePicture = @"/Assets/" + UserID + @"/UserProfile-" + UserID + FileExtenstion;

                    var files = Directory.GetFiles(UserDir, @"UserProfile-" + UserID + ".*");
                    if (files.Length > 0)
                    {
                        System.IO.File.Delete(files[0]);
                    }

                    up.ProfilePictureFile.SaveAs(UserDir + @"/UserProfile-" + UserID + Path.GetExtension(up.ProfilePictureFile.FileName));

                }

                //if user profilie does not exists create new one
                if (UpInDB == null)
                {
                    UserProfile NewUser = new UserProfile()
                    {
                        UserID = UserID,
                        DOB = up.DOB,
                        Gender = GenderID,
                        CountryCode = up.CountryCode,
                        PhoneNo = up.PhoneNo,
                        ProfilePicture = up.ProfilePicture,
                        AddressLine1 = up.AddressLine1,
                        AddressLine2 = up.AddressLine2,
                        City = up.AddressLine2,
                        State = up.State,
                        ZipCode = up.ZipCode,
                        Country = CountryID,
                        University = up.University,
                        College = up.College,
                        CreatedBy = UserID,
                        CreatedDate = System.DateTime.Now,
                        ModifiedBy = UserID,
                        ModifiedDate = System.DateTime.Now
                    };

                    context.Users.FirstOrDefault(u => u.UserID == UserID).RoleID = 3;
                    context.UserProfiles.Add(NewUser);
                    UpInDB = NewUser;
                }

                //if user profile exists enter new data
                else
                {
                    UpInDB.DOB = up.DOB;
                    UpInDB.Gender = GenderID;
                    UpInDB.CountryCode = up.CountryCode;
                    UpInDB.PhoneNo = up.PhoneNo;
                    UpInDB.ProfilePicture = up.ProfilePictureFile == null ? UpInDB.ProfilePicture : up.ProfilePicture;
                    UpInDB.AddressLine1 = up.AddressLine1;
                    UpInDB.AddressLine2 = up.AddressLine2;
                    UpInDB.City = up.AddressLine2;
                    UpInDB.State = up.State;
                    UpInDB.ZipCode = up.ZipCode;
                    UpInDB.Country = CountryID;
                    UpInDB.University = up.University;
                    UpInDB.College = up.College;
                    UpInDB.ModifiedBy = UserID;
                    UpInDB.ModifiedDate = System.DateTime.Now;
                }

                up.ProfilePicture = UpInDB.ProfilePicture;
                context.SaveChanges();
                return up;
            }
        }

        public static UserProfileModel GetUserData(int UID)
        {
            using(NotesMarketPlaceEntities context = new NotesMarketPlaceEntities()) {

                User user = context.Users.FirstOrDefault(u => u.UserID == UID);

                if(user == null)
                {
                    return null;
                }

                //get user sign data into model
                UserModel userModel = new UserModel()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleID = user.RoleID,
                    IsEmailVerified = user.IsEmailVerified
                };


                //If not found profile data then send user signup data only and redirect user on user profile page to enter data
                if (!context.UserProfiles.Any(u => u.UserID == UID))
                    return new UserProfileModel() { User = userModel} ;

                //if found user profile realted to this user return all data with signup data in user variable
                UserProfile up = context.UserProfiles.FirstOrDefault(u => u.UserID == UID);
                UserProfileModel upModel = new UserProfileModel() {
                    UserID = up.UserID,
                    DOB = up.DOB,
                    Gender = up.Gender != null ? context.ReferenceDatas.FirstOrDefault(r => r.DataID == up.Gender).DataKey : null,
                    SecondaryEmailAddress = up.SecondaryEmailAddress,
                    CountryCode = up.CountryCode,
                    PhoneNo = up.PhoneNo,
                    ProfilePicture = up.ProfilePicture,
                    AddressLine1 = up.AddressLine1,
                    AddressLine2 = up.AddressLine2,
                    City = up.City,
                    State = up.State,
                    ZipCode = up.ZipCode,
                    Country = up.Country1.CountryName,
                    University = up.University,
                    College = up.College,
                    User = userModel
                };
                return upModel;
            }
        }

        public static int VerifyEmail(int UID, string strGuid)
        {
            using ( NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                User u = context.Users.FirstOrDefault(us => us.UserID == UID && us.GUID == strGuid && us.IsActive);

                // return 1 if successful verification
                if (u != null)
                {
                    //If already Verified send -1
                    if (u.IsEmailVerified)
                        return -1;
                    u.GUID = null;
                    u.IsEmailVerified = true;
                    u.ModifiedDate = System.DateTime.Now;
                    u.ModifiedBy = u.UserID;
                    context.SaveChanges();
                    return 1;
                }

                //return 0 if uid and guid does not match
                else
                    return 0;
            }
        }
        public static string GetRole(int UID)
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                return context.Users.FirstOrDefault(u => u.UserID == UID).UserRole.RoleName;
            }
        }

        public static string ForgotPassword(string emailAddress)
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                User u = context.Users.FirstOrDefault(user => user.Email == emailAddress && user.IsActive);

                if (u == null)
                    return string.Empty;

                string elements = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%&*+=^";

                Random PassGenerator = new Random();

                StringBuilder NewPass = new StringBuilder();
                for (int i = 0; i < 10; i++)
                    NewPass.Append(elements[PassGenerator.Next(0,elements.Length)]);

                string pass = NewPass.ToString();
                u.Passwd = pass;
                u.ModifiedDate = System.DateTime.Now;
                u.ModifiedBy = 1;
                context.SaveChanges();
                return pass;
            }
        }

        public static int ChangePassword(ChangePasswordModel cp,int UID)
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                User u = context.Users.FirstOrDefault(user => user.Passwd == cp.Password && user.IsActive);
                if (u == null)
                    return 0;

                u.Passwd = cp.NewPassword;
                u.ModifiedDate = System.DateTime.Now;
                u.ModifiedBy = u.UserID;
                context.SaveChanges();
                return 1;
            }
        }

    }

    public class AdminUserRepository
    {
        /// <summary>
        /// Get Counts of new users of last <code>BeforeDays</code> Days.
        /// </summary>
        /// <param name="BeforeDays">No of past days to be considered</param>
        /// <returns>Int count of new users</returns>
        public static int CountNewUsers(int BeforeDays = 7)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                DateTime Criteria = System.DateTime.Now.AddDays(BeforeDays*-1);
                return context.Users.Where(u => u.CreatedDate >= Criteria).Count();
            }
        }
    }
}