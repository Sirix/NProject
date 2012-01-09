using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;

namespace NProject.BLL
{
    public class UserService : BaseService
    {
        public string GetDefaultLocationForRole(UserRole role)
        {
            switch (role)
            {
                case UserRole.Unspecified:
                    break;
                case UserRole.Programmer:
                    break;
                case UserRole.Manager:
                    return "projects/list";
                    break;
                case UserRole.TopManager:
                    break;
                case UserRole.Customer:
                    break;
                case UserRole.Tester:
                    break;
                case UserRole.Admin:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("role");
            }
            return null;
        }

        /// <summary>
        /// Retrieves the user instance from the database by passed id.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User instance, if exists; otherwise, null</returns>
        public User GetUser(int userId)
        {
            return Database.Users.FirstOrDefault(u => u.Id == userId);
        }
        public User GetUser(string username, string password)
        {
            string hash = MD5.EncryptMD5(password);
            return Database.Users.FirstOrDefault(u => u.Username == username && u.Hash == hash);
        }
    }
}



