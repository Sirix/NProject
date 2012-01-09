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
    public interface IUserService
    {
        string GetDefaultLocationByRole(UserRole role);
        User GetUserById(int id);
    }

    public class UserService : BaseService, IUserService
    {
        public string GetDefaultLocationByRole(UserRole role)
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
        public User GetUserById(int id)
        {
            return Database.Users.FirstOrDefault(u => u.Id == id);
        }
        public User GetUserByCredentials(string username, string password)
        {
            string hash = MD5.EncryptMD5(password);
            return Database.Users.FirstOrDefault(u => u.Username == username && u.Hash == hash);
        }
    }
}



